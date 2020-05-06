using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SINJ_MetaMiner.RN;
using System.Data;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using System.IO;
using util.BRLight;
using SINJ_MetaMiner.OV;

namespace Sinj_MetaMiner.ConsoleApp
{
    class Program
    {
        private SINJ_MetaMinerRN _sinj_metaminerRn;
        private FileInfo _file_error;
        private FileInfo _file_info;
        private bool _force_full;
        private StringBuilder _sb_error;
        private StringBuilder _sb_info;
        private long updates;
        private long inserts;
        private long deletes;
        private int cursorTop;
        public Program()
        {
            updates = 0;
            inserts = 0;
            deletes = 0;
            _sinj_metaminerRn = new SINJ_MetaMinerRN();
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "Sinj_MetaMiner_ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + "Sinj_MetaMiner_INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
            Console.Clear();
        }
        static void Main(string[] args)
        {
            var program = new Program();
            try
            {
                if (util.BRLight.Config.ValorChave("ExcecutionIsAllowed") != "true")
                {
                    return;
                }
                //Caso precise atualizar todos os registros do lexml pode ser passado o parametro -f
                if (args.Count<string>(a => a == "-f") > 0)
                {
                    program._force_full = true;
                    Console.WriteLine("force_full is enabled");
                    program._sb_info.AppendLine(DateTime.Now + ": parametros recebidos => " + JSON.Serialize<string[]>(args));
                }
                //Recupera um dataset que representa a tabela do lexml do sqlserver
                var normas_lexml = program._sinj_metaminerRn.ConsultarLexml();
                var total_lexml = normas_lexml.Count();
                var normas_ch = new List<string>();
                var length_page = Config.ValorChave("length_page");
                if(length_page == "-1"){
                    length_page = "500";
                }
                //Faz uma consulta para identificar o total, que será usado no laço
                var pesquisa = new Pesquisa { limit = "1", select = new string[] {"ch_norma"} };
                var result = new TCDF.Sinj.RN.NormaRN().Consultar(pesquisa);
                var result_length = result.result_count;
                //Recupera as normas no REST e processa cada uma
                pesquisa = new Pesquisa { select = new string[] { "nm_orgao_cadastrador", "nm_tipo_norma", "ds_ementa", "dt_assinatura", "nr_norma", "fontes", "ch_norma", "dt_doc", "alteracoes", "nm_apelido", "indexacoes", "ar_atualizado" }, offset = "0", limit = length_page, order_by = new Order_By { desc = new string[] { "dt_last_up::abstime" } } };
                program.cursorTop = 7;
                

                for (ulong i = 0; i < result_length; i = i + Convert.ToUInt64(length_page))
                {
                    pesquisa.offset = i.ToString();
                    program._sb_info.AppendLine(DateTime.Now + ": Pesquisa => " + JSON.Serialize<Pesquisa>(pesquisa));
                    result = new TCDF.Sinj.RN.NormaRN().Consultar(pesquisa);
                    foreach (var norma in result.results)
                    {
                        if (program.cursorTop >= 20)
                        {
                            program.cursorTop = 7;
                            Console.Clear();
                        }
                        //Mantenho todas as normas na lista para em seguida processar o delete ('D') no dataset
                        normas_ch.Add(norma.ch_norma);
                        ///As normas que não forem processadas ficam nessa lista para em seguida, se o parametro -f (force) tiver sido passado,
                        ///serem marcadas como 'D' no dataset
                        program.CompareRegisters(norma, normas_lexml);
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Comparando Update e Insert");
                        Console.WriteLine("Normas Lexml: " + total_lexml);
                        Console.WriteLine("Normas SINJ: " + normas_ch.Count);
                        Console.WriteLine("Normas Inseridas: " + program.inserts);
                        Console.WriteLine("Normas Atualizadas: " + program.updates);
                    }
                    program.Log();
                }

                normas_ch.Sort();
                //Verifica cada linha da tabela setando row["cd_status"] para 'D' para as normas não encontradas na lista de normas 
                foreach (var norma_lexml in normas_lexml)
                {
                    var ch_norma_lexml = norma_lexml.id_registro_item.Split('/')[1].Replace(";txtatlzdo", "");
                    ///Se cd_status igual a "D" então não precisa mexer.
                    ///Se a norma não existe na lista de normas, então, ela foi deletada do SINJ, lgo marca ela com 'D'.
                    ///se for para forçar ele valida com a lista de normas_ok, assim marca com 'D' até as normas que não foram processadas.
                    if (norma_lexml.cd_status != "D")
                    {
                        if (normas_ch.IndexOf(ch_norma_lexml) > -1)
                        {
                            continue;
                        }
                        norma_lexml.cd_status = "D";
                        norma_lexml.ts_registro_gmt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        program._sb_info.AppendLine(DateTime.Now + ": DELETING => " + norma_lexml.id_registro_item);
                        if (program._sinj_metaminerRn.AtualizarDoc(norma_lexml.id_registro_item, norma_lexml) > 0)
                        {
                            program.deletes++;
                        }
                        else
                        {
                            program._sb_error.AppendLine(DateTime.Now + ": " + norma_lexml.id_registro_item + " não deletou (cd_status = 'D')...");
                        }
                    }
                    Console.SetCursorPosition(0, 5);
                    Console.WriteLine("Normas Deletadas: " + program.deletes);
                }

                program._sb_info.AppendLine(DateTime.Now + ": Total de normas no lexml - " + total_lexml);
                program._sb_info.AppendLine(DateTime.Now + ": Total de normas no SINJ - " + normas_ch.Count);
                program._sb_info.AppendLine(DateTime.Now + ": Total de normas Inseridas - " + program.inserts);
                program._sb_info.AppendLine(DateTime.Now + ": Total de normas Atualizadas - " + program.updates);
                program._sb_info.AppendLine(DateTime.Now + ": Total de normas Deletadas - " + program.deletes);
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                program._sb_error.AppendLine(DateTime.Now + ": Exception - " + mensagem);
                program._sb_error.AppendLine(DateTime.Now + ": StackTrace - " + ex.StackTrace);
            }
            program.Log();
        }

        private void CompareRegisters(NormaOV norma, List<NormaLexml> normas_lexml)
        {
            try
            {
                //Valida se a norma possui os campos necessários para ser anexada no lexml
                _sinj_metaminerRn.ValidarAuxMetadadoLexml(norma);
                var auxMetadadoLexml = _sinj_metaminerRn.GetAuxMetadadoLexml(norma);
                try
                {
                    _sinj_metaminerRn.ValidarTxMetadadoXml(norma);
                    var id_lexml = _sinj_metaminerRn.GerarIdLexml(norma);
                    var filtered_normas = normas_lexml.Where<NormaLexml>(nl => nl.id_registro_item == id_lexml);
                    if (filtered_normas.Count() > 0)
                    {
                        var norma_lexml = filtered_normas.First();
                        if (_force_full || _sinj_metaminerRn.ItIsToUpgrade(norma, norma_lexml))
                        {
                            norma_lexml.cd_status = "N";
                            norma_lexml.cd_validacao = "I";
                            norma_lexml.ts_registro_gmt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            norma_lexml.tx_metadado_xml = _sinj_metaminerRn.GetTxMetadadoXml(norma, auxMetadadoLexml);
                            _sb_info.AppendLine(DateTime.Now + ": UPDATING => " + norma_lexml.id_registro_item);
                            if (_sinj_metaminerRn.AtualizarDoc(norma_lexml.id_registro_item, norma_lexml) > 0)
                            {
                                updates++;
                            }
                            else
                            {
                                _sb_error.AppendLine(DateTime.Now + ": " + norma_lexml.id_registro_item + " não atualizou...");
                            }
                        }
                    }
                    else
                    {
                        var norma_lexml = new NormaLexml();
                        norma_lexml.id_registro_item = id_lexml;
                        norma_lexml.cd_status = "N";
                        norma_lexml.cd_validacao = "I";
                        norma_lexml.ts_registro_gmt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        norma_lexml.tx_metadado_xml = _sinj_metaminerRn.GetTxMetadadoXml(norma, auxMetadadoLexml);
                        _sb_info.AppendLine(DateTime.Now + ": INSERTING => " + norma_lexml.id_registro_item);
                        if (_sinj_metaminerRn.InserirDoc(norma_lexml) > 0)
                        {
                            inserts++;
                        }
                        else
                        {
                            _sb_error.AppendLine(DateTime.Now + ": " + norma_lexml.id_registro_item + " não inseriu...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                    _sb_error.AppendLine(DateTime.Now + ": " + mensagem);
                    Console.SetCursorPosition(0, ++cursorTop);
                    Console.WriteLine("Erro: " + DateTime.Now + ": " + mensagem);
                    Console.WriteLine("Stack: " + ex.StackTrace);
                }

                try
                {
                    _sinj_metaminerRn.ValidarTxMetadadoXmlTxtAtlzdo(norma);
                    var id_lexml_txtatlzdo = _sinj_metaminerRn.GerarIdLexml(norma) + ";txtatlzdo";
                    var filtered_normas_txtatlzdo = normas_lexml.Where<NormaLexml>(nl => nl.id_registro_item == id_lexml_txtatlzdo);
                    if (filtered_normas_txtatlzdo.Count() > 0)
                    {
                        var norma_lexml = filtered_normas_txtatlzdo.First();
                        if (_sinj_metaminerRn.ItIsToUpgrade(norma, norma_lexml))
                        {
                            norma_lexml.cd_status = "N";
                            norma_lexml.cd_validacao = "I";
                            norma_lexml.ts_registro_gmt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            norma_lexml.tx_metadado_xml = _sinj_metaminerRn.GetTxMetadadoXmlTxtAtlzdo(norma, auxMetadadoLexml);
                            _sb_info.AppendLine(DateTime.Now + ": UPDATING => " + norma_lexml.id_registro_item);
                            if (_sinj_metaminerRn.AtualizarDoc(norma_lexml.id_registro_item, norma_lexml) > 0)
                            {
                                updates++;
                            }
                            else
                            {
                                _sb_error.AppendLine(DateTime.Now + ": " + norma_lexml.id_registro_item + " não atualizou...");
                            }

                        }
                    }
                    else
                    {
                        var norma_lexml = new NormaLexml();
                        norma_lexml.id_registro_item = id_lexml_txtatlzdo;
                        norma_lexml.cd_status = "N";
                        norma_lexml.cd_validacao = "I";
                        norma_lexml.ts_registro_gmt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        norma_lexml.tx_metadado_xml = _sinj_metaminerRn.GetTxMetadadoXmlTxtAtlzdo(norma, auxMetadadoLexml);
                        _sb_info.AppendLine(DateTime.Now + ": INSERTING => " + norma_lexml.id_registro_item);
                        if (_sinj_metaminerRn.InserirDoc(norma_lexml) > 0)
                        {
                            inserts++;
                        }
                        else
                        {
                            _sb_error.AppendLine(DateTime.Now + ": " + norma_lexml.id_registro_item + " não inseriu...");
                        }

                    }
                }
                catch (Exception ex)
                {
                    var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                    _sb_error.AppendLine(DateTime.Now + ": " + mensagem);
                    Console.SetCursorPosition(0, ++cursorTop);
                    Console.WriteLine("Erro: " + DateTime.Now + ": " + mensagem);
                    Console.WriteLine("Stack: " + ex.StackTrace);
                }
            }
            catch(Exception ex){
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                _sb_error.AppendLine(DateTime.Now + ": " + mensagem);
                Console.SetCursorPosition(0, ++cursorTop);
                Console.WriteLine("Erro: " + DateTime.Now + ": " + mensagem);
                Console.WriteLine("Stack: " + ex.StackTrace);
            }
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
