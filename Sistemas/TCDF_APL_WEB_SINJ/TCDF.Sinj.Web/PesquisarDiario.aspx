<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarDiario.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_diario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_pesquisar_diario').click(function () {
                return Pesquisar('form_pesquisa_diario');
            });
            ConstruirControlesDinamicos();
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Diário</label>
	</div>
    <div id="div_controls" class="control"></div>
    <div class="form">
        <form id="form_pesquisa_diario" name="formPesquisaDiario" action="./ResultadoDePesquisaDiario.aspx" method="get">
            <div id="div_pesquisa">
                <fieldset>
                    <!--<legend>Pesquisa</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Tipo:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_autocomplete_tipo_fonte" class="cell w-60-pc">
                                    <input id="ch_tipo_fonte" name="ch_tipo_fonte" type="hidden" value="" />
                                    <input id="nm_tipo_fonte" name="nm_tipo_fonte" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_fonte"></a>
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
                                    <input name="nr_diario" type="text" value="" class="w-30-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Letra:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="cr_diario" type="text" value="" class="w-10-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Seção:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <label style="margin-right:10px;"><input type="checkbox" name="secao_diario" value="1" style="vertical-align:middle;"/> 1</label>
                                    <label style="margin-right:10px;"><input type="checkbox" name="secao_diario" value="2" style="vertical-align:middle;"/> 2</label>
                                    <label style="margin-right:10px;"><input type="checkbox" name="secao_diario" value="3" style="vertical-align:middle;"/> 3</label>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Pendente de Revisão:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-80-pc">
                                    <input name="st_pendente" type="checkbox" value="true" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>Data de Publicação:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-90-pc">
                                    <div class="w-100-pc fl">
                                        <select class="fl" id="op_dt_assinatura" name="op_dt_assinatura" onchange="javascript:SelecionarOperador();">
                                            <option value="igual">igual</option>
                                            <option value="menor">menor</option>
                                            <option value="menorouigual">menor ou igual</option>
                                            <option value="maior">maior</option>
                                            <option value="maiorouigual">maior ou igual</option>
                                            <option value="diferente">diferente</option>
                                            <option value="intervalo">intervalo</option>
                                        </select>
                                        <input name="dt_assinatura" type="text" value="" class="date fl" style="margin-left:5px; width:100px;" />
                                        <div id="div_intervalo" class="w-30-pc fl" style="display:none; margin-left:5px;">
                                            até
                                            <input name="dt_assinatura_intervalo" type="text" value="" class="date" style="margin-left:5px; width:100px;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="div_diario_buttons" style="width:220px; margin:auto;" class="loaded">
                            <button id="button_pesquisar_diario">
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
            <div id="div_loading_diario" class="loading" style="display:none;"></div>
            <div id="div_notificacao_diario" class="notify" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
