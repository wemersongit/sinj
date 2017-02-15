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
    /// Summary description for OrgaoEditar
    /// </summary>
    public class OrgaoEditar : IHttpHandler
    {

        // ToDo:
        // Verificar regras de validaçao com respeito a data de fim de vigencia.
        private bool InativarOrgaosFilho(OrgaoOV orgaoPai, string dt_fim_vigencia)
        {
            try
            {
                var orgaoRn = new OrgaoRN();
                if (orgaoPai.st_orgao == false && !string.IsNullOrEmpty(dt_fim_vigencia))
                {
                    var results_orgaos_filho = orgaoRn.Consultar(new Pesquisa { literal = "ch_orgao_pai ='" + orgaoPai.ch_orgao + "'" });
                    foreach (var filho in results_orgaos_filho.results)
                    {
                        var alteracao = false;
                        if (filho.st_orgao != false)
                        {
                            filho.st_orgao = false;
                            alteracao = true;
                        }
                        if (string.IsNullOrEmpty(filho.dt_fim_vigencia))
                        {
                            filho.dt_fim_vigencia = orgaoPai.dt_fim_vigencia;
                            alteracao = true;
                        }
                        if (alteracao == true)
                        {
                            try
                            {
                                orgaoRn.Atualizar(filho._metadata.id_doc, filho);
                                InativarOrgaosFilho(filho, filho.dt_fim_vigencia);
                            }
                            catch (Exception ex) 
                            {
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            OrgaoOV orgaoOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.org_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var b_atualizar_normas = "0";
                    var _nm_orgao = context.Request["nm_orgao"];
                    var _sg_orgao = context.Request["sg_orgao"];
                    var _id_ambito = context.Request["id_ambito"];
                    var iId_ambito = 0;
                    if (!string.IsNullOrEmpty(_id_ambito))
                    {
                        iId_ambito = int.Parse(_id_ambito);
                    }
                    var _st_orgao = context.Request["st_orgao"];
                    var _orgaos_cadastradores = context.Request["orgao_cadastrador"];
                    var _dt_inicio_vigencia = context.Request["dt_inicio_vigencia"];
                    var _dt_fim_vigencia = context.Request["dt_fim_vigencia"];

                    var _ch_orgao_pai = context.Request["ch_orgao_pai"];
                    var _sOrgaos_anteriores = context.Request["orgao_anterior"];
                    var _orgao_filho = context.Request.Form.GetValues("orgao_filho");
                    var _norma_inicio_vigencia = context.Request["norma_inicio_vigencia"];
                    var _norma_fim_vigencia = context.Request["norma_fim_vigencia"];

                    var orgaoRn = new OrgaoRN();
                    orgaoOv = orgaoRn.Doc(id_doc);
                    if (orgaoOv.nm_orgao != _nm_orgao || orgaoOv.sg_orgao != _sg_orgao)
                    {
                        b_atualizar_normas = "1";
                    }
                    orgaoOv.nm_orgao = _nm_orgao;
                    orgaoOv.sg_orgao = _sg_orgao;
                    var ambito = new AmbitoRN().Doc(iId_ambito);
                    orgaoOv.ambito.id_ambito = ambito.id_ambito;
                    orgaoOv.ambito.nm_ambito = ambito.nm_ambito;
                    if (!string.IsNullOrEmpty(_orgaos_cadastradores))
                    {
                        orgaoOv.orgaos_cadastradores = new List<OrgaoCadastrador>();
                        foreach (var _orgao_cadastrador in _orgaos_cadastradores.Split(','))
                        {
                            var orgao_cadastrador = new OrgaoCadastradorRN().Doc(int.Parse(_orgao_cadastrador));
                            orgaoOv.orgaos_cadastradores.Add(new OrgaoCadastrador { id_orgao_cadastrador = orgao_cadastrador.id_orgao_cadastrador, nm_orgao_cadastrador = orgao_cadastrador.nm_orgao_cadastrador });
                        }
                    }
                    orgaoOv.st_orgao = bool.Parse(_st_orgao);
                    orgaoOv.dt_inicio_vigencia = _dt_inicio_vigencia;
                    if (!string.IsNullOrEmpty(_dt_fim_vigencia))
                    {
                        orgaoOv.dt_fim_vigencia = (Convert.ToDateTime(_dt_fim_vigencia)).ToString("dd'/'MM'/'yyyy");
                    }
                    else 
                    {
                        orgaoOv.dt_fim_vigencia = null;
                    }

                    // Função recursiva que inativa todos os descendentes caso esse orgao seja inativado.
                    var inativacaoOrgaosFilho = InativarOrgaosFilho(orgaoOv, orgaoOv.dt_fim_vigencia);

                    ///Note: Cria chave e sigla hierarquica 
                    if (!string.IsNullOrEmpty(_ch_orgao_pai))
                    {
                        var orgao_pai = orgaoRn.Doc(_ch_orgao_pai);
                        if (orgaoOv.ch_orgao_pai != orgao_pai.ch_orgao)
                        {
                            b_atualizar_normas = "1";
                        }
                        if (orgao_pai != null)
                        {
                            orgaoOv.ch_orgao_pai = orgao_pai.ch_orgao;
                            orgaoOv.ch_hierarquia = orgao_pai.ch_hierarquia + "." + orgaoOv.ch_orgao;
                            orgaoOv.sg_hierarquia = orgao_pai.sg_hierarquia + " > " + orgaoOv.sg_orgao;
                            orgaoOv.nm_hierarquia = orgao_pai.nm_hierarquia + " > " + orgaoOv.nm_orgao;
                        }
                        else
                        {
                            orgaoOv.ch_orgao_pai = "";
                            orgaoOv.ch_hierarquia = orgaoOv.ch_orgao;
                            orgaoOv.sg_hierarquia = orgaoOv.sg_orgao;
                            orgaoOv.nm_hierarquia = orgaoOv.nm_orgao;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(orgaoOv.ch_orgao_pai))
                        {
                            b_atualizar_normas = "1";
                        }
                        orgaoOv.ch_orgao_pai = "";
                        orgaoOv.ch_hierarquia = orgaoOv.ch_orgao;
                        orgaoOv.sg_hierarquia = orgaoOv.sg_orgao;
                        orgaoOv.nm_hierarquia = orgaoOv.nm_orgao;
                    }

                   
					var lista_chaves_anteriores = new List<string>();

                    orgaoOv.ch_cronologia = new List<string>();
                    orgaoOv.ch_orgao_anterior =  new string[0];

					///Note: Cria chave e sigla cronologica
                    if (!string.IsNullOrEmpty(_sOrgaos_anteriores))
                    {
                        var orgaos_anteriores = _sOrgaos_anteriores.Split(',');
                        orgaoOv.ch_orgao_anterior = new string[orgaos_anteriores.Length];
                        var split = new string[0];
                        var chave = "";
                        var dt_inicio_vigencia = "";
                        var dt_fim_vigencia = "";
                        for (var i = 0; i < orgaos_anteriores.Length; i++)
                        {
                            split = orgaos_anteriores[i].Split('#');
                            chave = split[0];
                            orgaoOv.ch_orgao_anterior[i] = chave;
							lista_chaves_anteriores.Add(chave);
							var orgaoAnteriorOv = orgaoRn.Doc(chave);

							if (split.Length > 1){
								dt_inicio_vigencia = split[1];
								dt_fim_vigencia = split[2];
								var bAtualizar = false;
								if (!string.IsNullOrEmpty(dt_inicio_vigencia))
								{
									orgaoAnteriorOv.dt_inicio_vigencia = dt_inicio_vigencia;
									bAtualizar = true;
								}
								if (!string.IsNullOrEmpty(dt_fim_vigencia))
								{
									orgaoAnteriorOv.dt_fim_vigencia = dt_fim_vigencia;
									bAtualizar = true;
								}
								if (orgaoAnteriorOv.st_orgao)
								{
									orgaoAnteriorOv.st_orgao = false;
									bAtualizar = true;
								}
								if (bAtualizar)
								{
                                    orgaoAnteriorOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
									orgaoRn.Atualizar(orgaoAnteriorOv._metadata.id_doc, orgaoAnteriorOv);
								}
							}

                            foreach (var chave_cronologica in orgaoAnteriorOv.ch_cronologia)
                            {
                                orgaoOv.ch_cronologia.Add(chave_cronologica + "." + orgaoOv.ch_orgao);
                            }
                        }
                    }
                    else
                    {
                        orgaoOv.ch_cronologia.Add(orgaoOv.ch_orgao);
                    }

                    orgaoOv.ch_norma_inicio_vigencia = "";
                    orgaoOv.ds_norma_inicio_vigencia = "";
                    if (!string.IsNullOrEmpty(_norma_inicio_vigencia))
                    {
                        var norma_inicio_vigencia = _norma_inicio_vigencia.Split('#');
                        orgaoOv.ch_norma_inicio_vigencia = norma_inicio_vigencia[0];
                        orgaoOv.ds_norma_inicio_vigencia = norma_inicio_vigencia[1];
                    }

                    orgaoOv.ch_norma_fim_vigencia = "";
                    orgaoOv.ds_norma_fim_vigencia = "";
                    if (!string.IsNullOrEmpty(_norma_fim_vigencia))
                    {
                        var norma_fim_vigencia = _norma_fim_vigencia.Split('#');
                        orgaoOv.ch_norma_fim_vigencia = norma_fim_vigencia[0];
                        orgaoOv.ds_norma_fim_vigencia = norma_fim_vigencia[1];
                    }

					var filhos_dt_fim_vigencia = context.Request.Form.GetValues("orgao_filho_dt_fim_vigencia");
					var dt_fim_vigencia_index = 0;
					var lista_chaves_filhos = new List<string>();
					var lista_filhos_errados = new List<string>();
					var dt_fim_vigencia_rollback = "";
					var st_orgao_rollback = false;
					if (_orgao_filho != null && _orgao_filho.Length > 0)
					{
						OrgaoOV orgaoFilhoAnteriorOv;
						OrgaoOV orgaoFilhoNovoOv;
						foreach (var _ch_orgao in _orgao_filho)
						{
							orgaoFilhoAnteriorOv = new OrgaoOV();
							orgaoFilhoNovoOv = new OrgaoOV();
							try{
								orgaoFilhoAnteriorOv = orgaoRn.Doc(_ch_orgao);
								orgaoFilhoNovoOv.ambito = orgaoFilhoAnteriorOv.ambito;
								orgaoFilhoNovoOv.ch_cronologia = orgaoFilhoAnteriorOv.ch_cronologia;
								orgaoFilhoNovoOv.sg_orgao = orgaoFilhoAnteriorOv.sg_orgao;
								orgaoFilhoNovoOv.st_autoridade = false;
								orgaoFilhoNovoOv.st_orgao = true;
								orgaoFilhoNovoOv.dt_cadastro = orgaoOv.dt_cadastro;
								orgaoFilhoNovoOv.ds_nota_de_escopo = orgaoFilhoAnteriorOv.ds_nota_de_escopo;
								orgaoFilhoNovoOv.dt_inicio_vigencia = orgaoOv.dt_inicio_vigencia;
								orgaoFilhoNovoOv.ch_orgao_anterior = new string[1];
								orgaoFilhoNovoOv.ch_orgao_anterior[0] = _ch_orgao;
								orgaoFilhoNovoOv.dt_inicio_vigencia = orgaoOv.dt_inicio_vigencia;
								orgaoFilhoNovoOv.nm_orgao = orgaoFilhoAnteriorOv.nm_orgao;
								orgaoFilhoNovoOv.nm_login_usuario_cadastro = orgaoOv.nm_login_usuario_cadastro;

								orgaoFilhoNovoOv.orgaos_cadastradores = orgaoOv.orgaos_cadastradores;

								st_orgao_rollback = orgaoFilhoAnteriorOv.st_orgao;
								dt_fim_vigencia_rollback = orgaoFilhoAnteriorOv.dt_fim_vigencia;
								orgaoFilhoAnteriorOv.st_orgao = false;
								orgaoFilhoAnteriorOv.dt_fim_vigencia = filhos_dt_fim_vigencia[dt_fim_vigencia_index];
								dt_fim_vigencia_index += 1;
								orgaoFilhoNovoOv.dt_inicio_vigencia = orgaoFilhoAnteriorOv.dt_fim_vigencia;

								orgaoFilhoNovoOv.ch_orgao = Guid.NewGuid().ToString("N");

								lista_chaves_filhos.Add(orgaoFilhoAnteriorOv.ch_orgao);
								lista_chaves_filhos.Add(orgaoFilhoNovoOv.ch_orgao);

								//						Isso eh caso o filho sendo cadastrado eh um filho imediato do orgao
								//						que esta sendo preenchido no cadastro (orgaoOv).
								if (lista_chaves_anteriores.Contains(orgaoFilhoAnteriorOv.ch_orgao_pai)){
									orgaoFilhoNovoOv.ch_orgao_pai = orgaoOv.ch_orgao;
									orgaoFilhoNovoOv.ch_hierarquia = orgaoOv.ch_hierarquia + '.' + orgaoFilhoNovoOv.ch_orgao;
									lista_chaves_filhos.Add(orgaoFilhoNovoOv.ch_hierarquia);
									orgaoFilhoNovoOv.sg_hierarquia = orgaoOv.sg_hierarquia + '>' + orgaoFilhoNovoOv.sg_orgao;
									lista_chaves_filhos.Add(orgaoFilhoNovoOv.sg_hierarquia);
                                    orgaoFilhoNovoOv.nm_hierarquia = orgaoOv.nm_hierarquia + " > " + orgaoFilhoNovoOv.nm_orgao;
                                    lista_chaves_filhos.Add(orgaoFilhoNovoOv.nm_hierarquia);
								}
								//						Se nao for um filho imediato, ele precisa receber valores 
								//						do orgao que eh o seu pai.
								//						Esses valores estarao la lista_chaves_filhos
								else{
									string[] array_chaves_filhos = lista_chaves_filhos.ToArray();
									int index_ch_pai = Array.IndexOf(array_chaves_filhos, orgaoFilhoAnteriorOv.ch_orgao_pai);
									var ch_pai_novo = array_chaves_filhos[index_ch_pai+1];
									orgaoFilhoNovoOv.ch_orgao_pai = ch_pai_novo;
									var ch_hierarquia_pai = array_chaves_filhos[index_ch_pai+2];
									orgaoFilhoNovoOv.ch_hierarquia = ch_hierarquia_pai + '.' + orgaoFilhoNovoOv.ch_orgao;
                                    lista_chaves_filhos.Add(orgaoFilhoNovoOv.ch_hierarquia);
                                    var sg_hierarquia_pai = array_chaves_filhos[index_ch_pai + 3];
                                    orgaoFilhoNovoOv.sg_hierarquia = sg_hierarquia_pai + ">" + orgaoFilhoNovoOv.sg_orgao;
									lista_chaves_filhos.Add(orgaoFilhoNovoOv.sg_hierarquia);
                                    var nm_hierarquia_pai = array_chaves_filhos[index_ch_pai + 4];
                                    orgaoFilhoNovoOv.nm_hierarquia = nm_hierarquia_pai + " > " + orgaoFilhoNovoOv.nm_orgao;
                                    lista_chaves_filhos.Add(orgaoFilhoNovoOv.nm_hierarquia);
								}


								orgaoRn.Atualizar(orgaoFilhoAnteriorOv._metadata.id_doc, orgaoFilhoAnteriorOv);
								orgaoRn.Incluir(orgaoFilhoNovoOv);
							}
							catch{
								if (orgaoFilhoAnteriorOv.dt_fim_vigencia != dt_fim_vigencia_rollback || orgaoFilhoAnteriorOv.st_orgao != st_orgao_rollback)
								{
									orgaoFilhoAnteriorOv.dt_fim_vigencia = dt_fim_vigencia_rollback;
									orgaoFilhoAnteriorOv.st_orgao = st_orgao_rollback;
									orgaoRn.Atualizar(orgaoFilhoAnteriorOv._metadata.id_doc, orgaoFilhoAnteriorOv);
								}
								lista_filhos_errados.Add(orgaoFilhoNovoOv.nm_orgao);
							}
						}
					}
					if (id_doc > 0)
					{
						if (lista_filhos_errados != null && lista_filhos_errados.Count > 0){
							var filhos_errados = "";
							foreach (var filho_errado in lista_filhos_errados){
								filhos_errados += (filhos_errados!= "" ? ", " : "") + filho_errado;
							}
							sRetorno = "{\"id_doc_success\":" + id_doc + "," +
								"\"filhos_errados\":\""+ filhos_errados +"\" }";
						}
						else{
							sRetorno = "{\"id_doc_success\":" + id_doc + "}";
						}
					}
					else
					{
						sRetorno = "{\"error_message\": \"Erro ao cadastrar órgão.\" }";
						throw new Exception("Erro ao incluir novo órgão.");
					}


                    orgaoOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (orgaoRn.Atualizar(id_doc, orgaoOv))
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"update\":true,\"b_atualizar_normas\":"+b_atualizar_normas+"}";

                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar órgão. id_doc:" + _id_doc);
                    }
                }
                else
                {
                    throw new Exception("Erro ao atualizar órgão. id_doc:" + _id_doc);
                }
                var log_atualizar = new LogAlterar<OrgaoOV>
                {
                    id_doc = id_doc,
                    registro = orgaoOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":\"" + _id_doc + "\"}";
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