<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarDiario.aspx.cs" Inherits="TCDF.Sinj.Web.EditarDiario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_diario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_diario').click(function () {
                return fnSalvar("form_diario");
            });
            ConstruirControlesDinamicos();
            PreencherDiarioEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Diário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_diario" name="formEdicaoDiario" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_diario">
                <fieldset>
                    <div id="div_notificacao_diario" class="notify" style="display:none;"></div>
                    <!--<legend>Diário</legend>-->
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
                                <div class="cell w-60-pc">
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
                                    <label>Pendente de Revisão:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input type="checkbox" label="Pendente de Revisão" id="st_pendente" name="st_pendente" value="true" onclick="javascript:toggleCheckedShowHide('#st_pendente', '#ds_pendencia')" />
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
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_diario" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_diario">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_diario" class="loading" style="display:none;"></div>
        </form>
    </div>
        <!-- FORMULÁRIO PARA ANEXAR ARQUIVO -->
    <div id="div_upload_file" style="display:none;">
        <form id="form_upload_file" name="form_upload_file" action="#" url-ajax="./ashx/Arquivo/UploadFile.ashx" method="post">
            <input type="hidden" name="nm_base" value="sinj_diario"/>
            <input type="file" name="file" onchange="javascript:salvarArquivoSelecionadoDiario(this)" />
        </form>
    </div>
</asp:Content>
