function PreencherTipoDeEdicaoEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_edicao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_edicao != null && data.ch_tipo_edicao != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_tipo_edicao').val(data.nm_tipo_edicao);
                    $('#ds_tipo_edicao').val(data.ds_tipo_edicao);
                    $('#st_edicao').prop("checked", data.st_edicao);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_edicao').show();
            $('#div_tipo_de_edicao').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_edicao').hide();
            $('#div_tipo_de_edicao').show();
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

function DetalhesTipoDeEdicao() {
    var id_doc = GetParameterValue("id_doc");
    var ch_tipo_edicao = GetParameterValue("ch_tipo_edicao");
    if (id_doc != "" || ch_tipo_edicao != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_edicao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_edicao != null && data.ch_tipo_edicao != "") {
                    if (ValidarPermissao(_grupos.tdf_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Edicão" href="./EditarTipoDeEdicao.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.tdf_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Ediçao" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_tipo_edicao').text(data.nm_tipo_edicao);
                    $('#div_ds_tipo_edicao').text(data.ds_tipo_edicao);
                    $('#div_st_edicao').text(data.st_edicao ? "Sim" : "Não");
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_edicao').show();
            $('#div_tipo_de_edicao').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_edicao').hide();
            $('#div_tipo_de_edicao').show();
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