using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Models.Hateoas;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.Moto;
using MotoFindrUserAPI.Utils.Samples.Motoqueiro;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [Authorize]
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
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso", typeof(MotoDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [SwaggerResponseExample(statusCode:200, typeof(MotoResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<MotoDto>> GetById(int id)
        {
            try
            {
                var moto = await _motoService.ObterPorIdAsync(id);
                if (moto == null) 
                    return NotFound("Moto não encontrada");

                var hateoas = new HateoasResponse<MotoDto>
                {
                    Data = moto,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id }), Method = "GET" },
                        new LinkDto { Rel = "GetByPlaca", Href = Url.Action(nameof(GetByPlaca), new { placa = moto.Placa }), Method = "GET" },
                        new LinkDto { Rel = "GetByChassi", Href = Url.Action(nameof(GetByChassi), new { chassi = moto.Chassi }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id }), Method = "DELETE" },
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" }
                    }
                };

                return Ok(hateoas);
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
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso", typeof(MotoDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Placa inválida")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<MotoDto>> GetByPlaca(string placa)
        {
            try
            {
                var moto = await _motoService.ObterPorPlacaAsync(placa);
                if (moto == null) return NotFound("Moto não encontrada");

                var hateoas = new HateoasResponse<MotoDto>
                {
                    Data = moto,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetByPlaca), new { placa }), Method = "GET" },
                        new LinkDto { Rel = "GetByChassi", Href = Url.Action(nameof(GetByChassi), new { chassi = moto.Chassi }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id = moto.Id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id = moto.Id }), Method = "DELETE" },
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" }
                    }
                };
                return Ok(hateoas);
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
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada com sucesso", typeof(MotoDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Chassi inválido")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<MotoDto>> GetByChassi(string chassi)
        {
            try
            {
                var moto = await _motoService.ObterPorChassiAsync(chassi);
                if (moto == null) return NotFound("Moto não encontrada");

                var hateoas = new HateoasResponse<MotoDto>
                {
                    Data = moto,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetByChassi), new { chassi }), Method = "GET" },
                        new LinkDto { Rel = "GetByPlaca", Href = Url.Action(nameof(GetByPlaca), new { placa = moto.Placa }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id = moto.Id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id = moto.Id }), Method = "DELETE" },
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" }
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
        [SwaggerRequestExample(typeof(MotoDto), typeof(MotoRequestSample))]
        [SwaggerOperation(
            Summary = ApiDoc.SalvarMotoSummary,
            Description = ApiDoc.SalvarMotoDescription
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Moto criada com sucesso", typeof(MotoDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados da moto inválidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoqueiroResponseSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<ActionResult<MotoDto>> Post([FromBody] MotoDto moto)
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

                var hateoas = new HateoasResponse<MotoDto>
                {
                    Data = novaMoto,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id = novaMoto.Id }), Method = "GET" },
                        new LinkDto { Rel = "GetByPlaca", Href = Url.Action(nameof(GetByPlaca), new { placa = novaMoto.Placa }), Method = "GET" },
                        new LinkDto { Rel = "GetByChassi", Href = Url.Action(nameof(GetByChassi), new { chassi = novaMoto.Chassi }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id = novaMoto.Id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id = novaMoto.Id }), Method = "DELETE" },
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" }
                    }
                };

                return CreatedAtAction(nameof(GetById), new { id = novaMoto.Id }, hateoas);
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
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Put(int id, [FromBody] MotoDto moto)
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
                if(!atualizado)
                    return BadRequest("IDs inconsistentes ou dados inválidos");

                var hateoas = new HateoasResponse<MotoDto>
                {
                    Data = moto,
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "self", Href = Url.Action(nameof(GetById), new { id }), Method = "GET" },
                        new LinkDto { Rel = "GetByPlaca", Href = Url.Action(nameof(GetByPlaca), new { placa = moto.Placa }), Method = "GET" },
                        new LinkDto { Rel = "GetByChassi", Href = Url.Action(nameof(GetByChassi), new { chassi = moto.Chassi }), Method = "GET" },
                        new LinkDto { Rel = "update", Href = Url.Action(nameof(Put), new { id }), Method = "PUT" },
                        new LinkDto { Rel = "delete", Href = Url.Action(nameof(Delete), new { id }), Method = "DELETE" },
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" }
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
            Summary = ApiDoc.DeletarMotoSummary,
            Description = ApiDoc.DeletarMotoDescription
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Moto removida com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var removido = await _motoService.RemoverAsync(id);
                if(!removido)
                    return BadRequest("Moto não encontrada");

                var hateoas = new HateoasResponse<string>
                {
                    Data = "Moto removida com sucesso",
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Rel = "all", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" },
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
            Summary = ApiDoc.BuscarTodasMotosSummary,
            Description = ApiDoc.BuscarTodasMotosDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motos obtida com sucesso", typeof(IEnumerable<MotoDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhuma moto encontrada")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoqueiroResponseListSample))]
        [EnableRateLimiting("rateLimitPolicy")]
        public async Task<IActionResult> BuscarTodos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _motoService.ObterTodos(pageNumber, pageSize);
                if(pageResult.Items == null || !pageResult.Items.Any())
                    return NotFound("Moto não encontrada");

                var pageResults = BuildPageResultsForBuscarTodos(pageResult);

                var response = new HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<MotoDto>>>>
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Helpers
        private PageResultModel<IEnumerable<HateoasResponse<MotoDto>>> BuildPageResultsForBuscarTodos(PageResultModel<IEnumerable<MotoDto>> pageResult)
        {
            var pageResults = new PageResultModel<IEnumerable<HateoasResponse<MotoDto>>>
            {
                Items = pageResult.Items.Select(moto => new HateoasResponse<MotoDto>
                {
                    Data = moto,
                    Links = new List<LinkDto>
                    {
                        new LinkDto
                        {
                            Rel = "self",
                            Href = Url.Action(nameof(GetById), new { id = moto.Id }) ?? string.Empty,
                            Method = "GET"
                        },
                        new LinkDto { 
                            Rel = "GetByPlaca", 
                            Href = Url.Action(nameof(GetByPlaca),  new { placa = moto.Placa }), 
                            Method = "GET" 
                        },
                        new LinkDto { 
                            Rel = "GetByChassi", 
                            Href = Url.Action(nameof(GetByChassi), new { chassi = moto.Chassi }), 
                            Method = "GET" 
                        },
                        new LinkDto
                        {
                            Rel = "update",
                            Href = Url.Action(nameof(Put), new { id = moto.Id }) ?? string.Empty,
                            Method = "PUT"
                        },
                        new LinkDto
                        {
                            Rel = "delete",
                            Href = Url.Action(nameof(Delete), new { id = moto.Id }) ?? string.Empty,
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
