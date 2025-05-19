using Cp2Mottu.Domain.Exceptions;
using System.Runtime.Serialization;

namespace Cp2Mottu.Domain.Persistence
{
    public class Filial
    {
        public int Id { get; private set; } // Identificador da filial
        public string Nome { get; private set; } // Nome da filial, usa private set para não permitir alterações externas
        public string Endereco { get; private set; } // Endereço da filial, usa private set para não permitir alterações externas
        public ICollection<Moto> Motos { get; private set; } = new List<Moto>(); // Coleção de motos associadas à filial, usa private set para não permitir alterações externas

        public Filial(string nome, string endereco)
        {
            SetNome(nome);
            SetEndereco(endereco);
        }

        private void SetEndereco(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
            {
                throw new DomainExcepetion("Endereço não pode ser nulo ou vazio.", nameof(endereco)); // Verifica se o endereço é nulo ou vazio
            }

            this.Endereco = endereco; // Atribui o endereço
        }

        private void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(nome)); // Verifica se o nome é nulo ou vazio
            }
            this.Nome = nome; // Atribui o nome
        }

        public void AlterarEndereco(string novoEndereco)
        {
            SetEndereco(novoEndereco);
        }

        public void AlterarNome(string novoNome)
        {
            SetNome(novoNome);
        }

    }
}