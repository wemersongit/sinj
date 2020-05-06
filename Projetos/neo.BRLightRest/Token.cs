using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;

namespace neo.BRLightREST
{
    public class Token
    {
        private AcessoAD<TokenOV> _acessoAd;
        public Token()
        {
            _acessoAd = new AcessoAD<TokenOV>("token");
        }
        /// <summary>
        /// Cria um token no banco
        /// </summary>
        /// <param name="ch_aplicacao">Determina a aplicação que criou o token, fica a critério do programador, o ideal é distinguir os tokens por aplicação, assim uma aplicação não interfere no tempo de vida dos tokens de outra aplicação.</param>
        /// <param name="ch_origem">Determina a origem do token, fica a critério do programador, digamos que o token criado é para recriar senha de usuário, a chave poderia ser senha.</param>
        /// <returns>uma string que é o token, a chave do token.</returns>
        public string Criar(string ch_aplicacao, string ch_origem)
        {
            Params.CheckNotNullOrEmpty("ch_aplicacao", ch_aplicacao);
            Params.CheckNotNullOrEmpty("ch_origem", ch_origem);
            string token = Guid.NewGuid().ToString("N");
            if (_acessoAd.Incluir(new TokenOV { ch_aplicacao= ch_aplicacao, ch_origem= ch_origem, token=token }) > 0)
            {
                return token;
            }
            throw new Exception("Não foi possível criar o token.");
        }
        /// <summary>
        /// Recupera o token
        /// </summary>
        /// <param name="token">string do token</param>
        /// <returns>Objeto TokenOV</returns>
        public TokenOV Doc(string token)
        {
            var results = _acessoAd.Consultar(new Pesquisa { literal = "token='" + token + "'" });
            if (results.results.Count > 0)
            {
                return results.results[0];
            }
            return null;
        }

        public TokenOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        ///Comentei essa parte do código porque entendo que cada aplicação deve ser obrigada a informar ch_aplicacao e ch_origem.
        ///Caso o programador queira limpar tokens sem passar essas informações ele deve utilizar outros meios. Assim não permitimos que uma aplicação
        ///limpe os tokens de outra. Cada aplicação pode possuir critérios diferentes para determinar o tempo de vida de um token.
        //public int Delete(double miliseconds_valid)
        //{
        //    var pesquisa = new Pesquisa { limit = null, literal = "CAST(dt_doc AS abstime) < '" + DateTime.Now.AddMilliseconds(-miliseconds_valid).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "'" };
        //    return Delete(pesquisa);
        //}
        //public int Delete(string ch_aplicacao, double miliseconds_valid)
        //{
        //    var pesquisa = new Pesquisa { limit = null, literal = "ch_aplicacao='" + ch_aplicacao + "' and CAST(dt_doc AS abstime) < '" + DateTime.Now.AddMilliseconds(-miliseconds_valid).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "'" };
        //    return Delete(pesquisa);
        //}
        public int Delete(string ch_aplicacao, string ch_origem, double miliseconds_valid)
        {
            var pesquisa = new Pesquisa { limit = null, literal = "ch_aplicacao='" + ch_aplicacao + "' and ch_origem='"+ch_origem+"' and CAST(dt_doc AS abstime) < '" + DateTime.Now.AddMilliseconds(-miliseconds_valid).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "'" };
            return Delete(pesquisa);
        }
        public bool Delete(ulong id_doc)
        {
            return _acessoAd.Excluir(id_doc);
        }
        private int Delete(Pesquisa pesquisa)
        {
            int deletados = 0;
            var results = _acessoAd.Consultar(pesquisa);
            foreach (var doc in results.results)
            {
                if (_acessoAd.Excluir(doc._metadata.id_doc))
                {
                    deletados++;
                }
            }
            return deletados;
        }
        
    }
}
