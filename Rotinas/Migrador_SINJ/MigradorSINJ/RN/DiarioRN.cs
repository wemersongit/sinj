using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;
using System.IO;
using util.BRLight;

namespace MigradorSINJ.RN
{
    public class DiarioRN
    {
        private DiarioAD _diarioAd;

        public DiarioRN()
        {
            _diarioAd = new DiarioAD();
        }

        public DiarioLBW BuscarCaminhosArquivos(string ch_diario)
        {
            return _diarioAd.BuscarCaminhosArquivos(ch_diario);
        }

        public List<DiarioLBW> BuscarCaminhosArquivosDiariosLBW()
        {
            return _diarioAd.BuscarCaminhosArquivosDiariosLBW();
        }

        public List<DiarioLBW> BuscarDiariosLBW(List<string> ids)
        {
            var where = "";
            foreach (var id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    where += (where != "" ? " or " : "") + "Id=" + id;
                }
            }
            return _diarioAd.BuscarDiariosLBW("select Id, AlfaNumero, Sessao, OrgaoCadastrador, DataDaAssinatura, SituacaoQuantoAPendencia, UsuarioDaUltimaAlteracao, UsuarioQueCadastrou, DataDaUltimaAlteracao, DataDoCadastro from versoesdosdodfs where " + where);
        }

        public List<string> BuscarIdsDiariosLBW()
        {
            return _diarioAd.BuscarIdsDiariosLBW();
        }

        public ArquivoOV EnviarArquivo(ulong id_doc, string caminho, string file_name, string content_type)
        {
            var arquivo = new ArquivoOV();
            using (var streamReader = new StreamReader(caminho))
            {
                using (var binaryReader = new BinaryReader(streamReader.BaseStream))
                {
                    if (File.Exists(caminho))
                    {
                        var bytes = File.ReadAllBytes(caminho);
                        var fileParameter = new FileParameter(bytes, file_name, content_type);
                        var sRetorno = _diarioAd.AnexarArquivo(fileParameter);
                        arquivo = JSON.Deserializa<ArquivoOV>(sRetorno);
                    }
                }
            }
            return arquivo;
        }

        public ulong Incluir(DiarioOV diarioOv)
        {
            return _diarioAd.Incluir(diarioOv);
        }

        public bool Atualizar(ulong id_doc, DiarioOV diarioOv)
        {
            return _diarioAd.Atualizar(id_doc, diarioOv);
        }
    }
}
