<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarNorma.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_pesquisar_norma').click(function () {
                return Pesquisar('form_pesquisa_norma');
            });
            ConstruirControlesDinamicos();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Norma</label>
	</div>
    <div id="div_controls" class="control"></div>
    <div class="form">
        <form id="form_pesquisa_norma" name="formPesquisaNorma" action="./ResultadoDePesquisaNorma.aspx" method="get">
            <div id="div_pesquisa">
                <fieldset>
                    <legend>Pesquisa</legend>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Tipo:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_tipo_norma" class="cell w-60-pc">
                                    <input id="ch_tipo_norma" name="ch_tipo_norma" type="hidden" value="" />
                                    <input id="nm_tipo_norma" name="nm_tipo_norma" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma"></a>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Tipo de Edição:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                            <div id="div_autocomplete_tipo_edicao" class="cell w-60-pc">
                                <input id="ch_tipo_edicao" name="ch_tipo_edicao" type="hidden" value="" label="Tipo de Edição" />
                                <input id="nm_tipo_edicao" name="nm_tipo_edicao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_edicao"></a>
                            </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Número:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="nr_norma" type="text" value="" class="w-20-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Ano de Assinatura:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="nr_ano" type="text" value="" class="w-20-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Origem:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_origem_modal" class="cell w-90-pc">
                                    <input id="ch_orgao" name="ch_orgao" type="hidden" value="" />
                                    <input id="sg_hierarquia_nm_vigencia" name="sg_hierarquia_nm_vigencia" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_origem"></a>
                                </div>
                            </div>
                        </div>
                        <div id="div_norma_buttons" style="width:220px; margin:auto;" class="loaded">
                            <button id="button_pesquisar_norma">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" />
                                Pesquisar
                            </button>
                            <button type="reset" >
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />
                                Limpar
                            </button>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_norma" class="loading" style="display:none;"></div>
            <div id="div_notificacao_norma" class="notify" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
