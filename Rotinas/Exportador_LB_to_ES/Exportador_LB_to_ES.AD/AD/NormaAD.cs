using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;
using LightInfocon.Data.LightBaseProvider;
using SINJ.ExtratorDeTexto;

namespace Exportador_LB_to_ES.AD.AD
{
    public class NormaAD : AD
    {
        private string _extentNorma;
        public string ExtentNorma
        {
            get { return _extentNorma; }
        }
        public NormaAD()
        {
            _extentNorma = Configuracao.LerValorChave(chaveBaseNormas);
        }

        

        /// <summary>
        /// busca todas as normas da base e indexa
        /// </summary>
        /// <param name="sql">consulta sql</param>
        public void BuscarNormasEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo Normas...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                List<Norma> normas = new List<Norma>();
                using (LightBaseDataReader rdr = conn.ExecuteDataReader(sql))
                {
                    Console.WriteLine("Reader.Count = " + rdr.Count);
                    EsAD indexa = new EsAD();
                    List<string> idsControle = new List<string>();
                    List<string> idsError = new List<string>();
                    List<string> todosIdsSucess = new List<string>();
                    total = rdr.Count;
                    while (rdr.Read())
                    {
                        try
                        {
                            i++;
                            j++;
                            try
                            {
                                idsControle.Add(rdr["Id"].ToString()); //Pega todos os Ids das normas
                                Norma norma = MontaNormaSimples(rdr);
                                normas.Add(norma);
                                Console.WriteLine("----------> Norma montada: " + norma.Id);
                            }
                            catch (Exception ex)
                            {
                                idsError.Add(rdr["Id"].ToString());
                                Console.WriteLine("Erro ao montar objeto norma " + rdr["Id"]);
                                Log.LogarExcecao("Exportação de Normas", "Erro ao montar objeto norma " + rdr["Id"], ex);
                                //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                            }
                            if (i >= 100)
                            {
                                List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentNorma, normas, "Id");
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
                                normas.Clear();
                                idsControle.Clear();
                                idsSucess.Clear();
                            }
                            else if (j == total)
                            {
                                List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentNorma, normas, "Id");
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
                                normas.Clear();
                                idsControle.Clear();
                                idsSucess.Clear();
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("Erro na norma " + rdr["Id"]);
                            Log.LogarExcecao("Exportação de Normas", "Erro na norma " + rdr["Id"], exception);
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Normas");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todas as normas...");
                Log.LogarExcecao("Exportação de Normas", "Erro na busca de todas as normas...", ex);
            }
        }

        /// <summary>
        /// Monta o objeto norma através de datareader
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <returns>Retorna um objeto Norma preenchido</returns>
        private Norma MontaNormaSimples(IDataReader reader)
        {
            Norma norma = new NormaSimples();
            if (reader["Id"] is DBNull)
            {
                throw new Exception("Norma com campo Id nulo não é permitido.");
            }
            norma.Id = Convert.ToInt32(reader["Id"]);
            if (!(reader["IdSileg"] is DBNull))
            {
                norma.IdSileg = Convert.ToInt32(reader["IdSileg"]);
            }
            norma.Rotulo = reader["Rotulo"].ToString();
            norma.Comentario = reader["Comentario"].ToString();
            norma.Tipo = new TipoDeNormaAD().ObtemTipoDeNorma(reader["Id_Tipo"].ToString());
            norma.Numero = reader["Numero"].ToString().Replace(".", "").Replace("-", "");
            norma.NumeroString = reader["Numero"].ToString();
            if (!string.IsNullOrEmpty(reader["DataAssinatura"].ToString()))
            {
                norma.DataAssinatura = Convert.ToDateTime(reader["DataAssinatura"]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
            }
            else
            {
                throw new Exception("Norma sem data de assinatura.");
            }
            if (!(reader["NumeroSequencial"] is DBNull))
            {
                norma.NumeroSequencial = Convert.ToInt32(reader["NumeroSequencial"]);
            }
            if (!(reader["Letra"] is DBNull))
            {
                norma.Letra = Convert.ToChar(reader["Letra"]);
            }
            norma.Ambito = reader["Ambito"].ToString();
            norma.Apelido = reader["Apelido"].ToString();
            object dataDeAutuacao = reader["DataDeAutuacao"];
            if (!(dataDeAutuacao is DBNull))
            {
                norma.DataDeAutuacao = Convert.ToDateTime(reader["DataDeAutuacao"], CultureInfo.GetCultureInfo(culturaPortuguesBrasileiro));
            }
            norma.DataDaUltimaAlteracao = reader["DataDaUltimaAlteracao"] as DateTime?;
            norma.Apelido = reader["Apelido"] as string;
            bool haPendencia;
            bool.TryParse(reader["HaPendencia"].ToString(), out haPendencia);
            norma.HaPendencia = haPendencia;
            bool destacada;
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
            char letra;
            char.TryParse(reader["Letra"].ToString(), out letra);
            norma.Letra = letra;
            long numeroSequencial;
            long.TryParse(reader["NumeroSequencial"].ToString(), out numeroSequencial);
            norma.NumeroSequencial = numeroSequencial;
            norma.NomeDoOrgao = reader["NomeDoOrgao"].ToString();
            norma.Situacao = reader["Situacao"].ToString();
            norma.UsuarioQueCadastrou = reader["UsuarioQueCadastrou"] as string;
            if (!(reader["DataDoCadastro"] is DBNull))
            {
                norma.DataDoCadastro = Convert.ToDateTime(reader["DataDoCadastro"], CultureInfo.GetCultureInfo("pt-br"));
            }
            norma.UsuarioDaUltimaAlteracao = reader["UsuarioDaUltimaAlteracao"] as string;
            if (!string.IsNullOrEmpty(reader["DataDaUltimaAlteracao"] as string)) norma.DataDaUltimaAlteracao = Convert.ToDateTime(reader["DataDaUltimaAlteracao"], CultureInfo.GetCultureInfo("pt-br"));
            norma.ObservacaoNorma = reader["ObservacaoNorma"] as string;
            norma.Versao = new InformacoesSobreVersao
            {
                Id = Convert.ToUInt32(reader["Id"]),
                Rotulo = reader["Rotulo"] as string,
                Comentario = reader["Comentario"] as string
            };
            norma.ChaveParaNaoDuplicacao = reader["ChaveParaNaoDuplicacao"] as string;
            norma.UsuarioQueCadastrou = reader["UsuarioQueCadastrou"] as string;
            norma.UsuarioDaUltimaAlteracao = reader["UsuarioDaUltimaAlteracao"] as string;
            norma.ChaveParaNaoDuplicacao = reader["ChaveParaNaoDuplicacao"] as string;
            norma.Ementa = reader["Ementa"].ToString();
            norma.ListaDeNomes = reader["ListaDeNomes"].ToString();
            if (reader["Origens"] != DBNull.Value)
            {
                object[] origens = (object[])reader["Origens"];
                foreach (object idOrigem in origens)
                {
                    Orgao orgao = new OrgaoAD().ObtemOrgaoPeloId(idOrigem.ToString());
                    norma.Origens.Add(orgao);
                }
            }
            norma.ParametroConstitucional = reader["ParametroConstitucional"].ToString();
            norma.Procedencia = reader["Procedencia"].ToString();
            norma.Requerentes = new List<Requerente>();
            norma.Requeridos = new List<Requerido>();
            norma.ProcuradoresResponsaveis = new List<ProcuradorResponsavel>();
            norma.Interessados = new List<Interessado>();
            norma.Relator = reader["Relator"].ToString();
            norma.CaminhoArquivoTextoConsolidado = reader["CaminhoArquivoTextoConsolidado"].ToString();
            norma.NomeArquivoTextoConsolidado = reader["NomeArquivoTextoConsolidado"].ToString();
            norma.ConteudoArquivoTextoConsolidado = reader["ConteudoArquivoTextoConsolidado"].ToString();
            //try
            //{
            //    bool extrairTexto = bool.Parse(Configuracao.LerValorChave(chaveFlagExtrairTextoArquivos));
            //    if (extrairTexto && norma.CaminhoArquivoTextoConsolidado != "")
            //    {
            //        ManagerExtratorDeTexto managerExtratorDeTexto = new ManagerExtratorDeTexto(Configuracao.LerValorChave(chaveLightBaseConnectionString));

            //        bool salvarTexto = bool.Parse(Configuracao.LerValorChave(chaveFlagSalvarTextoArquivos));
            //        if (salvarTexto)
            //        {
            //            norma.ConteudoArquivoTextoConsolidado = managerExtratorDeTexto.ExtrairTexto(norma.Id.ToString(),
            //                                                           _extentNorma, "Id",
            //                                                           "ConteudoArquivoTextoConsolidado",
            //                                                           "CaminhoArquivoTextoConsolidado",
            //                                                           Configuracao.LerValorChave(chavePathOfTheUploadedFiles));
            //        }
            //        else
            //        {
            //            norma.ConteudoArquivoTextoConsolidado = managerExtratorDeTexto.ExtrairTexto(norma.Id.ToString(),
            //                                                       _extentNorma, "Id",
            //                                                       "CaminhoArquivoTextoConsolidado",
            //                                                       Configuracao.LerValorChave(chavePathOfTheUploadedFiles));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    AuxiliarLog.LogarExcecao("Exportação de Norma", "Erro ao extrair texto de Norma. Id " + norma.Id, ex);
            //}
            norma.ConteudoArquivoTextoAcao = reader["ConteudoArquivoTextoAcao"].ToString();
            norma.CaminhoArquivoTextoAcao = reader["CaminhoArquivoTextoAcao"].ToString();
            norma.NomeArquivoTextoAcao = reader["NomeArquivoTextoAcao"].ToString();
            object[] requerentesAux = new object[0];
            if (reader["Requerente"] != DBNull.Value) requerentesAux = (object[])reader["Requerente"];
            norma.Requerentes = new RequerenteAD().ObtemRequerentes(requerentesAux);
            object[] requeridosAux = new object[0];
            if (reader["Requerido"] != DBNull.Value) requeridosAux = (object[])reader["Requerido"];
            norma.Requeridos = new RequeridoAD().ObtemRequeridos(requeridosAux);
            object[] procuradoresAux = new object[0];
            if (reader["ProcuradorResponsavel"] != DBNull.Value) procuradoresAux = (object[])reader["ProcuradorResponsavel"];
            norma.ProcuradoresResponsaveis = new ProcuradorAD().ObtemProcuradoresResponsaveis(procuradoresAux);

            object[] interessadosAux = new object[0];
            if (reader["InteressadoDaAcao"] != DBNull.Value) interessadosAux = (object[])reader["InteressadoDaAcao"];
            norma.Interessados = new InteressadoAD().ObtemInteressados(interessadosAux);
            //object[] indexacao = new object[0];
            //if (reader["Indexacao"] != DBNull.Value) indexacao = (object[])reader["Indexacao"];
            //foreach (object termoIndexado in indexacao) if (termoIndexado != null) norma.Indexacao.Add(termoIndexado.ToString());
            norma.NeoIndexacao = MontarNeoIndexacao(reader);
            //object[] autoridades = new object[0];
            //if (reader["Autoridades"] != DBNull.Value) autoridades = (object[])reader["Autoridades"];
            //foreach (object termoDeAutoridade in autoridades) if (termoDeAutoridade != null) norma.Autoridades.Add(termoDeAutoridade.ToString());
            norma.EfeitoDaDecisao = reader["EfeitoDaDecisao"] as string;
            norma.Fontes = ObtemFontesDaNorma(reader);

            norma.Vides = ObtemVidesQueAfetamANorma(reader) as List<VideEntreNormas>;

            //Parametros para rankeamento
            //norma.paramsForIndexer = new ParamsForIndexer();
            //norma.paramsForIndexer.Numero = reader["Numero"].ToString();
            //norma.paramsForIndexer.TipoENumero = norma.Tipo.Nome + " " + norma.Numero;
            //if (!string.IsNullOrEmpty(reader["DataAssinatura"].ToString()))
            //{
            //    norma.paramsForIndexer.DataAssinatura =
            //        Convert.ToDateTime(reader["DataAssinatura"]).ToString("dd/MM/yyyy");
            //}

            //norma.paramsForIndexer.AuxiliarDeRankeamento = PreencheAuxiliarDeRankeamento(norma.Id);
            object[] auxiliaresDeRankeamento = new object[0];
            if (reader["auxiliarDeRankeamento"] != DBNull.Value) auxiliaresDeRankeamento = (object[])reader["AuxiliarDeRankeamento"];
            foreach (object auxiliarDeRankeamento in auxiliaresDeRankeamento)
            {
                if (auxiliarDeRankeamento != null) norma.AuxiliarDeRankeamento.Add(auxiliarDeRankeamento.ToString());
            }
            norma.AuxiliarDeRankeamento.Add(norma.Tipo.Nome);
            norma.AuxiliarDeRankeamento.Add(norma.Numero);
            norma.AuxiliarDeRankeamento.Add(norma.DataAssinatura);
            norma.AuxiliarDeRankeamento.Add(norma.Tipo.Nome + " " + norma.Numero);
            norma.AuxiliarDeRankeamento.Add(norma.Tipo.Nome + " " + norma.Numero + " " + norma.DataAssinatura);

            return norma;
        }

        private List<Indexacao> MontarNeoIndexacao(IDataRecord reader)
        {
            List<Indexacao> listaNeoIndexacao = new List<Indexacao>();
            if (reader["NeoIndexacao"] is DataTable)
            {
                DataTable datatable = (DataTable)reader["NeoIndexacao"];
                foreach (DataRow dataRow in datatable.Rows)
                {
                    Indexacao neoIndexacao = new Indexacao();
                    neoIndexacao.InTipoTermo = Convert.ToInt32(dataRow["InTipoTermo"]);
                    neoIndexacao.NmTermo = dataRow["NmTermo"].ToString();
                    neoIndexacao.NmEspecificador = dataRow["NmEspecificador"].ToString();
                    neoIndexacao.NmTermoAuxiliar = dataRow["NmTermoAuxiliar"].ToString();
                    neoIndexacao.NmEspecificadorAuxiliar = dataRow["NmEspecificadorAuxiliar"].ToString();
                    listaNeoIndexacao.Add(neoIndexacao);
                }
            }
            return listaNeoIndexacao;
        }

        /// <summary>
        /// Obtem um IEnumerable de Fontes da Norma
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <returns>Retorna uma IEnumerable de Fontes</returns>
        private List<Fonte> ObtemFontesDaNorma(IDataRecord reader)
        {
            List<Fonte> listaDeFontes = new List<Fonte>();
            if (reader["Fontes"] is DataTable)
            {
                DataTable dadosDasFontes = (DataTable)reader["Fontes"];
                foreach (DataRow dadosDaFonte in dadosDasFontes.Rows) listaDeFontes.Add(CarregaFonte(dadosDaFonte));
            }
            return listaDeFontes;
        }

        /// <summary>
        /// Preenche o objeto Fonte da Norma
        /// </summary>
        /// <param name="dataRowFonte">DataRow</param>
        /// <returns>Retorna o objeto Fonte Preenchido</returns>
        private Fonte CarregaFonte(DataRow dataRowFonte)
        {
            try
            {
                Fonte fonte = new Fonte();
                fonte.Id = new Guid(dataRowFonte["IdDaFonte"].ToString());
                fonte.TipoFonte = new TipoDeFonteBO(dataRowFonte["TipoFonte"].ToString());
                fonte.TipoEdicao =
                    (TipoDeEdicao)Enum.Parse(typeof(TipoDeEdicao), dataRowFonte["TipoEdicao"].ToString());
                fonte.TipoPublicacao = new TipoDePublicacaoBO { Nome = dataRowFonte["TipoPublicacao"].ToString() };
                if (!(dataRowFonte["DataPublicacao"] is DBNull))
                {
                    fonte.DataPublicacao = Convert.ToDateTime(dataRowFonte["DataPublicacao"]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                    fonte.DataPublicacaoCompare = Convert.ToDateTime(dataRowFonte["DataPublicacao"]);
                }
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
                fonte.Observacao = dataRowFonte["Observacao"] as string;
                fonte.MotivoReduplicacao = dataRowFonte["MotivoReduplicacao"] as string;
                fonte.ConteudoArquivoTexto = dataRowFonte["ConteudoArquivoTexto"] as string;
                //Note: Essa bosta tá invertida no banco (hora sim, hora não), ou seja, "CaminhoArquivoTexto" no lugar de "NomeArquivoTexto". Para não ter que ajustar no banco registro a registro fiz essa bosta dessa "gambiarra".
                //Note: Essa validação não está segura pois se baseia na existência de '\' no campo "NomeArquivoTexto" para verificar quando "CaminhoArquivoTexto" está no lugar de "NomeArquivoTexto".
                if (dataRowFonte["NomeArquivoTexto"].ToString().IndexOf('\\') == -1)
                {
                    fonte.NomeArquivoTexto = dataRowFonte["NomeArquivoTexto"] as string;
                    fonte.CaminhoArquivoTexto = dataRowFonte["CaminhoArquivoTexto"] as string;
                }
                else
                {
                    fonte.NomeArquivoTexto = dataRowFonte["CaminhoArquivoTexto"] as string;
                    fonte.CaminhoArquivoTexto = dataRowFonte["NomeArquivoTexto"] as string;
                }
                return fonte;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível carregar fonte de ID: " + dataRowFonte["IdDaFonte"].ToString(), ex);
            }
        }

        /// <summary>
        /// Obtem um IEnumerable de Vides da Norma
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <returns>Retorna um IEnumerable IEnumerable de Vides</returns>
        private IEnumerable<VideEntreNormas> ObtemVidesQueAfetamANorma(IDataRecord reader)
        {
            List<VideEntreNormas> listaDeVides = new List<VideEntreNormas>();
            if (reader["Vides"] is DataTable)
            {
                DataTable dadosDosVides = (DataTable)reader["Vides"];
                foreach (DataRow dadosDoVide in dadosDosVides.Rows) listaDeVides.Add(CarregaVide(dadosDoVide));
            }
            return listaDeVides;
        }

        /// <summary>
        /// Preenche o Objeto VideEntreNormas
        /// </summary>
        /// <param name="dataRowFonte">DataRow</param>
        /// <returns>Retorna o objeto VideEntreNormas preenchido</returns>
        private VideEntreNormas CarregaVide(DataRow dataRowFonte)
        {
            try
            {
                VideEntreNormas vide = new VideEntreNormas();
                vide.Id = new Guid(dataRowFonte["idVide"].ToString());
                vide.ComentarioVide = dataRowFonte["ComentarioVide"] as string;
                vide.ArtigoDaNormaPosterior = dataRowFonte["ArtigoDaNormaPosterior"] as string;
                vide.ParagrafoDaNormaPosterior = dataRowFonte["ParagrafoDaNormaPosterior"] as string;
                vide.IncisoDaNormaPosterior = dataRowFonte["IncisoDaNormaPosterior"] as string;
                vide.AlineaDaNormaPosterior = dataRowFonte["AlineaDaNormaPosterior"] as string;
                vide.ItemDaNormaPosterior = dataRowFonte["ItemDaNormaPosterior"] as string;
                vide.CaputDaNormaPosterior = dataRowFonte["CaputDaNormaPosterior"] as string;
                vide.AnexoDaNormaPosterior = dataRowFonte["AnexoDaNormaPosterior"] as string;
                vide.ArtigoDaNormaAnterior = dataRowFonte["ArtigoDaNormaAnterior"] as string;
                vide.ParagrafoDaNormaAnterior = dataRowFonte["ParagrafoDaNormaAnterior"] as string;
                vide.IncisoDaNormaAnterior = dataRowFonte["IncisoDaNormaAnterior"] as string;
                vide.AlineaDaNormaAnterior = dataRowFonte["AlineaDaNormaAnterior"] as string;
                vide.ItemDaNormaAnterior = dataRowFonte["ItemDaNormaAnterior"] as string;
                vide.CaputDaNormaAnterior = dataRowFonte["CaputDaNormaAnterior"] as string;
                vide.AnexoDaNormaAnterior = dataRowFonte["AnexoDaNormaAnterior"] as string;
                vide.PaginaDaPublicacaoPosterior = dataRowFonte["PaginaDaPublicacaoPosterior"] as string;
                vide.ColunaDaPublicacaoPosterior = dataRowFonte["ColunaDaPublicacaoPosterior"] as string;
                vide.NumeroDaNormaPosterior = dataRowFonte["NumeroDaNormaPosterior"] as string;
                vide.IdDaNormaPosterior = Convert.ToInt32(dataRowFonte["IdDaNormaPosterior"]);
                if (!(dataRowFonte["DataDaNormaPosterior"] is DBNull))
                    if (!(dataRowFonte["DataDaNormaPosterior"].ToString()).Equals(""))
                        vide.DataDaNormaPosterior = Convert.ToDateTime(dataRowFonte["DataDaNormaPosterior"]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                if (!(dataRowFonte["IdTipoDaNormaPosterior"] is DBNull))
                    vide.TipoDeNorma = new TipoDeNorma { Id = Convert.ToInt32(dataRowFonte["IdTipoDaNormaPosterior"]) };
                //TODO: Essa verificação está tosca. Contorno.
                if (!(dataRowFonte["DataDePublicacaoPosterior"] is DBNull))
                    if (!(dataRowFonte["DataDePublicacaoPosterior"].ToString()).Equals(""))
                        vide.DataDePublicacaoPosterior = Convert.ToDateTime(dataRowFonte["DataDePublicacaoPosterior"]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                object tipoDeVinculo = Util.GetDBValueFromObject(dataRowFonte["IdTipoDeRelacaoDeVinculo"]);
                object tipoDeFonte = dataRowFonte["TipoDaFontePosterior"];
                if (!(tipoDeVinculo is DBNull))
                    vide.TipoDeVinculo = ObtemTipoDeRelacaoDeVinculo(tipoDeVinculo.ToString());
                if (!(tipoDeFonte is DBNull)) vide.TipoDeFonte = new TipoDeFonteBO(Convert.ToString(tipoDeFonte));
                return vide;
            }
            catch (Exception ex)
            {
                throw new Exception("Não possível carregar Vide de ID: " + dataRowFonte["idVide"].ToString(), ex);
            }
        }

        private TipoDeRelacaoDeVinculo ObtemTipoDeRelacaoDeVinculo(string id)
        {
            TipoDeRelacaoDeVinculo tipoDeRelacaoDeVinculo = new TipoDeRelacaoDeVinculo();
            var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            conn.OpenConnection();
            //LightBaseCommand command = new LightBaseCommand(string.Format("select * from TiposDeRelacao where oid = {0}", id), lightBaseConnection);
            string sql = string.Format("select * from TiposDeRelacao where oid = {0}", id);
            using (var rdr = conn.ExecuteDataReader(sql))
            {
                if (rdr.Read())
                {
                    if (rdr["oid"] != DBNull.Value)
                    {
                        tipoDeRelacaoDeVinculo.Oid = Convert.ToInt32(rdr["oid"]);
                    }
                    tipoDeRelacaoDeVinculo.Conteudo = rdr["Conteudo"].ToString();
                    tipoDeRelacaoDeVinculo.Descricao = rdr["Descricao"].ToString();
                    tipoDeRelacaoDeVinculo.TextoParaAlterador = rdr["TextoParaAlterador"].ToString();
                    tipoDeRelacaoDeVinculo.TextoParaAlterado = rdr["TextoParaAlterado"].ToString();
                    if (rdr["RelacaoDeAcao"] != DBNull.Value)
                    {
                        tipoDeRelacaoDeVinculo.RelacaoDeAcao = Convert.ToBoolean(rdr["RelacaoDeAcao"]);
                    }
                    if (rdr["Importancia"] != DBNull.Value)
                    {
                        tipoDeRelacaoDeVinculo.Importancia = Convert.ToInt32(rdr["Importancia"]);
                    }
                }
            }
            conn.CloseConection();
            return tipoDeRelacaoDeVinculo;
        }

        //private List<string> PreencheAuxiliarDeRankeamento(int id)
        //{
        //    List<string> auxiliarDeRankeamento = new List<string>();
        //    var ObjetoDesserializado = Configuracao.ObjetoDesserializado;
        //    foreach (var paramsForIndexerPorNormas in ObjetoDesserializado.paramsForIndexerPorNormas)
        //    {
        //        if (paramsForIndexerPorNormas.Id == id)
        //        {
        //            auxiliarDeRankeamento = paramsForIndexerPorNormas.AuxiliarDeRankeamento;
        //            break;
        //        }
        //    }
        //    return auxiliarDeRankeamento;
        //}

        /// <summary>
        /// Busca uma norma especifica, de início servirá para prencher algumas informações do PUSH antes de enviar ao ElasticSearch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Norma BuscarNormaPeloIdParaPush(string id)
        {
            NormaSimples norma = new NormaSimples();
            try
            {
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                string sql = string.Format("select id, id_tipo, dataassinatura, numero from {0} where Id = {1}", _extentNorma, id);

                using (var rdr = conn.ExecuteDataReader(sql))
                {
                    if (rdr.Read())
                    {
                        try
                        {
                            norma.Tipo = new TipoDeNormaAD().ObtemTipoDeNorma(rdr["id_tipo"].ToString());
                            norma.DataAssinatura = (Convert.ToDateTime(rdr["DataAssinatura"])).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                            norma.Numero = rdr["Numero"].ToString();
                        }
                        catch (Exception ex)
                        {
                            Log.LogarExcecao("Exportação de Push", "Montando Norma pelo ID para push de Id " + rdr["Id"], ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Push", "Montando Norma pelo ID para push de Id " + id, ex);
            }
            return norma;
        }

        public List<RegistrosParaExportar> BuscarCaminhosDeArquivosDeNorma(string sql)
        {
            List<RegistrosParaExportar> registrosParaExportar = new List<RegistrosParaExportar>();
            try
            {
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                using (var rdr = conn.ExecuteDataReader(sql))
                {
                    while (rdr.Read())
                    {
                        try
                        {
                            string campoCaminhoDeArquivo = Configuracao.LerValorChave(chaveCampoCaminhoArquivoTextoNormas);
                            if (rdr[campoCaminhoDeArquivo].ToString() != "")
                            {
                                RegistrosParaExportar registro = new RegistrosParaExportar();
                                registro.Id = rdr["id"].ToString();
                                registro.Nome = _extentNorma;
                                registro.CaminhoArquivoTexto = rdr[campoCaminhoDeArquivo].ToString();
                                registrosParaExportar.Add(registro);
                            }
                            List<Fonte> fontes = ObtemFontesDaNorma(rdr).ToList();
                            foreach (Fonte fonte in fontes)
                            {
                                if (!string.IsNullOrEmpty(fonte.CaminhoArquivoTexto))
                                {
                                    RegistrosParaExportar registro = new RegistrosParaExportar();
                                    registro.Id = fonte.Id.ToString();
                                    registro.Nome = _extentNorma;
                                    registro.CaminhoArquivoTexto = fonte.CaminhoArquivoTexto;
                                    registrosParaExportar.Add(registro);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.LogarExcecao("Exportação de arquivos de Normas", "Erro ao obter arquivos da normas. Norma = " + rdr["Id"], ex);
                        }
                    }
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Arquivos de Norma", "Erro ao consultar caminhos de arquivos das normas.", ex);
            }
            return registrosParaExportar;
        }

        public void ConfigurarMapping()
        {
            string extent = _extentNorma;
            string jsonMapping = Configuracao.LerValorChave(chaveMappingNormas);
            string uriElasticSearch = Configuracao.LerValorChave(chaveElasticSearch);
            if (jsonMapping != "")
            {
                new EsAD().ConfigurarIndexType(uriElasticSearch, jsonMapping, extent);
            }
        }
    }
}