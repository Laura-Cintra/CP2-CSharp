using Cp2Mottu.Application.DTOs.Filial;
using Cp2Mottu.Application.DTOs.Moto;
using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // TODO: Criar o summary para o método GetFiliais

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<FilialReadDto>>> GetFiliais()
    {
        var filiais = await _context.Filiais.ToListAsync(); // Inclui as motos relacionadas à filial na consulta

        if (filiais == null)
        {
            return NotFound("Nenhuma filial encontrada."); // Retorna 404 se não encontrar nenhuma filial
        }

        var filiaisDto = filiais.Select(f => new FiliaisReadDto
        {
            Id = f.Id,
            Nome = f.Nome,
            Endereco = f.Endereco,
        }).ToList(); // Mapeia as filiais para o DTO Filial, incluindo a entidade Moto relacionada
        
        return Ok(filiaisDto);
    }


    // TODO: Criar o summary para o método GetFilial
    [HttpGet("{id}")] // Define a rota para obter uma filial específica pelo ID
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FilialReadDto>> GetFilial(int id)
    {
        var filial = await _context.Filiais.Include(f => f.Motos).FirstOrDefaultAsync(f => f.Id == id);

        if (filial == null)
        {
            return NotFound();
        }

        var filialDto = new FilialReadDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoReadDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo.ToString().ToUpper(), // Converte o enum ModeloMoto para string
                NomeFilial = filial.Nome // Inclui o nome da filial relacionada
            }).ToList() // Mapeia as motos para o DTO Moto, incluindo a entidade Filial relacionada
        };

        return Ok(filialDto);
    }

    // T: Criar o summary para o método PostFilial
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] // Indica que este método pode retornar um sucesso 201 Created
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FilialReadDto>> PostFilial([FromBody] FilialCreateDto filialCreateDto)
    {
        if (filialCreateDto == null)
        {
            return BadRequest("Filial não pode ser nula.");
        }

        var filial = new Filial(filialCreateDto.Nome, filialCreateDto.Endereco); // Cria uma nova filial com os dados do DTO

        await _context.Filiais.AddAsync(filial);
        await _context.SaveChangesAsync();
        var filialReadDto = new FilialReadDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoReadDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo.ToString().ToUpper(), // Converte o enum ModeloMoto para string
                NomeFilial = filial.Nome // Inclui o nome da filial relacionada
            }).ToList() // Mapeia as motos para o DTO Moto, incluindo a entidade Filial relacionada
        };
        return CreatedAtAction(nameof(GetFilial), new { id = filial.Id }, filialReadDto);
    }

    // TODO: Criar o summary para o método PutFilial
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PatchFilial(int id, [FromBody] FilialUpdateDto filialUpdateDto)
    {
        var filial = await _context.Filiais.FirstOrDefaultAsync(f => f.Id == id);
        if (filialUpdateDto.Endereco != null)
        {
            filial.AlterarEndereco(filialUpdateDto.Endereco);
        }
        if (filialUpdateDto.Nome != null)
        {
            filial.AlterarNome(filialUpdateDto.Nome);
        }

        _context.Entry(filial).State = EntityState.Modified; // Marca a entidade como modificada para que o EF Core saiba que ela foi alterada e precisa ser atualizada no banco de dados e não adicionada como uma nova entidade
        await _context.SaveChangesAsync();

        var filialReadDto = new FilialReadDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoReadDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo.ToString().ToUpper(), // Converte o enum ModeloMoto para string
                NomeFilial = filial.Nome // Inclui o nome da filial relacionada
            }).ToList() // Mapeia as motos para o DTO Moto, incluindo a entidade Filial relacionada
        };
        return Ok(filialReadDto); // Retorna a filial atualizada
    }

    // TODO: Criar o summary para o método DeleteFilial
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
