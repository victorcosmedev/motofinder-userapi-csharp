using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Hosting;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Models.Hateoas;
using MotoFindrUserAPI.Models.PageResultModel;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.Motoqueiro;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [Authorize]
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
        [SwaggerResponse(StatusCodes.Status200OK, "Motoqueiro encontrado com sucesso", typeof(MotoqueiroDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoqueiroResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var motoqueiro = await _service.ObterPorIdAsync(id);
                if(motoqueiro == null)
                    return NotFound("Nenhum motoqueiro encontrado");

                var hateoas = new HateoasResponse<MotoqueiroDto>
                {
                    Data = motoqueiro,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(ObterPorId), new { id = id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "GetByCpf",
                            Href = Url.Action(nameof(ObterPorCpf), new { cpf = motoqueiro.Cpf }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Atualizar), new { id = id }) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Remover), new { id = id }) ?? string.Empty,
                            Method = "DELETE"
                        },
                        new LinkDto
                        {
                            Rel = "todos",
                            Href = Url.Action(nameof(BuscarTodos)) ?? string.Empty,
                            Method = "GET"
                        }
                    }
                };
                return Ok(hateoas);
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
        [SwaggerResponse(StatusCodes.Status200OK, "Motoqueiro encontrado com sucesso", typeof(MotoqueiroDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "CPF inválido ou não informado")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoqueiroResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                    return BadRequest("CPF não informado");

                var motoqueiro = await _service.ObterPorCpfAsync(cpf);

                if(motoqueiro == null)
                    return NotFound("Nenhum motoqueiro encontrado");

                var hateoas = new HateoasResponse<MotoqueiroDto>
                {
                    Data = motoqueiro,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(ObterPorCpf), new { cpf = cpf }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "ObterPorId",
                            Href = Url.Action(nameof(ObterPorId), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Atualizar), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Remover), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "DELETE"
                        },
                        new LinkDto
                        {
                            Rel = "todos",
                            Href = Url.Action(nameof(BuscarTodos)) ?? string.Empty,
                            Method = "GET"
                        }
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
        [SwaggerRequestExample(typeof(MotoqueiroDto), typeof(MotoqueiroRequestSample))]
        [SwaggerOperation(
            Summary = ApiDoc.SalvarMotoqueiroSummary,
            Description = ApiDoc.SalvarMotoqueiroDescription
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Motoqueiro criado com sucesso", typeof(MotoqueiroDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do motoqueiro inválidos")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoqueiroResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Criar([FromBody] MotoqueiroDto motoqueiro)
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

                var hateoas = new HateoasResponse<MotoqueiroDto>
                {
                    Data = novoMotoqueiro,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(ObterPorId), new { id = novoMotoqueiro.Id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "GetByCpf",
                            Href = Url.Action(nameof(ObterPorCpf), new { id = novoMotoqueiro.Cpf }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Atualizar), new { id = novoMotoqueiro.Id }) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Remover), new { id = novoMotoqueiro.Id }) ?? string.Empty,
                            Method = "DELETE"
                        },
                        new LinkDto
                        {
                            Rel = "todos",
                            Href = Url.Action(nameof(BuscarTodos)) ?? string.Empty,
                            Method = "GET"
                        }
                    }
                };

                return CreatedAtAction(nameof(ObterPorId), new { id = novoMotoqueiro.Id }, hateoas);
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] MotoqueiroDto motoqueiro)
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

                if(!sucesso)
                    return NotFound("Motoqueiro não encontrado");

                var hateoas = new HateoasResponse<MotoqueiroDto>
                {
                    Data = motoqueiro,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(ObterPorId), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "ObterPorCpf",
                            Href = Url.Action(nameof(ObterPorCpf), new { cpf = motoqueiro.Cpf }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Atualizar), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Remover), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "DELETE"
                        },
                        new LinkDto
                        {
                            Rel = "todos",
                            Href = Url.Action(nameof(BuscarTodos)) ?? string.Empty,
                            Method = "GET"
                        }
                    }
                };

                return sucesso ? NoContent() : NotFound("Motoqueiro não encontrado");
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _service.RemoverAsync(id);
                if(!sucesso)
                    return NotFound("Motoqueiro não encontrado");

                var hateoas = new HateoasResponse<object>
                {
                    Data = null,
                    Message = "Motoqueiro removido com sucesso",
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" },
                        new LinkDto { Rel = "create", Href = Url.Action(nameof(Criar)), Method = "POST" }
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
            Summary = ApiDoc.BuscarTodosMotoqueirosSummary,
            Description = ApiDoc.BuscarTodosMotoqueirosDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motoqueiros obtida com sucesso", typeof(IEnumerable<MotoqueiroDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhum motoqueiro encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        [SwaggerResponseExample(statusCode:200, typeof(MotoqueiroResponseListSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> BuscarTodos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _service.ObterTodos(pageNumber, pageSize);
                if (pageResult.Items == null && !pageResult.Items.Any())
                    return NotFound("Nenhum motoqueiro encontrado");

                var pageResults = BuildPageResultsForBuscarTodos(pageResult);

                var response = new HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<MotoqueiroDto>>>>
                {
                    Data = pageResults,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(BuscarTodos), new { pageNumber, pageSize }), Method = "GET" },
                        new LinkDto { Rel = "create", Href = Url.Action(nameof(Criar)), Method = "POST" }
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Helpers
        private PageResultModel<IEnumerable<HateoasResponse<MotoqueiroDto>>> BuildPageResultsForBuscarTodos(PageResultModel<IEnumerable<MotoqueiroDto>> pageResult)
        {
            var pageResults = new PageResultModel<IEnumerable<HateoasResponse<MotoqueiroDto>>>
            {
                Items = pageResult.Items.Select(motoqueiro => new HateoasResponse<MotoqueiroDto>
                {
                    Data = motoqueiro,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(ObterPorId), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "ObterPorCpf",
                            Href = Url.Action(nameof(ObterPorCpf), new { cpf = motoqueiro.Cpf }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Atualizar), new { id = motoqueiro.Id }) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Remover), new { id = motoqueiro.Id }) ?? string.Empty,
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
