<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="EditarArquivo.aspx.cs" Inherits="TCDF.Sinj.Web.EditarArquivo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Texto do Arquivo da Diário</title>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            CKEDITOR.replace('arquivo', { language: 'pt-br', height: '500' });
        });
        function fn_submit_arquivo(_sucesso) {
            if (!IsNotNullOrEmpty(form_editar_arquivo.filename.value)) {
                $('#editar_arquivo_notificacao').messagelight({
                    sContent: "É necessário informar o nome do arquivo.",
                    sType: "error"
                });
                focusElement(form_editar_arquivo.filename);
                $('#form_editar_arquivo').parent().animate({
                    scrollTop: '0px'
                }, 'fast');
                return false;
            }
            CKEDITOR.instances.arquivo.updateElement();
            document.getElementById('arquivo').value = window.escape(document.getElementById('arquivo').value);
            var sucesso = typeof _sucesso === "function" ? _sucesso : function (data) {
                $('#super_loading').hide();
                if (IsNotNullOrEmpty(data, 'error_message')) {
                    $('#editar_arquivo_notificacao').messagelight({
                        sContent: data.error_message,
                        sType: "error"
                    });
                }
                else if (IsNotNullOrEmpty(data, 'success_message')) {
                    $('#editar_arquivo_notificacao').messagelight({
                        sContent: data.success_message,
                        sType: "success"
                    });

                }
            }
            var beforeSubmit = function () {
                $('#super_loading').show();
            }
            $.ajaxlight({
                sUrl: './ashx/Arquivo/NormaEditarArquivo.ashx' + window.location.search,
                sType: "POST",
                sFormId: "form_editar_arquivo",
                fnSuccess: sucesso,
                fnBeforeSubmit: beforeSubmit,
                bAsync: true
            });
            return false;
        }
    </script>
</head>
<body>
    <div id="editar_arquivo_notificacao" class="notify" style="display:none;"></div>
    <form id="form_editar_arquivo" name="form_editar_arquivo" action="#" method="post">
        <%
            var normaRn = new TCDF.Sinj.RN.NormaRN();
            var _id_file = Request["id_file"];
            neo.BRLightREST.ArquivoOV oJsonDoc = null;
            var sArquivo = "";
            if (!string.IsNullOrEmpty(_id_file))
            {
                var sJsonDoc = normaRn.GetDoc(_id_file);
                oJsonDoc = util.BRLight.JSON.Deserializa<neo.BRLightREST.ArquivoOV>(sJsonDoc);
                var arquivo = normaRn.Download(_id_file);
                sArquivo = Encoding.UTF8.GetString(arquivo);
                if (sArquivo.IndexOf("charset=windows-1252") > -1)
                {
                    Encoding wind1252 = Encoding.GetEncoding(1252);
                    Encoding utf8 = Encoding.UTF8;
                    byte[] utfBytes = Encoding.Convert(wind1252, utf8, arquivo);
                    sArquivo = utf8.GetString(utfBytes);
                }
            }
        %>
        <%= "<input type='hidden' name='id_file' value='" + _id_file + "'/>"%>
        <%= "<input type='hidden' name='id_doc' value='" + Request["id_doc"] + "'/>"%>
        <%= "<input type='hidden' name='path' value='" + Request["path"] + "'/>"%>
        <%= "<label>Nome do Arquivo:</label><input type='text' name='filename' value='" + (oJsonDoc != null ? oJsonDoc.filename : "") + "'/>"%>
        <textarea name="arquivo" id="arquivo" rows="10" cols="80">
            <%
                if (sArquivo != "")
                {
                    Response.Write(sArquivo);
                }
            %>
        </textarea>
    </form>
</body>
</html>