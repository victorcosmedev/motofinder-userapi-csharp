using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.Precificacao;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class PrecificacaoController : ControllerBase
    {
        private readonly MLContext _mlContext;
        private readonly string _caminhoModelo = 
            Path.Combine(Environment.CurrentDirectory, "Treinamento", "ModeloPrecificacao.zip");
        private readonly IMotoApplicationService _motoService;

        public PrecificacaoController(IMotoApplicationService service)
        {
            _mlContext = new MLContext();
            _motoService = service;
        }

        public class MotoTrainingData
        {
            [LoadColumn(0)] public string Modelo { get; set; } = string.Empty;
            [LoadColumn(1)] public float Ano { get; set; }
            [LoadColumn(2)] public float PrecoReal { get; set; }
        }

        public class MotoPricePrediction
        {
            [ColumnName("Score")]
            public float PrecoEstimado { get; set; }
        }


        [HttpPost("treinar-modelo")]
        [EnableRateLimiting("rateLimitPolicy")]
        [SwaggerOperation(
            Summary = ApiDoc.TreinarModeloSummary,
            Description = ApiDoc.TreinarModeloDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Modelo treinado com sucesso", typeof(MessageResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Não há dados suficientes para treinar", typeof(MessageResponseDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno durante o treinamento", typeof(MessageResponseDto))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TreinarModeloResponseSample))]
        public async Task<IActionResult> TreinarModelo()
        {
            try
            {
                var result = await _motoService.ObterDadosTreinamento();

                if(!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

                if (!result.Value.Any())
                    return BadRequest("Cadastre preços para algumas motos antes de treinar.");

                var trainingData = result.Value.Select(d => new MotoTrainingData
                {
                    Modelo = d.Modelo,
                    Ano = (float)d.AnoDeFabricacao,
                    PrecoReal = (float)d.Preco
                });

                IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(MotoTrainingData.PrecoReal))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ModeloEncoded", nameof(MotoTrainingData.Modelo)))
                    .Append(_mlContext.Transforms.Concatenate("Features", "ModeloEncoded", nameof(MotoTrainingData.Ano)))
                    .Append(_mlContext.Regression.Trainers.Sdca());

                var model = pipeline.Fit(dataView);
                _mlContext.Model.Save(model, dataView.Schema, _caminhoModelo);

                return Ok("Modelo treinado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("prever")]
        [AllowAnonymous]
        [EnableRateLimiting("rateLimitPolicy")]
        [SwaggerOperation(
            Summary = ApiDoc.PreverPrecoSummary,
            Description = ApiDoc.PreverPrecoDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Previsão calculada com sucesso", typeof(PrevisaoPrecoDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Treine o modelo primeiro.", typeof(MessageResponseDto))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PreverPrecoResponseSample))]
        public IActionResult Prever(string modelo, int ano)
        {
            if (!System.IO.File.Exists(_caminhoModelo))
                return BadRequest(new MessageResponseDto { Message = "Treine o modelo primeiro." });

            ITransformer loadedModel;
            using (var stream = new FileStream(_caminhoModelo, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = _mlContext.Model.Load(stream, out var schema);
            }

            var engine = _mlContext.Model.CreatePredictionEngine<MotoTrainingData, MotoPricePrediction>(loadedModel);
            var prediction = engine.Predict(new MotoTrainingData { Modelo = modelo, Ano = (float)ano });


            var resultado = new PrecificacaoTreinamentoDto
            {
                Modelo = modelo,
                AnoDeFabricacao = ano,
                Preco = prediction.PrecoEstimado
            };

            return Ok(resultado);
        }
    }
}
