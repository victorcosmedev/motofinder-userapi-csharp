using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using MotoFindrUserAPI.Application.Interfaces;

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
        public async Task<IActionResult> DefinirPreco(int motoId, double preco)
        {
            var result = await _service.DefinirPrecoMotoAsync(motoId, preco);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }


        [HttpPost("treinar-modelo")]
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

            return Ok(new { Modelo = modelo, Ano = ano, PrecoEstimado = prediction.PrecoEstimado.ToString("C") });
        }
    }
}
