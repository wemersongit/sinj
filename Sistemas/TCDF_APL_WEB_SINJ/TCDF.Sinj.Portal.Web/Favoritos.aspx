<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true"
    CodeBehind="Favoritos.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.highlight.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>    
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/sessao_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        function PesquisarFavoritos(nm_base) {
            if (nm_base == "favoritos_norma") {
                $('#div_favoritos').dataTablesLight({
                    responsive: null,
                    sAjaxUrl: './ashx/Datatable/FavoritosDatatable.ashx?tipo_pesquisa=favoritos&b=norma',
                    aoColumns: _columns_norma_favoritos,
                    sIdTable: 'datatable_favoritos',
                    sSearch: "Filtrar Favoritos:",
                    bFilter: true,
                    bSorting: false,
                    aLengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
                    fnCreatedRow: function (nRow, aData, iDataIndex) {
                        aplicarHighlight('.', aData.highlight, nRow);
                    }
                });
            }
        }
        $(document).ready(function () {
            if (!IsNotNullOrEmpty(_notifiqueme)) {
                document.location.href = "./LoginNotifiqueme.aspx?back=1&message=Realize login para visualizar seus favoritos.&type=alert";
            }
            else {
                PesquisarFavoritos("favoritos_norma");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div>
        <div class="divIdentificadorDePagina">
            <label>
                .: Meus Favoritos</label>
        </div>
        <div id="div_loading_favoritos" class="loading" style="display: none;"></div>
        <div id="div_favoritos" style="padding:30px;">
            
        </div>
    </div>
    <div id="modal_detalhes_norma" class="modal form" style="display:none;">
        <div class="form">
            <form id="form_highlight" name="form_highlight" method="post" action="#" onsubmit="javascript:return fn_submit_highlight()">
                <div id="div_norma" class="loaded">
                    <fieldset class="w-90-pc">
                        <div class="mauto table">
                            <div class="w-100-pc fl" style="margin-top:10px;">
                                <div class="fr" style="margin-right:5px;margin-left:15px;">
                                    <a href="javascript:void(0);" onclick="javascript:Print();" class="fr" title="Imprimir"><img width="30" alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/></a>
                                </div>
                                <div class="fr" style="margin-right:5px;margin-left:15px;">
                                    <a id="a_adicionar_cesta" href="javascript:void(0);" class="fr" title="Adicionar na Cesta"><img width="27" alt="cesta" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_basket_p.png"/></a>
                                </div>
                                <div id="button_favoritos" class="fr" style="margin-right:5px;margin-left:15px;" ac="refresh">
                                </div>
                                <div id="button_notifiqueme" class="fr">
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc">
                                        <div class="w-90-pc mauto word-no-break nr_norma nr_norma_text nm_tipo_norma dt_assinatura dt_assinatura_text">
                                            <button type="submit" id="button_identificacao" title='Ir para a tela de Detalhes da Norma' class='campo link'>
                                            </button>
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
                                        <div id="div_arquivo" class="w-90-pc mauto campo">
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
                                        <div id="div_ds_ementa" class="w-90-pc mauto word-no-break campo ds_ementa" style="text-align:justify;">
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
                                        <div id="div_ds_origem" class="w-90-pc mauto word-no-break campo ds_orgao sg_orgao" style="text-align:justify;">
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
                                                    <th>Tipo</th>
                                                    <th>Edição</th>
                                                    <th>Tipo de Publicação</th>
                                                    <th>Data Pub.</th>
                                                    <th>Página</th>
                                                    <th>Coluna</th>
                                                    <th>Observação</th>
                                                    <th>Motivo De Republicação</th>
                                                    <th>Arquivo</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tbody_fontes" class="campo">
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="line_bottom"></div>
                            </div>
                            <div id="line_vides_normas_que_afetam" class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc">
                                        <table class="w-90-pc mauto">
                                            <caption>Vides - Normas que afetam </caption>
                                            <thead>
                                                <tr>
                                                    <th>Relação</th>
                                                    <th>Dispositivo Afetado</th>
                                                    <th></th>
                                                    <th>Norma</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tbody_vides_normas_que_afetam" class="campo">
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="line_bottom"></div>
                            </div>
                            <div id="line_vides_normas_afetadas" class="line">
                                <div class="column w-100-pc">
                                    <div class="cell w-100-pc">
                                        <table class="w-90-pc mauto">
                                            <caption>Vides - Normas afetadas</caption>
                                            <thead>
                                                <tr>
                                                    <th>Relação</th>
                                                    <th>Dispositivo Afetado</th>
                                                    <th></th>
                                                    <th>Norma</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tbody_vides_normas_afetadas" class="campo">
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
                                        <div id="div_observacao" class="w-90-pc mauto word-no-break campo observacao" style="text-align:justify;"">
                                        </div>
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
                                        <div id="div_nome" class="w-90-pc mauto campo nome">
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
                                        <div id="div_indexacao" class="w-90-pc mauto campo nm_termo">
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
                                        <div id="div_nm_ambito" class="w-90-pc mauto campo nm_ambito">
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
                                        <div id="div_nm_apelido" class="w-90-pc mauto campo nm_apelido">
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
                                        <div id="div_autorias" class="w-90-pc mauto campo nm_autoria">
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
                                        <div id="div_links" class="w-90-pc mauto campo">
                                        </div>
                                    </div>
                                </div>
                                <div class="line_bottom"></div>
                            </div>
                        </div>
                    </fieldset>
                </div>

                <div id="div_notificacao_norma" class="notify" style="display:none;"></div>
                <div id="div_loading_norma" class="loading" style="display:none;"></div>
                <input type="hidden" name="highlight" id="highlight" value="" />
            </form>
        </div>
    </div>
</asp:Content>
