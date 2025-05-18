namespace Cp2Mottu.Application.DTOs.Moto
{
    public class MotoUpdateDto
    {
        public int Id { get; set; } // ID da moto a ser atualizada
        public string Placa { get; set; } // Placa da moto
        public string Modelo { get; set; } // Modelo da moto (string para facilitar a conversão do enum)
        public string NomeFilial { get; set; } // Nome da filial relacionada à moto
        public MotoUpdateDto(int id, string placa, string modelo, string nomeFilial)
        {
            Id = id;
            Placa = placa;
            Modelo = modelo;
            NomeFilial = nomeFilial;
        }
    }
}
