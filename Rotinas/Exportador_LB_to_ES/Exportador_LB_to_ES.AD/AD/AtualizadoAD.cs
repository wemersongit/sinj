using System;
using System.Collections.Generic;
using System.Linq;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class AtualizadoAD : AD
    {
        private string _extentAtualizado;
        public string ExtentAtualizado
        {
            get { return _extentAtualizado; }
        }
        public AtualizadoAD()
        {
            _extentAtualizado = Configuracao.LerValorChave(chaveBaseAtualizados);
        }

        public void DespacharAtualizadosParaIndexacaoEExportacao(List<RegistrosParaExportar> lista)
        {
            string extentNorma = Configuracao.LerValorChave(chaveBaseNormas);
            string extentDodf = Configuracao.LerValorChave(chaveBaseDodf);
            string extentTipoDeNorma = Configuracao.LerValorChave(chaveBaseTipoDeNorma);
            string extentOrgao = Configuracao.LerValorChave(chaveBaseOrgaos);
            string extentInteressado = Configuracao.LerValorChave(chaveBaseInteressado);
            string extentAutoria = Configuracao.LerValorChave(chaveBaseAutoria);
            string extentRequerente = Configuracao.LerValorChave(chaveBaseRequerente);
            string extentRequerido = Configuracao.LerValorChave(chaveBaseRequerido);
            string extentPush = Configuracao.LerValorChave(chaveBasePush);
            string extentTipoDeRelacao = Configuracao.LerValorChave(chaveBaseTipoDeRelacao);
            string extentVocabularioControlado = Configuracao.LerValorChave(chaveBaseVocabularioControlado);

            IEnumerable<RegistrosParaExportar> ieNormas = from l in lista where l.Nome == extentNorma select l;
            if (ieNormas.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieNormas)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentNorma, where);
                new NormaAD().BuscarNormasEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieDodfs = from l in lista where l.Nome == extentDodf select l;
            if (ieDodfs.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieDodfs)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentDodf, where);
                new DodfAD().BuscarDodfsEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieTiposdenorma = from l in lista where l.Nome == extentTipoDeNorma select l;
            if (ieTiposdenorma.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieTiposdenorma)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentTipoDeNorma, where);
                new TipoDeNormaAD().BuscarTiposDeNormaEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieOrgaos = from l in lista where l.Nome == extentOrgao select l;
            if (ieOrgaos.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieOrgaos)
                {
                    if (where == "")
                    {
                        where = "where Id_Orgao = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id_Orgao = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentOrgao, where);
                new OrgaoAD().BuscarOrgaosEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieInteressados = from l in lista where l.Nome == extentInteressado select l;
            if (ieInteressados.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieInteressados)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentInteressado, where);
                new InteressadoAD().BuscarInteressadosEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieAutorias = from l in lista where l.Nome == extentAutoria select l;
            if (ieAutorias.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieAutorias)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentAutoria, where);
                new AutoriaAD().BuscarAutoriasEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieRequerentes = from l in lista where l.Nome == extentRequerente select l;
            if (ieRequerentes.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieRequerentes)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentRequerente, where);
                new RequerenteAD().BuscarRequerentesEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieRequeridos = from l in lista where l.Nome == extentRequerido select l;
            if (ieRequeridos.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieRequeridos)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentRequerido, where);
                new RequeridoAD().BuscarRequeridosEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> iePush = from l in lista where l.Nome == extentPush select l;
            if (iePush.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in iePush)
                {
                    if (where == "")
                    {
                        where = "where Id = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or Id = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentPush, where);
                new PushAD().BuscarPushEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieTiposderelacao = from l in lista where l.Nome == extentTipoDeRelacao select l;
            if (ieTiposderelacao.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieTiposderelacao)
                {
                    if (where == "")
                    {
                        where = "where oid = " + reg.Id;
                    }
                    else
                    {
                        where = where + " or oid = " + reg.Id;
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentTipoDeRelacao, where);
                new TipoDeRelacaoAD().BuscarTiposDeRelacaoEIndexar(sql);
            }

            IEnumerable<RegistrosParaExportar> ieTermos = from l in lista where l.Nome == extentVocabularioControlado select l;
            if (ieTermos.Count() > 0)
            {
                string where = "";
                foreach (RegistrosParaExportar reg in ieTermos)
                {
                    if (where == "")
                    {
                        where = "where Id_Termo = \"" + reg.Id + "\"";
                    }
                    else
                    {
                        where = where + " or Id_Termo = \"" + reg.Id + "\"";
                    }
                }
                string sql = string.Format("select * from {0} {1}", extentVocabularioControlado, where);
                new TermoAD().BuscarVocabularioControladoEIndexar(sql);
            }

        }

        public List<RegistrosParaExportar> BuscarAtualizados(string query)
        {
            string nomeDoExtent = Configuracao.LerValorChave(chaveBaseAtualizados);
            List<RegistrosParaExportar> lista = new List<RegistrosParaExportar>();
            try
            {
                int total = 0;
                Console.WriteLine("Iniciando Thread " + nomeDoExtent + "...");
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                using (var reader = conn.ExecuteDataReader(query))
                {
                    total = reader.Count;
                    while (reader.Read())
                    {
                        try
                        {
                            RegistrosParaExportar registro = new RegistrosParaExportar();
                            registro.Id = reader["Identificador"].ToString();
                            registro.Nome = reader["Tipo"].ToString();
                            registro.CaminhoArquivoTexto = reader["CaminhoArquivoTexto"].ToString();
                            lista.Add(registro);
                            Console.WriteLine("----------> Atualizado montado: " + registro.Nome + " " + registro.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro na busca de " + nomeDoExtent + "...");
                        }
                    }
                }
                Console.WriteLine("------ Total de registros recuperados " + nomeDoExtent + " : " + total);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na busca de " + nomeDoExtent + "...");
                Log.LogarExcecao("Busca de Atualizados", "Erro na busca de " + nomeDoExtent + "...", ex);
            }
            return lista;
        }

        public void DeletarRegistrosBaseForIndexerAndExporter()
        {
            int deletados = 0;
            try
            {
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                string sqlDelete = string.Format("delete from {0}", Configuracao.LerValorChave(chaveBaseAtualizados));
                deletados = conn.ExecuteNonQuery(sqlDelete);

            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Atualizados", "Erro deletando os registros de BaseForIndexerAndExporter", ex);
            }

            Console.WriteLine("----------> Registros deletados na base BaseForIndexerAndExporter montado: " + deletados);
        }
    }
}