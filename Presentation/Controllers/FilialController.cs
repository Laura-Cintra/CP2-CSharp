using Cp2Mottu.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Cp2Mottu.Presentation.Controllers;


[Route("api/[controller]")] // Define a rota base para o controller, removendo o prefixo "api" do caminho da URL, ficando apenas "filiais"
[ApiController] // Indica que este controller é um controlador de API
[Tags("Filiais")] // Define a tag para o Swagger, que agrupa os endpoints deste controller na documentação
public class FilialController : ControllerBase
{

    private readonly AppDbContext _context;
    public FilialController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<Domain.Persistence.Filial>>> GetFiliais()
    {
        return Ok(await _context.Filiais.ToListAsync());
    }

    [HttpGet("{id}")] // Define a rota para obter uma filial específica pelo ID
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Domain.Persistence.Filial>> GetFilial(int id)
    {
        var filial = await _context.Filiais.FindAsync(id);
        if (filial == null)
        {
            return NotFound();
        }
        return Ok(filial);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] // Indica que este método pode retornar um sucesso 201 Created
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Domain.Persistence.Filial>> PostFilial([FromBody] Domain.Persistence.Filial filial)
    {
        if (filial == null)
        {
            return BadRequest("Filial não pode ser nula.");
        }
        await _context.Filiais.AddAsync(filial);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFilial), new { id = filial.Id }, filial);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PatchFilial(int id, [FromBody] Domain.Persistence.Filial filial)
    {
        if (id != filial.Id)
        {
            return BadRequest("ID da filial não corresponde ao ID do recurso.");
        }
        _context.Entry(filial).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(filial); // Retorna a filial atualizada
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // Indica que este método pode retornar um sucesso 204 noContent
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteFilial(int id)
    {
        var filial = await _context.Filiais.FindAsync(id);
        if (filial == null)
        {
            return NotFound();
        }
        _context.Filiais.Remove(filial);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
