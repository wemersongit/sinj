using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using System.IO;
using util.BRLight;

namespace SINJ.Desfazer_Revogados.App
{
    class Program
    {
        private FileInfo _file_error;
        private FileInfo _file_info;
        private StringBuilder _sb_error;
        private StringBuilder _sb_info;
        private string _chave;
        public Program()
        {
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
            Console.Clear();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            var normas_atualizadas = 0;
            var normas_que_deram_erro = 0;
            try
            {
                var normaRn = new NormaRN();
                ulong offset = 0;
                ulong total = 1;
                while (offset < total)
                {
                    var result = normaRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "500", order_by = new Order_By { asc = new string[] { "id_doc" } } });

                    total = result.result_count;
                    Console.WriteLine("offset = " + offset);
                    Console.WriteLine("total = " + total);
                    Console.WriteLine("result.results = " + result.results.Count);
                    offset += 500;
                    foreach (var norma in result.results)
                    {
                        var situacao = normaRn.ObterSituacao(norma.vides);
                        var nm_situacao_antiga = norma.nm_situacao;
                        var nm_situacao_nova = situacao.nm_situacao;
                        if (situacao.ch_situacao != norma.ch_situacao)
                        {
                            norma.ch_situacao = situacao.ch_situacao;
                            norma.nm_situacao = situacao.nm_situacao;
                            Console.WriteLine("Atualizando Norma " + norma._metadata.id_doc + ". De " + nm_situacao_antiga + " para " + nm_situacao_nova);
                            if (normaRn.Atualizar(norma._metadata.id_doc, norma))
                            {
                                normas_atualizadas++;
                                program._sb_info.AppendLine(DateTime.Now + ": Norma " + norma._metadata.id_doc + " atualizada. De " + nm_situacao_antiga + " para " + nm_situacao_nova);
                            }
                            else
                            {
                                normas_que_deram_erro++;
                                program._sb_error.AppendLine(DateTime.Now + ": Norma " + norma._metadata.id_doc + "não foi atualizada. De " + nm_situacao_antiga + " para " + nm_situacao_nova);
                            }
                        }
                    }
                    program.Log();
                }
            }
            catch (Exception ex)
            {
                program._sb_error.AppendLine(DateTime.Now + ": " + Excecao.LerTodasMensagensDaExcecao(ex, false));
            }
            program._sb_info.AppendLine(DateTime.Now + ": Normas atualizadas = " + normas_atualizadas);
            program._sb_info.AppendLine(DateTime.Now + ": Normas que deram erro = " + normas_que_deram_erro);
            program.Log();
            
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
