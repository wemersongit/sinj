using System;
using System.Collections.Generic;
using System.Data;
using LightInfocon.Data.LightBaseProvider;

namespace SINJ.ExtratorDeTexto.AD
{
    internal class AcessaDados
    {

        private LightBaseConnection ConexaoBD{ get; set; }

        /// <summary>
        /// A instância da classe depende da string de conexão
        /// </summary>
        /// <param name="stringConexaoBD"></param>
        public AcessaDados(string stringConexaoBD)
        {
            try
            {
                ConexaoBD = new LightBaseConnection(stringConexaoBD);
            }
            catch (Exception ex)
            {
                throw new Exception("Acesso: " + stringConexaoBD, ex);
            }
        }

        private void OpenConnection()
        {
            try
            {
                if (ConexaoBD.State == ConnectionState.Closed)
                {
                    ConexaoBD.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("OpenConnection: ", ex);
            }
        }

        private LightBaseDataReader ExecuteDataReader(string sql)
        {
            try
            {
                OpenConnection();
                var cmd = new LightBaseCommand(sql, ConexaoBD);
                var dr = cmd.ExecuteReader();
                cmd.Dispose();
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ExecuteDataReader: ", ex);
            }
        }

        private int ExecuteNonQuery(string nonquery)
        {
            try
            {
                OpenConnection();
                var cmd = new LightBaseCommand(nonquery, ConexaoBD);
                var nq = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return nq;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ExecuteNonQuery: ", ex);
            }
        }

        private void CloseConection()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (ConexaoBD != null)
            {
                if (ConexaoBD.State != ConnectionState.Closed)
                {
                    ConexaoBD.Close();
                }
            }
        }

        internal string BuscarCaminhoArquivo(string id_reg, string nm_base, string nm_coluna_id, string nm_coluna_path_file)
        {
            string sql = string.Format("select {0} from {1} where {2}={3}", nm_coluna_path_file, nm_base, nm_coluna_id, id_reg);
            string pathFile = "";
            using (var reader = ExecuteDataReader(sql))
            {
                if(reader.Read())
                {
                    pathFile = reader[nm_coluna_path_file].ToString();
                }
            }
            CloseConection();
            return pathFile;
        }

        internal int SalvarTextoArquivo(string id_reg, string nm_base, string texto, string nm_coluna_id, string nm_coluna_texto)
        {
            string nonquery = string.Format("update {0} set {1}=\"{2}\" where {3}={4}", nm_base, nm_coluna_texto, texto.Replace("\"",""), nm_coluna_id, id_reg);
            int updated = ExecuteNonQuery(nonquery);
            CloseConection();
            return updated;
        }

        public string[] ListarIdsSemTexto(string nm_base, string nm_coluna_id, string nm_coluna_texto)
        {
            string sql = string.Format("select {0} from {1} where {2}=\"\"", nm_coluna_id, nm_base, nm_coluna_texto);
            List<string> ids = new List<string>();
            using (var reader = ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    ids.Add(reader[nm_coluna_id].ToString());
                }
            }
            CloseConection();
            return ids.ToArray();
        }
    }
}