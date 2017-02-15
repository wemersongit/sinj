using System;
using System.Data;
using LightInfocon.Data.LightBaseProvider;

namespace BRLight.DataAccess.LBW.Provider
{
    public class AcessaDados
    {
        private LightBaseConnection _conexaoBD;
        private string _stringConexaoBD;

        /// <summary>
        /// A instância da classe depende da string de conexão
        /// </summary>
        /// <param name="stringConexaoBD">String de conxão LBW</param>
        public AcessaDados(string stringConexaoBD)
        {
            _stringConexaoBD = stringConexaoBD;
            _conexaoBD = new LightBaseConnection(_stringConexaoBD);
        }

        ~AcessaDados()
        {
            Dispose();
        }

        public void OpenConnection()
        {
            try
            {
                if(_conexaoBD == null)
                {
                    _conexaoBD = new LightBaseConnection(_stringConexaoBD);
                }
                if (_conexaoBD.State == ConnectionState.Closed)
                {
                    _conexaoBD.Open();
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
                if (_conexaoBD == null)
                {
                    OpenConnection();
                }
                var cmd = new LightBaseCommand(sql, _conexaoBD);
                var dr = cmd.ExecuteReader();
                cmd.Dispose();
                return dr;
            }
            catch (Exception ex)
            {
                try
                {
                    if (_conexaoBD == null)
                    {
                        OpenConnection();
                    }
                    var cmd = new LightBaseCommand(sql, _conexaoBD);
                    var dr = cmd.ExecuteReader();
                    cmd.Dispose();
                    return dr;
                }
                catch (Exception _ex)
                {
                    throw new Exception("Erro ExecuteDataReader. sql:" + sql, _ex);
                }
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            try
            {
                if (_conexaoBD == null)
                {
                    OpenConnection();
                }
                var cmd = new LightBaseCommand(sql, _conexaoBD);
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
            if (_conexaoBD != null)
            {
                if (_conexaoBD.State != ConnectionState.Closed)
                {
                    _conexaoBD.Close();
                }
            }
        }

        public void Dispose()
        {
            CloseConection();
            _conexaoBD = null;
        }

        public ConnectionState GetConnectionState()
        {
            return _conexaoBD.State;
        }

        public IDbCommand CriarComando(string comando)
        {
            return new LightBaseCommand(comando, _conexaoBD);
        }

        public IDbDataParameter CriarParametro(string nomeDoParametro, object valor)
        {
            return new LightBaseParameter(nomeDoParametro, valor);
        }
    }
}
