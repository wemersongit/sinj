<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarVide.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarVide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide.js?<%= TCDF.Sinj.Util.MostrarVersao() %>" ></script>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

            $('#button_salvar_vide').click(function () {
                return salvarArquivosVide(sucesso_vide);
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
                            resetDispositivo('alteradora');
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
            construirControlesDinamicos();
        });
    </script>
    <style type="text/css">
        div.table div.head div.title{width:100%}
        div.table div.head div.title h2{font-size: 18px;}
        #div_tooltip_dispositivo{
            position:absolute;
            z-index:1001;
            width: 45%;
        }
        .tooltip > .tooltip-header {
            background-color: #EEE; 
            color: #006633; 
            border: 1px solid #006633;
            padding: 0px 15px;
            font-size: 14px;
            border-radius: 4px 4px 0px 0px;
        }
        .tooltip > .tooltip-header {
            border-bottom: none;
        }
        .tooltip > .tooltip-inner {
            background-color: #EEE; 
            color: #006633; 
            border: 1px solid #006633;
            padding: 0px 15px;
            font-size: 14px;
            border-radius: 0px 0px 4px 4px;
        }
        .tooltip > .tooltip-inner {
            border-top: none;
        }    
        #div_cad_dispositivo_alteradora div.div_conteudo_arquivo.copy-enabled{cursor:copy;}
        .button-close{float:right;}
        
        
        .wrapper-progressBar {
            width: 80%;
            margin:auto;
            padding:30px;
        }

        .progressBar {
            margin-bottom:50px;
        }

        .progressBar li 
        {
            color: #38953F;
            font-size: 14px;
            list-style-type: none;
            float: left;
            width: 20%;
            position: relative;
            text-align: center;
        }

        .progressBar li:before {
            content: " ";
            line-height: 30px;
            border-radius: 50%;
            width: 30px;
            height: 30px;
            border: 2px solid #036735;
            display: block;
            text-align: center;
            margin: 0 auto 10px;
            background-color: white
        }

        .progressBar li:after {
            content: "";
            position: absolute;
            width: 100%;
            height: 4px;
            background-color: #ddd;
            top: 15px;
            left: -50%;
            z-index: -1;
        }

        .progressBar li:first-child:after {
            content: none;
        }

        .progressBar li.active {
            
        }

        .progressBar li.active:before {
            border-color: #036735;
            background-color: #036735
        }

        .progressBar .active:after 
        {
            z-index: 1;
            background-color: #036735;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Vide</label>
	</div>
    <div class="form">
        <div id="modal_norma" style="display:none;">
            <form id="form_modal_consultar_norma" name="formModalConsultarNorma" action="#" method="post" onsubmit="return pesquisarNorma()">
                <div class="table w-100-pc">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Tipo:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_autocomplete_tipo_norma_modal" class="cell w-100-pc">
                                <input id="ch_tipo_norma_modal" name="ch_tipo_norma_modal" type="hidden" value="" />
                                <input id="nm_tipo_norma_modal" name="nm_tipo_norma_modal" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma_modal"></a>
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
                                <input id="nr_norma_modal" name="nr_norma_modal" type="text" value="" class="w-20-pc" />
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
                                <input id="dt_assinatura_modal" name="dt_assinatura_modal" type="text" value="" class="date" />
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="w-90-pc mauto">
                                <div class="div-light-button fr">
                                    <button><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" alt="consultar" />Pesquisar</button>
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
            </form>
        </div>
        <div class="stepper">
            <div class="row">
                <div class="col-xs-12 col-md-8 offset-md-2 block border">
                    <div class="wrapper-progressBar">
                        <ul class="progressBar">
                            <li class="relacao">Selecionar tipo de relação</li>
                            <li class="norma-alteradora">Selecionar norma alteradora</li>
                            <li class="norma-alterada">Selecionar norma alterada</li>
                            <li class="dispositivo-alterador">Selecionar dispositivo alterador</li>
                            <li class="dispositivo-alterado">Selecionar dispositivos alterados</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="div_cad_vide">
            <form id="form_vide" name="formCadastroVide" action="#" method="post">
                <input type="hidden" id="dt_controle_alteracao" name="dt_controle_alteracao" value="" />
                <input type="hidden" id="vide" name="vide" value="" />
                <div id="div_vide" class="loaded">
                    <fieldset class="w-95-pc">
                        <!--<legend>Vide</legend>-->
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="table w-100-pc">
                                        <div class="line">
                                            <div class="column w-100-pc text-center">
                                                Tipo de relação:
                                                <select id="selectTipoDeRelacao" onchange="changeTipoRelacao(this);">
                                                    <option value=""></option>
                                                    <option value="Alteração">Alteração</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div id="lineNormas" class="line step hidden">
                                            <div class="column w-50-pc">
                                                <div class="cell w-100-pc">
                                                    <div class="table w-99-pc mauto">
                                                        <div class="head">
                                                            <div class="title">
                                                                <h2>Norma Alteradora</h2>
                                                            </div>
                                                        </div>
                                                        <div class="line">
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="label_norma_vide_alteradora">Selecionar norma:</label><a id="a_adicionar_norma_alteradora" href="javascript:void(0);" onclick="javascript:abrirModalSelecionarNorma('alteradora');" title="Selecionar a norma alteradora"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="adicionar" /></a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line line_vigor_dispositivo" style="display:none;">
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
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alteradora" style="display:none;">
                                                                <div class="div_dispositivos_selecionados"></div>
                                                                <div class="table w-100-pc">
                                                                    <hr />
                                                                    <div class="line line_conteudo_arquivo" style="display:none;">
                                                                        <div class="column w-100-pc">
                                                                            <div class="div_conteudo_arquivo w-100-pc mauto">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="columnNormaAlterada" class="column w-50-pc step hidden">
                                                <div class="cell w-100-pc">
                                                    <div class="table w-99-pc mauto">
                                                        <div class="head">
                                                            <div class="title">
                                                                <h2>Norma Alterada</h2>
                                                            </div>
                                                        </div>
                                                        <div id="line_norma_dentro_do_sistema" class="line">
                                                            <div class="column w-30-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="label_norma_vide_alterada">Selecionar norma:</label><a href="javascript:void(0);" onclick="javascript:abrirModalSelecionarNorma('alterada');" title="Selecionar a norma alterada"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="adicionar" /></a>
                                                                </div>
                                                            </div>
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="labelAlteracaoCompleta" style="display:none;"><input type="checkbox" id="in_alteracao_completa" value="1" onclick="javascript:selecionarAlteracaoCompleta();" />Alterar a norma completa</label>
                                                                    <label id="labelNormaForaDoSistema"><input type="checkbox" id="in_norma_fora_do_sistema" value="1" onclick="javascript:selecionarNormaForaDoSistema();" />Norma Fora do Sistema</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="line_norma_fora_do_sistema" class="line">
                                                            <button type="button" class="clean button-close" title="Fechar" onclick="fecharNormaForaDoSistema()"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_fechar.png" alt="fechar" width="12px" height="12px" /></button>
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
                                                                </div>
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alterada" style="display:none;">
                                                                <div class="div_dispositivos_selecionados"></div>
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
                                                                            <div class="div_conteudo_arquivo w-100-pc mauto">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line line_ds_dispositivo_norma_alterada" style="display:none;">
                                                            <div class="column w-30-pc">
                                                                <div class="cell w-100-pc">
                                                                    Descrição:
                                                                </div>
                                                            </div>
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <textarea id="ds_dispositivo_norma_alterada" name="ds_dispositivo_norma_alterada" class="w-100-pc" rows="5"></textarea>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line line_texto_dispositivo" style="display:none;">
                                                            <div class="column w-100-pc">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="lineComentario" class="line comentario step hidden">
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
                    <div id="divButtons" style="width:210px; margin:auto;" class="buttons loaded step hidden">
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
        <div id="div_cad_arquivos" class="hidden">
            <form id="form_arquivo_norma_alteradora" name="form_arquivo_norma_alteradora" url-ajax="./ashx/Arquivo/UploadHtml.ashx" action="#" method="post">
                <input name="nm_base" type="hidden" value="sinj_norma" />
                <input name="nm_arquivo" type="hidden" value="" />
                <textarea name="arquivo"></textarea>
            </form>
            <form id="form_arquivo_norma_alterada" name="form_arquivo_norma_alterada" url-ajax="./ashx/Arquivo/UploadHtml.ashx" action="#" method="post">
                <input name="nm_base" type="hidden" value="sinj_norma" />
                <input name="nm_arquivo" type="hidden" value="" />
                <textarea name="arquivo"></textarea>
            </form>
        </div>
        <div id="div_tooltip_dispositivo"></div>
    </div>
</asp:Content>
