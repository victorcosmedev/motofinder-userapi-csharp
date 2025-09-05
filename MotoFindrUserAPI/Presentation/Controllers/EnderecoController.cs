using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Utils.Hateoas;
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<EnderecoDTO>> GetById(int id)
        {
            try
            {
                var endereco = await _enderecoService.ObterPorIdAsync(id);
                if (endereco == null) 
                    return NotFound();

                var hateoas = new HateoasResponse<EnderecoDTO>
                {
                    Data = endereco,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id }), Method = "DELETE" }
                    }
                };

                return Ok(hateoas);
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
        [EnableRateLimiting("rateLimitPolicy")]
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
                var hateoas = new HateoasResponse<EnderecoDTO>
                {
                    Data = novoEndereco,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id = novoEndereco.Id }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id = novoEndereco.Id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id = novoEndereco.Id }), Method = "DELETE" }
                    }
                };

                return CreatedAtAction(nameof(GetById), new { id = novoEndereco.Id }, hateoas);
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
        [EnableRateLimiting("rateLimitPolicy")]
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
                if (!atualizado)
                    return NotFound("Endereco não encontrado");

                var hateoas = new HateoasResponse<EnderecoDTO>
                {
                    Data = endereco,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id }), Method = "GET" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id }), Method = "DELETE" }
                    }
                };
                return Ok(hateoas);
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var removido = await _enderecoService.DeletarAsync(id);
                if (!removido)
                    return NotFound("Endereco não encontrado");
                var hateoas = new HateoasResponse<string>
                {
                    Message = "Endereco removido com sucesso",
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "create", Href = Url.Action(nameof(Post)), Method = "POST" }
                    }
                };  
                return Ok(hateoas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
