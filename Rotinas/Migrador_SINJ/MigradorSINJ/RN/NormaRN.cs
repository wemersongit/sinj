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
    public class NormaRN
    {
        private NormaAD _normaAd;

        public NormaRN()
        {
            _normaAd = new NormaAD();
        }

        public NormaOV BuscarNorma(string id_norma, string[] select)
        {
            return _normaAd.BuscarNorma(id_norma, select);
        }

        public List<NormaOV> BuscarNormasRest(int limit)
        {
            return _normaAd.BuscarNormasRest(limit);
        }
        public NormaLBW BuscarCaminhosArquivos(string ch_norma)
        {
            return _normaAd.BuscarCaminhosArquivos(ch_norma);
        }

        public List<NormaLBW> BuscarCaminhosArquivosNormasLBW()
        {
            return _normaAd.BuscarCaminhosArquivosNormasLBW();
        }

        public List<NormaLBW> BuscarNormasIndexacaoLBW(List<string> ids)
        {
            var where = "";
            foreach (var id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    where += (where != "" ? " or " : "") + "Id=" + id;
                }
            }
            return _normaAd.BuscarNormasIndexacaoLBW("select Id, NeoIndexacao from versoesdasnormas where " + where);
        }

        public List<NormaLBW> BuscarTodasNormasIndexacaoLBW()
        {
            return _normaAd.BuscarNormasLBW("select Id, NeoIndexacao from versoesdasnormas");
        }

        public List<NormaLBW> BuscarNormasLBW(List<string> ids)
        {
            var where = "";
            foreach (var id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    where += (where != "" ? " or " : "") + "Id=" + id;
                }
            }
            return _normaAd.BuscarNormasLBW("select Id, Id_Tipo, Numero, DataAssinatura, DataDaUltimaAlteracao, Ambito, Apelido, UrlReferenciaExterna, HaPendencia, Destacada, Autorias, Letra, NumeroSequencial, NomeDoOrgao, Situacao, UsuarioQueCadastrou, DataDoCadastro, UsuarioDaUltimaAlteracao, DataDaUltimaAlteracao, ObservacaoNorma, ChaveParaNaoDuplicacao, Ementa, ListaDeNomes, Origens, ParametroConstitucional, Procedencia, Relator, DataDeAutuacao, Requerente, Requerido, ProcuradorResponsavel, InteressadoDaAcao, EfeitoDaDecisao, Fontes, Vides, NeoIndexacao, AuxiliarDeRankeamento from versoesdasnormas where " + where);
        }

        public List<string> BuscarIdsNormasLBW()
        {
            return _normaAd.BuscarIdsNormasLBW();
        }


        public List<NormaLBW> BuscarUsuarioCadastradorNormasLBW()
        {
            return _normaAd.BuscarUsuarioCadastradorNormasLBW();
        }

        
        public ArquivoOV EnviarArquivo(ulong id_doc,string caminho, string file_name, string content_type)
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
                        var sRetorno = _normaAd.AnexarArquivo(fileParameter);
                        arquivo = JSON.Deserializa<ArquivoOV>(sRetorno);
                    }
                }
            }
            return arquivo;
        }

        public ulong Incluir(NormaOV normaOv)
        {
            return _normaAd.Incluir(normaOv);
        }

        public bool Atualizar(ulong id_doc, NormaOV normaOv)
        {
            return _normaAd.Atualizar(id_doc, normaOv);
        }

        public bool AtualizarPath(ulong id_doc, string caminho, string valor, string retorno)
        {
            return _normaAd.AtualizarPath(id_doc, caminho, valor, retorno);
        }

        public List<String> BuscarNormaJSON(string filename)
        {
            return _normaAd.BuscarNormaJSON(filename);
        }
    }
}
