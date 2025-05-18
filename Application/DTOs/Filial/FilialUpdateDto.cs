using Cp2Mottu.Application.DTOs.Moto;

namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialUpdateDto
    {
        public string? Nome { get; set; }
        public string? Endereco { get; set; }
        public FilialUpdateDto(string nome, string endereco)
        {
     
            Nome = nome;
            Endereco = endereco;
        }

        public FilialUpdateDto() { } // Construtor padrão para inicialização sem parâmetros
    }
}
