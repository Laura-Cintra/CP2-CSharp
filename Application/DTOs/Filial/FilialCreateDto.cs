using System.ComponentModel.DataAnnotations;

namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialCreateDto
    {
        [Required(ErrorMessage = "Nome da filial não pode estar vazio")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Endereço da filial não pode estar vazio")]
        public string Endereco { get; set; }

        public FilialCreateDto(string nome, string endereco)
        {
            Nome = nome;
            Endereco = endereco;
        }
    }
}
