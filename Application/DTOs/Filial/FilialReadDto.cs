using Cp2Mottu.Application.DTOs.Moto;

namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialReadDto
    {
        public int Id { get; set; } // Identificador da filial
        public string Nome { get; set; } // Nome da filial
        public string Endereco { get; set; } // Endereço da filial
        public ICollection<MotoReadDto> Motos { get; set; } = new List<MotoReadDto>();// Coleção de motos associadas à filial
        public FilialReadDto(int id, string nome, string endereco, ICollection<MotoReadDto> motos)
        {
            Id = id;
            Nome = nome;
            Endereco = endereco;
            Motos = motos;
        }

        public FilialReadDto() { }
    }
}
