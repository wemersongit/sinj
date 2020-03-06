using neo.BRLightREST;
using TCDF.Sinj.OV;
//using util.BRLight;
using System.Collections.Generic;
using System;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.ES;
using TCDF.Sinj;

namespace SinjMigracaoDecretos.AD
{
    public class NormaAD
    {
        private DocEs _docEs;
        private AcessoAD<NormaOV> _acessoAd;
        private string _nm_base;
      
        public NormaAD()
        {
            _nm_base = "sinj_norma";
            _acessoAd = new AcessoAD<NormaOV>(_nm_base);
            _docEs = new DocEs();
        }

        //Retorna uma consulta
        internal Results<NormaOV> Consultar(Pesquisa query)
        {
           return _acessoAd.Consultar(query);
        }



        internal ulong Incluir(NormaOV normaOv)
        {
            try
            {
                return _acessoAd.Incluir(normaOv);
            }
            catch (Exception ex)
            {
                throw new DocDuplicateKeyException("Erro ao incluir registro" + ex);
            }
        }




























        //internal NormaOV BuscarNorma(string id_norma, string[] select)
        //{
        //    NormaOV normaOv = null;
        //    string consulta = "$$={\"literal\":\"ch_norma='" + id_norma + "'\",\"limit\":\"1\"";
        //    if (select.Length > 0)
        //    {
        //        consulta += ",\"select\":" + JSON.Serialize(select);
        //    }
        //    consulta += "}";

        //    return normaOv;
        //}




        //internal NormaOV Doc(ulong id_doc)
        //{
        //    return _acessoAd.ConsultarReg(id_doc);
        //}

        //internal NormaOV Doc(string ch_norma)
        //{
        //    Pesquisa query = new Pesquisa();
        //    query.limit = "1";
        //    query.offset = "0";
        //    query.literal = string.Format("ch_norma='{0}'", ch_norma);
        //    var result = Consultar(query);
        //    if (result.result_count > 1)
        //    {
        //        throw new Exception("Foi verificado mais de um registro com a mesma chave.");
        //    }
        //    if (result.result_count > 0)
        //    {
        //        return result.results[0];
        //    }
        //    throw new DocNotFoundException("Nenhum registro foi encontrado, é possível que tenha sido excluído.");
        //}

        //internal string JsonReg(Pesquisa query)
        //{
        //    return _acessoAd.jsonReg(query);
        //}

        //internal string JsonReg(ulong id_doc)
        //{
        //    return _acessoAd.jsonReg(id_doc);
        //}

        //internal string PathPut(ulong id_doc, string path, string value, string retorno)
        //{
        //    return _acessoAd.pathPut(id_doc, path, value, retorno);
        //}

        //internal string PathPut<T>(Pesquisa pesquisa, List<opMode<T>> listopMode)
        //{
        //    return new AcessoAD<T>(_nm_base).OP(pesquisa, listopMode);
        //}

        ///// <summary>
        ///// Inclui uma norma e retorna o id_doc
        ///// </summary>
        ///// <param name="normaOv"></param>
        ///// <returns></returns>
        //internal ulong Incluir(NormaOV normaOv)
        //{
        //    try
        //    {
        //        return _acessoAd.Incluir(normaOv);
        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1) || (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1)))
        //        {
        //            throw new DocDuplicateKeyException("Registro duplicado!!!");
        //        }
        //        throw ex;
        //    }
        //}

        //internal bool Atualizar(ulong id_doc, NormaOV normaOv)
        //{
        //    try
        //    {
        //        return _acessoAd.Alterar(id_doc, normaOv);
        //    }
        //    catch (Exception ex)
        //    {
        //        if ((ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1) || (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1)))
        //        {
        //            throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
        //        }
        //        throw ex;
        //    }
        //}

        //internal bool Excluir(ulong id_doc)
        //{
        //    return _acessoAd.Excluir(id_doc);
        //}

        //#region Arquivo

        //public string GetDoc(string id_file)
        //{
        //    var doc = new Doc(_nm_base);
        //    return doc.pesquisarDoc(id_file);
        //}

        //public string AnexarArquivo(FileParameter fileParameter)
        //{
        //    string resultado;
        //    try
        //    {
        //        var doc = new Doc(_nm_base);
        //        var dicionario = new Dictionary<string, object>();
        //        dicionario.Add("file", fileParameter);
        //        resultado = doc.incluir(dicionario);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
        //    }
        //    return resultado;
        //}

        //public byte[] Download(string id_file)
        //{
        //    var doc = new Doc(_nm_base);
        //    return doc.download(id_file);
        //}

        //#endregion

        //#region ES

        //public Result<NormaOV> ConsultarEs(string query)
        //{
        //    var url_es = new DocEs().GetUrlEs(_nm_base) + "/_search";
        //    try
        //    {
        //        return _docEs.Pesquisar<NormaOV>(query, url_es);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao pesquisar normas. url_es: " + url_es + ". query: " + query, ex);
        //    }
        //}

        //public string PesquisarTotalEs(string query)
        //{
        //    var url_es = new DocEs().GetUrlEs(_nm_base) + "/_count";
        //    try
        //    {
        //        return _docEs.CountEs(query, url_es);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao pesquisar total de normas. url_es: " + url_es + ". query: " + query, ex);
        //    }
        //}

        //#endregion
    }
}
