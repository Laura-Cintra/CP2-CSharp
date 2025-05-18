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

    
    /// <summary>
    /// Obtém uma lista de todas as filiais sem as motos associadas.
    /// </summary>
    /// <returns>
    /// Retorna uma lista de objetos FiliaisReadDto representando as filiais sem as motos associadas.
    /// Retorna 200 OK se as filiais forem encontradas.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    public async Task<ActionResult<IEnumerable<FilialReadDto>>> GetFiliais()
    {
        var filiais = await _context.Filiais.ToListAsync(); // Inclui as motos relacionadas à filial na consulta

        var filiaisDto = filiais.Select(f => new FiliaisReadDto
        {
            Id = f.Id,
            Nome = f.Nome,
            Endereco = f.Endereco,
        }).ToList(); // Mapeia as filiais para o DTO Filial sem as motos associadas

        return Ok(filiaisDto);
    }


    /// <summary>
    ///  Retorna uma filial específica pelo Id passado por parâmetro junto com suas motos relacionadas.
    /// </summary>
    /// <param name="id">
    /// Identificador da filial a ser retornada.
    /// </param>
    /// <returns>
    /// Retorna um objeto FilialReadDto representando a filial encontrada.
    /// Retorna 200 OK se a filial for encontrada, ou 404 Not Found se não houver filial com o ID fornecido.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// </returns>
    [HttpGet("{id}")] // Define a rota para obter uma filial específica pelo ID
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found caso a filial não seja encontrada
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request caso o ID não seja válido (não seja um número inteiro)
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

    /// <summary>
    /// Cria uma nova filial no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="filialCreateDto">
    /// Dto de criação da filial, contendo os dados necessários para criar uma nova filial.
    /// </param>
    /// <returns>
    /// Retorna um objeto FilialReadDto representando a filial criada.
    /// Retorna 201 Created se a filial for criada com sucesso, ou 400 se o objeto filialCreateDto não for passado corretamente no corpo.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] // Indica que este método pode retornar um sucesso 201 Created caso a filial seja criada com sucesso
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request caso o objeto filialCreateDto não seja passado corretamente no corpo
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

    /// <summary>
    ///  Altera um ou mais dados de uma filial existente no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="id">ID da filial a ser atualizada</param>
    /// <param name="filialUpdateDto">Objeto contendo um ou mais atributos de uma filial</param>
    /// <returns>
    /// Retorna um objeto FilialReadDto representando a filial atualizada.
    /// Retorna 200 OK se a filial for atualizada com sucesso, ou 404 Not Found se não houver filial com o ID fornecido.
    /// REtorna 400 Bad Request se o objeto filialUpdateDto não for passado corretamente no corpo.
    /// </returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK caso a filial seja atualizada com sucesso
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Deleta uma filial existente no banco de dados com o ID fornecido.
    /// </summary>
    /// <param name="id">
    /// ID da filial a ser excluída.
    /// </param>
    /// <returns>
    /// REtona 204 No Content se a filial for excluída com sucesso, ou 404 Not Found se não houver filial com o ID fornecido.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // Indica que este método pode retornar um sucesso 204 noContent caso a filial seja excluída com sucesso
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found caso a filial não seja encontrada
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
