using System;
using System.Collections.Generic;
using util.BRLight;

namespace neo.BRLightREST
{
    public class AcessoAD<T> {

        public static string Base { get; set; }
        public static bool DisplayErrors { get; set; }

        public AcessoAD(string nm_base)
        {
            Base = nm_base;
            DisplayErrors = true;
        }

        public AcessoAD(string nm_base, bool b_DisplayErrors)
        {
            Base = nm_base;
            DisplayErrors = b_DisplayErrors;
        }

        public UInt64 Incluir(T Ov)
        {
            var oReg = new Reg(Base);
            var serealiza = JSON.Serialize<T>(Ov);
            var resultado = oReg.incluir(new Dictionary<string, object> { { "value", serealiza } });
            if (DisplayErrors) {
                if (oReg.Erro.error_message != null) {
                    throw new Exception(oReg.Response);
                }
            }
            return resultado;
        }

        public Results<T> Consultar(Pesquisa opesquisa)
        {
            try
            {
                var oReg = new Reg(Base);
                string resultado = oReg.pesquisar(opesquisa);
                var results = JSON.Deserializa<Results<T>>(resultado);
                if (DisplayErrors) {
                    if (oReg.Erro.error_message != null) {
                        throw new Exception(oReg.Response);
                    }
                }
                return results;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS Consultar - Não foi possivel Consultar na Base: " + Base, ex);
            }
        }

        public T ConsultarReg(ulong id_doc)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = JSON.Deserializa<T>(oReg.pesquisarReg(id_doc));
                if (DisplayErrors) {
                    if (oReg.Erro.error_message != null) {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS ConsultarReg - Não foi possivel Consultar ID na Base: " + Base, ex);
            }
        }

        public bool Alterar(ulong id, T t)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.alterar(id, new Dictionary<string, object> { { "value", JSON.Serialize<T>(t) } });

                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS Alterar - Não foi possivel alterar na Base: " + Base, ex);
            }
        }


        public string OP(Pesquisa oPesquisa, List<opMode<T>> listopMode)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.OP(oPesquisa, new Dictionary<string, object> { { "path", JSON.Serialize<List<opMode<T>>>(listopMode) } });
                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS opMode - Não foi possivel opMode na Base: " + Base, ex);
            }
        }

        public bool Excluir(ulong id)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.excluir(id);

                if (DisplayErrors){
                    if (oReg.Erro.error_message != null) {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS Excluir - Não foi possivel excluir na Base: " + Base, ex);
            }
        }

        public string jsonReg(Pesquisa oPesquisa)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.pesquisar(oPesquisa);
                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS ObterjsonReg - Não foi possivel Consultar na Base: " + Base, ex);
            }
           
        }

        public string jsonReg(ulong id_doc)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.pesquisarReg(id_doc);
                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS Reg - Não foi possivel Consultar ID na Base: " + Base, ex);
            }
        }

        [Obsolete("Reg is deprecated, please use jsonReg instead.")]
        public string Reg(ulong id_doc)
        {
            return jsonReg(id_doc);
        }

        [Obsolete("ObterjsonReg is deprecated, please use jsonReg instead.")]
        public string ObterjsonReg(Pesquisa oPesquisa)
        {
            return jsonReg(oPesquisa);
        }

        public string pathGet(ulong id_doc, string path)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.pathGet(id_doc, path);

                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS pathGet - Não foi possivel pesquisar na Base: " + Base, ex);
            }
          
        }

        public string pathPost(ulong id_doc, string path, string value, string retorno)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.pathPost(id_doc, path, value, retorno);

                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS pathPost - Não foi possivel alterar na Base: " + Base, ex);
            }
        }

        public string pathPut(ulong id_doc, string path, string value, string retorno)
        {
            try
            {
                var oReg = new Reg(Base);
                var resultado = oReg.pathPut(id_doc, path, value, retorno);

                if (DisplayErrors)
                {
                    if (oReg.Erro.error_message != null)
                    {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("AcessoDS pathPut - Não foi possivel alterar na Base: " + Base, ex);
            }
        }

        public string pathDelete(ulong id_doc, string path, string retorno)
        {
            try {
                var oReg = new Reg(Base);
                var resultado = oReg.pathDelete(id_doc, path, retorno);

                if (DisplayErrors) {
                    if (oReg.Erro.error_message != null) {
                        throw new Exception(oReg.Response);
                    }
                }
                return resultado;
            }
            catch (Exception ex) {
                throw new FalhaOperacaoException("AcessoDS pathDelete - Não foi possivel alterar na Base: " + Base, ex);
            }
        }

    }
}