using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using util.BRLight;
using System.IO;
using Newtonsoft.Json;
using TCDF.Sinj.OV;
using System.Net;

namespace MigrarProducaoParaTeste.ConsoleApp
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
                program._chave = program.CriarChaveProxySinj();
                //_Base base_norma = new _Base { metadata = new _MetaBase { name = "sinj_norma" } };
                //program.MigrarDocs(base_norma);
                program.MigrarArquivosNorma();
            }
            catch (Exception ex)
            {
                program._sb_error.AppendLine(DateTime.Now + ": " + Excecao.LerTodasMensagensDaExcecao(ex, false));
            }
        }

        public void MigrarArquivosNorma()
        {
            var rest = new REST_Teste(Config.ValorChave("URLBaseREST"), "sinj_norma");
            var rest_teste = new REST_Teste(Config.ValorChave("URLBaseTeste"), "sinj_norma");
            var length_page = Config.ValorChave("length_page");
            if (length_page == "-1")
            {
                length_page = "100";
            }
            //var pesquisa = new Pesquisa { offset = "0", limit = length_page, order_by = new Order_By { asc = new string[] { "id_doc" } } };
            var result_length = 1;
            var offset = "26000";
            while (result_length > 0)
            {
                var query = "?texto=" + _chave + "&_url=/sinj_norma/doc&$$={\"offset\":\"" + offset + "\",\"limit\":\"" + length_page + "\",\"order_by\":{\"asc\":[\"id_doc\"]}}";
                Console.WriteLine("Consultando normas query: " + query);
                Results<NormaOV> result;
                var sResult = rest.Consultar(query);
                try
                {
                    result = JsonConvert.DeserializeObject<Results<NormaOV>>(sResult);
                }
                catch
                {
                    continue;
                }
                //Quando o offset for maior que o limit então result.results.Count será igual a 0
                result_length = result.results.Count;
                Console.WriteLine("total de normas: " + result_length);
                if (result_length > 0)
                {
                    foreach (var _norma in result.results)
                    {
                        try
                        {
                            var update = false;
                            var sResult_teste = rest_teste.Consultar("/sinj_norma/doc?$$={\"literal\":\"ch_norma='"+_norma.ch_norma+"'\"}");
                            var norma = JsonConvert.DeserializeObject<Results<NormaOV>>(sResult_teste).results[0];
                            if (norma.ar_acao != null && !string.IsNullOrEmpty(norma.ar_acao.id_file))
                            {
                                using (var client = new WebClient())
                                {
                                    client.DownloadFile("http://www.sinj.df.gov.br/sinjcadastro/BaixarArquivoNorma.aspx?id_file=" + norma.ar_acao.id_file, "./" + norma.ar_acao.filename);
                                    try
                                    {
                                        Console.WriteLine("Migrando Arquivo " + norma.ar_acao.id_file + " do Doc " + norma._metadata.id_doc + " da Base sinj_norma");
                                        var arquivoOv = rest_teste.AnexarArquivo("./" + norma.ar_acao.filename, norma.ar_acao.filename, norma.ar_acao.mimetype, "sinj_norma");
                                        _sb_info.AppendLine(DateTime.Now + ": ar_acao enviado " + _norma.ar_acao.id_file + " do doc " + _norma._metadata.id_doc + " da base sinj_norma.");
                                        norma.ar_acao = arquivoOv;
                                        update = true;
                                    }
                                    catch
                                    {

                                    }
                                    System.IO.File.Delete("./" + norma.ar_acao.filename);
                                }
                            }
                            if (norma.ar_atualizado != null && !string.IsNullOrEmpty(norma.ar_atualizado.id_file))
                            {
                                using (var client = new WebClient())
                                {
                                    client.DownloadFile("http://www.sinj.df.gov.br/sinjcadastro/BaixarArquivoNorma.aspx?id_file=" + norma.ar_atualizado.id_file, "./" + norma.ar_atualizado.filename);
                                    try
                                    {
                                        Console.WriteLine("Migrando Arquivo " + norma.ar_atualizado.id_file + " do Doc " + norma._metadata.id_doc + " da Base sinj_norma");
                                        var arquivoOv = rest_teste.AnexarArquivo("./" + norma.ar_atualizado.filename, norma.ar_atualizado.filename, norma.ar_atualizado.mimetype, "sinj_norma");
                                        _sb_info.AppendLine(DateTime.Now + ": ar_atualizado enviado " + _norma.ar_atualizado.id_file + " do doc " + _norma._metadata.id_doc + " da base sinj_norma.");
                                        norma.ar_atualizado = arquivoOv;
                                        update = true;
                                    }
                                    catch
                                    {

                                    }
                                    System.IO.File.Delete("./" + norma.ar_atualizado.filename);
                                }
                            }
                            foreach(var fonte in norma.fontes){
                                if (fonte.ar_fonte != null && !string.IsNullOrEmpty(fonte.ar_fonte.id_file))
                                {
                                    using (var client = new WebClient())
                                    {
                                        client.DownloadFile("http://www.sinj.df.gov.br/sinjcadastro/BaixarArquivoNorma.aspx?id_file=" + fonte.ar_fonte.id_file, "./" + fonte.ar_fonte.filename);
                                        try
                                        {
                                            Console.WriteLine("Migrando Arquivo " + fonte.ar_fonte.id_file + " do Doc " + norma._metadata.id_doc + " da Base sinj_norma");
                                            var arquivoOv = rest_teste.AnexarArquivo("./" + fonte.ar_fonte.filename, fonte.ar_fonte.filename, fonte.ar_fonte.mimetype, "sinj_norma");
                                            _sb_info.AppendLine(DateTime.Now + ": ar_fonte enviado " + _norma.ar_atualizado.id_file + " do doc " + _norma._metadata.id_doc + " da base sinj_norma.");
                                            fonte.ar_fonte = arquivoOv;
                                            update = true;
                                        }
                                        catch
                                        {

                                        }
                                        System.IO.File.Delete("./" + fonte.ar_fonte.filename);
                                    }
                                }
                            }
                            if (update)
                            {
                                Console.WriteLine("Atualizando Doc " + norma._metadata.id_doc + " da Base sinj_norma");
                                if (rest_teste.Atualizar<NormaOV>(norma._metadata.id_doc, norma))
                                {
                                    _sb_info.AppendLine(DateTime.Now + ": Doc atualizado " + _norma._metadata.id_doc + " da base sinj_norma.");
                                }
                                else
                                {
                                    _sb_error.AppendLine(DateTime.Now + ": Doc não atualizou " + _norma._metadata.id_doc + " da base sinj_norma.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Doc " + norma._metadata.id_doc + " da Base sinj_norma sem arquivos...................");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("ERROR");
                            _sb_error.AppendLine(DateTime.Now + ": arquivo do doc " + _norma._metadata.id_doc + " da base sinj_norma não migrou. Exceção: "+ Excecao.LerTodasMensagensDaExcecao(ex, false) + ". StackTrace: " + ex.StackTrace);
                        }
                    }
                    Log();
                }
                offset = (Convert.ToInt64(offset) + Convert.ToInt64(length_page)).ToString();
            }
        }

        public string CriarChaveProxySinj()
        {
            var retorno_proxy = new REST_Teste(Config.ValorChave("URLBaseREST"), "").Get();
            var sDatetime = Convert.ToDateTime(retorno_proxy).ToString("ddMMyyyy");
            var dia_da_semana = (int)Convert.ToDateTime(retorno_proxy).DayOfWeek;
            var chaves_secretas = new string[] { "difundir", "sucumbir", "traslado", "quarador", "quizumba", "sequioso", "saturado" };

            var chave_encriptada = ""; for (var i = 0; i < sDatetime.Length; i++)
            {
                chave_encriptada += sDatetime[i].ToString() + chaves_secretas[dia_da_semana][i].ToString();
            }
            return chave_encriptada;
        }

        public void MigrarBases()
        {
            var bases = BuscarBases();
            var rest = new REST_Teste(Config.ValorChave("URLBaseTeste"), "");
            foreach(object _base in bases){
                try
                {
                    rest.CriarBase(_base);
                    MigrarDocs(_base);
                }
                catch (Exception ex)
                {
                    var sBase = JsonConvert.SerializeObject(_base);
                    var oBase = JsonConvert.DeserializeObject<_Base>(sBase);
                    _sb_error.AppendLine(DateTime.Now + ": " + oBase.metadata.name + " não migrou.");
                }
            }
        }

        private void MigrarDocs(object _base)
        {
            var sBase = JsonConvert.SerializeObject(_base);
            var oBase = JsonConvert.DeserializeObject<_Base>(sBase);
            var rest = new REST_Teste(Config.ValorChave("URLBaseREST"), oBase.metadata.name);
            var rest_teste = new REST_Teste(Config.ValorChave("URLBaseTeste"), oBase.metadata.name);
            var length_page = Config.ValorChave("length_page");
            if (length_page == "-1")
            {
                length_page = "100";
            }
            //var pesquisa = new Pesquisa { offset = "0", limit = length_page, order_by = new Order_By { asc = new string[] { "id_doc" } } };
            var result_length = 1;
            var offset = "0";
            while (result_length > 0)
            {
                var sResult = rest.Consultar("?texto=" + _chave + "&_url=/" + oBase.metadata.name + "/doc&$$={\"literal\":\"id_doc>78431\",\"offset\":\"" + offset + "\",\"limit\":\"" + length_page + "\",\"order_by\":{\"asc\":[\"id_doc\"]}}");

                var result = JsonConvert.DeserializeObject<Results<object>>(sResult);
                //Quando o offset for maior que o limit então result.results.Count será igual a 0
                result_length = result.results.Count;
                if (result_length > 0)
                {
                    foreach (var doc in result.results)
                    {
                        var sDoc = JsonConvert.SerializeObject(doc);
                        var oDoc = JsonConvert.DeserializeObject<NormaOV>(sDoc);
                        try
                        {
                            Console.WriteLine("Base " + oBase.metadata.name + " -> Migrando Doc " + oDoc._metadata.id_doc);
                            rest_teste.Incluir(oDoc);
                            Console.Write("OK");
                        }
                        catch (Exception ex)
                        {
                            Console.Write("ERROR");
                            _sb_error.AppendLine(DateTime.Now + ": doc " + oDoc._metadata.id_doc + " da base " + oBase.metadata.name + " não migrou.");
                        }
                    }
                    Log();
                }
                offset = (Convert.ToInt64(offset) + Convert.ToInt64(length_page)).ToString();
            }
        }

        public List<object> BuscarBases()
        {
            var rest = new REST_Teste(Config.ValorChave("URLBaseREST"), "");
            var bases = new List<object>();
            var length_page = Config.ValorChave("length_page");
            if (length_page == "-1")
            {
                length_page = "100";
            }
            var literal = Config.ValorChave("Literal");
            if (literal == "-1")
            {
                literal = null;
            }
            //var pesquisa = new Pesquisa { literal = literal, offset = "0", limit = length_page, order_by = new Order_By { asc = new string[] { "id_base" } } };
            var result_length = 1;
            var offset = "0";
            while (result_length > 0)
            {
                var sResult = rest.ConsultarBases("?texto="+_chave+"&$$={\"literal\":\""+literal+"\",\"offset\":\""+offset+"\",\"limit\":\""+length_page+"\",\"order_by\":{\"asc\":[\"id_base\"]}}");
                //var sResult = new Base().pesquisar(pesquisa);
                var result = JsonConvert.DeserializeObject<Results<object>>(sResult);
                //Quando o offset for maior que o limit então result.results.Count será igual a 0
                result_length = result.results.Count;
                if (result_length > 0)
                {
                    bases.AddRange(result.results);
                }
                offset = (Convert.ToInt64(offset) + Convert.ToInt64(length_page)).ToString();
            }
            return bases;
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
    public class Reg : metadata
    {

    }
}
