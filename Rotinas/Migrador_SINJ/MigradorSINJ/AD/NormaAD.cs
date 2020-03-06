using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;
using System.Data;

namespace MigradorSINJ.AD
{
    public class NormaAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public NormaAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseNorma", true));
        }

        internal NormaOV BuscarNorma(string id_norma,string[] select)
        {
            NormaOV normaOv = null;
            string consulta = "$$={\"literal\":\"ch_norma='" + id_norma + "'\",\"limit\":\"1\"";
            if(select.Length > 0){
                consulta += ",\"select\":"+ JSON.Serialize(select);
            }
            consulta += "}";
            var sResults = _rest.Consultar(consulta);
            var oResults = JSON.Deserializa<Results<NormaOV>>(sResults);
            if (oResults.results.Count == 1)
            {
                normaOv = oResults.results[0];
            }
            return normaOv;
        }

        internal List<NormaOV> BuscarNormasRest(int limit)
        {
            string consulta = "$$={\"limit\":\"1\"}";
            if (limit > 0)
            {
                consulta = "$$={\"limit\":\""+limit+"\"}";
            }
            var sResults = _rest.Consultar(consulta);
            var oResults = JSON.Deserializa<Results<NormaOV>>(sResults);
            return oResults.results;
        }

        internal List<NormaLBW> BuscarNormasLBW(string select)
        {
            List<NormaLBW> normasLbw = new List<NormaLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(select))
            {
                while (reader.Read())
                {
                    normasLbw.Add(MontaNormaSimples(reader));
                }
                reader.Close();
            }
            _ad.CloseConection();
            return normasLbw;
        }

        internal List<NormaLBW> BuscarNormasIndexacaoLBW(string select)
        {
            List<NormaLBW> normasLbw = new List<NormaLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader(select))
            {
                while (reader.Read())
                {

                    NormaLBW norma = new NormaLBW();
                    norma.Id = reader["Id"].ToString();
                    norma.NeoIndexacao = ObtemNeoIndexacao(reader);
                    normasLbw.Add(norma);
                }
                reader.Close();
            }
            _ad.CloseConection();
            return normasLbw;
        }

        internal List<string> BuscarIdsNormasLBW()
        {
            var where = Config.ValorChave("where_normas");
            if(where == "-1"){
                where = "";
            }
            List<string> idsLbw = new List<string>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id from versoesdasnormas" + where))
            {
                while (reader.Read())
                {
                    idsLbw.Add(reader["Id"].ToString());
                    Files.CriaArquivo("C:/temp/Id.txt", reader["Id"].ToString() + Environment.NewLine, true);
                }
                reader.Close();
            }
            _ad.CloseConection();
            return idsLbw;
        }

        internal List<string> BuscarIdsNormasLBW3()
        {
            List<string> idsLbw = new List<string>();
            string line;
            if (System.IO.File.Exists("./Id.txt"))
            {
                using (var reader = System.IO.File.OpenText("./Id.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        idsLbw.Add(line);
                    }
                }
            }
            return idsLbw;
        }


        internal List<NormaLBW> BuscarUsuarioCadastradorNormasLBW()
        {
            List<NormaLBW> normasLBW = new List<NormaLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id, UsuarioQueCadastrou, DataDoCadastro, UsuarioDaUltimaAlteracao, DataDaUltimaAlteracao from VersoesDasNormas"))
            {
                while (reader.Read())
                {
                    normasLBW.Add(new NormaLBW { Id = reader["Id"].ToString(),
                                                 UsuarioQueCadastrou = reader["UsuarioQueCadastrou"].ToString(),
                                                 DataDoCadastro = reader["DataDoCadastro"].ToString(),
                                                 UsuarioDaUltimaAlteracao = reader["UsuarioDaUltimaAlteracao"].ToString(),
                                                 DataDaUltimaAlteracao = reader["DataDaUltimaAlteracao"].ToString()
                                               }
                                  );
                }
                reader.Close();
            }
            _ad.CloseConection();
            return normasLBW;
        }

        internal List<NormaLBW> BuscarCaminhosArquivosNormasLBW()
        {
            var where = Config.ValorChave("where_normas");
            if (where == "-1")
            {
                where = "";
            }
            List<NormaLBW> normasLbw = new List<NormaLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id, CaminhoArquivoTextoAcao, CaminhoArquivoTextoConsolidado, Fontes from versoesdasnormas" + where))
            {
                while (reader.Read())
                {
                    normasLbw.Add(new NormaLBW{ Id = reader["Id"].ToString(),
                                                CaminhoArquivoTextoAcao = reader["CaminhoArquivoTextoAcao"].ToString(),
                                                CaminhoArquivoTextoConsolidado = reader["CaminhoArquivoTextoConsolidado"].ToString(),
                                                Fontes = ObtemFontesDaNorma(reader).ToList<FonteLBW>()});
                }
                reader.Close();
            }
            _ad.CloseConection();
            return normasLbw;
        }

        internal NormaLBW BuscarCaminhosArquivos(string ch_norma)
        {
            NormaLBW normaLbw = new NormaLBW();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select Id, CaminhoArquivoTextoAcao, CaminhoArquivoTextoConsolidado, Fontes from versoesdasnormas where Id=" + ch_norma))
            {
                if (reader.Read())
                {
                    normaLbw.Id = reader["Id"].ToString();
                    normaLbw.CaminhoArquivoTextoAcao = reader["CaminhoArquivoTextoAcao"].ToString();
                    normaLbw.CaminhoArquivoTextoConsolidado = reader["CaminhoArquivoTextoConsolidado"].ToString();
                    normaLbw.Fontes = ObtemFontesDaNorma(reader).ToList<FonteLBW>();
                }
                reader.Close();
            }
            _ad.CloseConection();
            return normaLbw;
        }



        private NormaLBW MontaNormaSimples(IDataRecord reader)
        {
            NormaLBW norma = new NormaLBW();
            norma.Id = reader["Id"].ToString();
            norma.Id_Tipo = reader["Id_Tipo"].ToString();
            norma.Numero = reader["Numero"].ToString();

            object dataAssinatura = reader["DataAssinatura"];
            norma.DataAssinatura = !(dataAssinatura is DBNull) ? Convert.ToDateTime(reader["DataAssinatura"]).ToString("dd'/'MM'/'yyyy") : null;

            norma.DataDaUltimaAlteracao = reader["DataDaUltimaAlteracao"].ToString();
            norma.Ambito = reader["Ambito"].ToString();
            norma.Apelido = reader["Apelido"].ToString(); ;
            norma.UrlReferenciaExterna = reader["UrlReferenciaExterna"].ToString();
            var haPendencia = false;
            bool.TryParse(reader["HaPendencia"].ToString(), out haPendencia);
            norma.HaPendencia = haPendencia;

            var destacada = false;
            bool.TryParse(reader["Destacada"].ToString(), out destacada);
            norma.Destacada = destacada;

            if (!(reader["Autorias"] is DBNull))
            {
                object[] autorias = (object[])reader["Autorias"];
                foreach (string autoria in autorias)
                {
                    norma.Autorias.Add(autoria);
                }
            }

            norma.Letra = reader["Letra"].ToString();

            norma.NumeroSequencial = reader["NumeroSequencial"].ToString();

            //Talvez nao sejam necessarios pois nao precisa-se expô-los ao sistema, por terem sido preenchidos automaticamente pelo sistema:
            norma.orgao_cadastrador = reader["NomeDoOrgao"].ToString();
            norma.Situacao = reader["Situacao"].ToString();
            norma.UsuarioQueCadastrou = reader["UsuarioQueCadastrou"].ToString();
            norma.DataDoCadastro = reader["DataDoCadastro"].ToString();
            norma.UsuarioDaUltimaAlteracao = reader["UsuarioDaUltimaAlteracao"].ToString();
            norma.DataDaUltimaAlteracao = reader["DataDaUltimaAlteracao"].ToString();
            norma.ObservacaoNorma = reader["ObservacaoNorma"].ToString();
            norma.ChaveParaNaoDuplicacao = reader["ChaveParaNaoDuplicacao"].ToString();
            norma.Ementa = reader["Ementa"].ToString();
            norma.ListaDeNomes = reader["ListaDeNomes"].ToString();
            if (reader["Origens"] != DBNull.Value)
            {
                object[] origens = (object[])reader["Origens"];
                foreach (object idOrigem in origens)
                {
                    norma.Origens.Add(Convert.ToInt32(idOrigem));
                }
            }
            norma.ParametroConstitucional = reader["ParametroConstitucional"].ToString();
            norma.Procedencia = reader["Procedencia"].ToString(); ;
            var relator = reader["Relator"].ToString();
            if(!string.IsNullOrEmpty(relator)){
                norma.Relatores.Add(relator);
            }

            object dataDeAutuacao = reader["DataDeAutuacao"];
            norma.DataDeAutuacao = !(dataDeAutuacao is DBNull) ? Convert.ToDateTime(reader["DataDeAutuacao"]).ToString("dd'/'MM'/'yyyy") : null;

            object[] requerentesAux = new object[0];
            if (reader["Requerente"] != DBNull.Value) requerentesAux = (object[])reader["Requerente"];
            foreach (int idRequerente in requerentesAux) norma.Requerentes.Add(idRequerente);
            object[] requeridosAux = new object[0];
            if (reader["Requerido"] != DBNull.Value) requeridosAux = (object[])reader["Requerido"];
            foreach (int idRequerido in requeridosAux) norma.Requeridos.Add(idRequerido);
            object[] procuradoresAux = new object[0];
            if (reader["ProcuradorResponsavel"] != DBNull.Value) procuradoresAux = (object[])reader["ProcuradorResponsavel"];
            foreach (int idProcurador in procuradoresAux) norma.ProcuradoresResponsaveis.Add(idProcurador);
            object[] interessadosAux = new object[0];
            if (reader["InteressadoDaAcao"] != DBNull.Value) interessadosAux = (object[])reader["InteressadoDaAcao"];
            foreach (int idInteressado in interessadosAux) norma.Interessados.Add(idInteressado);
            norma.EfeitoDaDecisao = reader["EfeitoDaDecisao"].ToString();
            norma.Fontes = ObtemFontesDaNorma(reader).ToList<FonteLBW>();

            norma.Vides = ObtemVides(reader);

            norma.NeoIndexacao = ObtemNeoIndexacao(reader);
            norma.HistoricoDeDecisoes = ObtemHistorico(norma.Id);
            object[] auxiliarDeRankeamentoAux = new object[0];
            if (reader["AuxiliarDeRankeamento"] != DBNull.Value) auxiliarDeRankeamentoAux = (object[])reader["AuxiliarDeRankeamento"];
            foreach (string rank in auxiliarDeRankeamentoAux) norma.AuxiliarDeRankeamento.Add(rank);

            return norma;

        }

        private IEnumerable<FonteLBW> ObtemFontesDaNorma(IDataRecord reader)
        {
            var listaDeFontes = new List<FonteLBW>();
            if (reader["Fontes"] is DataTable)
            {
                DataTable dadosDasFontes = (DataTable)reader["Fontes"];
                foreach (DataRow dadosDaFonte in dadosDasFontes.Rows) listaDeFontes.Add(CarregaFonte(dadosDaFonte));
            }
            return listaDeFontes;
        }

        private List<VideLBW> ObtemVides(IDataRecord reader)
        {
            List<VideLBW> vides = ObtemVidesQueAfetamANorma(reader);
            vides.AddRange(ObtemVidesQueAfetamOutrasNormas(Convert.ToInt32(reader["Id"])));
            return vides;
        }

        private List<VideLBW> ObtemVidesQueAfetamANorma(IDataRecord reader)
        {
            var listaDeVides = new List<VideLBW>();
            if (reader["Vides"] is DataTable)
            {
                DataTable dadosDosVides = (DataTable)reader["Vides"];
                foreach (DataRow dadosDoVide in dadosDosVides.Rows)
                {
                    listaDeVides.Add(CarregaVide(dadosDoVide));
                }
            }
            return listaDeVides;
        }

        private List<VideLBW> ObtemVidesQueAfetamOutrasNormas(int id_norma)
        {
            var listaDeVides = new List<VideLBW>();
            try
            {
                _ad.OpenConnection();
                using (var reader = _ad.ExecuteDataReader("select Id, Id_Tipo, DataAssinatura, Numero, Vides from versoesdasnormas where Vides.IdDaNormaPosterior=" + id_norma))
                {
                    while (reader.Read())
                    {
                        //Apenas os vides que tem Vides.IdDaNormaPosterior=id que devem ser guardados
                        if (reader["Vides"] is DataTable)
                        {
                            DataTable tabelaDeVides = (DataTable)reader["Vides"];
                            foreach (DataRow row in tabelaDeVides.Rows)
                            {
                                if (!(row["IdDaNormaPosterior"] is DBNull))
                                {
                                    if (id_norma == Convert.ToInt32(row["IdDaNormaPosterior"]))
                                    {
                                        VideLBW vide = CarregaVide(row);
                                        vide.VideAlterador = true;
                                        vide.TipoDeNorma = Convert.ToInt32(reader["Id_Tipo"]);
                                        object dataDaNormaPosterior = reader["DataAssinatura"];
                                        vide.DataDaNormaPosterior = !(dataDaNormaPosterior is DBNull) ? Convert.ToDateTime(reader["DataAssinatura"]).ToString("dd'/'MM'/'yyyy") : null;
                                        vide.IdDaNormaPosterior = Convert.ToInt32(reader["Id"]);
                                        vide.NumeroDaNormaPosterior = reader["numero"].ToString();
                                        listaDeVides.Add(vide);
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter decisões da norma.", ex);
            }
            return listaDeVides;
        }

        private List<NeoIndexacao> ObtemNeoIndexacao(IDataRecord reader)
        {
            List<NeoIndexacao> listaNeoIndexacao = new List<NeoIndexacao>();
            if (reader["NeoIndexacao"] is DataTable)
            {
                DataTable datatable = (DataTable)reader["NeoIndexacao"];
                int i = 0;
                foreach (DataRow dataRow in datatable.Rows)
                {
                    NeoIndexacao neoIndexacao = new NeoIndexacao();
                    neoIndexacao.InTipoTermo = Convert.ToInt32(dataRow["InTipoTermo"]);
                    neoIndexacao.NmTermo = dataRow["NmTermo"].ToString();
                    neoIndexacao.NmEspecificador = dataRow["NmEspecificador"].ToString();
                    listaNeoIndexacao.Add(neoIndexacao);
                }
            }
            return listaNeoIndexacao;
        }

        public List<DecisaoLBW> ObtemHistorico(string norma)
        {
            var historico = new List<DecisaoLBW>();
            try
            {
                _ad.OpenConnection();
                using (var reader = _ad.ExecuteDataReader("select * from HistoricosDeDecisoes where idDaNorma=" + norma))
                {
                    var decisao = new DecisaoLBW();
                    while(reader.Read()){
                        decisao.Id = Convert.ToInt64(reader["Id"]);
                        decisao.IdDaNorma = Convert.ToInt64(reader["IdDaNorma"]);
                        decisao.Tipo = (TipoDeDecisaoEnum)Enum.Parse(typeof(TipoDeDecisaoEnum), Convert.ToString(reader["Decisao"]));
                        decisao.DataDaPublicacao = !(reader["DataDePublicacao"] is DBNull) ? Convert.ToDateTime(reader["DataDePublicacao"]).ToString("dd'/'MM'/'yyyy") : "";
                        //decisao.DataDaPublicacao = reader["DataDePublicacao"].ToString();
                        decisao.Complemento = reader["ComplementoDaDecisao"].ToString();
                        historico.Add(decisao);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Obter decisões da norma.", ex);
            }
            return historico;
        }

        private FonteLBW CarregaFonte(DataRow dataRowFonte)
        {
            FonteLBW fonte = new FonteLBW();
            fonte.Id = new Guid(dataRowFonte["IdDaFonte"].ToString());
            fonte.TipoFonte = dataRowFonte["TipoFonte"].ToString();
            fonte.TipoEdicao = fonte.TipoEdicao = (TipoDeEdicaoEnum)Enum.Parse(typeof(TipoDeEdicaoEnum),Convert.ToString(dataRowFonte["TipoEdicao"]));
            fonte.TipoPublicacao = dataRowFonte["TipoPublicacao"].ToString();

            object dataPublicacao = dataRowFonte["DataPublicacao"];
            fonte.DataPublicacao = !(dataPublicacao is DBNull) ? Convert.ToDateTime(dataRowFonte["DataPublicacao"]).ToString("dd'/'MM'/'yyyy") : null;

            var valorPagina = dataRowFonte["Pagina"].ToString();
            if (!string.IsNullOrEmpty(valorPagina))
            {
                fonte.Pagina = Convert.ToInt32(valorPagina);
            }
            var valorColuna = dataRowFonte["Coluna"].ToString();
            if (!string.IsNullOrEmpty(valorColuna))
            {
                fonte.Coluna = Convert.ToInt32(valorColuna);
            }

            fonte.Observacao = dataRowFonte["Observacao"].ToString();
            fonte.MotivoReduplicacao = dataRowFonte["MotivoReduplicacao"].ToString();
            fonte.ConteudoArquivoTexto = dataRowFonte["ConteudoArquivoTexto"].ToString();

            //Note: Essa validação não está segura pois se baseia na existência de '\' no campo "NomeArquivoTexto" para verificar quando "CaminhoArquivoTexto" está no lugar de "NomeArquivoTexto".
            if (dataRowFonte["NomeArquivoTexto"].ToString().IndexOf('\\') == -1)
            {
                fonte.NomeArquivoTexto = dataRowFonte["NomeArquivoTexto"].ToString();
                fonte.CaminhoArquivoTexto = dataRowFonte["CaminhoArquivoTexto"].ToString();
            }
            else
            {
                fonte.NomeArquivoTexto = dataRowFonte["CaminhoArquivoTexto"].ToString();
                fonte.CaminhoArquivoTexto = dataRowFonte["NomeArquivoTexto"].ToString();
            }
            return fonte;

        }

        private static VideLBW CarregaVide(DataRow dataRowFonte)
        {
            var vide = new VideLBW();
            vide.Id = new Guid(dataRowFonte["idVide"].ToString());
            vide.ComentarioVide = dataRowFonte["ComentarioVide"].ToString();
            vide.ArtigoDaNormaPosterior = dataRowFonte["ArtigoDaNormaPosterior"].ToString();
            vide.ParagrafoDaNormaPosterior = dataRowFonte["ParagrafoDaNormaPosterior"].ToString();
            vide.IncisoDaNormaPosterior = dataRowFonte["IncisoDaNormaPosterior"].ToString();
            vide.AlineaDaNormaPosterior = dataRowFonte["AlineaDaNormaPosterior"].ToString();
            vide.ItemDaNormaPosterior = dataRowFonte["ItemDaNormaPosterior"].ToString();
            vide.CaputDaNormaPosterior = dataRowFonte["CaputDaNormaPosterior"].ToString();
            vide.AnexoDaNormaPosterior = dataRowFonte["AnexoDaNormaPosterior"].ToString();
            vide.ArtigoDaNormaAnterior = dataRowFonte["ArtigoDaNormaAnterior"].ToString();
            vide.ParagrafoDaNormaAnterior = dataRowFonte["ParagrafoDaNormaAnterior"].ToString();
            vide.IncisoDaNormaAnterior = dataRowFonte["IncisoDaNormaAnterior"].ToString();
            vide.AlineaDaNormaAnterior = dataRowFonte["AlineaDaNormaAnterior"].ToString();
            vide.ItemDaNormaAnterior = dataRowFonte["ItemDaNormaAnterior"].ToString();
            vide.CaputDaNormaAnterior = dataRowFonte["CaputDaNormaAnterior"].ToString();
            vide.AnexoDaNormaAnterior = dataRowFonte["AnexoDaNormaAnterior"].ToString();
            vide.PaginaDaPublicacaoPosterior = dataRowFonte["PaginaDaPublicacaoPosterior"].ToString();
            vide.ColunaDaPublicacaoPosterior = dataRowFonte["ColunaDaPublicacaoPosterior"].ToString();
            vide.NumeroDaNormaPosterior = dataRowFonte["NumeroDaNormaPosterior"].ToString();
            object idDaNormaPosterior = dataRowFonte["IdDaNormaPosterior"];
            vide.IdDaNormaPosterior = !(idDaNormaPosterior is DBNull) ? Convert.ToInt32(idDaNormaPosterior) : 0;
            object dataDaNormaPosterior = dataRowFonte["DataDaNormaPosterior"];
            vide.DataDaNormaPosterior = !(dataDaNormaPosterior is DBNull) ? Convert.ToDateTime(dataRowFonte["DataDaNormaPosterior"]).ToString("dd'/'MM'/'yyyy") : null;
            if (!(dataRowFonte["IdTipoDaNormaPosterior"] is DBNull))
            {
                vide.TipoDeNorma = Convert.ToInt32(dataRowFonte["IdTipoDaNormaPosterior"]);
            }
            object dataDePublicacaoPosterior = dataRowFonte["DataDePublicacaoPosterior"];
            vide.DataDePublicacaoPosterior = !(dataDePublicacaoPosterior is DBNull) ? Convert.ToDateTime(dataRowFonte["DataDePublicacaoPosterior"]).ToString("dd'/'MM'/'yyyy") : null;

            object tipoDeVinculo = dataRowFonte["IdTipoDeRelacaoDeVinculo"];
            if (!(tipoDeVinculo is DBNull)) {
                vide.TipoDeVinculo = Convert.ToInt32(tipoDeVinculo);
            }

            vide.TipoDeFonte = dataRowFonte["TipoDaFontePosterior"].ToString();

            return vide;

        }

        internal ulong Incluir(NormaOV normaOv)
        {
            normaOv._metadata.id_doc = _rest.Incluir(normaOv);
            return normaOv._metadata.id_doc;
        }

        internal bool Atualizar(ulong id_doc, NormaOV normaOv)
        {
            return _rest.Atualizar<NormaOV>(id_doc, normaOv);
        }

        internal bool AtualizarPath(ulong id_doc, string caminho, string valor, string retorno)
        {
            return _rest.AtualizarPath(id_doc, caminho, valor, retorno);
        }

        public string AnexarArquivo(FileParameter fileParameter)
        {
            string resultado;var doc = new Doc(Config.ValorChave("NmBaseNorma", true));
            var dicionario = new Dictionary<string, object>();
            dicionario.Add("file", fileParameter);
            resultado = doc.incluir(dicionario);
            return resultado;
        }


        public List<String> BuscarNormaJSON(string filename)
        {
            List<string> lista_normas_json = new List<string>();
            string line;
            if (System.IO.File.Exists(filename))
            {
                using (var reader = System.IO.File.OpenText(filename))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lista_normas_json.Add(line);
                    }
                }
            }
            return lista_normas_json;
        }

    }
}
