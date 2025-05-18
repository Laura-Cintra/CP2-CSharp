using System.ComponentModel.DataAnnotations;

namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialCreateDto
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Endereco { get; set; }

        public FilialCreateDto(string nome, string endereco)
        {
            Nome = nome;
            Endereco = endereco;
        }
    }
}
