using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Models.Hateoas;
using MotoFindrUserAPI.Utils.Doc;
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var motoqueiro = await _service.ObterPorIdAsync(id);
                if(motoqueiro == null)
                    return NotFound();

                var hateoas = new HateoasResponse<MotoqueiroDTO>
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
        [SwaggerResponse(StatusCodes.Status200OK, "Motoqueiro encontrado com sucesso", typeof(MotoqueiroDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Motoqueiro não encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "CPF inválido ou não informado")]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                    return BadRequest("CPF não informado");

                var motoqueiro = await _service.ObterPorCpfAsync(cpf);

                if(motoqueiro == null)
                    return NotFound();

                var hateoas = new HateoasResponse<MotoqueiroDTO>
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
        [SwaggerOperation(
            Summary = ApiDoc.SalvarMotoqueiroSummary,
            Description = ApiDoc.SalvarMotoqueiroDescription
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Motoqueiro criado com sucesso", typeof(MotoqueiroDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados do motoqueiro inválidos")]
        [EnableRateLimiting("rateLimitPolicy")]
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

                var hateoas = new HateoasResponse<MotoqueiroDTO>
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

                if(!sucesso)
                    return NotFound();

                var hateoas = new HateoasResponse<MotoqueiroDTO>
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                var sucesso = await _service.RemoverAsync(id);
                if(!sucesso)
                    return NotFound();

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
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motoqueiros obtida com sucesso", typeof(IEnumerable<MotoqueiroDTO>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Nenhum motoqueiro encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> BuscarTodos(int pageNumber, int pageSize)
        {
            try
            {
                var motoqueiros = await _service.ObterTodos(pageNumber, pageSize);
                if (motoqueiros == null && !motoqueiros.Any())
                    return NoContent();

                var response = motoqueiros.Select(m => new HateoasResponse<MotoqueiroDTO>
                {
                    Data = m,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(ObterPorId), new { id = m.Id }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Atualizar), new { id = m.Id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Remover), new { id = m.Id }), Method = "DELETE" }
                    }
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
