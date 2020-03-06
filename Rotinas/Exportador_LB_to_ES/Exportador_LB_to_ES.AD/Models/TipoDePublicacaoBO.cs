using System;

namespace Exportador_LB_to_ES.AD.Models
{
    [Serializable]
    public class TipoDePublicacaoBO
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public TipoDePublicacaoBO()
        {
        }

        public TipoDePublicacaoBO(string nome)
        {
            Nome = nome;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Nome) ? "Não tem definido." : Nome;
        }
    }
}
