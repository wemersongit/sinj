using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class TermoAD : AD
    {

        private AcessaDados _conn;
        private string _extentVocabularioControlado;
        public string ExtentVocabularioControlado
        {
            get { return _extentVocabularioControlado; }
        }

        public TermoAD()
        {
            _conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            _extentVocabularioControlado = Configuracao.LerValorChave(chaveBaseVocabularioControlado);
        }

        public void BuscarVocabularioControladoEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo VocabularioControlado...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<TermoVocabularioControlado> lista = new List<TermoVocabularioControlado>();
                _conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + _conn.GetConnectionState());
                using (var reader = _conn.ExecuteDataReader(sql))
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
                            idsControle.Add(reader["Id_Termo"].ToString()); //Pega todos os IdS
                            TermoVocabularioControlado termo = new TermoVocabularioControlado();
                            termo.Id_Termo = reader["Id_Termo"].ToString();
                            termo.Id_ListaRelacao = reader["Id_ListaRelacao"].ToString();
                            termo.Nm_ListaRelacao = reader["Nm_ListaRelacao"].ToString();
                            termo.In_TipoTermo = Convert.ToInt32(reader["In_TipoTermo"]);
                            termo.In_Lista = Convert.ToBoolean(reader["In_Lista"]);
                            termo.In_NivelLista = Convert.ToInt32(reader["In_NivelLista"]);
                            termo.In_TermoNaoAutorizado = Convert.ToBoolean(reader["In_TermoNaoAutorizado"]);
                            termo.Nm_Termo = reader["Nm_Termo"].ToString();
                            termo.Nm_Auxiliar = reader["Nm_Auxiliar"].ToString();
                            termo.NotaExplicativa = reader["NotaExplicativa"].ToString();
                            termo.FonteCatalogadora = reader["FonteCatalogadora"].ToString();
                            termo.FonteAlteradora = reader["FonteAlteradora"].ToString();
                            termo.TextoFonte = reader["TextoFonte"].ToString();
                            termo.FontesPesquisadas = reader["FontesPesquisadas"].ToString();
                            termo.In_Aprovado = Convert.ToBoolean(reader["In_Aprovado"]);
                            termo.In_Excluir = Convert.ToBoolean(reader["In_Excluir"]);
                            termo.In_Ativo = Convert.ToBoolean(reader["In_Ativo"]);
                            termo.Dt_Cadastro = reader["Dt_Cadastro"].ToString() != "" ? Convert.ToDateTime(reader["Dt_Cadastro"].ToString()).ToString("dd/MM/yyyy") : "";
                            termo.Dt_UltimaAlteracao = reader["Dt_UltimaAlteracao"].ToString() != "" ? Convert.ToDateTime(reader["Dt_UltimaAlteracao"].ToString()).ToString("dd/MM/yyyy") : "";
                            termo.Nm_UsuarioUltimaAlteracao = reader["Nm_UsuarioUltimaAlteracao"].ToString();
                            termo.Nm_UsuarioCadastro = reader["Nm_UsuarioCadastro"].ToString();
                            if (termo.In_TipoTermo == 1)
                            {
                                termo.TermosEspecificos = BuscarTermosEspecificos(termo.Id_Termo);
                                termo.TermosGerais = BuscarTermosGerais(termo.Id_Termo);
                                termo.TermosRelacionados = BuscarTermosRelacionados(termo.Id_Termo);
                            }
                            if (termo.In_TipoTermo == 4)
                            {
                                if (!string.IsNullOrEmpty(termo.Id_ListaRelacao))
                                {
                                    termo.TermosListas = new List<TermoVocabularioControladoRelacionamento> { BuscarTermoLista(termo.Id_ListaRelacao) };
                                }
                                termo.TermosItensDaLista = BuscarTermosItensDaLista(termo.Id_Termo);
                                termo.TermosSublistas = BuscarTermosSubListas(termo.Id_Termo);
                            }
                            termo.TermosNaoAutorizados = BuscarTermosNaoAutorizados(termo.Id_Termo);

                            termo.TermoUse = BuscarTermoUse(termo.Id_Termo);

                            lista.Add(termo);
                            Console.WriteLine("----------> VocabularioControlado montado: " + termo.Id_Termo);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentVocabularioControlado, lista, "Id_Termo");
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
                            lista.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentVocabularioControlado, lista, "Id_Termo");
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
                            lista.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de VocabularioControlado");

                }
                _conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos descritores...");
                Log.LogarExcecao("Exportação de VocabularioControlado", "Erro na busca de todos descritores...", ex);
            }

        }

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosEspecificos(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosGerais = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from {0} where Id_TermoGeral=\"{1}\"", Configuracao.LerValorChave(chaveBaseRelacaoTermoGeralEEspecifico), id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_TermoEspecifico"].ToString();
                    termo.Nm_Termo = reader["Nm_TermoEspecifico"].ToString();
                    termosGerais.Add(termo);
                }
                reader.Close();
            }
            return termosGerais;
        }

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosGerais(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosGerais = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from {0} where Id_TermoEspecifico=\"{1}\"", Configuracao.LerValorChave(chaveBaseRelacaoTermoGeralEEspecifico), id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_TermoGeral"].ToString();
                    termo.Nm_Termo = reader["Nm_TermoGeral"].ToString();
                    termosGerais.Add(termo);
                }
                reader.Close();
            }
            return termosGerais;
        }

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosRelacionados(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosRelacao = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from {0} where Id_Termo=@id_termo or Id_TermoRelacionado=\"{1}\"", Configuracao.LerValorChave(chaveBaseRelacaoTermoRelacionado), id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_Termo"].ToString();
                    termo.Nm_Termo = reader["Nm_Termo"].ToString();
                    TermoVocabularioControladoRelacionamento termoRelacionado = new TermoVocabularioControladoRelacionamento();
                    termoRelacionado.Id_Termo = reader["Id_TermoRelacionado"].ToString();
                    termoRelacionado.Nm_Termo = reader["Nm_TermoRelacionado"].ToString();
                    if (termo.Id_Termo == id_termo)
                    {
                        termosRelacao.Add(termoRelacionado);
                    }
                    else
                    {
                        termosRelacao.Add(termo);
                    }
                }
                reader.Close();
            }
            return termosRelacao;
        }

        public TermoVocabularioControladoRelacionamento BuscarTermoLista(string id_termo)
        {
            string sql = string.Format("select * from {0} where Id_Termo=\"{1}\"", _extentVocabularioControlado, id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_Termo"].ToString();
                    termo.Nm_Termo = reader["Nm_Termo"].ToString();
                    termo.Id_ListaRelacao = reader["Id_ListaRelacao"].ToString();
                    return termo;
                }
                reader.Close();
            }
            return null;
        }

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosItensDaLista(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosRelacao = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from {0} where Id_ListaRelacao=\"{1}\" and In_Lista=false and In_Excluir=false and In_Ativo=true", _extentVocabularioControlado, id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_Termo"].ToString();
                    termo.Nm_Termo = reader["Nm_Termo"].ToString();
                    termosRelacao.Add(termo);
                }
                reader.Close();
            }
            return termosRelacao;
        }

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosSubListas(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosRelacao = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from {0} where Id_ListaRelacao=\"{1}\" and In_Lista=true and In_Excluir=false and In_Ativo=true", _extentVocabularioControlado, id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_Termo"].ToString();
                    termo.Nm_Termo = reader["Nm_Termo"].ToString();
                    termosRelacao.Add(termo);
                }
                reader.Close();
            }
            return termosRelacao;
        }

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosNaoAutorizados(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosRelacao = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from {0} where Id_TermoUse=\"{1}\"", Configuracao.LerValorChave(chaveBaseRelacaoTermoNaoAutorizado), id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_TermoNaoAutorizado"].ToString();
                    termo.Nm_Termo = reader["Nm_TermoNaoAutorizado"].ToString();
                    termosRelacao.Add(termo);
                }
                reader.Close();
            }
            return termosRelacao;
        }

        public TermoVocabularioControladoRelacionamento BuscarTermoUse(string id_termo)
        {
            string sql = string.Format("select * from {0} where Id_TermoNaoAutorizado=\"{1}\"", Configuracao.LerValorChave(chaveBaseRelacaoTermoNaoAutorizado), id_termo);
            _conn.OpenConnection();
            using (IDataReader reader = _conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    TermoVocabularioControladoRelacionamento termo = new TermoVocabularioControladoRelacionamento();
                    termo.Id_Termo = reader["Id_TermoUse"].ToString();
                    termo.Nm_Termo = reader["Nm_TermoUse"].ToString();
                    return termo;
                }
                reader.Close();
            }
            return null;
        }

    }
}