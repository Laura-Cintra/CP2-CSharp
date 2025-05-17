using Cp2Mottu.Context;
using Cp2Mottu.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Controllers
{
    [Route("api/[controller]")] // Define a rota base para o controller, removendo o prefixo "api" do caminho da URL, ficando apenas "motos"
    [ApiController] // Indica que este controller é um controlador de API
    [Tags("Motos")] // Define a tag para o Swagger, que agrupa os endpoints deste controller na documentação
    public class MotosController : ControllerBase
    {
        private readonly MotosContext _context;

        public MotosController(MotosContext context)
        {
            this._context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return Ok(await _context.Motos.ToListAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable

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
        [ProducesResponseType(StatusCodes.Status201Created)] // Indica que este método pode retornar um sucesso 201 Created
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

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable
        public async Task<ActionResult<Moto>> PatchMoto(int id, [FromBody] Moto moto)
        {
            if (id != moto.Id) // Verifica se o ID da URL é igual ao ID do objeto, se não for, retorna 400 Bad Request
            {
                return BadRequest("ID da URL não corresponde ao ID do objeto.");
            }
            _context.Entry(moto).State = EntityState.Modified; // Marca a entidade como modificada
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            return Ok(moto); // Retorna a moto atualizada
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Indica que este método pode retornar um sucesso 204 No Content
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable
        public async Task<IActionResult> DelteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id); // Busca a moto pelo ID
            if (moto == null) // Se não encontrar, retorna 404 Not Found
            {
                return NotFound();
            }
            _context.Motos.Remove(moto); // Remove a moto do contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            return NoContent(); // Retorna 204 No Content
        }
    }
}
