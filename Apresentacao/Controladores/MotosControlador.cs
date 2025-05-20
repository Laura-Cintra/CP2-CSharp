using Cp2Mottu.Aplicacao.Repositorios;
using Cp2Mottu.Application;
using Cp2Mottu.Application.DTOs.Moto;
using Cp2Mottu.Domain.Enums;
using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Presentation.Controllers;



[Route("api/[controller]")] // Define a rota base para o controller, removendo o prefixo "api" do caminho da URL, ficando apenas "motos"
[ApiController] // Indica que este controller é um controlador de API
[Tags("Motos")] // Define a tag para o Swagger, que agrupa os endpoints deste controller na documentação
public class MotosControlador : ControllerBase
{
    private readonly MotoRepositorio _repositorio;
    private readonly FilialRepositorio _filialRepositorio;

    public MotosControlador(MotoRepositorio repositorio, FilialRepositorio filialRepositorio)
    {
        _repositorio = repositorio;
        _filialRepositorio = filialRepositorio;
    }   


    /// <summary>
    /// Retorna a lista de motos cadastradas no sistema.
    /// </summary>
    /// <returns>
    /// Retorna 200 OK com a lista de motos cadastradas.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que ocorreu um erro interno no servidor
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que o serviço está temporariamente indisponível
    public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
    {
        var motos = await _repositorio.ObterTodos();

        

        var motosDto = motos.Select(m => new MotoLeituraDto
        {
            Id = m.Id,
            Placa = m.Placa,
            Modelo = m.Modelo.ToString().ToUpper(), // Converte o enum ModeloMoto para string
            NomeFilial = m.Filial.Nome // Inclui o nome da filial relacionada
        }).ToList(); // Mapeia as motos para o DTO Moto, incluindo a entidade Filial relacionada

        return Ok(motosDto);
    }

    /// <summary>
    /// Retorna uma moto específica pelo ID passado por parâmetro.
    /// </summary>
    /// <param name="id"> ID da moto a ser buscada </param>
    /// <returns>
    /// Retorna 200 OK com a moto encontrada ou 404 Not Found se a moto não for encontrada.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request caso o ID não seja válido (não seja um número inteiro)
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que ocorreu um erro interno no servidor
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que o serviço está temporariamente indisponível
    public async Task<ActionResult<Moto>> GetMoto(int id)
    {
        var moto = await _repositorio.ObterPorId(id);
        if (moto == null) // Se não encontrar, retorna 404 Not Found
        {
            return NotFound();
        }
        var motoDto = new MotoLeituraDto
        {
            Id = moto.Id,
            Placa = moto.Placa,
            Modelo = moto.Modelo.ToString().ToUpper(), // Converte o enum ModeloMoto para string
            NomeFilial = moto.Filial.Nome // Inclui o nome da filial relacionada
        };
        return Ok(motoDto); // Retorna a moto encontrada
    }


    /// <summary>
    /// Insere uma nova moto no sistema.
    /// </summary>
    /// <param name="motoDto"> 
    /// Objeto que representa o cadastro de uma nova moto.
    /// </param>
    /// <returns>
    /// Retorna 201 Created com a moto criada ou 400 Bad Request se o corpo da requisição não for válido.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] // Indica que este método pode retornar um sucesso 201 Created
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que o corpo da requisição não é válido
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found caso a filial não seja encontrada
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Indica que este método pode retornar um erro 409 Conflict caso a placa já exista
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que ocorreu um erro interno no servidor
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que o serviço está temporariamente indisponível

    public async Task<ActionResult<Moto>> CriarMoto([FromBody] MotoCriarDto motoDto)
    {
        var filial = await _filialRepositorio.ObterPorId(motoDto.IdFilial);

        if (filial == null)
        {
            return NotFound("Filial não encontrada.");
        }
        
        if(Enum.IsDefined(typeof(ModeloMoto), motoDto.Modelo.ToUpper()) == false) // Verifica se o modelo é válido
        {
            return BadRequest("Modelo inválido.");
        }
        var moto = await _repositorio.Adicionar(new Moto(motoDto.Placa, motoDto.Modelo, motoDto.IdFilial, filial));

        var motoLeituraDto = new MotoLeituraDto
        {
            Id = moto.Id,
            Placa = moto.Placa,
            Modelo = moto.Modelo.ToString(),
            NomeFilial = moto.Filial.Nome
        };


        return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, motoLeituraDto); // Retorna o DTO de leitura com o status 201 Created, incluindo o ID da moto criada e o caminho para obter a moto
    }


    /// <summary>
    /// Retorna a moto com as informações atualizadas.
    /// </summary>
    /// <param name="id"> ID da moto a ser atualizada </param>
    /// <param name="motoUpdateDto">Objeto contendo um ou mais atributos de moto a serem atualizados</param>
    /// <returns>
    /// Retorna 200 OK com a moto atualizada ou 404 Not Found se a moto ou nova filial não for encontrada.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Indica que este método pode retornar um sucesso 200 OK
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request caso o corpo da requisição não seja válido
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found caso a moto não seja encontrada
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que ocorreu um erro interno no servidor
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que o serviço está temporariamente indisponível
    public async Task<ActionResult<Moto>> PatchMoto(int id, [FromBody] MotoAtualizarDto motoUpdateDto)
    {
        var moto = await _repositorio.ObterPorId(id); // Busca a moto pelo ID

        if (moto == null) // Se não encontrar, retorna 404 Not Found
        {
            return NotFound("Moto não encontrada");
        }

        var filial = await _filialRepositorio.ObterPorId(moto.IdFilial); // Busca a filial pelo ID

        if (motoUpdateDto.Placa != null)
        {
            moto.AlterarPlaca(motoUpdateDto.Placa);
        }
        if (motoUpdateDto.Modelo != null)
        {
            moto.AlterarModelo(motoUpdateDto.Modelo.ToString()); // Converte o enum para string
        }
        if (motoUpdateDto.IdFilial != null)
        {
            var novaFilial = await _filialRepositorio.ObterPorId(motoUpdateDto.IdFilial.Value); // Busca a nova filial pelo ID
            if (novaFilial == null) // Se não encontrar, retorna 404 Not Found
            {
                return NotFound("Filial não encontrada.");
            }
            moto.AlterarFilial(motoUpdateDto.IdFilial.Value, novaFilial); // Atualiza a filial
        }
        var motoAtualizada = await _repositorio.Atualizar(moto);
        var motoReadDto = new MotoLeituraDto(
            motoAtualizada.Id,
            motoAtualizada.Placa,
            motoAtualizada.Modelo.ToString(),
            motoAtualizada.Filial.Nome
        ); // Cria um DTO de leitura com os dados da moto atualizada para evitar circularidade de repetição de dados da filial
        return Ok(motoReadDto); // Retorna a moto atualizada
    }


    /// <summary>
    ///  Retorna código 204 confirmando a exclusão da moto com o ID passado por parâmetro.
    /// </summary>
    /// <param name="id"> ID da moto a ser excluída </param>
    /// <returns>
    /// Retorna código 204 No Content se a moto for excluída com sucesso ou 404 Not Found se a moto não for encontrada.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // Indica que este método pode retornar um sucesso 204 No Content
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Indica que este método pode retornar um erro 404 Not Found
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indica que este método pode retornar um erro 400 Bad Request caso o ID não seja válido (não seja um número inteiro)
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Indica que ocorreu um erro interno no servidor
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // Indica que o serviço está temporariamente indisponível
    public async Task<IActionResult> DeletarMoto(int id)
    {
        var moto = await _repositorio.ObterPorId(id); // Busca a moto pelo ID
        if (moto == null) // Se não encontrar, retorna 404 Not Found
        {
            return NotFound();
        }
        await _repositorio.Remover(moto); // Remove a moto do contexto
        return NoContent(); // Retorna 204 No Content
    }
}


