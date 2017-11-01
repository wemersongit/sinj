<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarDiario.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_diario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_diario').click(function () {
                return fnSalvar("form_diario");
            });
            ConstruirControlesDinamicos();
            
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div id="slave" class="on-show-top">
        <div class="divIdentificadorDePagina">
	        <label>.: Cadastrar Diário</label>
	    </div>
        <div id="div_controls" class="control">
        </div>
        <div class="form">
            <form id="form_diario" name="formCadastroDiario" action="#" method="post">
                <div id="div_diario">
                    <div id="div_identificacao">
                        <br />
                            <div id="div_notificacao_diario" class="notify w-80-pc mauto" style="display:none;"></div>
                        <fieldset>
                            <legend>Diário</legend>
                            <div class="mauto table">
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Tipo:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                    <div id="div_autocomplete_tipo_fonte" class="cell w-60-pc">
                                        <input id="ch_tipo_fonte" name="ch_tipo_fonte" type="hidden" value="" obrigatorio="sim" label="Tipo de Diário" />
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
                                        <input id="ch_tipo_edicao" name="ch_tipo_edicao" type="hidden" value="" obrigatorio="sim" label="Tipo de Edição" />
                                        <input id="nm_tipo_edicao" name="nm_tipo_edicao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_edicao"></a>
                                    </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Diferencial Edição:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input type="text" id="nm_diferencial_edicao" name="nm_diferencial_edicao" value="" class="w-20-pc"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Número:</label>
                                        </div>
                                    </div>
                                    <div class="column w-20-pc">
                                        <div class="cell w-80-pc">
                                            <input id="nr_diario" name="nr_diario" type="number" value="" class="w-100-pc" obrigatorio="sim" label="Número" />
                                        </div>
                                    </div>
                                    <div class="column">
                                        <div class="cell fr">
                                            <label>Letra:</label>
                                        </div>
                                    </div>
                                    <div class="column w-20-pc">
                                        <div class="cell w-50-pc">
                                            <input id="cr_diario" name="cr_diario" type="text" value="" class="w-50-pc"/>
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
                                            <label>Data de Publicação:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input label="Data" obrigatorio="sim" id="dt_assinatura" name="dt_assinatura" type="text" value="" class="w-30-pc date" />
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <label><input type="checkbox" id="st_suplemento" name="st_suplemento" value="true" onclick="javascript:toggleCheckedShowHide('#st_suplemento', '#line_nm_diferencial_suplemento')" style="vertical-align:middle;" /> Suplemento</label>
                                        </div>
                                    </div>
                                </div>
                                <div id="line_nm_diferencial_suplemento" class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Diferencial Suplemento:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input type="text" id="nm_diferencial_suplemento" name="nm_diferencial_suplemento" value="" class="w-20-pc"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                             <label><input type="checkbox" label="Pendente de Revisão" id="st_pendente" name="st_pendente" value="true" onclick="javascript:toggleCheckedShowHide('#st_pendente', '#line_ds_pendencia');" style="vertical-align:middle;"/> Pendente de Revisão</label>
                                        </div>
                                    </div>
                                </div>
                                <div id="line_ds_pendencia" class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Observação:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-80-pc">
                                            <input type="text" label="Pendente de Revisão" id="ds_pendencia" name="ds_pendencia" value="" class="w-80-pc"/>
                                        </div>
                                    </div>
                                </div>
								<div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                            <label>Arquivos:</label>
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div id="div_nr_arquivos" class="cell w-100-pc">
                                            <a href="javascript:void(0);" onclick="javascript:anexarInputFileDiario();" class="attach" style="display: inline;"><img valign="absmiddle" alt="Anexar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_attach_p.png"/></a>
                                        </div>
                                    </div>
                                </div>
								<div class="line">
                                    <div class="column w-20-pc">
                                        <div class="cell fr">
                                        </div>
                                    </div>
                                    <div class="column w-70-pc">
                                        <div class="cell w-100-pc">
                                            <table id="table_arquivos" class="w-100-pc">
                                                <thead>
                                                    <tr>
                                                        <th>Arquivo</th>
                                                        <th style="width:180px;">Descrição</th>
                                                        <th style="width:30px;">Remover</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr class="tr_vazia">
                                                        <td colspan="3">Nenhum arquivo</td>
                                                    </tr>
                                                    <%--<tr nr="1">
                                                        <td>
                                                            <input type="hidden" name="json_arquivo_diario" class="json_arquivo" value=""/>
                                                        </td>
                                                        <td>
                                                            <input type="text" name="ds_arquivo_diario" value="" />
                                                        </td>
                                                        <td class="text-center">
                                                            <button class="clean" title="Remover arquivo" type="button" onclick="javascript:deletarArquivoDiario(this);"><img valign="absmiddle" alt="Excluir" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" /></button>
                                                        </td>
                                                    </tr>--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div style="width:210px; margin:auto;" class="loaded pos_cad">
                    <button id="button_salvar_diario">
                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_save.png" alt="add" width="18px" height="18px" />Salvar
                    </button>
                    <button type="reset">
                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" alt="cancela" width="18px" height="18px" />Limpar
                    </button>
                </div>
                <div id="div_loading_diario" class="loading" style="display:none;"></div>
            </form>
        </div>
    </div>
    <!-- FORMULÁRIO PARA ANEXAR ARQUIVO -->
    <div id="div_upload_file" style="display:none;">
        <form id="form_upload_file" name="form_upload_file" action="#" url-ajax="./ashx/Arquivo/UploadFile.ashx" method="post">
            <input type="hidden" name="nm_base" value="sinj_diario"/>
            <input type="file" name="file" onchange="javascript:salvarArquivoSelecionadoDiario(this)" />
        </form>
    </div>
</asp:Content>
