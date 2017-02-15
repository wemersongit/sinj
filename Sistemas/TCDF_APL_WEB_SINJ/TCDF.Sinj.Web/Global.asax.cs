using System;
using System.Web;
using System.Web.SessionState;
using System.Threading;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Linq;
using TCDF.Sinj.Log;
using System.Web.Routing;

namespace TCDF.Sinj.Web
{
    public class Global : HttpApplication, IRequiresSessionState
    {
		void Application_Start(object sender, EventArgs e)
        {
            RegisterCustomRoutes(RouteTable.Routes);
		    new Thread(ClearLog).Start();
		    // Quando a aplicação iniciar a thread é iniciada se a variável for true
		    //if (Convert.ToBoolean(Config.ValorChave("MonitorarPush", true)))
		    //{
		    //new Thread (MonitorarPush).Start ();
		    //}
		    //new Thread (DeletarAcaoSemVide).Start ();
		    //new Thread(IniciarSinjMetaminer).Start();
		}

        public void ClearLog()
        {
            try
            {
                // data de hoje menos(-) dez dias....
                var _dt = DateTime.Now.AddDays(-10).ToString("dd'/'MM'/'yyyy HH:mm:ss");
                var query = new Pesquisa { literal = string.Format("CAST(dt_log_erro AS DATE) < '{0}'", _dt) };

                var olog_erro = new Reg("sinj_log_erro");
                olog_erro.excluir(query);
                olog_erro = null;
            }
            catch
            {
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var context = HttpContext.Current;
                var exception = HttpContext.Current.Server.GetLastError();

                if (exception != null)
                {
                    var httpException = (HttpException)exception;
                    var errorCode = httpException.GetHttpCode();

                    if (errorCode.ToString() != "404")
                    {
                        var _erro = new ErroNET
                        {
                            Code = errorCode.ToString(),
                            Mensagem = exception.Message.ToString(),
                            Trace = exception.StackTrace.ToString(),
                            Source = exception.Source.ToString(),
                            Pagina = context.Request.Url.ToString()
                        };

                        LogErro.gravar_erro(".NET", _erro, "", "");
                    }

                    if (errorCode.ToString() == "404")
                    {
                        Response.Clear();
                        Server.ClearError();
                        Response.Redirect(Config.ValorChave("Padrao", true) + "/404.html", false);
                    }
                }

                Response.Clear();
                Server.ClearError();

            }
            catch
            {
            }
        }

		public void MonitorarPush(){
			try{
				while (true){
					MonitorarNormasAtualizadasPush ();
					MonitorarNormasNovasPush ();
					Thread.Sleep (Convert.ToInt32 (Config.ValorChave ("MonitorarPushSleep", true)));
				}
			}
			catch{
			}
		}

		public void MonitorarNormasNovasPush()
		{
			Pesquisa pesquisa = new Pesquisa ();
			NotifiquemeOV notifiquemeOv = new NotifiquemeOV();
			NormaOV normaOv = new NormaOV ();
			try{
				NotifiquemeRN notifiquemeRN = new NotifiquemeRN (false);
				NormaRN normaRn = new NormaRN ();
				var query_push_st_criacao = "true = any(st_criacao)";
				pesquisa.literal = query_push_st_criacao; 
				var results_notifiqueme = notifiquemeRN.Consultar (pesquisa);
				foreach (var usuario_push in results_notifiqueme.results)
				{
					notifiquemeOv = usuario_push;
					var j = 0;
					foreach (var criacao_normas_monitoradas in usuario_push.criacao_normas_monitoradas)
					{
						var ch_exclusao = Guid.NewGuid().ToString("N").Substring(0,8);
						var query = "st_nova = true AND (";
						if (!String.IsNullOrEmpty(criacao_normas_monitoradas.ch_tipo_norma_criacao))
						{
							query += "ch_tipo_norma = '" + criacao_normas_monitoradas.ch_tipo_norma_criacao + "' ";
						}
						if (!String.IsNullOrEmpty(criacao_normas_monitoradas.primeiro_conector_criacao))
						{
							if (criacao_normas_monitoradas.primeiro_conector_criacao == "E")
							{
								query += "AND ";
							}
							else if (criacao_normas_monitoradas.primeiro_conector_criacao == "OU")
							{
								query += "OR ";
							}
						}
						if (!String.IsNullOrEmpty(criacao_normas_monitoradas.ch_orgao_criacao))
						{
							query += "'" + criacao_normas_monitoradas.ch_orgao_criacao + "' = any(ch_orgao) ";
						}
						if (!String.IsNullOrEmpty(criacao_normas_monitoradas.primeiro_conector_criacao))
						{
							if (criacao_normas_monitoradas.segundo_conector_criacao == "E")
							{
								query += "AND ";
							}
							else if (criacao_normas_monitoradas.segundo_conector_criacao == "OU")
							{
								query += "OR ";
							}
						}
						if (!String.IsNullOrEmpty(criacao_normas_monitoradas.ch_termo_criacao))
						{
							query += "'" + criacao_normas_monitoradas.ch_tipo_termo_criacao + "' = any(ch_termo) ";
						}
						query += ")";

						pesquisa.literal = query;
						var results_normas_novas = normaRn.Consultar (pesquisa);
						foreach (var norma_nova in results_normas_novas.results)
						{
                            int quantidadeDeOrgaos = norma_nova.origens.Count;
                            normaOv = norma_nova;
							var email = new EmailRN ();
                            var display_name_remetente = "SINJ Notifica";
							var destinatario = new [] { usuario_push.email_usuario_push };
                            var titulo = "Informações sobre o ato " + norma_nova.nm_tipo_norma + " " + norma_nova.nr_norma + " de " + norma_nova.dt_assinatura + (quantidadeDeOrgaos > 0 ? " do órgão " + norma_nova.origens[0].sg_orgao : "");
							var html = true;

							var _linkImagemEmailTopo = "" + Config.ValorChave("LinkSINJ", true)+ "/Imagens/topo_sinj.jpg";
							var _linkImagemEmailRodape = "" + Config.ValorChave("LinkSINJ", true)+ "/Imagens/rodape_sinj.jpg";

							var corpoEmail = "";
							corpoEmail = corpoEmail + "<table width=\"100%\" style=\"border:1px\"> ";
							corpoEmail = corpoEmail + "     <tr> <td>";
							corpoEmail = corpoEmail + "<table width = \"600\" style=\"background:#ddffdc;\" align=\"center\" >";
							corpoEmail = corpoEmail + "<tr>";
							corpoEmail = corpoEmail + "<td>";
							corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\">";
							corpoEmail = corpoEmail + "<tr>";
							corpoEmail = corpoEmail + "<td><a href=\"http://www.sinj.df.gov.br\" target=\"_blank\" title=\"http://www.sinj.df.gov.br\"><img src=" + _linkImagemEmailTopo + "></a></td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + "</table>";
							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + "<tr>";
							corpoEmail = corpoEmail + "<td>";
							corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\"><br/>";
							corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
							corpoEmail = corpoEmail + "<tr>";
							corpoEmail = corpoEmail + "<td style=\"background-color: #B4E6CBs; text-align: left;\">";
							corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 12px; font-weight: bold; background-color:#B4E6CBs\">";
							corpoEmail = corpoEmail + "		Um tipo de ato que você escolheu para monitorar foi criado.<br/>";
							corpoEmail = corpoEmail + "		As informações sobre esse ato estão abaixo:";
							corpoEmail = corpoEmail + "	</div>";

							corpoEmail = corpoEmail + "	<div>";
							corpoEmail = corpoEmail + "		Ato:";
							corpoEmail = corpoEmail + "		<table cellspacing=\"0\" cellpadding=\"2\" rules=\"all\" border=\"1\" style=\"border-color:#A3A3A3;border-style:Solid;width:100%;border-collapse:collapse;font-size: 11px;\">";
							corpoEmail = corpoEmail + "			<tbody>";
							corpoEmail = corpoEmail + "			<tr class=\"textoCorVide\" align=\"left\" style=\"color:#323232;background-color:#B4E6CB;height:30px;\">";
							corpoEmail = corpoEmail + "				<th scope=\"col\">Identificação</th>";
							corpoEmail = corpoEmail + "				<th scope=\"col\">Ementa</th>";
							corpoEmail = corpoEmail + "				<th scope=\"col\" style=\"width:80px;\">Link</th>";
							corpoEmail = corpoEmail + "			</tr><tr align=\"left\" style=\"background-color:#F0F0F0;height:20px;\">";

							corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:100px;\">" + norma_nova.nm_tipo_norma + " " + norma_nova.nr_norma + " de " + norma_nova.dt_assinatura;

							if (quantidadeDeOrgaos > 0)
							{
								if (quantidadeDeOrgaos > 1)
								{
									corpoEmail = corpoEmail + " dos órgãos: ";
									int i = 0;
									foreach (var orgao in norma_nova.origens)
									{
										if (i == quantidadeDeOrgaos - 1)
											corpoEmail = corpoEmail + " e " + orgao.sg_orgao;
										else
											corpoEmail = corpoEmail + orgao.sg_orgao + ", ";
										i++;
									}
								}
								else
								{
									corpoEmail = corpoEmail + " do órgão: " + norma_nova.origens.First().sg_orgao;
								}
							}

							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:200px;\">" + norma_nova.ds_ementa + "</td>";
							corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:80px;\"> <a href="+Config.ValorChave("LinkSINJ", true)+"/DetalhesDeNorma.aspx?id_norma=" + norma_nova.ch_norma + ">Clique para ver o ato</a></td>";
							corpoEmail = corpoEmail + "			</tr>";
							corpoEmail = corpoEmail + "		</tbody>";
							corpoEmail = corpoEmail + "		</table>";
							corpoEmail = corpoEmail + "	</div>";
							corpoEmail = corpoEmail + "		<div style=\"margin-bottom: 3px; font-size: 10px; font-weight: bold; background-color:#B4E6CBs\">";
							corpoEmail = corpoEmail + "		<br/>";
							corpoEmail = corpoEmail + "		<br/>";
							corpoEmail = corpoEmail + "	</div>";
							corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
							corpoEmail = corpoEmail + "		<a href="+Config.ValorChave("LinkSINJ", true)+"/DesativarNormaPush.aspx?email_usuario_push=" + usuario_push.email_usuario_push + "&" + "criacao_normas_monitoradas=" + j + "&"+ "ch_exclusao=" + ch_exclusao + ">Não quero mais receber informações sobre a criação desses atos.</a>";
							corpoEmail = corpoEmail + "	</div>";
							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + " </tr>";
							corpoEmail = corpoEmail + " </table>";
							corpoEmail = corpoEmail +
								"<table width=\"600px\" style=\"background:#ddffdc;\" align=\"center\" > <br/>";
							corpoEmail = corpoEmail + "<tr>";
							corpoEmail = corpoEmail + "<td>";
							corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
							corpoEmail = corpoEmail + "<img src=" + _linkImagemEmailRodape + " width=\"600px\">";

							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + " </table >";
							corpoEmail = corpoEmail + " </td>";
							corpoEmail = corpoEmail + " </tr>";
							corpoEmail = corpoEmail + " </table>";
							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + "</table>";

							email.EnviaEmail (display_name_remetente, destinatario, titulo, html, corpoEmail);

							normaRn.PathPut (norma_nova._metadata.id_doc, "st_nova", "false", null);
						}
						j++;
					}
				}
			}
			catch(Exception ex){
				try{
					var erro = new ErroPush
					{
						NormaOv = JSON.Serialize<NormaOV>(normaOv),
						NotifiquemeOv = JSON.Serialize<NotifiquemeOV>(notifiquemeOv),
						Pesquisa = JSON.Serialize<Pesquisa>(pesquisa),
						MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
						StackTrace = ex.StackTrace
					};
					LogErro.gravar_erro("PUSH", erro, "", "");
				}
				catch{
				}
			}
		}

		public void MonitorarNormasAtualizadasPush()
		{
			Pesquisa pesquisa = new Pesquisa ();
			NotifiquemeOV notifiquemeOv = new NotifiquemeOV();
			NormaOV normaOv = new NormaOV ();
			try
			{
				NormaRN normaRn = new NormaRN ();
				var query_normas = "st_atualizada=true";
				pesquisa.literal = query_normas;
				var results_normas = normaRn.Consultar (pesquisa);
				foreach (var resultado_norma in results_normas.results)
				{
                    normaOv = resultado_norma;
                    int quantidadeDeOrgaos = resultado_norma.origens.Count;
					NotifiquemeRN notifiquemeRn = new NotifiquemeRN (false);
					var query_user = "'" + resultado_norma.ch_norma + "'=any(ch_norma_monitorada)";
					pesquisa.literal = query_user;
					var results_notifiqueme = notifiquemeRn.Consultar (pesquisa);
					foreach (var usuario_push in results_notifiqueme.results) 
					{
						notifiquemeOv = usuario_push;
						if (!usuario_push.normas_monitoradas.Where (n => n.ch_norma_monitorada == resultado_norma.ch_norma).First().st_norma_monitorada)
							continue;
						var email = new EmailRN ();
						var display_name_remetente = "SINJ Notifica";
						var destinatario = new [] { usuario_push.email_usuario_push };
                        var titulo = "Informações sobre o ato " + resultado_norma.nm_tipo_norma + " " + resultado_norma.nr_norma + " de " + resultado_norma.dt_assinatura + ( quantidadeDeOrgaos > 0 ? " do órgão " + resultado_norma.origens[0].sg_orgao : "");
						var html = true;

                        var _linkImagemEmailTopo = "" + Config.ValorChave("LinkSINJ", true) + "/Imagens/topo_sinj.jpg";
                        var _linkImagemEmailRodape = "" + Config.ValorChave("LinkSINJ", true) + "/Imagens/rodape_sinj.jpg";


						var corpoEmail = "";
						corpoEmail = corpoEmail + "<table width=\"100%\" style=\"border:1px\"> ";
						corpoEmail = corpoEmail + "     <tr> <td>";
						corpoEmail = corpoEmail + "<table width = \"600\" style=\"background:#ddffdc;\" align=\"center\" >";
						corpoEmail = corpoEmail + "<tr>";
						corpoEmail = corpoEmail + "<td>";
						corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\">";
						corpoEmail = corpoEmail + "<tr>";
						corpoEmail = corpoEmail + "<td><a href=\"http://www.sinj.df.gov.br\" target=\"_blank\" title=\"http://www.sinj.df.gov.br\"><img src=" + _linkImagemEmailTopo + "></a></td>";
						corpoEmail = corpoEmail + "</tr>";
						corpoEmail = corpoEmail + "</table>";
						corpoEmail = corpoEmail + "</td>";
						corpoEmail = corpoEmail + "</tr>";
						corpoEmail = corpoEmail + "<tr>";
						corpoEmail = corpoEmail + "<td>";
						corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\"><br/>";
						corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
						corpoEmail = corpoEmail + "<tr>";
						corpoEmail = corpoEmail + "<td style=\"background-color: #B4E6CBs; text-align: left;\">";
						corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 12px; font-weight: bold; background-color:#B4E6CBs\">";
						corpoEmail = corpoEmail + "		Um ato que você escolheu para monitorar sofreu alteração.<br/>";
						corpoEmail = corpoEmail + "		As informações sobre esse ato estão abaixo:";
						corpoEmail = corpoEmail + "	</div>";

						corpoEmail = corpoEmail + "	<div>";
						corpoEmail = corpoEmail + "		Ato:";
						corpoEmail = corpoEmail + "		<table cellspacing=\"0\" cellpadding=\"2\" rules=\"all\" border=\"1\" style=\"border-color:#A3A3A3;border-style:Solid;width:100%;border-collapse:collapse;font-size: 11px;\">";
						corpoEmail = corpoEmail + "			<tbody>";
						corpoEmail = corpoEmail + "			<tr class=\"textoCorVide\" align=\"left\" style=\"color:#323232;background-color:#B4E6CB;height:30px;\">";
						corpoEmail = corpoEmail + "				<th scope=\"col\">Identificação</th>";
						corpoEmail = corpoEmail + "				<th scope=\"col\">Ementa</th>";
						corpoEmail = corpoEmail + "				<th scope=\"col\" style=\"width:80px;\">Link</th>";
						corpoEmail = corpoEmail + "			</tr><tr align=\"left\" style=\"background-color:#F0F0F0;height:20px;\">";

						corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:100px;\">" + resultado_norma.nm_tipo_norma + " " + resultado_norma.nr_norma + " de " + resultado_norma.dt_assinatura;

						if (quantidadeDeOrgaos > 0) {
							if (quantidadeDeOrgaos > 1) {
								corpoEmail = corpoEmail + " dos órgãos: ";
								int i = 0;
								foreach (var orgao in resultado_norma.origens) {
									if (i == quantidadeDeOrgaos - 1)
										corpoEmail = corpoEmail + " e " + orgao.sg_orgao;
									else
										corpoEmail = corpoEmail + orgao.sg_orgao + ", ";
									i++;
								}
							} else {
								corpoEmail = corpoEmail + " do órgão: " + resultado_norma.origens [0].sg_orgao;
							}
						}

							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:200px;\">" + resultado_norma.ds_ementa + "</td>";
							corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:80px;\"> <a href="+Config.ValorChave("LinkSINJ", true)+"/DetalhesDeNorma.aspx?id_norma=" + resultado_norma.ch_norma + ">Clique para ver o ato</a></td>";
							corpoEmail = corpoEmail + "			</tr>";
							corpoEmail = corpoEmail + "		</tbody>";
							corpoEmail = corpoEmail + "		</table>";
							corpoEmail = corpoEmail + "	</div>";
							corpoEmail = corpoEmail + "		<div style=\"margin-bottom: 3px; font-size: 10px; font-weight: bold; background-color:#B4E6CBs\">";
							corpoEmail = corpoEmail + "		<br/>";
							corpoEmail = corpoEmail + "		<br/>";
							corpoEmail = corpoEmail + "	</div>";
							corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
							corpoEmail = corpoEmail + "		<a href="+Config.ValorChave("LinkSINJ", true)+"/DesativarNormaPush.aspx?ch_norma=" + resultado_norma.ch_norma + "&" + "email_usuario_push=" + usuario_push.email_usuario_push + ">Não quero mais receber informações sobre este ato. </a>";
							corpoEmail = corpoEmail + "	</div>";
							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + " </tr>";
							corpoEmail = corpoEmail + " </table>";
							corpoEmail = corpoEmail + "<table width=\"600px\" style=\"background:#ddffdc;\" align=\"center\" > <br/>";
							corpoEmail = corpoEmail + "<tr>";
							corpoEmail = corpoEmail + "<td>";
							corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
							corpoEmail = corpoEmail + "<img src=" + _linkImagemEmailRodape + " width=\"600px\">";
							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
							corpoEmail = corpoEmail + " </table >";
							corpoEmail = corpoEmail + " </td>";
							corpoEmail = corpoEmail + " </tr>";
							corpoEmail = corpoEmail + " </table>";
							corpoEmail = corpoEmail + "</td>";
							corpoEmail = corpoEmail + "</tr>";
  							corpoEmail = corpoEmail + "</table>";


						email.EnviaEmail (display_name_remetente, destinatario, titulo, html, corpoEmail);
					}
						normaRn.PathPut (resultado_norma._metadata.id_doc, "st_atualizada", "false", null);
				}
			}
			catch(Exception ex){
				try{
					var erro = new ErroPush
					{
						NormaOv = JSON.Serialize<NormaOV>(normaOv),
						NotifiquemeOv = JSON.Serialize<NotifiquemeOV>(notifiquemeOv),
						Pesquisa = JSON.Serialize<Pesquisa>(pesquisa),
						MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
						StackTrace = ex.StackTrace
					};
					LogErro.gravar_erro("PUSH", erro, "", "");
				}
				catch{
				}
			}
		}

		public void DeletarAcaoSemVide()
		{
			while (true){
				Pesquisa pesquisa = new Pesquisa ();
				NormaOV normaOv = new NormaOV ();
				NormaRN normaRn = new NormaRN ();
				try{
					var query_normas = "st_acao = true and in_vides < 1 and CAST(dt_doc AS abstime) < '"+ DateTime.Now.AddMilliseconds(-Convert.ToInt32(Config.ValorChave("DeletarAcaoSemVideSleep", true))).ToString("dd'/'MM'/'yyyy HH:mm:ss") +"' ";
					pesquisa.literal = query_normas;
					var results_normas = normaRn.Consultar(pesquisa);
					foreach (var norma_acao in results_normas.results)
					{
						normaOv = norma_acao;
                        normaRn.Excluir(norma_acao);
					}
				}
				catch(Exception ex){
					try{
						var erro = new TCDF.Sinj.Log.ErroNET
						{
                            Mensagem = Excecao.LerTodasMensagensDaExcecao(ex, true)+(normaOv!= null ? (normaOv._metadata != null ? ". Erro na norma, id_doc = "+normaOv._metadata.id_doc : "") : ""),
                            Trace = ex.StackTrace,
                            Source = ex.Source
						};
						LogErro.gravar_erro("DeletarAcaoSemVide", erro, "", "");
					}
					catch{
					}
				}
				Thread.Sleep (Convert.ToInt32 (Config.ValorChave ("DeletarAcaoSemVideSleep", true)));
			}
		}

        private void IniciarSinjMetaminer()
        {
            while(true){
                try
                {
                    if (DateTime.Now.Hour > 21 && DateTime.Now.Hour < 22)
                    {

                        Thread.Sleep(7200000);
                    }
                }
                catch(Exception ex){
                    try
                    {
                        var erro = new TCDF.Sinj.Log.ErroNET
                        {
                            Mensagem = Excecao.LerTodasMensagensDaExcecao(ex, true),
                            Trace = ex.StackTrace,
                            Source = ex.Source
                        };
                        LogErro.gravar_erro("SinjMetaminer", erro, "", "");
                    }
                    catch
                    {

                    }
                }
                Thread.Sleep(60000);
            }
        }

        private void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Default", "Default", "~/Default.aspx");
            routes.MapPageRoute("Pesquisas", "Pesquisas.aspx", "~/Default.aspx");


            routes.MapPageRoute("FaleConosco", "FaleConosco", "~/FaleConosco.aspx");
            routes.MapPageRoute("HistoricoDePesquisa", "HistoricoDePesquisa", "~/HistoricoDePesquisa.aspx");
            routes.MapPageRoute("Notifiqueme", "Notifiqueme", "~/Notifiqueme.aspx");
            routes.MapPageRoute("Favoritos", "Favoritos", "~/Favoritos.aspx");

            routes.MapPageRoute("ResultadoDePesquisa", "ResultadoDePesquisa", "~/ResultadoDePesquisa.aspx");

            //routes.MapPageRoute("dwn", "dwn/{*keywords}", "~/dwn.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Download", "Download/{*keywords}", "~/Download.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Norma", "Norma/{*keywords}", "~/Norma.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Diario", "Diario/{*keywords}", "~/Diario.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });

            //routes.MapPageRoute("Htmltopdf", "Htmltopdf/{*keywords}", "~/Htmltopdf.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
        }
    }
}