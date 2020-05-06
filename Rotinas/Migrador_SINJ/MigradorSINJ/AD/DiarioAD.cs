using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class DiarioAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public DiarioAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseDiario", true));
        }

        internal List<string> BuscarIdsDiariosLBW()
        {
            var where = Config.ValorChave("where_diarios");
            if (where == "-1")
            {
                where = "";
            }
            List<string> idsLbw = new List<string>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id from VersoesDosDodfs" + where))
            {
                while (reader.Read())
                {
                    idsLbw.Add(reader["Id"].ToString());
                }
                reader.Close();
            }
            _ad.CloseConection();
            return idsLbw;
        }

        internal List<DiarioLBW> BuscarDiariosLBW(string select)
        {
            List<DiarioLBW> diariosLbw = new List<DiarioLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(select))
            {
                while (reader.Read())
                {
                    var diario = new DiarioLBW();
                    diario.Id = reader["Id"].ToString();
                    diario.Numero = reader["AlfaNumero"].ToString();
                    diario.Sessao = reader["Sessao"].ToString();
                    diario.OrgaoCadastrador = reader["OrgaoCadastrador"].ToString();

                    var dataDaAssinatura = reader["DataDaAssinatura"];
                    diario.DataDaAssinatura = !(dataDaAssinatura is DBNull) ? Convert.ToDateTime(dataDaAssinatura).ToString("dd'/'MM'/'yyyy") : null;

                    diario.SituacaoQuantoAPendencia = reader["SituacaoQuantoAPendencia"].ToString();

                    diario.UsuarioDaUltimaAlteracao = reader["UsuarioDaUltimaAlteracao"].ToString();
                    diario.UsuarioQueCadastrou = reader["UsuarioQueCadastrou"].ToString();

                    diario.DataDaUltimaAlteracao = reader["DataDaUltimaAlteracao"].ToString();
                    diario.DataDoCadastro = reader["DataDoCadastro"].ToString();

                    diariosLbw.Add(diario);
                }
                reader.Close();
            }
            _ad.CloseConection();
            return diariosLbw;
        }

        internal List<DiarioLBW> BuscarCaminhosArquivosDiariosLBW()
        {
            var where = Config.ValorChave("where_diarios");
            if (where == "-1")
            {
                where = "";
            }
            List<DiarioLBW> diariosLbw = new List<DiarioLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id, CaminhoArquivoTexto from versoesdosdodfs" + where))
            {
                while (reader.Read())
                {
                    diariosLbw.Add(new DiarioLBW
                    {
                        Id = reader["Id"].ToString(),
                        CaminhoArquivoTexto = reader["CaminhoArquivoTexto"].ToString()
                    });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return diariosLbw;
        }

        internal DiarioLBW BuscarCaminhosArquivos(string ch_diario)
        {
            DiarioLBW diarioLbw = new DiarioLBW();

            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id, CaminhoArquivoTexto from versoesdosdodfs where Id=" + ch_diario))
            {
                if (reader.Read())
                {
                    diarioLbw.Id = reader["Id"].ToString();
                    diarioLbw.CaminhoArquivoTexto = reader["CaminhoArquivoTexto"].ToString();
                }
                reader.Close();
            }
            _ad.CloseConection();
            return diarioLbw;
        }

        internal ulong Incluir(DiarioOV diarioOv)
        {
            diarioOv._metadata.id_doc = _rest.Incluir(diarioOv);
            return diarioOv._metadata.id_doc;
        }

        internal bool Atualizar(ulong id_doc, DiarioOV diarioOv)
        {
            return _rest.Atualizar<DiarioOV>(id_doc, diarioOv);
        }

        public string AnexarArquivo(FileParameter fileParameter)
        {
            string resultado;
            var doc = new Doc(Config.ValorChave("NmBaseDiario", true));
            var dicionario = new Dictionary<string, object>();
            dicionario.Add("file", fileParameter);
            resultado = doc.incluir(dicionario);
            return resultado;
        }
    }
}
