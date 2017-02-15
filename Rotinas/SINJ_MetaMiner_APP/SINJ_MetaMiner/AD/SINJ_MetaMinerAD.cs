using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using util.BRLight;
using System.Data;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using System.Data.OleDb;
using SINJ_MetaMiner.OV;

namespace SINJ_MetaMiner.AD
{
    public class SINJ_MetaMinerAD
    {
        private SqlDataAdapter _sqlAdapter;
        public SINJ_MetaMinerAD()
        {
            _sqlAdapter = new SqlDataAdapter("select * from registro_item", new SqlConnection(Config.ValorChave("StringConnectionLexml"))) { SelectCommand = { CommandTimeout = 90 } };
            var cmd_builder = new SqlCommandBuilder(_sqlAdapter);
            _sqlAdapter.UpdateCommand = cmd_builder.GetUpdateCommand();
            _sqlAdapter.UpdateCommand.CommandText = "UPDATE [registro_item] SET [id_registro_item] = @p1, [cd_status] = @p2, [cd_validacao] = @p3, [ts_registro_gmt] = @p4, [tx_metadado_xml] = @p5 WHERE [id_registro_item] = @p1";
            _sqlAdapter.InsertCommand = cmd_builder.GetInsertCommand();
            _sqlAdapter.InsertCommand.CommandText = "INSERT INTO [registro_item] ([id_registro_item], [cd_status], [cd_validacao], [ts_registro_gmt], [tx_metadado_xml]) VALUES (@p1, @p2, @p3, @p4, @p5)";
            _sqlAdapter.UpdateCommand.Parameters.Remove(_sqlAdapter.UpdateCommand.Parameters[6]);
            _sqlAdapter.UpdateCommand.Parameters.Remove(_sqlAdapter.UpdateCommand.Parameters[6]);
            _sqlAdapter.UpdateCommand.Parameters.Remove(_sqlAdapter.UpdateCommand.Parameters[6]);
            _sqlAdapter.UpdateCommand.Parameters.Remove(_sqlAdapter.UpdateCommand.Parameters[6]);
            _sqlAdapter.UpdateCommand.Parameters.Remove(_sqlAdapter.UpdateCommand.Parameters[6]);
            _sqlAdapter.UpdateCommand.Parameters.Remove(_sqlAdapter.UpdateCommand.Parameters[6]);
            //_sqlAdapter.UpdateCommand = new SqlCommand();
            //_sqlAdapter.UpdateCommand.CommandTimeout = 90;
            //_sqlAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            //_sqlAdapter.InsertCommand = new SqlCommand();
            //_sqlAdapter.InsertCommand.CommandTimeout = 90;
            //_sqlAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
        }

        public DataSet BuscarLexml()
        {
            _sqlAdapter.TableMappings.Add("Table", "registro_item");
            var dataSet = new DataSet();
            _sqlAdapter.FillSchema(dataSet, SchemaType.Mapped);
            _sqlAdapter.Fill(dataSet);
            return dataSet;
        }

        public List<NormaLexml> ConsultarLexml()
        {
            IDbConnection dbcon = new SqlConnection(Config.ValorChave("StringConnectionLexml"));
            dbcon.Open();
            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql = "SELECT * FROM registro_item";
            dbcmd.CommandText = sql;
            IDataReader reader = dbcmd.ExecuteReader();
            var normas_lexml = new List<NormaLexml>();
            while(reader.Read()) {
                normas_lexml.Add(new NormaLexml {
                    id_registro_item = reader["id_registro_item"].ToString(),
                    cd_status = reader["cd_status"].ToString(),
                    cd_validacao = reader["cd_validacao"].ToString(),
                    ts_registro_gmt = reader["ts_registro_gmt"].ToString(),
                    tx_metadado_xml = reader["tx_metadado_xml"].ToString()
                });
            }
            // clean up
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbcon.Close();
            dbcon = null;
            return normas_lexml;
        }

        public Results<NormaOV> BuscarNormas(Pesquisa pesquisa)
        {
            return new TCDF.Sinj.RN.NormaRN().Consultar(pesquisa);
        }

        internal void AtualizarLexml(DataSet dataset_lexml)
        {
            _sqlAdapter.Update(dataset_lexml, "registro_item");
            dataset_lexml.AcceptChanges();
        }

        internal int AtualizarDoc(string id_registro_item, NormaLexml norma_lexml)
        {
            IDbConnection dbcon = new SqlConnection(Config.ValorChave("StringConnectionLexml"));
            dbcon.Open();
            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql = string.Format("UPDATE registro_item SET cd_status='{1}', cd_validacao='{2}', ts_registro_gmt='{3}', tx_metadado_xml='{4}' where id_registro_item='{0}'", id_registro_item, norma_lexml.cd_status, norma_lexml.cd_validacao, norma_lexml.ts_registro_gmt, norma_lexml.tx_metadado_xml);
            dbcmd.CommandText = sql;
            var result = dbcmd.ExecuteNonQuery();
            // clean up
            dbcmd.Dispose();
            dbcmd = null;
            dbcon.Close();
            dbcon = null;
            return result;
        }

        internal int InserirDoc(NormaLexml norma_lexml)
        {
            IDbConnection dbcon = new SqlConnection(Config.ValorChave("StringConnectionLexml"));
            dbcon.Open();
            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql = string.Format("INSERT INTO registro_item (id_registro_item, cd_status, cd_validacao, ts_registro_gmt, tx_metadado_xml) VALUES ('{0}','{1}','{2}','{3}','{4}')", norma_lexml.id_registro_item, norma_lexml.cd_status, norma_lexml.cd_validacao, norma_lexml.ts_registro_gmt, norma_lexml.tx_metadado_xml);
            dbcmd.CommandText = sql;
            var result = dbcmd.ExecuteNonQuery();
            // clean up
            dbcmd.Dispose();
            dbcmd = null;
            dbcon.Close();
            dbcon = null;
            return result;
        }
    }


}
