using Exportador_LB_to_ES.util;
using System.Collections.Generic;
using System;
using LightInfocon.Data.LightBaseProvider;
using AcessaDadosLightBase;

namespace Exportador_LB_to_ES.AD.AD
{
    public class AD
    {
        internal const string chaveLightBaseConnectionString = "LightBaseConnectionString";
        internal const string chaveElasticSearch = "UriIndexacaoElasticSearch";
        internal const string chaveFlagExtrairTextoArquivos = "FlagExtrairTextoArquivos";
        internal const string chaveFlagSalvarTextoArquivos = "FlagSalvarTextoArquivos";
        internal const string chaveFlagDeletarArquivosZip = "FlagDeletarArquivosZip";
        internal const string chavePathOfTheUploadedFiles = "PathOfTheUploadedFiles";
        internal const string chavePathToSaveOnSFTP = "PathToSaveOnSFTP";
        internal const string chaveSFTPServer = "SFTPServer";
        internal const string chaveSFTPServerPort = "SFTPServer_Port";
        internal const string chaveSFTPServerUser = "SFTPServer_User";
        internal const string chaveSFTPServerPassword = "SFTPServer_Password";
        
        internal const string chaveCampoCaminhoArquivoTextoNormas = "CampoCaminhoArquivoDeNormas";
        internal const string chaveCampoCaminhoArquivoTextoDodfs = "CampoCaminhoArquivoDeDodfs";
        internal const string culturaPortuguesBrasileiro = "pt-br";

        internal const string chaveMappingTipos = "MappingTipos";
        internal const string chaveMappingOrgaos = "MappingOrgaos";
        internal const string chaveMappingDodfs = "MappingDodfs";
        internal const string chaveMappingNormas = "MappingNormas";
        
        internal const string chaveBaseAtualizados = "BaseForIndexerAndExporter";
        internal const string chaveBaseExcluidos = "BaseForExcluder";
        internal const string chaveBaseNormas = "BaseNormas";
        internal const string chaveBaseDodf = "BaseDodfs";
        internal const string chaveBaseTipoDeNorma = "BaseTiposDeNorma";
        //internal const string chaveBaseSinjOrgaos = "BaseSinjOrgaos";
        internal const string chaveBaseOrgaos = "BaseOrgaos";
        internal const string chaveBaseInteressado = "BaseInteressados";
        internal const string chaveBaseAutoria = "BaseSinjAutorias";
        internal const string chaveBaseRequerente = "BaseRequerentes";
        internal const string chaveBaseRequerido = "BaseRequeridos";
        internal const string chaveBaseProcuradoresResponsaveis = "BaseProcuradoresResponsaveis";
        internal const string chaveBasePush = "BasePush";
        internal const string chaveBaseTipoDeRelacao = "BaseTiposDeRelacao";
        internal const string chaveBaseVocabularioControlado = "BaseVocabularioControlado";
        internal const string chaveBaseRelacaoTermoGeralEEspecifico = "BaseRelacaoTermoGeralEEspecifico";
        internal const string chaveBaseRelacaoTermoRelacionado = "BaseRelacaoTermoRelacionado";
        internal const string chaveBaseRelacaoTermoNaoAutorizado = "BaseRelacaoTermoNaoAutorizado";

        public IList<string> BuscarTodosOsIds(string nm_base, string nm_field_id)
        {
            IList<string> ids = new List<string>();
            try
            {
                string sql = string.Format("select {0} from {1}", nm_field_id, nm_base);
                Console.WriteLine("Base: " + nm_base);
                Console.WriteLine("Iniciando Busca de todos os ids...");
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                using (LightBaseDataReader rdr = conn.ExecuteDataReader(sql))
                {
                    Console.WriteLine("Reader.Count = " + rdr.Count);
                    while (rdr.Read())
                    {
                        ids.Add(rdr[nm_field_id].ToString());
                    }
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na busca de todos ids da Base " + nm_base);
                throw new Exception("Erro na busca de todos ids da Base " + nm_base, ex);
            }
            return ids;
        }

        internal string ObterNomeAtributoId(string extent)
        {
            if (extent == Configuracao.LerValorChave(chaveBaseVocabularioControlado))
            {
                return "Id_Termo";
            }
            if (extent == Configuracao.LerValorChave(chaveBaseTipoDeRelacao))
            {
                return "Oid";
            }
            return "Id";
        }
    }
}