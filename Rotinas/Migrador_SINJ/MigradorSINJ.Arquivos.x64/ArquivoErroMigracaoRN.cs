using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using MigradorSINJ.Arquivos.x64.OV;
using MigradorSINJ.Arquivos.x64.AD;
using System.IO;
using util.BRLight;

namespace MigradorSINJ.Arquivos.x64
{
    public class ArquivoErroMigracaoRN
    {
        private ArquivoErroMigracaoAD _arquivoErroMigracaoAd;

        public ArquivoErroMigracaoRN()
        {
            _arquivoErroMigracaoAd = new ArquivoErroMigracaoAD();
        }
        public Results<ArquivoErroMigracaoOV> Consultar(Pesquisa query)
        {
            return _arquivoErroMigracaoAd.Consultar(query);
        }

        public string PathPut(ulong id_doc, string path, string value, string retorno, string nm_base)
        {
            return _arquivoErroMigracaoAd.PathPut(id_doc, path, value, retorno, nm_base);
        }

        public ArquivoOV EnviarArquivo(ArquivoErroMigracaoOV arquivoErroMigracaoOv)
        {
            var arquivo = new ArquivoOV();
            var caminho = Config.ValorChave("diretorio_arquivo", true) + arquivoErroMigracaoOv.path_file;
            var name_file = arquivoErroMigracaoOv.path_file.Split('\\').Last<string>();
            var content_type = MimeType.Get(name_file);
            using (var streamReader = new StreamReader(caminho))
            {
                using (var binaryReader = new BinaryReader(streamReader.BaseStream))
                {
                    if (System.IO.File.Exists(caminho))
                    {
                        var bytes = System.IO.File.ReadAllBytes(caminho);
                        var fileParameter = new FileParameter(bytes, name_file, content_type);
                        var sRetorno = _arquivoErroMigracaoAd.AnexarArquivo(fileParameter, arquivoErroMigracaoOv.nm_base);
                        arquivo = JSON.Deserializa<ArquivoOV>(sRetorno);
                    }
                }
            }
            return arquivo;
        }

        public bool Excluir(ulong id_doc)
        {
            return _arquivoErroMigracaoAd.Excluir(id_doc);
        }

    }
}
