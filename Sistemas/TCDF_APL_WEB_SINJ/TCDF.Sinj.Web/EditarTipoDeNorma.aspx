<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarTipoDeNorma.aspx.cs" Inherits="TCDF.Sinj.Web.EditarTipoDeNorma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_tipo_de_norma.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_tipo_de_norma').click(function () {
                return fnSalvar("form_tipo_de_norma");
            });
            ConstruirOrgaosCadastradores();
            PreencherTipoDeNormaEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Tipo de Norma</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_tipo_de_norma" name="formEdicaoTipoDeNorma" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_tipo_de_norma">
                <fieldset class="w-60-pc">
                    <!--<legend>Tipo de Norma</legend>-->
                    <div id="div_notificacao_tipo_de_norma" class="notify" style="display:none;"></div>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome do Tipo de Norma:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_tipo_norma" name="nm_tipo_norma" type="text" value="" class="w-90-pc" />
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
                                    <textarea label="Descrição" id="ds_tipo_norma" name="ds_tipo_norma" class="w-90-pc" cols="100" rows="5" ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Siglas:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_siglas" class="cell w-60-pc">
                                    <button add title="Adicionar uma sigla" class="link" onclick="addSigla()"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" /></button>
                                    <!--<input id="sgs_tipo_norma" name="sgs_tipo_norma" type="text" value="" class="w-40-pc" />-->
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Órgão Cadastrador:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div id="div_orgaos_cadastradores" class="cell w-100-pc">
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Grupo 1:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_g1" type="checkbox" name="grupos" value="in_g1" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Grupo 2:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_g2" type="checkbox" name="grupos" value="in_g2" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Grupo 3:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_g3" type="checkbox" name="grupos" value="in_g3" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Grupo 4:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_g4" type="checkbox" name="grupos" value="in_g4" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Grupo 5:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_g5" type="checkbox" name="grupos" value="in_g5" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Conjunta:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_conjunta" type="checkbox" name="in_conjunta" value="true" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Questionável:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_questionavel" type="checkbox" name="in_questionavel" value="true" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Numeração por órgão:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_numeracao_por_orgao" type="checkbox" name="in_numeracao_por_orgao" value="true" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Apelidável:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-100-pc">
                                    <input id="in_apelidavel" type="checkbox" name="in_apelidavel" value="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_tipo_de_norma" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_tipo_de_norma">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_tipo_de_norma" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
