<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Arquivos.aspx.cs" Inherits="TCDF.Sinj.Web.Arquivos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript"  src="<%= TCDF.Sinj.Util._urlPadrao %>/ckeditor/ckeditor.js"></script>
    <script type="text/javascript"  src="<%= TCDF.Sinj.Util._urlPadrao %>/ckeditor/adapters/jquery.js"></script>
    <script type="text/javascript"  src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_documentos.js"></script>

    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {
            var _diretorio = '<%= _diretorio %>';
            var nm_diretorio = _diretorio == "arquivos_orgao_cadastrador" ? _user.orgao_cadastrador.nm_orgao_cadastrador : '';
            $('#div_editor_file input[name="nm_arquivo"]').bind('keypress', function (event) {
                var regex = new RegExp("^[a-zA-Z0-9_ ]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            });

            $("#ch_arquivo_raiz").val(_diretorio);
            $("#span_titulo").text("Arquivos " + nm_diretorio);

            configureCkeditor();



            carregarArquivosProduzidos([_diretorio, "000shared"]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="content_geral w-90-pc">
        <div class="titulo_content">
            <h2>
                <span id="span_titulo"></span> 
            </h2>
        </div>
        <div id="div_arquivos" class="page_content div_arquivos">
            <input type="hidden" id="ch_arquivo_raiz" value="" />
            <div class="list_dir">
                <div>
                    <a class="open_dialog_folder_raiz" href="javascript:void(0);" onclick="javascript:openDialogFolder(this);" title="Criar Pasta" ch_folder_selected="raiz" nr_nivel_arquivo_selected="" >
                        <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_add_folder.png" />
                    </a>
                </div>
                <div id="div_list_dir" class="div_list_dir">
                </div>
            </div>
            <div class="list_arq">
                <div id="div_controls_folder" style="display:none;">
                    <a class="open_dialog_folder" href="javascript:void(0);" onclick="javascript:openDialogFolder(this);" title="Criar Pasta" ch_folder_selected="" nr_nivel_arquivo_selected="">
                        <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_add_folder.png" width="24px" height="24px" />
                    </a>
                    <a class="open_dialog_delete_folder" href="javascript:void(0);" onclick="javascript:openDialogDeleteFolder(this);" title="Excluir Pasta" ch_folder_selected="" nr_nivel_arquivo_selected="">
                        <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_del_folder.png" width="24px" height="24px" />
                    </a>
                    <a class="open_dialog_editor" href="javascript:void(0);" onclick="javascript:openDialogEditFile(this);" title="Criar Arquivo" ch_folder_selected="" nr_nivel_arquivo_selected="">
                        <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_editor_html.png" width="24px" height="24px" />
                    </a>
                    <a class="open_dialog_attach" href="javascript:void(0);" onclick="javascript:openDialogAttachFile(this);" title="Anexar Arquivo" ch_folder_selected="" nr_nivel_arquivo_selected="" div_attach="div_attach_file">
                        <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_attach_file.png" width="24px" height="24px" />
                    </a>
                </div>
                <div id="div_list_arq">
                </div>
            </div>
        </div>
    </div>
    <div id="div_modal_confirme" style="display:none;">
    </div>
    <div id="div_modal_folder" style="display:none;">
        <form id="form_add_folder" name="form_add_folder" action="#" url-ajax="./ashx/Cadastro/ArquivoIncluir.ashx" method="post" onsubmit="javascript:return  fnUploadDocument('div_modal_folder');">
            <input type="hidden" name="id_doc" value="" />
            <input type="hidden" name="nm_base" value="sinj_arquivo" />
            <input type="hidden" name="ch_arquivo_raiz" value="<%= _diretorio %>"/>
            <input type="hidden" name="ch_arquivo_superior" value=""/>
            <input type="hidden" name="nr_nivel_arquivo_superior" value=""/>
            <input type="hidden" name="nr_tipo_arquivo" value="0"/>
            <div class="table w-90-pc mauto">
                <div class="notify" style="display:none;"></div>
                <div class="line">
                    <div class="column w-30-pc">
                        <div class="cell">
                            <label>
                                *Nome:
                            </label>
                        </div>
                    </div>  
                    <div class="column w-70-pc">
                        <input type="text" name="nm_arquivo" obrigatorio="sim" label="Nome" class="w-100-pc" />
                    </div>
                </div>
                <div class="footer_table">
                </div>
            </div>
        </form>
    </div>
    <div id="div_editor_link" style="display:none; width:70%; margin:auto; border:1px outset #d3d3d3;">
        <div class="text-right">
            <button type="button" class="clean" title="Fechar" onclick="javascript:fnCloseEditorLink();">
                <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_fechar.png" width="18px" height="18px" />
            </button>
        </div>
        <div id="div_conteudo_arquivo"></div>
        <form id="form_editar_link" name="form_editar_link" action="#" url-ajax="./ashx/Cadastro/ArquivoIncluir.ashx" method="post" onsubmit="javascript:return fnSubmitEditarLink('form_editar_link');">
            <input type="hidden" name="id_doc" value="" />
            <input type="hidden" name="id_file" value="" />
            <input type="hidden" name="in_editar" value="1" />
            <input type="hidden" name="nm_base" value="sinj_arquivo" />
            <input type="hidden" name="ch_arquivo_raiz" value="<%= _diretorio %>"/>
            <input type="hidden" name="ch_arquivo_superior" value=""/>
            <input type="hidden" name="nr_nivel_arquivo_superior" value=""/>
            <input type="hidden" name="nr_tipo_arquivo" value="1" />
            <input type="hidden" name="nm_arquivo" value="" />

            <input type="hidden" name="arquivo" value="" obrigatorio="sim" label="Texto do Arquivo" />

            <div id="div_buttons_link" style="text-align: center;">
                <button type="submit" title="Salvar Arquivo">
                    <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_save.png" width="18px" height="18px" /> Salvar
                </button>
                <button type="button" title="Cancelar" onclick="javascript:fnCloseEditorLink();">
                    <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_fechar.png" width="18px" height="18px" /> Cancelar
                </button>
            </div>
        </form>
    </div>
    <div id="div_editor_file" style="display:none; width:70%; margin:auto; border:1px outset #d3d3d3;">
        <div id="editor_file_notificacao" class="notify" style="display:none;"></div>
        <form id="form_editor_file" name="form_editor_file" action="#" method="post" url-ajax="./ashx/Cadastro/ArquivoIncluir.ashx" method="post" onsubmit="javascript:return  fnUploadEditorFile();">
            <input type="hidden" name="id_doc" value="" />
            <input type="hidden" name="id_file" value="" />
            <input type="hidden" name="in_editar" value="" />
            <input type="hidden" name="nm_base" value="sinj_arquivo" />
            <input type="hidden" name="ch_arquivo_raiz" value="<%= _diretorio %>"/>
            <input type="hidden" name="ch_arquivo_superior" value=""/>
            <input type="hidden" name="nr_nivel_arquivo_superior" value=""/>
            <input type="hidden" name="nr_tipo_arquivo" value="1"/>
            <div class="text-right">
                <button type="button" class="clean" title="Fechar" onclick="javascript:fnCloseEditorFile();">
                    <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_fechar.png" width="18px" height="18px" />
                </button>
            </div>
            <div class="table w-95-pc mauto">
                <div class="notify" style="display:none;"></div>
                <div class="line">
                    <div class="column w-20-pc">
                        <div class="cell">
                            <label>
                                *Nome do arquivo:
                            </label>
                        </div>
                    </div>
                    <div class="column w-50-pc">
                        <input type="text" name="nm_arquivo" obrigatorio="sim" label="Nome do arquivo" class="w-100-pc" />
                    </div>
                </div>
                <div class="line">
                    <div class="column w-100-pc">
                        <textarea id="textarea_arquivo_editor" name="arquivo" rows="10" cols="10" style="display:none"></textarea>
                    </div>
                </div>
                <div class="line">
                    <div class="column w-100-pc">
                        <div class="text-center">
                            <button type="submit" title="Salvar Arquivo">
                                <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_save.png" width="18px" height="18px" /> Salvar
                            </button>
                            <button type="button" title="Cancelar" onclick="javascript:fnCloseEditorFile();">
                                <img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_fechar.png" width="18px" height="18px" /> Cancelar
                            </button>
                        </div>
                    </div>
                </div>
                <div class="footer_table">
                </div>
            </div>
            
        </form>
    </div>
    <div id="div_attach_file" style="display:none;">
        <form id="form_attach_file" name="form_attach_file" action="#" url-ajax="./ashx/Cadastro/ArquivoIncluir.ashx" method="post" onsubmit="javascript:return fnUploadDocument('div_attach_file')">
            <input type="hidden" name="ch_arquivo_raiz" value="<%= _diretorio %>"/>
            <input type="hidden" name="ch_arquivo_superior" value=""/>
            <input type="hidden" name="nr_nivel_arquivo_superior" value=""/>
            <input type="hidden" name="nr_tipo_arquivo" value="1"/>
            <input type="hidden" name="nm_base" value="sinj_arquivo"/>
            <input type="hidden" obrigatorio="sim" label="Nome" name="nm_arquivo" value="" />
            <input type="file" name="file" accept="" obrigatorio="sim" label="Arquivo" style="display:none;" targetform="form_attach_file" onchange="javascript:selecionarArquivo(this)" />
        </form>
    </div>
</asp:Content>
