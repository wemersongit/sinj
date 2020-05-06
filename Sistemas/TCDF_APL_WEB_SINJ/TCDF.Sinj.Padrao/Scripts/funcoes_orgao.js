
// Dependências: liblight.js, validation.js, jquery.modalLight.js, jquery.ajaxLight.js
//--------------------------------------------------------------------

var _ch_orgao = "";

function PesquisarOrgao() {
    $('#div_notificacao_orgao').html('');
    try {
        ValidarPesquisa(form_pesquisa_orgao);
        return true;
    } catch (ex) {
        $("#div_notificacao_orgao").messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
        return false;
    }
    return false;
}

function DetalhesOrgao() {
    var id_doc = GetParameterValue("id_doc");
    var ch_orgao = GetParameterValue("ch_orgao");
    if (id_doc != "" || ch_orgao != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                _ch_orgao = data.ch_orgao;
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_orgao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_orgao != null && data.ch_orgao != "") {
                    if (ValidarPermissao(_grupos.org_edt)) {
                        $('#div_controls_detalhes').append(
                            '<a title="Editar Órgão" href="./EditarOrgao.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"/></a> &nbsp;'
                        );
                    }
                    if (ValidarPermissao(_grupos.org_exc)) {
                        $('#div_controls_detalhes').append(
                            '<a title="Excluir Órgão" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="' + _urlPadrao + '/Imagens/ico_trash_p.png"/></a>'
                        );
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
                    DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#a_selecionar_normas').attr("onclick", "javascript:ExpandirTabelaDeNormas('" + data.ch_orgao + "')");
                    $('#button_atualizar_normas').attr("onclick", "javascript:CriarModalConfirmacaoAtualizarOrigemDasNormas('" + data.ch_orgao + "', '" + data.sg_orgao + "', '" + data.nm_orgao + "')");
                    $('#div_nm_orgao').text(data.nm_orgao);
                    $('#div_sg_orgao').text(data.sg_orgao);
                    $('#div_ambito').text(data.ambito.nm_ambito);
                    $('#div_orgaos_cadastradores').text(data.get_orgaos_cadastradores);
                    $('#div_st_orgao').text(data.get_st_orgao);
                    $('#div_dt_inicio_vigencia').text(IsNotNullOrEmpty(data.dt_inicio_vigencia) ? data.dt_inicio_vigencia : "");
                    $('#div_norma_inicio_vigencia').html(IsNotNullOrEmpty(data, 'ch_norma_inicio_vigencia') ? "<a href='./DetalhesDeNorma.aspx?id_norma=" + data.ch_norma_inicio_vigencia + "'> " + data.ds_norma_inicio_vigencia + " </a>" : "");
                    if (!data.st_orgao) { // Se o órgão for inativo, preenche a linha. Se não, esconde.
                        $('#div_dt_fim_vigencia').text(IsNotNullOrEmpty(data.dt_fim_vigencia) ? data.dt_fim_vigencia : "");
                    }
                    else {
                        $('#line_dt_fim_vigencia').hide();
                    }
                    $('#div_norma_fim_vigencia').html(IsNotNullOrEmpty(data, 'ch_norma_fim_vigencia') ? "<a href='./DetalhesDeNorma.aspx?id_norma=" + data.ch_norma_fim_vigencia + "'> " + data.ds_norma_fim_vigencia + " </a>" : "");
                    if (data.get_orgao_superior != null) {
                        $('#tbody_orgao_superior').html(
                            '<tr>' +
                                '<td>' + data.get_orgao_superior.nm_orgao + '</td>' +
                                '<td>' + data.get_orgao_superior.sg_orgao + '</td>' +
                                '<td>' + data.get_orgao_superior.sg_hierarquia + '</td>' +
                                '<td>' + data.get_orgao_superior.ambito.nm_ambito + '</td>' +
                                '<td>' + GetText(data.get_orgao_superior.dt_inicio_vigencia) + '</td>' +
                                '<td>' + (data.get_orgao_superior.dt_fim_vigencia != null ? data.get_orgao_superior.dt_fim_vigencia : '') + '</td>' +
                                '<td>' + data.get_orgao_superior.get_orgaos_cadastradores + '</td>' +
                                '<td class="control">' +
                                    '<a href="./DetalhesDeOrgao.aspx?id_doc=' + data.get_orgao_superior._metadata.id_doc + '"><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>' +
                                '</td>' +
                            '</tr>');
                    }
                    if (data.get_orgaos_inferiores != null && data.get_orgaos_inferiores.length > 0) {
                        $('#tbody_orgaos_inferiores').html('');
                        for (var i = 0; i < data.get_orgaos_inferiores.length; i++) {
                            $('#tbody_orgaos_inferiores').append(
                                '<tr>' +
                                    '<td>' + data.get_orgaos_inferiores[i].nm_orgao + '</td>' +
                                    '<td>' + data.get_orgaos_inferiores[i].sg_orgao + '</td>' +
                                    '<td>' + data.get_orgaos_inferiores[i].sg_hierarquia + '</td>' +
                                    '<td>' + data.get_orgaos_inferiores[i].ambito.nm_ambito + '</td>' +
                                    '<td>' + GetText(data.get_orgaos_inferiores[i].dt_inicio_vigencia) + '</td>' +
                                    '<td>' + (data.get_orgaos_inferiores[i].dt_fim_vigencia != null ? data.get_orgaos_inferiores[i].dt_fim_vigencia : '') + '</td>' +
                                    '<td>' + data.get_orgaos_inferiores[i].get_orgaos_cadastradores + '</td>' +
                                    '<td class="control">' +
                                        '<a href="./DetalhesDeOrgao.aspx?id_doc=' + data.get_orgaos_inferiores[i]._metadata.id_doc + '"><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>' +
                                    '</td>' +
                                '</tr>');
                        }
                    }
                    if (data.get_orgaos_anteriores != null && data.get_orgaos_anteriores.length > 0) {
                        $('#tbody_orgaos_anteriores').html('');
                        for (var i = 0; i < data.get_orgaos_anteriores.length; i++) {
                            $('#tbody_orgaos_anteriores').append(
                                '<tr>' +
                                    '<td>' + data.get_orgaos_anteriores[i].nm_orgao + '</td>' +
                                    '<td>' + data.get_orgaos_anteriores[i].sg_orgao + '</td>' +
                                    '<td>' + data.get_orgaos_anteriores[i].sg_hierarquia + '</td>' +
                                    '<td>' + data.get_orgaos_anteriores[i].ambito.nm_ambito + '</td>' +
                                    '<td>' + GetText(data.get_orgaos_anteriores[i].dt_inicio_vigencia) + '</td>' +
                                    '<td>' + (data.get_orgaos_anteriores[i].dt_fim_vigencia != null ? data.get_orgaos_anteriores[i].dt_fim_vigencia : '') + '</td>' +
                                    '<td>' + data.get_orgaos_anteriores[i].get_orgaos_cadastradores + '</td>' +
                                    '<td class="control">' +
                                        '<a href="./DetalhesDeOrgao.aspx?id_doc=' + data.get_orgaos_anteriores[i]._metadata.id_doc + '"><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>' +
                                    '</td>' +
                                '</tr>');
                        }
                    }
                    if (data.get_orgaos_posteriores != null && data.get_orgaos_posteriores.length > 0) {
                        $('#tbody_orgaos_posteriores').html('');
                        for (var i = 0; i < data.get_orgaos_posteriores.length; i++) {
                            $('#tbody_orgaos_posteriores').append(
                                '<tr>' +
                                    '<td>' + data.get_orgaos_posteriores[i].nm_orgao + '</td>' +
                                    '<td>' + data.get_orgaos_posteriores[i].sg_orgao + '</td>' +
                                    '<td>' + data.get_orgaos_posteriores[i].sg_hierarquia + '</td>' +
                                    '<td>' + data.get_orgaos_posteriores[i].ambito.nm_ambito + '</td>' +
                                    '<td>' + GetText(data.get_orgaos_posteriores[i].dt_inicio_vigencia) + '</td>' +
                                    '<td>' + (data.get_orgaos_posteriores[i].dt_fim_vigencia != null ? data.get_orgaos_posteriores[i].dt_fim_vigencia : '') + '</td>' +
                                    '<td>' + data.get_orgaos_posteriores[i].get_orgaos_cadastradores + '</td>' +
                                    '<td class="control">' +
                                        '<a href="./DetalhesDeOrgao.aspx?id_doc=' + data.get_orgaos_posteriores[i]._metadata.id_doc + '"><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>' +
                                    '</td>' +
                                '</tr>');
                        }
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_orgao_loading').show();
            $('#div_orgao').hide();
        }
        var complete = function () {
            $('#div_orgao_loading').hide();
            $('#div_orgao').show();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax('VIS') + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

function PreencherOrgaoEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_orgao_notificacao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_orgao != null && data.ch_orgao != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_orgao').val(data.nm_orgao);
                    $('#sg_orgao').val(data.sg_orgao);
                    $('#id_ambito option[value="' + data.ambito.id_ambito + '"]').attr('selected', 'selected');
                    for (var i = 0; i < data.orgaos_cadastradores.length; i++) {
                        $('#div_orgaos_cadastradores input[value="' + data.orgaos_cadastradores[i].id_orgao_cadastrador + '"]').prop("checked", true);
                    }
                    $('#div_st_orgao input[value="' + data.st_orgao + '"]').prop("checked", true);
                    if (!$('#div_st_orgao input[value="true"]').prop("checked")) {
                        SelecionarInativo();
                    }
                    else {
                        SelecionarAtivo();
                    }
                    $("#label_inativar_filhos").attr("ch_orgao", data.ch_orgao);
                    $('#dt_inicio_vigencia').val(IsNotNullOrEmpty(data.dt_inicio_vigencia) ? data.dt_inicio_vigencia : "");
                    $('#dt_fim_vigencia').val(IsNotNullOrEmpty(data.dt_fim_vigencia) ? data.dt_fim_vigencia : "");
                    if (data.get_orgao_superior != null) {
                        $('#tbody_hierarquia').html(
                            '<tr>' +
                                '<td>' + data.get_orgao_superior.nm_orgao + '<input type="hidden" id="hidden_ch_orgao_pai" name="ch_orgao_pai" value="' + data.get_orgao_superior.ch_orgao + '" /></td>' +
                                '<td>' + data.get_orgao_superior.sg_orgao + '</td>' +
                                '<td>' + data.get_orgao_superior.sg_hierarquia + '</td>' +
                                '<td>' + data.get_orgao_superior.ambito.nm_ambito + '</td>' +
                                '<td>' + GetText(data.get_orgao_superior.dt_inicio_vigencia) + '</td>' +
                                '<td>' + (data.get_orgao_superior.dt_fim_vigencia != null ? data.get_orgao_superior.dt_fim_vigencia : '') + '</td>' +
                                '<td>' + data.get_orgao_superior.get_orgaos_cadastradores + '</td>' +
                                '<td class="control">' +
                                    '<a href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>');
                    }
                    if (data.get_orgaos_anteriores != null) {
                        for (var i = 0; i < data.get_orgaos_anteriores.length; i++) {
                            $('#tbody_cronologia .tr_vazia').remove();
                            $('#tbody_cronologia').append(
	                            '<tr ch_orgao="' + data.get_orgaos_anteriores[i].ch_orgao + '">' +
	                                '<td>' + data.get_orgaos_anteriores[i].nm_orgao + '<input type="hidden" name="orgao_anterior" value="' + data.get_orgaos_anteriores[i].ch_orgao + '" /></td>' +
	                                '<td>' + data.get_orgaos_anteriores[i].sg_orgao + '</td>' +
	                                '<td sg_hierarquia>' + data.get_orgaos_anteriores[i].sg_hierarquia + '</td>' +
	                                '<td>' + data.get_orgaos_anteriores[i].ambito.nm_ambito + '</td>' +
	                                '<td>' + GetText(data.get_orgaos_anteriores[i].dt_inicio_vigencia) + '</td>' +
	                                '<td>' + (data.get_orgaos_anteriores[i].dt_fim_vigencia != null ? data.get_orgaos_anteriores[i].dt_fim_vigencia : '') + '</td>' +
	                                '<td>' + data.get_orgaos_anteriores[i].get_orgaos_cadastradores + '</td>' +
	                                '<td class="control">' +
	                                    '<a href="javascript:void(0);" onclick="javascript:DeletarLinha(event); javascript:DeletarFilhos(\'' + data.get_orgaos_anteriores[i].ch_orgao + '\')"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
	                                '</td>' +
	                            '</tr>');
                        }
                    }
                    if (IsNotNullOrEmpty(data, 'ch_norma_inicio_vigencia')) {
                        SelecionarNormaAssociada(data.ch_norma_inicio_vigencia, data.ds_norma_inicio_vigencia, 'inicio');
                    }
                    if (IsNotNullOrEmpty(data, 'ch_norma_fim_vigencia')) {
                        SelecionarNormaAssociada(data.ch_norma_fim_vigencia, data.ds_norma_fim_vigencia, 'fim');
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_loading_orgao').show();
            $('#div_orgao').hide();
        }
        var complete = function () {
            $('#div_loading_orgao').hide();
            $('#div_orgao').show();
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

function CriarModalHierarquia() {
    $('#modal_hierarquia .dados_orgao').hide();
    $("#modal_hierarquia").modallight({
        sTitle: "Adicionar Órgão Pai",
        sWidth: '600',
        oButtons: [{
            text: "Salvar",
            click: function () {
                var ch_orgao = $('#ch_orgao_pai_modal').val();
                var sg_ds_orgao = $('#sg_nm_orgao_pai_modal').val();
                if (IsNotNullOrEmpty(ch_orgao) && IsNotNullOrEmpty(sg_ds_orgao)) {
                    $('#tbody_hierarquia').html(
                                '<tr>' +
                                    '<td>' + $('#label_nm_orgao_pai_modal').text().trim() + '<input type="hidden" id="hidden_ch_orgao_pai" name="ch_orgao_pai" value="' + ch_orgao + '" /></td>' +
                                    '<td>' + $('#label_sg_orgao_pai_modal').text().trim() + '</td>' +
                                    '<td>' + $('#label_sg_hierarquia_orgao_pai_modal').text().trim() + '</td>' +
                                    '<td>' + $('#label_in_ambito_orgao_pai_modal').text().trim() + '</td>' +
                                    '<td>' + $('#label_dt_inicio_vigencia_orgao_pai_modal').text().trim() + '</td>' +
                                    '<td>' + $('#label_dt_fim_vigencia_orgao_pai_modal').text().trim() + '</td>' +
                                    '<td>' + $('#label_in_orgao_cadastrador_orgao_pai_modal').text().trim() + '</td>' +
                                    '<td class="control">' +
                                        '<a href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                    '</td>' +
                                '</tr>');
                    LimparModal('modal_hierarquia');
                    $(this).dialog('close');
                }
            }
        }, {
            text: "Cancelar",
            click: function () {
                $(this).dialog('close');
            }
        }],
        fnClose: function () {
            LimparModal('modal_hierarquia');
        }
    });
}

function CriarModalCronologia() {
    $('#modal_cronologia .notify').hide();
    $('#modal_cronologia .dados_orgao').hide();
    $("#modal_cronologia").modallight({
        sTitle: "Adicionar Órgão Anterior",
        sWidth: '600',
        oButtons: [{
            text: "Salvar",
            click: function () {
                var ch_orgao = $('#ch_orgao_anterior_modal').val();
                var nm_orgao = $('#label_nm_orgao_anterior_modal').text();
                var sg_nm_orgao = $('#sg_nm_orgao_anterior_modal').val();
                var dt_inicio_vigencia = $('#label_dt_inicio_vigencia_orgao_anterior_modal').text().trim();
                var dt_fim_vigencia = $('#label_dt_fim_vigencia_orgao_anterior_modal').text().trim();
                var dt_inicio_vigencia_editar = "";
                var dt_fim_vigencia_editar = "";
                if (!IsNotNullOrEmpty(dt_inicio_vigencia)) {
                    dt_inicio_vigencia = $('#input_dt_inicio_vigencia_orgao_anterior_modal').val();
                    if (!IsNotNullOrEmpty(dt_inicio_vigencia)) {
                        $('#modal_cronologia .notify').messagelight({
                            sContent: "O Órgão precisa ter data de início de vigência.",
                            sType: "error"
                        });
                        return;
                    }
                    dt_inicio_vigencia_editar = dt_inicio_vigencia;
                }
                if (!IsNotNullOrEmpty(dt_fim_vigencia)) {
                    dt_fim_vigencia = $('#input_dt_fim_vigencia_orgao_anterior_modal').val();
                    if (!IsNotNullOrEmpty(dt_fim_vigencia)) {
                        $('#modal_cronologia .notify').messagelight({
                            sContent: "O Órgão precisa ter data de fim de vigência.",
                            sType: "error"
                        });
                        return;
                    }
                    dt_fim_vigencia_editar = dt_fim_vigencia;
                }
                if (IsNotNullOrEmpty(ch_orgao) && IsNotNullOrEmpty(sg_nm_orgao)) {
                    var hiddens_orgaos_anteriores = $('#table_cronologia').find('input[name="orgao_anterior"]');
                    var i = hiddens_orgaos_anteriores.length;
                    while (i--) {
                        if ($(hiddens_orgaos_anteriores[i]).val().split('#')[0] == ch_orgao) {
                            $('#modal_cronologia .notify').messagelight({
                                sContent: "O Órgão já existe na " + (i + 1) + "ª linha da tabela.",
                                sType: "error"
                            });
                            return;
                        }
                    }
                    $('#tbody_cronologia .tr_vazia').remove();
                    $('#tbody_cronologia').append(
                                '<tr onmouseover="DestacarLinha(event)" onmouseout="RemoverDestaqueLinha()" ch_orgao="'+ch_orgao+'">' +
                                    '<td>' + $('#label_nm_orgao_anterior_modal').text() + '<input type="hidden" name="orgao_anterior" value="' + ch_orgao + '#' + dt_inicio_vigencia_editar + '#' + dt_fim_vigencia_editar + '" /></td>' +
                                    '<td>' + $('#label_sg_orgao_anterior_modal').text() + '</td>' +
                                    '<td sg_hierarquia>' + $('#label_sg_hierarquia_orgao_anterior_modal').text() + '</td>' +
                                    '<td>' + $('#label_in_ambito_orgao_anterior_modal').text() + '</td>' +
                                    '<td>' + dt_inicio_vigencia + '</td>' +
                                    '<td >' + dt_fim_vigencia + '</td>' + 
                                    '<td>' + $('#label_in_orgao_cadastrador_orgao_anterior_modal').text() + '</td>' +
                                    '<td class="control">' +
                                        '<a href="javascript:void(0);" onclick="javascript:DeletarLinha(event); javascript:DeletarFilhos(\'' + ch_orgao + '\')"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                        '<a href="javascript:void(0);" onclick="javascript:CriarModalFilhos(\''+ch_orgao+'\',\''+nm_orgao+'\',\''+dt_fim_vigencia+'\');"><img valign="absmiddle" title="Selecionar Órgãos Filho" alt="Selecionar Órgãos Filho" src="'+_urlPadrao+'/Imagens/ico_arrow_down.png"  /></a>' +
                                    '</td>' +
                                '</tr>');
                    $(this).dialog('close');
                }
                
            }
        }, {
            text: "Cancelar",
            click: function () {
                $(this).dialog('close');
            }
        }],
        fnClose: function () {
            LimparModal('modal_cronologia');
        }
    });
}

function CriarModalFilhos(ch_orgao, nm_orgao, dt_fim_vigencia) {
    	$('<div id="modal_filhos"/>').modallight({
        sTitle: "Selecionar Órgãos Filho: "+nm_orgao,
        sContent: '<div id="div_orgaos_filho" class="table w-100-pc"></div>',
        sWidth: '600',
        fnCreated: function(){
        	var sucesso = function(data){
            	if (!IsNotNullOrEmpty(data.results)) {
	            	$('#modal_filhos').append(
	            		'Esse órgão não tem nenhum filho associado'
	            	);
            	}
            	else{
            		$('#modal_filhos').append(
            			'<ul id="ul_orgao"> </ul>'
            		);
	        		for (var orgao in data.results){
	        			var orgaos_cadastradores = "";
	        			for (i in data.results[orgao].orgaos_cadastradores){
	        				orgaos_cadastradores += (orgaos_cadastradores == "" ? "" : ", ") + data.results[orgao].orgaos_cadastradores[i].nm_orgao_cadastrador;
	        			}    
		            	$('#ul_orgao').append(
		            		'<li id="li_orgao_filho'+orgao+'"> <input type="checkbox" class="checkbox_orgao_filho" id="checkbox_orgao'+orgao+'"' +
		            		'ch_hierarquia="'+data.results[orgao].ch_hierarquia+'" ch_orgao="'+data.results[orgao].ch_orgao+'"' +
		            		'ch_orgao_pai="'+data.results[orgao].ch_orgao_pai+'" nm_orgao="'+data.results[orgao].nm_orgao+'" sg_orgao="'+data.results[orgao].sg_orgao+'"' +
		            		'sg_hierarquia="'+data.results[orgao].sg_hierarquia+'" nm_ambito="'+data.results[orgao].ambito.nm_ambito+'"' +
		            		'dt_inicio_vigencia="'+data.results[orgao].dt_inicio_vigencia+'" dt_fim_vigencia="'+dt_fim_vigencia+'"' +
		            		'orgaos_cadastradores="'+orgaos_cadastradores+'"> </input>' + data.results[orgao].nm_orgao + '</li>'
		            	);
	        			var orgao_existente = $('tr[ch_orgao="'+data.results[orgao].ch_orgao+'"]');
	        			if (IsNotNullOrEmpty(orgao_existente)){
	        				$('#checkbox_orgao'+orgao).prop('checked', true);
	        			}	  
	        		}
            	}
        	}
	        var inicio = function () {
	            $('#div_loading_orgao').show();
	            $('#div_orgaos_filho').hide();
	        }
	        var complete = function () {
	            $('#div_loading_orgao').hide();
	            $('#div_orgaos_filho').show();
            }
        	$.ajaxlight({
        		sUrl: "./ashx/Consulta/OrgaosInferioresConsulta.ashx?ch_orgao="+ch_orgao, 
           		fnSuccess: sucesso,
                fnBeforeSubmit: inicio,
                fnComplete: complete,
                bAsync: true
        	});
    	},
        oButtons: [
        	{
	            text: "Salvar",
	            click: function () {
	            	var filhos_selecionados = $('.checkbox_orgao_filho:checked');
	            	var filhos_nao_selecionados = $('.checkbox_orgao_filho:not(:checked)');
				    if (IsNotNullOrEmpty(filhos_selecionados)) {
				    	$('#div_filhos_selecionados').show();
				  		$('#tbody_filhos .tr_vazia').remove();
		            	for (var i=0; i<filhos_selecionados.length;i++){
		            		var obj_filho = {
		            			ch_orgao : $(filhos_selecionados[i]).attr('ch_orgao'),		
		            			ch_hierarquia : $(filhos_selecionados[i]).attr('ch_hierarquia'),		
		            			ch_orgao_pai : $(filhos_selecionados[i]).attr('ch_orgao_pai'),		
		            			nm_orgao : $(filhos_selecionados[i]).attr('nm_orgao'),		
		            			sg_orgao : $(filhos_selecionados[i]).attr('sg_orgao'),		
		            			sg_hierarquia : $(filhos_selecionados[i]).attr('sg_hierarquia'),		
		            			nm_ambito : $(filhos_selecionados[i]).attr('nm_ambito'),		
		            			dt_inicio_vigencia : $(filhos_selecionados[i]).attr('dt_inicio_vigencia'),		
		            			dt_fim_vigencia : $(filhos_selecionados[i]).attr('dt_fim_vigencia'),			
		            			orgaos_cadastradores : $(filhos_selecionados[i]).attr('orgaos_cadastradores')		            	
		            		};
		        			var orgao_existente = $('tr[ch_orgao="'+obj_filho.ch_orgao+'"]');
		        			if (!IsNotNullOrEmpty(orgao_existente)){
			            		$('#tbody_filhos').append(
			            			'<tr onmouseover="DestacarLinha(event)" onmouseout="RemoverDestaqueLinha()" ch_orgao="'+obj_filho.ch_orgao+'" ch_hierarquia="'+obj_filho.ch_hierarquia+'" ch_orgao_pai="'+obj_filho.ch_orgao_pai+'">' +
			            				'<td nm_orgao>' + obj_filho.nm_orgao + '<input type="hidden" name="orgao_filho" value="'+obj_filho.ch_orgao+'"/> </td>' +
			            				'<td sg_orgao>' + obj_filho.sg_orgao + '</td>' +
			            				'<td sg_hierarquia>' + obj_filho.sg_hierarquia + '</td>' +
			            				'<td nm_ambito>' + obj_filho.nm_ambito + '</td>' +
			            				'<td dt_inicio_vigencia>' + obj_filho.dt_inicio_vigencia + '</td>' +
		                   				'<td dt_fim_vigencia>'+ (obj_filho.dt_fim_vigencia != "null" ? '<input type="text" name="orgao_filho_dt_fim_vigencia" onblur="javascript:AlterarDataFimVigencia(event)" class="w-100-pc date" value="' + obj_filho.dt_fim_vigencia + '"</input> ' : '')+'</td>' +
			            				'<td orgaos_cadastradores>' + obj_filho.orgaos_cadastradores + '</td>' +
					                    '<td class="control">' +
					                        '<a href="javascript:void(0);" onclick="javascript:DeletarFilhos(\''+obj_filho.ch_orgao+'\')"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +  
					                        '<a class="modal" href="javascript:void(0);" onclick="javascript:CriarModalFilhos(\''+obj_filho.ch_orgao+'\',\''+obj_filho.nm_orgao+'\',\''+obj_filho.dt_fim_vigencia+'\');"><img valign="absmiddle" title="Selecionar Órgãos Filho" alt="Selecionar Órgãos Filho" src="'+_urlPadrao+'/Imagens/ico_arrow_down.png"  /></a>' +
					                    '</td>' +
				                    '</tr>'  
			            		);
			    				var onselect = function(date, inst){
									var hoje = new Date();
									// O split recebe o string com a data selecionada
									// e o divide em tres partes: data, mes, ano
									var split_date = date.split("/");
									var data_selecionada = new Date();
									data_selecionada.setDate(parseInt(split_date[0]));
									data_selecionada.setMonth(parseInt(split_date[1]) -1);
									data_selecionada.setFullYear(parseInt(split_date[2]));
									var dt_inicio_vigencia_recebida = $(this).closest('tr').find('[dt_inicio_vigencia]').text();	
									var split_dt_inicio_vigencia = dt_inicio_vigencia_recebida.split("/");
									var dt_inicio_vigencia = new Date();
									dt_inicio_vigencia.setDate(parseInt(split_dt_inicio_vigencia[0]));
									dt_inicio_vigencia.setMonth(parseInt(split_dt_inicio_vigencia[1]) -1);
									dt_inicio_vigencia.setFullYear(parseInt(split_dt_inicio_vigencia[2]));
									if (data_selecionada > hoje || data_selecionada < dt_inicio_vigencia){
										$(this).val("");
										$('<div id="modal_notificacao_data_invalida" />').modallight({
							                sTitle: "Data inválida",
							                sContent: (data_selecionada > hoje ? "A data do fim de vigência não pode ser maior que a data de hoje" : "A data do fim de vigência não pode ser maior que a data de início."),
							                sType: "error",
											sWidth: 500,
							                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
							                fnClose: function () {
							                    $(this).remove();
							                }
							            });
									}
									else{
										var ch_orgao = $(this).parent().closest('tr').attr('ch_orgao');
										AlterarDataFimVigenciaDatePicker(date, ch_orgao, this);
									}
								}
								inicializarDatePicker({onSelect:onselect});
		        			}	
		            	}
			  		}
					if (IsNotNullOrEmpty(filhos_nao_selecionados)) {
		            	for (var i=0; i<filhos_nao_selecionados.length;i++){
		            		var obj_filho = {
		            			ch_orgao : $(filhos_nao_selecionados[i]).attr('ch_orgao'),		
		            			ch_hierarquia : $(filhos_nao_selecionados[i]).attr('ch_hierarquia'),		
		            			ch_orgao_pai : $(filhos_nao_selecionados[i]).attr('ch_orgao_pai'),		
		            			nm_orgao : $(filhos_nao_selecionados[i]).attr('nm_orgao'),		
		            			sg_orgao : $(filhos_nao_selecionados[i]).attr('sg_orgao'),		
		            			sg_hierarquia : $(filhos_nao_selecionados[i]).attr('sg_hierarquia'),		
		            			nm_ambito : $(filhos_nao_selecionados[i]).attr('nm_ambito'),		
		            			dt_inicio_vigencia : $(filhos_nao_selecionados[i]).attr('dt_inicio_vigencia'),		
		            			dt_fim_vigencia : $(filhos_nao_selecionados[i]).attr('dt_fim_vigencia'),			
		            			orgaos_cadastradores : $(filhos_nao_selecionados[i]).attr('orgaos_cadastradores')		            	
		            		};
		        			var orgao_existente = $('tr[ch_orgao="'+obj_filho.ch_orgao+'"]');
		        			if (IsNotNullOrEmpty(orgao_existente)){
		        				orgao_existente.remove();
		        			}
		        			function RemoverDescendentes(ch_orgao_pai){
			        			var filho_existente = $('tr[ch_orgao_pai="'+ch_orgao_pai+'"]');
			        			if (IsNotNullOrEmpty(filho_existente)){
			        				filho_existente.remove();
			        				var ch_descendente = $(filho_existente).attr('ch_orgao');
			        				RemoverDescendentes(ch_descendente);
			        			}
		        			}
							RemoverDescendentes(obj_filho.ch_orgao);
	            		}
	            		if ($('#tbody_filhos tr').length <= 0){
	            			$('#div_filhos_selecionados').hide();
	            		}
					}
                    $(this).dialog('close');
                }
            }, 
            {
	            text: "Cancelar",
	            click: function () {
	                $(this).dialog('close');
	            }
            }
        ],
        fnClose: function(){
       			$('#modal_filhos').dialog('destroy');
       		}
        });
}

function DeletarFilhos(ch_orgao){
	$('tr[ch_orgao="'+ch_orgao+'"]').remove();
	$('tr[ch_orgao_pai="'+ch_orgao+'"]').each(function(){
		var ch_descendente = $(this).attr('ch_orgao');
		$('tr[ch_orgao_pai="'+ch_orgao+'"]').remove();
		DeletarFilhos(ch_descendente);
	});
	if ($('#tbody_filhos tr').length <= 0){
		$('#div_filhos_selecionados').hide();
	}
}

function AlterarDataFimVigenciaDatePicker(dt_fim_vigencia, ch_orgao, calendario){
	$('tr[ch_orgao="'+ch_orgao+'"]').attr("dt_fim_vigencia", dt_fim_vigencia);
	var nm_orgao = $('tr[ch_orgao="'+ch_orgao+'"] td[nm_orgao]').text();
	$('tr[ch_orgao="'+ch_orgao+'"] a.modal').attr("onclick", "javascript:CriarModalFilhos('"+ch_orgao+"','"+nm_orgao+"','"+dt_fim_vigencia+"')");
}

function AlterarDataFimVigencia(event){
	var dt_inicio_vigencia_recebida = $(event.target).closest('tr').find('[dt_inicio_vigencia]').text();	
	var split_dt_inicio_vigencia = dt_inicio_vigencia_recebida.split("/");
	var dt_inicio_vigencia = new Date();
	dt_inicio_vigencia.setDate(parseInt(split_dt_inicio_vigencia[0]));
	dt_inicio_vigencia.setMonth(parseInt(split_dt_inicio_vigencia[1]) -1);
	dt_inicio_vigencia.setFullYear(parseInt(split_dt_inicio_vigencia[2]));
	var dt_fim_vigencia = event.target.value;
	var hoje = new Date();
	var split_date = dt_fim_vigencia.split("/");
	var data_selecionada = new Date();
	data_selecionada.setDate(parseInt(split_date[0]));
	data_selecionada.setMonth(parseInt(split_date[1]) -1);
	data_selecionada.setFullYear(parseInt(split_date[2]));
	if (data_selecionada > hoje || data_selecionada < dt_inicio_vigencia){
		$(event.target).val("");
		$('<div id="modal_notificacao_data_invalida" />').modallight({
            sTitle: "Data inválida",
            sContent: (data_selecionada > hoje ? "A data do fim de vigência não pode ser maior que a data de hoje" : "A data do fim de vigência não pode ser maior que a data de início."),
            sType: "error",
			sWidth: 500,
            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
            fnClose: function () {
                $(this).remove();
            }
        });
	}
	else{
		$(event.target).closest('tr').attr("dt_fim_vigencia", dt_fim_vigencia);
		var nm_orgao = $(event.target).closest('tr').find('td[nm_orgao]').text();
		var ch_orgao = $(event.target).closest('tr[ch_orgao]').attr("ch_orgao");
		$(event.target).closest('tr').find('a.modal').attr("onclick", "javascript:CriarModalFilhos('"+ch_orgao+"','"+nm_orgao+"','"+dt_fim_vigencia+"')");
	}
}

function DestacarLinha(event){
	var linha_pai =$( event.target).closest('tr');
	if (IsNotNullOrEmpty($(linha_pai).attr('ch_hierarquia'))){
		var ch_hierarquia = $(linha_pai).attr('ch_hierarquia');
		var split_ch_hierarquia = ch_hierarquia.split('.');
		for (var i=0; i<split_ch_hierarquia.length;i++){
			$('tr[ch_orgao="'+split_ch_hierarquia[i]+'"]').addClass('linha-destacada');
			$('tr[ch_orgao="'+split_ch_hierarquia[i]+'"]').find('[sg_hierarquia]').addClass('hierarquia-destacada');
		}
	}
	else{ //Cai no else se nao for o maior ancestral (Orgao Anterior na tabela Cronologia)
		$(linha_pai).find('[sg_hierarquia]').addClass('hierarquia-destacada');
		$(linha_pai).addClass('linha-destacada');
		var ch_orgao = $(linha_pai).attr('ch_orgao');
		var filhos_existentes = $('#tbody_filhos tr[ch_orgao]');
		for (var i=0; i<filhos_existentes.length; i++){
			var ch_hierarquia = $(filhos_existentes[i]).attr('ch_hierarquia');
			var split_ch_hierarquia = ch_hierarquia.split('.');
			if (ch_orgao == split_ch_hierarquia[0]){
				$(filhos_existentes[i]).addClass('linha-destacada');
				$(filhos_existentes[i]).find('[sg_hierarquia]').addClass('hierarquia-destacada');
			}
		}
	}
}

function RemoverDestaqueLinha(){
	$('#tbody_filhos tr, #tbody_cronologia tr').removeClass('linha-destacada');
	$('#tbody_filhos tr, #tbody_cronologia tr').find('[sg_hierarquia]').removeClass('hierarquia-destacada');
}

function SelecionarOrgaoAnteriorAutocompleteModal() {
    var dt_inicio = $('#label_dt_inicio_vigencia_orgao_anterior_modal').text();
    var dt_fim = $('#label_dt_fim_vigencia_orgao_anterior_modal').text();
    if (!IsNotNullOrEmpty(dt_inicio)) {
        $('#label_dt_inicio_vigencia_orgao_anterior_modal').parent().html('<input type="text" id="input_dt_inicio_vigencia_orgao_anterior_modal" value="" />');
    }
    if (!IsNotNullOrEmpty(dt_fim)) {
        $('#label_dt_fim_vigencia_orgao_anterior_modal').parent().html('<input type="text" id="input_dt_fim_vigencia_orgao_anterior_modal" value="" />');
    }
}

function ConstruirControlesDinamicos() {
    ConstruirOrgaosCadastradores();
    ConstruirAmbitos();
    $('#div_autocomplete_orgao_pai').autocompletelight({
        sKeyDataName: "ch_orgao",
        sValueDataName: "get_sg_hierarquia_nm_vigencia",
        sInputHiddenName: "ch_orgao_pai_modal",
        sInputName: "sg_nm_orgao_pai_modal",
        sAjaxUrl: "./ashx/Autocomplete/OrgaoPaiAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_orgao_pai_modal",
        dOthersHidden: [
                        { campo_app: "label_nm_orgao_pai_modal", campo_base: "nm_orgao" },
                        { campo_app: "label_sg_orgao_pai_modal", campo_base: "sg_orgao" },
                        { campo_app: "label_sg_hierarquia_orgao_pai_modal", campo_base: "sg_hierarquia" },
                        { campo_app: "label_in_ambito_orgao_pai_modal", campo_base: "ambito.nm_ambito" },
                        { campo_app: "label_dt_inicio_vigencia_orgao_pai_modal", campo_base: "dt_inicio_vigencia" },
                        { campo_app: "label_dt_fim_vigencia_orgao_pai_modal", campo_base: "dt_fim_vigencia" },
                        { campo_app: "label_in_orgao_cadastrador_orgao_pai_modal", campo_base: "get_orgaos_cadastradores" }
                    ]
    });
    $('#div_autocomplete_orgao_anterior').autocompletelight({
        sKeyDataName: "ch_orgao",
        sValueDataName: "get_sg_hierarquia_nm_vigencia",
        sInputHiddenName: "ch_orgao_anterior_modal",
        sInputName: "sg_nm_orgao_anterior_modal",
        sAjaxUrl: "./ashx/Autocomplete/OrgaoAnteriorAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_orgao_anterior_modal",
        dOthersHidden: [
                        { campo_app: "label_nm_orgao_anterior_modal", campo_base: "nm_orgao" },
                        { campo_app: "label_sg_orgao_anterior_modal", campo_base: "sg_orgao" },
                        { campo_app: "label_sg_hierarquia_orgao_anterior_modal", campo_base: "sg_hierarquia" },
                        { campo_app: "label_in_ambito_orgao_anterior_modal", campo_base: "ambito.nm_ambito" },
                        { campo_app: "label_dt_inicio_vigencia_orgao_anterior_modal", campo_base: "dt_inicio_vigencia" },
                        { campo_app: "label_dt_fim_vigencia_orgao_anterior_modal", campo_base: "dt_fim_vigencia" },
                        { campo_app: "label_in_orgao_cadastrador_orgao_anterior_modal", campo_base: "get_orgaos_cadastradores" }
                    ]
    });
    $('#div_autocomplete_tipo_norma_modal').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma_modal",
        sInputName: "nm_tipo_norma_modal",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_norma_modal"
    });
    if (!$('#div_st_orgao input[value="true"]').prop("checked")) {
        SelecionarInativo();
    }
    else {
        SelecionarAtivo();
    }
}

function SelecionarOrgaoPai() {
    if ($('#ch_orgao_pai_modal').val() != "") {
        $('#modal_hierarquia .dados_orgao').show();
    }
    else {
        $('#modal_hierarquia .dados_orgao').hide();
    }
}

function SelecionarOrgaoAnterior() {
    if ($('#ch_orgao_anterior_modal').val() != "") {
        $('#modal_cronologia .dados_orgao').show();
        if ($('#label_dt_inicio_vigencia_orgao_anterior_modal').text().trim() == "") {
            $('#input_dt_inicio_vigencia_orgao_anterior_modal').show();
        }
        else {
            $('#input_dt_inicio_vigencia_orgao_anterior_modal').hide();
        }
        if ($('#label_dt_fim_vigencia_orgao_anterior_modal').text().trim() == "") {
            $('#input_dt_fim_vigencia_orgao_anterior_modal').show();
        }
        else {
            $('#input_dt_fim_vigencia_orgao_anterior_modal').hide();
        }
    }
    else {
        $('#modal_cronologia .dados_orgao').hide();
    }
}

function SelecionarAtivo() {
    $("#line_dt_fim_vigencia").hide();
    $("#label_inativar_filhos").hide();
    $("#dt_fim_vigencia").val("");
}

function SelecionarInativo() {
    $("#line_dt_fim_vigencia").show();
    $("#label_inativar_filhos").show();
    $("#checkbox_inativar_filhos").prop("checked", false);
}

function AbrirModalSelecionarNormaAssociada(associacao) {
    $('#a_pesquisar_norma_associada').attr('associacao',associacao);
    $('#modal_norma_associada').modallight({
        sTitle:"Consultar Norma",
        sWidth:"800",
        oButtons:[]
    });
}

function PesquisarNorma() {
    $("#datatable_normas_modal").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?ch_tipo_norma=' + $('#ch_tipo_norma_modal').val() + '&nr_norma=' + $('#nr_norma_modal').val() + '&dt_assinatura=' + $('#dt_assinatura_modal').val(),
        aoColumns: fn_columns_norma_associada($('#a_pesquisar_norma_associada').attr('associacao')),
        sIdTable: 'table_normas_modal',
        bFilter: true
    });
}

function SelecionarNormaAssociada(ch_norma, ds_norma, associacao) {
    if (IsNotNullOrEmpty(ch_norma) && IsNotNullOrEmpty(associacao)) {
        if (associacao == "inicio") {
            $('#disabled_norma_inicio_vigencia').val(ds_norma);
            var norma_associada = ch_norma + "#" + ds_norma;
            $('#norma_inicio_vigencia').val(norma_associada);
            $('#a_remover_norma_inicio_vigencia').show();
            $('#a_adicionar_norma_inicio_vigencia').hide();
        }
        else {
            $('#disabled_norma_fim_vigencia').val(ds_norma);
            var norma_associada = ch_norma + "#" + ds_norma;
            $('#norma_fim_vigencia').val(norma_associada);
            $('#a_remover_norma_fim_vigencia').show();
            $('#a_adicionar_norma_fim_vigencia').hide();
        }
        $('#datatable_normas_modal').html('');
        if ($('#modal_norma_associada').is(':ui-dialog')) {
            $('#modal_norma_associada').dialog('close');
        }
    }
}

function RemoverNormaAssociada(associacao) {
    if (IsNotNullOrEmpty(associacao)) {
        if (associacao == "inicio") {
            $('#disabled_norma_inicio_vigencia').val("");
            $('#norma_inicio_vigencia').val("");
            $('#a_remover_norma_inicio_vigencia').hide();
            $('#a_adicionar_norma_inicio_vigencia').show();
        }
        else {
            $('#disabled_norma_fim_vigencia').val("");
            $('#norma_fim_vigencia').val("");
            $('#a_remover_norma_fim_vigencia').hide();
            $('#a_adicionar_norma_fim_vigencia').show();
        }
    }
}


function CriarModalDescricaoHierarquia(ch_hierarquia) {
	$('#modal_descricao_hierarquia').modallight({
        sTitle: "Descrição Hierárquica",
        sWidth: '600',
		sContent: '<div id="consulta_orgao_hierarquia"></div>',
		fnCreated: function(){
			var sucesso = function(data){
				$('#consulta_orgao_hierarquia').append(data.ds_hierarquia);
			}
			$.ajaxlight({
        		sUrl: "./ashx/Consulta/HierarquiaOrgaoConsulta.ashx?ch_hierarquia="+ch_hierarquia, 
           		fnSuccess: sucesso,
           		bAsync: true
			});
		}, 
        fnClose: function () {
            LimparModal('modal_descricao_hierarquia');
        }
	})
}


function ExpandirTabelaDeNormas(ch_orgao) {
    if ($('#a_selecionar_normas img').hasClass('opened')) {
        $('#a_selecionar_normas img').removeClass('opened');
        $('#a_selecionar_normas img').addClass('closed');
        $('#a_selecionar_normas img').prop('src', _urlPadrao + '/Imagens/ico_arrow_down.png');
        $('#div_datatable_normas').hide();
    }
    else {
        $('#a_selecionar_normas img').removeClass('closed');
        $('#a_selecionar_normas img').addClass('opened');
        $('#div_datatable_normas').show();
        $('#a_selecionar_normas img').prop('src', _urlPadrao + '/Imagens/ico_arrow_up.png');
        $("#div_datatable_normas").dataTablesLight({
            sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?ch_orgao=' + ch_orgao,
            aoColumns: _columns_norma_detalhes_orgao,
            sIdTable: 'datatable_normas',
            bFilter: true
        });
    }
}

function AtualizarOrigemDasNormas(ch_orgao, sg_orgao, nm_orgao) {
    if (IsNotNullOrEmpty(ch_orgao) && IsNotNullOrEmpty(sg_orgao) && IsNotNullOrEmpty(nm_orgao)) {
        var inicio = function () {
            $('#texto_orgao_modal').hide();
            $('#div_orgao_loading_modal').show();
        }
        var complete = function () {
            $('#div_orgao_loading_modal').hide();
            $('#texto_orgao_modal').show();
        }
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data, "count_normas_alteradas")) {
                $('#modal_confirmacao_atualizar_normas').modallight({
                    sTitle: "Sucesso",
                    sType: "success",
                    sWidth: '350',
                    sContent: (data.count_normas_alteradas == 1 ? data.count_normas_alteradas + ' norma atualizada.' : data.count_normas_alteradas + ' normas atualizadas.'),
                    oButtons: [
                        {
                            text: "Fechar",
                            click: function () {
                                $(this).dialog('close');
                            }
                        }
                    ],
                    fnClose: function () {
                        LimparModal('modal_confirmacao_atualizar_normas');
                    }
                });
            }
            else if (IsNotNullOrEmpty(data, "nenhuma")) {
                $('#modal_confirmacao_atualizar_normas').modallight({
                    sTitle: "Sucesso",
                    sType: "success",
                    sWidth: '350',
                    sContent: 'Nenhuma norma foi atualizada.',
                    oButtons: [
                        {
                            text: "Fechar",
                            click: function () {
                                $(this).dialog('close');
                            }
                        }
                    ],
                    fnClose: function () {
                        LimparModal('modal_confirmacao_atualizar_normas');
                    }
                });
            }
        }
        $.ajaxlight({
        	sUrl: "./ashx/AtualizarOrigemDasNormas.ashx?ch_orgao=" +ch_orgao+ "&sg_orgao=" +sg_orgao+ "&nm_orgao=" +nm_orgao, 
           	fnSuccess: sucesso,
            fnBeforeSend: inicio,
            fnComplete: complete,
            iTimeout: 0,
            bAsync: true
        });
    }
}

function CriarModalConfirmacaoAtualizarOrigemDasNormas(ch_orgao, sg_orgao, nm_orgao) {
    $('#modal_confirmacao_atualizar_normas').modallight({
        sTitle: "Confirmação",
        sWidth: '350',
        sContent: '<span id="texto_orgao_modal">Deseja realmente atualizar todas as normas que tem esse órgão como origem?</span>' +
                  '<div id="div_orgao_loading_modal" class="loading" style="display:none;"></div>',
        oButtons: [
            {
                text: "Confirmar",
                click: function () {
                    AtualizarOrigemDasNormas(ch_orgao, sg_orgao, nm_orgao);
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('close');
                }
            }
        ],
        fnClose: function () {
            LimparModal('modal_confirmacao_atualizar_normas');
        }
    });
}

function SalvarOrgao(id_form) {
    var success = function (data) {
        gComplete();
        if (IsNotNullOrEmpty(data)) {
            if (IsNotNullOrEmpty(data, 'error_message')) {
                $('#div_notificacao_orgao').messagelight({
                    sTitle: "Erro",
                    sContent: data.error_message,
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
            else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                if (IsNotNullOrEmpty(data, 'filhos_errados')) {
                    $('#div_notificacao_orgao').modallight({
                        sTitle: "Atenção",
                        sContent: "Salvo com sucesso.<br/>No entanto, os filhos " + data.filhos_errados + " não foram herdados.",
                        sType: "alert",
                        oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                        fnClose: function () {
                            if (data.b_atualizar_normas == "1") {
                                $('#div_notificacao_orgao').modallight({
                                    sTitle: "Atenção",
                                    sContent: "As normas que utilizam o órgão estão sendo atualizadas. Por favor, aguarde...",
                                    sType: "alert",
                                    oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }]
                                });
                            }
                            else if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                location.reload();
                            }
                        }
                    });
                }
                else {
                    $('#div_notificacao_orgao').modallight({
                        sTitle: (data.b_atualizar_normas == "1" ? "Atenção" : "Sucesso"),
                        sContent: "Salvo com sucesso." + (data.b_atualizar_normas == "1" ? "<br/>As normas que utilizam o órgão estão sendo atualizadas. Por favor, aguarde..." : ""),
                        sType: (data.b_atualizar_normas == "1" ? "alert" : "success"),
                        oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                        fnClose: function () {
                            if (!data.b_atualizar_normas == "1") {
                                if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                    location.reload();
                                }
                            }
                        }
                    });
                }
                if (data.b_atualizar_normas == "1") {
                    var sucesso_atualizar_normas = function (data_atualizar_normas) {
                        if (IsNotNullOrEmpty(data_atualizar_normas, 'error_message')) {
                            $('#div_notificacao_atualizar_normas').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            var fnPreencherTabelaRecursiva = function (msg_orgao) {
                                var tr = '<tr><td>' + msg_orgao.sg_orgao + '</td><td>' + msg_orgao.sucesso + ' de ' + msg_orgao.total + '</td><td>' + msg_orgao.erro + ' de ' + msg_orgao.total + '</td></tr>';
                                for (var i = 0; i < msg_orgao.filhos.length; i++) {
                                    tr += fnPreencherTabelaRecursiva(msg_orgao.filhos[i]);
                                }
                                return tr;
                            }
                            $('#div_notificacao_atualizar_normas').modallight({
                                sWidth: "500",
                                sTitle: "Sucesso",
                                sContent: '<div class="table w-100-pc"><div class="line"><div class="column w-100-pc"><div class="cell w-100-pc"><table class="w-90-pc mauto"><caption>Informações das Normas Atualizadas</caption><thead><tr><th>Órgão</th><th>Normas atualizadas</th><th>Normas não atualizadas</th></tr></thead><tbody>' + fnPreencherTabelaRecursiva(data_atualizar_normas) + '</tbody></table></div></div></div></div>',
                                sType: "success",
                                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                                fnClose: function () {
                                    if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                        location.reload();
                                    }
                                }
                            });
                        }
                    }
                    var complete_atualizar_normas = function () {
                        gComplete();
                    }
                    var inicio_atualizar_Normas = function () {
                        gInicio();
                        $('#div_notificacao_atualizar_normas').messagelight({
                            sTitle: "",
                            sContent: 'O sistema está atualizando as normas que utilizam o órgao. Por favor, aguarde...',
                            sType: "info",
                            sWidth: "",
                            iTime: null
                        });
                    }
                    $.ajaxlight({
                        sUrl: "./ashx/Path/OrigemDaNormaPath.ashx" + window.location.search,
                        sType: "POST",
                        fnSuccess: sucesso_atualizar_normas,
                        sFormId: id_form,
                        fnBeforeSend: inicio_atualizar_Normas,
                        fnComplete: complete_atualizar_normas,
                        bAsync: true,
                        iTimeout: 0
                    });
                }
            }
            else {
                $('#div_notificacao_orgao').messagelight({
                    sTitle: "Erro",
                    sContent: "Erro ao salvar.",
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
        }
    }
    return fnSalvar("form_orgao", 'SAL', success);
}

function SalvarComInativarFilhos(){
    if ($("#checkbox_inativar_filhos").prop("checked")){
        return CriarModalInativarFilhos();
    }
    else return SalvarOrgao("form_orgao");
}

function CriarModalInativarFilhos() {
    try {
        var ch_orgao = $("#label_inativar_filhos").attr("ch_orgao");
        var dt_fim_vigencia = $('#dt_fim_vigencia').val();
        if (!IsNotNullOrEmpty(dt_fim_vigencia)) {
            throw "Data de Fim de Vigência Obrigatório caso o Órgão esteja Inativo.";
        }

        var inicio = function () {
            $('#div_orgao_loading').show();
            $('#div_orgao').hide();
        };
        var complete = function () {
            $('#div_orgao_loading').hide();
            $('#div_orgao').show();
        };
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (IsNotNullOrEmpty(data, "lista_validacao_erros")) {
                    $("#modal_inativar_filhos").modallight({
                        sTitle: "Inativação Hierarquia Inferior",
                        sType: "alert",
                        sWidth: "700",
                        oButtons: [
                        {
                            text: "Continuar",
                            click: function () {
                                return SalvarOrgao("form_orgao");
                                $(this).dialog('close');
                            }
                        },
                        {
                            text: "Atualizar",
                            click: function () {
                                LimparModal('modal_inativar_filhos');
//                                $(this).dialog('close');
                                CriarModalInativarFilhos();
                            }
                        },
                        {
                            text: "Cancelar",
                            click: function () {
                                LimparModal('modal_inativar_filhos');
                                $(this).dialog('close');
                            }
                        }
                    ],
                        fnCreated: function () {
                            $('#tbody_validacao_filhos .tr_vazia').remove();
                            var length_lista_validacao = data.lista_validacao_erros.length;
                            for (var i = 0; i < length_lista_validacao; i++) {
                                $("#tbody_validacao_filhos").append(
                                "<tr>" +
                                    "<td>" +
                                        "<a href='./DetalhesDeOrgao.aspx?ch_orgao=" + data.lista_validacao_erros[i].ch_orgao + "' onclick='window.open(this.href); return false;'>" + data.lista_validacao_erros[i].nm_orgao + "<img alt='detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a> " +
                                    "</td>" +
                                    "<td>" +
                                        data.lista_validacao_erros[i].error_msg +
                                    "</td>" +
                                "</tr>"
                            );
                            }
                        },
                        fnClose: function () {
                            LimparModal('modal_inativar_filhos');
                        }
                    });
                }
                else {
                    $("#modal_inativar_filhos_sucesso").modallight({
                        sTitle: "Inativação Hierarquia Inferior",
                        sWidth: "500",
                        sType: "success",
                        sContent: (data.sucesso == "todos" ? "Todos os órgãos inferiores serão inativados." : "Não há órgãos inferiores ativos."),
                        oButtons: [
                        {
                            text: "Continuar",
                            click: function () {
                                return SalvarOrgao("form_orgao");
                                $(this).dialog('close');
                            }
                        }
                    ]
                    });
                }
            }
        };
        $.ajaxlight({
            sUrl: "./ashx/Consulta/ValidarInativacaoHierarquiaInferior.ashx?ch_orgao=" + ch_orgao + "&dt_fim_vigencia=" + dt_fim_vigencia,
            fnSuccess: sucesso,
            fnBeforeSend: inicio,
            fnComplete: complete,
            iTimeout: 0,
            bAsync: true
        });
    }
    catch (ex) {
        $('#form_orgao .notify').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
    }
    return false;
}
