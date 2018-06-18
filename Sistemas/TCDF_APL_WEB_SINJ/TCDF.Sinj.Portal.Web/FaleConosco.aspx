<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="FaleConosco.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.FaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_fale_conosco.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/sessao_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript">
        var _chamados = <%= sChamados %>;
        $(document).ready(function(){
                $("#div_datatable_fale_conosco_atendimento").dataTablesLight({
                    bServerSide: false,
                    aaSorting: [[0, "desc"]],
                    aoData: _chamados,
                    aoColumns: _columns_fale_conosco_atendimento,
                    sIdTable: 'datatable_fale_conosco_atendimento'
                });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Fale Conosco</label>
	</div>
    <div class="form">
        <div id="div_fale_conosco_atendimento">
            <fieldset class="w-90-pc">
                <div class="mauto table w-90-pc">
                    <div class="notify" style="display: none;"></div>
                    <p><br />Acompanhamento das mensagens trocadas através do nosso canal de atendimento.</p>
                </div>
                <div class="mauto table w-90-pc">
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell w-100-pc">
                                <div id="div_datatable_fale_conosco_atendimento">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                        
                <div id="div_detalhes_fale_conosco_atendimento" class="mauto table fale_conosco_atendimento">
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
