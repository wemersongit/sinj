using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for NormaEditar
    /// </summary>
    public class NormaEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            NormaOV normaOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);

                    var _orgao = context.Request.Form.GetValues("orgao");


                    var _st_vacatio_legis = context.Request["st_vacatio_legis"];
                    var _ds_vacatio_legis = context.Request["ds_vacatio_legis"];
                    var _dt_inicio_vigencia = context.Request["dt_inicio_vigencia"];

                    var _nm_apelido = context.Request["nm_apelido"];
                    var _ds_ementa = context.Request["ds_ementa"];
                    var _ds_observacao = context.Request["ds_observacao"];
                    var _ds_pendencia = context.Request["ds_pendencia"];
                    var _st_destaque = context.Request["st_destaque"];
                    var st_destaque = false;
                    var _st_pendencia = context.Request["st_pendencia"];
                    var st_pendencia = false;

                    

                    var _st_habilita_email = "";
                    var st_habilita_email = false;

                    if (TCDF.Sinj.Util.UsuarioTemPermissao(TCDF.Sinj.Web.Sinj.oSessaoUsuario, TCDF.Sinj.AcoesDoUsuario.nor_eml))
                    {
                        _st_habilita_email = context.Request["st_habilita_email"];
                    }
                    else
                    {
                        _st_habilita_email = "false";
                    }

                    var _st_habilita_pesquisa = "";
                    var st_habilita_pesquisa = false;
                    if (TCDF.Sinj.Util.UsuarioTemPermissao(TCDF.Sinj.Web.Sinj.oSessaoUsuario, TCDF.Sinj.AcoesDoUsuario.nor_hsp))
                    {
                        _st_habilita_pesquisa = context.Request["st_habilita_pesquisa"];
                    }
                    else
                    {
                        _st_habilita_pesquisa = "false";
                    }




                    var _interessado = context.Request.Form.GetValues("interessado");
                    var _requerente = context.Request.Form.GetValues("requerente");
                    var _requerido = context.Request.Form.GetValues("requerido");
                    var _procurador_responsavel = context.Request.Form.GetValues("procurador_responsavel");
                    var _relator = context.Request.Form.GetValues("relator");
                    
                    var _nm_nome = context.Request.Form.GetValues("nm_nome");
                    var _fonte = context.Request.Form.GetValues("fonte");

                    //var _uuid_texto_atualizado = context.Request["uuid_texto_atualizado"];
                    //var _mimetype_texto_atualizado = context.Request["mimetype_texto_atualizado"];
                    //var _filesize_texto_atualizado = context.Request["filesize_texto_atualizado"];
                    //var _id_file_texto_atualizado = context.Request["id_file_texto_atualizado"];
                    //var _filename_texto_atualizado = context.Request["filename_texto_atualizado"];
                    var _json_arquivo_texto_atualizado = context.Request["json_arquivo_texto_atualizado"];
                    var _opcao_texto_atualizado = context.Request["opcao_texto_atualizado"];

                    //var _uuid_texto_acao = context.Request["uuid_texto_acao"];
                    //var _mimetype_texto_acao = context.Request["mimetype_texto_acao"];
                    //var _filesize_texto_acao = context.Request["filesize_texto_acao"];
                    //var _id_file_texto_acao = context.Request["id_file_texto_acao"];
                    //var _filename_texto_acao = context.Request["filename_texto_acao"];
                    var _json_arquivo_texto_acao = context.Request["json_arquivo_texto_acao"];
                    var _ds_efeito_decisao = context.Request["ds_efeito_decisao"];
                    var _url_referencia = context.Request["url_referencia"];

                    var _url_projeto_lei= context.Request["url_projeto_lei"];
                    var _nr_projeto_lei = context.Request["nr_projeto_lei"];

                    var _st_situacao_forcada = context.Request["st_situacao_forcada"];
                    var _ch_situacao = context.Request["ch_situacao"];
                    var _nm_situacao = context.Request["nm_situacao"];

                    var _indexacao = context.Request.Form.GetValues("indexacao");
                    var _decisao = context.Request.Form.GetValues("decisao");
                    
                    var _autoria = context.Request.Form.GetValues("autoria"); 

                      NormaRN normaRn = new NormaRN();
                    normaOv = normaRn.Doc(id_doc);

                    var podeEditar = false;
                    var nm_tipo_norma = "";

                    if (sessao_usuario.ch_perfil == "super_administrador")
                    {
                        podeEditar = true;
                    }
                    else
                    {
                        var tipoDeNormaOv = new TipoDeNormaRN().Doc(normaOv.ch_tipo_norma);
                        nm_tipo_norma = tipoDeNormaOv.nm_tipo_norma;
                        foreach (var orgaoCadastrador in tipoDeNormaOv.orgaos_cadastradores)
                        {
                            if (sessao_usuario.orgao_cadastrador.id_orgao_cadastrador == orgaoCadastrador.id_orgao_cadastrador)
                            {
                                podeEditar = true;
                                break;
                            }
                        }
                    }
                    if (!podeEditar)
                    {
                        throw new PermissionException("Usuário não tem permissão para editar " + (nm_tipo_norma != "" ? nm_tipo_norma : "esse tipo de ato") + ".");
                    }

                    normaOv.st_vacatio_legis = _st_vacatio_legis == "1";
                    normaOv.dt_inicio_vigencia = null;
                    normaOv.ds_vacatio_legis = null;
                    if (normaOv.st_vacatio_legis)
                    {
                        normaOv.dt_inicio_vigencia = _dt_inicio_vigencia;
                        normaOv.ds_vacatio_legis = _ds_vacatio_legis;
                    }


                    normaOv.origens = new List<Orgao>();
                    if (_orgao != null)
                    {
                        for (var i = 0; i < _orgao.Length; i++)
                        {
                            normaOv.origens.Add(util.BRLight.JSON.Deserializa<Orgao>(_orgao[i]));
                        }
                    }
                                        
                    normaOv.nm_pessoa_fisica_e_juridica = new List<String>();
                    if (_nm_nome != null)
                    {
                        for (var i = 0; i < _nm_nome.Length; i++)
                        {
                            normaOv.nm_pessoa_fisica_e_juridica.Add(_nm_nome[i]);
                        }
                    }

                    normaOv.autorias = new List<Autoria>();
                    if (_autoria != null)
                    {
                        for (var i = 0; i < _autoria.Length; i++)
                        {
                            normaOv.autorias.Add(util.BRLight.JSON.Deserializa<Autoria>(_autoria[i]));
                        }
                    }

                    normaOv.interessados = new List<Interessado>();
                    if (_interessado != null)
                    {
                        for (var i = 0; i < _interessado.Length; i++)
                        {
                            var interessado_split = _interessado[i].Split('#');
                            normaOv.interessados.Add(new Interessado() { ch_interessado = interessado_split[0], nm_interessado = interessado_split[1] });
                        }
                    }

                    normaOv.requerentes = new List<Requerente>();
                    if (_requerente != null)
                    {
                        for (var i = 0; i < _requerente.Length; i++)
                        {
                            var requerente_split = _requerente[i].Split('#');
                            normaOv.requerentes.Add(new Requerente() { ch_requerente = requerente_split[0], nm_requerente = requerente_split[1] });
                        }
                    }
                    normaOv.requeridos = new List<Requerido>();
                    if (_requerido != null)
                    {
                        for (var i = 0; i < _requerido.Length; i++)
                        {
                            var requerido_split = _requerido[i].Split('#');
                            normaOv.requeridos.Add(new Requerido() { ch_requerido = requerido_split[0], nm_requerido = requerido_split[1] });
                        }
                    }
                    normaOv.procuradores_responsaveis = new List<Procurador>();
                    if (_procurador_responsavel != null)
                    {
                        for (var i = 0; i < _procurador_responsavel.Length; i++)
                        {
                            var procuradores_split = _procurador_responsavel[i].Split('#');
                            normaOv.procuradores_responsaveis.Add(new Procurador() { ch_procurador_responsavel = procuradores_split[0], nm_procurador_responsavel = procuradores_split[1] });
                        }
                    }
                    normaOv.relatores = new List<Relator>();
                    if (_relator != null)
                    {
                        for (var i = 0; i < _relator.Length; i++)
                        {
                            var relator_split = _relator[i].Split('#');
                            normaOv.relatores.Add(new Relator() { ch_relator = relator_split[0], nm_relator = relator_split[1] });
                        }
                    }

                    normaOv.url_referencia = "";
                    if (_url_referencia != null)
                    {
                        normaOv.url_referencia = _url_referencia;
                    }

                    normaOv.url_projeto_lei= "";
                    if (_url_projeto_lei != null)
                    {
                        normaOv.url_projeto_lei = _url_projeto_lei;
                    }

                    normaOv.nr_projeto_lei = "";
                    if (_nr_projeto_lei != null)
                    {
                        normaOv.nr_projeto_lei = _nr_projeto_lei;
                    }

                    normaOv.ds_efeito_decisao = "";
                    if (_ds_efeito_decisao != null)
                    {
                        normaOv.ds_efeito_decisao = _ds_efeito_decisao;
                    }


                    normaOv.nm_apelido = _nm_apelido;
                    normaOv.ds_ementa = _ds_ementa;
                    normaOv.ds_observacao = _ds_observacao;
                    normaOv.ds_pendencia = _ds_pendencia;

                    if(!string.IsNullOrEmpty(_st_pendencia) && bool.TryParse(_st_pendencia, out st_pendencia))
                    {
                        normaOv.st_pendencia = st_pendencia;
                    }
                    else 
                    {
                        normaOv.st_pendencia = st_pendencia;
                    }

                    if (!string.IsNullOrEmpty(_st_habilita_pesquisa) && bool.TryParse(_st_habilita_pesquisa, out st_habilita_pesquisa))
                    {
                        normaOv.st_habilita_pesquisa = st_habilita_pesquisa;
                    }
                    else
                    {
                        normaOv.st_habilita_pesquisa = st_habilita_pesquisa;
                    }

                    if (!string.IsNullOrEmpty(_st_habilita_email) && bool.TryParse(_st_habilita_email, out st_habilita_email))
                    {
                        normaOv.st_habilita_email = st_habilita_email;
                    }
                    else
                    {
                        normaOv.st_habilita_email= st_habilita_email;
                    }

                    if (!string.IsNullOrEmpty(_st_destaque) && bool.TryParse(_st_destaque, out st_destaque))
                    {
                        normaOv.st_destaque = st_destaque;
                    }
                    else 
                    {
                        normaOv.st_destaque = st_destaque;
                    }
                    normaOv.fontes = new List<Fonte>();
                    if (_fonte != null)
                    {
                        for (var i = 0; i < _fonte.Length; i++)
                        {
                            normaOv.fontes.Add(JSON.Deserializa<Fonte>(_fonte[i]));
                        }
                    }
                    

                    normaOv.ar_atualizado = new ArquivoOV();
                    if (!string.IsNullOrEmpty(_json_arquivo_texto_atualizado))
                    {
                        normaOv.ar_atualizado = JSON.Deserializa<ArquivoOV>(_json_arquivo_texto_atualizado);
                    }

                    normaOv.ar_acao = new ArquivoOV();
                    if (!string.IsNullOrEmpty(_json_arquivo_texto_acao))
                    {
                        normaOv.ar_acao = JSON.Deserializa<ArquivoOV>(_json_arquivo_texto_acao);
                    }

                    normaOv.indexacoes = new List<Indexacao> ();
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

                    normaOv.decisoes = new List<Decisao>();
                    if (_decisao != null)
                    {
                        for (var i = 0; i < _decisao.Length; i++)
                        {
                            normaOv.decisoes.Add(JSON.Deserializa<Decisao>(_decisao[i]));
                        }
                    }

                    //var situacao = normaRn.ObterSituacao(normaOv.vides);
                    //normaOv.ch_situacao = situacao.ch_situacao;
                    //normaOv.nm_situacao = situacao.nm_situacao;

                    normaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });

                    if (_st_situacao_forcada == "1")
                    {
                        normaOv.st_situacao_forcada = true;
                        normaOv.ch_situacao = _ch_situacao;
                        normaOv.nm_situacao = _nm_situacao;
                    }
                    else
                    {
                        if (normaOv.st_situacao_forcada)
                        {
                            var situacao = normaRn.ObterSituacao(normaOv.vides);
                            normaOv.ch_situacao = situacao.ch_situacao;
                            normaOv.nm_situacao = situacao.nm_situacao;
                        }
                        normaOv.st_situacao_forcada = false;
                    }

                    if (normaRn.Atualizar(id_doc, normaOv))
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"update\":true, \"ch_norma\":\""+normaOv.ch_norma+"\"}";
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
                var log_atualizar = new LogAlterar<NormaOV>
                {
                    id_doc = id_doc,
                    registro = normaOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
