using Cp2Mottu.Context;
using Cp2Mottu.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Controllers
{
    public class MotosController : ControllerBase
    {
        private readonly MotosContext _context;

        public MotosController(MotosContext context)
        {
            this._context = context;
        }


        [HttpGet]
        [Route("api/motos")]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return Ok(await _context.Motos.ToListAsync());
        }

        [HttpGet]
        [Route("api/motos/{id}")]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id); // Busca a moto pelo ID
            if (moto == null) // Se não encontrar, retorna 404 Not Found
            {
                return NotFound();
            }
            return Ok(moto); // Retorna a moto encontrada
        }



        [HttpPost]
        [Route("api/motos")]
        public async Task<ActionResult<Moto>> PostMoto([FromBody] Moto moto)
        {
            if (moto == null)
            {
                return BadRequest("Moto não pode ser nula.");
            }
            await _context.Motos.AddAsync(moto); // Adiciona a moto ao contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto); // Retorna o status 201 Created com a localização do recurso criado - Ex: /api/motos/1
        }
    }
}
