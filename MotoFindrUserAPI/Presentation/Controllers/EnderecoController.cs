using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Models.Hateoas;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.Endereco;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [Authorize]
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
        [SwaggerResponse(StatusCodes.Status200OK, "Endereço encontrado com sucesso", typeof(EnderecoDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Endereço não encontrado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [SwaggerResponseExample(statusCode: 200, typeof(EnderecoResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<EnderecoDto>> GetById(int id)
        {
            try
            {
                var result = await _enderecoService.ObterPorIdAsync(id);
                if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

                var hateoas = new HateoasResponse<EnderecoDto>
                {
                    Data = result.Value,
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
        [SwaggerRequestExample(typeof(EnderecoDto), typeof(EnderecoRequestSample))]
        [SwaggerOperation(
            Summary = ApiDoc.SalvarEnderecoSummary,
            Description = ApiDoc.SalvarEnderecoDescription
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Endereço criado com sucesso", typeof(EnderecoDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do endereço inválidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<EnderecoDto>> Post([FromBody] EnderecoDto endereco)
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
                var result = await _enderecoService.CriarAsync(endereco);
                if(result.IsSuccess == false) return StatusCode(result.StatusCode, result.Error);

                var hateoas = new HateoasResponse<EnderecoDto>
                {
                    Data = result.Value,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id = result.Value?.Id }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id = result.Value?.Id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id = result.Value?.Id }), Method = "DELETE" }
                    }
                };

                return CreatedAtAction(nameof(GetById), new { id = result.Value?.Id }, hateoas);
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
        public async Task<IActionResult> Put(int id, [FromBody] EnderecoDto endereco)
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
                var result = await _enderecoService.AtualizarAsync(id, endereco);
                if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

                var hateoas = new HateoasResponse<EnderecoDto>
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
                var result = await _enderecoService.DeletarAsync(id);
                if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

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
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de endereços obtida com sucesso", typeof(IEnumerable<EnderecoDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhum endereço encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        [SwaggerResponseExample(statusCode: 200, typeof(EnderecoResponseListSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> BuscarTodos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _enderecoService.ObterTodos(pageNumber, pageSize);
                if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

                if (result.Value?.Items == null || (!result.Value?.Items.Any() ?? false))
                    return NotFound("Não há itens.");

                var pageResults = BuildPageResultsForBuscarTodos(result.Value);

                var response = new HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<EnderecoDto>>>>
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Helpers

        private PageResultModel<IEnumerable<HateoasResponse<EnderecoDto>>> BuildPageResultsForBuscarTodos(PageResultModel<IEnumerable<EnderecoDto>> pageResult)
        {
            var pageResults = new PageResultModel<IEnumerable<HateoasResponse<EnderecoDto>>>
            {
                Items = pageResult.Items.Select(endereco => new HateoasResponse<EnderecoDto>
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
