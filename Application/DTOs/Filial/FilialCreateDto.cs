namespace Cp2Mottu.Application.DTOs.Filial
{
    public class FilialCreateDto
    {
        public string Nome { get; set; }
        public string Endereco { get; set; }

        public FilialCreateDto(string nome, string endereco)
        {
            Nome = nome;
            Endereco = endereco;
        }
    }
}
