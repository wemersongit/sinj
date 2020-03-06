using System;
using System.Linq;
using System.Xml.Linq;

namespace Sinj.Notifica.util
{
    public class Configuracao
    {
        public static string NomeDoExtentDeAmbiente
        {
            get { return ValorChave("extentAmbiente"); }
        }
        public static string LightBaseConnection
        {
            get { return ValorChave("LightBaseConnectionString"); }
        }
        public static string NomeDoExtentDeDodfs
        {
            get { return ValorChave("extentDodfs"); }
        }
        public static string NomeDoExtentDeOrgaos
        {
            get { return ValorChave("extentSinjOrgaos"); }
        }
        public static string NomeDoExtentDeRequeridos
        {
            get { return ValorChave("extendRequeridos"); }
        }
        public static string NomeDoExtentDeProcuradoresResponsaveiss
        {
            get { return ValorChave("extentProcuradoresResponsaveis"); }
        }
        public static string NomeDoExtentDeInteressados
        {
            get { return ValorChave("extentInteressados"); }
        }
        public static string NomeDoExtentDeRequerentes
        {
            get { return ValorChave("extentRequerentes"); }
        }
        public static string NomeDoExtentDeNormas
        {
            get { return ValorChave("extentNormas"); }
        }
        public static string NomeDoExtentDeDescritores
        {
            get { return ValorChave("extentVocabularioControladoSINJDescritores"); }
        }
        public static string NomeDoExtentDeEspecificadores
        {
            get { return ValorChave("extentVocabularioControladoSINJEspecificadores"); }
        }
        public static string NomeDoExtentDeTermosRelacionadosDescritor
        {
            get { return ValorChave("extentVocabularioControladoSINJRelacionamentosTermosRelacionadosDescritor"); }
        }
        public static string NomeDoExtentDeTermosNaoAutorizadosDescritor
        {
            get { return ValorChave("extentVocabularioControladoSINJRelacionamentosTermosNaoAutorizadosDescritor"); }
        }
        public static string NomeDoExtentDeTermosEspecificosDescritor
        {
            get { return ValorChave("extentVocabularioControladoSINJRelacionamentosTermosEspecificosDescritor"); }
        }
        public static string NomeDoExtentDeDescritoresEspecificador
        {
            get { return ValorChave("extentVocabularioControladoSINJRelacionamentosDescritoresEspecificor"); }
        }
        public static string NomeDoExtentDeTiposDeRelacao
        {
            get { return ValorChave("extentDeTiposDeRelacao"); }
        }
        public static string NomeDoExtentDeTiposDeNorma
        {
            get { return ValorChave("extentTiposDeNorma"); }
        }
        public static string NomeDoExtentDePush
        {
            get { return ValorChave("extentPush"); }
        }
        public static string NomeDoExtentDeCampoCaminhoArquivoDeNormas
        {
            get { return ValorChave("extentCampoCaminhoArquivoDeNormas"); }
        }
        public static string NomeDoExtentDeCampoCaminhoArquivoDeDodfs
        {
            get { return ValorChave("extentCampoCaminhoArquivoDeDodfs"); }
        }

        public static string NomeDoExtentEmailFrom
        {
            get { return ValorChave("extentEmailFrom"); }
        }

        public static string NomeDoExtentEmailSenha
        {
            get { return ValorChave("extentEmailSenha"); }
        }

        public static string NomeDoExtentEmailPorta
        {
            get { return ValorChave("extentEmailPorta"); }
        }

        public static string NomeDoExtentEnableSsl
        {
            get { return ValorChave("extentEnableSsl"); }
        }

        public static string NomeDoExtentEnableCredentials
        {
            get { return ValorChave("extentEnableCredentials"); }
        }

        public static string NomeDoExtentServidorDeEmail
        {
            get { return ValorChave("extentServidorDeEmail"); }
        }

        public static string NomeDoExtentLinkImagemEmailTopo
        {
            get { return ValorChave("extentLinkImagemEmailTopo"); }
        }

        public static string NomeDoExtentLinkImagemEmailRodape
        {
            get { return ValorChave("extentLinkImagemEmailRodape"); }
        }

        public static string ValorChave(string sChave)
        {
            return GetValueFromXml(sChave);
        }

        public static string GetValueFromXml(string chave)
        {
            XElement xml = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + @"config.xml");
            var element = from x in xml.Elements(chave) select x.Element("value");
            return element.First().Value;
        }
    }
}