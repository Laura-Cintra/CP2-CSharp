namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FiliaisReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }

        public FiliaisReadDto(int id, string nome, string endereco)
        {
            Id = id;
            Nome = nome;
            Endereco = endereco;
        }

        public FiliaisReadDto() { } // Construtor padrão para inicialização sem parâmetros
    }
}
