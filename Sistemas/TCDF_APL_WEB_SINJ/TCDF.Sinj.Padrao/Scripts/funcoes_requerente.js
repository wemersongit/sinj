function PreencherRequerenteEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_requerente').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_requerente != null && data.ch_requerente != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_requerente').val(data.nm_requerente);
                    $('#ds_requerente').val(data.ds_requerente);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_requerente').show();
            $('#div_requerente').hide();
        }
        var complete = function () {
            $('#div_loading_requerente').hide();
            $('#div_requerente').show();
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

function DetalhesRequerente() {
    var id_doc = GetParameterValue("id_doc");
    var ch_requerente = GetParameterValue("ch_requerente");
    if (id_doc != "" || ch_requerente != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_requerente').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_requerente != null && data.ch_requerente != "") {
                    if (ValidarPermissao(_grupos.rqe_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar" href="./EditarRequerente.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.rqe_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_requerente').text(data.nm_requerente);
                    $('#div_ds_requerente').text(data.ds_requerente);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_requerente').show();
            $('#div_requerente').hide();
        }
        var complete = function () {
            $('#div_loading_requerente').hide();
            $('#div_requerente').show();
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
