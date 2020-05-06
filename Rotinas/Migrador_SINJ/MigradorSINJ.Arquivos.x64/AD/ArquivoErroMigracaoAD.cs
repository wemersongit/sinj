using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using MigradorSINJ.Arquivos.x64.OV;
using util.BRLight;

namespace MigradorSINJ.Arquivos.x64.AD
{
    public class ArquivoErroMigracaoAD
    {
        private AcessoAD<ArquivoErroMigracaoOV> _acessoAd;

        public ArquivoErroMigracaoAD()
        {
            _acessoAd = new AcessoAD<ArquivoErroMigracaoOV>(Config.ValorChave("NmBaseArquivoErroMigracao", true));
        }

        internal Results<ArquivoErroMigracaoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno, string nm_base)
        {
            return new AcessoAD<object>(nm_base).pathPut(id_doc, path, value, retorno);
        }

        internal string AnexarArquivo(FileParameter fileParameter, string nm_base)
        {
            string resultado; var doc = new Doc(nm_base) { TimeOut = 1200000 };
            var dicionario = new Dictionary<string, object>();
            dicionario.Add("file", fileParameter);
            resultado = doc.incluir(dicionario);
            return resultado;
        }

        internal bool Excluir(ulong id_doc)
        {
            return _acessoAd.Excluir(id_doc);
        }
    }
}
