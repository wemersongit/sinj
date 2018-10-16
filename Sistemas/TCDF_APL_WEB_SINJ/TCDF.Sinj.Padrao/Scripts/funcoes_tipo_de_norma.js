function PreencherTipoDeNormaEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_norma').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_norma != null && data.ch_tipo_norma != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_tipo_norma').val(data.nm_tipo_norma);
                    $('#ds_tipo_norma').val(data.ds_tipo_norma);
                    for (var i = 0; i < data.sgs_tipo_norma.length; i++) {
                        $('#div_siglas').preppend('<div><input name="sgs_tipo_norma" type="text" value="' + data.sgs_tipo_norma[i] + '" class="w-40-pc" /><button title="Remover sigla" class="link" onclick="delSigla(this)"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></button><div>');
                    }
                    for (var i = 0; i < data.orgaos_cadastradores.length; i++) {
                        $('#div_orgaos_cadastradores input[value="' + data.orgaos_cadastradores[i].id_orgao_cadastrador + '"]').prop("checked", true);
                    }
                    if (data.in_g1) {
                        $('#in_g1').prop("checked", true);
                    }
                    if (data.in_g2) {
                        $('#in_g2').prop("checked", true);
                    }
                    if (data.in_g3) {
                        $('#in_g3').prop("checked", true);
                    }
                    if (data.in_g4) {
                        $('#in_g4').prop("checked", true);
                    }
                    if (data.in_g5) {
                        $('#in_g5').prop("checked", true);
                    }
                    if (data.in_conjunta) {
                        $('#in_conjunta').prop("checked", true);
                    }
                    if (data.in_questionavel) {
                        $('#in_questionavel').prop("checked", true);
                    }
                    if (data.in_numeracao_por_orgao) {
                        $('#in_numeracao_por_orgao').prop("checked", true);
                    }
                    if (data.in_apelidavel) {
                        $('#in_apelidavel').prop("checked", true);
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_norma').show();
            $('#div_tipo_de_norma').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_norma').hide();
            $('#div_tipo_de_norma').show();
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

function DetalhesTipoDeNorma() {
    var id_doc = GetParameterValue("id_doc");
    var ch_tipo_norma = GetParameterValue("ch_tipo_norma");
    if (id_doc != "" || ch_tipo_norma != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_tipo_de_norma').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_norma != null && data.ch_tipo_norma != "") {
                    if (ValidarPermissao(_grupos.tdn_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Fonte" href="./EditarTipoDeNorma.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.tdn_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Fonte" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_tipo_norma').text(data.nm_tipo_norma);
                    $('#div_ds_tipo_norma').text(data.ds_tipo_norma);
                    $('#div_orgaos_cadastradores').text(data.get_orgaos_cadastradores);
                    for (var i = 0; i < data.sgs_tipo_norma.length; i++) {
                        $('#div_siglas').append(data.sgs_tipo_norma[i] + '<br/>');
                    }
                    $('#div_grupos').text(data.get_grupos);
                    if (data.in_g1) {
                        $('#div_in_g1').text("sim");
                    }
                    else {
                        $('#div_in_g1').text("não");
                    }
                    if (data.in_g2) {
                        $('#div_in_g2').text("sim");
                    }
                    else {
                        $('#div_in_g2').text("não");
                    }
                    if (data.in_g3) {
                        $('#div_in_g3').text("sim");
                    }
                    else {
                        $('#div_in_g3').text("não");
                    }
                    if (data.in_g4) {
                        $('#div_in_g4').text("sim");
                    }
                    else {
                        $('#div_in_g4').text("não");
                    }
                    if (data.in_g5) {
                        $('#div_in_g5').text("sim");
                    }
                    else {
                        $('#div_in_g5').text("não");
                    }
                    if (data.in_conjunta) {
                        $('#div_in_conjunta').text("sim");
                    }
                    else {
                        $('#div_in_conjunta').text("não");
                    }
                    if (data.in_questionavel) {
                        $('#div_in_questionavel').text("sim");
                    }
                    else {
                        $('#div_in_questionavel').text("não");
                    }
                    if (data.in_numeracao_por_orgao) {
                        $('#div_in_numeracao_por_orgao').text("sim");
                    }
                    else {
                        $('#div_in_numeracao_por_orgao').text("não");
                    }
                    if (data.in_apelidavel) {
                        $('#div_in_apelidavel').text("sim");
                    }
                    else {
                        $('#div_in_apelidavel').text("não");
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_loading_tipo_de_norma').show();
            $('#div_tipo_de_norma').hide();
        }
        var complete = function () {
            $('#div_loading_tipo_de_norma').hide();
            $('#div_tipo_de_norma').show();
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


function delSigla(el) {
    $(el).closest('div').remove();
}
function addSigla() {
    //
    $('#div_siglas>button[add]').remove();
    $('#div_siglas').append('<div><input name="sgs_tipo_norma" type="text" value="" class="w-40-pc" /><button title="Remover sigla" class="link" onclick="delSigla(this)"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></button><div>');
    $('#div_siglas').append('<button add title="Adicionar uma sigla" class="link" onclick="addSigla()"><img src="' + _urlPadrao + '/Imagens/ico_add_p.png" /></button>');
}
