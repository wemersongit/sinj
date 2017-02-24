using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for NormaIncluir
    /// </summary>
    public class NormaIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            NormaOV normaOv = null;
            var action = AcoesDoUsuario.nor_inc;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var _ch_tipo_norma = context.Request["ch_tipo_norma"];
                var _nm_tipo_norma = context.Request["nm_tipo_norma"];

                var _st_acao = context.Request["in_g2"];

                var _nr_norma = context.Request["nr_norma"];

                var _cr_norma = context.Request["cr_norma"];

                var _nr_sequencial = context.Request["nr_sequencial"];
                var nr_sequencial = 0;

                var _dt_assinatura = context.Request["dt_assinatura"];
                var _id_ambito = context.Request["id_ambito"];
                var id_ambito = 0;

                var _orgao = context.Request.Form.GetValues("orgao");

                var _nm_apelido = context.Request["nm_apelido"];
                var _ds_ementa = context.Request["ds_ementa"];
                var _ds_observacao = context.Request["ds_observacao"];

                var _st_pendencia = context.Request["st_pendencia"];
                var st_pendencia = false;

                var _ds_pendencia = context.Request["ds_pendencia"];

                var _st_destaque = context.Request["st_destaque"];
                var st_destaque = false;

                var _autoria = context.Request.Form.GetValues("autoria");

                var _interessado = context.Request.Form.GetValues("interessado");

                var _nm_nome = context.Request.Form.GetValues("nm_nome");

                var _fonte = context.Request.Form.GetValues("fonte");

                //var _uuid_texto_atualizado = context.Request["uuid_texto_atualizado"];
                //var _mimetype_texto_atualizado = context.Request["mimetype_texto_atualizado"];
                //var _filesize_texto_atualizado = context.Request["filesize_texto_atualizado"];
                //var _id_file_texto_atualizado = context.Request["id_file_texto_atualizado"];
                //var _filename_texto_atualizado = context.Request["filename_texto_atualizado"];
                //var _opcao_texto_atualizado = context.Request["opcao_texto_atualizado"];

                var _indexacao = context.Request.Form.GetValues("indexacao");

                var _decisao = context.Request.Form.GetValues("decisao");

                var _ds_procedencia = context.Request["ds_procedencia"];
                var _ds_paramentro_constitucional = context.Request["ds_paramentro_constitucional"];

                var _requerente = context.Request.Form.GetValues("requerente");

                var _requerido = context.Request.Form.GetValues("requerido");

                var _procurador_responsavel = context.Request.Form.GetValues("procurador_responsavel");

                var _relator = context.Request.Form.GetValues("relator");

                //var _uuid_texto_acao = context.Request["uuid_texto_acao"];
                //var _mimetype_texto_acao = context.Request["mimetype_texto_acao"];
                //var _filesize_texto_acao = context.Request["filesize_texto_acao"];
                //var _id_file_texto_acao = context.Request["id_file_texto_acao"];
                //var _filename_texto_acao = context.Request["filename_texto_acao"];
                var _json_arquivo_texto_acao = context.Request["json_arquivo_texto_acao"];
                var _ds_efeito_decisao = context.Request["ds_efeito_decisao"];
                var _url_referencia = context.Request["url_referencia"];
                
                normaOv = new NormaOV();
                normaOv.ch_tipo_norma = _ch_tipo_norma;
                normaOv.nm_tipo_norma = _nm_tipo_norma;

                var in_acao = false;
                bool.TryParse(_st_acao, out in_acao);
                normaOv.st_acao = in_acao;
                normaOv.nr_norma = _nr_norma;
                normaOv.cr_norma = _cr_norma;
                if (int.TryParse(_nr_sequencial, out nr_sequencial))
                {
                    normaOv.nr_sequencial = nr_sequencial;
                }
                normaOv.dt_assinatura = _dt_assinatura;
                normaOv.dt_assinatura = _dt_assinatura;
                if (int.TryParse(_id_ambito, out id_ambito))
                {
                    normaOv.id_ambito = id_ambito;
                    normaOv.nm_ambito = new AmbitoRN().Doc(id_ambito).nm_ambito;
                }

                if (_orgao != null)
                {
                    for (var i = 0; i < _orgao.Length; i++)
                    {
                        normaOv.origens.Add(util.BRLight.JSON.Deserializa<Orgao>(_orgao[i]));
                    }
                }

                normaOv.nm_apelido = _nm_apelido;
                normaOv.ds_ementa = _ds_ementa;
                normaOv.ds_observacao = _ds_observacao;

                if (bool.TryParse(_st_pendencia, out st_pendencia))
                {
                    normaOv.st_pendencia = st_pendencia;
                    normaOv.ds_pendencia = _ds_pendencia;
                }

                if (bool.TryParse(_st_destaque, out st_destaque))
                {
                    normaOv.st_destaque = st_destaque;
                }
                if (_autoria != null)
                {
                    for (var i = 0; i < _autoria.Length; i++)
                    {
                        normaOv.autorias.Add(util.BRLight.JSON.Deserializa<Autoria>(_autoria[i]));
                    }
                }
                if (_interessado != null)
                {
                    for (var i = 0; i < _interessado.Length; i++)
                    {
                        var interessado_split = _interessado[i].Split('#');
                        normaOv.interessados.Add(new Interessado { ch_interessado = interessado_split[0], nm_interessado = interessado_split[1] });
                    }
                }
                if (_nm_nome != null)
                {
                    for (var i = 0; i < _nm_nome.Length; i++)
                    {
                        normaOv.nm_pessoa_fisica_e_juridica.Add(_nm_nome[i]);
                    }
                }
                if (_fonte != null)
                {
                    for (var i = 0; i < _fonte.Length; i++)
                    {
                        normaOv.fontes.Add(JSON.Deserializa<Fonte>(_fonte[i]));
                    }
                }
                if (_indexacao != null)
                {
                    Indexacao indexacao_da_norma;
                    for (var i = 0; i < _indexacao.Length; i++)
                    {
                        indexacao_da_norma = new Indexacao();
                        var indexacao_split = _indexacao[i].Split('|');

                        for (var k = 0; k < indexacao_split.Length; k++)
                        {
                            var termos_split = indexacao_split[k].Split('#');
                            indexacao_da_norma.vocabulario.Add(new Vocabulario { ch_termo = termos_split[0], ch_tipo_termo = termos_split[1], nm_termo = termos_split[2] });
                        }
                        if (indexacao_da_norma != null && indexacao_da_norma.vocabulario != null && indexacao_da_norma.vocabulario.Count > 0)
                        {
                            normaOv.indexacoes.Add(indexacao_da_norma);
                        }
                    }
                }
                if (_decisao != null)
                {
                    for (var i = 0; i < _decisao.Length; i++)
                    {
                        normaOv.decisoes.Add(JSON.Deserializa<Decisao>(_decisao[i]));
                    }
                }
                normaOv.ds_procedencia = _ds_procedencia;
                normaOv.ds_parametro_constitucional = _ds_paramentro_constitucional;

                if (_requerente != null)
                {
                    for (var i = 0; i < _requerente.Length; i++)
                    {
                        var requerente_split = _requerente[i].Split('#');
                        normaOv.requerentes.Add(new Requerente { ch_requerente = requerente_split[0], nm_requerente = requerente_split[1] });
                    }
                }
                if (_requerido != null)
                {
                    for (var i = 0; i < _requerido.Length; i++)
                    {
                        var requerido_split = _requerido[i].Split('#');
                        normaOv.requeridos.Add(new Requerido { ch_requerido = requerido_split[0], nm_requerido = requerido_split[1] });
                    }
                }
                if (_procurador_responsavel != null)
                {
                    for (var i = 0; i < _procurador_responsavel.Length; i++)
                    {
                        var procurador_split = _procurador_responsavel[i].Split('#');
                        normaOv.procuradores_responsaveis.Add(new Procurador { ch_procurador_responsavel = procurador_split[0], nm_procurador_responsavel = procurador_split[1] });
                    }
                }
                if (_relator != null)
                {
                    for (var i = 0; i < _relator.Length; i++)
                    {
                        var relator_split = _relator[i].Split('#');
                        normaOv.relatores.Add(new Relator { ch_relator = relator_split[0], nm_relator = relator_split[1] });
                    }
                }
                if (!string.IsNullOrEmpty(_json_arquivo_texto_acao))
                {
                    normaOv.ar_acao = JSON.Deserializa<ArquivoOV>(_json_arquivo_texto_acao);
                }
                normaOv.ds_efeito_decisao = _ds_efeito_decisao;
                normaOv.url_referencia = _url_referencia;
                normaOv.id_orgao_cadastrador = sessao_usuario.orgao_cadastrador.id_orgao_cadastrador;
                normaOv.nm_orgao_cadastrador = sessao_usuario.orgao_cadastrador.nm_orgao_cadastrador;
                

                normaOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
                normaOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                var id_doc = new NormaRN().Incluir(normaOv);
                if (id_doc > 0)
                {
                    sRetorno = "{\"id_doc_success\":" + id_doc + ",\"norma_st_acao\":"+normaOv.st_acao.ToString().ToLower()+", \"ch_norma\":\""+ normaOv.ch_norma +"\"}";
                }
                else
                {
                    throw new Exception("Erro ao cadastrar Norma.");
                }
                var log_incluir = new LogIncluir<NormaOV>
                {
                    registro = normaOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_incluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}