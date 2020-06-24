<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarVide.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarVide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide.js?<%= TCDF.Sinj.Util.MostrarVersao() %>" ></script>
    <script type="text/javascript" language="javascript">
<<<<<<< HEAD
        function getSelectedText() {
            if (window.getSelection) {
                return window.getSelection().toString();
            } else if (document.selection) {
                return document.selection.createRange().text;
            }
            return '';
        }

=======
        var id_doc = GetParameterValue('id_doc');
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
        $(document).ready(function () {
            $('#button_salvar_vide').click(function () {
                return fnSalvar("form_vide", "", sucesso_vide);
            });

            var open_modal = function (data) {
                $('<div id="modal_notificacao_modal_salvar" />').modallight({
                    sTitle: "Sucesso",
                    sContent: "Salvo com sucesso." + (IsNotNullOrEmpty(data, 'alert_message') ? '<br/>Observação:<br/>O vide foi incluído com sucesso mas houve um erro na alteração do arquivo.<br/>Mensagem do erro:<br/>' + data.alert_message : ''),
                    sType: "success",
                    oButtons: [
                        {
                            text: "Continuar", click: function () {
                                if (IsNotNullOrEmpty(data, "ch_norma")) {
                                    Redirecionar('?id_norma=' + data.ch_norma);
                                }
                                else {
                                    if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                        location.reload();
                                    }
                                }
                            }
                        },
                        {
                            text: "Adicionar outro Vide", click: function () {
                                var content = 'Adicionar outro Vide com as seguintes normas:<br/>' +
                                                '<br/> <input type="checkbox" id="checkbox_norma_alteradora" value="norma_alteradora">' + $('#label_norma_vide_alteradora').html() + ' </input> ' +
                                                '<br/> <input type="checkbox" id="checkbox_norma_alterada" value="norma_alterada">' + $('#label_norma_vide_alterada').html() + ' </input> ';
                                $(this).html(content);
                                $(this).dialog("option", "buttons", [
                                    {
                                        text: "Continuar",
                                        click: function () {
                                            if (IsNotNullOrEmpty(data, 'dt_controle_alteracao')) {
                                                $('#dt_controle_alteracao').val(data.dt_controle_alteracao);
                                            }
                                            $("#ch_tipo_relacao").val("");
                                            $("#nm_tipo_relacao").val("");
                                            $("#ds_comentario_vide").val("");

                                            if (!$('#checkbox_norma_alteradora').prop("checked")) {
                                                $('#ch_norma_alteradora').val("");
                                                $('#label_norma_vide_alteradora').html("");
                                                //$('#caput_norma_vide').prop("checked", false);
                                                $('#artigo_norma_vide').val("");
                                                $('#paragrafo_norma_vide').val("");
                                                $('#inciso_norma_vide').val("");
                                                $('#alinea_norma_vide').val("");
                                                $('#item_norma_vide').val("");
                                                $('#anexo_norma_vide').val("");
                                            }
                                            if (!$('#checkbox_norma_alterada').prop("checked")) {
                                                $('#ch_norma_alterada').val("");
                                                $('#label_norma_vide_alterada').html("");
                                                //$('#caput_norma_vide_alterada').prop("checked", false);
                                                $('#artigo_norma_vide_alterada').val("");
                                                $('#paragrafo_norma_vide_alterada').val("");
                                                $('#inciso_norma_vide_alterada').val("");
                                                $('#alinea_norma_vide_alterada').val("");
                                                $('#item_norma_vide_alterada').val("");
                                                $('#anexo_norma_vide_alterada').val("");
                                            }

                                            $(this).dialog('destroy');
                                        }
                                    }
                                ]);
                            }
                        }
                    ],
                    fnClose: function () {
                        if ($('#checkbox_norma_alteradora').prop("checked") == undefined && $('#checkbox_norma_alteradora').prop("checked") == undefined) {
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
                        else if (IsNotNullOrEmpty(data.ch_tipo_norma)) {
                            $('#ch_norma_alteradora').val(data.ch_norma);
                            $('#dt_controle_alteracao').val(data.dt_controle_alteracao);
                            $('#nm_tipo_norma_alteradora').val(data.nm_tipo_norma);
                            $('#dt_assinatura_alteradora').val(data.dt_assinatura);
                            $('#label_norma_vide_alteradora').text(data.nm_tipo_norma + " " + data.nr_norma + " " + data.dt_assinatura);
                            resetCaput('alteradora');
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Vide</label>
	</div>
    <div class="form">
        <div id="modal_norma" style="display:none;">
            <input type="hidden" id="b_norma_alteradora" value=""/>
            <div class="table w-100-pc">
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Tipo:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div id="div_autocomplete_tipo_norma_modal" class="cell w-100-pc">
                            <input id="ch_tipo_norma_modal" type="hidden" value="" />
                            <input id="nm_tipo_norma_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma_modal"></a>
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Número:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="nr_norma_modal" type="text" value="" class="w-20-pc" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell fr">
                            <label>Data de Assinatura:</label>
                        </div>
                    </div>
                    <div class="column w-70-pc">
                        <div class="cell w-100-pc">
                            <input id="dt_assinatura_modal" type="text" value="" class="date" />
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="w-90-pc mauto">
                            <div class="div-light-button fr">
                                <a href="javascript:void(0);" onclick="javascript:PesquisarNorma();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" alt="consultar" />Pesquisar</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="w-100-pc mauto">
                            <div id="datatable_normas_modal"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
<<<<<<< HEAD
        <div id="div_cad_caput_alteradora" style="display:none;">
            <div class="div_caputs_selecionados"></div>
            <div class="table w-100-pc">
                <div class="head">
                    <div class="title" style="width:100%; padding:10px;">Texto da Norma Alteradora</div>
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
                        <button type="button" title="Concluir a seleção do caput" onclick="javascript:selecionarCaput('alteradora');" >Concluir</button>
                        <button type="button" title="Cancelar a seleção do caput" onclick="javascript:fecharSelecionarCaput('alteradora');" >Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
        <div id="div_cad_caput_alterada" style="display:none;">
            <div class="div_caputs_selecionados"></div>
            <div class="table w-100-pc">
                <div class="head">
                    <div class="title" style="width:100%; padding:10px;">Texto da Norma Alterada</div>
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
                        <button type="button" title="Concluir a seleção do caput" onclick="javascript:selecionarCaput('alterada');" >Concluir</button>
                        <button type="button" title="Cancelar a seleção do caput" onclick="javascript:fecharSelecionarCaput('alterada');" >Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
=======
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
        <div id="div_cad_vide">
            <form id="form_vide" name="formCadastroVide" action="#" method="post">
                <input type="hidden" id="dt_controle_alteracao" name="dt_controle_alteracao" value="" />
                <div id="div_vide" class="loaded">
                    <fieldset class="w-95-pc">
                        <!--<legend>Vide</legend>-->
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="table w-100-pc">
                                        <div class="line">
<<<<<<< HEAD
                                            <div class="column w-35-pc">
=======
                                            <div id="columnRelacao" class="column w-100-pc text-center hidden selecionar-tipo-de-relacao bold">
                                                <label>Tipo de Relação:</label>
                                                <select id="selectTipoDeRelacao" onchange="changeTipoRelacao(this);">
                                                    <option value=""></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div id="lineNormas" class="line">
                                            <div class="column w-50-pc">
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
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
                                                                        <input type="hidden" id="nm_tipo_norma_alteradora" name="nm_tipo_norma_alteradora" value="" />
                                                                        <input type="hidden" id="dt_assinatura_alteradora" name="dt_assinatura_alteradora" value="" />
                                                                        <input type="hidden" id="ch_norma_alteradora" name="ch_norma_alteradora" obrigatorio="sim" label="Norma Alteradora" value="" />
                                                                        <label id="label_norma_vide_alteradora"></label><a id="a_adicionar_norma_alteradora" href="javascript:void(0);" onclick="javascript:AbrirModalSelecionarNorma(true);" title="Selecionar a norma alteradora"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="adicionar" /></a>
                                                                    </div>
                                                                </div>
                                                            </div>
<<<<<<< HEAD
                                                            <div class="line line_caput_norma_alteradora" style="display:none;">
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
=======
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="labelSemCitacaoNormaAlteradora" style="display:none;"><input type="checkbox" id="inSemCitacaoNormaAlteradora" value="1" onclick="javascript:selecionarSemCitacaoNormaAlteradora();" />Sem citação no texto</label>
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
                                                                </div>
                                                            </div>
                                                            <div class="line line_vigor_caput" style="display:none;">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Início da Vigência:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="text" id="dt_inicio_vigor" name="dt_inicio_vigor" value="" class="datePicker" />
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
<<<<<<< HEAD
                                                    </fieldset>
                                                </div>
                                            </div>
                                            <div class="column w-30-pc">
                                                <div class="cell w-100-pc">
                                                    <div class="table mauto w-90-pc">
=======
                                                        <div class="line hidden selecionar-arquivo-norma-alteradora">
                                                            <div class="column">
                                                                <label>Selecionar arquivo <input type="checkbox" value="1" onchange="changeSelecionarArquivoDaNorma(this)" norma="alteradora"/></label>
                                                            </div>
                                                        </div>
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
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
                                                                    <input type="text" id="nm_tipo_relacao" name="nm_tipo_relacao" value="" class="w-80-pc" onblur="javascript:verificarTipoSelecionado(this)" /><a title="Listar" id="a_tipo_relacao"></a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line">
                                                            <div id="div_notificacao_inversao" class="notify" style="display:none;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
<<<<<<< HEAD
                                            <div class="column w-35-pc">
=======
                                            <div id="columnNormaAlterada" class="column hidden selecionar-norma-alterada w-50-pc">
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
                                                <div class="cell w-100-pc">
                                                    <fieldset class="w-90-pc">
                                                        <legend>Norma Alterada</legend>
                                                        <div class="table">
                                                            <div class="line">
                                                                <div class="column w-80-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <label><input type="checkbox" id="in_norma_fora_do_sistema" name="in_norma_fora_do_sistema" value="1" onclick="javascript:SelecionarNormaForaDoSistema();" />Norma Fora do Sistema</label>
                                                                    </div>
                                                                </div>
                                                            </div>
<<<<<<< HEAD
                                                            <div id="line_norma_dentro_do_sistema" class="line">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Norma:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="hidden" id="nm_tipo_norma_alterada" name="nm_tipo_norma_alterada" value="" />
                                                                        <input type="hidden" id="dt_assinatura_alterada" name="dt_assinatura_alterada" value="" />
                                                                        <input type="hidden" id="ch_norma_alterada" name="ch_norma_alterada" obrigatorio="sim" label="Norma Alterada" value="" />
                                                                        <label id="label_norma_vide_alterada"></label><a href="javascript:void(0);" onclick="javascript:AbrirModalSelecionarNorma('alterada');" title="Selecionar a norma alterada"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="adicionar" /></a>
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
=======
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="labelAlteracaoCompleta" style="display:none;"><input type="checkbox" id="in_alteracao_completa" value="1" onclick="javascript:selecionarAlteracaoCompleta();" />Alterar a norma completa</label>
                                                                    <label id="labelNormaForaDoSistema"><input type="checkbox" id="in_norma_fora_do_sistema" value="1" onclick="javascript: selecionarNormaForaDoSistemaCadastrar();" />Norma Fora do Sistema</label>
                                                                    <label id="labelSemCitacaoNormaAlterada" style="display:none;"><input type="checkbox" id="inSemCitacaoNormaAlterada" value="1" onclick="javascript: selecionarSemCitacaoNormaAlterada();" />Sem citação no texto</label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div id="line_norma_fora_do_sistema" class="line hidden">
                                                            <button type="button" class="clean button-close" title="Fechar" onclick="fecharNormaForaDoSistemaCadastrar()"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_fechar.png" alt="fechar" width="12px" height="12px" /></button>
                                                            <div class="column w-100-pc">
                                                                <div class="table w-100-pc">
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Tipo:
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
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
                                                                                    <input type="text" name="nr_norma_vide_fora_do_sistema" value="" />
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
                                                                                    <input type="text" name="dt_publicacao_norma_vide_fora_do_sistema" value="" class="date" />
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
                                                                                    <input type="text" name="nr_pagina_publicacao_norma_vide_fora_do_sistema" value="" />
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
                                                                                    <input type="text" name="nr_coluna_publicacao_norma_vide_fora_do_sistema" value="" />
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
<<<<<<< HEAD
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
                                                            <div class="line line_caput_norma_alterada" style="display:none;">
                                                                <div class="column w-30-pc">
                                                                    <div class="cell w-100-pc">
                                                                        Dispositivo:
                                                                    </div>
                                                                </div>
                                                                <div class="column w-70-pc">
                                                                    <div class="cell w-100-pc">
                                                                        <input type="hidden" id="caput_norma_vide_alterada" name="caput_norma_vide_alterada" value="" />
                                                                        <label id="label_caput_norma_alterada"></label><a id="a_selecionar_caput_norma_alterada" href="javascript:void(0);" onclick="javascript:abrirSelecionarCaput('alterada');" title="Selecionar o caput da norma alterada"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_edit_dir.png" alt="adicionar" width="16px" height="16px" /></a>
=======
                                                        </div>
                                                        <div class="line hidden selecionar-arquivo-norma-alterada">
                                                            <div class="column">
                                                                <label>Selecionar arquivo <input type="checkbox" value="1" onchange="changeSelecionarArquivoDaNorma(this)" norma="alterada"/></label>
                                                            </div>
                                                        </div>
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alterada" style="display:none;">
                                                                <div class="table w-100-pc">
                                                                    <hr />
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
                                                                        <div class="column w-100-pc">
                                                                            <div class="table w-100-pc">
                                                                                <div class="line">
                                                                                    <div class="column w-100-pc">
                                                                                        <div class="div_conteudo_arquivo w-100-pc mauto">
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
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
<<<<<<< HEAD
                                                    </fieldset>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="line">
=======
                                                        <div class="line hidden ds-dispositivos-alterados">
                                                            <div class="column w-100-pc">
                                                                <label>Dispositivos alterados: </label><br />
                                                                <textarea name="ds_dispositivos_alterados" rows="10" cols="50" onblur="changeStepper('preencheDsDispositivoAlterado')"></textarea>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="lineComentario" class="line ds-comentario-vide hidden">
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
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
<<<<<<< HEAD
                    <div style="width:210px; margin:auto;" class="buttons loaded">
=======
                    <div id="divButtons" style="width:210px; margin:auto;" class="buttons loaded hidden">
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
                        <button id="button_salvar_vide">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" alt="salvar" />Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" alt="limpar" />Limpar
                        </button>
                    </div>
                </div>
                <div id="div_loading_vide" class="loading" style="display:none;"></div>
            </form>
        </div>
    </div>
    <div id="modal_vide"></div>
</asp:Content>
