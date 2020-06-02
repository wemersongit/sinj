<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ExcluirVide.aspx.cs" Inherits="TCDF.Sinj.Web.ExcluirVide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide.js?<%= TCDF.Sinj.Util.MostrarVersao() %>" ></script>
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide_excluir.js?<%= TCDF.Sinj.Util.MostrarVersao() %>" ></script>
<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        var id_doc = GetParameterValue('id_doc');
        var ch_vide = GetParameterValue('ch_vide');

        var open_modal = function (data) {
            var append_eml_pes = (ValidarPermissao(_grupos.nor_eml) ? "<div style='display:block;'> Habilita enviar e-mail:" +
                "<input id='vide_st_habilita_email' name='vide_st_habilita_email' value='true' type='checkbox' title='Habilita o envio de e- mails.' " + (data.st_habilita_email == "true" || data.st_habilita_email == "True" ? 'checked' : '') + "  /> </div>" : "") +
                (ValidarPermissao(_grupos.nor_hsp) ? "<div style='display:block'> Habilita no SINJ Pesquisa:" +
                "<input id='vide_st_habilita_pesquisa' name='vide_st_habilita_pesquisa' value='true' type='checkbox' title='Habilita no SINJ Pesquisa.' " + (data.st_habilita_pesquisa == "true" || data.st_habilita_pesquisa == "True" ? 'checked' : '') + " /> </div>" : "");

            $('<div id="modal_notificacao_modal_excluir" />').modallight({
                sTitle: "Sucesso",
                sContent: "Excluído com sucesso." + append_eml_pes,
                sType: "success",
                oButtons: [
                        {
                        text: "Ok", click: function () {
                                let hsp = $('#vide_st_habilita_pesquisa').is(':checked');
                                let heml = $('#vide_st_habilita_email').is(':checked');
                                habilitarEmailPesquisa(data.id_doc_vide, hsp, heml);
                                Redirecionar('?id_norma=' + data.ch_norma);
                            }
                        }
                    ],
                fnClose: function () {
                    let hsp = $('#vide_st_habilita_pesquisa').is(':checked');
                    let heml = $('#vide_st_habilita_email').is(':checked');
                    habilitarEmailPesquisa(data.id_doc_vide, hsp, heml);
                    Redirecionar('?id_norma=' + data.ch_norma);
                }
            });
        }

        var sucesso_vide = function (data) {
            console.log(data);
            var append_eml_pes = "";
            if (IsNotNullOrEmpty(data, 'id_doc_vide')) {
                (ValidarPermissao(_grupos.nor_eml) ? "<div style='display:block;'> Habilita enviar e-mail:" +
                    "<input id='vide_st_habilita_email' name='vide_st_habilita_email' value='true' type='checkbox' title='Habilita o envio de e- mails.' " + (data.st_habilita_email == "true" || data.st_habilita_email == "True" ? 'checked' : '') + "  /> </div>" : "") +
                (ValidarPermissao(_grupos.nor_hsp) ? "<div style='display:block'> Habilita no SINJ Pesquisa:" +
                    "<input id='vide_st_habilita_pesquisa' name='vide_st_habilita_pesquisa' value='true' type='checkbox' title='Habilita no SINJ Pesquisa.' " + (data.st_habilita_pesquisa == "true" || data.st_habilita_pesquisa == "True" ? 'checked' : '') + " /> </div>" : "");
            }
            gComplete();
            if (IsNotNullOrEmpty(data)) {
                if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                    $('<div id="modal_notificacao_modal_excluir" />').modallight({
                        sTitle: "Sucesso",
                        sContent: "Excluído com sucesso." + append_eml_pes,
                        sType: "success",
                        oButtons: [
                            {
                                text: "Ok", click: function () {
                                    if (IsNotNullOrEmpty(append_eml_pes)) {
                                        let hsp = $('#vide_st_habilita_pesquisa').is(':checked');
                                        let heml = $('#vide_st_habilita_email').is(':checked');
                                        habilitarEmailPesquisa(data.id_doc_vide, hsp, heml);
                                    }
                                    Redirecionar('?id_doc=' + data.id_doc_success);
                                }
                            }
                        ],
                        fnClose: function () {
                            if (IsNotNullOrEmpty(append_eml_pes)) {
                                let hsp = $('#vide_st_habilita_pesquisa').is(':checked');
                                let heml = $('#vide_st_habilita_email').is(':checked');
                                habilitarEmailPesquisa(normaAlterada.id_doc, hsp, heml);
                            }
                            Redirecionar('?id_doc=' + data.id_doc_success);
                        }
                    });
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
                                    text: "Excluir mesmo assim", click: function () {
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


        if (IsNotNullOrEmpty(id_doc)) {
            $('#id_doc').val(id_doc);
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
                        gComplete();
                    }
                    else if (IsNotNullOrEmpty(data.ch_norma)) {
                        $('#dt_controle_alteracao').val(data.dt_controle_alteracao);
                        var videExcluir = {};
                        var index = 0;
                        for (var i = 0; i < data.vides.length; i++) {
                            if (data.vides[i].ch_vide == ch_vide) {
                                videExcluir = data.vides[i];
                                index = i;
                                if (!IsNotNullOrEmpty(videExcluir.ds_comentario_vide)) {
                                    $('#lineComentario').remove();
                                }

                                $('#button_salvar_vide').click(function () {
                                    return salvarArquivosVideExcluir(sucesso_vide, videExcluir.ch_vide);
                                });

                                var deferredTipoDeRelacao = $.Deferred();

                                $.when(deferredTipoDeRelacao).done(function () {
                                    if (!IsNotNullOrEmpty(data, 'ar_atualizado.id_file')) {
                                        videExcluir.alteracao_texto_vide.in_sem_arquivo = true;
                                    }
                                    if(IsNotNullOrEmpty(videExcluir.caput_norma_vide, 'caput')){
                                        videExcluir.alteracao_texto_vide.dispositivos_norma_vide = [];
                                        for(var i = 0; i < videExcluir.caput_norma_vide.caput.length; i++){
                                            videExcluir.alteracao_texto_vide.dispositivos_norma_vide.push({
                                                linkname: videExcluir.caput_norma_vide.caput[i],
                                                texto: (IsNotNullOrEmpty(videExcluir.caput_norma_vide, 'texto_novo['+i+']') ? videExcluir.caput_norma_vide.texto_novo[i] : IsNotNullOrEmpty(videExcluir.caput_norma_vide.link) ? videExcluir.caput_norma_vide.link : '')
                                            });
                                        }
                                    }
                                    if (videExcluir.in_norma_afetada) {
                                        videsDaNormaAlterada = data.vides;
                                        indexVideAlterado = index;
                                        selecionarNormaAlteradaExcluir({ ch_norma: data.ch_norma, nr_norma: data.nr_norma, dt_assinatura: data.dt_assinatura, nm_tipo_norma: data.nm_tipo_norma, sem_arquivo: videExcluir.alteracao_texto_vide.in_sem_arquivo, arquivo: data.ar_atualizado, dispositivos: videExcluir.alteracao_texto_vide.dispositivos_norma_vide });
                                    }
                                    else{
                                        selecionarNormaAlteradoraExcluir({ ch_norma: data.ch_norma, nr_norma: data.nr_norma, dt_assinatura: data.dt_assinatura, nm_tipo_norma: data.nm_tipo_norma, sem_arquivo: videExcluir.alteracao_texto_vide.in_sem_arquivo, arquivo: data.ar_atualizado, dispositivos: videExcluir.alteracao_texto_vide.dispositivos_norma_vide });
                                    }
                                    if (!videExcluir.in_norma_fora_sistema) {
                                        buscarOutraNorma(videExcluir.ch_norma_vide, videExcluir.ch_vide);
                                    }
                                    else{
                                        selecionarArquivoNormaAlteradoraExcluir();
                                    }
                                });
                                
                                
                                buscarTipoDeRelacao(videExcluir.ch_tipo_relacao, deferredTipoDeRelacao);
                            }
                        }
                        if (!IsNotNullOrEmpty(videExcluir, 'ch_vide')) {
                            $('#div_notificacao_vide').messagelight({
                                sTitle: "Erro",
                                sContent: "O vide não existe.",
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                            gComplete();
                        }
                    }
                }
            };
            $.ajaxlight({
                sUrl: './ashx/Visualizacao/NormaDetalhes.ashx' + window.location.search,
                sType: "GET",
                fnSuccess: sucesso,
                fnComplete: null,
                fnBeforeSend: gInicio,
                fnError: null,
                bAsync: true
            });
        }
    });
    function buscarOutraNorma(ch_norma, ch_vide){
            var sucesso = function (data) {
                if (IsNotNullOrEmpty(data)) {
                    if (data.error_message != null && data.error_message != "") {
                        var msg = data.error_message;
                        if (IsNotNullOrEmpty(data.error_type) && data.error_type == 'NotFound') {
                            msg = "A norma do vide não foi encontrada. É possível que tenha sido excluída."
                        }
                        $('#div_notificacao_vide').messagelight({
                            sTitle: "Erro",
                            sContent: msg,
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                        gComplete();
                    }
                    else if (IsNotNullOrEmpty(data.ch_norma)) {
                        for (var i = 0; i < data.vides.length; i++) {
                            if (data.vides[i].ch_vide == ch_vide) {
                                if (!IsNotNullOrEmpty(data, 'ar_atualizado.id_file')) {
                                    data.vides[i].alteracao_texto_vide.in_sem_arquivo = true;
                                }
                                if(IsNotNullOrEmpty(data.vides[i].caput_norma_vide, 'caput')){
                                    data.vides[i].alteracao_texto_vide.dispositivos_norma_vide = [];
                                    for(var i = 0; i < data.vides[i].caput_norma_vide.caput.length; i++){
                                        data.vides[i].alteracao_texto_vide.dispositivos_norma_vide.push({
                                            linkname: data.vides[i].caput_norma_vide.caput[i],
                                            texto: (IsNotNullOrEmpty(data.vides[i].caput_norma_vide, 'texto_novo['+i+']') ? data.vides[i].caput_norma_vide.texto_novo[i] : IsNotNullOrEmpty(data.vides[i].caput_norma_vide.link) ? data.vides[i].caput_norma_vide.link : '')
                                        });
                                    }
                                }
                                if (data.vides[i].in_norma_afetada) {
                                    videsDaNormaAlterada = data.vides;
                                    indexVideAlterado = i;
                                    selecionarNormaAlteradaExcluir({ ch_norma: data.ch_norma, nr_norma: data.nr_norma, dt_assinatura: data.dt_assinatura, nm_tipo_norma: data.nm_tipo_norma, sem_arquivo: data.vides[i].alteracao_texto_vide.in_sem_arquivo, arquivo: data.ar_atualizado, dispositivos: data.vides[i].alteracao_texto_vide.dispositivos_norma_vide });
                                }
                                else{
                                    selecionarNormaAlteradoraExcluir({ ch_norma: data.ch_norma, nr_norma: data.nr_norma, dt_assinatura: data.dt_assinatura, nm_tipo_norma: data.nm_tipo_norma, sem_arquivo: data.vides[i].alteracao_texto_vide.in_sem_arquivo, arquivo: data.ar_atualizado, dispositivos: data.vides[i].alteracao_texto_vide.dispositivos_norma_vide });
                                }
                                selecionarArquivosExcluir();
                            }
                        }
                    }
                }
            };
            $.ajaxlight({
                sUrl: './ashx/Visualizacao/NormaDetalhes.ashx?id_norma=' + ch_norma,
                sType: "GET",
                fnSuccess: sucesso,
                fnComplete: null,
                fnBeforeSend: null,
                fnError: null,
                bAsync: true
            });
        }
</script>
<style type="text/css">
    div.table div.head div.title{width:100%}
    div.table div.head div.title h2{font-size: 18px;}
    .div_conteudo_arquivo p { margin-bottom: 10px; font-size: 13px; }
    .div_conteudo_arquivo .remover { background-color: #ffa9a9; }
    .div_conteudo_arquivo .desfazer { background-color: #b4f3b4; }
        
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Excluir Vide</label>
	</div>
    <div class="form">
        <div id="div_cad_vide">
            <form id="form_vide_excluir" name="formExcluirVide" action="#" method="post">
                <input type="hidden" id="dt_controle_alteracao" name="dt_controle_alteracao" value="<%= DateTime.Now.ToString("dd'/'MM'/'yyyy HH':'mm':'ss") %>" />
                <input type="hidden" id="id_doc" name="id_doc" value="" />
                <input type="hidden" id="vide" name="vide" value="" />
                <div id="div_vide" class="loaded">
                    <fieldset class="w-95-pc">
                        <div id="div_notificacao_vide" class="w-50-pc mauto h-45-px"></div> 
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="table w-100-pc">
                                        <div class="line">
                                            <div id="columnRelacao" class="column w-100-pc text-center bold">
                                                <label>Tipo de Relação:</label>
                                                <input type="text" id="tipoDeRelacao" value="" disabled="disabled" />
                                            </div>
                                        </div>
                                        <div class="legenda-excluir-vide">
                                            <p class="desfazer">O dispositivo terá a modificação desfeita.</p>
                                            <p class="refazer">O dispositivo voltará a sofrer a alteração que sofria antes do vide.</p>
                                            <p class="remover">O dispositivo será removido junto com o vide.</p>
                                        </div>
                                        <div id="lineNormas" class="line">
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
                                                                    <label id="label_norma_vide_alteradora"></label>
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
                                                                    <input type="text" id="dt_inicio_vigor" value="" disabled="disabled" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alteradora" style="display:none;">
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
                                            <div id="columnNormaAlterada" class="column w-50-pc">
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
                                                                    <label id="label_norma_vide_alterada"></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="line_norma_fora_do_sistema" class="line hidden">
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
                                                                                <input type="text" id="nm_tipo_norma_vide_fora_do_sistema" value="" class="w-80-pc" disabled="disabled" />
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
                                                                                <input type="text" id="nr_norma_vide_fora_do_sistema" value="" disabled="disabled" />
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
                                                                                <input type="text" id="nm_tipo_fonte_vide_fora_do_sistema" value="" class="w-80-pc" disabled="disabled" />
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
                                                                                <input type="text" id="dt_publicacao_norma_vide_fora_do_sistema" value="" class="date" disabled="disabled" />
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
                                                                                <input type="text" id="nr_pagina_publicacao_norma_vide_fora_do_sistema" value="" disabled="disabled" />
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
                                                                                <input type="text" id="nr_coluna_publicacao_norma_vide_fora_do_sistema" value="" disabled="disabled" />
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
                                                                                <input type="text" id="artigo_norma_vide_alterada" value="" disabled="disabled" />
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
                                                                                <input type="text" id="paragrafo_norma_vide_alterada" value="" disabled="disabled" />
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
                                                                                <input type="text" id="inciso_norma_vide_alterada" value="" disabled="disabled" />
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
                                                                                <input type="text" id="alinea_norma_vide_alterada" value="" disabled="disabled" />
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
                                                                                <input type="text" id="item_norma_vide_alterada" value="" disabled="disabled" />
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
                                                                                <input type="text" id="anexo_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alterada" style="display:none;">
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
                                        </div>
                                        <div id="lineComentario" class="line comentario">
                                            <div class="column w-100-pc">
                                                <div class="table w-80-pc mauto">
                                                    <div class="line">
                                                        <div class="column w-100-pc">
                                                            Comentário:
                                                        </div>
                                                        <div class="column w-100-pc">
                                                            <textarea id="ds_comentario_vide" class="w-100-pc" rows="5" cols="50" disabled="disabled"></textarea>
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
                    <div id="divButtons" class="buttons loaded text-center">
                        <button id="button_salvar_vide">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_trash_p.png" alt="escluir" />Excluir
                        </button>
                        <button type="button" id="button_cancelar" onclick="javascript:voltar()" >
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" alt="cancelar" />Cancelar
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
    </div>
</asp:Content>
