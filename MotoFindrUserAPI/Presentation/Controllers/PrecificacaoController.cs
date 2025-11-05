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
        private readonly IPrecificacaoMotoApplicationService _service;

        public PrecificacaoController(IPrecificacaoMotoApplicationService service)
        {
            _mlContext = new MLContext();
            _service = service;
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

        [HttpPost("definir-preco")]
        [EnableRateLimiting("rateLimitPolicy")]
        [SwaggerOperation(
            Summary = ApiDoc.DefinirPrecoSummary,
            Description = ApiDoc.DefinirPrecoDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Preço definido com sucesso", typeof(PrecificacaoMotoDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Moto não encontrada ou dados inválidos", typeof(MessageResponseDto))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DefinirPrecoResponseSample))]
        public async Task<IActionResult> DefinirPreco(int motoId, double preco)
        {
            var result = await _service.DefinirPrecoMotoAsync(motoId, preco);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
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
        public IActionResult TreinarModelo()
        {
            try
            {
                var dadosBrutos = _service.ObterDadosTreinamento();

                if (!dadosBrutos.Any())
                    return BadRequest("Cadastre preços para algumas motos antes de treinar.");

                var trainingData = dadosBrutos.Select(d => new MotoTrainingData
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

                return Ok("Modelo treinado.");
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
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Modelo ainda não foi treinado", typeof(MessageResponseDto))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PreverPrecoResponseSample))]
        public IActionResult Prever(string modelo, int ano)
        {
            if (!System.IO.File.Exists(_caminhoModelo))
                return BadRequest("Treine o modelo primeiro.");

            ITransformer loadedModel;
            using (var stream = new FileStream(_caminhoModelo, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = _mlContext.Model.Load(stream, out var schema);
            }

            var engine = _mlContext.Model.CreatePredictionEngine<MotoTrainingData, MotoPricePrediction>(loadedModel);
            var prediction = engine.Predict(new MotoTrainingData { Modelo = modelo, Ano = (float)ano });


            var precificacao = new PrecificacaoTreinamentoDto
            {
                Modelo = modelo,
                AnoDeFabricacao = ano,
                Preco = prediction.PrecoEstimado
            };

            return Ok(precificacao);
        }
    }
}
