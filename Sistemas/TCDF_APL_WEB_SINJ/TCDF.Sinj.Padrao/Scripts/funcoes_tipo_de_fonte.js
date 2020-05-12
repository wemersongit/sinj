function PreencherTipoDeFonteEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_fonte').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_fonte != null && data.ch_tipo_fonte != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_tipo_fonte').val(data.nm_tipo_fonte);
                    $('#ds_tipo_fonte').val(data.ds_tipo_fonte);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_fonte').show();
            $('#div_tipo_de_fonte').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_fonte').hide();
            $('#div_tipo_de_fonte').show();
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

function DetalhesTipoDeFonte() {
    var id_doc = GetParameterValue("id_doc");
    var ch_tipo_fonte = GetParameterValue("ch_tipo_fonte");
    if (id_doc != "" || ch_tipo_fonte != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_fonte').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_fonte != null && data.ch_tipo_fonte != "") {
                    if (ValidarPermissao(_grupos.tdf_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Fonte" href="./EditarTipoDeFonte.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.tdf_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Fonte" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_tipo_fonte').text(data.nm_tipo_fonte);
                    $('#div_ds_tipo_fonte').text(data.ds_tipo_fonte);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_fonte').show();
            $('#div_tipo_de_fonte').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_fonte').hide();
            $('#div_tipo_de_fonte').show();
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
