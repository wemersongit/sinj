using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using System.Data.SqlClient;
using SINJ_MetaMiner.AD;
using System.Data;
using util.BRLight;
using SINJ_MetaMiner.OV;

namespace SINJ_MetaMiner.RN
{
    public class SINJ_MetaMinerRN
    {
        private SINJ_MetaMinerAD _ad;

        public SINJ_MetaMinerRN()
        {
            _ad = new SINJ_MetaMinerAD();
        }


        public Results<NormaOV> BuscarNormas(Pesquisa pesquisa)
        {
            return _ad.BuscarNormas(pesquisa);
        }

        //public DataSet BuscarLexml()
        //{
        //    return _ad.BuscarLexml();
        //}

        public List<NormaLexml> ConsultarLexml()
        {
            return _ad.ConsultarLexml();
        }


        public bool ItIsToUpgrade(NormaOV norma, NormaLexml norma_lexml)
        {
            var ts_registro_gmt = Convert.ToDateTime(norma_lexml.ts_registro_gmt);
            DateTime dt_alteracao;
            bool itIsUpgrade = false;
            foreach(var alteracao in norma.alteracoes){
                dt_alteracao = Convert.ToDateTime(alteracao.dt_alteracao);
                if(dt_alteracao > ts_registro_gmt){
                    itIsUpgrade = true;
                    break;
                }
            }
            return itIsUpgrade;
        }

        public AuxMetadadoLexml GetAuxMetadadoLexml(NormaOV norma)
        {
            AuxMetadadoLexml auxMetadadoLexml = null;
            var sb_xml_tag_individual = new StringBuilder();
            var document_type = "";
            var publisher_code = "";
            sb_xml_tag_individual.Append("urn:lex:br;distrito.federal:");
            switch (norma.nm_orgao_cadastrador.ToUpper())
            {
                case "CLDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução" }))
                    {
                        sb_xml_tag_individual.Append("camara.legislativa" + ":");
                        sb_xml_tag_individual.Append("resolucao");
                        document_type = "Resolução";
                    }
                    else
                    {
                        sb_xml_tag_individual.Append("distrital" + ":");
                        if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Decreto Legislativo" }))
                        {
                            sb_xml_tag_individual.Append("decreto.legislativo");
                            document_type = "Decreto Legislativo";
                        }
                        //"emenda.lei.organica" -> "Emenda a lei Orgânica" NAO " Emenda a lei Orgânica" && "Emenda a lei Orgânica "
                        else if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Emenda a lei Orgânica" }))
                        {
                            sb_xml_tag_individual.Append("emenda.lei.organica");
                            document_type = "Emenda a lei Orgânica";
                        }
                        //"lei" -> "Lei" NAO " Lei" && "Lei "
                        else if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Lei" }))
                        {
                            sb_xml_tag_individual.Append("lei");
                            document_type = "Lei";
                        }
                        //"lei.complementar" -> "Lei Complementar" NAO " Lei Complementar" && "Lei Complementar "
                        else if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Lei Complementar" }))
                        {
                            sb_xml_tag_individual.Append("lei.complementar");
                            document_type = "Lei Complementar";
                        }
                        //"lei.organica" -> "Lei Orgânica" NAO " Lei Orgânica" && "Lei Orgânica "
                        else if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Lei Orgânica" }))
                        {
                            sb_xml_tag_individual.Append("lei.organica");
                            document_type = "Lei Orgânica";
                        }
                    }
                    publisher_code = "70";
                    break;
                case "PGDF":
                    sb_xml_tag_individual.Append("procuradoria.geral" + ":");
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução" }))
                    {
                        sb_xml_tag_individual.Append("resolucao");
                        document_type = "Resolução";
                    }
                    publisher_code = "71";
                    break;
                case "SEPLAG":
                    sb_xml_tag_individual.Append("distrital" + ":");
                    //"decreto" -> "Decreto" NAO " Decreto" && "Decreto "
                    if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Decreto" }))
                    {
                        sb_xml_tag_individual.Append("decreto");
                        document_type = "Decreto";
                    }
                    publisher_code = "72";
                    break;
                case "TCDF":
                    sb_xml_tag_individual.Append("tribunal.contas" + ":");
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução" }))
                    {
                        sb_xml_tag_individual.Append("resolucao");
                        document_type = "Resolução";
                    }
                    //"sumula" -> "Súmula" NAO " Súmula" && "Súmula "
                    else if (ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Súmula" }))
                    {
                        sb_xml_tag_individual.Append("sumula");
                        document_type = "Súmula";
                    }
                    publisher_code = "73";
                    break;
                default:
                    throw new Exception("Não foi possível localizar o nm_orgao_cadastrador da norma. NormaOV =>" + JSON.Serialize<NormaOV>(norma));
            }
            if (document_type != "")
            {
                if (string.IsNullOrEmpty(norma.nr_norma) || norma.nr_norma == "0"){
                    throw new Exception("Número da norma inválido. nr_norma: "+norma.nr_norma);
                }
                if (string.IsNullOrEmpty(norma.dt_assinatura)){
                    throw new Exception("Data de assinatura da norma inválida. dt_assinatura: "+norma.dt_assinatura);
                }
                var dt_assinatura = Convert.ToDateTime(norma.dt_assinatura).ToString("yyyy-MM-dd");
                sb_xml_tag_individual.Append(":" + dt_assinatura + ";" + norma.nr_norma);

                auxMetadadoLexml = new AuxMetadadoLexml { document_type = document_type, xml_tag_individual = sb_xml_tag_individual.ToString(), publisher_code = publisher_code };
            }
            else
            {
                throw new Exception("document_type do xml não foi setado");
            }
            return auxMetadadoLexml;
        }
        
        private bool ChecksIfContains(string nm_tipo, List<string> comparar)
        {
            for (var i = 0; i < comparar.Count; i++ )
            {
                if (nm_tipo.ToLower().Trim().IndexOf(comparar[i].ToLower()) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetTxMetadadoXml(NormaOV norma, AuxMetadadoLexml auxMetadadoLexml)
        {
            var tx_metadado_xml = new StringBuilder();

            tx_metadado_xml.AppendLine("<LexML xmlns=\"http://www.lexml.gov.br/oai_lexml\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.lexml.gov.br/oai_lexml http://projeto.lexml.gov.br/esquemas/oai_lexml.xsd\">");
            tx_metadado_xml.AppendLine("    <Item formato=\"text/html\" idPublicador=\"" + auxMetadadoLexml.publisher_code + "\" tipo=\"metadado\">");
            tx_metadado_xml.AppendLine("		http://www.sinj.df.gov.br/SINJ/DetalhesDeNorma.aspx?id_norma=" + norma.ch_norma);
            tx_metadado_xml.AppendLine("	</Item>");
            var fonte = GetFirstFonteWithFile(norma);
            if (fonte != null)
            {
                tx_metadado_xml.AppendLine("    <Item formato=\"" + fonte.ar_fonte.mimetype + "\" idPublicador=\"" + auxMetadadoLexml.publisher_code + "\" tipo=\"conteudo\">");
                tx_metadado_xml.AppendLine("		http://www.sinj.df.gov.br/SINJ/Download/" + Config.ValorChave("NmBaseNorma", true) + "/" + fonte.ar_fonte.id_file + "/" + fonte.ar_fonte.filename);
                tx_metadado_xml.AppendLine("	</Item>");
            }
            tx_metadado_xml.AppendLine("	<DocumentoIndividual>" + auxMetadadoLexml.xml_tag_individual + "</DocumentoIndividual>");
            tx_metadado_xml.AppendLine("	<Epigrafe>" + auxMetadadoLexml.document_type + " nº " + norma.nr_norma.ToString() + ", de " + Convert.ToDateTime(norma.dt_assinatura).ToString("dd' de 'MMMMM' de 'yyyy") + "</Epigrafe>"); //Ex.: Lei nº 8.078, de 11 de setembro de 1990

            if (!string.IsNullOrEmpty(norma.nm_apelido))
            {
                tx_metadado_xml.AppendLine("	<Apelido>" + norma.nm_apelido + "</Apelido>");
            }

            tx_metadado_xml.AppendLine("	<Ementa>");
            tx_metadado_xml.AppendLine("		" + norma.ds_ementa);
            tx_metadado_xml.AppendLine("	</Ementa>");

            if (norma.indexacoes != null && norma.indexacoes.Count > 0)
            {
                var indexacoes = "";
                foreach (var indexacao in norma.indexacoes)
                {
                    var termos = "";
                    foreach (var termo in indexacao.vocabulario)
                    {
                        termos += (termos != "" ? "," : "") + termo.nm_termo;
                    }
                    indexacoes += (indexacoes != "" ? ". " : "") + termos;
                }
                indexacoes += ".";
                tx_metadado_xml.AppendLine("	<Indexacao>" + indexacoes + "</Indexacao>");
            }

            tx_metadado_xml.AppendLine("</LexML>");

            return tx_metadado_xml.ToString();
        }

        public string GetTxMetadadoXmlTxtAtlzdo(NormaOV norma, AuxMetadadoLexml auxMetadadoLexml)
        {
            var tx_metadado_xml = new StringBuilder();
            tx_metadado_xml.AppendLine("<LexML xmlns=\"http://www.lexml.gov.br/oai_lexml\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.lexml.gov.br/oai_lexml http://projeto.lexml.gov.br/esquemas/oai_lexml.xsd\">");

            tx_metadado_xml.AppendLine("    <Item formato=\"" + norma.ar_atualizado.mimetype + "\" idPublicador=\"" + auxMetadadoLexml.publisher_code + "\" tipo=\"conteudo\">");
            tx_metadado_xml.AppendLine("		http://www.sinj.df.gov.br/SINJ/Norma/" + norma.ch_norma + "/" + norma.getNameFileArquivoVigente());
            tx_metadado_xml.AppendLine("	</Item>");

            tx_metadado_xml.AppendLine("	<DocumentoIndividual>" + auxMetadadoLexml.xml_tag_individual + "@multivigente;texto.atualizado~texto;pt-br</DocumentoIndividual>");
            tx_metadado_xml.AppendLine("</LexML>");
            return tx_metadado_xml.ToString();
        }

        public void ValidarAuxMetadadoLexml(NormaOV norma)
        {
            if (string.IsNullOrEmpty(norma.nm_orgao_cadastrador))
            {
                throw new Exception("nm_orgao_cadastrador está em nulo ou em branco");
            }
            switch (norma.nm_orgao_cadastrador.ToUpper())
            {
                case "CLDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução", "Decreto Legislativo", "Emenda a lei Orgânica", "Lei", "Lei Complementar", "Lei Orgânica" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                case "PGDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                case "SEPLAG":
                    //"decreto" -> "Decreto" NAO " Decreto" && "Decreto "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Decreto" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                case "TCDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução", "Súmula" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                default:
                    throw new Exception("Não foi possível localizar o nm_orgao_cadastrador da norma.");
            }
        }

        public void ValidarTxMetadadoXml(NormaOV norma)
        {
            if (string.IsNullOrEmpty(norma.ds_ementa))
            {
                throw new Exception("ds_ementa está em nulo ou em branco");
            }
            if (string.IsNullOrEmpty(norma.dt_assinatura))
            {
                throw new Exception("dt_assinatura está em nulo ou em branco");
            }
            if (norma.nr_norma == "0" || string.IsNullOrEmpty(norma.nr_norma))
            {
                throw new Exception("nr_norma vazio ou igual a zero");
            }
            if (GetFirstFonteWithFile(norma) == null)
            {
                throw new Exception("Fonte igual a null ou sem arquivo");
            }
        }

        public void ValidarTxMetadadoXmlTxtAtlzdo(NormaOV norma)
        {
            if (norma.ar_atualizado == null || string.IsNullOrEmpty(norma.ar_atualizado.id_file) || string.IsNullOrEmpty(norma.ar_atualizado.mimetype))
            {
                throw new Exception("norma sem ar_atualizado");
            }
        }

        public void ValidarCamposDaNorma(NormaOV norma, bool bTextAtlzdo)
        {
            if (string.IsNullOrEmpty(norma.nm_orgao_cadastrador))
            {
                throw new Exception("nm_orgao_cadastrador está em nulo ou em branco");
            }
            switch (norma.nm_orgao_cadastrador.ToUpper())
            {
                case "CLDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução", "Decreto Legislativo", "Emenda a lei Orgânica", "Lei", "Lei Complementar", "Lei Orgânica" }))
                    {
                        throw new Exception("O Tipo de norma ("+norma.nm_tipo_norma+") do órgão ("+norma.nm_orgao_cadastrador+") não deve ser inserido no lexml.");
                    }
                    break;
                case "PGDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                case "SEPLAG":
                    //"decreto" -> "Decreto" NAO " Decreto" && "Decreto "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Decreto" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                case "TCDF":
                    //"resolucao" -> "Resolução" NAO " Resolução" && "Resolução "
                    if (!ChecksIfContains(norma.nm_tipo_norma, new List<string> { "Resolução", "Súmula" }))
                    {
                        throw new Exception("O Tipo de norma (" + norma.nm_tipo_norma + ") do órgão (" + norma.nm_orgao_cadastrador + ") não deve ser inserido no lexml.");
                    }
                    break;
                default:
                    throw new Exception("Não foi possível localizar o nm_orgao_cadastrador da norma.");
            }
            if (string.IsNullOrEmpty(norma.ds_ementa) && !bTextAtlzdo)
            {
                throw new Exception("ds_ementa está em nulo ou em branco");
            }
            if (string.IsNullOrEmpty(norma.dt_assinatura) && !bTextAtlzdo)
            {
                throw new Exception("dt_assinatura está em nulo ou em branco");
            }
            if ((norma.nr_norma == "0" || string.IsNullOrEmpty(norma.nr_norma)) && !bTextAtlzdo)
            {
                throw new Exception("nr_norma igual a zero");
            }
            if (GetFirstFonteWithFile(norma) == null && !bTextAtlzdo)
            {
                throw new Exception("Fonte igual a null ou sem arquivo");
            }
            if (bTextAtlzdo && (norma.ar_atualizado == null || string.IsNullOrEmpty(norma.ar_atualizado.id_file) || string.IsNullOrEmpty(norma.ar_atualizado.mimetype)))
            {
                throw new Exception("norma sem ar_atualizado");
            }
        }

        public Fonte GetFirstFonteWithFile(NormaOV norma)
        {
            Fonte fonte = null;
            if (norma.fontes != null || norma.fontes.Count > 0)
            {
                foreach (var _fonte in norma.fontes)
                {
                    if (!string.IsNullOrEmpty(_fonte.ar_fonte.id_file) && !string.IsNullOrEmpty(_fonte.ar_fonte.mimetype))
                    {
                        fonte = _fonte;
                        break;
                    }
                }
            }
            return fonte;
        }

        public string GerarIdLexml(NormaOV norma)
        {
            return "oai:" + norma.nm_orgao_cadastrador.ToLower() + ".tc.df.gov.br:sinj/" + norma.ch_norma;
        }

        //public void AtualizarLexml(DataSet dataset_lexml)
        //{
        //    _ad.AtualizarLexml(dataset_lexml);
        //}

        public int InserirDoc(NormaLexml norma_lexml)
        {
            return _ad.InserirDoc(norma_lexml);
        }

        public int AtualizarDoc(string id_registro_item, NormaLexml norma_lexml)
        {
            return _ad.AtualizarDoc(id_registro_item, norma_lexml);
        }
    }
}
