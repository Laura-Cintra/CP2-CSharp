using Cp2Mottu.Aplicacao.Repositorios;
using Cp2Mottu.Application;
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
public class FilialControlador : ControllerBase
{

    private readonly FilialRepositorio _repositorio;
    private readonly MotoRepositorio _motoRepositorio;
    public FilialControlador(FilialRepositorio repositorio, MotoRepositorio motoRepositorio)
    {
        _repositorio = repositorio;
        _motoRepositorio = motoRepositorio;
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error caso ocorra algum erro inesperado
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable caso o serviço esteja indisponível
    public async Task<ActionResult<IEnumerable<FilialLeituraDto>>> GetFiliais()
    {
        var filiais = await _repositorio.ObterTodos(); // Altera para obter as filiais através do repositório

        var filiaisDto = filiais.Select(f => new FiliaisLeituraDto
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
    /// ID da filial a ser retornada.
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error caso ocorra algum erro inesperado
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable caso o serviço esteja indisponível
    public async Task<ActionResult<FilialLeituraDto>> GetFilial(int id)
    {
        var filial = await _repositorio.ObterPorId(id);

        if (filial == null)
        {
            return NotFound();
        }

        var filialDto = new FilialLeituraDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoLeituraDto
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error caso ocorra algum erro inesperado
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable caso o serviço esteja indisponível
    public async Task<ActionResult<FilialLeituraDto>> CriarFilial([FromBody] FilialCriarDto filialCreateDto)
    {
        if (filialCreateDto == null)
        {
            return BadRequest("Filial não pode ser nula.");
        }

        var filial = new Filial(filialCreateDto.Nome, filialCreateDto.Endereco); // Cria uma nova filial com os dados do DTO

        await _repositorio.Adicionar(filial); // Adicionar a filial ao repositório


        
        var filialReadDto = new FilialLeituraDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco
        }; // Cria um DTO de leitura com os dados da filial criada
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
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found caso a filial não seja encontrada
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error caso ocorra algum erro inesperado
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable caso o serviço esteja indisponível
    public async Task<IActionResult> PatchFilial(int id, [FromBody] FilialAtualizarDto filialUpdateDto)
    {
        var filial = await _repositorio.ObterPorId(id);

        if (filial == null)
        {
            return NotFound();
        }
        if (filialUpdateDto.Endereco != null)
        {
            filial.AlterarEndereco(filialUpdateDto.Endereco);
        }
        if (filialUpdateDto.Nome != null)
        {
            filial.AlterarNome(filialUpdateDto.Nome);
        }

        await _repositorio.Atualizar(filial);

        var filialReadDto = new FilialLeituraDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoLeituraDto
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request caso o ID não seja válido (não seja um número inteiro)
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que este método pode retornar um erro 500 Internal Server Error caso ocorra algum erro inesperado
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que este método pode retornar um erro 503 Service Unavailable caso o serviço esteja indisponível
    public async Task<IActionResult> DeletaFilial(int id)
    {
        var filial = await _repositorio.ObterPorId(id);
        if (filial == null)
        {
            return NotFound();
        }

        await _repositorio.Remover(filial);
        
        return NoContent();
    }
}
