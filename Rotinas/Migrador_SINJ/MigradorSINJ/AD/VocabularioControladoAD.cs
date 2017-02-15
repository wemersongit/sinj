using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using BRLight.DataAccess.LBW.Provider;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class VocabularioControladoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;


        private REST _rest;

        public VocabularioControladoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseVocabularioControlado", true));
        }

        internal List<VocabularioControladoLBW> BuscarVocabulariosControladosLBW()
        {
            List<VocabularioControladoLBW> vocabulariosControladosLbw = new List<VocabularioControladoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from VocabularioControlado"))
            {
                while(reader.Read()){
                    var termo = new  VocabularioControladoLBW();
                    termo.Id_Termo = reader["Id_Termo"].ToString();
                    termo.Id_ListaRelacao = reader["Id_ListaRelacao"].ToString();
                    termo.Nm_ListaRelacao = reader["Nm_ListaRelacao"].ToString();
                    termo.In_TipoTermo = Convert.ToInt32(reader["In_TipoTermo"]);
                    termo.In_Lista = Convert.ToBoolean(reader["In_Lista"]);
                    termo.In_NivelLista = Convert.ToInt32(reader["In_NivelLista"]);
                    termo.In_TermoNaoAutorizado = Convert.ToBoolean(reader["In_TermoNaoAutorizado"]);
                    termo.Nm_Termo = reader["Nm_Termo"].ToString();
                    Files.CriaArquivo("C:/temp/Nm_Termo.txt", reader["Nm_Termo"].ToString() + Environment.NewLine, true);
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
                    termo.TermosNaoAutorizados = BuscarTermosNaoAutorizados(termo.Id_Termo);

                    termo.TermoUse = BuscarTermoUse(termo.Id_Termo);
                    vocabulariosControladosLbw.Add(termo);
                    Files.CriaArquivo("C:/temp/termos.txt", JSON.Serialize(termo) + Environment.NewLine, true);
                }
                reader.Close();
            }
            _ad.CloseConection();


            return vocabulariosControladosLbw;
        }

        internal List<string> BuscarVocabularioControladoLBW2()
        {
            List<string> vocabularioControladoLBW = new List<string>();
            string line;
            if (System.IO.File.Exists("C:/temp/Nm_termo.txt"))
            {
                using (var reader = System.IO.File.OpenText("C:/temp/Nm_termo.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        vocabularioControladoLBW.Add(line);
                    }
                }
            }
            return vocabularioControladoLBW;
        }




        private List<TermoVocabularioControladoRelacionamento> BuscarTermosEspecificos(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosGerais = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from TERMOS_RELACAO_TERMO_GERAL_E_ESPECIFICO where Id_TermoGeral=\"{0}\"", id_termo);
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(sql))
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
            string sql = string.Format("select * from TERMOS_RELACAO_TERMO_GERAL_E_ESPECIFICO where Id_TermoEspecifico=\"{0}\"", id_termo);
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(sql))
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
            string sql = string.Format("select * from TERMOS_RELACAO_TERMO_RELACIONADO where Id_Termo=@id_termo or Id_TermoRelacionado=\"{0}\"", id_termo);
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(sql))
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

        private List<TermoVocabularioControladoRelacionamento> BuscarTermosNaoAutorizados(string id_termo)
        {
            List<TermoVocabularioControladoRelacionamento> termosRelacao = new List<TermoVocabularioControladoRelacionamento>();
            string sql = string.Format("select * from TERMOS_RELACAO_TERMO_NAO_AUTORIZADO where Id_TermoUse=\"{0}\"", id_termo);
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(sql))
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
            string sql = string.Format("select * from TERMOS_RELACAO_TERMO_NAO_AUTORIZADO where Id_TermoNaoAutorizado=\"{0}\"", id_termo);
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(sql))
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

        internal ulong Incluir(VocabularioControladoOV vocabularioControladoOv)
        {
            return _rest.Incluir(vocabularioControladoOv);
        }
    }
}
