using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for ArquivoIncluir
    /// </summary>
    public class ArquivoIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{}";
            string _in_editar = context.Request["in_editar"];

            SessaoUsuarioOV sessao_usuario = null;
            var action = AcoesDoUsuario.arq_pro;

            try
            {
                sessao_usuario = Util.ValidarSessao();
                if (_in_editar == "1")
                {
                    sRetorno = AlterarArquivo(context, sessao_usuario, action);
                }
                else
                {
                    sRetorno = IncluirArquivo(context, sessao_usuario, action);
                }
            }
            catch (ParametroInvalidoException ex)
            {
                sRetorno = "{\"error_message\": \"" + ex.Message + "\" }";
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
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }

            context.Response.Write(sRetorno);
            context.Response.End();
        }

        private string IncluirArquivo(HttpContext context, SessaoUsuarioOV sessao_usuario, AcoesDoUsuario action)
        {

            string _nm_arquivo = context.Request["nm_arquivo"];
            string _ds_arquivo = context.Request["ds_arquivo"];
            string _json_file = context.Request["json_file"];
            string _arquivo_text = context.Request["arquivo"];
            string _ch_arquivo_raiz = context.Request["ch_arquivo_raiz"];
            string _ch_arquivo_superior = context.Request["ch_arquivo_superior"];
            string _nr_tipo_arquivo = context.Request["nr_tipo_arquivo"];
            int nr_tipo_arquivo = 0;
            int.TryParse(_nr_tipo_arquivo, out nr_tipo_arquivo);
            string _nr_nivel_arquivo_superior = context.Request["nr_nivel_arquivo_superior"];
            int nr_nivel_arquivo_superior = 0;
            int.TryParse(_nr_nivel_arquivo_superior, out nr_nivel_arquivo_superior);
            var arquivoOv = new SINJ_ArquivoOV();
            if (_ch_arquivo_raiz == "meus_arquivos")
            {
                _ch_arquivo_raiz = sessao_usuario.nm_login_usuario;
            }
            else if (_ch_arquivo_raiz == "arquivos_orgao_cadastrador")
            {
                _ch_arquivo_raiz = sessao_usuario.orgao_cadastrador.nm_orgao_cadastrador;
            }
            if (!string.IsNullOrEmpty(_ch_arquivo_superior))
            {
                arquivoOv.ch_arquivo_superior = _ch_arquivo_superior;
            }
            else if (!string.IsNullOrEmpty(_ch_arquivo_raiz))
            {
                arquivoOv.ch_arquivo_superior = _ch_arquivo_raiz;
            }
            arquivoOv.ch_arquivo = (!string.IsNullOrEmpty(arquivoOv.ch_arquivo_superior) ? arquivoOv.ch_arquivo_superior + "/" : "") + _nm_arquivo;
            arquivoOv.nm_arquivo = _nm_arquivo;
            arquivoOv.nr_tipo_arquivo = nr_tipo_arquivo;
            arquivoOv.nr_nivel_arquivo = nr_nivel_arquivo_superior + 1;
            arquivoOv.ds_arquivo = _ds_arquivo;

            arquivoOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
            arquivoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

            if (!string.IsNullOrEmpty(_json_file))
            {
                arquivoOv.ar_arquivo = JSON.Deserializa<neo.BRLightREST.ArquivoOV>(_json_file);
            }
            else if (nr_tipo_arquivo == 1)
            {
                var sArquivo = "";
                if (!string.IsNullOrEmpty(_arquivo_text))
                {
                    sArquivo = new Arquivo.UploadHtml().AnexarHtml(context);
                }
                else
                {
                    HttpPostedFile _file = context.Request.Files["file"];
                    var _nm_base = context.Request["nm_base"];
                    ///tem um gerenciador de imagens que salva imagens nesse caminho 'images/'
                    ///aqui valida se o ContentType do arquivo recebido começa com image/
                    ///caso contrário dispara exceção de validação.
                    if (arquivoOv.ch_arquivo_superior == "images")
                    {
                        if (_file.ContentType.ToLower().IndexOf("image/") != 0)
                        {
                            throw new DocValidacaoException("O arquivo inserido não é imagem.");
                        }
                    }
                    sArquivo = new Arquivo.UploadFile().AnexarArquivo(_nm_base, _file);
                }
                if (!string.IsNullOrEmpty(sArquivo))
                {
                    arquivoOv.ar_arquivo = JSON.Deserializa<ArquivoOV>(sArquivo);
                }
                else
                {
                    throw new ParametroInvalidoException("Erro ao receber Arquivo. Parâmetro arquivoOv incorreto.");
                }
            }
            var id_doc = new SINJ_ArquivoRN().Incluir(arquivoOv);
            if (id_doc <= 0)
            {
                throw new Exception("Erro ao salvar arquivo.");
            }
            arquivoOv._metadata.id_doc = id_doc;
            var log_incluir = new LogIncluir<SINJ_ArquivoOV>
            {
                registro = arquivoOv
            };
            LogOperacao.gravar_operacao(Util.GetEnumDescription(action)+".INC", log_incluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            return "{\"id_doc\": \"" + id_doc + "\", \"success_message\":\"Arquivo salvo com sucesso.\", \"arquivo\": " + JSON.Serialize<SINJ_ArquivoOV>(arquivoOv) + ", \"action\":\"INSERTED\"}";
        }

        private string AlterarArquivo(HttpContext context, SessaoUsuarioOV sessao_usuario, AcoesDoUsuario action)
        {
            string _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            id_doc = ulong.Parse(_id_doc);
            string _nm_arquivo = context.Request["nm_arquivo"];
            string _ds_arquivo = context.Request["ds_arquivo"];
            string _json_file = context.Request["json_file"];
            string _arquivo_text = context.Request["arquivo"];
            string _nr_tipo_arquivo = context.Request["nr_tipo_arquivo"];
            int nr_tipo_arquivo = 0;
            int.TryParse(_nr_tipo_arquivo, out nr_tipo_arquivo);

            var arquivoOv = new SINJ_ArquivoRN().Doc(id_doc);

            var ch_arquivo = (!string.IsNullOrEmpty(arquivoOv.ch_arquivo_superior) ? arquivoOv.ch_arquivo_superior + "/" : "") + _nm_arquivo;
            var chave_antiga = arquivoOv.ch_arquivo;
            var nome_antigo = arquivoOv.nm_arquivo;

            arquivoOv.ch_arquivo = ch_arquivo;
            arquivoOv.nm_arquivo = _nm_arquivo;
            arquivoOv.ds_arquivo = _ds_arquivo;

            arquivoOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
            

            if (!string.IsNullOrEmpty(_json_file))
            {
                arquivoOv.ar_arquivo = JSON.Deserializa<neo.BRLightREST.ArquivoOV>(_json_file);
            }
            else if (nr_tipo_arquivo == 1)
            {
                var sArquivo = "";
                if (!string.IsNullOrEmpty(_arquivo_text))
                {
                    sArquivo = new Arquivo.UploadHtml().AnexarHtml(context);
                }
                else
                {
                    HttpPostedFile _file = context.Request.Files["file"];
                    var _nm_base = context.Request["nm_base"];
                    sArquivo = new Arquivo.UploadFile().AnexarArquivo(_nm_base, _file);
                }
                if (!string.IsNullOrEmpty(sArquivo))
                {
                    arquivoOv.ar_arquivo = JSON.Deserializa<ArquivoOV>(sArquivo);
                }
                else
                {
                    throw new ParametroInvalidoException("Erro ao receber Arquivo. Parâmetro arquivoOv incorreto.");
                }
            }
            if (!new SINJ_ArquivoRN().Atualizar(id_doc, arquivoOv))
            {
                throw new Exception("Erro ao salvar arquivo.");
            }
            //Quando a chave for alterada (alterando o nome do diretorio ou arquivo)
            var aux_retorno_arquivos = "";
            if (ch_arquivo != chave_antiga && arquivoOv.nr_tipo_arquivo == 0)
            {
                var query = new Pesquisa();
                query.literal = "ch_arquivo like '" + chave_antiga + "/%'";
                query.limit = null;
                query.order_by.asc = new string[] { "ch_arquivo" };
                var oResult = new SINJ_ArquivoRN().Consultar(query);
                foreach (var result in oResult.results)
                {
                    var ch_arquivo_splited = result.ch_arquivo.Split('/');
                    var ch_arquivo_superior_splited = result.ch_arquivo_superior.Split('/');
                    var i = 0;
                    for (; i < ch_arquivo_splited.Length; i++)
                    {
                        if (ch_arquivo_splited[i] == nome_antigo)
                        {
                            ch_arquivo_splited[i] = arquivoOv.nm_arquivo;
                            result.ch_arquivo = string.Join("/", ch_arquivo_splited);
                            break;
                        }
                    }
                    for (i = 0; i < ch_arquivo_superior_splited.Length; i++)
                    {
                        if (ch_arquivo_superior_splited[i] == nome_antigo)
                        {
                            ch_arquivo_superior_splited[i] = arquivoOv.nm_arquivo;
                            result.ch_arquivo_superior = string.Join("/", ch_arquivo_superior_splited);
                            break;
                        }
                    }
                    if (!new SINJ_ArquivoRN().Atualizar(result._metadata.id_doc, result))
                    {
                        aux_retorno_arquivos += (aux_retorno_arquivos != "" ? "," : "") + "\"" + result.ch_arquivo + "\"";
                    }
                }
            }
            var log_atualizar = new LogAlterar<SINJ_ArquivoOV>
            {
                id_doc = id_doc,
                registro = arquivoOv
            };
            LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".EDT", log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            return "{\"id_doc\": \"" + id_doc + "\", \"success_message\":\"Arquivo alterado com sucesso.\", \"arquivos_erro_atualizacao\":[" + aux_retorno_arquivos + "], \"arquivo\": " + JSON.Serialize<SINJ_ArquivoOV>(arquivoOv) + ", \"action\":\"UPDATED\"}";
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
