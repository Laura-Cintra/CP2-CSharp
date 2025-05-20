using Cp2Mottu.Application.DTOs.Moto;

namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialLeituraDto
    {
        public int Id { get; set; } // Identificador da filial
        public string Nome { get; set; } // Nome da filial
        public string Endereco { get; set; } // Endereço da filial
        public ICollection<MotoLeituraDto> Motos { get; set; } = new List<MotoLeituraDto>();// Coleção de motos associadas à filial
        public FilialLeituraDto(int id, string nome, string endereco, ICollection<MotoLeituraDto> motos)
        {
            Id = id;
            Nome = nome;
            Endereco = endereco;
            Motos = motos;
        }

        public FilialLeituraDto() { }
    }
}
