using System;
using System.Collections.Generic;

namespace Exportador_LB_to_ES.AD.Models
{
    [Serializable]
    public class Push
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string passNoMd5 { get; set; }
        public DateTime DtCadUsuario { get; set; }
        public DateTime DtCadAssinatura { get; set; }
        public bool AtivoUsuario { get; set; }
        public List<AtosVerifAtlzcao> AtosVerifAtlzcaoValue { get; set; }
        public List<NovosAtosPorCriterios> NovosAtosPorCriteriosValue { get; set; }
    }
}
