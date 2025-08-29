using Microsoft.AspNetCore.Mvc;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoApplicationService _enderecoService;
        public EnderecoController(IEnderecoApplicationService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Buscar endereço por ID",
        Description = "Retorna um endereço específico com base no ID informado."
    )]
        [SwaggerResponse(StatusCodes.Status200OK, "Endereço encontrado com sucesso", typeof(EnderecoDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Endereço não encontrado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<EnderecoDTO>> GetById(int id)
        {
            try
            {
                var endereco = await _enderecoService.ObterPorIdAsync(id);
                if (endereco == null) return NotFound();
                return Ok(endereco);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar novo endereço",
            Description = "Salva um novo endereço no sistema."
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Endereço criado com sucesso", typeof(EnderecoDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do endereço inválidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<ActionResult<EnderecoDTO>> Post([FromBody] EnderecoDTO endereco)
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
                var novoEndereco = await _enderecoService.CriarAsync(endereco);
                return CreatedAtAction(nameof(GetById), new { id = novoEndereco.Id }, novoEndereco);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar endereço",
            Description = "Atualiza um endereço existente com base no ID informado."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Endereço atualizado com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "IDs inconsistentes ou dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Endereço não encontrado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<IActionResult> Put(int id, [FromBody] EnderecoDTO endereco)
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
                var atualizado = await _enderecoService.AtualizarAsync(id, endereco);
                return atualizado ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletar endereço",
            Description = "Remove um endereço existente com base no ID informado."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Endereço removido com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Endereço não encontrado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var removido = await _enderecoService.DeletarAsync(id);
                return removido ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
