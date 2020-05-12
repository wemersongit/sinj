using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace util.BRLight
{
    /// <summary>
    /// Classe responsável por manipular documentos XML de forma mais produtiva e com maior performance.
    /// </summary>
    public class XMLUtil
    {
        /*
         * Para maiores detalhes sobre pesquisa de documentos XML via XPath:
         * http://www.microsoft.com/brasil/msdn/Tecnologias/visualc/XPath.mspx
         */

        /// <summary>
        /// Consulta o valor dos nodos XML do documento especificado, por meio da estrutura de pesquisa XPath.
        /// </summary>
        /// <param name="documentoXML">Documento XML.</param>
        /// <param name="comandoPesquisaXPath">Comando de pesquisa XPath.</param>
        /// <returns>
        /// Array com o valor do cada elemento (na ordem com que os elementos aparecem no documento) encontrado na pesquisa. 
        /// </returns>
        public static string[] ConsultarXMLValores(XmlDocument documentoXML, string comandoPesquisaXPath)
        {
            // Seleciona os nodos de pesquisa de acordo com o filtro XPath informado.
            XmlNodeList nodeList = documentoXML.SelectNodes(comandoPesquisaXPath);
            string[] resultadoPesquisa = null;
            
            // Verifica se a pesquisa encontrou algum nó.
            if(nodeList.Count > 0)
            {
                // Cria o array com o tamanho certo para quantidade de nós encontrados.
                resultadoPesquisa = new string[nodeList.Count];
                
                // Varre os nós e preenche o array com seus valores.
                for(int i = 0; i < nodeList.Count; i++)
                {
                    // Verifica a existência de nós filhos.
                    if(nodeList.Item(i).HasChildNodes)
                    {
                        // Varre o conteúdo dos nós filhos e adiciona seus valores.
                        for(int j = 0; j < nodeList.Item(i).ChildNodes.Count; j++)
                        {
                            resultadoPesquisa[i] = string.Concat(resultadoPesquisa[i], nodeList.Item(i).ChildNodes.Item(j).InnerText);
                        }
                    }
                }
            }

            // Retorna os valores encontrados nos nodos especificados.
            return resultadoPesquisa;
        }

        /// <summary>
        /// Consulta o valor dos nós XML do documento especificado, por meio da estrutura de pesquisa XPath.
        /// </summary>
        /// <param name="documentoXML">Documento XML.</param>
        /// <param name="comandoPesquisaXPath">Comando de pesquisa XPath.</param>
        /// <param name="atributo">Atributo do nó que se deseja filtrar.</param>
        /// <param name="filtro">
        /// Valor do filtro de pesquisa que vai ser realizada dentro do atributo informado. 
        /// Serão retornados apenas os nós que possuírem o valor informado neste parâmetro.
        /// </param>
        /// <returns>
        /// Array com o valor do cada elemento (na ordem com que os elementos aparecem no documento) encontrado na pesquisa. 
        /// </returns>
        public static string[] ConsultarXMLValoresFiltro(XmlDocument documentoXML, string comandoPesquisaXPath, string atributo, string filtro)
        {
            // Formata a string de pesquisa XPath.
            string pesquisaFiltro = comandoPesquisaXPath + @"[ @" + atributo + "='" + filtro + @"' ]";
            
            // Seleciona os nodos de pesquisa de acordo com o filtro XPath informado.
            XmlNodeList nodeList = documentoXML.SelectNodes(pesquisaFiltro);
            string[] resultadoPesquisa = null;

            // Verifica se a pesquisa encontrou algum nó.
            if(nodeList.Count > 0)
            {
                // Cria o array com o tamanho certo para quantidade de nós encontrados.
                resultadoPesquisa = new string[nodeList.Count];
                
                // Varre os nós e preenche o array com seus valores.
                for(int i = 0; i < nodeList.Count; i++)
                {
                    // Verifica a existência de nós filhos.
                    if(nodeList.Item(i).HasChildNodes)
                    {
                        // Varre o conteúdo dos nós filhos e adiciona seus valores.
                        for(int j = 0; j < nodeList.Item(i).ChildNodes.Count; j++)
                        {
                            resultadoPesquisa[i] += nodeList.Item(i).ChildNodes.Item(j).InnerText;
                        }
                    }
                }
            }

            return resultadoPesquisa;
        }

        public static string CriaXml(DataTable dt)
        {
            var xml = new StringBuilder();
            if (dt != null)
                foreach (DataRow dr in dt.Rows)
                    foreach (DataColumn coluna in dt.Columns)
                        xml.AppendFormat("<{0} name=\"{1}\">{2}</{3}>", ManipulaTexto.RetiraCaracteresEspeciais(coluna.ColumnName, false).Replace(" ", "_"), coluna.ColumnName, dr[coluna.ColumnName], ManipulaTexto.RetiraCaracteresEspeciais(coluna.ColumnName, false).Replace(" ", "_"));

            return string.Concat("<CONTEUDO>", xml, "</CONTEUDO>");

        }

        private static string RetornaXmlFormatado(string xml)
        {
            var xmlFormat = new StringBuilder();

            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                var ws = new XmlWriterSettings {Indent = true};
                using (XmlWriter.Create(xmlFormat, ws))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
                            xmlFormat.AppendFormat("<p>{0}{1}</p>", reader.GetAttribute(0), reader.GetAttribute(1));
                    }
                }
            }

            return xmlFormat.ToString();
        }
    }
}