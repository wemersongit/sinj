<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeNorma.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.highlight.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <script type="text/javascript" language="javascript">
        <%
            var data = new TCDF.Sinj.Web.ashx.Visualizacao.NormaDetalhes().GetNormaDetalhes(HttpContext.Current);
            var highlight = HttpUtility.UrlDecode(Request.Form["highlight"]);
            var success_message = Request["success_message"];
        %>
        var data = <%= data != "" ? data : "\"\"" %>;
        var highlight = <%= !string.IsNullOrEmpty(highlight) ? highlight : "\"\"" %>;
        var success_message = "<%= success_message %>";

        $(document).ready(function () {
        	cookies_aux = ['back_history_aux_datatable','back_history_aux_filtros','back_history_aux_tab'];
            DetalhesNorma(data, highlight);
            if(IsNotNullOrEmpty(success_message)){
                $('#div_norma .notify').messagelight({
                    sContent: success_message,
                    sType: "success"
                });
            }
        });
        function Print() {
            preencherImpressao();
            $('#div_impressao').printElement({pageTitle: $('#div_identificacao').text() +'.html'});
            $('#div_impressao').html("");
        }

    </script>
    <style type="text/css">
        .line_bottom {clear: both;border-bottom: 1px solid #DDD; width: 90%; margin: auto; padding-top: 10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="w-100-pc">
        <div class="divIdentificadorDePagina fl">
	        <label>.: Detalhes de Norma</label>
	    </div>
    </div>
    <div id="div_controls" class="control cb">
    </div>
    <div id="modal_arquivo" style="display:none;">
    </div>
    <div id="modal_confirmar_excluir_vide" style="display:none;">
    </div>
    <div class="form">
        <div id="modal_texto" style="display:none;">
            <div class="loading" style="display:none;"></div>
            <div class="notify" style="display:none;"></div>
            <div class="text"></div>
        </div>
        <div id="div_norma" class="loaded">
            <div class="mauto table w-90-pc">
                <div class="line">
                    <div class="column w-100-pc" style="padding-top:0px !important;">
                        <div id="div_controls_detalhes" class="cell fr">
                        </div>
                    </div>
                </div>
            </div>
            <div id="div_notificacao_norma" class="notify w-90-pc mauto" style="display:none;"></div>
            <fieldset class="w-90-pc">
                <div class="mauto table">
                    <div class="w-100-pc fr" style="margin-top:10px;">
                        <div class="fr" style="margin-right:5px;margin-left:15px;">
                            <a href="javascript:void(0);" onclick="javascript:Print();" class="fr" title="Imprimir"><img width="25" alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/></a>
                        </div>
                        <div class="fr" style="margin-right:5px;margin-left:15px;">
                            <a id="a_cesta" href="javascript:void(0);" class="fr" title=""></a>
                        </div>
                        <div id="button_favoritos" class="fr" style="margin-right:5px;margin-left:15px;">
                        </div>
                        <div id="button_notifiqueme" class="fr">
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_pendencia" class="w-90-pc mauto value" print="sim" print-type="linha" print-label="Pendência">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_identificacao" class="w-90-pc mauto word-no-break nr_norma nr_norma_text nm_tipo_norma dt_assinatura dt_assinatura_text value" style="font-size:18px;" print="sim" print-type="linha" print-label="Identificação">
                                </div>
                            </div>
                        </div>
                    </div>                    
                    <div id="div_url_acompanhamento" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <H4></H4>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="line_arquivo" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc mauto">
                                <div class="w-90-pc mauto">
                                    <label>Baixar Arquivo:</label>
                                </div>
                            </div>
                        </div><br />
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_arquivo" class="w-90-pc mauto value">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc mauto">
                                <div class="w-90-pc mauto">
                                    <label>Ementa:</label>
                                </div>
                            </div>
                        </div><br />
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_ds_ementa" class="w-90-pc mauto word-no-break ds_ementa value" style="text-align:justify;" print="sim" print-type="linha" print-label="Ementa">
                                    
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc mauto">
                                <div class="w-90-pc mauto">
                                    <label>Origem:</label>
                                </div>
                            </div>
                        </div><br />
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_ds_origem" class="w-90-pc mauto word-no-break ds_orgao sg_orgao value" style="text-align:justify;" print="sim" print-type="linha" print-label="Origem">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_fontes" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Fontes:</label>
                                </div>
                                <table class="w-90-pc mauto word-no-break" print="sim" print-type="tabela" print-label="Fontes" print-table-id="table_fontes">
                                    <thead>
                                        <tr>
                                            <th>Diário</th>
                                            <th>Tipo de Publicação</th>
                                            <th>Página</th>
                                            <th>Coluna</th>
                                            <th>Observação</th>
                                            <th>Motivo De Republicação</th>
                                            <%
                                                if (TCDF.Sinj.Util.EhCadastro())
                                                {
                                            %>
                                            <th>Arquivo Original</th>
                                            <%
                                                }
                                            %>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_fontes">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_vides_normas_que_afetam" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Vides - Normas que afetam:</label>
                                </div>
                                <table class="w-90-pc mauto" print="sim" print-type="tabela" print-label="Vides - Normas que afetam" print-table-id="table_vides_normas_que_afetam">
                                    <thead>
                                        <tr>
                                            <th>Relação</th>
                                            <th>Dispositivo Afetado</th>
                                            <th></th>
                                            <th>Norma</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_vides_normas_que_afetam">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_vides_normas_afetadas" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Vides - Normas afetadas:</label>
                                </div>
                                <table class="w-90-pc mauto" print="sim" print-type="tabela" print-label="Vides - Normas afetadas" print-table-id="table_vides_normas_afetadas">
                                    <thead>
                                        <tr>
                                            <th>Relação</th>
                                            <th>Dispositivo Afetado</th>
                                            <th></th>
                                            <th>Norma</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_vides_normas_afetadas">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_observacao" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Observação:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_observacao" class="w-90-pc mauto word-no-break observacao value" style="text-align:justify;" print="sim" print-type="linha" print-label="Observação">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_interessados" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Interessados:</label>
                                </div>
                                <table id="table_interessados" class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nome
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_interessados" class="nm_interessado">
                                        <tr class="tr_vazia">
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_nomes" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Nomes:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_nome" class="w-90-pc mauto nome value" print="sim" print-type="linha" print-label="Nomes">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_indexacoes" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Indexação:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_indexacao" class="w-90-pc mauto nm_termo value" print="sim" print-type="coluna" print-label="Indexação">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_ambito" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Âmbito:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_nm_ambito" class="w-90-pc mauto nm_ambito value">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_nm_apelido" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Apelido:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_nm_apelido" class="w-90-pc mauto nm_apelido value" print="sim" print-type="linha" print-label="Apelido">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="line_autorias" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Autoria:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_autorias" class="w-90-pc mauto nm_autoria value" print="sim" print-type="linha" print-label="Autoria">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_decisoes" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Decisões:</label>
                                </div>
                                <table id="table_decisoes" class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>
                                                Tipo
                                            </th>
                                            <th>
                                                Complemento
                                            </th>
                                            <th>
                                                Data de Publicação
                                            </th>
                                        </tr> 
                                    </thead>
                                    <tbody id="tbody_decisoes">
                                        <tr class="tr_vazia">
                                            <td colspan="3"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>                    
                    <div id="div_procedencia" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Procedência:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="ds_procedencia" class="w-90-pc mauto value">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_parametro_constitucional" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Parâmetro Constitucional:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="ds_parametro_constitucional" class="w-90-pc mauto value">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_requerentes" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Requerentes:</label>
                                </div>
                                <table id="table_requerentes" class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nome
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_requerentes" class="nm_requerente">
                                        <tr class="tr_vazia">
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_requeridos" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Requeridos:</label>
                                </div>
                                <table id="table_requeridos" class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nome
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_requeridos" class="nm_requerido">
                                        <tr class="tr_vazia">
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_procuradores" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Procuradores Responsáveis:</label>
                                </div>
                                <table id="table_procuradores" class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nome
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_procuradores">
                                        <tr class="tr_vazia">
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_relatores" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Relatores:</label>
                                </div>
                                <table id="table_relatores" class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>
                                                Nome
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody_relatores" class="nm_relator">
                                        <tr class="tr_vazia">
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_texto_da_acao" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Texto da Ação:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_ds_texto_da_acao" class="w-90-pc mauto value">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div id="div_efeito_decisao" class="line dados-das-acoes">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Efeito da Decisão:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="ds_efeito_decisao" class="w-90-pc mauto">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div class="w-90-pc mauto">
                                    <label>Links:</label>
                                </div>
                            </div>
                        </div>
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_links" class="w-90-pc mauto">
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
                    </div>
					<div class="line no-hide">
	                    <div class="column w-100-pc">
	                        <div class="cell w-100-pc" >
                                <div class="w-90-pc mauto text-center" style="background-color:#CCC;">
	                                <a href="javascript:void(0);" onclick="javascript:ExpandirDadosDeCadastro();" class="expansible closed" >Cadastro <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                        	    <div id="div_detalhes_cadastro" style="display:none;"> 
									    <span id="span_nome_usuario_cadastro"></span>
									    <span id="span_data_cadastro"> -  </span>
								    </div>
                                </div>
	                        </div>
	                    </div>
	                </div>
					<div class="line no-hide">
	                    <div class="column w-100-pc">
	                        <div class="cell w-100-pc" >
                                <div class="w-90-pc mauto text-center" style="background-color:#CCC;">
	                                <a href="javascript:void(0);" onclick="javascript:ExpandirAlteracoes();" class="expansible closed" >Alterações <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_arrow_down.png"/></a>
	                        	    <div id="div_alteracoes" style="display:none;"> 
								    </div>
	                            </div>
                            </div>
	                    </div>
	                </div>
                </div>
            </fieldset>
        </div>
        <div id="div_loading_norma" class="loading" style="display:none;"></div>
        <div style="display:none;">
            <div id="div_impressao"></div> 
        </div>
    </div>
</asp:Content>
