using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.Log;
using TCDF.Sinj.ES;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for DiarioEditar
    /// </summary>
    public class DiarioEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            DiarioOV diarioOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.dio_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var _nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                    var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    var _ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                    var _nm_tipo_edicao = context.Request["nm_tipo_edicao"];
                    var _nm_diferencial_edicao = context.Request["nm_diferencial_edicao"];
                       var _nr_diario = context.Request["nr_diario"];
                    int nr_diario = 0;
                    if (!int.TryParse(_nr_diario, out nr_diario))
                    {
                        throw new DocValidacaoException("Número inválido.");
                    }
                    var _cr_diario = context.Request["cr_diario"];
                    var _secoes_diario = context.Request.Form.GetValues("secao_diario");
                    var _dt_assinatura = context.Request["dt_assinatura"];

                    var _st_suplemento = context.Request["st_suplemento"];
                    var st_suplemento = false;
                    var _nm_diferencial_suplemento = context.Request["nm_diferencial_suplemento"];

                    var _st_pendente = context.Request["st_pendente"];
                    var st_pendente = false;
                    var _ds_pendencia = context.Request["ds_pendencia"];

                    var _uuid_arquivo_diario = context.Request["uuid_arquivo_diario"];
                    var _mimetype_arquivo_diario = context.Request["mimetype_arquivo_diario"];
                    var _filesize_arquivo_diario = context.Request["filesize_arquivo_diario"];
                    var _id_file_arquivo_diario = context.Request["id_file_arquivo_diario"];
                    var _filename_arquivo_diario = context.Request["filename_arquivo_diario"];
                    var _nr_arquivos = context.Request.Form.GetValues("nr_arquivos");

                    DiarioRN diarioRn = new DiarioRN();
                    diarioOv = diarioRn.Doc(id_doc);
                    Util.rejeitarInject(_ch_tipo_fonte);
                    diarioOv.ch_tipo_fonte = _ch_tipo_fonte;
                    diarioOv.nm_tipo_fonte = _nm_tipo_fonte;
                    diarioOv.ch_tipo_edicao = _ch_tipo_edicao;
                    diarioOv.nm_tipo_edicao = _nm_tipo_edicao;
                    diarioOv.nm_diferencial_edicao = _nm_diferencial_edicao;
                    diarioOv.nr_diario = nr_diario;
                    diarioOv.cr_diario = _cr_diario;
                    diarioOv.secao_diario = "";
                    if (_secoes_diario != null)
                    {
                        for (var i = 0; i < _secoes_diario.Length; i++)
                        {
                            diarioOv.secao_diario += (diarioOv.secao_diario != "" ? (i < (_secoes_diario.Length - 1) ? ", " : " e ") : "") + _secoes_diario[i];
                        }
                    }
                    diarioOv.dt_assinatura = _dt_assinatura;

                    bool.TryParse(_st_suplemento, out st_suplemento);
                    diarioOv.st_suplemento = st_suplemento;
                    if (st_suplemento)
                    {
                        diarioOv.nm_diferencial_suplemento = _nm_diferencial_suplemento;
                    }
                    else
                    {
                        diarioOv.nm_diferencial_suplemento = "";
                    }

                    bool.TryParse(_st_pendente, out st_pendente);
                    diarioOv.st_pendente = st_pendente;
                    if(st_pendente)
                    {
                        diarioOv.ds_pendencia = _ds_pendencia;
                    }
                    else
                    {
                        diarioOv.ds_pendencia = "";
                    }

                    //if (!string.IsNullOrEmpty(_uuid_arquivo_diario))
                    //{
                    //    diarioOv.ar_diario = new neo.BRLightREST.ArquivoOV { uuid = _uuid_arquivo_diario, mimetype = _mimetype_arquivo_diario, filesize = ulong.Parse(_filesize_arquivo_diario), id_file = _id_file_arquivo_diario, filename = _filename_arquivo_diario };
                    //}

                    diarioOv.ar_diario = new neo.BRLightREST.ArquivoOV();

                    List<ArquivoDiario> old_arq = diarioOv.arquivos;
                    List<ArquivoDiario> new_arq = new List<ArquivoDiario>();
                    diarioOv.arquivos = new List<ArquivoDiario>();

                   

                    if (_nr_arquivos != null)
                    {
                        var sArquivo = "";
                        var ds_arquivo = "";
                        foreach (var nr_arquivo in _nr_arquivos)
                        {
                            sArquivo = context.Request["json_arquivo_diario_" + nr_arquivo];
                            ds_arquivo = context.Request["ds_arquivo_diario_" + nr_arquivo];
                            ArquivoDiario arqDiario = new ArquivoDiario
                            {
                                arquivo_diario = JSON.Deserializa<neo.BRLightREST.ArquivoOV>(sArquivo),
                                ds_arquivo = ds_arquivo
                            };
                            diarioOv.arquivos.Add(arqDiario);
                            //ArquivoDiario arqDel = new ArquivoDiario();
                            bool add_diff_item = true;

                            var arqDel = from item in old_arq
                                     where arqDiario.arquivo_diario.id_file == item.arquivo_diario.id_file
                                        select item ;

                            List<ArquivoDiario> listarqDel = arqDel.ToList<ArquivoDiario>();
                            if (listarqDel.Count > 0)
                            {
                                old_arq.Remove(listarqDel[0]);
                            }
                            else
                            {
                                new_arq.Add(arqDiario);
                            }
                            //foreach (var arq in old_arq)
                            //{
                            //    if (arq.arquivo_diario.id_file == arqDiario.arquivo_diario.id_file)
                            //    {
                            //        add_diff_item = false;
                            //        arqDel = arq;
                            //        break;
                            //    }
                            //}
                            //if (add_diff_item)
                            //{
                            //    new_arq.Add(arqDiario);
                            //}
                            //else
                            //{
                            //    old_arq.Remove(arqDel);
                            //}
                        }
                    }

                    if (old_arq.Count > 0 && new_arq.Count > 0)
                    {
                        List<opMode<object>> operations = new List<opMode<object>>();
                        String esQueryNorma = "{\"query\":{\"query_string\":{\"fields\":[],\"query\":\"fontes.ar_diario.id_file:(";
            
                        for (int i = 0; i < old_arq.Count; i++)
                        {
                            if (i > 0)
                            {
                                esQueryNorma += " OR ";
                            }
                            esQueryNorma += old_arq[i].arquivo_diario.id_file;
                            if (new_arq.Count - 1 >= i)
                            {
                                opMode<object> op = new opMode<object>();
                                op.path = "fontes/*/ar_diario";
                                op.fn = "attr_equals";
                                op.mode = "update";
                                op.args = new object[] { "id_file", old_arq[i].arquivo_diario.id_file, new  
                                {
                                    mimetype = new_arq[i].arquivo_diario.mimetype,
                                    filesize = new_arq[i].arquivo_diario.filesize,
                                    id_file = new_arq[i].arquivo_diario.id_file,
                                    uuid = new_arq[i].arquivo_diario.uuid,
                                    filename = new_arq[i].arquivo_diario.filename
                                }};
                                operations.Add(op);
                            }
                        }

                        esQueryNorma += ")\",\"default_operator\":\"AND\"}},\"sort\":[{\"_score\":{\"order\":\"desc\"}}],\"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"fontes.ar_diario.id_file\"]}}}";
                        var url_es = new DocEs().GetUrlEs("sinj_norma") + "/_search?from=0&size=20";

                        DocEs _docEs = new DocEs();
                        Result<NormaOV> nov = _docEs.Pesquisar<NormaOV>(esQueryNorma, url_es);

                        if (nov.hits.hits.Count > 0) 
                        {
                            string query_update_norma = "id_doc in(";
                            bool addComma = false;
                            foreach (var doc in nov.hits.hits)
                            {
                                if (addComma)
                                {
                                    query_update_norma += ", ";
                                }
                                query_update_norma += doc._id;
                                addComma = true;
                            }
                            query_update_norma += ")";
                            
                            Pesquisa pesquisa = new Pesquisa();
                            pesquisa.literal = query_update_norma;
                            pesquisa.limit = old_arq.Count.ToString();

                            NormaRN normaRN = new NormaRN();
                            string retorno = normaRN.PathPut(pesquisa, operations);

                        }
                    }


                    diarioOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (diarioRn.Atualizar(id_doc, diarioOv))
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"update\":true}";
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar registro. id_doc:" + id_doc);
                    }
                }
                else
                {
                    throw new Exception("Erro ao atualizar registro. id_doc:" + _id_doc);
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            context.Response.Write(sRetorno);
            var log_atualizar = new LogAlterar<DiarioOV>
            {
                id_doc = id_doc,
                registro = diarioOv
            };
            LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
