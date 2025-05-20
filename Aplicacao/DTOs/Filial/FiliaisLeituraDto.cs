namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FiliaisLeituraDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }

        public FiliaisLeituraDto(int id, string nome, string endereco)
        {
            Id = id;
            Nome = nome;
            Endereco = endereco;
        }

        public FiliaisLeituraDto() { } // Construtor padrão para inicialização sem parâmetros
    }
}
