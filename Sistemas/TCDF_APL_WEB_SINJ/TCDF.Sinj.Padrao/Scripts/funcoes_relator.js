function PreencherRelatorEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_relator').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_relator != null && data.ch_relator != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_relator').val(data.nm_relator);
                    $('#ds_relator').val(data.ds_relator);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_relator').show();
            $('#div_relator').hide();
        }
        var complete = function () {
            $('#div_loading_relator').hide();
            $('#div_relator').show();
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

function DetalhesRelator() {
    var id_doc = GetParameterValue("id_doc");
    var ch_relator = GetParameterValue("ch_relator");
    if (id_doc != "" || ch_relator != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_relator').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_relator != null && data.ch_relator != "") {
                    if (ValidarPermissao(_grupos.rel_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar" href="./EditarRelator.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.rel_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_relator').text(data.nm_relator);
                    $('#div_ds_relator').text(data.ds_relator);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_relator').show();
            $('#div_relator').hide();
        }
        var complete = function () {
            $('#div_loading_relator').hide();
            $('#div_relator').show();
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
