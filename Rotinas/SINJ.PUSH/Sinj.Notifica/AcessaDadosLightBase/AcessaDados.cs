using System;
using System.Data;
using LightInfocon.Data.LightBaseProvider;

namespace AcessaDadosLightBase
{
    public class AcessaDados
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

        public void OpenConnection()
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

        public LightBaseDataReader ExecuteDataReader(string sql)
        {
            try
            {
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

        public int ExecuteNonQuery(string sql)
        {
            try
            {
                var cmd = new LightBaseCommand(sql, ConexaoBD);
                var nq = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return nq;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ExecuteNonQuery: ", ex);
            }
        }

        public void CloseConection()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (ConexaoBD != null)
            {
                if (ConexaoBD.State != ConnectionState.Closed)
                {
                    ConexaoBD.Close();
                    ConexaoBD = null;
                }
            }
        }

        public ConnectionState GetConnectionState()
        {
            return ConexaoBD.State;
        }
    }
}