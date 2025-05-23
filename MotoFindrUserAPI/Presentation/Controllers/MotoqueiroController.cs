using Microsoft.AspNetCore.Mvc;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MotoqueiroController : ControllerBase
    {
        private readonly IMotoqueiroApplicationService _service;

        public MotoqueiroController(IMotoqueiroApplicationService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = ApiDoc.BuscarMotoqueiroPorIdSummary,
            Description = ApiDoc.BuscarMotoqueiroPorIdDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Motoqueiro encontrado com sucesso", typeof(MotoqueiroDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var motoqueiro = await _service.ObterPorIdAsync(id);
                return motoqueiro == null ? NotFound() : Ok(motoqueiro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cpf/{cpf}")]
        [SwaggerOperation(
            Summary = ApiDoc.BuscarMotoqueiroPorCpfSummary,
            Description = ApiDoc.BuscarMotoqueiroPorCpfDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Motoqueiro encontrado com sucesso", typeof(MotoqueiroDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "CPF inválido ou não informado")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                    return BadRequest("CPF não informado");

                var motoqueiro = await _service.ObterPorCpfAsync(cpf);
                return motoqueiro == null ? NotFound() : Ok(motoqueiro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = ApiDoc.SalvarMotoqueiroSummary,
            Description = ApiDoc.SalvarMotoqueiroDescription
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Motoqueiro criado com sucesso", typeof(MotoqueiroDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do motoqueiro inválidos")]
        public async Task<IActionResult> Criar([FromBody] MotoqueiroDTO motoqueiro)
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
                var novoMotoqueiro = await _service.CriarAsync(motoqueiro);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoMotoqueiro.Id }, novoMotoqueiro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = ApiDoc.AtualizarMotoqueiroSummary,
            Description = ApiDoc.AtualizarMotoqueiroDescription
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Motoqueiro atualizado com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "IDs inconsistentes ou dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] MotoqueiroDTO motoqueiro)
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
                if (id != motoqueiro.Id)
                    return BadRequest("ID da URL não corresponde ao ID do motoqueiro");

                var sucesso = await _service.AtualizarAsync(id, motoqueiro);
                return sucesso ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = ApiDoc.DeletarMotoqueiroSummary,
            Description = ApiDoc.DeletarMotoqueiroDescription
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Motoqueiro removido com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Erro ao remover motoqueiro")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _service.RemoverAsync(id);
                return sucesso ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = ApiDoc.BuscarTodosMotoqueirosSummary,
            Description = ApiDoc.BuscarTodosMotoqueirosDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motoqueiros obtida com sucesso", typeof(IEnumerable<MotoqueiroDTO>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Nenhum motoqueiro encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var motoqueiro = await _service.ObterTodos();
                if (motoqueiro != null && motoqueiro.Any())
                    return Ok(motoqueiro);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
