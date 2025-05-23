using Microsoft.AspNetCore.Mvc;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoApplicationService _motoService;

        public MotoController(IMotoApplicationService motoService)
        {
            _motoService = motoService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = ApiDoc.GetMotoByIdSummary,
            Description = ApiDoc.GetMotoByIdDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso", typeof(MotoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<MotoDTO>> GetById(int id)
        {
            try
            {
                var moto = await _motoService.ObterPorIdAsync(id);
                if (moto == null) return NotFound();
                return Ok(moto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("placa/{placa}")]
        [SwaggerOperation(
            Summary = ApiDoc.GetByPlacaSummary,
            Description = ApiDoc.GetByPlacaDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso", typeof(MotoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Placa inválida")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<MotoDTO>> GetByPlaca(string placa)
        {
            try
            {
                var moto = await _motoService.ObterPorPlacaAsync(placa);
                if (moto == null) return NotFound();
                return Ok(moto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("chassi/{chassi}")]
        [SwaggerOperation(
            Summary = ApiDoc.GetMotoByChassiSummary,
            Description = ApiDoc.GetMotoByChassiDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso", typeof(MotoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Chassi inválido")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<MotoDTO>> GetByChassi(string chassi)
        {
            try
            {
                var moto = await _motoService.ObterPorChassiAsync(chassi);
                if (moto == null) return NotFound();
                return Ok(moto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = ApiDoc.SalvarMotoSummary,
            Description = ApiDoc.SalvarMotoDescription
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Moto criada com sucesso", typeof(MotoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados da moto inválidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<MotoDTO>> Post([FromBody] MotoDTO moto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var novaMoto = await _motoService.CriarAsync(moto);
                return CreatedAtAction(nameof(GetById), new { id = novaMoto.Id }, novaMoto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = ApiDoc.AtualizarMotoSummary,
            Description = ApiDoc.AtualizarMotoDescription
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Moto atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "IDs inconsistentes ou dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<IActionResult> Put(int id, [FromBody] MotoDTO moto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var atualizado = await _motoService.AtualizarAsync(id, moto);
                return atualizado ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = ApiDoc.DeletarMotoSummary,
            Description = ApiDoc.DeletarMotoDescription
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Moto removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var removido = await _motoService.RemoverAsync(id);
                return removido ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = ApiDoc.BuscarTodasMotosSummary,
            Description = ApiDoc.BuscarTodasMotosDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motos obtida com sucesso", typeof(IEnumerable<MotoDTO>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Nenhuma moto encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var motos = await _motoService.ObterTodos();
                if (motos != null && motos.Any())
                    return Ok(motos);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
