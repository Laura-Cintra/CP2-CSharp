using Cp2Mottu.Domain.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Cp2Mottu.Application.DTOs.Moto;

public class MotoCreateDto
{
    [Required(ErrorMessage = "Nome não pode estar vazio.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Placa não pode estar vazia.")]
    [StringLength(7, MinimumLength = 7,ErrorMessage = "Placa deve ter 7 caracteres.")] // Considerando apenas Brasil
    public string Placa { get; set; }

    [Required(ErrorMessage = "Modelo não pode estar vazio.")]
    public int Modelo { get; set; } // Considerando que o modelo é um inteiro que representa o enum ModeloMoto

    [Required(ErrorMessage = "Filial não pode estar vazia.")]
    public int IdFilial { get; set; } // Considerando que o idFilial é um inteiro que representa a filial

    public MotoCreateDto(string nome, string placa, int modelo, int idFilial)
    {
        Nome = nome;
        Placa = placa;
        Modelo = modelo;
        IdFilial = idFilial;
    }
}
