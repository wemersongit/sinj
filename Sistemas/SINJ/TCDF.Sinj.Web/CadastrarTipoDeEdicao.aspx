<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarTipoDeEdicao.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarTipoDeEdicao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_tipo_de_edicao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_tipo_de_edicao').click(function () {
                return fnSalvar("form_tipo_de_edicao");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Tipo de Edição</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_tipo_de_edicao" name="formCadastroTipoDeEdicao" action="#" method="post">
            <div id="div_tipo_de_edicao">
            <div id="div_notificacao_tipo_de_edicao" class="notify" style="display:none;"></div>
                <fieldset class="w-60-pc">
                    <legend>Tipo de Edição</legend>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome da Edição:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_tipo_edicao" name="nm_tipo_edicao" type="text" value="" class="w-90-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Descrição:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <textarea label="Descrição" id="ds_tipo_edicao" name="ds_tipo_edicao" class="w-90-pc" cols="100" rows="5" ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Ativo:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input type="checkbox" id="st_edicao" name="st_edicao" value="true" checked="checked" />
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div id="div_buttons_tipo_de_edicao" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_tipo_de_edicao">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/none.png" class="save" />Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/none.png" class="clear" />Limpar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_tipo_de_edicao" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
