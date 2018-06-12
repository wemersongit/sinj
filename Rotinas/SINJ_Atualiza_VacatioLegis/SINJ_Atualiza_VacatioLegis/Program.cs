using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;

namespace SINJ_Atualiza_VacatioLegis
{
    class Program
    {
        private FileInfo _file_error;
        private FileInfo _file_info;
        private StringBuilder _sb_error;
        private StringBuilder _sb_info;
        private DateTime _dtInicio;

        public Program()
        {
            _dtInicio = DateTime.Now;
            // Cria uma pasta de log por dia, separado por mes e por ano
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Atualiza_VacatioLegis_ERROR_" + _dtInicio.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Atualiza_VacatioLegis_INFO_" + _dtInicio.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");

            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
            Console.Clear();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            try
            {
                if (Convert.ToBoolean(Config.ValorChave("MonitorarPush", true)))
                {
                    program.AtualizarVacatioLegis();
                }
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                program._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
            program.Log();
        }

        private void AtualizarVacatioLegis()
        {
            Pesquisa pesquisa_norma = new Pesquisa();
            NormaRN normaRn = new NormaRN();
            pesquisa_norma.literal = string.Format("st_vacatio_legis AND dt_inicio_vigor::date <= '{0}'", DateTime.Now.ToString("dd/MM/yyyy"));
            pesquisa_norma.limit = null;
            pesquisa_norma.order_by = new Order_By() { asc = new string[] { "dt_assinatura::abstime" } };
            var resultNormas = normaRn.Consultar(pesquisa_norma);

            if (resultNormas.results != null && resultNormas.results.Count > 0)
            {
                foreach(var normaAlteradora in resultNormas.results){
                    if(normaAlteradora.vides != null && normaAlteradora.vides.Count > 0){
                        foreach(var videAlterador in normaAlteradora.vides){
                            //Se o vide em questão afeta a norma atualizadora pula para o proximo
                            if(videAlterador.in_norma_afetada){
                                continue;
                            }
                            try
                            {
                                var normaAlterada = normaRn.Doc(videAlterador.ch_norma_vide);
                                if (!normaAlterada.st_situacao_forcada)
                                {
                                    var situacao = normaRn.ObterSituacao(normaAlterada.vides);
                                    
                                    if (normaAlterada.ch_situacao != situacao.ch_situacao)
                                    {
                                        normaAlterada.ch_situacao = situacao.ch_situacao;
                                        normaAlterada.nm_situacao = situacao.nm_situacao;
                                        normaAlterada.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = "vacatio_legis" });
                                        if (normaRn.Atualizar(normaAlterada._metadata.id_doc, normaAlterada))
                                        {
                                            if (normaAlterada.vides != null && normaAlterada.vides.Count > 0)
                                            {
                                                foreach (var videAlterado in normaAlterada.vides)
                                                {
                                                    if(videAlterado.ch_vide == videAlterador.ch_vide){
                                                        normaRn.VerificarDispositivosESalvarOsTextosAntigosDasNormas(normaAlteradora, normaAlterada, videAlterador, videAlterado, "vacatio_legis");
                                                        normaRn.VerificarDispositivosEAlterarOsTextosDasNormas(normaAlteradora, normaAlterada, videAlterador, videAlterado);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
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
