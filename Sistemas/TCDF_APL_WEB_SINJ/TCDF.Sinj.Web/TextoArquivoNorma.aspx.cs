using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.UI.HtmlControls;

namespace TCDF.Sinj.Web
{
    public partial class TextoArquivoNorma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var _id_file = Request["id_file"];
            var _highlight = Request["highlight"]; // Objeto serializado que vem pelo form post de ResultadoDePesquisa
            var lista_highlight = new List<string>(); // lista que armazena as palavras que deverao estar destacadas
            try
            {
                if (!string.IsNullOrEmpty(_id_file))
                {
                    Util.rejeitarInject(_id_file);
                    var normaRn = new NormaRN();
                    var json_doc = normaRn.GetDoc(_id_file);

                    if (json_doc.IndexOf("\"status\": 500") > -1)
                    {
                        throw new Exception("Erro ao obter texto do arquivo.");
                    }
                    if (json_doc.IndexOf("\"status\": 404") > -1)
                    {
                        throw new Exception("Arquivo não encontrado.");
                    }

                    if (json_doc.IndexOf("\"filetext\": null") > -1)
                    {
                        throw new Exception("O texto do arquivo não foi extraído.");
                    }
                    if (!string.IsNullOrEmpty(_highlight)) 
                    {
                        // var pattern eh o padrao pelo qual sera criado a Regex
                        // (.*?) significa:
                        // . -> Coincide com qualquer caracter, exceto \n
                        // * -> Faz com o que o caracter precedente (no caso, "." [ponto] ) coincida zero ou mais vezes. 
                        // ? -> Torna a expressao em 'nao-gananciosa' (non-greedy). Isso significa que vai coincidir com qualquer parte da string que satisfaça o requisito.
                        //      Do contrário, buscaria a maior seleçao possível. Ou seja, iria do primeiro _pre_tag_highlight ate o ultimo _post_tag_highlight.
                        var pattern = @"_pre_tag_highlight_(.*?)_post_tag_highlight_";
                        var regex = new Regex(pattern);
                        Match m = regex.Match(_highlight); // Eh a primeira coincidencia da string com o padrao.
                        while (m.Success) // Booleano que define se houve coincidencia ou nao.
                        {
                            // Cada coincidencia (match) tem um ou mais grupos.
                            // Nesse caso, o primeiro grupo (indice 0) eh toda a parte da string que coincidiu. Ex: _pre_tag_highlight_Tribunal_post_tag_highlight_
                            // O segundo grupo eh a parte que coincidiu com o grupo definido no padrao (.*?) . Ex: Tribunal
                            lista_highlight.Add(m.Groups[1].ToString());  // Adiciona apenas a palavra na lista
                            m = m.NextMatch(); // Passa para o proximo match
                        }
                    }
                    var doc_full = JSON.Deserializa<ArquivoFullOV>(json_doc);
                    if (doc_full.mimetype.IndexOf("/htm") > -1)
                    {
                        var texto = Regex.Replace(doc_full.filetext, "\\<[^\\>]*\\>", string.Empty);
                        foreach (var palavra_highlight in lista_highlight) // Percorre a lista de palavras que devem ser destacadas
                        {
                            // Substitui as palavras no texto original (precedidas e seguidas por espaço em branco, para nao destacar parte da palavra)
                            texto = texto.Replace(" " + palavra_highlight + " ", " <span class='highlight'>"+palavra_highlight+"</span> "); 
                        }
                        div_texto.InnerHtml = texto; // Escreve na div como InnerHtml para entender as tags, e nao como InnerText
                    }
                    else
                    {
                        var texto = doc_full.filetext;
                        foreach (var palavra_highlight in lista_highlight)
                        {
                            texto = texto.Replace(" " + palavra_highlight + " ", " <span class='highlight'>" + palavra_highlight + "</span> ");
                        }
                        div_texto.InnerHtml = texto;
                    }
                }
            }
            catch (ParamDangerousException)
            {
                div_texto.InnerHtml = "Erro ao obter arquivo.<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.";
                a_print.Visible = false;
            }
            catch (Exception Ex)
            {
                div_texto.InnerHtml = util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.";
                a_print.Visible = false;
            }
        }
    }
}