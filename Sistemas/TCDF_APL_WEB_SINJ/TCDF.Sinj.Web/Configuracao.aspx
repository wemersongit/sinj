<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Configuracao.aspx.cs" Inherits="TCDF.Sinj.Web.Configuracao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_configuracao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            DetalhesConfiguracao();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Configuração</label>
	</div>
    <div class="form">
        <div id="div_configuracao">
            <fieldset>
                <!--<legend>Configuração</legend>-->
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nome de Usuário:</label>
                            </div>
                        </div>
                        <div class="column w-30-pc">
                            <div class="cell w-100-pc">
                                <div id="div_nm_usuario"></div>
                            </div>
                        </div>
                    </div>
                    <div id="line_senha" class="line" style="display:none;">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Senha:</label>
                            </div>
                        </div>
                        <div class="column w-30-pc">
                            <div class="cell w-100-pc">
                                <div id="div_senha"></div>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>E-mail:</label>
                            </div>
                        </div>
                        <div class="column w-30-pc">
                            <div class="cell w-100-pc">
                                <div id="div_email_usuario"></div>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Página Inicial:</label>
                            </div>
                        </div>
                        <div class="column w-30-pc">
                            <div class="cell w-100-pc">
                                <div id="div_pagina_inicial"></div>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Tema de Cores:</label>
                            </div>
                        </div>
                        <div class="column w-30-pc">
                            <div class="cell w-100-pc">
                                <div id="div_ch_tema"></div>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-90-pc">
                            <div class="cell w-100-pc">
                                <table id="table_grupos" class="w-60-pc mauto" style="text-align:center;">
                                    <caption>Permissões</caption>
                                    <thead>
                                        <tr>
                                            <th>Formulários / Ações</th>
                                            <th>Cadastrar</th>
                                            <th>Editar</th>
                                            <th>Pesquisar</th>
                                            <th>Visualizar</th>
                                            <th>Excluir</th>
                                            <th>Gerenciar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div id="div_notificacao_configuracao" class="notify" style="display:none;"></div>
        <div id="div_loading_configuracao" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
