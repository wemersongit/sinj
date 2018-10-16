function PreencherSituacaoEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_situacao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_situacao != null && data.ch_situacao != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_situacao').val(data.nm_situacao);
                    $('#ds_situacao').val(data.ds_situacao);
                    $('#nr_peso_situacao').val(data.nr_peso_situacao);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_situacao').show();
            $('#div_situacao').hide();
        }
        var complete = function () {
            $('#div_loading_situacao').hide();
            $('#div_situacao').show();
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

function DetalhesSituacao() {
    var id_doc = GetParameterValue("id_doc");
    var ch_situacao = GetParameterValue("ch_situacao");
    if (id_doc != "" || ch_situacao != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_situacao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_situacao != null && data.ch_situacao != "") {
                    if (ValidarPermissao(_grupos.sit_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Situação" href="./EditarSituacao.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.sit_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Situação" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_situacao').text(data.nm_situacao);
                    $('#div_ds_situacao').text(data.ds_situacao);
                    $('#div_nr_peso_situacao').text(data.nr_peso_situacao);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_situacao').show();
            $('#div_situacao').hide();
        }
        var complete = function () {
            $('#div_loading_situacao').hide();
            $('#div_situacao').show();
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
