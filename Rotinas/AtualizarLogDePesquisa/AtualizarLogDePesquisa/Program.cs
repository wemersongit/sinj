using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;

namespace AtualizarLogDePesquisa
{
    class Program
    {
        private FileInfo _file_error;
        private FileInfo _file_info;
        private StringBuilder _sb_error;
        private StringBuilder _sb_info;

        public Program()
        {
            // Cria uma pasta de log por dia, separado por mes e por ano
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("dd") + Path.DirectorySeparatorChar.ToString() + "ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("dd") + Path.DirectorySeparatorChar.ToString() + "INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");

            //_file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log " + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Push_App_ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            //_file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log " + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Push_App_INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");

            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
        }

        static void Main(string[] args)
        {
            var p = new Program();
            try
            {
                p.atualizarLogDePesquisa();
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                p._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
            p.Log();

        }

        private void atualizarLogDePesquisa()
        {

            var rn = new HistoricoDePesquisaRN();
            //ulong offset = 0;
            ulong total = 1;
            int sucesso = 0, falha = 0, i = 0;
            bool travadoNoErro = false;
            //while (offset < total)
            while (total > 0)
            {
                try
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    //Console.WriteLine("Consultando offset='" + offset + "'");
                    Console.WriteLine("Consultando total='" + total + "'");
                    var result = rn.Consultar(new Pesquisa { literal = "dt_last_up<'13/09/2017 12:00:00'", limit = "1000", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                    total = result.result_count;
                    //offset += 1000;
                    foreach (var doc in result.results)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine("Total de Pesquisas: " + total);
                        Console.WriteLine("Pesquisas Processadas: " + i);
                        Console.WriteLine("Pesquisa em Execução: " + doc._metadata.id_doc);
                        Console.WriteLine("Pesquisas com sucesso: " + sucesso);
                        Console.WriteLine("Pesquisas com falha: " + falha);
                        try
                        {
                            if (doc.contador <= 0)
                            {
                                doc.contador = 1;
                            }
                            if (doc.ds_historico.IndexOf("(Pesquisa Geral)") == 0)
                            {
                                doc.nm_tipo_pesquisa = "Pesquisa Geral";
                            }
                            else if (doc.ds_historico.IndexOf("(Pesquisa de Normas)") == 0)
                            {
                                doc.nm_tipo_pesquisa = "Pesquisa de Normas";
                            }
                            else if (doc.ds_historico.IndexOf("(Pesquisa de Diário)") == 0)
                            {
                                doc.nm_tipo_pesquisa = "Pesquisa de Diário";
                            }
                            else if (doc.ds_historico.IndexOf("(Pesquisa Avançada)") == 0)
                            {
                                doc.nm_tipo_pesquisa = "Pesquisa Avançada";
                            }
                            if (rn.Atualizar(doc._metadata.id_doc, doc))
                            {
                                sucesso++;
                            }
                            else
                            {
                                falha++;
                                this._sb_error.AppendLine(DateTime.Now + ": Pesquisa não foi salva. Pesquisa= " + doc.ch_consulta + ", id_doc= " + doc._metadata.id_doc);
                            }
                            travadoNoErro = false;
                        }
                        catch (Exception ex)
                        {
                            falha++;
                            if (!travadoNoErro)
                            {
                                this._sb_error.AppendLine(DateTime.Now + ": Erro ao atualizar Pesquisa. Pesquisa= " + doc.ch_consulta + ", id_doc= " + doc._metadata.id_doc);
                                this._sb_error.AppendLine("       Mensagem da Exceção: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                                this._sb_error.AppendLine("       StackTrace: " + ex.StackTrace);
                            }
                            travadoNoErro = true;
                        }

                        i++;
                    }
                }
                catch (Exception ex)
                {
                    Console.SetCursorPosition(0, 6);
                    Console.WriteLine("Erro: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    this._sb_error.AppendLine(DateTime.Now + ": Erro na raiz do método para atualizar Pesquisas.");
                    this._sb_error.AppendLine("       Mensagem da Exceção: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    this._sb_error.AppendLine("       StackTrace: " + ex.StackTrace);
                }
                if (this._sb_error.Length > 100000 || this._sb_info.Length > 100000)
                {
                    Log();
                }
            }
            this._sb_info.AppendLine(DateTime.Now + ": Conclusão do procedimento para atualizar as Pesquisa.");
            this._sb_info.AppendLine("                Total de Pesquisa: " + total);
            this._sb_info.AppendLine("                Sucessos: " + sucesso);
            this._sb_info.AppendLine("                Falhas: " + falha);
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
            _sb_error.Clear();
            _sb_info.Clear();
        }
    }
}
