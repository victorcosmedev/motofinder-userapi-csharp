using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Models.Hateoas;
using MotoFindrUserAPI.Models.PageResultModel;
using MotoFindrUserAPI.Utils.Doc;
using Swashbuckle.AspNetCore.Annotations;
using System;

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
            Summary = ApiDoc.BuscarEnderecoPorIdSummary,
            Description = ApiDoc.BuscarEnderecoPorIdDescription
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
            Summary = ApiDoc.SalvarEnderecoSummary,
            Description = ApiDoc.SalvarEnderecoDescription
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
            Summary = ApiDoc.AtualizarEnderecoSummary,
            Description = ApiDoc.AtualizarEnderecoDescription
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
            Summary = ApiDoc.DeletarEnderecoSummary,
            Description = ApiDoc.DeletarEnderecoDescription
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

        [HttpGet]
        [SwaggerOperation(
            Summary = ApiDoc.BuscarTodosEnderecosSummary,
            Description = ApiDoc.BuscarTodosEnderecosDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de endereços obtida com sucesso", typeof(IEnumerable<EnderecoDTO>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Nenhum endereço encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> BuscarTodos(int pageNumber = 1, int pageSize = 10)
        {
            var pageResult = await _enderecoService.ObterTodos(pageNumber, pageSize);

            if (pageResult.Items == null || !pageResult.Items.Any())
                return NoContent();

            var pageResults = BuildPageResultsForBuscarTodos(pageResult);
            var response = new HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<EnderecoDTO>>>>
            {
                Data = pageResults,
                Links = new List<LinkDto>
                {
                    new LinkDto { Rel = "self", Href = Url.Action(nameof(BuscarTodos), new { pageNumber, pageSize }), Method = "GET" },
                    new LinkDto { Rel = "create", Href = Url.Action(nameof(Post)), Method = "POST" }
                }
            };

            return Ok(response);
        }

        #region Helpers

        private PageResultModel<IEnumerable<HateoasResponse<EnderecoDTO>>> BuildPageResultsForBuscarTodos(PageResultModel<IEnumerable<EnderecoDTO>> pageResult)
        {
            var pageResults = new PageResultModel<IEnumerable<HateoasResponse<EnderecoDTO>>>
            {
                Items = pageResult.Items.Select(endereco => new HateoasResponse<EnderecoDTO>
                {
                    Data = endereco,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(GetById), new { id = endereco.Id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Put), new { id = endereco.Id}) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Delete), new { id = endereco.Id }) ?? string.Empty,
                            Method = "DELETE"
                        }
                    }
                }),
                TotalItens = pageResult.TotalItens,
                NumeroPagina = pageResult.NumeroPagina,
                TamanhoPagina = pageResult.TamanhoPagina
            };

            return pageResults;
        }
        #endregion
    }
}
