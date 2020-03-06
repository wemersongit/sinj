using System;
using System.Collections.Generic;
using System.Linq;

namespace Sinj.Notifica.Objetos
{
    public class Norma
    {

        public Norma()
        {
            Tipo = new TipoDeNorma();
            Origens = new List<OrgaoSinj>();
            NeoIndexacao = new List<NeoIndexacao>();
        }

        public int Id { get; set; }
        public TipoDeNorma Tipo { get; set; }
        public string Numero { get; set; }
        public DateTime DataAssinatura { get; set; }
        public List<OrgaoSinj> Origens { get; set; }
        public List<string> Indexacao
        {
            get { return GetIndexacao(); }
        }
        public List<NeoIndexacao> NeoIndexacao { get; set; }
        public string Ementa { get; set; }
        public bool AtlzNorma { get; set; }
        public bool NovaNorma { get; set; }
        private List<string> GetIndexacao()
        {
            List<string> termos = new List<string>();
            foreach (var indexacao in NeoIndexacao)
            {
                if (termos.Count > 0)
                {
                    string ultimo = termos.Last();
                    string[] ultimoSplit = ultimo.Split(',');
                    if (ultimoSplit[0].Trim(' ') == indexacao.NmTermo && !string.IsNullOrEmpty(indexacao.NmEspecificador))
                    {
                        int index = termos.LastIndexOf(ultimo);
                        termos[index] = ultimo + ", " + indexacao.NmEspecificador;
                    }
                    else
                    {
                        termos.Add(indexacao.NmTermo + (!string.IsNullOrEmpty(indexacao.NmEspecificador) ? ", " + indexacao.NmEspecificador : ""));
                    }
                }
                else
                {
                    termos.Add(indexacao.NmTermo + (!string.IsNullOrEmpty(indexacao.NmEspecificador) ? ", " + indexacao.NmEspecificador : ""));
                }
            }
            return termos;
        }
    }

    public class NeoIndexacao
    {
        public int InTipoTermo { get; set; }
        public string NmTermo { get; set; }
        public string NmEspecificador { get; set; }
        public string NmTermoAuxiliar { get; set; }
        public string NmEspecificadorAuxiliar { get; set; }
    }
}