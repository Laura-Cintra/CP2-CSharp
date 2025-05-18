using Cp2Mottu.Application.DTOs.Moto;
using Cp2Mottu.Domain.Enums;
using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Presentation.Controllers
{
    [Route("api/[controller]")] // Define a rota base para o controller, removendo o prefixo "api" do caminho da URL, ficando apenas "motos"
    [ApiController] // Indica que este controller é um controlador de API
    [Tags("Motos")] // Define a tag para o Swagger, que agrupa os endpoints deste controller na documentação
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotosController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            var motos = await _context.Motos.Include(m => m.Filial).ToListAsync(); // Busca todas as motos no banco de dados e inclui a entidade Filial relacionada

            var motosDto = motos.Select(m => new MotoReadDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo.ToString(), // Converte o enum ModeloMoto para string
                NomeFilial = m.Filial.Nome // Inclui o nome da filial relacionada
            }).ToList(); // Mapeia as motos para o DTO Moto, incluindo a entidade Filial relacionada

            return Ok(motosDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos.Include(m => m.Filial).ToListAsync(); // Busca a moto pelo ID
            if (moto == null) // Se não encontrar, retorna 404 Not Found
            {
                return NotFound();
            }
            var motoDto = moto.Select(moto => new MotoReadDto
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Modelo = moto.Modelo.ToString(), // Converte o enum ModeloMoto para string
                NomeFilial = moto.Filial.Nome // Inclui o nome da filial relacionada
            });
            return Ok(motoDto); // Retorna a moto encontrada
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // Indica que este método pode retornar um sucesso 201 Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<Moto>> PostMoto([FromBody] MotoCreateDto motoDto)
        {

            if (motoDto == null)
            {
                return BadRequest("Moto não pode ser nula.");
            }
            if (!Enum.IsDefined(typeof(ModeloMoto), motoDto.Modelo)) // Verifica se o modelo é válido, pode passar tanto id quanto o enum
            {
                return BadRequest("Modelo inválido.");
            }
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == motoDto.Placa); // Verifica se a placa já existe no banco de dados
            
            if (moto != null)
            {
                return BadRequest("Placa já existe.");
            }
            var filial = await _context.Filiais.FindAsync(motoDto.IdFilial); // Busca a filial pelo ID
            moto = new Moto(motoDto.Placa, (int)motoDto.Modelo, motoDto.IdFilial, filial); // Cria uma nova moto com os dados do DTO e a filial encontrada
            await _context.Motos.AddAsync(moto); // Adiciona a moto ao contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            var motoReadDto = new MotoReadDto(
                moto.Id,
                moto.Placa,
                moto.Modelo.ToString(),
                moto.Filial.Nome
            ); // Cria um DTO de leitura com os dados da moto criada
            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, motoReadDto); // Retorna o DTO de leitura com o status 201 Created, incluindo o ID da moto criada e o caminho para obter a moto
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable
        public async Task<ActionResult<Moto>> PatchMoto(int id, [FromBody] MotoUpdateDto motoUpdateDto)
        {
            if (id != motoUpdateDto.Id) // Verifica se o ID da URL é igual ao ID do DTO
            {
                return BadRequest("ID da URL não corresponde ao ID do DTO.");
            }

            var moto = await _context.Motos.Include(m => m.Filial).FirstOrDefaultAsync(m => m.Id == id); // Busca a moto pelo ID

            if (moto == null) // Se não encontrar, retorna 404 Not Found
            {
                return NotFound();
            }
            
            var filial = await _context.Filiais.FindAsync(moto.IdFilial); // Busca a filial pelo ID
            moto.AlterarPlaca(motoUpdateDto.Placa); // Atualiza a placa da moto
            moto.AlterarModelo(motoUpdateDto.Modelo); // Atualiza o modelo da moto, convertendo o enum para int

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
