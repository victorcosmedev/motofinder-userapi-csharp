using Microsoft.AspNetCore.Mvc;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
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
            Summary = "Obter moto por ID",
            Description = "Retorna uma moto específica com base no ID fornecido"
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
            Summary = "Obter moto por placa",
            Description = "Retorna uma moto específica com base na placa fornecida"
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
            Summary = "Obter moto por chassi",
            Description = "Retorna uma moto específica com base no chassi fornecido"
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
            Summary = "Criar nova moto",
            Description = "Cadastra uma nova moto no sistema"
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Moto criada com sucesso", typeof(MotoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados da moto inválidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<MotoDTO>> Post([FromBody] MotoDTO moto)
        {
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
            Summary = "Atualizar moto existente",
            Description = "Atualiza os dados de uma moto existente"
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Moto atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "IDs inconsistentes ou dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<IActionResult> Put(int id, [FromBody] MotoDTO moto)
        {
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
            Summary = "Remover moto",
            Description = "Exclui permanentemente uma moto do sistema"
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
            Summary = "Obter todas as motos cadastradas",
            Description = "Buscar todas as motos salvas no sistema."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motos obtida com sucesso", typeof(IEnumerable<MotoDTO>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Nenhuma moto encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var motos = await _motoService.ObterTodos();
                if (motos != null)
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
