using System;
using System.Data;
using System.Text;
using System.Linq;
using neo.BRLightREST;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TCDF.Sinj.OV
{
    /// <summary>
    /// Replicar o vide em ambas as normas, não só na norma afetada. Isso vai render um ganho na busca e na recuperação.
    /// Tratar os dados que cada uma vai exibir durante a persistencia no banco, ou seja, a norma afetada fica com o campos preenchidos
    /// para na hora de exibir os vides já estarem tratados os campos em ambas as normas.
    /// </summary>
    public class Vide
    {
        public Vide()
        {
            alteracao_texto_vide = new AlteracaoDeTexoVide();
        }
        public string ch_vide { get; set; }
        public string ds_comentario_vide { get; set; }
        /// <summary>
        /// Se verdadeiro então a norma corrente é afetada pelo vide.
        /// </summary>
        public bool in_norma_afetada { get; set; }

        //Tipo de Relação
        public string ch_tipo_relacao { get; set; }
        public string nm_tipo_relacao { get; set; }
        /// <summary>
        /// Se a norma corrente for a afetada use o texto ds_texto_para_alterado, se não, use ds_texto_para_alterador, oriundos de de TipoDeRelacao
        /// </summary>
        public string ds_texto_relacao { get; set; }
        public bool in_relacao_de_acao { get; set; }

        //Dados da outra Norma
        public string dt_publicacao_fonte_norma_vide { get; set; }
        public string ch_norma_vide { get; set; }
        public string dt_inicio_vigencia_norma_vide {get;set;}
        public string ch_tipo_norma_vide { get; set; }
        public string nm_tipo_norma_vide { get; set; }
        public string nr_norma_vide { get; set; }
        public string dt_assinatura_norma_vide { get; set; }
        public string pagina_publicacao_norma_vide { get; set; }
        public string coluna_publicacao_norma_vide { get; set; }
        public string ch_tipo_fonte_norma_vide { get; set; }
        public string nm_tipo_fonte_norma_vide { get; set; }

        public string artigo_norma_vide { get; set; }
        public string paragrafo_norma_vide { get; set; }
        public string inciso_norma_vide { get; set; }
        public string alinea_norma_vide { get; set; }
        public string item_norma_vide { get; set; }
        public Caput caput_norma_vide { get; set; }
        public string anexo_norma_vide { get; set; }

        public string artigo_norma_vide_outra { get; set; }
        public string paragrafo_norma_vide_outra { get; set; }
        public string inciso_norma_vide_outra { get; set; }
        public string alinea_norma_vide_outra { get; set; }
        public string item_norma_vide_outra { get; set; }
        public Caput caput_norma_vide_outra { get; set; }
        public string anexo_norma_vide_outra { get; set; }

        public AlteracaoDeTexoVide alteracao_texto_vide { get; set; }

        public bool possuiDispositivoInformadoManualmente()
        {
            return !string.IsNullOrEmpty(artigo_norma_vide) ||
            !string.IsNullOrEmpty(paragrafo_norma_vide) ||
            !string.IsNullOrEmpty(inciso_norma_vide) ||
            !string.IsNullOrEmpty(alinea_norma_vide) ||
            !string.IsNullOrEmpty(item_norma_vide) ||
            !string.IsNullOrEmpty(anexo_norma_vide);
        }
    }

    public class Caput
    {
        public string[] caput { get; set; }
        public string ds_caput { get; set; }
        public string id_file { get; set; }
        public string ch_norma { get; set; }
        public string ds_norma { get; set; }
        public string path { get; set; }
        public string linkname { get; set; }
        public string[] texto_antigo { get; set; }
        public string[] texto_novo { get; set; }
        public string link { get; set; }
        public string filename { get; set; }
        public string dt_inicio_vigor { get; set; }
        public string st_aguardar_vigor { get; set; }
        public string nm_relacao_aux { get; set; }
        public string ds_texto_para_alterador_aux { get; set; }

        //public string ds_dispositivo()
        //{
        //    var descricao = "";
        //    var descricao_linha = "";
        //    var descricao_caput = "";
        //    var cSplited = new string[0];
        //    if(caput != null && caput.Length > 0)
        //    {
        //        foreach(var c in caput)
        //        {
        //            descricao += (!string.IsNullOrEmpty(descricao)) ? "; " : "";
        //            cSplited = c.Split('_');
        //            foreach(var cs in cSplited){
        //                descricao_caput = GetDescricaoDoDispositivo(cs);
        //                if(!string.IsNullOrEmpty(descricao_caput)){
        //                    descricao_linha += ((!string.IsNullOrEmpty(descricao_linha)) ? ", " : "") + descricao_caput;
        //                }
        //            }
        //        }
        //    }
        //    return descricao;
        //}

        //private string GetDescricaoDoDispositivo(string caput)
        //{
        //    var caput1 = caput.Substring(0, 3).ToLower();
        //    var caput2 = "";
        //    var caput3 = "";
        //    if(caput.Length > 3)
        //        caput2 = caput.Substring(3);
        //    if (caput1 == "ane")
        //    {
        //        caput3 = "Anexo";
        //    }
        //    else if (caput == "art")
        //    {
        //        caput3 = "Artigo";
        //    }
        //    else if (caput == "par")
        //    {
        //        caput3 = "Parágrafo";
        //    }
        //    else if (caput == "inc")
        //    {
        //        caput3 = "Inciso";
        //    }
        //    else if (caput == "ltr")
        //    {
        //        caput3 = "Letra";
        //    }
        //    else if (caput == "aln")
        //    {
        //        caput3 = "Alínea";
        //    }
        //    if (!string.IsNullOrEmpty(caput3))
        //    {
        //        caput3 += (!string.IsNullOrEmpty(caput2) ? " " : "") + caput2;
        //    }
        //    return caput3;
        //}
    }

    public class AlteracaoDeTexoVide
    {
        public AlteracaoDeTexoVide()
        {
            dispositivos_norma_vide = new List<DispositivoVide>();
            dispositivos_norma_vide_outra = new List<DispositivoVide>();
        }
        public string ds_dispositivos_alterados { get; set; }
        public List<DispositivoVide> dispositivos_norma_vide { get; set; }
        public List<DispositivoVide> dispositivos_norma_vide_outra { get; set; }
    }

    public class DispositivoVide
    {
        public string linkname { get; set; }
        public string nm_linkname { get; set; }
        public string ds_linkname { get; set; }
        public string texto { get; set; }
    }

    public class UtilVides
    {
        public static string EscapeCharsInToPattern(string text)
        {
            var aChars = new string[] { "(", ")", "$", ".", "?", "=" };
            foreach (var aChar in aChars)
            {
                text = text.Replace(aChar, "\\" + aChar);
            }
            return text;
        }

        public static string gerarDescricaoDoCaput(string _caput)
        {

            var caput_splited = _caput.Split('_');
            var last_caput = caput_splited.Last();
            if (last_caput.IndexOf("art") == 0)
            {
                _caput = "Artigo ";
            }
            else if (last_caput.IndexOf("par") == 0)
            {
                _caput = "Parágrafo ";
            }
            else if (last_caput.IndexOf("inc") == 0)
            {
                _caput = "Inciso ";
            }
            else if (last_caput.IndexOf("ali") == 0)
            {
                _caput = "Alínea ";
            }
            else if (last_caput.IndexOf("let") == 0)
            {
                _caput = "Alínea ";
            }
            else
            {
                _caput = "";
            }
            return _caput;
        }

        public static string gerarDescricaoDoTexto(string texto)
        {
            var caput = "";
            texto = texto.Trim().ToUpper();
            if (texto.IndexOf(' ') >= 0)
            {
                caput = texto.Substring(0, texto.IndexOf(' '));
            }
            if (caput == "ART" || caput == "ART.")
            {
                caput = "Artigo ";
            }
            else if (caput == "PARAGRAFO" || caput == "PARÁGRAFO" || caput == "§")
            {
                caput = "Parágrafo ";
            }
            else if (ehInciso(caput))
            {
                caput = "Inciso ";
            }
            else if (ehAlinea(caput))
            {
                caput = "Alínea ";
            }
            else
            {
                caput = "";
            }
            return caput;
        }

        public static string getRelacaoParaTextoAlterador(string relacao, bool regexExcluir = false)
        {
            string ds = relacao;

            var relacaoSplited = relacao.Split(' ');
            if (!(relacaoSplited[0].Equals("veto", StringComparison.InvariantCultureIgnoreCase) || relacaoSplited[0].Equals("texto", StringComparison.InvariantCultureIgnoreCase) || relacaoSplited[0].Equals("denominação", StringComparison.InvariantCultureIgnoreCase) || relacaoSplited[0].Equals("legislação", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (relacaoSplited[0].EndsWith("o"))
                {
                    relacaoSplited[0] += regexExcluir ? ".*?" : "(a)";
                }
                else if (relacaoSplited[0].EndsWith("a"))
                {
                    relacaoSplited[0] += regexExcluir ? ".*?" : "(o)";
                }
                ds = string.Join<string>(" ", relacaoSplited);
            }

            return ds;
        }

        public static bool ehInciso(string termo)
        {
            var lastIndex = termo.IndexOf("-");
            if (lastIndex > 0)
            {
                termo = termo.Substring(0, lastIndex);
            }
            return Regex.IsMatch(termo, "^[IVXLCDM]+$", RegexOptions.IgnoreCase);
        }

        public static bool ehAlinea(string termo)
        {
            var lastIndex = termo.IndexOf(")");
            if (lastIndex < 0)
            {
                return false;
            }
            if (termo.Length == (lastIndex + 1))
            {
                termo = termo.Substring(0, lastIndex);
                return Regex.IsMatch(termo, "^[a-z]+$", RegexOptions.IgnoreCase);
            }
            return false;
        }

        public static bool ehNum(string termo)
        {
            var lastIndex = termo.IndexOf(".");
            if (lastIndex < 0)
            {
                return false;
            }
            if (termo.Length == (lastIndex + 1))
            {
                termo = termo.Substring(0, lastIndex);
                int iTermo;
                return int.TryParse(termo, out iTermo);
            }
            return false;
        }

        public static bool possuiDispositivo(Caput dispositivo)
        {
            return dispositivo != null && dispositivo.caput != null && dispositivo.caput.Length > 0;
        }

        public static bool ehDiferente(Caput a, Caput b)
        {
            var ehDiferente = false;
            if (a.caput.Length != b.caput.Length)
            {
                ehDiferente = true;
            }
            else if (a.link != b.link)
            {
                ehDiferente = true;
            }
            else if (a.nm_relacao_aux != b.nm_relacao_aux)
            {
                ehDiferente = true;
            }
            else if (a.texto_novo != null)
            {
                for (var i = 0; i < a.caput.Length; i++)
                {
                    if (a.caput[i] != b.caput[i] || a.texto_novo[i] != b.texto_novo[i])
                    {
                        ehDiferente = true;
                        break;
                    }
                }
            }
            else
            {
                for (var i = 0; i < a.caput.Length; i++)
                {
                    if (a.caput[i] != b.caput[i])
                    {
                        ehDiferente = true;
                        break;
                    }
                }
            }
            return ehDiferente;
        }

        /// <summary>
        /// Verifica, com base na situação e na descrição relação de vide, se o vide implica em alteração no texto completo da norma, e não somente em um dispositivo
        /// </summary>
        /// <param name="nmSituacaoAlterada"></param>
        /// <param name="dsTextoParaAlterador"></param>
        /// <returns></returns>

        /// Para hanchura completa
        public static bool EhAlteracaoCompleta(string nmSituacaoAlterada, string dsTextoParaAlterador)
        {
            //int a = 1;

            //return (nmSituacaoAlterada == "revogado" && dsTextoParaAlterador == "revogado") ||
                   //(nmSituacaoAlterada == "anulado" && dsTextoParaAlterador == "anulado") ||
                   //(nmSituacaoAlterada == "extinta" && dsTextoParaAlterador == "extinta") ||
                   //(nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado inconstitucional") ||
                   //(nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado(a) inconstitucional") ||
                   //(nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "julgada procedente") ||
                   //(nmSituacaoAlterada == "cancelada" && dsTextoParaAlterador == "cancelada") ||
                   //(nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso(a) liminarmente") ||
                   //(nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso totalmente") ||
                   //(nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso") ||
                   //(nmSituacaoAlterada == "sustado" && dsTextoParaAlterador == "sustado");
            //bool maria = (nmSituacaoAlterada == "revogado" && dsTextoParaAlterador == "revogado") ||
            //        (nmSituacaoAlterada == "anulado" && dsTextoParaAlterador == "anulado") ||
            //        (nmSituacaoAlterada == "extinta" && dsTextoParaAlterador == "extinta") ||
            //        (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado inconstitucional") ||
            //        (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado(a) inconstitucional") ||
            //        (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "julgada procedente") ||
            //        (nmSituacaoAlterada == "cancelada" && dsTextoParaAlterador == "cancelada") ||
            //        (nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso(a) liminarmente") ||
            //        (nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso totalmente") ||
            //        (nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso") ||
            //        (nmSituacaoAlterada == "sustado" && dsTextoParaAlterador == "sustado");
            //return maria;
             return (nmSituacaoAlterada == "revogado" && dsTextoParaAlterador == "revogado") ||
                    (dsTextoParaAlterador == "suspenso") || // Nota: linha alterada 
                    (nmSituacaoAlterada == "anulado" && dsTextoParaAlterador == "anulado") ||
                    (nmSituacaoAlterada == "extinta" && dsTextoParaAlterador == "extinta") ||
                    (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado inconstitucional") ||
                    (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado(a) inconstitucional") ||
                    (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "julgada procedente") ||
                    (nmSituacaoAlterada == "cancelada" && dsTextoParaAlterador == "cancelada") ||
                    (nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso(a) liminarmente") ||
                    //(nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso totalmente") ||
                    //(nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso") ||
                    (nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso(a) totalmente") || //Nota: Paramentros adicionados para 'suspenso totalmente'
                    (nmSituacaoAlterada == "sustado" && dsTextoParaAlterador == "sustado");
                     

        }

        /// <summary>
        /// Verifica, com base na situação e na descrição relação de vide, se o vide implica somente em linkar as duas normas, não necessariamente precisa ser legislação correlata
        /// </summary>
        /// <param name="dsTextoParaAlterador"></param>
        /// <returns></returns>

        //Adicionar legislação correlata
        public static bool EhLegislacaoCorrelata(string dsTextoParaAlterador)
        {
            return dsTextoParaAlterador == "ratificado" ||
                   dsTextoParaAlterador == "reeditado" ||
                   dsTextoParaAlterador == "regulamentado" ||
                   dsTextoParaAlterador == "prorrogado" ||
                   dsTextoParaAlterador == "legislação correlata";
                   // ||
                   //dsTextoParaAlterador == "suspenso(a) liminarmente";
        }
    }

}
