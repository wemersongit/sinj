<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Imagens.aspx.cs" Inherits="TCDF.Sinj.Web.Imagens" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    <meta http-equiv="Content-Language" content="pt-br" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Jquery-ui/jquery-ui.min.css" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/dataTables.responsive.css" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/table_jui.css" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/default.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/sinj.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />

    <script type="text/javascript" charset="utf-8"> <% Response.Write(jsValorChave()); %> </script>

    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.form.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.cookie.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/on_error.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Jquery-ui/jquery-ui.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/liblight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/stacktrace.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.ajaxLight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.autocompleteLight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jMenu.jquery.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.modalLight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_sinj.js?<%= TCDF.Sinj.Util.MostrarVersao() %>" charset="utf-8"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/dataTables.responsive.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.dataTablesLight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/columns_datatables.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>

    <link rel="shortcut icon" type="image/x-icon" href="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/favicon.png" />

    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {
            carregarImagens('000shared/images');
        });

        var dir = [];

        function voltarPasta(){
            carregarImagens(dir.splice(-2)[0]);
        }

        function carregarImagens(ch_dir) {
            dir.push(ch_dir);
            if(dir.length > 1){
                $('#button_voltar_pasta').show();
            }
            else{
                $('#button_voltar_pasta').hide();
            }
            $("#div_list_arq").dataTablesLight({
                sAjaxUrl: "./ashx/Datatable/ArquivoDatatable.ashx?ch_arquivo_superior=" + ch_dir,
                aoColumns: _columns_imagens,
                sIdTable: 'datatable_arquivos',
                responsive: null,
                bFilter: true,
                fnCreatedRow: function (nRow, aData, iDataIndex) {
                    if(aData.nr_tipo_arquivo == '1'){
                        nRow.className += " tr_arq";
                        nRow.setAttribute('tipo', aData.ar_arquivo.mimetype.split('/')[1]);
                        nRow.setAttribute('chave', aData.ch_arquivo);
                        nRow.setAttribute('id_file', aData.ar_arquivo.id_file);
                        nRow.setAttribute('filename', aData.ar_arquivo.filename);
                        nRow.setAttribute('nm_arquivo', aData.nm_arquivo);
                    }
                },
            });
        }

        function selectImage(url){
            var urlSinjPortal = '<%=util.BRLight.Util.GetVariavel("URLSinjPortal",true)%>';
            window.opener.CKEDITOR.tools.callFunction(<%=Request["CKEditorFuncNum"]%>, urlSinjPortal + "/" + url);
            window.close();
        }

    </script>
</head>
<body>
    <div class="content_geral w-90-pc">
        <div class="titulo_content">
            <h2>
                <span id="span_titulo">Imagens</span> 
            </h2>
        </div>
        <div style="height:30px;">
            <button class="clean" id="button_voltar_pasta" onclick="voltarPasta();"><img valign="absmiddle" src="<%=TCDF.Sinj.Util._urlPadrao%>/Imagens/ico_up_folder.png" width="24px" height="24px" /></button>
        </div>
        <div id="div_importar_arquivo">
            <form id="form_importar_arquivo" name="form_importar_arquivo" action="#" method="get" onsubmit="javascript:return importarImagem('form_importar_arquivo');">
                <input type="hidden" name="id_file" value="" />
                <input type="hidden" name="filename" value="" />
                <div id="div_arquivos">
                    <div>
                        <div id="div_list_arq">
                        </div>
                    </div>
                </div>

                <div class="table w-100-pc" style="display:none;">
                    <div class="line">
                        <div class="column w-10-pc">
                            <label>Arquivo:</label>
                        </div>
                        <div class="column w-90-pc">
                            <input class="input_chave_importar w-90-pc" disabled="disabled" id="input_chave_importar" label="Arquivo" obrigatorio="sim" type="text" value="" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <div id="div_modal_confirme" style="display:none;">
    </div>
</body>
</html>
