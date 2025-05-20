namespace Cp2Mottu.Application.DTOs.Moto
{
    public class MotoAtualizarDto
    {
        public string? Placa { get; set; } // Placa da moto. `?` para permitir nulo
        public string? Modelo { get; set; } // Modelo da moto (string para facilitar a conversão do enum)
        public int? IdFilial { get; set; } // Nome da filial relacionada à moto
        public MotoAtualizarDto()
        {
            
        }
    }
}
