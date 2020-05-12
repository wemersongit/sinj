using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using System.IO;
using util.BRLight;
using TCDF.Sinj.OV;

namespace exportar_texto_diario
{
    class Program
    {
        private FileInfo _file;
        
        static void Main(string[] args)
        {
            try
            {
                new Program().MigrarTexto();
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                Console.Beep();
                Console.Read();
            }
        }

        private void MigrarTexto(){

            ulong from = 0;
            ulong size = 100;
            ulong result_count = 1;
            var query = new Pesquisa();
            var diarionRn = new TCDF.Sinj.RN.DiarioRN();
            while(from <= result_count){
                try
                {
                    query.offset = from.ToString();
                    query.limit = size.ToString();
                    query.order_by.asc = new string[] { "dt_assinatura" };

                    Console.WriteLine("offset: " + query.offset);

                    var result = diarionRn.Consultar(query);
                    result_count = result.result_count;

                    foreach (var diario in result.results)
                    {
                        try
                        {
                            Console.WriteLine("id_doc: " + diario._metadata.id_doc);
                            if (!string.IsNullOrEmpty(diario.ar_diario.id_file))
                            {
                                _file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "textos_diarios" + Path.DirectorySeparatorChar.ToString() + diario.nm_tipo_fonte + Path.DirectorySeparatorChar.ToString() + DateTime.Parse(diario.dt_assinatura).ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + DateTime.Parse(diario.dt_assinatura).ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + diario.nm_tipo_fonte + "_" + diario.nr_diario + "_" + diario.secao_diario + "_de_" + diario.dt_assinatura.Replace("/", "") + ".txt");
                                var ar_diario = JSON.Deserializa<ArquivoFullOV>(diarionRn.GetDoc(diario.ar_diario.id_file));
                                if (!string.IsNullOrEmpty(ar_diario.filetext))
                                {
                                    if (!_file.Directory.Exists)
                                    {
                                        _file.Directory.Create();
                                    }
                                    var stream = _file.AppendText();
                                    stream.Write(ar_diario.filetext);
                                    stream.Flush();
                                    stream.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                            Console.WriteLine("Exception: " + mensagem);
                            Console.Beep();
                            Console.Read();
                        }
                    }
                    from += size;
                }
                catch (Exception ex)
                {
                    var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                    Console.WriteLine("Exception: " + mensagem);
                    Console.Beep();
                    Console.Read();
                }
            }
        }
    }
}
