using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.OV;
using System.IO;

namespace ajustar_edicao_diario
{
    class Program
    {

        private FileInfo _file_error;
        private FileInfo _file_info;
        private FileInfo _file_secoes;
        private StringBuilder _sb_error;
        private StringBuilder _sb_info;
        private StringBuilder _sb_secoes;
        public Program()
        {
            Console.Clear();
        }
        static void Main(string[] args)
        {
            new Program().AtualizarDiarios();
            new Program().AtualizarNormas();
        }

        private void AtualizarNormas()
        {
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "NORMAS_ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "NORMAS_INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_secoes = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "NORMAS_SECOES_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
            _sb_secoes = new StringBuilder();

            //var _ad = new AcessoAD<DiarioOV>("sinj_diario");
            Pesquisa query = new Pesquisa() { literal = "", offset = "0", limit = "500", order_by = new Order_By { asc = new string[] { "id_doc" } }, select = new string[] { "id_doc", "fontes" } };

            var result_length = 1;
            var i = 0;
            var iAlterado = 0;
            var iSucesso = 0;
            var iFalha = 0;
            while (result_length > 0)
            {
                try
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Consulta:");
                    Console.WriteLine(JSON.Serialize<Pesquisa>(query));

                    var result = new AcessoAD<NormaOV>("sinj_norma").Consultar(query);
                    result_length = result.results.Count;
                    foreach (var norma in result.results)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Consulta:");
                        Console.WriteLine("Total de Docs: " + result.result_count);
                        Console.WriteLine("Docs Processados: " + i);
                        Console.WriteLine("Docs Alterados: " + iAlterado);
                        Console.WriteLine("Doc em Execução: " + norma._metadata.id_doc);
                        Console.WriteLine("Docs com sucesso: " + iSucesso);
                        Console.WriteLine("Docs com falha: " + iFalha);
                        try
                        {
                            AlterarCamposDaNorma(norma);
                            var retorno = new AcessoAD<NormaOV>("sinj_norma").pathPut(norma._metadata.id_doc, "fontes", JSON.Serialize<List<Fonte>>(norma.fontes),null);
                            if (retorno == "UPDATED")
                            {
                                iSucesso++;
                                _sb_info.AppendLine(DateTime.Now + ": id_doc " + norma._metadata.id_doc + " alterado com sucesso.");
                            }
                            else
                            {
                                _sb_info.AppendLine(DateTime.Now + ": id_doc " + norma._metadata.id_doc + " não salvou no banco.");
                                throw new Exception("pathPut retornou "+retorno+";");
                            }
                        }
                        catch (Exception ex)
                        {
                            iFalha++;
                            Console.Clear();
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("Erro:" + Excecao.LerTodasMensagensDaExcecao(ex, false));
                            Console.WriteLine("StackTrace:" + ex.StackTrace);
                            Console.Beep(10000, 1000);

                            _sb_error.AppendLine(DateTime.Now + ": Exception - " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                            _sb_error.AppendLine(DateTime.Now + ": StackTrace - " + ex.StackTrace);
                            _sb_error.AppendLine(DateTime.Now + ": id_doc " + norma._metadata.id_doc);
                            //Console.Read();
                        }
                        i++;
                    }
                    query.offset = (Convert.ToInt64(query.offset) + 500).ToString();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Erro:" + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    Console.WriteLine("StackTrace:" + ex.StackTrace);
                    Console.Beep(10000, 1000);

                    _sb_error.AppendLine(DateTime.Now + ": Exception - " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    _sb_error.AppendLine(DateTime.Now + ": StackTrace - " + ex.StackTrace);
                    //Console.Read();
                }
            }

            Log();
        }

        private void AlterarCamposDaNorma(NormaOV norma)
        {
            foreach (var fonte in norma.fontes)
            {
                if (!string.IsNullOrEmpty(fonte.ch_tipo_fonte) && !string.IsNullOrEmpty(fonte.dt_publicacao))
                {
                    Pesquisa query = new Pesquisa() { literal = "ch_tipo_fonte='" + fonte.ch_tipo_fonte + "' AND dt_assinatura='" + fonte.dt_publicacao + "'", offset = "0", limit = "1", order_by = new Order_By { asc = new string[] { "id_doc" } } };
                    var result = new AcessoAD<DiarioOV>("sinj_diario").Consultar(query);
                    if(result.result_count == 1){
                        fonte.ds_diario = result.results[0].nm_tipo_fonte + " nº " +
                        result.results[0].nr_diario + 
                        (!string.IsNullOrEmpty(result.results[0].cr_diario) ? " " + result.results[0].cr_diario : "") +
                        (((!string.IsNullOrEmpty(result.results[0].nm_tipo_edicao) && result.results[0].nm_tipo_edicao != "Normal") || !string.IsNullOrEmpty(result.results[0].nm_diferencial_edicao)) ? " Edição " + result.results[0].nm_tipo_edicao + (!string.IsNullOrEmpty(result.results[0].nm_diferencial_edicao) ? " " + result.results[0].nm_diferencial_edicao : "") : "") +
                        (result.results[0].st_suplemento ? ", Suplemento" + (!string.IsNullOrEmpty(result.results[0].nm_diferencial_suplemento) ? " " + result.results[0].nm_diferencial_suplemento : "") : "") +
                        ", seção " + result.results[0].secao_diario + " de " +
                        result.results[0].dt_assinatura;
                        if (result.results[0].ar_diario != null && !string.IsNullOrEmpty(result.results[0].ar_diario.id_file)){
                            fonte.ar_diario = result.results[0].ar_diario;
                        }
                        else if (result.results[0].arquivos != null && result.results[0].arquivos.Count == 1)
                        {
                            if (!string.IsNullOrEmpty(result.results[0].arquivos[0].arquivo_diario.id_file))
                            {
                                fonte.ar_diario = result.results[0].arquivos[0].arquivo_diario;
                            }
                        }
                    }
                }
            }
        }

        private void AtualizarDiarios()
        {
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "DIARIOS_ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "DIARIOS_INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_secoes = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "DIARIOS_SECOES_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
            _sb_secoes = new StringBuilder();
            //var _ad = new AcessoAD<DiarioOV>("sinj_diario");
            Pesquisa query = new Pesquisa() { literal = "", offset = "0", limit = "500", order_by = new Order_By { asc = new string[] { "id_doc" } } };
            var result_length = 1;
            var i = 0;
            var iAlterado = 0;
            var iSucesso = 0;
            var iFalha = 0;
            while (result_length > 0)
            {
                try
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Consulta:");
                    Console.WriteLine(JSON.Serialize<Pesquisa>(query));

                    var result = new AcessoAD<DiarioOV>("sinj_diario").Consultar(query);
                    result_length = result.results.Count;
                    foreach (var diario in result.results)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Consulta:");
                        Console.WriteLine("Total de Docs: " + result.result_count);
                        Console.WriteLine("Docs Processados: " + i);
                        Console.WriteLine("Docs Alterados: " + iAlterado);
                        Console.WriteLine("Doc em Execução: " + diario._metadata.id_doc);
                        Console.WriteLine("Docs com sucesso: " + iSucesso);
                        Console.WriteLine("Docs com falha: " + iFalha);
                        try
                        {
                            if (AlterarCamposDoDiario(diario))
                            {
                                iAlterado++;
                                new DiarioRN().GerarChaveDoDiario(diario);
                                if (new AcessoAD<DiarioOV>("sinj_diario").Alterar(diario._metadata.id_doc, diario))
                                {
                                    iSucesso++;
                                    _sb_info.AppendLine(DateTime.Now + ": id_doc " + diario._metadata.id_doc + " alterado com sucesso.");
                                }
                                else
                                {
                                    _sb_info.AppendLine(DateTime.Now + ": id_doc " + diario._metadata.id_doc + " não salvou no banco.");
                                    throw new Exception("_ad.Alterar(diario._metadata.id_doc, diario) retornou false;");
                                }
                            }
                            else
                            {
                                _sb_info.AppendLine(DateTime.Now + ": id_doc " + diario._metadata.id_doc + " não sofreu alteração.");
                                _sb_secoes.AppendLine(DateTime.Now + ": id_doc " + diario._metadata.id_doc + ". Seção: " + diario.secao_diario);
                                throw new Exception("AlterarCamposDoDiario(diario) retornou false;");
                            }
                        }
                        catch (Exception ex)
                        {
                            iFalha++;
                            Console.Clear();
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("Erro:" + Excecao.LerTodasMensagensDaExcecao(ex, false));
                            Console.WriteLine("StackTrace:" + ex.StackTrace);
                            Console.Beep(10000, 1000);

                            _sb_error.AppendLine(DateTime.Now + ": Exception - " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                            _sb_error.AppendLine(DateTime.Now + ": StackTrace - " + ex.StackTrace);
                            _sb_error.AppendLine(DateTime.Now + ": id_doc " + diario._metadata.id_doc);
                            //Console.Read();
                        }
                        i++;
                    }
                    query.offset = (Convert.ToInt64(query.offset) + 500).ToString();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Erro:" + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    Console.WriteLine("StackTrace:" + ex.StackTrace);
                    Console.Beep(10000, 1000);

                    _sb_error.AppendLine(DateTime.Now + ": Exception - " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    _sb_error.AppendLine(DateTime.Now + ": StackTrace - " + ex.StackTrace);
                    //Console.Read();
                }
            }

            Log();
        }

        private void Log()
        {
            if (!_file_error.Directory.Exists)
            {
                _file_error.Directory.Create();
            }
            var stream_error = _file_error.AppendText();
            stream_error.Write(_sb_error.ToString());
            stream_error.Flush();
            stream_error.Close();

            if (!_file_info.Directory.Exists)
            {
                _file_info.Directory.Create();
            }
            var stream_info = _file_info.AppendText();
            stream_info.Write(_sb_info.ToString());
            stream_info.Flush();
            stream_info.Close();

            if (!_file_secoes.Directory.Exists)
            {
                _file_secoes.Directory.Create();
            }
            var stream_secoes = _file_secoes.AppendText();
            stream_secoes.Write(_sb_secoes.ToString());
            stream_secoes.Flush();
            stream_secoes.Close();
            _sb_error.Clear();
            _sb_info.Clear();
            _sb_secoes.Clear();
        }

        public bool AlterarCamposDoDiario(DiarioOV diario)
        {
            var bAlterou = false;
            var tipoDiarioDodf = new TipoDeFonteOV { ch_tipo_fonte = "1", nm_tipo_fonte = "DODF" };
            var tipoDiarioDou = new TipoDeFonteOV { ch_tipo_fonte = "3", nm_tipo_fonte = "DOU" };
            var tipoDiarioDistritoFederal = new TipoDeFonteOV { ch_tipo_fonte = "3b3a2d936f2241758921c951c53f4ff9", nm_tipo_fonte = "Distrito Federal" };
            var tipoDiarioEncarteEspecial = new TipoDeFonteOV { ch_tipo_fonte = "bae70b0e72b74453986c7ea26d36e371", nm_tipo_fonte = "Encarte Especial" };
            var tipoDiarioServidor = new TipoDeFonteOV { ch_tipo_fonte = "da474624d627464299cfc60f04424166", nm_tipo_fonte = "O Servidor" };
            var tipoDiarioDfCultura = new TipoDeFonteOV { ch_tipo_fonte = "9dc0ffe544924a49a6393a2b56bf0585", nm_tipo_fonte = "DF Cultura" };

            var tipoEdicaoNormal = new TipoDeEdicaoOV { ch_tipo_edicao = "a3bec9b53c444d64af5947c832e10b99", nm_tipo_edicao = "Normal" };
            var tipoEdicaoExtra = new TipoDeEdicaoOV { ch_tipo_edicao = "0f76157ae4d54bfeb2d3a1c74968096e", nm_tipo_edicao = "Extra" };
            var tipoEdicaoEspecial = new TipoDeEdicaoOV { ch_tipo_edicao = "c899906a8836430c8a1805e9e280549f", nm_tipo_edicao = "Especial" };

            if (diario.arquivos == null || diario.arquivos.Count == 0 || diario.arquivos[0].arquivo_diario == null || string.IsNullOrEmpty(diario.arquivos[0].arquivo_diario.id_file))
            {
                diario.arquivos = new List<ArquivoDiario>();
                ArquivoDiario arquivodiario = new ArquivoDiario(){ ds_arquivo = "" };
                arquivodiario.arquivo_diario = new ArquivoOV();
                arquivodiario.arquivo_diario.id_file = diario.ar_diario.id_file;
                arquivodiario.arquivo_diario.mimetype = diario.ar_diario.mimetype;
                arquivodiario.arquivo_diario.uuid = diario.ar_diario.uuid;
                arquivodiario.arquivo_diario.filename = diario.ar_diario.filename;
                arquivodiario.arquivo_diario.filesize = diario.ar_diario.filesize;
                diario.arquivos.Add(arquivodiario);

            }

            diario.ar_diario = new ArquivoOV();

            var secao = diario.secao_diario.Trim().TrimStart('0').ToLower().Replace(" - ", " ").Replace("-", " ").Replace("  ", " ").Replace("01", "1").Replace("02", "2");
            if (secao == "i")
            {
                secao = "1";
            }
            else if (secao == "ii")
            {
                secao = "2";
            }
            else if (secao == "iii")
            {
                secao = "3";
            }
            else if (secao == "1,2")
            {
                secao = "1 e 2";
            }
            else if (secao == "1, 2,3" || secao == "1,2,3" || secao == "1, 2 e 2" || secao == "1,2 e 3" || secao == "1 2 3" || secao == "1, 2, 3")
            {
                secao = "1, 2 e 3";
            }

            if (secao == "1" || secao == "2" || secao == "3" || secao == "1, 2 e 3" || secao == "2 e 3" || secao == "1 e 2")
            {
                //edição normal
                //seção não muda
                if (string.IsNullOrEmpty(diario.ch_tipo_fonte))
                {
                    diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                    diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;
                }

                if (string.IsNullOrEmpty(diario.ch_tipo_edicao))
                {
                    diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                    diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;
                }
                diario.secao_diario = secao;

                bAlterou = true;
            }
            else if (secao == "normal" || secao == "úni" || secao == "único" || secao == "unico" || secao == "única" || secao == "unica" || secao == "131")
            {
                //edição normal
                //seção 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "único 2")
            {
                //edição: normal
                //diferencial edição: 2º Edição
                //seção: 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.nm_diferencial_edicao = "2ª Edição";

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "1 e 2 edição extra")
            {
                //edição extra
                //seção 1 e 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1 e 2";
                bAlterou = true;
            }
            else if (secao == "1 e 2 suplemento")
            {
                //edição normal
                //seção 1 e 2
                //suplemento
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1 e 2";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "1 edição extra")
            {
                //edição extra
                //seção 1
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1";
                bAlterou = true;
            }
            else if (secao == "edição extra" || secao == "edição extra seção 1, 2 e 3")
            {
                //edição extra
                //seção 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "edição extra 1 a, a")
            {
                //numero: 1
                //edição extra
                //seção 1 e 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.nr_diario = 1;

                diario.secao_diario = "1 e 2";
                bAlterou = true;
            }
            else if (secao == "edição extra 1 a, b")
            {
                //numero: 1
                //Letra: A
                //edição extra
                //seção 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.nr_diario = 1;

                diario.cr_diario = "A";

                diario.secao_diario = "2";
                bAlterou = true;
            }
            else if (secao == "edição extra 1b")
            {
                //numero: 1
                //Letra: B
                //edição especial
                //seção 1 e 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoEspecial.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoEspecial.nm_tipo_edicao;

                diario.nr_diario = 1;

                diario.cr_diario = "B";

                diario.secao_diario = "1 e 2";
                bAlterou = true;
            }
            else if (secao == "edição extra 1 e 2" || secao == "edição extra seção 1 e 2")
            {
                //edição extra
                //seção 1 e 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1 e 2";
                bAlterou = true;
            }
            else if (secao == "edição extra especial")
            {
                //edição extra
                //diferencial edição especial
                //seção 1 e 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.nm_diferencial_edicao = "Especial";

                diario.secao_diario = "1 e 2";
                bAlterou = true;
            }
            else if (secao == "edição extra seção 1")
            {
                //edição extra
                //seção 1
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1";
                bAlterou = true;
            }
            else if (secao == "edição extra seção 2")
            {
                //edição extra
                //seção 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "2";
                bAlterou = true;
            }
            else if (secao == "edição extra seção 2 e 3")
            {
                //edição extra
                //seção 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "2 e 3";
                bAlterou = true;
            }
            else if (secao == "edição extra seção 3" || secao == "3 edição extra")
            {
                //edição extra
                //seção 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "3";
                bAlterou = true;
            }
            else if (secao == "dou")
            {
                //tipo DOU
                //edição normal
                //seção 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDou.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDou.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "dou suplemento")
            {
                //tipo DOU
                //edição normal
                //seção 1, 2 e 3
                //suplmeneto sim
                diario.nm_tipo_fonte = tipoDiarioDou.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDou.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "a")
            {
                //tipo DOU
                //numero 13
                //letra A
                //edição normal
                //seção 1
                diario.nm_tipo_fonte = tipoDiarioDou.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDou.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.nr_diario = 13;
                diario.cr_diario = "A";

                diario.secao_diario = "1";
                bAlterou = true;
            }
            else if (secao == "aviso importante")
            {
                //pendente de revisão
                //edição normal
                //diferencial edição Aviso Importante
                //seção -
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.nm_diferencial_edicao = "Aviso Importante";

                diario.st_pendente = true;
                bAlterou = true;

                //diario.secao_diario = "1, 2 e 3";
            }
            else if (secao == "distrito federal")
            {
                //pendente de revisão
                //tipo Distrito Federal
                //edição normal
                //seção -
                diario.nm_tipo_fonte = tipoDiarioDistritoFederal.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDistritoFederal.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.st_pendente = true;
                bAlterou = true;

                //diario.secao_diario = "1, 2 e 3";
            }
            else if (secao == "ed. 1")
            {
                //edição normal
                //diferencial edição 1ª Edição
                //seção 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.nm_diferencial_edicao = "1ª Edição";

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "ed. 2")
            {
                //edição normal
                //diferencial edição 2ª Edição
                //seção 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.nm_diferencial_edicao = "2ª Edição";

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "ed. especial")
            {
                //edição especial
                //seção 1 e 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoEspecial.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoEspecial.nm_tipo_edicao;

                diario.secao_diario = "1 e 2";
                bAlterou = true;
            }
            else if (secao == "encarte especial")
            {
                //pendente de revisão
                //tipo Encarte Especial
                //edição normal
                //seção -
                diario.nm_tipo_fonte = tipoDiarioEncarteEspecial.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioEncarteEspecial.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.st_pendente = true;
                bAlterou = true;

                //diario.secao_diario = "1, 2 e 3";
            }
            else if (secao == "especial")
            {
                //pendente de revisão
                //tipo Distrito Federal
                //edição especial
                //seção - 
                diario.nm_tipo_fonte = tipoDiarioDistritoFederal.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDistritoFederal.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoEspecial.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoEspecial.nm_tipo_edicao;

                diario.st_pendente = true;
                bAlterou = true;

                //diario.secao_diario = "1, 2 e 3";
            }
            else if (secao == "edição especial")
            {
                //edição especial
                //seção 1, 2 e 3
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoEspecial.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoEspecial.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "extra")
            {
                //pendente de revisão
                //edição extra
                //seção -
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.st_pendente = true;

                bAlterou = true;
            }
            else if (secao == "extra suplemento 2")
            {
                //edição extra
                //seção 1, 2 e 3
                //suplmeneto sim
                //diferencial suplemento 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";

                diario.st_suplemento = true;
                diario.nm_diferencial_suplemento = "2";
                bAlterou = true;
            }
            else if (secao == "extra suplemento 1" || secao == "extra suplemento i")
            {
                //edição extra
                //seção 1, 2 e 3
                //suplmeneto sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "o servidor")
            {
                //pendente de revisão
                //tipo O Servidor
                //edição normal
                //seção -
                diario.nm_tipo_fonte = tipoDiarioServidor.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioServidor.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.st_pendente = true;

                //diario.secao_diario = "1, 2 e 3";
                bAlterou = true;
            }
            else if (secao == "suplemento" || secao == "suplemento 1" || secao == "suplemento 2" || secao == "suplemento 3" || secao == "suplemento 4" || secao == "suplemento 5" || secao == "suplemento 5a" || secao == "suplemento 5b" || secao == "suplemento 6" || secao == "suplemento a" || secao == "suplemento b" || secao == "suplemento c" || secao == "suplemento b iptu" || secao == "suplemento b seção 1, 2 e 3" || secao == "suplemento especial" || secao == "suplemento i" || secao == "suplemento ii" || secao == "suplemento parte 2" || secao == "suplemento parte 3" || secao == "suplemento parte 5")
            {
                //pendente de revisão
                //edição ?
                //seção ?
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                //diario.ch_tipo_edicao = tipoEdicaoExtra.ch_tipo_edicao;
                //diario.nm_tipo_edicao = tipoEdicaoExtra.nm_tipo_edicao;

                //diario.secao_diario = "1, 2 e 3";

                diario.st_pendente = true;
                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento 193")
            {
                //edição normal
                //seção 1
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento a ipva")
            {
                //edição normal
                //seção 1
                //suplemento sim
                //diferencial suplemeneto A
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1";

                diario.st_suplemento = true;
                diario.nm_diferencial_suplemento = "A";
                bAlterou = true;
            }
            else if (secao == "suplemento mensal do distrito federal")
            {
                //pendente de revisão
                //tipo DF Cultura
                //edição normal
                //seção -
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDfCultura.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDfCultura.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                //diario.secao_diario = "1, 2 e 3";

                diario.st_pendente = true;
                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento seção 1" || secao == "suplemento seção i")
            {
                //pendente de revisão
                //edição normal
                //seção 1
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1";

                diario.st_pendente = true;
                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento seção 2" || secao == "suplemento seção ii")
            {
                //pendente de revisão
                //edição normal
                //seção 2
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "2";

                diario.st_pendente = true;
                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento seção 3")
            {
                //edição normal
                //seção 3
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "3";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento seção 1 e 2")
            {
                //edição normal
                //seção 1 e 2
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1 e 2";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplemento seção 2 e 3")
            {
                //edição normal
                //seção 2 e 3
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "2 e 3";

                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplmento")
            {
                //pendente de revisão
                //edição normal
                //seção 1, 2 e 3
                //suplemento sim
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";

                diario.st_pendente = true;
                diario.st_suplemento = true;
                bAlterou = true;
            }
            else if (secao == "suplmento 2")
            {
                //edição normal
                //seção 1, 2 e 3
                //suplemento sim
                //diferencial suplemento 2
                diario.nm_tipo_fonte = tipoDiarioDodf.nm_tipo_fonte;
                diario.ch_tipo_fonte = tipoDiarioDodf.ch_tipo_fonte;

                diario.ch_tipo_edicao = tipoEdicaoNormal.ch_tipo_edicao;
                diario.nm_tipo_edicao = tipoEdicaoNormal.nm_tipo_edicao;

                diario.secao_diario = "1, 2 e 3";

                diario.st_suplemento = true;
                diario.nm_diferencial_suplemento = "2";
                bAlterou = true;
            }
            return bAlterou;
        }

    }
}
