using System;
using System.Collections.Generic;
using System.Linq;
using AcessaDadosElasticSearch;
using AcessaDadosElasticSearch.Objetos;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class ExcluidoAD : AD
    {
        public int DespacharDeletadosParaDelecao(List<RegistrosParaDeletar> lista)
        {
            List<string> extents = new List<string>();
            extents.Add(Configuracao.LerValorChave(chaveBaseNormas));
            extents.Add(Configuracao.LerValorChave(chaveBaseDodf));
            extents.Add(Configuracao.LerValorChave(chaveBaseTipoDeNorma));
            extents.Add(Configuracao.LerValorChave(chaveBaseOrgaos));
            extents.Add(Configuracao.LerValorChave(chaveBaseInteressado));
            extents.Add(Configuracao.LerValorChave(chaveBaseRequerente));
            extents.Add(Configuracao.LerValorChave(chaveBaseRequerido));
            extents.Add(Configuracao.LerValorChave(chaveBasePush));
            extents.Add(Configuracao.LerValorChave(chaveBaseTipoDeRelacao));
            extents.Add(Configuracao.LerValorChave(chaveBaseVocabularioControlado));
            return DespacharDeletadosParaDelecao(lista, extents);
        }

        private int DespacharDeletadosParaDelecao(IEnumerable<RegistrosParaDeletar> lista, IEnumerable<string> lExtents)
        {
            int i = 0;
            string uriElasticSearch = Configuracao.LerValorChave(chaveElasticSearch);
            foreach (var sExtent in lExtents)
            {
                var extent = sExtent;
                IEnumerable<RegistrosParaDeletar> iedeletar = from l in lista where l.Nome == extent select l;
                Log.LogarInformacao("Deletando do ES no type " + extent, "Total de docs para deletar: " + iedeletar.Count());
                if (iedeletar.Count() > 0)
                {
                    List<string> idsSucesso = new List<string>();
                    List<string> idsErro = new List<string>();
                    EsAD indexaRegistros = new EsAD();
                    foreach (var deletar in iedeletar)
                    {
                        string uri = string.Format("{0}{1}/{2}", uriElasticSearch, extent, deletar.Id);
                        if (indexaRegistros.DelatarNoElasticSearch(uri))
                        {
                            idsSucesso.Add(deletar.Id);
                            i++;
                        }
                        else
                        {
                            idsErro.Add(deletar.Id);
                        }
                    }
                    Log.LogarInformacao(idsSucesso, idsErro, "Deletando " + extent);
                }
            }
            return i;
        }

        public void DeletarRegistrosBaseForExcluder()
        {
            string nomeDoExtent = Configuracao.LerValorChave(chaveBaseExcluidos);
            int deletados = 0;
            try
            {
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                string sqlDelete = string.Format("delete from {0}", nomeDoExtent);
                deletados = conn.ExecuteNonQuery(sqlDelete);
            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Excluindo Registros", "Erro deletando os registros de " + nomeDoExtent, ex);
            }
            Console.WriteLine("----------> Registros deletados na base " + nomeDoExtent + " montado: " + deletados);
        }

        public List<RegistrosParaDeletar> BuscarExcluidos(string sql)
        {
            List<RegistrosParaDeletar> lista = new List<RegistrosParaDeletar>();
            try
            {
                int total = 0;
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                using (var reader = conn.ExecuteDataReader(sql))
                {
                    total = reader.Count;
                    while (reader.Read())
                    {
                        try
                        {
                            RegistrosParaDeletar registro = new RegistrosParaDeletar();
                            registro.Id = reader["Identificador"].ToString();
                            registro.Nome = reader["Nome"].ToString();
                            registro.DataDeletado = reader["DataDeletado"].ToString();
                            lista.Add(registro);
                            Console.WriteLine("----------> Deletado montado: " + registro.Nome + " " + registro.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro na busca de Excluídos...");
                            //Se der bronca guarda o Id para catalogar o Id que deram erro
                        }
                    }
                }
                Console.WriteLine("------ Total de registros recuperados da Base de Excluídos: " + total);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na busca de Excluídos...");
                Log.LogarExcecao("Exclusão de Registros", "Erro na busca de Excluídos...", ex);
            }
            return lista;
        }

        public List<RegistrosParaDeletar> BuscarDeletadosComparandoLbwComElasticSearch()
        {
            List<RegistrosParaDeletar> lista = new List<RegistrosParaDeletar>();
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseNormas)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseDodf)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseTipoDeNorma)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseOrgaos)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseInteressado)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseRequerente)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseRequerido)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBasePush)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseTipoDeRelacao)));
            lista.AddRange(BuscarDeletadosComparandoLbwComElasticSearch(Configuracao.LerValorChave(chaveBaseVocabularioControlado)));
            return lista;
        }

        private IEnumerable<RegistrosParaDeletar> BuscarDeletadosComparandoLbwComElasticSearch(string extent)
        {
            Console.WriteLine("Verificando " + extent + ":");
            List<RegistrosParaDeletar> registrosParaDeletar = new List<RegistrosParaDeletar>();
            try{
                List<string> idsElasticSearch = BuscarIdsDosRegistrosNoElasticSearch(extent);
                Console.WriteLine("            " + idsElasticSearch.Count + " registros recuperados no SINJ PORTAL.");
                Log.LogarInformacao("Buscando no ES no type " + extent, "Total de docs: " + idsElasticSearch.Count);

                List<string> idsLightBase = new List<string>();
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                var coluna_id = "Id";
                if (extent == "Orgaos")
                {
                    coluna_id = "Id_Orgao";
                }
                else if(extent == "VOCABULARIOCONTROLADO"){
                    coluna_id = "Id_Termo";
                }
                string sql = string.Format("select {0} from {1}", coluna_id, extent);
                Log.LogarInformacao("Buscando no LBW na Base " + extent, sql);
                using (var reader = conn.ExecuteDataReader(sql))
                {
                    while (reader.Read())
                    {
                        idsLightBase.Add(reader[coluna_id].ToString());
                    }
                    reader.Close();
                }
                conn.CloseConection();
                Console.WriteLine("            " + idsLightBase.Count + " Docs recuperados no SINJ CADASTRO.");
                Log.LogarInformacao("Buscando no LBW na Base " + extent, "Total de docs: " + idsLightBase.Count);

                string data = DateTime.Now.ToString("dd/MM/yyyy");
                Console.WriteLine("            Comparando PORTAL com CADASTRO...");
                foreach (var id in idsElasticSearch)
                {
                    Console.CursorLeft = 0;
                    Console.Write(id);
                    if (!idsLightBase.Contains(id))
                    {
                        Console.WriteLine("");
                        Console.WriteLine("            " + id + " inexistente no CADASTRO");
                        Log.LogarInformacao("Comparando Docs de " + extent, "ID " + id + " não existe no LBW.");
                        RegistrosParaDeletar registroParaDeletar = new RegistrosParaDeletar { DataDeletado = data, Id = id, Nome = extent };
                        registrosParaDeletar.Add(registroParaDeletar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("###ERRO###", ex.Message);
                Log.LogarExcecao("Comparando Docs de " + extent, "Erro durante a comparação de docs entre ES e LBW", ex);
            }
            
            Console.WriteLine("");
            Console.WriteLine("            " + registrosParaDeletar.Count + " Docs inexistentes no CADASTRO.");
            Log.LogarInformacao("Finalizando comparação de Docs de " + extent, registrosParaDeletar.Count + " Docs inexistentes no LBW.");
            return registrosParaDeletar;
        }

        private List<string> BuscarIdsDosRegistrosNoElasticSearch(string type)
        {
            int size = TotalDeRegistrosNoElasticSearch(type);
            string get = string.Format("{0}{1}/_search?fields={2}&size={3}", Configuracao.LerValorChave(chaveElasticSearch), type, ObterNomeAtributoId(type), size);
            Log.LogarInformacao("Buscando no ES no type " + type, "GET/" + get);
            string json = new ElasticSearch().Get(get);
            ObjetoConsultaElasticSearch<IdReg> resultado = JSON.Desserializa<ObjetoConsultaElasticSearch<IdReg>>(json);
            IEnumerable<string> ids = from r in resultado.hits.hits select r.fields.Id;
            return ids.ToList();
        }

        private int TotalDeRegistrosNoElasticSearch(string type)
        {
            string json = new ElasticSearch().Get(string.Format("{0}{1}/{2}", Configuracao.LerValorChave(chaveElasticSearch), type, "_count"));
            Count count = JSON.Desserializa<Count>(json);
            return count.count;
        }

        public void DeletarRegistrosDoElasticDeletadosDoLightBase()
        {
            try
            {
                string sql = string.Format("select * from {0}", Configuracao.LerValorChave(chaveBaseExcluidos));
                List<RegistrosParaDeletar> registrosParaDeletar = BuscarExcluidos(sql);
                Console.WriteLine("Total arquivos deletados no SINJ de Cadastro: " + registrosParaDeletar.Count);
                Console.WriteLine("Deletando agora no SINJ de Pesquisa...");
                int deletados = DespacharDeletadosParaDelecao(registrosParaDeletar);
                Console.WriteLine("Total arquivos deletados no SINJ de Pesquisa: " + deletados);
                DeletarRegistrosBaseForExcluder();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro no processo de atualizados...");
                Log.LogarExcecao("Exclusão de registros", "Erro no processo de exclusão...", ex);
            }
        }
    }
}