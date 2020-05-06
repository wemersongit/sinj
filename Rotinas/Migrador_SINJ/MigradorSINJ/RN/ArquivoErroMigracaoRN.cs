using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class ArquivoErroMigracaoRN
    {
        private ArquivoErroMigracaoAD _arquivoErroMigracaoAd;

        public ArquivoErroMigracaoRN()
        {
            _arquivoErroMigracaoAd = new ArquivoErroMigracaoAD();
        }

        public ulong Incluir(ArquivoErroMigracaoOV arquivoErroMigracaoOv)
        {
            arquivoErroMigracaoOv.ch_para_nao_duplicacao = arquivoErroMigracaoOv.nm_base + "#" + arquivoErroMigracaoOv.id_doc_arquivo;
            return _arquivoErroMigracaoAd.Incluir(arquivoErroMigracaoOv);
        }

    }
}
