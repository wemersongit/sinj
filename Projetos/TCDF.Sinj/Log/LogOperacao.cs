using util.BRLight;
using TCDF.Sinj.Log.OV;
using System;
using TCDF.Sinj.Log.RN;
namespace TCDF.Sinj.Log
{
    public class LogOperacao
    {
        public static ulong gravar_operacao(string _ch_operacao, LogBuscar busca, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogBuscar> { ch_tipo_operacao = "pesquisar", operacao = busca };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogBuscar>>(operacao), 0, nm_user, nm_login_user);
        }
        public static ulong gravar_operacao(string _ch_operacao, LogRelatorio relatorio, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogRelatorio> { ch_tipo_operacao = "pesquisar", operacao = relatorio };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogRelatorio>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao(string _ch_operacao, LogVisualizar busca, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogVisualizar> { ch_tipo_operacao = "visualizar", operacao = busca };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogVisualizar>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao<T>(string _ch_operacao, LogIncluir<T> incluir, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogIncluir<T>> { ch_tipo_operacao = "incluir", operacao = incluir };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogIncluir<T>>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao(string _ch_operacao, LogUpload arquivo, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogUpload> { ch_tipo_operacao = "upload", operacao = arquivo };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogUpload>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao(string _ch_operacao, LogDownload arquivo, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogDownload> { ch_tipo_operacao = "download", operacao = arquivo };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogDownload>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao<T>(string _ch_operacao, LogAlterar<T> alterar, ulong _id_doc_origem, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogAlterar<T>> { ch_tipo_operacao = "editar", operacao = alterar };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogAlterar<T>>>(operacao), _id_doc_origem, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao<T>(string _ch_operacao, LogPutPath<T> putPath, ulong _id_doc_origem, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogPutPath<T>> { ch_tipo_operacao = "putpath", operacao = putPath };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogPutPath<T>>>(operacao), _id_doc_origem, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao(string _ch_operacao, LogExcluir excluir, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogExcluir> { ch_tipo_operacao = "excluir", operacao = excluir };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogExcluir>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao(string _ch_operacao, LogSair sair, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogSair> { ch_tipo_operacao = "sair", operacao = sair };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogSair>>(operacao), 0, nm_user, nm_login_user);
        }

        public static ulong gravar_operacao(string _ch_operacao, LogEmail email, string nm_user, string nm_login_user)
        {
            var operacao = new LogOperacaoOV<LogEmail> { ch_tipo_operacao = "email", operacao = email };
            return gravar_operacao(_ch_operacao, JSON.Serialize<LogOperacaoOV<LogEmail>>(operacao), 0, nm_user, nm_login_user);
        }



        private static ulong gravar_operacao(string _ch_operacao, string _ds_operacao_detalhes, ulong id_doc_origem, string nm_user, string nm_login_user)
        {
            ulong id_log = 0;
            try
            {
                var olog_operacaoOV = new log_operacaoOV();
                olog_operacaoOV.id_doc_origem = (id_doc_origem == 0) ? (ulong?)null : id_doc_origem;
                olog_operacaoOV.ch_operacao = _ch_operacao;
                olog_operacaoOV.ds_operacao_detalhes = _ds_operacao_detalhes;

                olog_operacaoOV.nr_ip_usuario = util.BRLight.Util.GetUserIp();

                olog_operacaoOV.nm_user_operacao = nm_user;
                olog_operacaoOV.nm_login_user_operacao = nm_login_user;

                olog_operacaoOV.dt_inicio = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                id_log = new log_operacaoRN().Incluir(olog_operacaoOV);
            }
            catch
            {
                // gerar log em txt 
                // ou no sistema de eventos do sistema operaciona 
                // ou disparar email
            }
            return id_log;
        }

        public static bool gravar_operacao_dt_fim(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_operacaoRN().AlterarPath_dt_fim(id_doc);
        }
    }
}
