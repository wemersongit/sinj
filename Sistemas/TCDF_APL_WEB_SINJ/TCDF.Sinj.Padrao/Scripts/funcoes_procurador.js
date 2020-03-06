function PreencherProcuradorEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_procurador').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_procurador != null && data.ch_procurador != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_procurador').val(data.nm_procurador);
                    $('#ds_procurador').val(data.ds_procurador);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_procurador').show();
            $('#div_procurador').hide();
        }
        var complete = function () {
            $('#div_loading_procurador').hide();
            $('#div_procurador').show();
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

function DetalhesProcurador() {
    var id_doc = GetParameterValue("id_doc");
    var ch_procurador = GetParameterValue("ch_procurador");
    if (id_doc != "" || ch_procurador != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_procurador').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_procurador != null && data.ch_procurador != "") {
                    if (ValidarPermissao(_grupos.pro_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar" href="./EditarProcurador.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.pro_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_procurador').text(data.nm_procurador);
                    $('#div_ds_procurador').text(data.ds_procurador);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_procurador').show();
            $('#div_procurador').hide();
        }
        var complete = function () {
            $('#div_loading_procurador').hide();
            $('#div_procurador').show();
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
