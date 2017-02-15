
function PreencherTipoDeDiarioEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_diario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_diario != null && data.ch_tipo_diario != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#sg_tipo_diario').val(data.sg_tipo_diario);
                    $('#nm_tipo_diario').val(data.nm_tipo_diario);
                    $('#ds_tipo_diario').val(data.ds_tipo_diario);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_diario').show();
            $('#div_tipo_de_diario').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_diario').hide();
            $('#div_tipo_de_diario').show();
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

function DetalhesTipoDeDiario() {
    var id_doc = GetParameterValue("id_doc");
    var ch_tipo_diario = GetParameterValue("ch_tipo_diario");
    if (id_doc != "" || ch_tipo_diario != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_diario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_diario != null && data.ch_tipo_diario != "") {
                    if (ValidarPermissao(_grupos.tdd_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Fonte" href="./EditarTipoDeDiario.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.tdd_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Fonte" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_sg_tipo_diario').text(data.sg_tipo_diario);
                    $('#div_nm_tipo_diario').text(data.nm_tipo_diario);
                    $('#div_ds_tipo_diario').text(data.ds_tipo_diario);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_diario').show();
            $('#div_tipo_de_diario').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_diario').hide();
            $('#div_tipo_de_diario').show();
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