function PreencherInteressadoEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_interessado').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_interessado != null && data.ch_interessado != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_interessado').val(data.nm_interessado);
                    $('#ds_interessado').val(data.ds_interessado);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_interessado').show();
            $('#div_interessado').hide();
        }
        var complete = function () {
            $('#div_loading_interessado').hide();
            $('#div_interessado').show();
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

function DetalhesInteressado() {
    var id_doc = GetParameterValue("id_doc");
    var ch_interessado = GetParameterValue("ch_interessado");
    if (id_doc != "" || ch_interessado != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_interessado').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_interessado != null && data.ch_interessado != "") {
                    if (ValidarPermissao(_grupos.int_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Interessado" href="./EditarInteressado.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.int_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Interessado" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_interessado').text(data.nm_interessado);
                    $('#div_ds_interessado').text(data.ds_interessado);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_interessado').show();
            $('#div_interessado').hide();
        }
        var complete = function () {
            $('#div_loading_interessado').hide();
            $('#div_interessado').show();
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
