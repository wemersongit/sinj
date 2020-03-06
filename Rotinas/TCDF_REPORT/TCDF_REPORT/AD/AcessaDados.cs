using System;
using System.Data;
using LightInfocon.Data.LightBaseProvider;

namespace TCDF_REPORT.AD
{
    internal class AcessaDados
    {

        private LightBaseConnection ConexaoBD { get; set; }

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

        ~AcessaDados()
        {
            CloseConection();
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

        internal LightBaseDataReader ExecuteDataReader(string sql)
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

        internal int ExecuteNonQuery(string nonquery)
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
    }
}