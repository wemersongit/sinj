using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.RN;
using MigradorSINJ.OV;
using util.BRLight;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace MigradorSINJ.ConsoleApp
{
    class Program
    {
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"jsons\";
        private List<UsuarioOV> _usuarios;
        private List<AutoriaOV> _autorias;
        private List<InteressadoOV> _interessados;
        private List<RelatorOV> _relatores;
        private List<RequerenteOV> _requerentes;
        private List<RequeridoOV> _requeridos;
        private List<SituacaoOV> _situacoes;
        private List<TipoDeNormaOV> _tipos_de_norma;
        private List<OrgaoOV> _orgaos;
        private List<TipoDeFonteOV> _tipos_de_fonte;
        private List<TipoDePublicacaoOV> _tipos_de_publicacao;
        private List<TipoDeRelacaoOV> _tipos_de_relacao;
        private List<VocabularioControladoOV> _indexacoes;
        public List<string> lista_controle_action;
        public List<string> lista_normas_erradas;
        public List<VocabularioControladoLBW> vocabulariosControladosLbw;
        public List<string> vocabulariosControladosLbw2;
        private StringBuilder sb_normas_erradas;
        private int count_normas_erradas;

        static void Main(string[] args)
        {
            var program = new Program();
            try
            {
                program.lista_controle_action = new List<string>();
                args = new string[] { "ajustar_indexacao" };
                if (args.Length == 1)
                {
                //    if (args[0] == "atualizar_indexacao")
                //    {
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.AtualizarIndexacao();
                //    }
                //    if (args[0] == "partial_salvar_json")
                //    {
                //        program.MigrarUsuarios("salvar_json");
                //        program.MigrarAutorias("salvar_json");
                //        program.MigrarInteressados("salvar_json");
                //        program.MigrarRelatores("salvar_json");
                //        program.MigrarRequerentes("salvar_json");
                //        program.MigrarRequeridos("salvar_json");
                //        program.MigrarSituacoes("salvar_json");
                //        program.MigrarTiposDeNorma("salvar_json");
                //        program.MigrarOrgaos("salvar_json");
                //        program.MigrarTiposDeFonte("salvar_json");
                //        program.MigrarTiposDePublicacao("salvar_json");
                //        program.MigrarTiposDeRelacao("salvar_json");
                //        program.MigrarVocabularioControlado("salvar_json");
                //    }
                //    if (args[0] == "full_salvar_json")
                //    {
                //        program.MigrarUsuarios("salvar_json");
                //        program.MigrarAutorias("salvar_json");
                //        program.MigrarInteressados("salvar_json");
                //        program.MigrarRelatores("salvar_json");
                //        program.MigrarRequerentes("salvar_json");
                //        program.MigrarRequeridos("salvar_json");
                //        program.MigrarSituacoes("salvar_json");
                //        program.MigrarTiposDeNorma("salvar_json");
                //        program.MigrarOrgaos("salvar_json");
                //        program.MigrarTiposDeFonte("salvar_json");
                //        program.MigrarTiposDePublicacao("salvar_json");
                //        program.MigrarTiposDeRelacao("salvar_json");
                //        program.MigrarVocabularioControlado("salvar_json");
                //        program.MigrarNormas("salvar_json");
                //        program.MigrarDiarios("salvar_json");
                //    }
                //    if (args[0] == "normas_e_diarios_salvar_json")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarAutorias("subir_json");
                //        program.MigrarInteressados("subir_json");
                //        program.MigrarRelatores("subir_json");
                //        program.MigrarRequerentes("subir_json");
                //        program.MigrarRequeridos("subir_json");
                //        program.MigrarSituacoes("subir_json");
                //        program.MigrarTiposDeNorma("subir_json");
                //        program.MigrarOrgaos("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarTiposDePublicacao("subir_json");
                //        program.MigrarTiposDeRelacao("subir_json");
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.MigrarNormas("salvar_json");
                //        program.MigrarDiarios("salvar_json");
                //    }
                //    if (args[0] == "normas_salvar_json")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarAutorias("subir_json");
                //        program.MigrarInteressados("subir_json");
                //        program.MigrarRelatores("subir_json");
                //        program.MigrarRequerentes("subir_json");
                //        program.MigrarRequeridos("subir_json");
                //        program.MigrarSituacoes("subir_json");
                //        program.MigrarTiposDeNorma("subir_json");
                //        program.MigrarOrgaos("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarTiposDePublicacao("subir_json");
                //        program.MigrarTiposDeRelacao("subir_json");
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.MigrarNormas("salvar_json");
                //    }
                //    if (args[0] == "diarios_salvar_json")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarDiarios("salvar_json");
                //    }
                //    if (args[0] == "normas_e_diarios_json")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarAutorias("subir_json");
                //        program.MigrarInteressados("subir_json");
                //        program.MigrarRelatores("subir_json");
                //        program.MigrarRequerentes("subir_json");
                //        program.MigrarRequeridos("subir_json");
                //        program.MigrarSituacoes("subir_json");
                //        program.MigrarTiposDeNorma("subir_json");
                //        program.MigrarOrgaos("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarTiposDePublicacao("subir_json");
                //        program.MigrarTiposDeRelacao("subir_json");
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.MigrarNormas("migrar_do_arquivo");
                //        program.MigrarDiarios("migrar_do_arquivo");
                //        program.MigrarArquivosNormas();
                //        program.MigrarArquivosDiarios();
                //    }
                //    if (args[0] == "full_json")
                //    {
                //        program.MigrarUsuarios("migrar_do_arquivo");
                //        program.MigrarAutorias("migrar_do_arquivo");
                //        program.MigrarInteressados("migrar_do_arquivo");
                //        program.MigrarRelatores("migrar_do_arquivo");
                //        program.MigrarRequerentes("migrar_do_arquivo");
                //        program.MigrarRequeridos("migrar_do_arquivo");
                //        program.MigrarSituacoes("migrar_do_arquivo");
                //        program.MigrarTiposDeNorma("migrar_do_arquivo");
                //        program.MigrarOrgaos("migrar_do_arquivo");
                //        program.MigrarTiposDeFonte("migrar_do_arquivo");
                //        program.MigrarTiposDePublicacao("migrar_do_arquivo");
                //        program.MigrarTiposDeRelacao("migrar_do_arquivo");
                //        program.MigrarVocabularioControlado("migrar_do_arquivo");
                //        program.MigrarNormas("migrar_do_arquivo");
                //        program.MigrarDiarios("migrar_do_arquivo");
                //        program.MigrarArquivosNormas();
                //        program.MigrarArquivosDiarios();
                //    }
                //    if (args[0] == "full_banco")
                //    {
                //        program.MigrarUsuarios();
                //        program.MigrarAutorias();
                //        program.MigrarInteressados();
                //        program.MigrarRelatores();
                //        program.MigrarRequerentes();
                //        program.MigrarRequeridos();
                //        program.MigrarSituacoes();
                //        program.MigrarTiposDeNorma();
                //        program.MigrarOrgaos();
                //        program.MigrarTiposDeFonte();
                //        program.MigrarTiposDePublicacao();
                //        program.MigrarTiposDeRelacao();
                //        program.MigrarVocabularioControlado();
                //        program.MigrarNormas();
                //        program.MigrarDiarios();
                //        program.MigrarArquivosNormas();
                //        program.MigrarArquivosDiarios();
                //    }
                //    if (args[0] == "parcial_banco")
                //    {
                //        program.MigrarUsuarios();
                //        program.MigrarAutorias();
                //        program.MigrarInteressados();
                //        program.MigrarRelatores();
                //        program.MigrarRequerentes();
                //        program.MigrarRequeridos();
                //        program.MigrarSituacoes();
                //        program.MigrarTiposDeNorma();
                //        program.MigrarOrgaos();
                //        program.MigrarTiposDeFonte();
                //        program.MigrarTiposDePublicacao();
                //        program.MigrarTiposDeRelacao();
                //        program.MigrarVocabularioControlado();
                //    }
                //    if (args[0] == "parcial_json")
                //    {
                //        program.MigrarUsuarios("migrar_do_arquivo");
                //        program.MigrarAutorias("migrar_do_arquivo");
                //        program.MigrarInteressados("migrar_do_arquivo");
                //        program.MigrarRelatores("migrar_do_arquivo");
                //        program.MigrarRequerentes("migrar_do_arquivo");
                //        program.MigrarRequeridos("migrar_do_arquivo");
                //        program.MigrarSituacoes("migrar_do_arquivo");
                //        program.MigrarTiposDeNorma("migrar_do_arquivo");
                //        program.MigrarOrgaos("migrar_do_arquivo");
                //        program.MigrarTiposDeFonte("migrar_do_arquivo");
                //        program.MigrarTiposDePublicacao("migrar_do_arquivo");
                //        program.MigrarTiposDeRelacao("migrar_do_arquivo");
                //        program.MigrarVocabularioControlado("migrar_do_arquivo");
                //    }
                //    if (args[0] == "normas_json")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarAutorias("subir_json");
                //        program.MigrarInteressados("subir_json");
                //        program.MigrarRelatores("subir_json");
                //        program.MigrarRequerentes("subir_json");
                //        program.MigrarRequeridos("subir_json");
                //        program.MigrarSituacoes("subir_json");
                //        program.MigrarTiposDeNorma("subir_json");
                //        program.MigrarOrgaos("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarTiposDePublicacao("subir_json");
                //        program.MigrarTiposDeRelacao("subir_json");
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.MigrarNormas("migrar_do_arquivo");
                //    }
                //    if (args[0] == "normas_json_e_arquivos")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarAutorias("subir_json");
                //        program.MigrarInteressados("subir_json");
                //        program.MigrarRelatores("subir_json");
                //        program.MigrarRequerentes("subir_json");
                //        program.MigrarRequeridos("subir_json");
                //        program.MigrarSituacoes("subir_json");
                //        program.MigrarTiposDeNorma("subir_json");
                //        program.MigrarOrgaos("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarTiposDePublicacao("subir_json");
                //        program.MigrarTiposDeRelacao("subir_json");
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.MigrarNormas("migrar_do_arquivo");
                //        program.MigrarArquivosNormas();
                //    }
                //    if (args[0] == "normas_banco")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarAutorias("subir_json");
                //        program.MigrarInteressados("subir_json");
                //        program.MigrarRelatores("subir_json");
                //        program.MigrarRequerentes("subir_json");
                //        program.MigrarRequeridos("subir_json");
                //        program.MigrarSituacoes("subir_json");
                //        program.MigrarTiposDeNorma("subir_json");
                //        program.MigrarOrgaos("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarTiposDePublicacao("subir_json");
                //        program.MigrarTiposDeRelacao("subir_json");
                //        program.MigrarVocabularioControlado("subir_json");
                //        program.MigrarNormas();
                //    }
                //    if (args[0] == "diarios_json")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarDiarios("migrar_do_arquivo");
                //    }
                //    if (args[0] == "diarios_json_e_arquivos")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarDiarios("migrar_do_arquivo");
                //        program.MigrarArquivosDiarios();
                //    }
                //    if (args[0] == "diarios_banco")
                //    {
                //        program.MigrarUsuarios("subir_json");
                //        program.MigrarTiposDeFonte("subir_json");
                //        program.MigrarDiarios();
                //    }
                //    if (args[0] == "arquivos")
                //    {
                //        program.MigrarArquivosNormas();
                //        program.MigrarArquivosDiarios();
                //    }
                //    if (args[0] == "arquivos_normas")
                //    {
                //        program.MigrarArquivosNormas();
                //    }
                //    if (args[0] == "arquivos_diarios")
                //    {
                //        program.MigrarArquivosDiarios();
                    //    }
                    //if (args[0] == "usuarios_push")
                    //{
                    //    program.MigrarUsuariosPush();
                    //}
                    //if (args[0] == "atualizar_cadastro_norma")
                    //{
                    //    program.MigrarUsuarios("subir_json");
                    //    program.AtualizarUsuarioCadastrador();
                    //}
                    //if (args[0] == "buscar_normas_rest")
                    //{
                    //    program.TesteBuscaNormaRest();
                    //}
                    //if (args[0] == "teste_normas_lbw")
                    //{
                    //    program.TesteNormasLbw();
                    //}
                    if (args[0] == "ajustar_indexacao")
                    {
                        program.AjustarIndexacao();
                        //program.GerarJSONLegadoLBW();
                    }
                }
                //else
                //{
                //    program.MigrarUsuarios("subir_json");
                //    program.MigrarAutorias("subir_json");
                //    program.MigrarInteressados("subir_json");
                //    program.MigrarRelatores("subir_json");
                //    program.MigrarRequerentes("subir_json");
                //    program.MigrarRequeridos("subir_json");
                //    program.MigrarSituacoes("subir_json");
                //    program.MigrarTiposDeNorma("subir_json");
                //    program.MigrarOrgaos("subir_json");
                //    program.MigrarTiposDeFonte("subir_json");
                //    program.MigrarTiposDePublicacao("subir_json");
                //    program.MigrarTiposDeRelacao("subir_json");
                //    program.MigrarVocabularioControlado("subir_json");
                //    program.MigrarNormas();
                //    program.MigrarDiarios();
                //    program.MigrarArquivosNormas();
                //    program.MigrarArquivosDiarios();
                //}
            }
            catch(Exception ex){
                program.EscreverLog(ex);
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        public void AjustarIndexacao()
        {
            //Inicializar variaveis
            var normaRn = new NormaRN();
            var vocabularioRn = new VocabularioControladoRN();
            REST _rest = new REST("sinj_vocabulario");

            var filename = "C:/temp/legado_normas_inconsistentes_unificado.txt";
            //var filename = "C:/temp/legado_normas_inconsistentes_que_tinha_dado_erro.txt";
            //var filename = "C:/temp/legado_normas_inconsistentes_sem_erro.txt";
            var lista_normas_ajuste = normaRn.BuscarNormaJSON(filename);

            var descritores = new List<VocabularioControladoOV>();
            var especificadores = new List<VocabularioControladoOV>();
            var descritores_nao_encontrados = new List<NeoIndexacao>();
            var especificadores_nao_encontrados = new List<NeoIndexacao>();
            var termos_nao_existentes = new List<NeoIndexacao>();
            var count_normas_verificadas = 0;
            var excecoes = new List<Exception>();

            foreach (var json_norma in lista_normas_ajuste)
            {
                count_normas_verificadas++;
                try
                {
                    var obj_legado_norma = JSON.Deserializa<LegadoNormaLBW>(json_norma);
                    var normaOv = normaRn.BuscarNorma(obj_legado_norma.Id, new string[0]);
                    if (normaOv != null)
                    {
                        foreach (var neoIndexacao in obj_legado_norma.NeoIndexacao)
                        {
                            // Se tem NmEspecificador nesse neoIndexacao
                            if (!string.IsNullOrEmpty(neoIndexacao.NmEspecificador))
                            {
                                // Verifica se esse especificador ja existe na norma no REST
                                var tem_especificador = normaOv.indexacoes.Exists(i => i.vocabulario.Exists(v => v.nm_termo == neoIndexacao.NmEspecificador.Trim()));

                                // Se nao tem, procura um equivalente que possa ser atribuido
                                if (!tem_especificador)
                                {
                                    // Verifica se nao foi processado ainda, para nao haver duplicaçoes
                                    if (!termos_nao_existentes.Exists(i => i.NmEspecificador == neoIndexacao.NmEspecificador.Trim()))
                                    {
                                        termos_nao_existentes.Add(neoIndexacao);
                                        string consulta = "$$={\"literal\":\"ch_tipo_termo='ES' and nm_termo='" + neoIndexacao.NmEspecificador.ToLower().Trim() + "'\", \"limit\":1}";
                                        var sResults = _rest.Consultar(consulta);
                                        var oResults = JSON.Deserializa<Results<VocabularioControladoOV>>(sResults);
                                        if (oResults.result_count > 0)
                                        {
                                            especificadores.Add(oResults.results[0]);
                                        }
                                        else
                                        {
                                            especificadores_nao_encontrados.Add(neoIndexacao);
                                        }
                                    }
                                }
                            }

                            else
                            {
                                // Verifica se esse termo ja existe na norma no REST
                                var tem = normaOv.indexacoes.Exists(i => i.vocabulario.Exists(v => v.nm_termo == neoIndexacao.NmTermo.Trim()));

                                if (!tem)
                                {
                                    // Se o termo ainda nao foi processado
                                    if (!termos_nao_existentes.Exists(i => i.NmTermo == neoIndexacao.NmTermo.Trim() && i.InTipoTermo == neoIndexacao.InTipoTermo))
                                    {
                                        termos_nao_existentes.Add(neoIndexacao);

                                        // Procurar um termo equivalente
                                        // Se começar com maiusculo, procura DESCRITOR

                                        bool maiusculo = false;
                                        foreach (var caracter in neoIndexacao.NmTermo)
                                        {
                                            if (Char.IsLetter(caracter))
                                            {
                                                if (Char.IsUpper(caracter))
                                                {
                                                    maiusculo = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (maiusculo)
                                        {
                                            string consulta = "$$={\"literal\":\"ch_tipo_termo='DE' and nm_termo='" + neoIndexacao.NmTermo.ToUpper().Trim() + "'\", \"limit\":1}";
                                            var sResults = _rest.Consultar(consulta);
                                            var oResults = JSON.Deserializa<Results<VocabularioControladoOV>>(sResults);
                                            if (oResults.result_count > 0)
                                            {
                                                descritores.Add(oResults.results[0]);
                                            }
                                            else
                                            {
                                                descritores_nao_encontrados.Add(neoIndexacao);
                                            }
                                        }

                                        // Se nao, especificador
                                        else
                                        {
                                            string consulta = "$$={\"literal\":\"ch_tipo_termo='ES' and nm_termo='" + neoIndexacao.NmTermo.ToUpper().Trim() + "'\", \"limit\":1}";
                                            var sResults = _rest.Consultar(consulta);
                                            var oResults = JSON.Deserializa<Results<VocabularioControladoOV>>(sResults);
                                            if (oResults.result_count > 0)
                                            {
                                                especificadores.Add(oResults.results[0]);
                                            }
                                            else
                                            {
                                                especificadores_nao_encontrados.Add(neoIndexacao);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Total de normas = " + lista_normas_ajuste.Count);
                        Console.WriteLine("Normas processadas = " + count_normas_verificadas);
                        Console.WriteLine("Termos que nao existem no rest = " + termos_nao_existentes.Count);
                        Console.WriteLine("Descritores que possuem correspondente no REST = " + descritores.Count);
                        Console.WriteLine("Especificadores que possuem correspondente no REST = " + especificadores.Count);
                        Console.WriteLine("Descritores que NÃO possuem correspondente no REST = " + descritores_nao_encontrados.Count);
                        Console.WriteLine("Especificadores que NÃO possuem correspondente no REST = " + especificadores_nao_encontrados.Count);
                        Console.WriteLine("Excecoes = " + excecoes.Count);
                    }
                }
                catch (Exception ex)
                {
                    excecoes.Add(ex);
                }
            }

            // Salva log
            try
            {
                var pasta = "C:/temp/21-12-2015/";
                foreach (var descritor in descritores)
                {
                    Files.ArquivoApend(descritor.nm_termo + Environment.NewLine, pasta, "descritores_existentes_no_rest.txt");
                }

                foreach (var especificador in especificadores)
                {
                    Files.ArquivoApend(especificador.nm_termo + Environment.NewLine, pasta, "especificadores_existentes_no_rest.txt");
                }

                foreach (var termo_nao_existente in termos_nao_existentes)
                {
                    Files.ArquivoApend("{\"NmTermo\":\"" + termo_nao_existente.NmTermo + "\", \"NmEspecificador\":\"" + termo_nao_existente.NmEspecificador + "\", \"InTipoTermo\":\"" + termo_nao_existente.InTipoTermo + "\"}" + Environment.NewLine, pasta, "termos_nao_existentes_no_rest.txt");
                }

                foreach (var especificador_nao_encontrado in especificadores_nao_encontrados)
                {
                    Files.ArquivoApend("{\"NmTermo\":\"" + especificador_nao_encontrado.NmTermo + "\", \"NmEspecificador\":\"" + especificador_nao_encontrado.NmEspecificador + "\", \"InTipoTermo\":\"" + especificador_nao_encontrado.InTipoTermo + "\"}" + Environment.NewLine, pasta, "especificadores_nao_existentes_no_rest.txt");
                }

                foreach (var descritor_nao_encontrado in descritores_nao_encontrados)
                {
                    Files.ArquivoApend("{\"NmTermo\":\"" + descritor_nao_encontrado.NmTermo + "\", \"NmEspecificador\":\"" + descritor_nao_encontrado.NmEspecificador + "\", \"InTipoTermo\":\"" + descritor_nao_encontrado.InTipoTermo + "\"}" + Environment.NewLine, pasta, "descritores_nao_existentes_no_rest.txt");
                }

                if (excecoes.Count > 0)
                {
                    foreach (var exception in excecoes)
                    {
                        Files.ArquivoApend("{\"inner_exception\":\""+exception.InnerException+"\"}" + Environment.NewLine, pasta, "excecoes.txt");
                    }
                }


                Files.ArquivoApend("Rotina finalizada em " + DateTime.Now + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Normas processadas = " + count_normas_verificadas + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Termos que nao existem no rest = " + termos_nao_existentes.Count + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Descritores que possuem correspondente no REST = " + descritores.Count + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Especificadores que possuem correspondente no REST = " + especificadores.Count + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Descritores que NÃO possuem correspondente no REST = " + descritores_nao_encontrados.Count + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Especificadores que NÃO possuem correspondente no REST = " + especificadores_nao_encontrados.Count + Environment.NewLine, pasta, "log.txt");
                Files.ArquivoApend("Excecoes = " + excecoes.Count + Environment.NewLine, pasta, "log.txt");
            }
            catch (Exception ex)
            {
                
            }
        }


        //public void AjustarIndexacao()
        //{
        //    //Inicializar variaveis
        //    var normaRn = new NormaRN();
        //    var vocabularioRn = new VocabularioControladoRN();
        //    REST _rest = new REST("sinj_vocabulario");
        //    //REST _rest = new REST("legado_versoesdasnormas");

        //    // Buscar json das normas
        //    List<string> lista_normas_ajuste = new List<string>();
        //    List<string> lista_termos_existentes_rest = new List<string>();
        //    List<string> lista_termos_nao_existentes_rest = new List<string>();
        //    string line;
        //    if (System.IO.File.Exists("C:/temp/legado_normas_inconsistentes.txt"))
        //    {
        //        using (var reader = System.IO.File.OpenText("C:/temp/legado_normas_inconsistentes.txt"))
        //        {
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                lista_normas_ajuste.Add(line);
        //            }
        //        }
        //    }

        //    // Percorrer normas que estao salvas como json no txt
        //    foreach (var json_norma in lista_normas_ajuste)
        //    {
        //        try
        //        {
        //            var obj_legado_norma = JSON.Deserializa<LegadoNormaLBW>(json_norma);
        //            if (obj_legado_norma.Indexacao.Count == 0 && obj_legado_norma.NeoIndexacao.Count == 0)
        //            {
        //                // Comentado porque esse arquivo já foi gravado
        //                //var obj_erro_legado_norma = JSON.Deserializa<ErroLegadoNormaLBW>(json_norma);
        //                //Files.ArquivoApend(obj_erro_legado_norma.id + Environment.NewLine, "C:/temp/", "ids_nao_migrados_legado_normas_inconsistentes.txt");
        //            }
        //            else
        //            {
        //                var normaOv = normaRn.BuscarNorma(obj_legado_norma.Id, null);
        //                foreach (var neoIndexacao in obj_legado_norma.NeoIndexacao)
        //                {
        //                    // Se começar com maiuscula, busca no rest um termo que esta escrito com tudo maiusculo
        //                    if (Char.IsUpper(neoIndexacao.NmTermo[0]))
        //                    {
        //                        string consulta = "$$={\"literal\":\"ch_tipo_termo='DE' and nm_termo='"+ neoIndexacao.NmTermo.ToUpper() + "'\", \"limit\":1}";
        //                        var sResults = _rest.Consultar(consulta);
        //                        var oResults = JSON.Deserializa<Results<VocabularioControladoOV>>(sResults);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }

        //}

        public void GerarJSONLegadoLBW()
        {

            //Inicializar variaveis
            var normaRn = new NormaRN();
            REST _rest = new REST("legado_versoesdasnormas");

            // Buscar ids das normas
            List<string> lista_ids_normas_ajuste = new List<string>();
            var filename = "C:/temp/ids_nao_migrados_legado_normas_inconsistentes.txt";
            string line;

            if (File.Exists(filename))
            {
                using (var reader = File.OpenText(filename))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lista_ids_normas_ajuste.Add(line);
                    }
                }
            }

            // Consultar esses ids no rest 
            foreach (var id_norma in lista_ids_normas_ajuste)
            {
                try
                {
                    string consulta = "$$={\"literal\":\"id='" + id_norma + "'\",\"limit\":\"1\", \"select\":[\"id\", \"indexacao\", \"neoindexacao\", \"id_tipo\"]}";
                    var sResults = _rest.Consultar(consulta);
                    var oResults = JSON.Deserializa<Results<LegadoNormaLBW>>(sResults);
                    var json_legado_norma = JSON.Serialize(oResults.results[0]);
                    Files.ArquivoApend(json_legado_norma + Environment.NewLine, "C:/temp/", "legado_normas_inconsistentes_que_tinha_dado_erro.txt");
                }
                catch (Exception ex)
                {
                    var oErro = "{\"id\":\"" + id_norma + "\", \"mensagem\":\"" + ex.Message + "\", \"inner_exception\":\"" + ex.InnerException.Message + "\"}";
                    Files.ArquivoApend(oErro + Environment.NewLine, "C:/temp/", "legado_normas_inconsistentes_que_tinha_dado_erro.txt");
                }
            }
        }

        //public void AjustarIndexacao()
        //{
        //    //Inicializar variaveis
        //    var normaRn = new NormaRN();
        //    REST _rest = new REST("legado_versoesdasnormas");

        //    // Buscar ids das normas
        //    List<string> lista_ids_normas_ajuste = new List<string>();
        //    string line;
        //    if (System.IO.File.Exists("C:/temp/id_normas_inconsistentes_unico.txt"))
        //    {
        //        using (var reader = System.IO.File.OpenText("C:/temp/id_normas_inconsistentes_unico.txt"))
        //        {
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                lista_ids_normas_ajuste.Add(line);
        //            }
        //        }
        //    }

        //    // Consultar esses ids no rest 
        //    foreach (var id_norma in lista_ids_normas_ajuste)
        //    {
        //        try
        //        {
        //            string consulta = "$$={\"literal\":\"id='" + id_norma + "'\",\"limit\":\"1\", \"select\":[\"id\", \"indexacao\", \"neoindexacao\", \"id_tipo\"]}";
        //            var sResults = _rest.Consultar(consulta);
        //            var oResults = JSON.Deserializa<Results<LegadoNormaLBW>>(sResults);
        //            var json_legado_norma = JSON.Serialize(oResults.results[0]);
        //            Files.ArquivoApend(json_legado_norma + Environment.NewLine, "C:/temp/", "legado_normas_inconsistentes.txt");
        //        }
        //        catch (Exception ex)
        //        {
        //            var oErro = "{\"id\":\"" + id_norma + "\", \"mensagem\":\"" + ex.Message + "\", \"inner_exception\":\"" + ex.InnerException.Message + "\"}";
        //            Files.ArquivoApend(oErro + Environment.NewLine, "C:/temp/", "legado_normas_inconsistentes.txt");
        //        }
        //    }

        //}

        public void TesteNormasLbw()
        {
            count_normas_erradas = 0;
            var vocabularioControladoRn = new VocabularioControladoRN();
            sb_normas_erradas = new StringBuilder();
            //vocabulariosControladosLbw = vocabularioControladoRn.BuscarVocabulariosControladosLBW();
            vocabulariosControladosLbw2 = vocabularioControladoRn.BuscarVocabulariosControladosLBW2();
            VerificarIndexacao();
            Files.CriaArquivo("./inconsistencia_neoindexacao.txt", sb_normas_erradas.ToString());
        }


        public void VerificarIndexacao()
        {
            Console.WriteLine("========== Verificar Indexação ==========");
            var normaRn = new NormaRN();
            var ids_normas = new List<string>();
            ids_normas = normaRn.BuscarIdsNormasLBW();
            var lista_invoke = new List<string>();
            for (var i = 0; i < ids_normas.Count(); i++)
            {
                lista_invoke.Add(ids_normas[i]);
                if (lista_invoke.Count() == 5 || i == (ids_normas.Count() - 1))
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Total: " + ids_normas.Count);
                    Console.WriteLine("Normas verificadas: " + i);
                    Console.WriteLine("Normas erradas: " + count_normas_erradas);
                    VerificarIndexacao(lista_invoke);
                    lista_invoke = new List<string>();
                }
            }
        }


        public void VerificarIndexacao(List<string> ids)
        {
            var normaRn = new NormaRN();
            var normasLbw = normaRn.BuscarNormasIndexacaoLBW(ids);
            foreach (var normaLbw in normasLbw)
            {
                foreach (var indexacao in normaLbw.NeoIndexacao)
                {
                    if (!vocabulariosControladosLbw2.Exists(v => v == indexacao.NmTermo))
                    {
                        count_normas_erradas++;
                        sb_normas_erradas.AppendLine("Id = " + normaLbw.Id + " , Termo = " + indexacao.NmTermo);
                    }
                }
            }
        }



        //public List<string> GetListStringIndexacao()
        //{
        //    List<string> termos = new List<string>();
        //    foreach (var indexacao in NeoIndexacao)
        //    {
        //        if (termos.Count > 0)
        //        {
        //            string ultimo = termos.Last();
        //            string[] ultimoSplit = ultimo.Split(',');
        //            if (ultimoSplit[0].Trim(' ') == indexacao.NmTermo && !string.IsNullOrEmpty(indexacao.NmEspecificador))
        //            {
        //                int index = termos.LastIndexOf(ultimo);
        //                termos[index] = ultimo + ", " + indexacao.NmEspecificador;
        //            }
        //            else
        //            {
        //                termos.Add(indexacao.NmTermo + (!string.IsNullOrEmpty(indexacao.NmEspecificador) ? ", " + indexacao.NmEspecificador : ""));
        //            }
        //        }
        //        else
        //        {
        //            termos.Add(indexacao.NmTermo + (!string.IsNullOrEmpty(indexacao.NmEspecificador) ? ", " + indexacao.NmEspecificador : ""));
        //        }
        //    }
        //    return termos;
        //}






        public void TesteBuscaNormaRest()
        {
            Console.WriteLine("========= TESTE - BUSCAR NORMA NO REST =========");
            var normaRn = new NormaRN();
            var lista_normas = normaRn.BuscarNormasRest(100);
            foreach (var norma in lista_normas)
            {
                Console.WriteLine("ch norma = "+ norma.ch_norma);
            }
            Console.ReadLine();
        }

        public void MigrarUsuarios(string acao = "")
        {
            Console.WriteLine("========== Usuarios ==========");
            var usuarioRn = new UsuarioRN();
            if (acao == "subir_json")
            {
                _usuarios = RecuperarJson<UsuarioOV>("usuario");
            }
            else{
                if (acao == "migrar_do_arquivo")
                {
                    _usuarios = RecuperarJson<UsuarioOV>("usuario");
                    foreach (var usuario in _usuarios)
                    {
                        Console.WriteLine("> Migrando............" + usuario.nm_login_usuario);

                        var id_doc = usuarioRn.Incluir(usuario);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("usuario " + usuario.nm_login_usuario);
                        }
                    }
                }
                else{
                    var usuariosLbw = usuarioRn.BuscarUsuariosLBW();
                    UsuarioOV usuarioOv;
                    _usuarios = new List<UsuarioOV>();

                    foreach (var usuarioLbw in usuariosLbw)
                    {
                        usuarioOv = new UsuarioOV();
                        try
                        {
                            usuarioOv.nm_usuario = usuarioLbw.Name;
                            usuarioOv.nm_login_usuario = usuarioLbw.Login;
                            usuarioOv.pagina_inicial = "Pesquisas.aspx";
                            usuarioOv.ds_pagina_inicial = "Pesquisas";
                            usuarioOv.st_usuario = !usuarioLbw.Disabled;
                            usuarioOv.ch_tema = "verde";
                            usuarioOv.nr_tentativa_login = 0;

                            foreach (var grupoLbw in usuarioLbw.Groups)
                            {
                                var grupos = PermissaoMigracao.GetPermissoes(grupoLbw);
                                foreach (var grupo in grupos)
                                {
                                    if (!usuarioOv.grupos.Contains(grupo))
                                    {
                                        usuarioOv.grupos.Add(grupo);
                                    }
                                }
                            }
                            if (usuarioLbw.Area.Name == "TCDF")
                            {
                                usuarioOv.orgao_cadastrador = new OrgaoCadastradorOV { id_orgao_cadastrador = 1, nm_orgao_cadastrador = "TCDF" };
                            }
                            else if (usuarioLbw.Area.Name == "SEPLAG")
                            {
                                usuarioOv.orgao_cadastrador = new OrgaoCadastradorOV { id_orgao_cadastrador = 2, nm_orgao_cadastrador = "SEPLAG" };
                            }
                            else if (usuarioLbw.Area.Name == "PGDF")
                            {
                                usuarioOv.orgao_cadastrador = new OrgaoCadastradorOV { id_orgao_cadastrador = 3, nm_orgao_cadastrador = "PGDF" };
                            }
                            else if (usuarioLbw.Area.Name == "CLDF")
                            {
                                usuarioOv.orgao_cadastrador = new OrgaoCadastradorOV { id_orgao_cadastrador = 4, nm_orgao_cadastrador = "CLDF" };
                            }
                            usuarioOv.senha_usuario = Criptografia.CalcularHashMD5("sinj2015", true);

                            usuarioOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            usuarioOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if(acao != "salvar_json"){
                                Console.WriteLine("> Migrando............" + usuarioOv.nm_usuario);
                                var id_doc = usuarioRn.Incluir(usuarioOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("usuario " + usuarioLbw.Login.ToString());
                                }
                            }
                            _usuarios.Add(usuarioOv);
                            SalvarJson(usuarioOv, "usuario", usuarioLbw.Login.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, usuarioOv, "usuario");
                        }
                    }
                }
            }
        }

        public void MigrarAutorias(string acao = "")
        {
            Console.WriteLine("========== Autorias ==========");
            var autoriaRn = new AutoriaRN();
            if (acao == "subir_json")
            {
                _autorias = RecuperarJson<AutoriaOV>("autoria");
            }
            else{
                if (acao == "migrar_do_arquivo")
                {
                    _autorias = RecuperarJson<AutoriaOV>("autoria");
                    foreach (var autoria in _autorias)
                    {
                        Console.WriteLine("> Migrando............" + autoria.nm_autoria);
                        var id_doc = autoriaRn.Incluir(autoria);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("autoria " + autoria.ch_autoria);
                        }
                    }
                }
                else
                {
                    var autoriasLbw = autoriaRn.BuscarAutoriasLBW();
                    AutoriaOV autoriaOv;
                    _autorias = new List<AutoriaOV>();
                    foreach (var autoriaLbw in autoriasLbw)
                    {
                        autoriaOv = new AutoriaOV();
                        try
                        {
                            autoriaOv.ch_autoria = autoriaLbw.Id.ToString();
                            autoriaOv.nm_autoria = autoriaLbw.Nome;
                            autoriaOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            autoriaOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if(acao != "salvar_json"){
                                Console.WriteLine("> Migrando............" + autoriaOv.nm_autoria);
                                var id_doc = autoriaRn.Incluir(autoriaOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("autoria " + autoriaLbw.Id.ToString());
                                }
                            }
                            _autorias.Add(autoriaOv);
                            SalvarJson(autoriaOv, "autoria", autoriaLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, autoriaOv, "autoria");
                        }
                    }
                }
            }
        }

        public void MigrarInteressados(string acao = "")
        {
            Console.WriteLine("========== Interessados ==========");
            var interessadoRn = new InteressadoRN();
            if (acao == "subir_json")
            {
                _interessados = RecuperarJson<InteressadoOV>("interessado");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _interessados = RecuperarJson<InteressadoOV>("interessado");
                    foreach (var interessado in _interessados)
                    {
                        Console.WriteLine("> Migrando............" + interessado.nm_interessado);
                        var id_doc = interessadoRn.Incluir(interessado);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("interessado " + interessado.ch_interessado);
                        }
                    }
                }
                else
                {
                    var interessadosLbw = interessadoRn.BuscarInteressadosLBW();
                    InteressadoOV interessadoOv;
                    _interessados = new List<InteressadoOV>();
                    foreach (var interessadoLbw in interessadosLbw)
                    {
                        interessadoOv = new InteressadoOV();
                        try
                        {
                            interessadoOv.ch_interessado = interessadoLbw.Id.ToString();
                            interessadoOv.nm_interessado = interessadoLbw.Nome;
                            interessadoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            interessadoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + interessadoOv.nm_interessado);
                                var id_doc = interessadoRn.Incluir(interessadoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("interessado " + interessadoLbw.Id.ToString());
                                }
                            }
                            _interessados.Add(interessadoOv);
                            SalvarJson(interessadoOv, "interessado", interessadoLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, interessadoOv, "interessado");
                        }
                    }
                }
            }
        }

        public void MigrarRelatores(string acao = "")
        {
            Console.WriteLine("========== Relatores ==========");
            var relatorRn = new RelatorRN();
            if (acao == "subir_json")
            {
                _relatores = RecuperarJson<RelatorOV>("relator");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _relatores = RecuperarJson<RelatorOV>("relator");
                    foreach (var relator in _relatores)
                    {
                        Console.WriteLine("> Migrando............" + relator.nm_relator);
                        var id_doc = relatorRn.Incluir(relator);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("relator " + relator.ch_relator);
                        }
                    }
                }
                else
                {
                    var relatoresLbw = relatorRn.BuscarRelatorsLBW();
                    var relatorOv = new RelatorOV();
                    _relatores = new List<RelatorOV>();
                    foreach (var relatorLbw in relatoresLbw)
                    {
                        relatorOv = new RelatorOV();
                        try
                        {
                            relatorOv.ch_relator = relatorLbw.Id.ToString();
                            relatorOv.nm_relator = relatorLbw.Nome;
                            relatorOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            relatorOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + relatorOv.nm_relator);
                                var id_doc = relatorRn.Incluir(relatorOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("relator " + relatorLbw.Id.ToString());
                                }
                            }
                            _relatores.Add(relatorOv);
                            SalvarJson(relatorOv, "relator", relatorLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, relatorOv, "relator");
                        }
                    }
                }
            }
        }

        public void MigrarRequerentes(string acao = "")
        {
            Console.WriteLine("========== Requerentes ==========");
            var requerenteRn = new RequerenteRN();
            if (acao == "subir_json")
            {
                _requerentes = RecuperarJson<RequerenteOV>("requerente");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _requerentes = RecuperarJson<RequerenteOV>("requerente");
                    foreach (var requerente in _requerentes)
                    {
                        Console.WriteLine("> Migrando............" + requerente.nm_requerente);
                        var id_doc = requerenteRn.Incluir(requerente);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("requerente " + requerente.ch_requerente);
                        }
                    }
                }
                else
                {
                    var requerentesLbw = requerenteRn.BuscarRequerentesLBW();
                    RequerenteOV requerenteOv;
                    _requerentes = new List<RequerenteOV>();
                    foreach (var requerenteLbw in requerentesLbw)
                    {
                        requerenteOv = new RequerenteOV();
                        try
                        {
                            requerenteOv.ch_requerente = requerenteLbw.Id.ToString();
                            requerenteOv.nm_requerente = requerenteLbw.Nome;
                            requerenteOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            requerenteOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if(acao != "salvar_json"){
                                Console.WriteLine("> Migrando............" + requerenteOv.nm_requerente);
                                var id_doc = requerenteRn.Incluir(requerenteOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("requerente " + requerenteLbw.Id.ToString());
                                }
                            }
                            _requerentes.Add(requerenteOv);
                            SalvarJson(requerenteOv, "requerente", requerenteLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, requerenteOv, "requerente");
                        }
                    }
                }
            }
        }

        public void MigrarRequeridos(string acao = "")
        {
            Console.WriteLine("========== Requeridos ==========");
            var requeridoRn = new RequeridoRN();
            if (acao == "subir_json")
            {
                _requeridos = RecuperarJson<RequeridoOV>("requerido");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _requeridos = RecuperarJson<RequeridoOV>("requerido");
                    foreach (var requerido in _requeridos)
                    {
                        Console.WriteLine("> Migrando............" + requerido.nm_requerido);
                        var id_doc = requeridoRn.Incluir(requerido);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("requerido " + requerido.ch_requerido);
                        }
                    }
                }
                else
                {
                    var requeridosLbw = requeridoRn.BuscarRequeridosLBW();
                    RequeridoOV requeridoOv;
                    _requeridos = new List<RequeridoOV>();
                    foreach (var requeridoLbw in requeridosLbw)
                    {
                        requeridoOv = new RequeridoOV();
                        try
                        {
                            requeridoOv.ch_requerido = requeridoLbw.Id.ToString();
                            requeridoOv.nm_requerido = requeridoLbw.Nome;
                            requeridoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            requeridoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + requeridoOv.nm_requerido);
                                var id_doc = requeridoRn.Incluir(requeridoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("requerido " + requeridoLbw.Id.ToString());
                                }
                            }
                            _requeridos.Add(requeridoOv);
                            SalvarJson(requeridoOv, "requerido", requeridoLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, requeridoOv, "requerido");
                        }
                    }
                }
            }
        }

        public void MigrarSituacoes(string acao = "")
        {
            Console.WriteLine("========== Situacoes ==========");
            var situacaoRn = new SituacaoRN();
            if (acao == "subir_json")
            {
                _situacoes = RecuperarJson<SituacaoOV>("situacao");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _situacoes = RecuperarJson<SituacaoOV>("situacao");
                    foreach (var situacao in _situacoes)
                    {
                        Console.WriteLine("> Migrando............" + situacao.nm_situacao);
                        var id_doc = situacaoRn.Incluir(situacao);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("situacao " + situacao.ch_situacao);
                        }
                    }
                }
                else
                {
                    var situacoesLbw = situacaoRn.BuscarSituacoesLBW();
                    SituacaoOV situacaoOv;
                    _situacoes = new List<SituacaoOV>();
                    foreach (var situacaoLbw in situacoesLbw)
                    {
                        situacaoOv = new SituacaoOV();
                        try
                        {
                            situacaoOv.ch_situacao = Util.RemoverCaracteresEspeciais(situacaoLbw.Descricao).ToLower();
                            situacaoOv.nm_situacao = situacaoLbw.Descricao;
                            situacaoOv.nr_peso_situacao = situacaoLbw.Peso;
                            situacaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            situacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + situacaoOv.nm_situacao);
                                var id_doc = situacaoRn.Incluir(situacaoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("situacao " + situacaoLbw.Id.ToString());
                                }
                            }
                            _situacoes.Add(situacaoOv);
                            SalvarJson(situacaoOv, "situacao", situacaoLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, situacaoOv, "situacao");
                        }
                    }
                    situacaoOv = new SituacaoOV();
                    try
                    {
                        situacaoOv.ch_situacao = "aguardandojulgamentolnardeferida";
                        situacaoOv.nm_situacao = "Aguardando Julgamento (Lnar. Deferida)";
                        situacaoOv.nr_peso_situacao = 14;
                        situacaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                        situacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        if (acao != "salvar_json")
                        {
                            Console.WriteLine("> Migrando............" + situacaoOv.nm_situacao);
                            var id_doc = situacaoRn.Incluir(situacaoOv);
                            Console.WriteLine("> Id_doc.............." + id_doc);
                            if (id_doc <= 0)
                            {
                                EscreverLogDocNaoMigrado("situacao aguardandojulgamentolnardeferida");
                            }
                        }
                        _situacoes.Add(situacaoOv);
                        SalvarJson(situacaoOv, "situacao", "aguardandojulgamentolnardeferida");
                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, situacaoOv, "situacao");
                    }
                    try{

                        situacaoOv.ch_situacao = "aguardandojulgamentolnarindeferida";
                        situacaoOv.nm_situacao = "Aguardando Julgamento (Lnar. Indeferida)";
                        situacaoOv.nr_peso_situacao = 14;
                        situacaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                        situacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        if (acao != "salvar_json")
                        {
                            Console.WriteLine("> Migrando............" + situacaoOv.nm_situacao);
                            var id_doc = situacaoRn.Incluir(situacaoOv);
                            Console.WriteLine("> Id_doc.............." + id_doc);
                            if (id_doc <= 0)
                            {
                                EscreverLogDocNaoMigrado("situacao aguardandojulgamentolnarindeferida");
                            }
                        }
                        _situacoes.Add(situacaoOv);
                        SalvarJson(situacaoOv, "situacao", "aguardandojulgamentolnarindeferida");
                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, situacaoOv, "situacao");
                    }
                    try{

                        situacaoOv.ch_situacao = "aguardandojulgamentoajuizado";
                        situacaoOv.nm_situacao = "Aguardando Julgamento (Ajuizado)";
                        situacaoOv.nr_peso_situacao = 14;
                        situacaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                        situacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        if (acao != "salvar_json")
                        {
                            Console.WriteLine("> Migrando............" + situacaoOv.nm_situacao);
                            var id_doc = situacaoRn.Incluir(situacaoOv);
                            Console.WriteLine("> Id_doc.............." + id_doc);
                            if (id_doc <= 0)
                            {
                                EscreverLogDocNaoMigrado("situacao aguardandojulgamentoajuizado");
                            }
                        }
                        _situacoes.Add(situacaoOv);
                        SalvarJson(situacaoOv, "situacao", "aguardandojulgamentoajuizado");

                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, situacaoOv, "situacao");
                    }
                }
            }
        }

        public void MigrarTiposDeNorma(string acao = "")
        {
            Console.WriteLine("========== TiposDeNorma ==========");
            var tipoDeNormaRn = new TipoDeNormaRN();
            if (acao == "subir_json")
            {
                _tipos_de_norma = RecuperarJson<TipoDeNormaOV>("tipo_de_norma");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _tipos_de_norma = RecuperarJson<TipoDeNormaOV>("tipo_de_norma");
                    foreach (var tipo_de_norma in _tipos_de_norma)
                    {
                        Console.WriteLine("> Migrando............" + tipo_de_norma.nm_tipo_norma);
                        var id_doc = tipoDeNormaRn.Incluir(tipo_de_norma);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("tipo_de_norma " + tipo_de_norma.ch_tipo_norma);
                        }
                    }
                }
                else
                {
                    var tiposDeNormaLbw = tipoDeNormaRn.BuscarTiposDeNormaLBW();
                    TipoDeNormaOV tipoDeNormaOv;
                    _tipos_de_norma = new List<TipoDeNormaOV>();
                    foreach (var tipoDeNormaLbw in tiposDeNormaLbw)
                    {
                        tipoDeNormaOv = new TipoDeNormaOV();
                        try
                        {
                            tipoDeNormaOv.ch_tipo_norma = tipoDeNormaLbw.Id.ToString();
                            tipoDeNormaOv.nm_tipo_norma = tipoDeNormaLbw.Nome;
                            tipoDeNormaOv.ds_tipo_norma = tipoDeNormaLbw.Descricao;
                            tipoDeNormaOv.in_apelidavel = false;
                            tipoDeNormaOv.in_conjunta = tipoDeNormaLbw.Conjunta;
                            tipoDeNormaOv.in_numeracao_por_orgao = tipoDeNormaLbw.ControleDeNumeracaoPorOrgao;
                            tipoDeNormaOv.in_questionavel = tipoDeNormaLbw.Questionaveis;
                            tipoDeNormaOv.in_g1 = tipoDeNormaLbw.Grupo1;
                            tipoDeNormaOv.in_g2 = tipoDeNormaLbw.Grupo2;
                            tipoDeNormaOv.in_g3 = tipoDeNormaLbw.Grupo3;
                            tipoDeNormaOv.in_g4 = tipoDeNormaLbw.Grupo4;
                            tipoDeNormaOv.in_g5 = tipoDeNormaLbw.Grupo5;
                            if (tipoDeNormaLbw.TCDF)
                            {
                                tipoDeNormaOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 1, nm_orgao_cadastrador = "TCDF" });
                            }
                            if (tipoDeNormaLbw.SEPLAG)
                            {
                                tipoDeNormaOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 2, nm_orgao_cadastrador = "SEPLAG" });
                            }
                            if (tipoDeNormaLbw.PGDF)
                            {
                                tipoDeNormaOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 3, nm_orgao_cadastrador = "PGDF" });
                            }
                            if (tipoDeNormaLbw.CLDF)
                            {
                                tipoDeNormaOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 4, nm_orgao_cadastrador = "CLDF" });
                            }
                            tipoDeNormaOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            tipoDeNormaOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + tipoDeNormaOv.nm_tipo_norma);
                                var id_doc = tipoDeNormaRn.Incluir(tipoDeNormaOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("tipo_de_norma " + tipoDeNormaLbw.Id.ToString());
                                }
                            }
                            _tipos_de_norma.Add(tipoDeNormaOv);
                            SalvarJson(tipoDeNormaOv, "tipo_de_norma", tipoDeNormaLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, tipoDeNormaOv, "tipo_de_norma");
                        }
                    }
                }
            }
        }

        public void MigrarOrgaos(string acao = "")
        {
            Console.WriteLine("========== Orgaos ==========");
            var orgaoRn = new OrgaoRN();
            if (acao == "subir_json")
            {
                _orgaos = RecuperarJson<OrgaoOV>("orgao");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _orgaos = RecuperarJson<OrgaoOV>("orgao");
                    foreach (var orgao in _orgaos)
                    {
                        Console.WriteLine("> Migrando............" + orgao.nm_orgao);
                        var id_doc = orgaoRn.Incluir(orgao);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("orgao " + orgao.ch_orgao);
                        }
                    }
                }
                else
                {
                    var orgaosLbw = orgaoRn.BuscarOrgaosLBW();
                    OrgaoOV orgaoOv;
                    _orgaos = new List<OrgaoOV>();
                    foreach (var orgaoLbw in orgaosLbw)
                    {
                        orgaoOv = new OrgaoOV();
                        try
                        {
                            orgaoOv.ch_orgao = orgaoLbw.Id_Orgao.ToString();
                            orgaoOv.nm_orgao = orgaoLbw.Nm_Orgao;
                            orgaoOv.sg_orgao = orgaoLbw.Sg_Orgao;
                            if (orgaoLbw.Nm_Ambito.ToLower() == "distritofederal" || orgaoLbw.Nm_Ambito.ToLower() == "distrito federal")
                            {
                                orgaoOv.ambito.id_ambito = 1;
                                orgaoOv.ambito.nm_ambito = "Distrito Federal";
                            }
                            else if (orgaoLbw.Nm_Ambito.ToLower() == "federal")
                            {
                                orgaoOv.ambito.id_ambito = 2;
                                orgaoOv.ambito.nm_ambito = "Federal";
                            }
                            else if (orgaoLbw.Nm_Ambito.ToLower() == "estadual")
                            {
                                orgaoOv.ambito.id_ambito = 3;
                                orgaoOv.ambito.nm_ambito = "Estadual";
                            }
                            else if (orgaoLbw.Nm_Ambito.ToLower() == "municipal")
                            {
                                orgaoOv.ambito.id_ambito = 4;
                                orgaoOv.ambito.nm_ambito = "Municipal";
                            }
                            orgaoOv.ds_nota_de_escopo = orgaoLbw.Ds_NotaEscopoOrgao;
                            orgaoOv.dt_fim_vigencia = orgaoLbw.Dt_FimVigencia;
                            orgaoOv.dt_inicio_vigencia = orgaoLbw.Dt_InicioVigencia;
                            orgaoOv.st_orgao = orgaoLbw.In_Status;
                            if (!string.IsNullOrEmpty(orgaoOv.dt_inicio_vigencia))
                            {
                                orgaoOv.dt_inicio_vigencia = orgaoOv.dt_inicio_vigencia.Split(' ')[0];
                                var dt_inicio = Convert.ToDateTime(orgaoOv.dt_inicio_vigencia);
                                if (!string.IsNullOrEmpty(orgaoOv.dt_fim_vigencia))
                                {
                                    orgaoOv.dt_fim_vigencia = orgaoOv.dt_fim_vigencia.Split(' ')[0];
                                    var dt_fim = Convert.ToDateTime(orgaoOv.dt_fim_vigencia);
                                    if (dt_inicio > dt_fim)
                                    {
                                        orgaoOv.dt_fim_vigencia = null;
                                        orgaoOv.st_orgao = true;
                                    }
                                    else
                                    {
                                        orgaoOv.st_orgao = false;
                                    }
                                }
                                else
                                {
                                    orgaoOv.dt_fim_vigencia = null;
                                    orgaoOv.st_orgao = true;
                                }
                            }
                            else
                            {
                                orgaoOv.dt_inicio_vigencia = null;
                                orgaoOv.dt_fim_vigencia = null;
                                orgaoOv.st_orgao = true;
                            }
                            orgaoOv.st_autoridade = orgaoLbw.In_Autoridade;
                            if (!string.IsNullOrEmpty(orgaoLbw.Ch_Cronologica))
                            {
                                var ch_cronologica_split = orgaoLbw.Ch_Cronologica.Split(',');
                                IEnumerable<OrgaoLBW> orgaos;
                                foreach (var ch_cronologica in ch_cronologica_split)
                                {
                                    var chave_split = ch_cronologica.Split('.');
                                    var ch_cronologia = "";
                                    foreach (var chave in chave_split)
                                    {
                                        orgaos = orgaosLbw.Where<OrgaoLBW>(o => o.Id_Orgao.ToString().PadLeft(6, '0') == chave);
                                        if (orgaos.Count() > 0)
                                        {
                                            ch_cronologia += (ch_cronologia != "" ? "." : "") + orgaos.First<OrgaoLBW>().Id_Orgao.ToString();
                                        }
                                    }
                                    if (ch_cronologia != "")
                                    {
                                        orgaoOv.ch_cronologia.Add(ch_cronologia);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(orgaoLbw.Ch_Hierarquica))
                            {
                                var ch_hierarquica_split = orgaoLbw.Ch_Hierarquica.Split('.');
                                IEnumerable<OrgaoLBW> orgaos;
                                foreach (var ch_hierarquica in ch_hierarquica_split)
                                {
                                    orgaos = orgaosLbw.Where<OrgaoLBW>(o => o.Id_Orgao.ToString().PadLeft(6, '0') == ch_hierarquica);
                                    if (orgaos.Count() > 0)
                                    {
                                        orgaoOv.ch_hierarquia += (!string.IsNullOrEmpty(orgaoOv.ch_hierarquia) ? "." : "") + orgaos.First<OrgaoLBW>().Id_Orgao.ToString();
                                        orgaoOv.sg_hierarquia += (!string.IsNullOrEmpty(orgaoOv.sg_hierarquia) ? ">" : "") + orgaos.First<OrgaoLBW>().Sg_Orgao;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(orgaoLbw.Id_OrgaoAnterior))
                            {
                                var id_orgaos_anterioesr = orgaoLbw.Id_OrgaoAnterior.Split(',');
                                IEnumerable<OrgaoLBW> orgaos;
                                List<string> chaves = new List<string>();
                                foreach (var id_orgao_anterior in id_orgaos_anterioesr)
                                {
                                    orgaos = orgaosLbw.Where<OrgaoLBW>(o => o.Id_Orgao.ToString() == id_orgao_anterior);
                                    if (orgaos.Count() > 0)
                                    {
                                        chaves.Add(orgaos.First<OrgaoLBW>().Id_Orgao.ToString());
                                    }
                                }
                                if (chaves.Count > 0)
                                {
                                    orgaoOv.ch_orgao_anterior = new string[chaves.Count];
                                    chaves.CopyTo(orgaoOv.ch_orgao_anterior);
                                }
                            }
                            if (!string.IsNullOrEmpty(orgaoLbw.Id_OrgaoPai))
                            {
                                var orgaos = orgaosLbw.Where<OrgaoLBW>(o => o.Id_Orgao.ToString() == orgaoLbw.Id_OrgaoPai);
                                if (orgaos.Count() > 0)
                                {
                                    orgaoOv.ch_orgao_pai = orgaos.First<OrgaoLBW>().Id_Orgao.ToString();
                                }
                            }
                            if (orgaoLbw.In_TCDF)
                            {
                                orgaoOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 1, nm_orgao_cadastrador = "TCDF" });
                            }
                            if (orgaoLbw.In_SEPLAG)
                            {
                                orgaoOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 2, nm_orgao_cadastrador = "SEPLAG" });
                            }
                            if (orgaoLbw.In_PGDF)
                            {
                                orgaoOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 3, nm_orgao_cadastrador = "PGDF" });
                            }
                            if (orgaoLbw.In_CLDF)
                            {
                                orgaoOv.orgaos_cadastradores.Add(new OrgaoCadastradorOV { id_orgao_cadastrador = 4, nm_orgao_cadastrador = "CLDF" });
                            }
                            orgaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            orgaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (!string.IsNullOrEmpty(orgaoLbw.Nm_UsuarioCadastro))
                            {
                                var usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == orgaoLbw.Nm_UsuarioCadastro);
                                if (usuarios.Count() > 0)
                                {
                                    if (!string.IsNullOrEmpty(orgaoLbw.Dt_Cadastro))
                                    {
                                        orgaoOv.nm_login_usuario_cadastro = _usuarios.First<UsuarioOV>().nm_login_usuario;
                                        orgaoOv.dt_cadastro = Convert.ToDateTime(orgaoLbw.Dt_Cadastro).ToString("dd'/'MM'/'yyyy HH:mm:ss");
                                        if (!string.IsNullOrEmpty(orgaoLbw.Nm_UsuarioUltimaAlteracao))
                                        {
                                            usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == orgaoLbw.Nm_UsuarioUltimaAlteracao);
                                            if (usuarios.Count() > 0)
                                            {
                                                if (!string.IsNullOrEmpty(orgaoLbw.Dt_UltimaAlteracao))
                                                {
                                                    orgaoOv.alteracoes.Add(new AlteracaoOV { nm_login_usuario_alteracao = _usuarios.First<UsuarioOV>().nm_login_usuario, dt_alteracao = Convert.ToDateTime(orgaoLbw.Dt_UltimaAlteracao).ToString("dd'/'MM'/'yyyy HH:mm:ss") });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            orgaoOv.alteracoes.Add(new AlteracaoOV { nm_login_usuario_alteracao = "MigradorSINJ", dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss") });
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + orgaoOv.nm_orgao);
                                var id_doc = orgaoRn.Incluir(orgaoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("orgao " + orgaoLbw.Id_Orgao.ToString());
                                }
                            }
                            _orgaos.Add(orgaoOv);
                            SalvarJson(orgaoOv, "orgao", orgaoLbw.Id_Orgao.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, orgaoOv, "orgao");
                        }
                    }
                }
            }
        }

        public void MigrarTiposDeFonte(string acao = "")
        {
            Console.WriteLine("========== TiposDeFonte ==========");
            var tipoDeFonteRn = new TipoDeFonteRN();
            if (acao == "subir_json")
            {
                _tipos_de_fonte = RecuperarJson<TipoDeFonteOV>("tipo_de_fonte");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _tipos_de_fonte = RecuperarJson<TipoDeFonteOV>("tipo_de_fonte");
                    foreach (var tipo_de_fonte in _tipos_de_fonte)
                    {
                        Console.WriteLine("> Migrando............" + tipo_de_fonte.nm_tipo_fonte);
                        var id_doc = tipoDeFonteRn.Incluir(tipo_de_fonte);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("tipo_de_fonte " + tipo_de_fonte.ch_tipo_fonte);
                        }
                    }
                }
                else
                {
                    var tiposDeFonteLbw = tipoDeFonteRn.BuscarTiposDeFonteLBW();
                    TipoDeFonteOV tipoDeFonteOv;
                    _tipos_de_fonte = new List<TipoDeFonteOV>();
                    foreach (var tipoDeFonteLbw in tiposDeFonteLbw)
                    {
                        tipoDeFonteOv = new TipoDeFonteOV();
                        try
                        {
                            tipoDeFonteOv.ch_tipo_fonte = tipoDeFonteLbw.Id.ToString();
                            tipoDeFonteOv.nm_tipo_fonte = tipoDeFonteLbw.Nome;
                            tipoDeFonteOv.ds_tipo_fonte = tipoDeFonteLbw.Nome;
                            tipoDeFonteOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            tipoDeFonteOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + tipoDeFonteOv.nm_tipo_fonte);
                                var id_doc = tipoDeFonteRn.Incluir(tipoDeFonteOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("tipo_de_fonte " + tipoDeFonteLbw.Id.ToString());
                                }
                            }
                            _tipos_de_fonte.Add(tipoDeFonteOv);
                            SalvarJson(tipoDeFonteOv, "tipo_de_fonte", tipoDeFonteLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, tipoDeFonteOv, "tipo_de_fonte");
                        }
                    }
                }
            }
        }

        public void MigrarTiposDePublicacao(string acao = "")
        {
            Console.WriteLine("========== TiposDePublicacao ==========");
            var tipoDePublicacaoRn = new TipoDePublicacaoRN();
            if (acao == "subir_json")
            {
                _tipos_de_publicacao = RecuperarJson<TipoDePublicacaoOV>("tipo_de_publicacao");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _tipos_de_publicacao = RecuperarJson<TipoDePublicacaoOV>("tipo_de_publicacao");
                    foreach (var tipo_de_publicacao in _tipos_de_publicacao)
                    {
                        Console.WriteLine("> Migrando............" + tipo_de_publicacao.nm_tipo_publicacao);
                        var id_doc = tipoDePublicacaoRn.Incluir(tipo_de_publicacao);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("tipo_de_publicacao " + tipo_de_publicacao.ch_tipo_publicacao);
                        }
                    }
                }
                else
                {
                    var tiposDePublicacaoLbw = tipoDePublicacaoRn.BuscarTiposDePublicacaoLBW();
                    TipoDePublicacaoOV tipoDePublicacaoOv;
                    _tipos_de_publicacao = new List<TipoDePublicacaoOV>();
                    foreach (var tipoDePublicacaoLbw in tiposDePublicacaoLbw)
                    {
                        tipoDePublicacaoOv = new TipoDePublicacaoOV();
                        try
                        {
                            tipoDePublicacaoOv.ch_tipo_publicacao = tipoDePublicacaoLbw.Id.ToString();
                            tipoDePublicacaoOv.nm_tipo_publicacao = tipoDePublicacaoLbw.Nome;
                            tipoDePublicacaoOv.ds_tipo_publicacao = tipoDePublicacaoLbw.Nome;
                            tipoDePublicacaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            tipoDePublicacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + tipoDePublicacaoOv.nm_tipo_publicacao);
                                var id_doc = tipoDePublicacaoRn.Incluir(tipoDePublicacaoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("tipo_de_publicacao " + tipoDePublicacaoLbw.Id.ToString());
                                }
                            }
                            _tipos_de_publicacao.Add(tipoDePublicacaoOv);
                            SalvarJson(tipoDePublicacaoOv, "tipo_de_publicacao", tipoDePublicacaoLbw.Id.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, tipoDePublicacaoOv, "tipo_de_publicacao");
                        }
                    }
                }
            }
        }

        public void MigrarTiposDeRelacao(string acao = "")
        {
            Console.WriteLine("========== TiposDeRelacao ==========");
            var tipoDeRelacaoRn = new TipoDeRelacaoRN();
            if (acao == "subir_json")
            {
                _tipos_de_relacao = RecuperarJson<TipoDeRelacaoOV>("tipo_de_relacao");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _tipos_de_relacao = RecuperarJson<TipoDeRelacaoOV>("tipo_de_relacao");
                    foreach (var tipo_de_relacao in _tipos_de_relacao)
                    {
                        Console.WriteLine("> Migrando............" + tipo_de_relacao.nm_tipo_relacao);
                        var id_doc = tipoDeRelacaoRn.Incluir(tipo_de_relacao);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("tipo_de_relacao " + tipo_de_relacao.ch_tipo_relacao);
                        }
                    }
                }
                else
                {
                    var tiposDeRelacaoLbw = tipoDeRelacaoRn.BuscarTiposDeRelacaoLBW();
                    TipoDeRelacaoOV tipoDeRelacaoOv;
                    _tipos_de_relacao = new List<TipoDeRelacaoOV>();
                    foreach (var tipoDeRelacaoLbw in tiposDeRelacaoLbw)
                    {
                        tipoDeRelacaoOv = new TipoDeRelacaoOV();
                        try
                        {
                            tipoDeRelacaoOv.ch_tipo_relacao = tipoDeRelacaoLbw.Oid.ToString();
                            tipoDeRelacaoOv.nm_tipo_relacao = tipoDeRelacaoLbw.Conteudo;
                            tipoDeRelacaoOv.ds_tipo_relacao = tipoDeRelacaoLbw.Descricao;
                            tipoDeRelacaoOv.in_relacao_de_acao = tipoDeRelacaoLbw.RelacaoDeAcao;
                            tipoDeRelacaoOv.ds_texto_para_alterado = tipoDeRelacaoLbw.TextoParaAlterado;
                            tipoDeRelacaoOv.ds_texto_para_alterador = tipoDeRelacaoLbw.TextoParaAlterador;
                            tipoDeRelacaoOv.nr_importancia = tipoDeRelacaoLbw.Importancia;
                            tipoDeRelacaoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            tipoDeRelacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + tipoDeRelacaoOv.nm_tipo_relacao);
                                var id_doc = tipoDeRelacaoRn.Incluir(tipoDeRelacaoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("tipo_de_relacao " + tipoDeRelacaoLbw.Oid.ToString());
                                }
                            }
                            _tipos_de_relacao.Add(tipoDeRelacaoOv);

                            SalvarJson(tipoDeRelacaoOv, "tipo_de_relacao", tipoDeRelacaoLbw.Oid.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, tipoDeRelacaoOv, "tipo_de_relacao");
                        }
                    }
                }
            }
        }

        public void MigrarVocabularioControlado(string acao = "")
        {
            Console.WriteLine("========== Vocabulario ==========");
            var vocabularioControladoRn = new VocabularioControladoRN();
            if (acao == "subir_json")
            {
                _indexacoes = RecuperarJson<VocabularioControladoOV>("indexacao");
            }
            else
            {
                if (acao == "migrar_do_arquivo")
                {
                    _indexacoes = RecuperarJson<VocabularioControladoOV>("indexacao");
                    foreach (var indexacao in _indexacoes)
                    {
                        Console.WriteLine("> Migrando............" + indexacao.nm_termo);
                        var id_doc = vocabularioControladoRn.Incluir(indexacao);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("indexacao " + indexacao.ch_termo);
                        }
                    }
                }
                else
                {
                    var vocabulariosControladosLbw = vocabularioControladoRn.BuscarVocabulariosControladosLBW();
                    VocabularioControladoOV vocabularioControladoOv;
                    _indexacoes = new List<VocabularioControladoOV>();
                    foreach (var vocabularioControladoLbw in vocabulariosControladosLbw)
                    {
                        vocabularioControladoOv = new VocabularioControladoOV();
                        try
                        {
                            vocabularioControladoOv.ch_termo = new Guid(vocabularioControladoLbw.Id_Termo).ToString("N");
                            vocabularioControladoOv.nm_termo = vocabularioControladoLbw.Nm_Termo;
                            vocabularioControladoOv.ch_tipo_termo = vocabularioControladoLbw.getTipoTermoMigracao();
                            vocabularioControladoOv.ds_fontes_pesquisadas = vocabularioControladoLbw.FontesPesquisadas;
                            vocabularioControladoOv.ds_nota_explicativa = vocabularioControladoLbw.NotaExplicativa;
                            vocabularioControladoOv.ds_texto_fonte = vocabularioControladoLbw.TextoFonte;
                            vocabularioControladoOv.in_lista = vocabularioControladoLbw.In_Lista;
                            vocabularioControladoOv.in_nao_autorizado = vocabularioControladoLbw.In_TermoNaoAutorizado;
                            vocabularioControladoOv.st_aprovado = vocabularioControladoLbw.In_Aprovado;
                            vocabularioControladoOv.st_ativo = vocabularioControladoLbw.In_Ativo;
                            vocabularioControladoOv.st_excluir = vocabularioControladoLbw.In_Excluir;
                            if (vocabularioControladoLbw.TermosGerais != null && vocabularioControladoLbw.TermosGerais.Count() > 0)
                            {
                                foreach (var termo in vocabularioControladoLbw.TermosGerais)
                                {
                                    vocabularioControladoOv.termos_gerais.Add(new Vocabulario_TG { ch_termo_geral = new Guid(termo.Id_Termo).ToString("N"), nm_termo_geral = termo.Nm_Termo });
                                }
                            }
                            if (vocabularioControladoLbw.TermosEspecificos != null && vocabularioControladoLbw.TermosEspecificos.Count() > 0)
                            {
                                foreach (var termo in vocabularioControladoLbw.TermosEspecificos)
                                {
                                    vocabularioControladoOv.termos_especificos.Add(new Vocabulario_TE { ch_termo_especifico = new Guid(termo.Id_Termo).ToString("N"), nm_termo_especifico = termo.Nm_Termo });
                                }
                            }
                            if (vocabularioControladoLbw.TermosRelacionados != null && vocabularioControladoLbw.TermosRelacionados.Count() > 0)
                            {
                                foreach (var termo in vocabularioControladoLbw.TermosRelacionados)
                                {
                                    vocabularioControladoOv.termos_relacionados.Add(new Vocabulario_TR { ch_termo_relacionado = new Guid(termo.Id_Termo).ToString("N"), nm_termo_relacionado = termo.Nm_Termo });
                                }
                            }
                            if (vocabularioControladoLbw.TermosNaoAutorizados != null && vocabularioControladoLbw.TermosNaoAutorizados.Count() > 0)
                            {
                                foreach (var termo in vocabularioControladoLbw.TermosNaoAutorizados)
                                {
                                    vocabularioControladoOv.termos_nao_autorizados.Add(new Vocabulario_TNA { ch_termo_nao_autorizado = new Guid(termo.Id_Termo).ToString("N"), nm_termo_nao_autorizado = termo.Nm_Termo });
                                }
                            }
                            if (vocabularioControladoLbw.TermoUse != null && !string.IsNullOrEmpty(vocabularioControladoLbw.TermoUse.Id_Termo))
                            {
                                vocabularioControladoOv.ch_termo_use = new Guid(vocabularioControladoLbw.TermoUse.Id_Termo).ToString("N");
                                vocabularioControladoOv.nm_termo_use = vocabularioControladoLbw.TermoUse.Nm_Termo;
                            }
                            if (!string.IsNullOrEmpty(vocabularioControladoLbw.Id_ListaRelacao))
                            {
                                vocabularioControladoOv.ch_lista_superior = new Guid(vocabularioControladoLbw.Id_ListaRelacao).ToString("N");
                                vocabularioControladoOv.nm_lista_superior = vocabularioControladoLbw.Nm_ListaRelacao;
                            }
                            if (vocabularioControladoLbw.Id_Orgao > 0)
                            {
                                var orgaos = _orgaos.Where<OrgaoOV>(o => o.ch_orgao == vocabularioControladoLbw.Id_Orgao.ToString());
                                if (orgaos.Count() > 0)
                                {
                                    vocabularioControladoOv.ch_orgao = orgaos.First<OrgaoOV>().ch_orgao;
                                }
                            }

                            vocabularioControladoOv.nm_login_usuario_cadastro = "MigradorSINJ";
                            vocabularioControladoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            if (acao != "salvar_json")
                            {
                                Console.WriteLine("> Migrando............" + vocabularioControladoOv.nm_termo);
                                var id_doc = vocabularioControladoRn.Incluir(vocabularioControladoOv);
                                Console.WriteLine("> Id_doc.............." + id_doc);
                                if (id_doc <= 0)
                                {
                                    EscreverLogDocNaoMigrado("indexacao " + vocabularioControladoLbw.Id_Termo.ToString());
                                }
                            }
                            _indexacoes.Add(vocabularioControladoOv);
                            SalvarJson(vocabularioControladoOv, "indexacao", vocabularioControladoLbw.Id_Termo.ToString());
                        }
                        catch (Exception ex)
                        {
                            EscreverLogDocErro(ex, vocabularioControladoOv, "indexacao");
                        }
                    }
                }
            }
        }

        public void AtualizarIndexacao()
        {
            Console.WriteLine("========== Indexação ==========");
            var normaRn = new NormaRN();
            var ids_normas = new List<string>();
            var sIds_normas = Config.ValorChave("ids_normas");
            if (sIds_normas != "-1" && !string.IsNullOrEmpty(sIds_normas))
            {
                var ids_normas_split = sIds_normas.Split(',');
                foreach (var id_norma in ids_normas_split)
                {
                    ids_normas.Add(id_norma);
                }
            }
            else
            {
                ids_normas = normaRn.BuscarIdsNormasLBW();
            }
            var lista_invoke = new List<string>();
            for (var i = 0; i < ids_normas.Count(); i++)
            {
                if (lista_controle_action.Count() < 5)
                {
                    lista_invoke.Add(ids_normas[i]);
                    if (lista_invoke.Count() == 5 || i == (ids_normas.Count() - 1))
                    {
                        var chave = Guid.NewGuid().ToString();
                        lista_controle_action.Add(chave);
                        AtualizarIndexacao(lista_invoke, chave);
                        lista_invoke = new List<string>();
                    }
                }
                else
                {
                    i--;
                }
            }
        }

        public void AtualizarIndexacao(List<string> ids, string chave_lista_controle)
        {
            var normaRn = new NormaRN();
            var normasLbw = normaRn.BuscarNormasIndexacaoLBW(ids);
            NormaOV normaOv;
            foreach (var normaLbw in normasLbw)
            {
                normaOv = new NormaOV();
                try
                {
                    normaOv = normaRn.BuscarNorma(normaLbw.Id, new string[]{"id_doc"});
                    var bAtualizar = false;
                    var reindexacoes = new List<AuxReindexacao>();
                    var indexacoes = new List<Indexacao>();
                    if (normaOv != null)
                    {
                        normaOv.indexacoes = new List<Indexacao>();
                        foreach (var neo_indexacao in normaLbw.NeoIndexacao)
                        {
                            var termos = _indexacoes.Where<VocabularioControladoOV>(i => i.nm_termo == neo_indexacao.NmTermo && i.ch_tipo_termo == neo_indexacao.GetSiglaTipoTermo());
                            var especificadores = _indexacoes.Where<VocabularioControladoOV>(i => i.nm_termo == neo_indexacao.NmEspecificador && i.ch_tipo_termo == "ES");
                            if (termos.Count() > 0)
                            {
                                var termo = termos.First<VocabularioControladoOV>();
                                if (!(normaOv.indexacoes.Count<Indexacao>(i => i.vocabulario.Count<Vocabulario>(v => v.nm_termo == termo.nm_termo && v.ch_tipo_termo == termo.ch_tipo_termo) > 0) > 0))
                                {
                                    normaOv.indexacoes.Add(new Indexacao { vocabulario = new List<Vocabulario> { new Vocabulario { ch_termo = termo.ch_termo, ch_tipo_termo = termo.ch_tipo_termo, nm_termo = termo.nm_termo } } });
                                }
                                //else
                                //{
                                if (especificadores.Count() > 0)
                                {
                                    var especificador = especificadores.First<VocabularioControladoOV>();
                                    var indexacao = normaOv.indexacoes.Where<Indexacao>(i => i.vocabulario.Count<Vocabulario>(v => v.nm_termo == termo.nm_termo && v.ch_tipo_termo == termo.ch_tipo_termo) > 0).First<Indexacao>();
                                    indexacao.vocabulario.Add(new Vocabulario { ch_termo = especificador.ch_termo, ch_tipo_termo = especificador.ch_tipo_termo, nm_termo = especificador.nm_termo });

                                }
                                //}
                            }
                        }
                        if (normaOv.indexacoes.Count > 0)
                        {
                            if (normaRn.AtualizarPath(normaOv._metadata.id_doc, "indexacoes", JSON.Serialize(normaOv.indexacoes), null))
                            {
                                Console.WriteLine("Atualizando norma id_doc = " + normaOv._metadata.id_doc);
                            }
                            else
                            {
                                EscreverLogDocNaoMigrado("indexacao da norma " + normaOv._metadata.id_doc);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EscreverLogErroMigracao(ex, ids, "indexacao da norma", chave_lista_controle);
                }
                finally
                {
                    EndAction(chave_lista_controle);
                }
            }
        }

        public void MigrarNormas(string acao = "")
        {
            Console.WriteLine("========== Normas ==========");
            var normaRn = new NormaRN();
            if (acao == "migrar_do_arquivo")
            {
                var normas_neolight = RecuperarJson<NormaOV>("norma");
                var lista_invoke = new List<NormaOV>();
                for (var i = 0; i < normas_neolight.Count(); i++)
                {
                    if (lista_controle_action.Count() < 5)
                    {
                        lista_invoke.Add(normas_neolight[i]);
                        if (lista_invoke.Count() == 50 || i == (normas_neolight.Count() - 1))
                        {
                            var chave = Guid.NewGuid().ToString();
                            lista_controle_action.Add(chave);
                            MigrarNormas(lista_invoke, chave);
                            lista_invoke = new List<NormaOV>();
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            else
            {
                var ids_normas = new List<string>();
                var sIds_normas = Config.ValorChave("ids_normas");
                if (sIds_normas != "-1" && !string.IsNullOrEmpty(sIds_normas))
                {
                    var ids_normas_split = sIds_normas.Split(',');
                    foreach (var id_norma in ids_normas_split)
                    {
                        ids_normas.Add(id_norma);
                    }
                }
                else
                {
                    ids_normas = normaRn.BuscarIdsNormasLBW();
                }
                var lista_invoke = new List<string>();
                for (var i = 0; i < ids_normas.Count(); i++)
                {
                    if (lista_controle_action.Count() < 5)
                    {
                        lista_invoke.Add(ids_normas[i]);
                        if (lista_invoke.Count() == 5 || i == (ids_normas.Count() - 1))
                        {
                            var chave = Guid.NewGuid().ToString();
                            lista_controle_action.Add(chave);
                            MigrarNormas(lista_invoke, acao, chave);
                            lista_invoke = new List<string>();
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        public void MigrarNormas(List<string> ids, string acao, string chave_lista_controle)
        {
            try
            {
                var normaRn = new NormaRN();
                var normasLbw = normaRn.BuscarNormasLBW(ids);
                NormaOV normaOv;
                foreach (var normaLbw in normasLbw)
                {
                    normaOv = new NormaOV();
                    try
                    {
                        normaOv.ch_norma = normaLbw.Id.ToString();
                        foreach (var autoria in normaLbw.Autorias)
                        {
                            var autorias = _autorias.Where<AutoriaOV>(a => a.nm_autoria == autoria);
                            if (autorias.Count() > 0)
                            {
                                normaOv.autorias.Add(new Autoria { ch_autoria = autorias.First<AutoriaOV>().ch_autoria, nm_autoria = autorias.First<AutoriaOV>().nm_autoria });
                            }
                            else
                            {
                                var autoria_ov = new AutoriaOV { ch_autoria = Guid.NewGuid().ToString("N"), nm_autoria = autoria, dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_cadastro = "MigradorSINJ" };
                                try
                                {
                                    Console.WriteLine("> Migrando............" + autoria_ov.nm_autoria);
                                    var id_doc = new AutoriaRN().Incluir(autoria_ov);
                                    Console.WriteLine("> Id_doc.............." + id_doc);
                                    if (id_doc <= 0)
                                    {
                                        EscreverLogDocNaoMigrado("autoria " + autoria_ov.ch_autoria);
                                    }
                                    SalvarJson(autoria_ov, "autoria", autoria_ov.ch_autoria);
                                }
                                catch
                                {

                                }
                                _autorias.Add(autoria_ov);
                                normaOv.autorias.Add(new Autoria { ch_autoria = autoria_ov.ch_autoria, nm_autoria = autoria_ov.nm_autoria });
                            }
                        }
                        var ch_situacao = !string.IsNullOrEmpty(normaLbw.Situacao) ? Util.RemoverCaracteresEspeciais(normaLbw.Situacao).ToLower() : "semrevogacaoexpressa";
                        if (ch_situacao == "aguardandojulgamento1")
                        {
                            ch_situacao = "aguardandojulgamentolnarindeferida";
                        }
                        else if (ch_situacao == "AguardandoJulgamento2")
                        {
                            ch_situacao = "aguardandojulgamentolnardeferida";
                        }
                        else if (ch_situacao == "AguardandoJulgamento3")
                        {
                            ch_situacao = "aguardandojulgamentoajuizado";
                        }
                        var situacoes = _situacoes.Where<SituacaoOV>(s => s.ch_situacao == ch_situacao);
                        if (situacoes.Count() > 0)
                        {
                            normaOv.ch_situacao = situacoes.First<SituacaoOV>().ch_situacao;
                            normaOv.nm_situacao = situacoes.First<SituacaoOV>().nm_situacao;
                        }
                        
                        var tipos_de_norma = _tipos_de_norma.Where<TipoDeNormaOV>(t => t.ch_tipo_norma == normaLbw.Id_Tipo.ToString());
                        if (tipos_de_norma.Count() > 0)
                        {
                            normaOv.ch_tipo_norma = tipos_de_norma.First<TipoDeNormaOV>().ch_tipo_norma;
                            normaOv.nm_tipo_norma = tipos_de_norma.First<TipoDeNormaOV>().nm_tipo_norma;
                            normaOv.st_acao = tipos_de_norma.First<TipoDeNormaOV>().in_g2;
                        }
                        normaOv.cr_norma = normaLbw.Letra;
                        foreach (var decisaoLbw in normaLbw.HistoricoDeDecisoes)
                        {
                            normaOv.decisoes.Add(new Decisao { in_decisao = decisaoLbw.Tipo, ds_complemento = decisaoLbw.Complemento, dt_decisao = decisaoLbw.DataDaPublicacao });
                        }

                        normaOv.ds_efeito_decisao = normaLbw.EfeitoDaDecisao;
                        normaOv.ds_ementa = normaLbw.Ementa;
                        normaOv.ds_observacao = normaLbw.ObservacaoNorma;
                        normaOv.ds_parametro_constitucional = normaLbw.ParametroConstitucional;
                        normaOv.st_pendencia = normaLbw.HaPendencia;
                        normaOv.ds_procedencia = normaLbw.Procedencia;
                        normaOv.dt_assinatura = normaLbw.DataAssinatura;
                        normaOv.dt_autuacao = normaLbw.DataDeAutuacao;
                        foreach (var fonteLbw in normaLbw.Fontes)
                        {
                            var tipos_de_fonte = _tipos_de_fonte.Where<TipoDeFonteOV>(tdf => tdf.nm_tipo_fonte == fonteLbw.TipoFonte);
                            var tipo_de_fonte_ov = new TipoDeFonteOV();
                            if(tipos_de_fonte.Count() > 0){
                                tipo_de_fonte_ov.ch_tipo_fonte = tipos_de_fonte.First<TipoDeFonteOV>().ch_tipo_fonte;
                                tipo_de_fonte_ov.nm_tipo_fonte = tipos_de_fonte.First<TipoDeFonteOV>().nm_tipo_fonte;
                            }
                            else
                            {
                                tipo_de_fonte_ov.ch_tipo_fonte = Guid.NewGuid().ToString("N");
                                tipo_de_fonte_ov.nm_tipo_fonte = fonteLbw.TipoFonte;
                                tipo_de_fonte_ov.ds_tipo_fonte = fonteLbw.TipoFonte;
                                tipo_de_fonte_ov.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                                tipo_de_fonte_ov.nm_login_usuario_cadastro = "MigradorSINJ";
                                Console.WriteLine("> Migrando............" + tipo_de_fonte_ov.nm_tipo_fonte);
                                try
                                {
                                    var id_doc = new TipoDeFonteRN().Incluir(tipo_de_fonte_ov);
                                    Console.WriteLine("> Id_doc.............." + id_doc);
                                    if (id_doc <= 0)
                                    {
                                        EscreverLogDocNaoMigrado("tipo_de_fonte " + tipo_de_fonte_ov.ch_tipo_fonte);
                                    }
                                    SalvarJson(tipo_de_fonte_ov, "tipo_de_fonte", tipo_de_fonte_ov.ch_tipo_fonte);
                                }
                                catch { }
                                _tipos_de_fonte.Add(tipo_de_fonte_ov);
                            }
                            var tipos_de_publicacao = _tipos_de_publicacao.Where<TipoDePublicacaoOV>(tdp => tdp.nm_tipo_publicacao == fonteLbw.TipoPublicacao);
                            var tipo_de_publicacao_ov = new TipoDePublicacaoOV();
                            if (tipos_de_publicacao.Count() > 0)
                            {
                                tipo_de_publicacao_ov.ch_tipo_publicacao = tipos_de_publicacao.First<TipoDePublicacaoOV>().ch_tipo_publicacao;
                                tipo_de_publicacao_ov.nm_tipo_publicacao = tipos_de_publicacao.First<TipoDePublicacaoOV>().nm_tipo_publicacao;
                            }
                            else
                            {
                                tipo_de_publicacao_ov.ch_tipo_publicacao = Guid.NewGuid().ToString("N");
                                tipo_de_publicacao_ov.nm_tipo_publicacao = fonteLbw.TipoPublicacao;
                                tipo_de_publicacao_ov.ds_tipo_publicacao = fonteLbw.TipoPublicacao;
                                tipo_de_publicacao_ov.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                                tipo_de_publicacao_ov.nm_login_usuario_cadastro = "MigradorSINJ";
                                Console.WriteLine("> Migrando............" + tipo_de_publicacao_ov.nm_tipo_publicacao);
                                try
                                {
                                    var id_doc = new TipoDePublicacaoRN().Incluir(tipo_de_publicacao_ov);
                                    Console.WriteLine("> Id_doc.............." + id_doc);
                                    if (id_doc <= 0)
                                    {
                                        EscreverLogDocNaoMigrado("tipo_de_publicacao " + tipo_de_publicacao_ov.ch_tipo_publicacao);
                                    }
                                    SalvarJson(tipo_de_publicacao_ov, "tipo_de_publicacao", tipo_de_publicacao_ov.ch_tipo_publicacao);
                                }
                                catch { }
                                _tipos_de_publicacao.Add(tipo_de_publicacao_ov);
                            }
                            normaOv.fontes.Add(new Fonte
                            {
                                ch_fonte = fonteLbw.Id.ToString("N"),
                                ch_tipo_fonte = tipo_de_fonte_ov.ch_tipo_fonte,
                                nm_tipo_fonte = tipo_de_fonte_ov.nm_tipo_fonte,
                                ch_tipo_publicacao = tipo_de_publicacao_ov.ch_tipo_publicacao,
                                nm_tipo_publicacao = tipo_de_publicacao_ov.nm_tipo_publicacao,
                                ds_observacao_fonte = fonteLbw.Observacao,
                                ds_republicacao = fonteLbw.MotivoReduplicacao,
                                dt_publicacao = fonteLbw.DataPublicacao,
                                in_tipo_edicao = fonteLbw.TipoEdicao,
                                nr_coluna = fonteLbw.Coluna.ToString(),
                                nr_pagina = fonteLbw.Pagina.ToString()
                            });
                        }
                        if (normaLbw.Ambito.ToLower() == "distritofederal" || normaLbw.Ambito.ToLower() == "distrito federal")
                        {
                            normaOv.id_ambito = 1;
                            normaOv.nm_ambito = "Distrito Federal";
                        }
                        else if (normaLbw.Ambito.ToLower() == "federal")
                        {
                            normaOv.id_ambito = 2;
                            normaOv.nm_ambito = "Federal";
                        }
                        else if (normaLbw.Ambito.ToLower() == "estadual")
                        {
                            normaOv.id_ambito = 3;
                            normaOv.nm_ambito = "Estadual";
                        }
                        else if (normaLbw.Ambito.ToLower() == "municipal")
                        {
                            normaOv.id_ambito = 4;
                            normaOv.nm_ambito = "Municipal";
                        }

                        if (normaLbw.orgao_cadastrador.ToUpper() == "TCDF")
                        {
                            normaOv.id_orgao_cadastrador = 1;
                            normaOv.nm_orgao_cadastrador = "TCDF";
                        }
                        if (normaLbw.orgao_cadastrador.ToUpper() == "SEPLAG")
                        {
                            normaOv.id_orgao_cadastrador = 2;
                            normaOv.nm_orgao_cadastrador = "SEPLAG";
                        }
                        if (normaLbw.orgao_cadastrador.ToUpper() == "PGDF")
                        {
                            normaOv.id_orgao_cadastrador = 3;
                            normaOv.nm_orgao_cadastrador = "PGDF";
                        }
                        if (normaLbw.orgao_cadastrador.ToUpper() == "CLDF")
                        {
                            normaOv.id_orgao_cadastrador = 4;
                            normaOv.nm_orgao_cadastrador = "CLDF";
                        }
                        foreach (var neo_indexacao in normaLbw.NeoIndexacao)
                        {
                            var termos = _indexacoes.Where<VocabularioControladoOV>(i => i.nm_termo == neo_indexacao.NmTermo && i.ch_tipo_termo == neo_indexacao.GetSiglaTipoTermo());
                            var especificadores = _indexacoes.Where<VocabularioControladoOV>(i => i.nm_termo == neo_indexacao.NmEspecificador && i.ch_tipo_termo == "ES");
                            if (termos.Count() > 0)
                            {
                                var termo = termos.First<VocabularioControladoOV>();
                                if (!(normaOv.indexacoes.Count<Indexacao>(i => i.vocabulario.Count<Vocabulario>(v => v.nm_termo == termo.nm_termo && v.ch_tipo_termo == termo.ch_tipo_termo) > 0) > 0))
                                {
                                    normaOv.indexacoes.Add(new Indexacao { vocabulario = new List<Vocabulario> { new Vocabulario { ch_termo = termo.ch_termo, ch_tipo_termo = termo.ch_tipo_termo, nm_termo = termo.nm_termo } } });
                                }
                                //else
                                //{
                                    if (especificadores.Count() > 0)
                                    {
                                        var especificador = especificadores.First<VocabularioControladoOV>();
                                        var indexacao = normaOv.indexacoes.Where<Indexacao>(i => i.vocabulario.Count<Vocabulario>(v => v.nm_termo == termo.nm_termo && v.ch_tipo_termo == termo.ch_tipo_termo) > 0).First<Indexacao>();
                                        indexacao.vocabulario.Add(new Vocabulario { ch_termo = especificador.ch_termo, ch_tipo_termo = especificador.ch_tipo_termo, nm_termo = especificador.nm_termo });

                                    }
                                //}
                            }
                        }
                        foreach (var interessadoLbw in normaLbw.Interessados)
                        {
                            var interessados = _interessados.Where<InteressadoOV>(i => i.ch_interessado == interessadoLbw.ToString());
                            if (interessados.Count() > 0)
                            {
                                normaOv.interessados.Add(new Interessado { ch_interessado = interessados.First<InteressadoOV>().ch_interessado, nm_interessado = interessados.First<InteressadoOV>().nm_interessado });
                            }
                        }
                        normaOv.nm_apelido = normaLbw.Apelido;
                        foreach (var pessoa in normaLbw.ListaReferenciasPessoasFisicasEJuridicas)
                        {
                            normaOv.nm_pessoa_fisica_e_juridica.Add(pessoa);
                        }
                        normaOv.nr_norma = !string.IsNullOrEmpty(normaLbw.Numero) ? Convert.ToInt32(normaLbw.Numero.Replace(" ", "").Replace(".", "").Replace(",", "").Replace("-", "")) : 0;
                        normaOv.nr_sequencial = !string.IsNullOrEmpty(normaLbw.NumeroSequencial) ? Convert.ToInt32(normaLbw.NumeroSequencial) : 0;
                        foreach (var origem in normaLbw.Origens)
                        {
                            var origens = _orgaos.Where<OrgaoOV>(o => o.ch_orgao == origem.ToString());
                            if (origens.Count() > 0)
                            {
                                normaOv.origens.Add(new Orgao { ch_orgao = origens.First<OrgaoOV>().ch_orgao, nm_orgao = origens.First<OrgaoOV>().nm_orgao, sg_orgao = origens.First<OrgaoOV>().sg_orgao });
                            }
                        }
                        normaOv.rankeamentos.Add(normaOv.nm_tipo_norma);
                        normaOv.rankeamentos.Add(normaOv.nr_norma.ToString());
                        normaOv.rankeamentos.Add(normaOv.dt_assinatura);
                        foreach (var origem in normaOv.origens)
                        {
                            normaOv.rankeamentos.Add(origem.sg_orgao);
                            normaOv.rankeamentos.Add(origem.nm_orgao);
                        }
                        foreach (var rankeamento in normaLbw.AuxiliarDeRankeamento)
                        {
                            normaOv.rankeamentos.Add(rankeamento);
                        }
                        foreach (var relatorLbw in normaLbw.Relatores)
                        {
                            var relatores = _relatores.Where<RelatorOV>(r => r.nm_relator == relatorLbw);
                            if (relatores.Count() > 0)
                            {
                                normaOv.relatores.Add(new Relator { ch_relator = relatores.First<RelatorOV>().ch_relator, nm_relator = relatores.First<RelatorOV>().nm_relator });
                            }
                        }
                        foreach (var requerenteLbw in normaLbw.Requerentes)
                        {
                            var requerentes = _requerentes.Where<RequerenteOV>(r => r.ch_requerente == requerenteLbw.ToString());
                            if (requerentes.Count() > 0)
                            {
                                normaOv.requerentes.Add(new Requerente { ch_requerente = requerentes.First<RequerenteOV>().ch_requerente, nm_requerente = requerentes.First<RequerenteOV>().nm_requerente });
                            }
                        }
                        foreach (var requeridoLbw in normaLbw.Requeridos)
                        {
                            var requeridos = _requeridos.Where<RequeridoOV>(r => r.ch_requerido == requeridoLbw.ToString());
                            if (requeridos.Count() > 0)
                            {
                                normaOv.requeridos.Add(new Requerido { ch_requerido = requeridos.First<RequeridoOV>().ch_requerido, nm_requerido = requeridos.First<RequeridoOV>().nm_requerido });
                            }
                        }
                        normaOv.st_atualizada = false;
                        normaOv.st_destaque = normaLbw.Destacada;
                        normaOv.st_nova = false;
                        normaOv.st_pendencia = normaLbw.HaPendencia;
                        normaOv.url_referencia = normaLbw.UrlReferenciaExterna;
                        foreach (var videLbw in normaLbw.Vides)
                        {
                            var tipos_de_fonte_vide = _tipos_de_fonte.Where<TipoDeFonteOV>(t => t.nm_tipo_fonte == videLbw.TipoDeFonte);
                            var tipos_de_norma_vide = _tipos_de_norma.Where<TipoDeNormaOV>(t => t.ch_tipo_norma == videLbw.TipoDeNorma.ToString());
                            var tipos_de_relacao_vide = _tipos_de_relacao.Where<TipoDeRelacaoOV>(t => t.ch_tipo_relacao == videLbw.TipoDeVinculo.ToString());
                            var vide = new Vide();
                            if (videLbw.VideAlterador)
                            {
                                vide.item_norma_vide = videLbw.ItemDaNormaPosterior;
                                vide.alinea_norma_vide = videLbw.AlineaDaNormaPosterior;
                                vide.anexo_norma_vide = videLbw.AnexoDaNormaPosterior;
                                vide.artigo_norma_vide = videLbw.ArtigoDaNormaPosterior;
                                vide.inciso_norma_vide = videLbw.IncisoDaNormaPosterior;
                                vide.caput_norma_vide = videLbw.CaputDaNormaPosterior;
                                vide.paragrafo_norma_vide = videLbw.ParagrafoDaNormaPosterior;

                                vide.item_norma_vide_outra = videLbw.ItemDaNormaAnterior;
                                vide.alinea_norma_vide_outra = videLbw.AlineaDaNormaAnterior;
                                vide.anexo_norma_vide_outra = videLbw.AnexoDaNormaAnterior;
                                vide.artigo_norma_vide_outra = videLbw.ArtigoDaNormaAnterior;
                                vide.inciso_norma_vide_outra = videLbw.IncisoDaNormaAnterior;
                                vide.caput_norma_vide_outra = videLbw.CaputDaNormaAnterior;
                                vide.paragrafo_norma_vide_outra = videLbw.ParagrafoDaNormaAnterior;
                            }
                            else
                            {
                                vide.item_norma_vide = videLbw.ItemDaNormaAnterior;
                                vide.alinea_norma_vide = videLbw.AlineaDaNormaAnterior;
                                vide.anexo_norma_vide = videLbw.AnexoDaNormaAnterior;
                                vide.artigo_norma_vide = videLbw.ArtigoDaNormaAnterior;
                                vide.inciso_norma_vide = videLbw.IncisoDaNormaAnterior;
                                vide.caput_norma_vide = videLbw.CaputDaNormaAnterior;
                                vide.paragrafo_norma_vide = videLbw.ParagrafoDaNormaAnterior;

                                vide.item_norma_vide_outra = videLbw.ItemDaNormaPosterior;
                                vide.alinea_norma_vide_outra = videLbw.AlineaDaNormaPosterior;
                                vide.anexo_norma_vide_outra = videLbw.AnexoDaNormaPosterior;
                                vide.artigo_norma_vide_outra = videLbw.ArtigoDaNormaPosterior;
                                vide.inciso_norma_vide_outra = videLbw.IncisoDaNormaPosterior;
                                vide.caput_norma_vide_outra = videLbw.CaputDaNormaPosterior;
                                vide.paragrafo_norma_vide_outra = videLbw.ParagrafoDaNormaPosterior;
                            }
                            if (tipos_de_norma_vide.Count() > 0)
                            {
                                vide.ch_tipo_norma_vide = tipos_de_norma_vide.First<TipoDeNormaOV>().ch_tipo_norma;
                                vide.nm_tipo_norma_vide = tipos_de_norma_vide.First<TipoDeNormaOV>().nm_tipo_norma;
                            }
                            if (tipos_de_relacao_vide.Count() > 0)
                            {
                                vide.ch_tipo_relacao = tipos_de_relacao_vide.First<TipoDeRelacaoOV>().ch_tipo_relacao;
                                vide.nm_tipo_relacao = tipos_de_relacao_vide.First<TipoDeRelacaoOV>().nm_tipo_relacao;
                                vide.ds_texto_relacao = videLbw.VideAlterador ? tipos_de_relacao_vide.First<TipoDeRelacaoOV>().ds_texto_para_alterado : tipos_de_relacao_vide.First<TipoDeRelacaoOV>().ds_texto_para_alterador;
                            }
                            if (tipos_de_fonte_vide.Count() > 0)
                            {
                                vide.ch_tipo_fonte_norma_vide = tipos_de_fonte_vide.First<TipoDeFonteOV>().ch_tipo_fonte;
                                vide.nm_tipo_fonte_norma_vide = tipos_de_fonte_vide.First<TipoDeFonteOV>().nm_tipo_fonte;
                            }
                            vide.ch_vide = videLbw.Id.ToString("N");
                            vide.ds_comentario_vide = videLbw.ComentarioVide;
                            vide.dt_assinatura_norma_vide = videLbw.DataDaNormaPosterior;
                            vide.dt_publicacao_fonte_norma_vide = videLbw.DataDePublicacaoPosterior;
                            vide.ch_norma_vide = videLbw.IdDaNormaPosterior.ToString();
                            vide.in_norma_afetada = !videLbw.VideAlterador;
                            vide.nr_norma_vide = videLbw.NumeroDaNormaPosterior.ToString();

                            normaOv.vides.Add(vide);
                        }

                        normaOv.nm_login_usuario_cadastro = "MigradorSINJ";
                        normaOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        if (!string.IsNullOrEmpty(normaLbw.UsuarioQueCadastrou))
                        {
                            var usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == normaLbw.UsuarioQueCadastrou);
                            if (usuarios.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(normaLbw.DataDoCadastro))
                                {
                                    normaOv.nm_login_usuario_cadastro = usuarios.First<UsuarioOV>().nm_login_usuario;
                                    normaOv.dt_cadastro = Convert.ToDateTime(normaLbw.DataDoCadastro).ToString("dd'/'MM'/'yyyy HH:mm:ss");
                                    if (!string.IsNullOrEmpty(normaLbw.UsuarioDaUltimaAlteracao))
                                    {
                                        usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == normaLbw.UsuarioDaUltimaAlteracao);
                                        if (usuarios.Count() > 0)
                                        {
                                            if (!string.IsNullOrEmpty(normaLbw.DataDaUltimaAlteracao))
                                            {
                                                normaOv.alteracoes.Add(new AlteracaoOV { nm_login_usuario_alteracao = _usuarios.First<UsuarioOV>().nm_login_usuario, dt_alteracao = Convert.ToDateTime(normaLbw.DataDaUltimaAlteracao).ToString("dd'/'MM'/'yyyy HH:mm:ss") });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        normaOv.alteracoes.Add(new AlteracaoOV { nm_login_usuario_alteracao = "MigradorSINJ", dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss") });
                        GerarChaveParaNaoDuplicacao(normaOv);
                        if (acao != "salvar_json")
                        {
                            Console.WriteLine("> Migrando............" + normaOv.nm_tipo_norma + " " + normaOv.nr_norma);
                            var id_doc = normaRn.Incluir(normaOv);
                            Console.WriteLine("> Id_doc.............." + id_doc);
                            if (id_doc <= 0)
                            {
                                EscreverLogDocNaoMigrado("norma " + normaLbw.Id.ToString());
                            }
                        }
                        SalvarJson(normaOv, "norma", normaLbw.Id.ToString());
                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, normaOv, "norma", chave_lista_controle);
                    }
                }
            }
            catch (Exception ex)
            {
                EscreverLogErroMigracao(ex, ids, "norma", chave_lista_controle);
            }
            finally
            {
                EndAction(chave_lista_controle);
            }
        }

        public void MigrarNormas(List<NormaOV> normas, string chave_lista_controle)
        {
            try
            {
                var normaRn = new NormaRN();
                foreach (var norma in normas)
                {
                    try{
                        Console.WriteLine("> Migrando............" + norma.nm_tipo_norma + " " + norma.nr_norma);
                        var id_doc = normaRn.Incluir(norma);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("norma " + norma.ch_norma, chave_lista_controle);
                        }
                        SalvarJson(norma, "norma", norma.ch_norma);
                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, norma, "norma", chave_lista_controle);
                    }
                }
            }
            catch(Exception ex)
            {
                EscreverLogErroMigracao(ex, normas, "norma", chave_lista_controle);
            }
            finally
            {
                EndAction(chave_lista_controle);
            }
        }

        public void GerarChaveParaNaoDuplicacao(NormaOV normaOv)
        {
            StringBuilder chave = new StringBuilder();
            var tipodenorma = _tipos_de_norma.Where<TipoDeNormaOV>(t => t.ch_tipo_norma == normaOv.ch_tipo_norma);
            if (tipodenorma.Count() == 1)
            {
                if (!tipodenorma.First<TipoDeNormaOV>().in_numeracao_por_orgao)
                {
                    chave.Append(normaOv.ch_tipo_norma + "#");
                    chave.Append(normaOv.nr_norma + "#");
                    chave.Append(normaOv.cr_norma + "#");
                    chave.Append(normaOv.nr_sequencial);
                    normaOv.ch_para_nao_duplicacao.Add(chave.ToString());
                }
                else
                {
                    chave.Append(normaOv.ch_tipo_norma + "#");
                    chave.Append(normaOv.nr_norma + "#");
                    var dt_assinatura_split = normaOv.dt_assinatura.Split('/');
                    var ano = "0000";
                    if (dt_assinatura_split.Length == 3)
                    {
                        ano = dt_assinatura_split[2];
                    }
                    chave.Append(ano + "#");
                    chave.Append(normaOv.cr_norma + "#");
                    chave.Append(normaOv.nr_sequencial + "#");
                    foreach (var origem in normaOv.origens)
                    {
                        normaOv.ch_para_nao_duplicacao.Add(origem.ch_orgao + "|" + chave.ToString());
                    }
                }
            }
        }

        public void MigrarDiarios(string acao = "")
        {
            Console.WriteLine("========== Diario ==========");
            var diarioRn = new DiarioRN();
            if (acao == "migrar_do_arquivo")
            {
                var diarios_neolight = RecuperarJson<DiarioOV>("diario");
                var lista_invoke = new List<DiarioOV>();
                for (var i = 0; i < diarios_neolight.Count(); i++)
                {
                    if (lista_controle_action.Count() < 5)
                    {
                        lista_invoke.Add(diarios_neolight[i]);
                        if (lista_invoke.Count() == 50 || i == (diarios_neolight.Count() - 1))
                        {
                            var chave = Guid.NewGuid().ToString();
                            lista_controle_action.Add(chave);
                            MigrarDiarios(lista_invoke, chave);
                            lista_invoke = new List<DiarioOV>();
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            else
            {
                var ids_diarios = new List<string>();
                var sIds_diarios = Config.ValorChave("ids_diarios");
                if (sIds_diarios != "-1" && !string.IsNullOrEmpty(sIds_diarios))
                {
                    var ids_diarios_split = sIds_diarios.Split(',');
                    foreach (var id_diario in ids_diarios_split)
                    {
                        ids_diarios.Add(id_diario);
                    }
                }
                else
                {
                    ids_diarios = diarioRn.BuscarIdsDiariosLBW();
                }
                var lista_invoke = new List<string>();
                for (var i = 0; i < ids_diarios.Count(); i++)
                {
                    if (lista_controle_action.Count() < 5)
                    {
                        lista_invoke.Add(ids_diarios[i]);
                        if (lista_invoke.Count() == 10 || i == (ids_diarios.Count() - 1))
                        {
                            var chave = Guid.NewGuid().ToString();
                            lista_controle_action.Add(chave);
                            MigrarDiarios(lista_invoke, acao, chave);
                            lista_invoke = new List<string>();
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        public void MigrarDiarios(List<string> ids, string acao, string chave_lista_controle)
        {
            try
            {
                var diarioRn = new DiarioRN();
                var diariosLbw = diarioRn.BuscarDiariosLBW(ids);
                DiarioOV diarioOv;
                var tipos_de_fonte = _tipos_de_fonte.Where<TipoDeFonteOV>(tdf => tdf.nm_tipo_fonte == "DODF");
                var ch_tipo_fonte = tipos_de_fonte.First<TipoDeFonteOV>().ch_tipo_fonte;
                var nm_tipo_fonte = tipos_de_fonte.First<TipoDeFonteOV>().nm_tipo_fonte;
                foreach (var diarioLbw in diariosLbw)
                {
                    diarioOv = new DiarioOV();
                    try
                    {
                        diarioOv.ch_diario = diarioLbw.Id.ToString();
                        diarioOv.ch_tipo_fonte = ch_tipo_fonte;
                        diarioOv.nm_tipo_fonte = nm_tipo_fonte;

                        diarioOv.dt_assinatura = diarioLbw.DataDaAssinatura;
                        diarioOv.secao_diario = diarioLbw.Sessao;

                        var sNumero = diarioLbw.Numero;
                        var iNumero = 0;
                        if (int.TryParse(sNumero, out iNumero))
                        {
                            diarioOv.nr_diario = iNumero;
                        }
                        else
                        {
                            sNumero = !string.IsNullOrEmpty(sNumero) ? Util.RemoverCaracteresEspeciais(sNumero) : "0";
                            var sNumeroAux = "";
                            var sLetraAux = "";
                            for (var i = 0; i < sNumero.Length; i++)
                            {
                                if (int.TryParse(sNumero[i].ToString(), out iNumero))
                                {
                                    sNumeroAux += sNumero[i];
                                }
                                else
                                {
                                    sLetraAux += sNumero[i];
                                }
                            }
                            diarioOv.nr_diario = int.Parse(sNumeroAux);
                            diarioOv.cr_diario = sLetraAux;
                        }
                        if (!string.IsNullOrEmpty(diarioLbw.SituacaoQuantoAPendencia))
                        {
                            diarioOv.st_pendente = true;
                            diarioOv.ds_pendencia = diarioLbw.SituacaoQuantoAPendencia;
                        }

                        diarioOv.nm_login_usuario_cadastro = "MigradorSINJ";
                        diarioOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        if (!string.IsNullOrEmpty(diarioLbw.UsuarioQueCadastrou))
                        {
                            var usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == diarioLbw.UsuarioQueCadastrou);
                            if (usuarios.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(diarioLbw.DataDoCadastro))
                                {
                                    diarioOv.nm_login_usuario_cadastro = usuarios.First<UsuarioOV>().nm_login_usuario;
                                    diarioOv.dt_cadastro = Convert.ToDateTime(diarioLbw.DataDoCadastro).ToString("dd'/'MM'/'yyyy HH:mm:ss");
                                    if (!string.IsNullOrEmpty(diarioLbw.UsuarioDaUltimaAlteracao))
                                    {
                                        usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == diarioLbw.UsuarioDaUltimaAlteracao);
                                        if (usuarios.Count() > 0)
                                        {
                                            if (!string.IsNullOrEmpty(diarioLbw.DataDaUltimaAlteracao))
                                            {
                                                diarioOv.alteracoes.Add(new AlteracaoOV { nm_login_usuario_alteracao = usuarios.First<UsuarioOV>().nm_login_usuario, dt_alteracao = Convert.ToDateTime(diarioLbw.DataDaUltimaAlteracao).ToString("dd'/'MM'/'yyyy HH:mm:ss") });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        diarioOv.alteracoes.Add(new AlteracaoOV { nm_login_usuario_alteracao = "MigradorSINJ", dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss") });
                        GerarChaveParaNaoDuplicacao(diarioOv);
                        if (acao != "salvar_json")
                        {
                            Console.WriteLine("> Migrando............" + diarioOv.ch_diario);
                            var id_doc = diarioRn.Incluir(diarioOv);
                            Console.WriteLine("> Id_doc.............." + id_doc);
                            if (id_doc <= 0)
                            {
                                EscreverLogDocNaoMigrado("diario " + diarioLbw.Id.ToString(), chave_lista_controle);
                            }
                        }
                        SalvarJson(diarioOv, "diario", diarioLbw.Id.ToString());
                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, diarioOv, "diario", chave_lista_controle);
                    }
                }
            }
            catch (Exception ex)
            {
                EscreverLogErroMigracao(ex, ids, "diario", chave_lista_controle);
            }
            finally
            {
                EndAction(chave_lista_controle);
            }
        }

        public void MigrarDiarios(List<DiarioOV> diarios, string chave_lista_controle)
        {
            try{
                var diarioRn = new DiarioRN();
                foreach (var diario in diarios)
                {
                    try{
                        Console.WriteLine("> Migrando............" + diario.nm_tipo_fonte);
                        var id_doc = diarioRn.Incluir(diario);
                        Console.WriteLine("> Id_doc.............." + id_doc);
                        if (id_doc <= 0)
                        {
                            EscreverLogDocNaoMigrado("diario " + diario.ch_diario, chave_lista_controle);
                        }
                        SalvarJson(diario, "diario", diario.ch_diario);
                    }
                    catch (Exception ex)
                    {
                        EscreverLogDocErro(ex, diario, "diario", chave_lista_controle);
                    }
                }
            }
            catch (Exception ex)
            {
                EscreverLogErroMigracao(ex, diarios, "diario", chave_lista_controle);
            }
            finally
            {
                EndAction(chave_lista_controle);
            }
        }

        public void GerarChaveParaNaoDuplicacao(DiarioOV diarioOv)
        {
            StringBuilder chave = new StringBuilder();
            chave.Append(diarioOv.ch_tipo_fonte + "#");
            chave.Append(diarioOv.dt_assinatura.Split('/')[2] + "#");
            chave.Append(diarioOv.nr_diario.ToString().PadLeft(8, '0') + "#");
            if (!string.IsNullOrEmpty(diarioOv.cr_diario))
            {
                chave.Append(diarioOv.cr_diario + "#");
            }
            chave.Append(diarioOv.secao_diario);
            diarioOv.ch_para_nao_duplicacao = chave.ToString();
        }

        public void MigrarArquivosNormas()
        {
            Console.WriteLine("========== Norma ==========");
            var normas_neolight = RecuperarJson<NormaOV>("norma");
            var normaRn = new NormaRN();
            var normas_lbw = normaRn.BuscarCaminhosArquivosNormasLBW();
            var caminho = Config.ValorChave("diretorio_arquivo", true);
            var name_file = "";
            var content_type = "";
            var bTem_arquivo = false;
            var norma = new NormaOV();
            IEnumerable<NormaOV> normas;
            foreach (var norma_lbw in normas_lbw)
            {
                try
                {
                    normas = normas_neolight.Where<NormaOV>(n => n.ch_norma == norma_lbw.Id.ToString());
                    if(normas.Count() <= 0) continue;
                    norma = normas.First<NormaOV>();

                    if (!string.IsNullOrEmpty(norma_lbw.CaminhoArquivoTextoAcao))
                    {
                        try
                        {
                            name_file = norma_lbw.CaminhoArquivoTextoAcao.Split('\\').Last<string>();
                            content_type = MimeType.Get(name_file);
                            norma.ar_acao = normaRn.EnviarArquivo(norma._metadata.id_doc, caminho + norma_lbw.CaminhoArquivoTextoAcao, name_file, content_type);
                            bTem_arquivo = true;
                        }
                        catch (Exception ex)
                        {
                            var arquivoErroMigracao = new ArquivoErroMigracaoOV { id_doc_arquivo = norma._metadata.id_doc, nm_base = "sinj_norma", path_file = norma_lbw.CaminhoArquivoTextoAcao, path_put = "ar_acao", ds_erro = Excecao.LerTodasMensagensDaExcecao(ex, true) };
                            try
                            {
                                new ArquivoErroMigracaoRN().Incluir(arquivoErroMigracao);
                            }
                            catch
                            {

                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(norma_lbw.CaminhoArquivoTextoConsolidado))
                    {
                        try{
                            name_file = norma_lbw.CaminhoArquivoTextoConsolidado.Split('\\').Last<string>();
                            content_type = MimeType.Get(name_file);
                            norma.ar_atualizado = normaRn.EnviarArquivo(norma._metadata.id_doc, caminho + norma_lbw.CaminhoArquivoTextoConsolidado, name_file, content_type);
                            bTem_arquivo = true;
                        }
                        catch (Exception ex)
                        {
                            var arquivoErroMigracao = new ArquivoErroMigracaoOV { id_doc_arquivo = norma._metadata.id_doc, nm_base = "sinj_norma", path_file = norma_lbw.CaminhoArquivoTextoConsolidado, path_put = "ar_atualizado", ds_erro = Excecao.LerTodasMensagensDaExcecao(ex, true) };
                            try
                            {
                                new ArquivoErroMigracaoRN().Incluir(arquivoErroMigracao);
                            }
                            catch
                            {

                            }
                        }
                    }
                    for (var i = 0; i < norma.fontes.Count(); i++)
                    {
                        var fontes = norma_lbw.Fontes.Where<FonteLBW>(f => f.Id.ToString("N") == norma.fontes[i].ch_fonte);
                        if (!string.IsNullOrEmpty(norma_lbw.Fontes[i].CaminhoArquivoTexto) && fontes.Count<FonteLBW>() == 1)
                        {
                            try{
                                name_file = fontes.First<FonteLBW>().CaminhoArquivoTexto.Split('\\').Last<string>();
                                content_type = MimeType.Get(name_file);
                                norma.fontes[i].ar_fonte = normaRn.EnviarArquivo(norma._metadata.id_doc, caminho + fontes.First<FonteLBW>().CaminhoArquivoTexto, name_file, content_type);
                                bTem_arquivo = true;
                            }
                            catch (Exception ex)
                            {
                                var arquivoErroMigracao = new ArquivoErroMigracaoOV { id_doc_arquivo = norma._metadata.id_doc, nm_base = "sinj_norma", path_file = fontes.First<FonteLBW>().CaminhoArquivoTexto, path_put = "fontes/" + i + "/ar_fonte", ds_erro = Excecao.LerTodasMensagensDaExcecao(ex, true) };
                                try
                                {
                                    new ArquivoErroMigracaoRN().Incluir(arquivoErroMigracao);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    if (bTem_arquivo)
                    {
                        var updated = normaRn.Atualizar(norma._metadata.id_doc, norma);
                        Console.WriteLine("> Id_doc.............." + norma._metadata.id_doc);
                        if (!updated)
                        {
                            EscreverLogDocNaoMigrado("norma_file" + norma_lbw.Id.ToString());
                        }
                    }
                    SalvarJson(norma, "norma_file", norma_lbw.Id.ToString());
                }
                catch (Exception ex)
                {
                    EscreverLogDocErro(ex, norma, "norma_file");
                }
                bTem_arquivo = false;
            }
        }

        public void MigrarArquivosDiarios()
        {

            Console.WriteLine("========== Diario ==========");
            var diarios_neolight = RecuperarJson<DiarioOV>("diario");
            var diarioRn = new DiarioRN();
            var diarios_lbw = diarioRn.BuscarCaminhosArquivosDiariosLBW();
            var caminho = Config.ValorChave("diretorio_arquivo", true);
            var name_file = "";
            var content_type = "";
            var bTem_arquivo = false;
            var diario = new DiarioOV();
            IEnumerable<DiarioOV> diarios;
            foreach (var diario_lbw in diarios_lbw)
            {
                try
                {
                    diarios = diarios_neolight.Where<DiarioOV>(d => d.ch_diario == diario_lbw.Id.ToString());
                    if (diarios.Count() <= 0) continue;
                    diario = diarios.First<DiarioOV>();

                    if (!string.IsNullOrEmpty(diario_lbw.CaminhoArquivoTexto))
                    {
                        try{
                            name_file = diario_lbw.CaminhoArquivoTexto.Split('\\').Last<string>();
                            content_type = MimeType.Get(name_file);
                            diario.ar_diario = diarioRn.EnviarArquivo(diario._metadata.id_doc, caminho + diario_lbw.CaminhoArquivoTexto, name_file, content_type);
                        
                        }
                        catch (Exception ex)
                        {
                            var arquivoErroMigracao = new ArquivoErroMigracaoOV { id_doc_arquivo = diario._metadata.id_doc, nm_base = "sinj_diario", path_file = diario_lbw.CaminhoArquivoTexto, path_put = "ar_diario", ds_erro = Excecao.LerTodasMensagensDaExcecao(ex, true) };
                            try
                            {
                                new ArquivoErroMigracaoRN().Incluir(arquivoErroMigracao);
                            }
                            catch
                            {

                            }
                        }
                        var updated = diarioRn.Atualizar(diario._metadata.id_doc, diario);

                        Console.WriteLine("> Id_doc.............." + diario._metadata.id_doc);
                        if (!updated)
                        {
                            EscreverLogDocNaoMigrado("diario_file" + diario_lbw.Id.ToString());
                        }
                    }
                    SalvarJson(diario, "diario_file", diario_lbw.Id.ToString());
                }
                catch (Exception ex)
                {
                    EscreverLogDocErro(ex, diario, "diario_file");
                }
            }
        }

        //private void MigrarUsuariosPush()
        //{
        //    Console.WriteLine("============ Usuarios Push ============");
        //    var notifiquemeRn = new NotifiquemeRN();
        //    NotifiquemeOV notifiquemeOv;
        //    var usuariosPushLBW = notifiquemeRn.BuscarPushLBW();
        //    var i = 0;
        //    foreach (var usuarioPushLBW in usuariosPushLBW)
        //    {
        //        notifiquemeOv = new NotifiquemeOV();
        //        Console.WriteLine("Email " + i + " = " + usuarioPushLBW.email);
        //        notifiquemeOv.email_usuario_push = usuarioPushLBW.email;
        //        notifiquemeOv.nm_usuario_push = usuarioPushLBW.nome;
        //        notifiquemeOv.st_push = usuarioPushLBW.st_push;
        //        notifiquemeOv.senha_usuario_push = "6e413a7638e56929717a5b5fce52fae7";

        //        foreach (var criacao_norma_monitorada in usuarioPushLBW.criacao_normas_monitoradas)
        //        {
        //            var criacao_norma_monitoradaOv = new CriacaoDeNormaMonitoradaPushOV();
        //            criacao_norma_monitoradaOv.ch_orgao_criacao = criacao_norma_monitorada.ch_orgao_criacao;
        //            criacao_norma_monitoradaOv.ch_tipo_norma_criacao = criacao_norma_monitorada.ch_tipo_norma_criacao;
        //            criacao_norma_monitoradaOv.st_criacao = criacao_norma_monitorada.st_criacao;
        //            criacao_norma_monitoradaOv.primeiro_conector_criacao = criacao_norma_monitorada.primeiro_conector_criacao;
        //            notifiquemeOv.criacao_normas_monitoradas.Add(criacao_norma_monitoradaOv);
        //        }

        //        foreach (var norma_monitorada in usuarioPushLBW.normas_monitoradas)
        //        {
        //            var norma_monitoradaOv = new NormaMonitoradaPushOV();
        //            norma_monitoradaOv.ch_norma_monitorada = norma_monitorada.id_norma_monitorada.ToString();
        //            norma_monitoradaOv.st_norma_monitorada = norma_monitorada.st_norma_monitorada;
        //            norma_monitoradaOv.dt_cadastro_norma_monitorada = norma_monitorada.dt_cadastro_norma_monitorada.Split(' ')[0];
        //            notifiquemeOv.normas_monitoradas.Add(norma_monitoradaOv);
        //        }
        //        notifiquemeRn.Incluir(notifiquemeOv);
        //        i++;
        //    }
        //    Console.WriteLine("Fim do processo");
        //}

        private void AtualizarUsuarioCadastrador()
        {
            Console.WriteLine("========= Atualizar Usuario Cadastrador da Norma =========");
            var normaRn = new NormaRN();
            var normaOv = new NormaOV();
            var normasLBW = normaRn.BuscarUsuarioCadastradorNormasLBW();
            var listaNormasUsuarioAlterado = new List<string>();
            var listaNormasComAlteracao = new List<string>();
            try
            {
                foreach (var normaLbw in normasLBW)
                {
                    var id_norma = normaLbw.Id.ToString();
                    int int_id_norma;
                    
                    if (Int32.TryParse(id_norma, out int_id_norma) && int_id_norma > 78984)
                    {
                        Console.WriteLine("id da norma: " + id_norma);
                        var select = new string[] { "id_doc", "alteracoes" };
                        normaOv = normaRn.BuscarNorma(id_norma, select);
                        if (!string.IsNullOrEmpty(normaLbw.UsuarioQueCadastrou))
                        {
                            var usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_login_usuario.ToLower() == normaLbw.UsuarioQueCadastrou.ToLower());
                            if (usuarios.Count() > 0)
                            {
                                if (normaOv != null)
                                {
                                    normaRn.AtualizarPath(normaOv._metadata.id_doc, "nm_login_usuario_cadastro", usuarios.First<UsuarioOV>().nm_login_usuario, null);
                                    Console.WriteLine("Norma alterada: Usuario Cadastrador - ID = " + id_norma);
                                    listaNormasUsuarioAlterado.Add(id_norma);
                                    if (!string.IsNullOrEmpty(normaLbw.DataDoCadastro))
                                    {
                                        normaRn.AtualizarPath(normaOv._metadata.id_doc, "dt_cadastro", Convert.ToDateTime(normaLbw.DataDoCadastro).ToString("dd'/'MM'/'yyyy HH:mm:ss"), null);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(normaLbw.UsuarioDaUltimaAlteracao))
                        {
                            var usuarios = _usuarios.Where<UsuarioOV>(u => u.nm_usuario == normaLbw.UsuarioDaUltimaAlteracao);
                            if (usuarios.Count() > 0)
                            {
                                if (normaOv != null)
                                {
                                    normaOv.alteracoes.Insert(0, (new AlteracaoOV { nm_login_usuario_alteracao = usuarios.First<UsuarioOV>().nm_login_usuario, dt_alteracao = Convert.ToDateTime(normaLbw.DataDaUltimaAlteracao).ToString("dd'/'MM'/'yyyy HH:mm:ss") }));
                                    normaRn.AtualizarPath(normaOv._metadata.id_doc, "alteracoes", JSON.Serialize(normaOv.alteracoes), null);
                                    Console.WriteLine("Norma alterada: Lista de Alteracoes - ID = " + id_norma);
                                    listaNormasComAlteracao.Add(id_norma);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Lista de normas alteradas no usuario cadastrador:");
                Console.WriteLine(listaNormasUsuarioAlterado);
                Console.WriteLine("Total: " + listaNormasUsuarioAlterado.Count());
                Console.WriteLine("Lista de normas alteradas na lista de alteracoes:");
                Console.WriteLine(listaNormasComAlteracao);
                Console.WriteLine("Total: " + listaNormasComAlteracao.Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private void SalvarJson(object data, string nm_diretorio, string nm_file)
        {
            if (!Directory.Exists(dir + nm_diretorio))
            {
                Directory.CreateDirectory(dir + nm_diretorio);
            }
            System.IO.File.WriteAllText(dir + nm_diretorio + "\\" + nm_file, JSON.Serialize(data));
        }

        private List<T> RecuperarJson<T>(string nm_diretorio)
        {
            var lista = new List<T>();
            if (Directory.Exists(dir + nm_diretorio))
            {
                foreach (var file in Directory.GetFiles(dir + nm_diretorio))
                {
                    var texto = System.IO.File.ReadAllText(file);
                    lista.Add(JSON.Deserializa<T>(texto));
                }
            }
            return lista;
        }

        private void EscreverLogDocNaoMigrado(string log, string chave_action = "")
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\erro_migracao_" + chave_action + ".txt", true);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }

        private void EscreverLogDocErro(Exception ex, object data, string nm_base, string chave_action = "")
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\erro_aplicacao_" + nm_base + "_" + chave_action + ".txt", true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
            sb.AppendLine("StackTrace: " + ex.StackTrace);
            sb.AppendLine("Mensagem: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
            sb.AppendLine("JsonDoc:" + JSON.Serialize(data));
            writer.Write(sb.ToString());
            writer.Flush();
            writer.Close();
        }

        private void EscreverLogErroMigracao(Exception ex, object data, string nm_base, string chave_action)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\erro_"+nm_base+"_" + chave_action + ".txt", true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
            sb.AppendLine("StackTrace: " + ex.StackTrace);
            sb.AppendLine("Mensagem: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
            sb.AppendLine("Base: " + nm_base);
            sb.AppendLine("JsonData:" + JSON.Serialize(data));
            writer.Write(sb.ToString());
            writer.Flush();
            writer.Close();
        }

        private void EscreverLog(Exception ex)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\log_exception.txt", true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
            sb.AppendLine("StackTrace: " + ex.StackTrace);
            sb.AppendLine("Mensagem: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
            writer.Write(sb.ToString());
            writer.Flush();
            writer.Close();
        }

        private void EscreverLog(string msg)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\log.txt", true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
            sb.AppendLine("Mensagem: " + msg);
            writer.Write(sb.ToString());
            writer.Flush();
            writer.Close();
        }

        private void EscreverLog(Exception ex, string msg)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\log.txt", true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
            sb.AppendLine("StackTrace: " + ex.StackTrace);
            sb.AppendLine("Mensagem: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
            sb.AppendLine("msg: " + msg);
            writer.Write(sb.ToString());
            writer.Flush();
            writer.Close();
        }

        private void EndAction(string chave_lista_controle)
        {
            lista_controle_action.RemoveAll(l => l == chave_lista_controle);
        }

        private void ResetarLightBase()
        {
            string fileBatch = AppDomain.CurrentDomain.BaseDirectory + @"ResetLightBase.bat";
            Process.Start(fileBatch).WaitForExit();
            Thread.Sleep(20000);
        }
    }
}
