using Cp2Mottu.Application.DTOs.Moto;

namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialAtualizarDto
    {
        public string? Nome { get; set; }
        public string? Endereco { get; set; }
        public FilialAtualizarDto(string nome, string endereco)
        {
            Nome = nome;
            Endereco = endereco;
        }

        public FilialAtualizarDto() { } // Construtor padrão para inicialização sem parâmetros
    }
}
