using System;

namespace Exportador_LB_to_ES.AD.Models
{
    [Serializable]
    public class TipoDeFonteBO
    {
        public long Id { get; set; }
        public string Nome { get; set; }

        public TipoDeFonteBO()
        {
        }

        public TipoDeFonteBO(string nome)
        {
            Nome = nome;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Nome) ? "Não tem definido." : Nome;
        }
    }
}
