using System;
using util.BRLight;
using neo.BRLightREST;
using SinjMigracaoDecretos;
using TCDF.Sinj;
using SinjMigracaoDecretos.AD;
using TCDF.Sinj.OV;
//using TCDF.Sinj.Web.ashx;
using System.Web;
using TCDF.Sinj.RN;


namespace SinjMigracaoDecretos
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Resultado da busca");

            NormaAD normaAD = new NormaAD();

            Pesquisa query = new Pesquisa();

            //Cria a query literal para a busca personalizada - nesse caso buscando todos os decretos de 2004
            query.literal = "nm_orgao_cadastrador = 'SEPLAG' and nm_tipo_norma = 'Decreto' and dt_assinatura > '01/01/2004' and dt_assinatura< '31/12/2004'";

            //Declara o limite do retorno
            query.limit = "100000";

            //Envia a query para retornar a consulta
            normaAD.Consultar(query);

            //Percorre o resultado da pesquisa
            foreach (var consulta in normaAD.Consultar(query).results)
            {

                //Mandar o arquivo para sinj_arquivo
                //vai retornar o uuid
                //montar o objeto File



                //Populando as variáveis
                try
                {
                    Doc document = new Doc("sinj_norma");

                    string nm_arquivo = document.doc(consulta.ar_atualizado.id_file).filename.Replace(".html","");

                    string arquivo_text = document.doc(consulta.ar_atualizado.id_file).filetext;
                    string ch_arquivo_superior = "SEPLAG/Decreto/2004";
                    string sArquivo = AnexarHtml(nm_arquivo, arquivo_text, ch_arquivo_superior);


                    //Imprimir no console
                    //Console.WriteLine(nm_arquivo);
                    //Console.WriteLine(arquivo_text);
                    //Console.WriteLine(ch_arquivo_superior);

                    //Montar objeto 
                    var arquivoOv = new SINJ_ArquivoOV();
                    arquivoOv.ch_arquivo_superior = ch_arquivo_superior;
                    arquivoOv.nm_arquivo = nm_arquivo;
                    arquivoOv.nr_tipo_arquivo = 1;
                    arquivoOv.ch_arquivo = ch_arquivo_superior + "/" + nm_arquivo;
                    arquivoOv.nr_nivel_arquivo = 3;
                    arquivoOv.nm_login_usuario_cadastro = "usrselag";
                    arquivoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                    arquivoOv.ds_arquivo = null;
                    arquivoOv.ar_arquivo = JSON.Deserializa<ArquivoOV>(sArquivo);



                    var id_doc = new SINJ_ArquivoRN().Incluir(arquivoOv);

                    //if (id_doc <= 0)
                    //{
                    //    throw new Exception("Erro ao salvar arquivo.");
                    //}

                    arquivoOv._metadata.id_doc = id_doc;



                    //Imprime o objeto
                    //Console.WriteLine("Arquivo OV: "+arquivoOv);

                    //Enviar para o sinj
                    //var sArquivo = "";
                    //sArquivo = new Arquivo.UploadHtml().AnexarHtml(context);


                    //NormaOV norma = arquivoOv;
                    //normaAD.Incluir(norma);



                    //var sArquivo = "";
                    //sArquivo = new Arquivo.UploadHtml().AnexarHtml(context);

                    //arquivoOv.ar_arquivo = JSON.Deserializa<ArquivoOV>(sArquivo);

                    //Incluir arquivo
                    //var id_doc = new SINJ_ArquivoRN().Incluir(arquivoOv);


                    //Console.WriteLine(document.doc(consulta.ar_atualizado.id_file).filename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Nuloooooooo: "+ ex.Message);
                }




            }
            //return "{\"id_doc\": \"" + id_doc + "\", \"success_message\":\"Arquivo salvo com sucesso.\", \"arquivo\": " 
            //+ JSON.Serialize<SINJ_ArquivoOV>(arquivoOv) + ", \"action\":\"INSERTED\"}";

        }


        public static string AnexarHtml(string nm_arquivo, string arquivo_text, string ch_arquivo_superior)
        {
            string sRetorno = "";
            string _arquivo_text = arquivo_text;
            _arquivo_text = HttpUtility.UrlDecode(_arquivo_text);
            string _filename = nm_arquivo;
            string _nm_base = "sinj_arquivo";

            //SessaoUsuarioOV sessao_usuario = null;

            try
            {
               //sessao_usuario = Util.ValidarSessao();
                sRetorno = new UtilArquivoHtml().AnexarHtml(_arquivo_text, _filename, _nm_base);
                //var log_arquivo = new LogUpload
                //{
                //    arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                //};
                //LogOperacao.gravar_operacao("HTML.INC", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (ParametroInvalidoException ex)
            {
                sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                   //context.Response.StatusCode = 500;
                }
                //var erro = new ErroRequest
                //{
                //    Pagina = context.Request.Path,
                //    RequestQueryString = context.Request.QueryString,
                //    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                //    StackTrace = ex.StackTrace
                //};
                //if (sessao_usuario != null)
                //{
                //    LogErro.gravar_erro("HTML.INC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                //}
            }
            return sRetorno;
        }





    }
}
//modelos de query
//query.literal = "$$={\"nm_orgao_cadastrador='SEPLAG' and nm_tipo_norma='Decreto' and dt_assinatura > '01/01/2004' and dt_assinatura < '31/12/2004' \"}";
//query.literal = "$$={}";
//document.download(re.ar_atualizado.id_file);
//string ch_arquivo_superior = "\"SEPLAG/Decreto/2044\"";
