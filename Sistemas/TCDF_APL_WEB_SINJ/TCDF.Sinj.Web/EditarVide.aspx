<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarVide.aspx.cs" Inherits="TCDF.Sinj.Web.EditarVide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_vide').click(function () {
                return fnSalvar("form_editar_vide", "", sucesso_vide);
            });

            var open_modal = function (data) {
                $('<div id="modal_notificacao_modal_salvar" />').modallight({
                    sTitle: "Sucesso",
                    sContent: "Salvo com sucesso." + (IsNotNullOrEmpty(data, 'alert_message') ? '<br/>Observação:<br/>O vide foi alterado com sucesso mas houve um erro na alteração do arquivo.<br/>Mensagem do erro:<br/>' + data.alert_message : ''),
                    sType: "success",
                    oButtons: [{
                        text: "Ok",
                        click: function () {
                            $(this).dialog('close');
                        }
                    }],
                    fnClose: function () {
                        if (IsNotNullOrEmpty(data, "ch_norma")) {
                            Redirecionar('?id_norma=' + data.ch_norma);
                        } else {
                            if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                location.reload();
                            }
                        }
                    }
                });
            }

            var sucesso_vide = function (data) {
                gComplete();
                if (IsNotNullOrEmpty(data)) {
                    if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                        open_modal(data);
                    }
                    else if (IsNotNullOrEmpty(data, 'type_error') && data.type_error == 'RiskOfInconsistency') {
                        $('<div id="modal_notificacao_modal_salvar" />').modallight({
                            sTitle: "Erro de Inconsistência",
                            sContent: data.error_message,
                            sType: "error",
                            oButtons: [
                                {
                                    text: "Atualizar Página", click: function () {
                                        location.reload();
                                    }
                                },
                                {
                                    text: "Salvar mesmo assim", click: function () {
                                        $("#dt_controle_alteracao").val(data.dt_controle_alteracao);
                                        $('#button_salvar_vide').click();

                                        $(this).dialog('destroy');
                                    }
                                }
                            ],
                            fnClose: function () {
                                location.reload();
                            }
                        });
                    }
                    else {
                        $('#form_vide .notify').messagelight({
                            sTitle: "Erro",
                            sContent: "Erro ao salvar.<br>" + data.error_message,
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    }
                }
            };

            var id_doc = GetParameterValue('id_doc');
            var ch_vide = GetParameterValue('ch_vide');
            if (IsNotNullOrEmpty(id_doc)) {
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data)) {
                        if (data.error_message != null && data.error_message != "") {
                            $('#div_notificacao_vide').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.ch_norma)) {
                            $('#dt_controle_alteracao').val(data.dt_controle_alteracao);
                            for (var i = 0; i < data.vides.length; i++) {
                                if (data.vides[i].ch_vide == ch_vide) {
                                    $('#ch_tipo_relacao').val(data.vides[i].ch_tipo_relacao);
                                    $('#nm_tipo_relacao').val(data.vides[i].nm_tipo_relacao);
                                    $('#ds_comentario_vide').val(data.vides[i].ds_comentario_vide);
                                    if (data.vides[i].in_norma_afetada) {
                                        $('#ch_norma_alteradora').val(data.vides[i].ch_norma_vide);
                                        $('#label_norma_vide_alteradora').text(data.vides[i].nm_tipo_norma_vide + " " + data.vides[i].nr_norma_vide + " " + data.vides[i].dt_assinatura_norma_vide);
                                        if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide_outra, 'caput')) {
                                            $('#caput_norma_vide_alteradora').val(JSON.stringify(data.vides[i].caput_norma_vide_outra));
                                            $('#label_caput_norma_alteradora').text(data.vides[i].caput_norma_vide_outra.ds_norma + '#' + data.vides[i].caput_norma_vide_outra.linkname + ' ');
                                            $('#a_selecionar_caput_norma_alteradora').attr('onclick', 'javascript:removerCaput(\'alteradora\');').attr('title', 'Remover o caput da norma alteradora').html('<img src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="remover" width="16px" height="16px" />');
                                            $('div.line_link_caput').show();
                                            if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide_outra.link)) {
                                                $('#a_texto_link').text(data.vides[i].caput_norma_vide_outra.link);
                                            }
                                        }
                                        $('#artigo_norma_vide').val(data.vides[i].artigo_norma_vide_outra);
                                        $('#paragrafo_norma_vide').val(data.vides[i].paragrafo_norma_vide_outra);
                                        $('#inciso_norma_vide').val(data.vides[i].inciso_norma_vide_outra);
                                        $('#alinea_norma_vide').val(data.vides[i].alinea_norma_vide_outra);
                                        $('#item_norma_vide').val(data.vides[i].item_norma_vide_outra);
                                        $('#anexo_norma_vide').val(data.vides[i].anexo_norma_vide_outra);
                                        $('#line_norma_fora_do_sistema').hide();
                                        $('#line_norma_dentro_do_sistema').show();

                                        $('#ch_norma_alterada').val(data.ch_norma);
                                        $('#label_norma_vide_alterada').text(data.nm_tipo_norma + " " + data.nr_norma + " " + data.dt_assinatura);

                                        $('#artigo_norma_vide_alterada').val(data.vides[i].artigo_norma_vide);
                                        $('#paragrafo_norma_vide_alterada').val(data.vides[i].paragrafo_norma_vide);
                                        $('#inciso_norma_vide_alterada').val(data.vides[i].inciso_norma_vide);
                                        $('#alinea_norma_vide_alterada').val(data.vides[i].alinea_norma_vide);
                                        $('#item_norma_vide_alterada').val(data.vides[i].item_norma_vide);
                                        $('#anexo_norma_vide_alterada').val(data.vides[i].anexo_norma_vide);
                                        if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide, 'caput')) {
                                            $('#caput_norma_vide_alterada').val(JSON.stringify(data.vides[i].caput_norma_vide));
                                            $('#label_caput_norma_alterada').text(data.vides[i].caput_norma_vide.ds_norma + '#' + data.vides[i].caput_norma_vide.linkname + ' ');
                                            $('#a_selecionar_caput_norma_alterada').attr('onclick', 'javascript:removerCaput(\'alterada\');').attr('title', 'Remover o caput da norma alterada').html('<img src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="remover" width="16px" height="16px" />');
                                            if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide.texto_antigo)) {
                                                $('div.line_texto_caput').show();
                                                $('div.line_ds_caput_norma_alterada').show();
                                                if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide.ds_caput)) {
                                                    $('#ds_caput_norma_alterada').val(data.vides[i].caput_norma_vide.ds_caput);
                                                }
                                                $('div.line_texto_caput').html($('<div class="column w-100-pc"></div>'));
                                                for (var j = 0; j < data.vides[i].caput_norma_vide.texto_antigo.length; j++) {
                                                    $('div.line_texto_caput>div.column').append(
                                                        '<div class="line">' +
                                                            '<div class="column w-30-pc">' +
                                                                '<div class="cell w-100-pc">Texto Antigo:</div>' +
                                                            '</div>' +
                                                            '<div class="column w-70-pc">' +
                                                                '<div class="cell w-100-pc">' +
                                                                    '<div id="texto_antigo" class="w-95-pc">' + data.vides[i].caput_norma_vide.texto_antigo[j] + '</div>' +
                                                                '</div>' +
                                                            '</div>' +
                                                        '</div>' +
                                                        '<div class="line">' +
                                                            '<div class="column w-30-pc">' +
                                                                '<div class="cell w-100-pc">Texto Novo:</div>' +
                                                            '</div>' +
                                                            '<div class="column w-70-pc">' +
                                                                '<div class="cell w-100-pc">' +
                                                                    '<textarea name="texto_novo" class="w-95-pc" rows="5">' + data.vides[i].caput_norma_vide.texto_novo[j] + '</textarea>' +
                                                                '</div>' +
                                                            '</div>' +
                                                        '</div>'
                                                    );
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        $('#ch_norma_alteradora').val(data.ch_norma);
                                        $('#label_norma_vide_alteradora').text(data.nm_tipo_norma + " " + data.nr_norma + " " + data.dt_assinatura);
                                        if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide, 'caput')) {
                                            $('#caput_norma_vide_alteradora').val(JSON.stringify(data.vides[i].caput_norma_vide));
                                            $('#label_caput_norma_alteradora').text(data.vides[i].caput_norma_vide.ds_norma + '#' + data.vides[i].caput_norma_vide.linkname + ' ');
                                            $('#a_selecionar_caput_norma_alteradora').attr('onclick', 'javascript:removerCaput(\'alteradora\');').attr('title', 'Remover o caput da norma alteradora').html('<img src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="remover" width="16px" height="16px" />');
                                            $('div.line_link_caput').show();
                                            if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide.link)) {
                                                $('#a_texto_link').text(data.vides[i].caput_norma_vide.link);
                                            }
                                        }
                                        $('#artigo_norma_vide').val(data.vides[i].artigo_norma_vide);
                                        $('#paragrafo_norma_vide').val(data.vides[i].paragrafo_norma_vide);
                                        $('#inciso_norma_vide').val(data.vides[i].inciso_norma_vide);
                                        $('#alinea_norma_vide').val(data.vides[i].alinea_norma_vide);
                                        $('#item_norma_vide').val(data.vides[i].item_norma_vide);
                                        $('#anexo_norma_vide').val(data.vides[i].anexo_norma_vide);

                                        if (IsNotNullOrEmpty(data.vides[i].ch_norma_vide)) {
                                            $('#ch_norma_alterada').val(data.vides[i].ch_norma_vide);
                                            $('#label_norma_vide_alterada').text(data.vides[i].nm_tipo_norma_vide + " " + data.vides[i].nr_norma_vide + " " + data.vides[i].dt_assinatura_norma_vide);
                                            $('#line_norma_fora_do_sistema').hide();
                                            $('#line_norma_dentro_do_sistema').show();
                                        }
                                        else {
                                            $('#ch_tipo_norma_vide_fora_do_sistema').val(data.vides[i].ch_tipo_norma_vide);
                                            $('#nm_tipo_norma_vide_fora_do_sistema').val(data.vides[i].nm_tipo_norma_vide);
                                            $('#nr_norma_vide_fora_do_sistema').val(data.vides[i].nr_norma_vide);
                                            $('#dt_assinatura_norma_vide_fora_do_sistema').val(data.vides[i].dt_assinatura_norma_vide);
                                            $('#ch_tipo_fonte_vide_fora_do_sistema').val(data.vides[i].ch_tipo_fonte_norma_vide);
                                            $('#nm_tipo_fonte_vide_fora_do_sistema').val(data.vides[i].nm_tipo_fonte_norma_vide);
                                            $('#dt_publicacao_norma_vide_fora_do_sistema').val(data.vides[i].dt_publicacao_fonte_norma_vide);
                                            $('#nr_pagina_publicacao_norma_vide_fora_do_sistema').val(data.vides[i].pagina_publicacao_norma_vide);
                                            $('#nr_coluna_publicacao_norma_vide_fora_do_sistema').val(data.vides[i].coluna_publicacao_norma_vide);
                                            $('#line_norma_fora_do_sistema').show();
                                            $('#line_norma_dentro_do_sistema').hide();
                                        }
                                        if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide_outra, 'caput')) {
                                            $('#caput_norma_vide_alterada').val(JSON.stringify(data.vides[i].caput_norma_vide_outra));
                                            $('#label_caput_norma_alterada').text(data.vides[i].caput_norma_vide_outra.ds_norma + '#' + data.vides[i].caput_norma_vide_outra.linkname + ' ');
                                            $('#a_selecionar_caput_norma_alterada').attr('onclick', 'javascript:removerCaput(\'alterada\');').attr('title', 'Remover o caput da norma alterada').html('<img src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="remover" width="16px" height="16px" />');

                                            if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide_outra.texto_antigo)) {
                                                $('div.line_texto_caput').show();
                                                $('div.line_ds_caput_norma_alterada').show();
                                                if (IsNotNullOrEmpty(data.vides[i].caput_norma_vide_outra.ds_caput)) {
                                                    $('#ds_caput_norma_alterada').val(data.vides[i].caput_norma_vide_outra.ds_caput);
                                                }
                                                var labeltextoantigo = 'Texto Antigo';
                                                var labeltextonovo = 'Texto Novo';
                                                if (data.vides[i].caput_norma_vide_outra.nm_relacao_aux == 'acrescimo') {
                                                    labeltextoantigo = 'Após o Texto';
                                                    labeltextonovo = 'Inserir o Texto';
                                                } else if (data.vides[i].caput_norma_vide_outra.nm_relacao_aux == 'renumeração') {
                                                    labeltextoantigo = 'Renumerar o Texto';
                                                    labeltextonovo = 'Texto Renumerado';
                                                }
                                                $('div.line_texto_caput').html($('<div class="column w-100-pc"></div>'));
                                                for (var j = 0; j < data.vides[i].caput_norma_vide_outra.texto_antigo.length; j++) {
                                                    $('div.line_texto_caput>div.column').append(
                                                        '<div class="table w-100-pc caputedit">' +
                                                            '<div class="line">' +
                                                                '<div class="column w-30-pc">' +
                                                                    '<div class="cell w-100-pc">Texto Antigo:</div>' +
                                                                '</div>' +
                                                                '<div class="column w-70-pc">' +
                                                                    '<div class="cell w-100-pc">' +
                                                                        '<div id="texto_antigo" class="w-95-pc">' + data.vides[i].caput_norma_vide_outra.texto_antigo[j] + '</div>' +
                                                                        '<button type="button" class="del clean" onclick="javascript:removerCaputParagrafo(this);" posicao="' + j + '"><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" width="12px" /></button>' +
                                                                    '</div>' +
                                                                '</div>' +
                                                            '</div>' +
                                                            '<div class="line">' +
                                                                '<div class="column w-30-pc">' +
                                                                    '<div class="cell w-100-pc">Texto Novo:</div>' +
                                                                '</div>' +
                                                                '<div class="column w-70-pc">' +
                                                                    '<div class="cell w-100-pc">' +
                                                                        '<button id="button_texto_antigo_' + j + '" type="button" class="clean" onclick="javascript:abrirSelecionarCaputCopiar(this);" title="Abre o texto da norma alteradora para copia e colar o texto novo no campo abaixo."><img src="' + _urlPadrao + '/Imagens/ico_copy.png" width="15px" /></button>' +
                                                                        '<textarea name="texto_novo" class="w-95-pc" rows="5">' + data.vides[i].caput_norma_vide_outra.texto_novo[j] + '</textarea>' +
                                                                    '</div>' +
                                                                '</div>' +
                                                            '</div>' +
                                                        '</div>'
                                                    );
                                                }
                                            }
                                        }
                                        $('#artigo_norma_vide_alterada').val(data.vides[i].artigo_norma_vide_outra);
                                        $('#paragrafo_norma_vide_alterada').val(data.vides[i].paragrafo_norma_vide_outra);
                                        $('#inciso_norma_vide_alterada').val(data.vides[i].inciso_norma_vide_outra);
                                        $('#alinea_norma_vide_alterada').val(data.vides[i].alinea_norma_vide_outra);
                                        $('#item_norma_vide_alterada').val(data.vides[i].item_norma_vide_outra);
                                        $('#anexo_norma_vide_alterada').val(data.vides[i].anexo_norma_vide_outra);
                                    }
                                }
                            }

                        }
                    }
                };
                $.ajaxlight({
                    sUrl: './ashx/Visualizacao/NormaDetalhes.ashx' + window.location.search,
                    sType: "GET",
                    fnSuccess: sucesso,
                    fnComplete: gComplete,
                    fnBeforeSend: gInicio,
                    fnError: null,
                    bAsync: true
                });
            }
            ConstruirControlesDinamicos();
            verificarTipoSelecionado(document.getElementById('nm_tipo_relacao'));
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Vide</label>
	</div>
    <div class="form">
        <div id="div_cad_caput_alteradora" style="display:none;">
            <div class="div_caputs_selecionados"></div>
            <div class="table w-100-pc">
                <div class="head">
                    <div class="title">Caput da Norma Alteradora</div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecione um Arquivo:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="arquivos_norma" class="cell w-100-pc">
                        </div>
                    </div>
                </div>
                <div class="line line_conteudo_arquivo" style="display:none;">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecione o Link:</label>
                        </div>
                    </div>
                    <div class="column w-100-pc">
                        <div class="div_conteudo_arquivo" class="w-70-pc mauto">
                        </div>
                    </div>
                </div>
                <div class="line line_buttons" style="display:none;">
                    <div class="column w-100-pc text-center">
                        <button type="button" title="Concluir a seleção do caput" onclick="selecionarCaput('alteradora');" >Concluir</button>
                        <button type="button" title="Cancelar a seleção do caput" onclick="fecharSelecionarCaput('alteradora');" >Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        <div id="div_cad_caput_alterada" style="display:none;">
            <div class="div_caputs_selecionados"></div>
            <div class="table w-100-pc">
                <div class="head">
                    <div class="title">Caput da Norma Alterada</div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecione um Arquivo:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="arquivos_norma" class="cell w-100-pc">
                        </div>
                    </div>
                </div>
                <div class="line line_enable_replaced" style="display:none;">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Exibir dispositivos alterados:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <input type="checkbox" onclick="clickEnableReplaced(this)" />
                    </div>
                </div>
                <div class="line line_conteudo_arquivo" style="display:none;">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Selecione o Texto:</label>
                        </div>
                    </div>
                    <div class="column w-100-pc">
                        <div class="div_conteudo_arquivo" class="w-70-pc mauto">
                        </div>
                    </div>
                </div>
                <div class="line line_buttons" style="display:none;">
                    <div class="column w-100-pc text-center">
                        <button type="button" title="Concluir a seleção do caput" onclick="selecionarCaput('alterada');" >Concluir</button>
                        <button type="button" title="Cancelar a seleção do caput" onclick="fecharSelecionarCaput('alterada');" >Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        <div id="div_cad_vide">
            <form id="form_editar_vide" name="formEditarVide" action="#" method="post">
                <input type="hidden" id="dt_controle_alteracao" name="dt_controle_alteracao" value="" />
                <div id="div_vide" class="loaded">
                    <fieldset class="w-95-pc">
                        <!--<legend>Vide</legend>-->
                        <div id="div_notificacao_vide" class="notify" style="display:none;"></div>
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="table w-100-pc">
                                        <div class="line">
                                            <div class="column w-35-pc">
                                                <div class="cell w-100-pc">
                                                    <fieldset class="w-90-pc">
                                                        <legend>Norma Alteradora</legend>
                                                        <div class="table">
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Norma:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="hidden" id="ch_norma_alteradora" value="" />
                                                                        <label id="label_norma_vide_alteradora"></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line line_caput_norma_alteradora">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Dispositivo:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="hidden" id="caput_norma_vide_alteradora" name="caput_norma_vide_alteradora" value="" />
                                                                        <label id="label_caput_norma_alteradora"></label><a id="a_selecionar_caput_norma_alteradora" href="javascript:void(0);" onclick="javascript:abrirSelecionarCaput('alteradora');" title="Selecionar o caput da norma alteradora"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_edit_dir.png" alt="adicionar" width="16px" height="16px" /></a>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line line_link_caput" style="display:none;">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Texto do link:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <a href="javascript:void(0);" id="a_texto_link"></a>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                            <div class="column w-30-pc">
                                                <div class="cell w-100-pc">
                                                    <div class="table mauto w-90-pc">
                                                        <div class="line">
                                                            <div class="column w-20-pc">
                                                                <div class="cell fr">
                                                                    <label>Relação:</label>
                                                                </div>
                                                            </div>
                                                            <div class="column w-80-pc">
                                                                <div id="div_autocomplete_tipo_relacao" class="cell w-100-pc">
                                                                    <input type="hidden" id="ch_tipo_relacao" obrigatorio="sim" label="Vide &gt; Relação" name="ch_tipo_relacao" value="" />
                                                                    <input type="hidden" id="ds_texto_para_alterador" name="ds_texto_para_alterador" value="" />
                                                                    <input type="hidden" id="ds_texto_para_alterado" name="ds_texto_para_alterado" value="" />
                                                                    <input type="text" id="nm_tipo_relacao" name="nm_tipo_relacao" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_relacao"></a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="column w-35-pc">
                                                <div class="cell w-100-pc">
                                                    <fieldset class="w-90-pc">
                                                        <legend>Norma Alterada</legend>
                                                        <div class="table">
                                                            <div id="line_norma_dentro_do_sistema" class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Norma:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-40-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="hidden" id="ch_norma_alterada" value="" />
                                                                        <label id="label_norma_vide_alterada"></label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div id="line_norma_fora_do_sistema" class="line">
                                                                <div class="column w-100-pc">
                                                                    <div class="table w-100-pc">
                                                                        <div class="line">
                                                                            <div class="column w-30-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    Tipo:
                                                                                </div>
                                                                            </div>
                                                                            <div class="column w-70-pc">
                                                                                <div id="div_autocomplete_tipo_norma_vide_fora_de_sistema" class="cell w-100-pc">
                                                                                    <input type="hidden" id="ch_tipo_norma_vide_fora_do_sistema" name="ch_tipo_norma_vide_fora_do_sistema" value="" />
                                                                                    <input type="text" id="nm_tipo_norma_vide_fora_do_sistema" name="nm_tipo_norma_vide_fora_do_sistema" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma_vide_fora_do_sistema"></a>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="line">
                                                                            <div class="column w-30-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    Número:
                                                                                </div>
                                                                            </div>
                                                                            <div class="column w-70-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    <input type="text" id="nr_norma_vide_fora_do_sistema" name="nr_norma_vide_fora_do_sistema" value="" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="line">
                                                                            <div class="column w-30-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    Tipo de Fonte:
                                                                                </div>
                                                                            </div>
                                                                            <div class="column w-70-pc">
                                                                                <div id="div_autocomplete_tipo_fonte_vide_fora_do_sistema" class="cell w-100-pc">
                                                                                    <input type="hidden" id="ch_tipo_fonte_vide_fora_do_sistema" name="ch_tipo_fonte_vide_fora_do_sistema" value="" />
                                                                                    <input type="text" id="nm_tipo_fonte_vide_fora_do_sistema" name="nm_tipo_fonte_vide_fora_do_sistema" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_fonte_vide_fora_do_sistema"></a>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="line">
                                                                            <div class="column w-30-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    Data de Publicação:
                                                                                </div>
                                                                            </div>
                                                                            <div class="column w-70-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    <input type="text" id="dt_publicacao_norma_vide_fora_do_sistema" name="dt_publicacao_norma_vide_fora_do_sistema" value="" class="date" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="line">
                                                                            <div class="column w-30-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    Página:
                                                                                </div>
                                                                            </div>
                                                                            <div class="column w-70-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    <input type="text" id="nr_pagina_publicacao_norma_vide_fora_do_sistema" name="nr_pagina_publicacao_norma_vide_fora_do_sistema" value="" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="line">
                                                                            <div class="column w-30-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    Coluna:
                                                                                </div>
                                                                            </div>
                                                                            <div class="column w-70-pc">
                                                                                <div class="cell w-100-pc">
                                                                                    <input type="text" id="nr_coluna_publicacao_norma_vide_fora_do_sistema" name="nr_coluna_publicacao_norma_vide_fora_do_sistema" value="" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Artigo:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="artigo_norma_vide_alterada" name="artigo_norma_vide_alterada" value="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Parágrafo:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="paragrafo_norma_vide_alterada" name="paragrafo_norma_vide_alterada" value="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Inciso:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="inciso_norma_vide_alterada" name="inciso_norma_vide_alterada" value="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Alínea:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="alinea_norma_vide_alterada" name="alinea_norma_vide_alterada" value="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Item:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="item_norma_vide_alterada" name="item_norma_vide_alterada" value="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Anexo:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="anexo_norma_vide_alterada" name="anexo_norma_vide_alterada" value="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line line_caput_norma_alterada">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Dispositivo:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="hidden" id="caput_norma_vide_alterada" name="caput_norma_vide_alterada" value="" />
                                                                        <label id="label_caput_norma_alterada"></label><a id="a_selecionar_caput_norma_alterada" href="javascript:void(0);" onclick="javascript:abrirSelecionarCaput('alterada');" title="Selecionar o caput da norma alterada"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_edit_dir.png" alt="adicionar" width="16px" height="16px" /></a>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line line_ds_caput_norma_alterada" style="display:none;">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Descrição:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <textarea id="ds_caput_norma_alterada" name="ds_caput_norma_alterada" class="w-100-pc" rows="5"></textarea>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="line line_texto_caput" style="display:none;">
                                                                <div class="column w-100-pc">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-100-pc">
                                                <div class="table w-80-pc mauto">
                                                    <div class="line">
                                                        <div class="column w-100-pc">
                                                            Comentário:
                                                        </div>
                                                        <div class="column w-100-pc">
                                                            <textarea id="ds_comentario_vide" name="ds_comentario_vide" class="w-100-pc" rows="5" cols="50"></textarea>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div style="width:210px; margin:auto;" class="buttons loaded">
                        <button id="button_salvar_vide">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" alt="salvar" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" alt="limpar" />Limpar
                        </button>
                    </div>
                </div>
                <div id="div_loading_vide" class="loading" style="display:none;"></div>
            </form>
        </div>
    </div>
</asp:Content>
