<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeNorma.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.DetalhesDeNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <asp:PlaceHolder runat="server" id="placeHolderHeader"></asp:PlaceHolder>  
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.highlight.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%=TCDF.Sinj.Util.MostrarVersao()%>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.fileDownload.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <script type="text/javascript" language="javascript">
        <%
            var json_norma = new TCDF.Sinj.Portal.Web.ashx.Visualizacao.NormaDetalhes().GetNormaDetalhes(HttpContext.Current);
            var highlight = HttpUtility.UrlDecode(Request.Form["highlight"]);
            var success_message = Request["success_message"];
        %>
        var json_norma = <%= json_norma != "" ? json_norma : "\"\"" %>;
        var highlight = <%= !string.IsNullOrEmpty(highlight) ? highlight : "\"\"" %>;
        var success_message = "<%= success_message %>";
        var date_now = '<%= DateTime.Now.ToString("dd/MM/yyyy") %>';

        $(document).ready(function () {
        	cookies_aux = ['back_history_aux_datatable','back_history_aux_filtros','back_history_aux_tab'];
            DetalhesNorma(json_norma, highlight);
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
    <script type="text/javascript" async> !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } } (document, 'script', 'twitter-wjs');</script>
    <script type="text/javascript" async> (function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (d.getElementById(id)) return; js = d.createElement(s); js.id = id; js.src = "//connect.facebook.net/pt_BR/sdk.js#xfbml=1&version=v2.5"; fjs.parentNode.insertBefore(js, fjs); } (document, 'script', 'facebook-jssdk'));</script>
    <script type="text/javascript" src="https://apis.google.com/js/platform.js" async defer>{ lang: 'pt-BR' }</script>
    <script type="text/javascript" src="//platform.linkedin.com/in.js" async> lang: pt_BR</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="w-100-pc">
        <div class="divIdentificadorDePagina fl">
	        <label>.: Detalhes de Norma</label>
	    </div>
    </div>
    <div id="div_controls" class="control cb">
    </div>
    <div class="form">
        <div id="div_norma" class="loaded">
            <div id="div_notificacao_norma" class="notify w-90-pc mauto" style="display:none;"></div>
            <fieldset class="w-90-pc">
                <div class="mauto table">
                    <div class="w-90-pc mauto" style="margin-top:10px;">
                        <div id="div_redes_sociais" class="fl">
                            <ul>
                                <li>
                                    <div id="fb-root"></div>
                                    <div class="fb-share-button" data-href="" data-layout="button"></div>
                                </li>
                                <li>
                                    <script type="IN/Share"></script>
                                </li>
                                <li>
                                    <a href="https://twitter.com/share" class="twitter-share-button" data-lang="pt" data-text=""></a>
                                </li>
                                <li>
                                    <div class="g-plus" data-action="share" data-annotation="none"></div>
                                </li>
                            </ul>
                        </div>
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
                                <div id="div_identificacao" class="w-90-pc mauto word-no-break nr_norma nr_norma_text nm_tipo_norma dt_assinatura dt_assinatura_text value">
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
                    
                    <div id="line_dt_inicio_vigencia" class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc mauto">
                                <div class="w-90-pc mauto">
                                    <label>Vacatio Legis:</label>
                                </div>
                            </div>
                        </div><br />
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_ds_vacatio_legis" class="w-90-pc mauto value" style="text-align:justify;" print="sim" print-type="linha" print-label="Vacatio Legis">
                                    Entrará em vigor a partir de <span id="span_dt_inicio_vigencia" class="dt_inicio_vigencia"></span>
                                </div>
                            </div>
                        </div>
                        <div class="line_bottom"></div>
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
                                <div id="div_ds_ementa" class="w-90-pc mauto word-no-break ds_ementa value" style="text-align:justify;">
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
                                <div id="div_ds_origem" class="w-90-pc mauto word-no-break ds_orgao sg_orgao value" style="text-align:justify;">
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
                                <table class="w-90-pc mauto word-no-break">
                                    <thead>
                                        <tr>
                                            <th>Diário</th>
                                            <th>Tipo de Publicação</th>
                                            <th>Página</th>
                                            <th>Coluna</th>
                                            <th>Observação</th>
                                            <th>Motivo De Republicação</th>
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
                                <table class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>Relação</th>
                                            <th>Dispositivo Afetado</th>
                                            <th></th>
                                            <th>Norma</th>
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
                                <table class="w-90-pc mauto">
                                    <thead>
                                        <tr>
                                            <th>Relação</th>
                                            <th>Dispositivo Afetado</th>
                                            <th></th>
                                            <th>Norma</th>
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
                                <div id="div_observacao" class="w-90-pc mauto word-no-break observacao value" style="text-align:justify;"">
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
                                <div id="div_nome" class="w-90-pc mauto nome value">
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
                                <div id="div_indexacao" class="w-90-pc mauto nm_termo value">
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
                                <div id="div_nm_apelido" class="w-90-pc mauto nm_apelido value">
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
                                <div id="div_autorias" class="w-90-pc mauto nm_autoria value">
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
                                <div id="ds_efeito_decisao" class="w-90-pc mauto value">
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
                </div>
            </fieldset>
        </div>
        <div id="div_loading_norma" class="loading" style="display:none;"></div>
        <div style="display:none;">
            <div id="div_impressao"></div> 
        </div>
    </div>
</asp:Content>
