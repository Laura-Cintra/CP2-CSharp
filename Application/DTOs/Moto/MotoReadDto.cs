namespace Cp2Mottu.Application.DTOs.Moto
{
    public class MotoReadDto
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public string NomeFilial { get; set; }

        public MotoReadDto(int id, string placa, string modelo, string nomeFilial)
        {
            Id = id;
            Placa = placa;
            Modelo = modelo;
            NomeFilial = nomeFilial;
        }
    }   
}
