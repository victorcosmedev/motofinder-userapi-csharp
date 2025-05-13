using Microsoft.AspNetCore.Mvc;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;

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
        public async Task<ActionResult<MotoEntity>> GetById(int id)
        {
            var moto = await _motoService.ObterPorIdAsync(id);
            if (moto == null) return NotFound();

            return Ok(moto);
        }

        [HttpGet("placa/{placa}")]
        public async Task<ActionResult<MotoEntity>> GetByPlaca(string placa)
        {
            var moto = await _motoService.ObterPorPlacaAsync(placa);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpGet("chassi/{chassi}")]
        public async Task<ActionResult<MotoEntity>> GetByChassi(string chassi)
        {
            var moto = await _motoService.ObterPorChassiAsync(chassi);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<MotoEntity>> Post([FromBody] MotoEntity moto)
        {
            var novaMoto = await _motoService.CriarAsync(moto);
            return CreatedAtAction(nameof(GetById), new { id = novaMoto.Id }, novaMoto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MotoEntity moto)
        {
            var atualizado = await _motoService.AtualizarAsync(id, moto);
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _motoService.RemoverAsync(id);
            return removido ? NoContent() : NotFound();
        }
    }
}
