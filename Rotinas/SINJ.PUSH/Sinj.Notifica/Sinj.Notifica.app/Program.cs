using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BRLight.Util;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.app
{
    class Program
    {
        private static string _stringConnection;
        private static string _servidorDeEmail;
        private static string _contaDeEmail;
        private static string _senhaDeEmail;
        private static int _portaDeEmail;
        private static bool _ssl;
        private static bool _credentials;
        private static string _linkImagemEmailTopo;
        private static string _linkImagemEmailRodape;

        static void Main(string[] args)
        {
            try
            {
                _stringConnection = Configuracao.ValorChave("LightBaseConnectionString");
                _servidorDeEmail = Configuracao.ValorChave("ServidorDeEmail");
                _contaDeEmail = Configuracao.ValorChave("EmailFrom");
                _senhaDeEmail = Configuracao.ValorChave("EmailSenha");
                _portaDeEmail = int.Parse(Configuracao.ValorChave("EmailPorta"));
                _ssl = bool.Parse(Configuracao.ValorChave("EnableSsl"));
                _credentials = bool.Parse(Configuracao.ValorChave("EnableCredentials"));
                _linkImagemEmailTopo = Configuracao.ValorChave("LinkImagemEmailTopo");
                _linkImagemEmailRodape = Configuracao.ValorChave("LinkImagemEmailRodape");

                NotificaCtrl notificador = new NotificaCtrl(_stringConnection);

                Console.WriteLine("Iniciando...");
                List<Push> listaDeUsuariosPush = notificador.BuscaUsuariosPush();
                Console.WriteLine("Usuários selecionados: " + listaDeUsuariosPush.Count);
                Log.Writer("Usuários selecionados: " + listaDeUsuariosPush.Count);

                List<Norma> listaDeNormasAlteradas = notificador.BuscaNormasAlteradas();
                Console.WriteLine("Atos atualizados     : " + listaDeNormasAlteradas.Count);
                Log.Writer("Atos atualizados     : " + listaDeNormasAlteradas.Count);

                List<Norma> listaDeNormasNovas = notificador.BuscaNormasNovas();
                Console.WriteLine("Atos criados         : " + listaDeNormasNovas.Count);
                Log.Writer("Atos criados         : " + listaDeNormasNovas.Count);

                NotificaSobreNormasAlteradas(listaDeUsuariosPush, listaDeNormasAlteradas);
                NotificaSobreNormasNovas(listaDeUsuariosPush, listaDeNormasNovas);

                int updateds = notificador.AtualizaAtosNotificados();

                Console.WriteLine("Registros atualizados na Base no campo atlz e nova: " + updateds);

                Thread.Sleep(60000);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception         : " + ex.Message);
                Log.Writer(ex, "Erro no método Principal.");
            }
        }

        private static void NotificaSobreNormasNovas(List<Push> listaDeUsuariosPush, List<Norma> listaDeNormasNovas)
        {
            Console.WriteLine("Iniciando notificação sobre os atos criados...");
            List<Notificacao> lista = new List<Notificacao>();
            foreach (Push push in listaDeUsuariosPush)
            {
                lista.AddRange(CriaListaDeNotificoesDeNormasNovas(push, listaDeNormasNovas));
            }
            foreach (var notificacao in lista)
            {
                try
                {
                    Notificar(notificacao);
                    Console.WriteLine("Usuário notificado: " + notificacao.To);
                    Log.Writer("Notificacao, de ato criado, enviada para o email " + notificacao.To + ", usuário de id " + notificacao.IdUsuário);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro usuário: " + notificacao.To);
                    Log.Writer(ex, "Erro na notificacao sobre norma criada para email " + notificacao.To + ", usuário de id " + notificacao.IdUsuário);
                }
            }

        }

        private static void NotificaSobreNormasAlteradas(List<Push> listaDeUsuariosPush, List<Norma> listaDeNormasAlteradas)
        {
            Console.WriteLine("Iniciando notificação sobre os atos atualizados...");
            List<Notificacao> lista = new List<Notificacao>();
            foreach (Norma normaAlterada in listaDeNormasAlteradas)
            {
                IEnumerable<Push> iePush = from push in listaDeUsuariosPush where push.AtosVerifAtlzcaoValue.Count(a => a.IdNorma == normaAlterada.Id.ToString()) > 0 select push;
                foreach (var push in iePush)
                {
                    lista.Add(CriaNotificacaoNormasAlteradas(push, normaAlterada));
                }
            }
            foreach (var notificacao in lista)
            {
                try
                {
                    Notificar(notificacao);
                    Console.WriteLine("Usuário notificado: " + notificacao.To);
                    Log.Writer("Notificacao, de ato alterado, enviada para o email " + notificacao.To + ", usuário de id " + notificacao.IdUsuário);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro usuário: " + notificacao.To);
                    Log.Writer(ex, "Erro na notificacao sobre norma alterada para email " + notificacao.To + ", usuário de id " + notificacao.IdUsuário);
                }
            }

        }

        private static List<Notificacao> CriaListaDeNotificoesDeNormasNovas(Push push, List<Norma> listaDeNormasNovas)
        {
            List<Notificacao> lista = new List<Notificacao>();
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriterios = from p in push.NovosAtosPorCriteriosValue where p.AtivoItemNovosAtosPorCriterios select p;
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriteriosOuOu = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "OU" && novo.SegundoConec == "OU" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriteriosOuOu)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto || norma.Origens.Count(o => o.Id == novo.Origem) > 0 || norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriteriosEE = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "E" && novo.SegundoConec == "E" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriteriosEE)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto && norma.Origens.Count(o => o.Id == novo.Origem) > 0 && norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriteriosEOu = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "E" && novo.SegundoConec == "OU" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriteriosEOu)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto && norma.Origens.Count(o => o.Id == novo.Origem) > 0 || norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriteriosOuE = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "OU" && novo.SegundoConec == "E" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriteriosOuE)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto || norma.Origens.Count(o => o.Id == novo.Origem) > 0 && norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriterios_E = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "" && novo.SegundoConec == "E" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriterios_E)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where (norma.Tipo.Id == novo.TipoAto || norma.Origens.Count(o => o.Id == novo.Origem) > 0) && norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriteriosE_ = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "E" && novo.SegundoConec == "" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriteriosE_)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto && (norma.Origens.Count(o => o.Id == novo.Origem) > 0 || norma.Indexacao.Contains(novo.Indexacao)) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriterios_Ou = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "" && novo.SegundoConec == "OU" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriterios_Ou)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto || norma.Origens.Count(o => o.Id == novo.Origem) > 0 || norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriteriosOu_ = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "OU" && novo.SegundoConec == "" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriteriosOu_)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto || norma.Origens.Count(o => o.Id == novo.Origem) > 0 || norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }
            IEnumerable<NovosAtosPorCriterios> novosAtosPorCriterios__ = from novo in novosAtosPorCriterios where novo.PrimeiroConec == "" && novo.SegundoConec == "" select novo;
            foreach (NovosAtosPorCriterios novo in novosAtosPorCriterios__)
            {
                IEnumerable<Norma> normas = from norma in listaDeNormasNovas where norma.Tipo.Id == novo.TipoAto || norma.Origens.Count(o => o.Id == novo.Origem) > 0 || norma.Indexacao.Contains(novo.Indexacao) select norma;
                foreach (var norma in normas)
                {
                    lista.Add(CriaNotificacaoNormaNova(push, norma, novo));
                }
            }

            return lista;
        }
        private static void Notificar(Notificacao notificacao)
        {
            Email.EnviaEmailSmtp(notificacao.Body, notificacao.Subject, notificacao.From, "Sinj Notifica", notificacao.To, _servidorDeEmail, _portaDeEmail, _senhaDeEmail, _ssl, _credentials);
        }



        private static Notificacao CriaNotificacaoNormaNova(Push push, Norma norma, NovosAtosPorCriterios novo)
        {
            Notificacao notificacao = new Notificacao();
            notificacao.IdNorma = norma.Id;
            notificacao.IdUsuário = Convert.ToInt32(push.Id);
            notificacao.From = _contaDeEmail;
            notificacao.To = push.Email;
            notificacao.Subject = CriaAssuntoDaNotificacao(norma);
            notificacao.Body = CriaMensagemDaNotificacaoDeNovas(push, norma, novo);
            return notificacao;
        }

        private static Notificacao CriaNotificacaoNormasAlteradas(Push push, Norma normaAlterada)
        {
            Notificacao notificacao = new Notificacao();
            notificacao.IdNorma = normaAlterada.Id;
            notificacao.IdUsuário = Convert.ToInt32(push.Id);
            notificacao.From = _contaDeEmail;
            notificacao.To = push.Email;
            notificacao.Subject = CriaAssuntoDaNotificacao(normaAlterada);
            notificacao.Body = CriaMensagemDaNotificacaoDeAlterados(push, normaAlterada);
            return notificacao;
        }

        private static string CriaMensagemDaNotificacaoDeAlterados(Push push, Norma norma)
        {
            int linha = push.AtosVerifAtlzcaoValue.FindIndex(a => a.IdNorma == norma.Id.ToString());
            string corpoEmail = "";
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

            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:100px;\">" + norma.Tipo.Nome + " " + norma.Numero + " de " + norma.DataAssinatura.ToString("dd/MM/yyyy");

            int quantidadeDeOrgaos = norma.Origens.Count;
            if (quantidadeDeOrgaos > 0)
            {
                if (quantidadeDeOrgaos > 1)
                {
                    corpoEmail = corpoEmail + " dos órgãos: ";
                    int i = 0;
                    foreach (OrgaoSinj orgao in norma.Origens)
                    {
                        if (i == quantidadeDeOrgaos - 1)
                            corpoEmail = corpoEmail + " e " + orgao.Sigla;
                        else
                            corpoEmail = corpoEmail + orgao.Sigla + ", ";
                        i++;
                    }
                }
                else
                {
                    corpoEmail = corpoEmail + " do órgão: " + norma.Origens.First().Sigla;
                }
            }

            corpoEmail = corpoEmail + "</td>";
            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:200px;\">" + norma.Ementa + "</td>";
            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:80px;\"> <a href=http://www.sinj.df.gov.br/SINJ/DetalhesDeNorma.aspx?id_norma=" + norma.Id + ">Clique para ver o ato</a></td>";
            corpoEmail = corpoEmail + "			</tr>";
            corpoEmail = corpoEmail + "		</tbody>";
            corpoEmail = corpoEmail + "		</table>";
            corpoEmail = corpoEmail + "	</div>";
            corpoEmail = corpoEmail + "		<div style=\"margin-bottom: 3px; font-size: 10px; font-weight: bold; background-color:#B4E6CBs\">";
            corpoEmail = corpoEmail + "		<br/>";
            corpoEmail = corpoEmail + "		<br/>";
            corpoEmail = corpoEmail + "	</div>";
            corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
            corpoEmail = corpoEmail + "		<a href=http://www.sinj.df.gov.br/SINJ/DesativaNormaPush.aspx?id_norma=" + norma.Id + "&" + "id_usuario=" + push.Id + "&linha=" + linha + "&tipo=2" + ">Não quero mais receber informações sobre este ato.";
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

            return corpoEmail;
        }

        private static string CriaMensagemDaNotificacaoDeNovas(Push push, Norma norma, NovosAtosPorCriterios novo)
        {
            int linha = push.NovosAtosPorCriteriosValue.FindIndex(n => n.IdNovosAtosPorCriterios == novo.IdNovosAtosPorCriterios);
            string corpoEmail = "";
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

            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:100px;\">" + norma.Tipo.Nome + " " + norma.Numero + " de " + norma.DataAssinatura.ToString("dd/MM/yyyy");

            int quantidadeDeOrgaos = norma.Origens.Count;
            if (quantidadeDeOrgaos > 0)
            {
                if (quantidadeDeOrgaos > 1)
                {
                    corpoEmail = corpoEmail + " dos órgãos: ";
                    int i = 0;
                    foreach (OrgaoSinj orgao in norma.Origens)
                    {
                        if (i == quantidadeDeOrgaos - 1)
                            corpoEmail = corpoEmail + " e " + orgao.Sigla;
                        else
                            corpoEmail = corpoEmail + orgao.Sigla + ", ";
                        i++;
                    }
                }
                else
                {
                    corpoEmail = corpoEmail + " do órgão: " + norma.Origens.First().Sigla;
                }
            }

            corpoEmail = corpoEmail + "</td>";
            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:200px;\">" + norma.Ementa + "</td>";
            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:80px;\"> <a href=http://www.sinj.df.gov.br/SINJ/DetalhesDeNorma.aspx?id_norma=" + norma.Id + ">Clique para ver o ato</a></td>";
            corpoEmail = corpoEmail + "			</tr>";
            corpoEmail = corpoEmail + "		</tbody>";
            corpoEmail = corpoEmail + "		</table>";
            corpoEmail = corpoEmail + "	</div>";
            corpoEmail = corpoEmail + "		<div style=\"margin-bottom: 3px; font-size: 10px; font-weight: bold; background-color:#B4E6CBs\">";
            corpoEmail = corpoEmail + "		<br/>";
            corpoEmail = corpoEmail + "		<br/>";
            corpoEmail = corpoEmail + "	</div>";
            corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
            corpoEmail = corpoEmail + "		<a href=http://www.sinj.df.gov.br/SINJ/DesativaNormaPush.aspx?id_norma=" + novo.IdNovosAtosPorCriterios + "&" + "id_usuario=" + push.Id + "&linha=" + linha + "&tipo=1" + ">Não quero mais receber informações sobre este ato.";
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

            return corpoEmail;
        }

        private static string CriaAssuntoDaNotificacao(Norma norma)
        {
            string assunto = "Informações sobre o ato " + norma.Tipo.Nome + " " + norma.Numero + " de " + norma.DataAssinatura.ToString("dd/MM/yyyy");

            int quantidadeDeOrgaos = norma.Origens.Count;
            if (quantidadeDeOrgaos > 0)
            {
                if (quantidadeDeOrgaos > 1)
                {
                    assunto = assunto + " dos órgãos: ";
                    int i = 0;
                    foreach (OrgaoSinj orgao in norma.Origens)
                    {
                        if (i == quantidadeDeOrgaos - 1)
                            assunto = assunto + " e " + orgao.Sigla;
                        else
                            assunto = assunto + orgao.Sigla + ", ";
                        i++;
                    }
                }
                else
                {
                    assunto = assunto + " do órgão: " + norma.Origens.First().Sigla;
                }
            }
            return assunto;
        }
    }
}
