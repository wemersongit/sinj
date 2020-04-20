using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for NormaConsulta
    /// </summary>
    public class NormaConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            Pesquisa pesquisa = new Pesquisa();

            var _ch_tipo_norma = context.Request["ch_tipo_norma"];
            var _nr_norma = context.Request["nr_norma"];
            var _cr_norma = context.Request["cr_norma"];
            var _nr_sequencial = context.Request["nr_sequencial"];
            var _dt_assinatura = context.Request["dt_assinatura"];
            var _ch_orgao = context.Request["ch_orgao"];
            var _bConsultarNormaDuplicada = context.Request["b_consultar_norma_duplicada"];
            var _bNormaSemNumero = context.Request["b_norma_sem_numero"];
            var _st_habilita_pesquisa = context.Request["st_habilita_pesquisa"];
            var _st_habilita_email = context.Request["st_habilita_email"];

            var action = AcoesDoUsuario.nor_pes;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                var normaRn = new NormaRN();
                var query = "";

                if (_bConsultarNormaDuplicada == "1")
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_norma) && !string.IsNullOrEmpty(_dt_assinatura) && !string.IsNullOrEmpty(_ch_orgao))
                    {
                        var ch_orgao_split = _ch_orgao.Split(',');
                        var nr_sequencial = 0;
                        int.TryParse(_nr_sequencial, out nr_sequencial);

                        // O campo nr_norma pode conter caracteres especiais
                        // É necessário buscar somente pelos números ou caso esteja com hifen
                        // Atualmente está sendo feito duas buscas
                        // ToDO: Analisar a maneira como a chave é gerada e talvez mudar a regra para colocar apenas os digitos em ch_para_nao_duplicacao


                        if (!string.IsNullOrEmpty(_nr_norma))
                        {
                            // Gerar chave usando somente os digitos de nr_norma
                            var numeros_nr_norma = new String(_nr_norma.Where(Char.IsDigit).ToArray());
                            if (numeros_nr_norma != _nr_norma)
                            {
                                var chaves_digitos = normaRn.GerarChaveParaNaoDuplicacaoDaNorma(_ch_tipo_norma, (_bNormaSemNumero == "1" ? "" : numeros_nr_norma), nr_sequencial, _cr_norma, _dt_assinatura, ch_orgao_split);
                                foreach (var chave in chaves_digitos)
                                {
                                    query += (query != "" ? " or " : "") + "'" + chave + "'=any(ch_para_nao_duplicacao)";
                                }
                            }

                            // Gerar chave usando nr_norma com hifen antes do ultimo digito
                            var nr_norma_com_hifen = numeros_nr_norma.Substring(0, numeros_nr_norma.Length - 1) + "-" + numeros_nr_norma.Substring(numeros_nr_norma.Length -1);
                            if (nr_norma_com_hifen != _nr_norma)
                            {
                                var chaves_hifen = normaRn.GerarChaveParaNaoDuplicacaoDaNorma(_ch_tipo_norma, (_bNormaSemNumero == "1" ? "" : nr_norma_com_hifen), nr_sequencial, _cr_norma, _dt_assinatura, ch_orgao_split);
                                foreach (var chave in chaves_hifen)
                                {
                                    query += (query != "" ? " or " : "") + "'" + chave + "'=any(ch_para_nao_duplicacao)";
                                }
                            }
                        }

                        var chaves = normaRn.GerarChaveParaNaoDuplicacaoDaNorma(_ch_tipo_norma, (_bNormaSemNumero == "1" ? "" : _nr_norma), nr_sequencial, _cr_norma, _dt_assinatura, ch_orgao_split);
                        foreach(var chave in chaves){
                            query += (query != "" ? " or " : "") + "'" + chave + "'=any(ch_para_nao_duplicacao)";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_norma))
                    {
                        query += (query != "" ? " and " : "") + "ch_tipo_norma='" + _ch_tipo_norma + "'";
                    }
                    if (!string.IsNullOrEmpty(_st_habilita_pesquisa))
                    {
                        query += (query != "" ? " and " : "") + "st_habilita_pesquisa='" + _st_habilita_pesquisa + "'";
                    }
                    if (!string.IsNullOrEmpty(_st_habilita_email))
                    {
                        query += (query != "" ? " or " : "") + "st_habilita_email='" + _st_habilita_email + "'";
                    }
                    if (!string.IsNullOrEmpty(_nr_norma))
                    {
                        query += (query != "" ? " and " : "") + "nr_norma='" + _nr_norma + "'";
                    }
                    if (!string.IsNullOrEmpty(_cr_norma))
                    {
                        query += (query != "" ? " and " : "") + "cr_norma='" + _cr_norma + "'";
                    }
                    if (!string.IsNullOrEmpty(_nr_sequencial))
                    {
                        query += (query != "" ? " and " : "") + "nr_sequencial='" + _nr_sequencial + "'";
                    }
                    if (!string.IsNullOrEmpty(_dt_assinatura))
                    {
                        query += (query != "" ? " and " : "") + "dt_assinatura='" + _dt_assinatura + "'";
                    }
                    if (!string.IsNullOrEmpty(_ch_orgao))
                    {
                        var ch_orgao_split = _ch_orgao.Split(',');
                        var query_orgaos = "";
                        for (var i = 0; i < ch_orgao_split.Length; i++)
                        {
                            query_orgaos += (query_orgaos != "" ? " or " : "(") + "'" + _ch_orgao + "'=any(ch_orgao)";
                        }
                        query += (query != "" ? " and " : "") + query_orgaos + ")";
                    }
                }
                pesquisa.literal = query;
                sRetorno = normaRn.JsonReg(pesquisa);

                var ind_count = sRetorno.IndexOf("\"result_count\": ") + "\"result_count\": ".Length;
                var ind_chaves = sRetorno.IndexOf("}", ind_count);
                var ind_virgula = sRetorno.IndexOf(",", ind_count);
                var Busca = new LogBuscar
                {
                    RegistrosTotal = sRetorno.Substring(ind_count, (ind_chaves > 0 ? ind_chaves : ind_virgula) - ind_count),
                    PesquisaLight = pesquisa
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), Busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\": \"Ocorreu erro um erro na consulta da norma.\"}";
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
