
function PreencherAutoriaEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_autoria').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_autoria != null && data.ch_autoria != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_autoria').val(data.nm_autoria);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_autoria').show();
            $('#div_autoria').hide();
        }
        var complete = function () {
            $('#div_loading_autoria').hide();
            $('#div_autoria').show();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("VIS") + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

function DetalhesAutoria() {
    var id_doc = GetParameterValue("id_doc");
    var ch_autoria = GetParameterValue("ch_autoria");
    if (id_doc != "" || ch_autoria != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_autoria').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_autoria != null && data.ch_autoria != "") {
                    if (ValidarPermissao(_grupos.aut_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Autoria" href="./EditarAutoria.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.aut_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Autoria" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_autoria').text(data.nm_autoria);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_autoria').show();
            $('#div_autoria').hide();
        }
        var complete = function () {
            $('#div_loading_autoria').hide();
            $('#div_autoria').show();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("VIS") + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}
