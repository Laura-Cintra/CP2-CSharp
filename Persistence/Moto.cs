using System.ComponentModel.DataAnnotations;

namespace Cp2Mottu.Persistence
{
    public class Moto
    {
        public int Id { get; private set; } // Guid Global Unique Identifier
        
        [StringLength(7, MinimumLength = 7)] // Define o tamanho máximo e mínimo da string
        public string Placa { get; set; }

        // Define o relacionamento com a tabela de Modelos
        //public Guid IdModelo { get; set; }
        // Propriedade de navegação para o modelo
        //public Modelo Modelo { get; set; }

        // Define o relacionamento com a tabela de Filiais
        //public Guid IdFilial { get; set; }
        // Propriedade de navegação para a filial
        //public Filial Filial { get; set; }
    }
}
