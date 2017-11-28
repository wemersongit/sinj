using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using util.BRLight;
using neo.BRLightREST;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using TCDF.Sinj.ESUtil;
using System.Web;

namespace TCDF.Sinj.RN
{
    public class DiarioRN
    {
        private DocEs _docEs;
        private DiarioAD _diarioAd;
        private List<SituacaoOV> _situacoes;
        private List<SituacaoOV> Situacoes
        {
            get
            {
                if (_situacoes == null || _situacoes.Count <= 0)
                {
                    Pesquisa pesquisa_situacao = new Pesquisa();
                    pesquisa_situacao.limit = null;
                    var result = new SituacaoRN().Consultar(pesquisa_situacao);
                    _situacoes = result.results;
                }
                return _situacoes;
            }
        }

        public DiarioRN()
        {
            _diarioAd = new DiarioAD();
            _docEs = new DocEs();
        }

        public Results<DiarioOV> Consultar(Pesquisa query)
        {
            return _diarioAd.Consultar(query);
        }

        public DiarioOV Doc(ulong id_doc)
        {
            return _diarioAd.Doc(id_doc);
        }

        public DiarioOV Doc(string ch_diario)
        {
            return _diarioAd.Doc(ch_diario);
        }

        public string JsonReg(ulong id_doc)
        {
            return _diarioAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _diarioAd.JsonReg(query);
        }

        /// <summary>
        /// Atualiza o campo especifico do documento
        /// </summary>
        /// <param name="id_doc"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <param name="retorno"></param>
        /// <returns>caso sucesso retorna "UPDATED"</returns>
        public string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _diarioAd.PathPut(id_doc, path, value, retorno);
        }

        public string PathPut<T>(Pesquisa pesquisa, List<opMode<T>> listopMode)
        {
            return _diarioAd.PathPut<T>(pesquisa, listopMode);
        }

        public ulong Incluir(DiarioOV diarioOv)
        {
            Validar(diarioOv);
			diarioOv.ch_diario = Guid.NewGuid().ToString("N");
            diarioOv.nr_ano = int.Parse(diarioOv.dt_assinatura.Split('/')[2]);
            GerarChaveDoDiario(diarioOv);
            diarioOv.st_novo = true;
            return _diarioAd.Incluir(diarioOv);
        }

        public void GerarChaveDoDiario(DiarioOV diarioOv)
        {
            StringBuilder chave = new StringBuilder();
            chave.Append(diarioOv.ch_tipo_fonte + "#");
            chave.Append(diarioOv.nr_ano + "#");
            chave.Append(diarioOv.nr_diario.ToString().PadLeft(8, '0') + "#");
            if(!string.IsNullOrEmpty(diarioOv.cr_diario)){
                chave.Append(diarioOv.cr_diario + "#");
            }
            chave.Append(diarioOv.ch_tipo_edicao);
            if (diarioOv.st_suplemento)
            {
                chave.Append("#suplemento");
                if (!string.IsNullOrEmpty(diarioOv.nm_diferencial_suplemento))
                {
                    chave.Append("#" + diarioOv.nm_diferencial_suplemento.ToLower().Replace(" ",""));
                }
            }
            if (!string.IsNullOrEmpty(diarioOv.secao_diario))
            {
                chave.Append("#" + diarioOv.secao_diario);
            }
            diarioOv.ch_para_nao_duplicacao = chave.ToString();
        }

        private void Validar(DiarioOV diarioOv)
        {
            if (string.IsNullOrEmpty(diarioOv.ch_tipo_fonte) || string.IsNullOrEmpty(diarioOv.nm_tipo_fonte))
            {
                throw new DocValidacaoException("Tipo de Diário não informado.");
            }
            if(diarioOv.arquivos == null || diarioOv.arquivos.Count <= 0){
                throw new DocValidacaoException("Não é permitido cadastrar diário sem arquivo.");
            }
        }

        public bool Atualizar(ulong id_doc, DiarioOV diarioOv)
        {
            Validar(diarioOv);
            diarioOv.nr_ano = int.Parse(diarioOv.dt_assinatura.Split('/')[2]);
            GerarChaveDoDiario(diarioOv);
            return _diarioAd.Atualizar(id_doc, diarioOv);
        }

        public bool AtualizarSemValidar(ulong id_doc, DiarioOV diarioOv)
        {
            diarioOv.nr_ano = int.Parse(diarioOv.dt_assinatura.Split('/')[2]);
            return _diarioAd.Atualizar(id_doc, diarioOv);
        }

        public bool Excluir(ulong id_doc)
        {
            return _diarioAd.Excluir(id_doc);
        }

        public List<DiarioOV> BuscarDiariosDoTipoDeEdicao(string ch_tipo_edicao)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("ch_tipo_edicao='{0}'", ch_tipo_edicao);
            return Consultar(query).results;
        }
		
		
        #region Arquivo

        public string AnexarArquivo(FileParameter fileParameter)
        {
            return _diarioAd.AnexarArquivo(fileParameter);
        }

        public string GetDoc(string id_file)
        {
            return _diarioAd.GetDoc(id_file);
        }

        public byte[] Download(string id_file)
        {
            return _diarioAd.Download(id_file);
        }

        #endregion

        #region ES
        public Result<DiarioOV> ConsultarEs(HttpContext context)
        {
            return _diarioAd.ConsultarEs(context);
        }

        public string PesquisarTotalEs(HttpContext context)
        {
            return _diarioAd.PesquisarTotalEs(context);
        }

        #endregion
    }
}