using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class __OrgaoAD : AD
    {
        private string _extentOrgao;

        public string ExtentOrgao
        {
            get { return _extentOrgao; }
        }

        public __OrgaoAD()
        {
            //_extentOrgao = Configuracao.LerValorChave(chaveBaseSinjOrgaos);
        }

        public void BuscarOrgaosEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo Orgaos...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<OrgaoSinj> orgaos = new List<OrgaoSinj>();
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                using (var reader = conn.ExecuteDataReader(sql))
                {
                    EsAD indexa = new EsAD();
                    List<string> idsControle = new List<string>();
                    List<string> todosIdsSucess = new List<string>();
                    List<string> idsError = new List<string>();
                    total = reader.Count;

                    while (reader.Read())
                    {
                        i++;
                        j++;
                        try
                        {
                            idsControle.Add(reader["Id"].ToString()); //Pega todos os IdS
                            OrgaoSinj orgao = new OrgaoSinj();
                            orgao.Id = Convert.ToInt32(reader["Id"]);
                            orgao.IdString = Convert.ToString(reader["Id"]);
                            orgao.Descricao = Convert.ToString(reader["Descricao"]);
                            orgao.Sigla = Convert.ToString(reader["Sigla"]).Replace("/", ",");
                            orgao.CodigoAssociacao = Convert.ToString(reader["CodigoAssociacao"]);
                            orgao.DtVigencia = Convert.ToString(reader["DtVigencia"]).Replace("00:00:00", "");
                            orgao.DtFimVigencia = Convert.ToString(reader["DtFimDeVigencia"]).Replace("00:00:00", "");
                            string valorCodigo = reader["Codigo"].ToString();
                            if (!string.IsNullOrEmpty(valorCodigo))
                            {
                                orgao.Codigo = valorCodigo;
                            }
                            string valorCodigoOrgaoPai = reader["CodigoOrgaoPai"].ToString();
                            if (!string.IsNullOrEmpty(valorCodigoOrgaoPai)) orgao.CodigoOrgaoPai = valorCodigoOrgaoPai;
                            orgao.OrgaoPaI = ObtemOrgao(orgao.CodigoOrgaoPai);
                            orgao.Ambito = reader["Ambito"].ToString();
                            orgao.Status = reader["Status"].ToString();
                            orgao.OrgaoCadastrador = ObtemOrgaoCadastrador(reader);
                            orgaos.Add(orgao);
                            Console.WriteLine("----------> Orgao montado: " + orgao.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentOrgao, orgaos, "Id");
                            todosIdsSucess.AddRange(idsSucess);
                            i = 0;
                            //Varre todos os Ids para achar os que não foram indexados e adiciona-los à lista idsError
                            foreach (string id in idsControle)
                            {
                                if (!idsSucess.Contains(id))
                                {
                                    if (!idsError.Contains(id))
                                    {
                                        idsError.Add(id);
                                    }
                                }
                            }
                            contPesquisa += idsControle.Count;
                            contIndexacao += idsSucess.Count;
                            orgaos.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentOrgao, orgaos, "Id");
                            todosIdsSucess.AddRange(idsSucess);
                            i = 0;
                            //Varre todos os Ids para achar os que não foram indexados e adiciona-los à lista idsError
                            foreach (string id in idsControle)
                            {
                                if (!idsSucess.Contains(id))
                                {
                                    if (!idsError.Contains(id))
                                    {
                                        idsError.Add(id);
                                    }
                                }
                            }
                            contPesquisa += idsControle.Count;
                            contIndexacao += idsSucess.Count;
                            orgaos.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Orgãos");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos os orgaos... SQL: " + sql);
                Log.LogarExcecao("Exportação de Orgãos", "Erro na busca de todos os orgaos...", ex);
            }
        }

        public OrgaoSinj ObtemOrgaoPeloId(string idOrigem)
        {
            if (string.IsNullOrEmpty(idOrigem)) return null;
            OrgaoSinj orgao = new OrgaoSinj();
            try
            {
                string sql = string.Format("select * from {0} where id={1}", _extentOrgao, idOrigem);
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                using (var reader = conn.ExecuteDataReader(sql))
                {
                    if (reader.Read()) orgao = CarregaOrgao(reader);
                    reader.Close();
                }
                conn.CloseConection();
                return orgao;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private OrgaoSinj CarregaOrgao(IDataRecord reader)
        {
            //Note: mudei aqui Adicionei o atributo IdString.
            OrgaoSinj orgao = new OrgaoSinj();
            orgao.Id = Convert.ToInt32(reader["Id"]);
            orgao.IdString = Convert.ToString(reader["Id"]);
            orgao.Descricao = Convert.ToString(reader["Descricao"]);
            orgao.Sigla = Convert.ToString(reader["Sigla"]).Replace("/", ",");
            orgao.CodigoAssociacao = Convert.ToString(reader["CodigoAssociacao"]);
            orgao.DtVigencia = Convert.ToString(reader["DtVigencia"]).Replace("00:00:00", "");
            orgao.DtFimVigencia = Convert.ToString(reader["DtFimDeVigencia"]).Replace("00:00:00", "");
            string valorCodigo = reader["Codigo"].ToString();
            if (!string.IsNullOrEmpty(valorCodigo))
            {
                orgao.Codigo = valorCodigo;
            }
            string valorCodigoOrgaoPai = reader["CodigoOrgaoPai"].ToString();
            if (!string.IsNullOrEmpty(valorCodigoOrgaoPai)) orgao.CodigoOrgaoPai = valorCodigoOrgaoPai;
            orgao.OrgaoPaI = ObtemOrgao(orgao.CodigoOrgaoPai);
            orgao.Ambito = reader["Ambito"].ToString();
            orgao.Status = reader["Status"].ToString();
            orgao.OrgaoCadastrador = ObtemOrgaoCadastrador(reader);
            return orgao;
        }

        public OrgaoSinj ObtemOrgao(string codigo)
        {
            if (codigo == null) return null;
            OrgaoSinj orgao = new OrgaoSinj();
            try
            {
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                string sql = string.Format("select * from {0} where Codigo=\"{1}\"", _extentOrgao, codigo);
                using (var reader = conn.ExecuteDataReader(sql))
                {
                    if (reader.Read()) orgao = CarregaOrgao(reader);
                    reader.Close();
                }
                conn.CloseConection();
                return orgao;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ObtemOrgaoCadastrador(IDataRecord reader)
        {

            int total;
            string orgaoCadastrador = "";
            if (reader["TCDF"] is bool)
            {
                if (Convert.ToBoolean(reader["TCDF"]))
                {
                    orgaoCadastrador = "TCDF";
                }
            }
            if (reader["Seplag"] is bool)
            {
                if (Convert.ToBoolean(reader["Seplag"]))
                {
                    orgaoCadastrador = "SEPLAG";
                }
            }
            if (reader["CLDF"] is bool)
            {
                if (Convert.ToBoolean(reader["CLDF"]))
                {
                    orgaoCadastrador = "CLDF";
                }
            }
            if (reader["PGDF"] is bool)
            {
                if (Convert.ToBoolean(reader["PGDF"]))
                {
                    orgaoCadastrador = "PGDF";
                }
            }
            return orgaoCadastrador;
        }

        public void ConfigurarMapping()
        {
            string extent = _extentOrgao;
            string jsonMapping = Configuracao.LerValorChave(chaveMappingOrgaos);
            string uriElasticSearch = Configuracao.LerValorChave(chaveElasticSearch);
            if (jsonMapping != "")
            {
                new EsAD().ConfigurarIndexType(uriElasticSearch, jsonMapping, extent);
            }
        }
    }
}