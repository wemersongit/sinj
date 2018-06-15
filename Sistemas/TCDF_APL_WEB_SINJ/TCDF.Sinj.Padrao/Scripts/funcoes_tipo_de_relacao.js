function PreencherTipoDeRelacaoEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_relacao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_relacao != null && data.ch_tipo_relacao != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_tipo_relacao').val(data.nm_tipo_relacao);
                    $('#ds_tipo_relacao').val(data.ds_tipo_relacao);
                    $('#ds_texto_para_alterador').val(data.ds_texto_para_alterador);
                    $('#ds_texto_para_alterado').val(data.ds_texto_para_alterado);
                    $('#nr_importancia').val(data.nr_importancia);
                    $('#in_relacao_de_acao').prop("checked", data.in_relacao_de_acao);
                    $('#in_selecionavel').prop("checked", data.in_selecionavel);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_relacao').show();
            $('#div_tipo_de_relacao').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_relacao').hide();
            $('#div_tipo_de_relacao').show();
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

function DetalhesTipoDeRelacao() {
    var id_doc = GetParameterValue("id_doc");
    var ch_tipo_relacao = GetParameterValue("ch_tipo_relacao");
    if (id_doc != "" || ch_tipo_relacao != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_relacao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_relacao != null && data.ch_tipo_relacao != "") {
                    if (ValidarPermissao(_grupos.tdr_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Tipo de Relação" href="./EditarTipoDeRelacao.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.tdr_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Tipo de Relação" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_tipo_relacao').text(data.nm_tipo_relacao);
                    $('#div_ds_tipo_relacao').text(data.ds_tipo_relacao);
                    $('#div_ds_texto_para_alterador').text(data.ds_texto_para_alterador);
                    $('#div_ds_texto_para_alterado').text(data.ds_texto_para_alterado);
                    $('#div_nr_importancia').text(data.nr_importancia);
                    $('#div_in_relacao_de_acao').text(data.in_relacao_de_acao ? "sim" : "não");
                    $('#div_in_selecionavel').text(data.in_selecionavel ? "sim" : "não");
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_relacao').show();
            $('#div_tipo_de_relacao').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_relacao').hide();
            $('#div_tipo_de_relacao').show();
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