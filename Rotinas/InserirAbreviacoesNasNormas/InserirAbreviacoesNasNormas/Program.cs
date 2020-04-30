using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using System.IO;

namespace InserirAbreviacoesNasNormas
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

        private string getArg(string[] args, int index)
        {
            var arg = "";
            if(args.Length > index){
                arg = args[index];
            }
            return arg;
        }

        static void Main(string[] args)
        {
            var p = new Program();
            try
            {
                var arg = p.getArg(args, 0);
                if (arg.Equals("tipos"))
                {
                    p.atualizarSiglasNosTiposDeNorma();
                }
                p.atualizarNormas();
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                p._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
            p.Log();

        }

        private void atualizarNormas()
        {

            var rn = new NormaRN();
            ulong offset = 0;
            ulong total = 1;
            int sucesso = 0, falha = 0, i = 0;
            bool travadoNoErro = false;
            while (offset < total)
            {
                try
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Consultando offset='" + offset + "'");
                    var result = rn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "100", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                    total = result.result_count;
                    offset += 100;
                    foreach (var doc in result.results)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine("Total de Normas: " + total);
                        Console.WriteLine("Normas Processadas: " + i);
                        Console.WriteLine("Norma em Execução: " + doc._metadata.id_doc);
                        Console.WriteLine("Normas com sucesso: " + sucesso);
                        Console.WriteLine("Normas com falha: " + falha);
                        try
                        {
                            if (rn.Atualizar(doc._metadata.id_doc, doc))
                            {
                                sucesso++;
                            }
                            else
                            {
                                falha++;
                                this._sb_error.AppendLine(DateTime.Now + ": Norma não foi salva. Norma= " + doc.getDescricaoDaNorma() + ", id_doc= " + doc._metadata.id_doc);
                            }
                            travadoNoErro = false;
                        }
                        catch (Exception ex)
                        {
                            falha++;
                            if (!travadoNoErro)
                            {
                                this._sb_error.AppendLine(DateTime.Now + ": Erro ao atualizar norma. Norma= " + doc.getDescricaoDaNorma() + ", id_doc= " + doc._metadata.id_doc);
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
                    this._sb_error.AppendLine(DateTime.Now + ": Erro na raiz do método para atualizar normas.");
                    this._sb_error.AppendLine("       Mensagem da Exceção: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    this._sb_error.AppendLine("       StackTrace: " + ex.StackTrace);
                }
            }
            this._sb_info.AppendLine(DateTime.Now + ": Conclusão do procedimento para atualizar as normas.");
            this._sb_info.AppendLine("                Total de Normas: " + total);
            this._sb_info.AppendLine("                Sucesso: " + sucesso);
            this._sb_info.AppendLine("                Falhas: " + falha);
        }

        private void atualizarSiglasNosTiposDeNorma()
        {
            this._sb_info.AppendLine(DateTime.Now + ": Atualizar Siglas Na Base Tipos De Norma");
            var rn = new TipoDeNormaRN();
            var result = rn.Consultar(new Pesquisa {limit = null, order_by = new Order_By { asc = new string[] { "id_doc" } } });
            var sigla = "";
            foreach (var doc in result.results)
            {
                sigla = getSigla(doc.nm_tipo_norma);
                doc.sgs_tipo_norma = new List<string>();
                doc.sgs_tipo_norma.Add(sigla);
                if (rn.Atualizar(doc._metadata.id_doc, doc))
                {
                    this._sb_info.AppendLine(DateTime.Now + ": Sigla criada com sucesso. Nome=" + doc.nm_tipo_norma + ", sigla= " + sigla);
                }
                else
                {
                    this._sb_error.AppendLine(DateTime.Now + ": Sigla não criada. Nome=" + doc.nm_tipo_norma + ", sigla= " + sigla);
                }
            }
            this._sb_info.AppendLine(DateTime.Now + ": Conclusão do procedimento para atualizar siglas na base tipos de norma.");
        }

        private string getSigla(string nm_tipo_norma)
        {
            string sg_tipo_norma = "";

            switch (nm_tipo_norma)
            {
                case "ADC":
                    sg_tipo_norma = "ADC";
                    break;
                case "ADI":
                    sg_tipo_norma = "ADI";
                    break;
                case "ADO":
                    sg_tipo_norma = "ADO";
                    break;
                case "ADPF":
                    sg_tipo_norma = "ADPF";
                    break;
                case "AIL":
                    sg_tipo_norma = "AIL";
                    break;
                case "Ata":
                    sg_tipo_norma = "ATA";
                    break;
                case "Ato Conjunto":
                    sg_tipo_norma = "AC";
                    break;
                case "Ato da Mesa Diretora":
                    sg_tipo_norma = "AMD";
                    break;
                case "Ato Declaratório":
                    sg_tipo_norma = "AD";
                    break;
                case "Ato Normativo":
                    sg_tipo_norma = "AN";
                    break;
                case "Ato Regimental":
                    sg_tipo_norma = "AR";
                    break;
                case "Aviso":
                    sg_tipo_norma = "AVS";
                    break;
                case "Constituição Federal":
                    sg_tipo_norma = "CF";
                    break;
                case "Contrato Social":
                    sg_tipo_norma = "CS";
                    break;
                case "Decisão":
                    sg_tipo_norma = "DCS";
                    break;
                case "Decisão Administrativa":
                    sg_tipo_norma = "DCA";
                    break;
                case "Decisão do Presidente":
                    sg_tipo_norma = "DCP";
                    break;
                case "Decisão Liminar":
                    sg_tipo_norma = "DCL";
                    break;
                case "Decisão Normativa":
                    sg_tipo_norma = "DN";
                    break;
                case "Decreto":
                    sg_tipo_norma = "DEC";
                    break;
                case "Decreto Executivo":
                    sg_tipo_norma = "DE";
                    break;
                case "Decreto Legislativo":
                    sg_tipo_norma = "DL";
                    break;
                case "Decreto Lei":
                    sg_tipo_norma = "DCL";
                    break;
                case "Despacho":
                    sg_tipo_norma = "DSP";
                    break;
                case "Despacho do Governador":
                    sg_tipo_norma = "DSG";
                    break;
                case "Despacho singular":
                    sg_tipo_norma = "DSS";
                    break;
                case "Diretriz":
                    sg_tipo_norma = "DIR";
                    break;
                case "Edital":
                    sg_tipo_norma = "EDT";
                    break;
                case "Emenda a lei Orgânica":
                    sg_tipo_norma = "ELO";
                    break;
                case "Emenda Constitucional":
                    sg_tipo_norma = "EC";
                    break;
                case "Emenda Regimental":
                    sg_tipo_norma = "ER";
                    break;
                case "Estatuto":
                    sg_tipo_norma = "EST";
                    break;
                case "Exposição de Motivos":
                    sg_tipo_norma = "EM";
                    break;
                case "Instrução":
                    sg_tipo_norma = "INS";
                    break;
                case "Instrução Conjunta":
                    sg_tipo_norma = "INC";
                    break;
                case "Instrução de Serviço":
                    sg_tipo_norma = "IS";
                    break;
                case "Instrução de Serviço Conjunta":
                    sg_tipo_norma = "ISC";
                    break;
                case "Instrução Normativa":
                    sg_tipo_norma = "IN";
                    break;
                case "Instrução Normativa Conjunta":
                    sg_tipo_norma = "INC";
                    break;
                case "Lei":
                    sg_tipo_norma = "LEI";
                    break;
                case "Lei Complementar":
                    sg_tipo_norma = "LC";
                    break;
                case "Lei Orgânica":
                    sg_tipo_norma = "LODF";
                    break;
                case "Manual":
                    sg_tipo_norma = "MAN";
                    break;
                case "Mensagem do Governador":
                    sg_tipo_norma = "MG";
                    break;
                case "Norma de serviço":
                    sg_tipo_norma = "NS";
                    break;
                case "Norma Técnica":
                    sg_tipo_norma = "NT";
                    break;
                case "Ordem de Serviço":
                    sg_tipo_norma = "OS";
                    break;
                case "Ordem de Serviço Conjunta":
                    sg_tipo_norma = "OSC";
                    break;
                case "Ordem de Serviço Normativa":
                    sg_tipo_norma = "OSN";
                    break;
                case "Orientação Normativa":
                    sg_tipo_norma = "ON";
                    break;
                case "Parecer":
                    sg_tipo_norma = "PAR";
                    break;
                case "Parecer Normativo":
                    sg_tipo_norma = "PAN";
                    break;
                case "Plano":
                    sg_tipo_norma = "PLA";
                    break;
                case "Portaria":
                    sg_tipo_norma = "PRT";
                    break;
                case "Portaria Conjunta":
                    sg_tipo_norma = "POC";
                    break;
                case "Portaria Normativa":
                    sg_tipo_norma = "PN";
                    break;
                case "Portaria Reservada":
                    sg_tipo_norma = "PR";
                    break;
                case "Processo":
                    sg_tipo_norma = "PRC";
                    break;
                case "Provimento":
                    sg_tipo_norma = "PRV";
                    break;
                case "Recomendação":
                    sg_tipo_norma = "RCM";
                    break;
                case "Regimento":
                    sg_tipo_norma = "REG";
                    break;
                case "Regimento Interno":
                    sg_tipo_norma = "RI";
                    break;
                case "Regulamento":
                    sg_tipo_norma = "RGL";
                    break;
                case "Rejeição de Veto":
                    sg_tipo_norma = "RVT";
                    break;
                case "Representação":
                    sg_tipo_norma = "REP";
                    break;
                case "Resolução":
                    sg_tipo_norma = "RES";
                    break;
                case "Resolução Administrativa":
                    sg_tipo_norma = "REA";
                    break;
                case "Resolução Conjunta":
                    sg_tipo_norma = "REC";
                    break;
                case "Resolução Intergovernamental":
                    sg_tipo_norma = "REI";
                    break;
                case "Resolução Normativa":
                    sg_tipo_norma = "RN";
                    break;
                case "Resolução Ordinária":
                    sg_tipo_norma = "RO";
                    break;
                case "Súmula":
                    sg_tipo_norma = "SUM";
                    break;
                case "Súmula Administrativa":
                    sg_tipo_norma = "SA";
                    break;
                case "Termo Aditivo":
                    sg_tipo_norma = "TA";
                    break;
                default:
                    sg_tipo_norma = nm_tipo_norma;
                    break;
            }

            return sg_tipo_norma;
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
