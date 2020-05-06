using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;
using System.Data;

namespace MigradorSINJ.AD
{
    public class ArquivoErroMigracaoAD
    {
        private REST _rest;

        public ArquivoErroMigracaoAD()
        {
            _rest = new REST(Config.ValorChave("NmBaseArquivoErroMigracao", true));
        }

        internal ulong Incluir(ArquivoErroMigracaoOV arquivoErroMigracaoOv)
        {
            return _rest.Incluir(arquivoErroMigracaoOv);
        }

    }
}