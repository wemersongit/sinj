using System;
using System.Collections.Generic;
using System.Data;
using LightInfocon.Data.LightBaseProvider;

namespace AcessaDadosLightBase
{
    public class AcessaDados
    {
        private LightBaseConnection conexaoBD { get; set; }


        /// <summary>
        /// A instância da classe depende da string de conexão
        /// </summary>
        /// <param name="stringConexaoBD"></param>
        public AcessaDados(string stringConexaoBD)
        {
            try
            {
                conexaoBD = new LightBaseConnection(stringConexaoBD);
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
                if (conexaoBD.State == ConnectionState.Closed)
                {
                    conexaoBD.Open();
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
                var cmd = new LightBaseCommand(sql, conexaoBD);
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
                var cmd = new LightBaseCommand(sql, conexaoBD);
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
            if (conexaoBD != null)
            {
                if (conexaoBD.State != ConnectionState.Closed)
                {
                    conexaoBD.Close();
                    conexaoBD = null;
                }
            }
        }

        public ConnectionState GetConnectionState()
        {
            return conexaoBD.State;
        }

        public IDbCommand CriarComando(string comando)
        {
            return new LightBaseCommand(comando, conexaoBD);
        }

        public IDbDataParameter CriarParametro(string nomeDoParametro, object valor)
        {
            return new LightBaseParameter(nomeDoParametro, valor);
        }
    }
}
