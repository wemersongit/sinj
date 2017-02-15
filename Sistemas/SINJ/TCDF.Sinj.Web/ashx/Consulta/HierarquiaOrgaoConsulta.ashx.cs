using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Consulta
{
	public class HierarquiaOrgaoConsulta : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			var sRetorno = "";

			var _ch_hierarquia = context.Request["ch_hierarquia"];
			var _split_ch_hierarquia = _ch_hierarquia.Split('.');


			try
			{
				foreach ( var ch_orgao in _split_ch_hierarquia){
					if (!string.IsNullOrEmpty(ch_orgao))
					{
						var orgaoOv = new OrgaoRN().Doc(ch_orgao);

						sRetorno += (sRetorno != "" ? ">" : "") + orgaoOv.nm_orgao ;
					}
				}
				sRetorno = "{\"ds_hierarquia\":\""+sRetorno+"\"}";
			}
			catch (Exception ex)
			{
				sRetorno = "{\"error_message\": \"Ocorreu erro um erro na consulta de órgão.\"}";
				var erro = new ErroRequest
				{
					Pagina = context.Request.Path,
					RequestQueryString = context.Request.QueryString,
					MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
					StackTrace = ex.StackTrace
				};
				LogErro.gravar_erro(Util.GetEnumDescription(AcoesDoUsuario.nor_pes), erro, "","");
			}
			context.Response.ContentType = "application/json";
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
