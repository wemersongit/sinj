<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditarLinks.aspx.cs" Inherits="TCDF.Sinj.Web.EditarLinks" ClientIDMode="Static" %>
<!DOCTYPE html>
<html>
<head>
    <title>.:: SINJ-DF - Sistema Integrado de Normas Jurídicas do DF ::.</title>
    
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Jquery-ui/jquery-ui.min.css" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/jquery.modalLight.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Styles/sinj.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Themes/<%=TCDF.Sinj.Web.Sinj.MostrarTema()%>/css/colors.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />

    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.form.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Jquery-ui/jquery-ui.min.js"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/liblight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.ajaxLight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.modalLight.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_sinj.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/tooltip.js"></script>
    <script type="text/javascript" charset="utf-8">
        function salvarParagrafo(el) {
            var $tooltip = $(el).closest('div.tooltip');
            var novo_linkname = $('input[name="nome"]', $tooltip).val();

            var id_tooltip = $tooltip.attr('id');
            var $button = $('button[aria-describedby="' + id_tooltip + '"]');
            var $p = $button.closest('p');
            var linkname = $p.attr('linkname');
            $('a[name="' + linkname + '"]', $p).remove();
            $p.removeAttr("linkname");

            if (IsNotNullOrEmpty(novo_linkname)) {
                $p.prepend('<a id="' + novo_linkname + '" name="' + novo_linkname + '"></a>');
                $p.attr('linkname', novo_linkname);
            }
            $button.html('<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_anchor_p.png" alt="editar" />');
            $button.click();
        }
        function clickTooltip(el) {
            var id_tooltip = $(el).attr('aria-describedby');
            $('button.buttontooltip').tooltip('hide');
            if (isNullOrEmpty(id_tooltip)) {
                $(el).tooltip('show');
                id_tooltip = $(el).attr('aria-describedby');

                var $p = $(el).closest('p');
                var linkname = $p.attr('linkname');

                if (IsNotNullOrEmpty(linkname)) {
                    $('#' + id_tooltip + ' input[name="nome"]').val(linkname);
                }

                $('#' + id_tooltip + ' button.tooltip-close').bind('click',function () { $(el).tooltip('hide'); });
            }
            else {
                $('#' + id_tooltip + ' button.tooltip-close').unbind('click');
            }
        }
        function fnSubmitEditarLink(id_form) {
            $('input[name="arquivo"]').val(window.escape($('#div_conteudo_arquivo').html()));
            var sucesso = function (data) {
                $('#super_loading').hide();
                if (data.error_message != null && data.error_message != "") {
                    notificar('#' + id_form, data.error_message, 'error');
                }
                else if (IsNotNullOrEmpty(data, "success_message")) {
                    ShowDialog({
                        id_element: "modal_notificacao_success",
                        sTitle: "Sucesso",
                        sContent: data.success_message,
                        sType: "success",
                        oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                        fnClose: function () {
                            //fnCloseEditorFile();
//                            if (IsNotNullOrEmpty(data, 'arquivo')) {
//                                if (data.action == "UPDATED") {
//                                    for (var i = 0; i < results.length; i++) {
//                                        if (results[i]._metadata.id_doc == data.arquivo._metadata.id_doc) {
//                                            results[i] = data.arquivo;
//                                            break;
//                                        }
//                                    }
//                                }
//                                else {
//                                    results.push(data.arquivo);
//                                }
//                                $('#div_list_dir div.div_folder.selected a').click();
//                            }
                        }
                    });
                }
            }
            return fnSalvarForm(id_form, sucesso);
        }

        function ehInciso(termo) {
            var chars = ["I", "V", "X", "L", "C", "D", "M"];
            for (var i = 0; i < termo.length; i++) {
                if (chars.indexOf(termo[i]) < 0) {
                    return false;
                }
            }
            return true;
        }

        function ehAlinea(termo) {
            var chars = ["i", "v", "x", "l", "c", "d", "m"];
            for (var i = 0; i < termo.length; i++) {
                if (chars.indexOf(termo[i]) < 0) {
                    return false;
                }
            }
            return true;
        }

        //Gera um linkname para ser usado na ancora com base no conteúdo
        function generateLinkName(text) {
            var id = '';
            if(IsNotNullOrEmpty(text)){
                var palavras = text.split(' ');
                if (palavras[0] == 'CAPÍTULO') {
                    id = 'cap' + palavras[1];
                }
                else if (palavras[0] == 'Art.') {
                    id = 'art' + palavras[1];
                    id = id.replace(/°/, '');
                }
                else if (palavras[0] == 'Parágrafo') {
                    id = 'par';
                }
                else if (palavras[0] == '§') {
                    id = 'par' + palavras[1];
                    id = id.replace(/°/, '');
                }
                else if (ehInciso(palavras[0])) {
                    id = 'inc' + palavras[0];
                }
                else if (palavras[0].length == 2 && palavras[0][1] == ")") {
                    id = 'let' + palavras[0][0];
                }
                else if (ehAlinea(palavras[0])) {
                    id = 'inc' + palavras[0];
                }
            }
            return id;
        }

        //definir nivel do arquivo
        function definirNivel(name) {
            var nivel = 10;
            var niveis = ["cap", "art", "par", "inc", "ltr", "aln"];
            if (IsNotNullOrEmpty(name) && name.indexOf('_') > -1) {
                name = name.substring(name.lastIndexOf('_') + 1);
            }
            for (var i = 0; i < niveis.length; i++) {
                if(name.indexOf(niveis[i]) == 0){
                    nivel = i;
                    break;
                }
            }
            return nivel;
        }

        function definirEstrutura(name, niveis) {
            var old_name = name;
            if (IsNotNullOrEmpty(name)){
                if (name.indexOf('_') > -1) {
                    name = name.substring(name.lastIndexOf('_') + 1);
                }
                old_name = name;
                name = name.substring(0, 3);
            }
            var nivel = definirNivel(name);
            var primeiro_nivel = 0;
            var ultimo_nivel = 0;
            if (IsNotNullOrEmpty(niveis)) {
                primeiro_nivel = definirNivel(niveis[0]);
                ultimo_nivel = definirNivel(niveis[niveis.length - 1]);
            }

            if (nivel < primeiro_nivel) {
                niveis = [old_name];
            }
            if (nivel > ultimo_nivel) {
                niveis.push(old_name);
            }
            else if (nivel <= ultimo_nivel) {
                var niveis2 = [];
                for (var i = 0; i < niveis.length; i++) {
                    if(niveis[i].indexOf(name) == 0){
                        niveis2.push(old_name);
                        break;
                    }
                    niveis2.push(niveis[i]);
                }
                niveis = niveis2;
            }
            return niveis;
        }

        $(document).ready(function () {
            if ($('#div_conteudo_arquivo p').length > 0) {
                var niveis = [];
                var ultimo_nivel = 0;
                $.each($('#div_conteudo_arquivo p'), function (key_p, value_p) {
                    var linkname = $(value_p).attr('linkname');
                    var nivel = 0;
                    //Se o paragrafo já possuir o atributo linkname (atributo criado para o editor de links conseguir exibir os icones e fazer a funcionalidade de criar e editar ancoras),
                    //ele não deve ser mexido
                    if (!IsNotNullOrEmpty(linkname)) {
                        //se não tem conteúdo no paragrafo não há porquê tentar criar linkname e ancora
                        if ($.trim(value_p.textContent)) {
                            //tenta criar o atributo linkname no paragrafo com base na primeira ancora dele
                            if ($('a[name]', value_p).length > 0) {
                                var value_a = $('a[name]', value_p)[0];
                                if (value_a.textContent == '' && value_a.hasAttribute('id') && value_a.hasAttribute('name')) {
                                    linkname = value_a.getAttribute('name');
                                    $(value_p).attr('linkname', linkname);
                                }
                            }
                            //se o procedimento anterior não conseguiu determinar um linkname então o paragrafo possui uma ancora criada pelo editor de links,
                            //deve tentar criar uma ancora e um linkname com base no conteúdo do paragrafo (art_1, paragrafo_3, capitulo_2, capitulo_2_art_1_paragrafo_3, etc)
                            if (!IsNotNullOrEmpty(linkname)) {
                                linkname = generateLinkName(value_p.textContent);
                                var name = linkname;
                                if (IsNotNullOrEmpty(linkname)) {
                                    if (IsNotNullOrEmpty(niveis)) {
                                        niveis = definirEstrutura(linkname, niveis);
                                        name = niveis.join('_');
                                    }
                                    $(value_p).prepend('<a id="' + name + '" name="' + name + '"></a>');
                                    $(value_p).attr('linkname', name);
                                }
                            }
                        }
                    }
                    if (IsNotNullOrEmpty(linkname)) {
                        niveis = definirEstrutura(linkname, niveis);
                        $(value_p).prepend('<button class="buttontooltip clean" type="button" onclick="javascript:clickTooltip(this);"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_anchor_p.png" alt="editar" /></a>');
                    }
                    else if ($.trim(value_p.textContent)) {
                        $(value_p).prepend('<button class="buttontooltip clean" type="button" onclick="javascript:clickTooltip(this);"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" alt="editar" /></a>');
                    }
                });
                var title =
                    '<div class="table">' +
                        '<div class="line">' +
                            '<div class="column w-20-pc">' +
                                '<label>Nome: </label>' +
                            '</div>' +
                            '<div class="column w-70-pc">' +
                                '<input name="nome" type="text" class="w-80-pc" value="" />' +
                            '</div>' +
                        '</div>' +
                        '<div class="line">' +
                            '<div class="column w-100-pc text-center">' +
                                '<button type="button" onclick="javascript:salvarParagrafo(this)">Salvar</button>' +
                            '</div>' +
                        '</div>' +
                    '</div>';
                $('#div_conteudo_arquivo p button.buttontooltip').tooltip({ title: title, container: "#div_tooltip", html: true, trigger: "manual", placement: "auto top", template: '<div class="tooltip" role="tooltip"><button class="tooltip-close clean">x</button><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>' });
            }
        });
    </script>
</head>
<body>
    <div id="div_conteudo_arquivo" runat="server">
    </div>
    <div id="div_tooltip"></div>
    <form id="form_editar_link" name="form_editar_link" action="#" url-ajax="./ashx/Cadastro/ArquivoIncluir.ashx" method="post" onsubmit="javascript:return fnSubmitEditarLink('form_editar_link');">
        <input id="id_doc" type="hidden" name="id_doc" value="" runat="server" />
        <input id="id_file" type="hidden" name="id_file" value="" runat="server" />
        <input type="hidden" name="in_editar" value="1" />
        <input type="hidden" name="nm_base" value="sinj_arquivo" />
        <input id="nm_arquivo" type="hidden" name="nm_arquivo" value="" runat="server"  />
        <input type="hidden" name="arquivo" value="" obrigatorio="sim" label="Texto do Arquivo" />
        <div id="div_buttons_link" style="text-align: center;">
            <button type="submit" id="button_salvar_link">
                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
            </button>
            <button type="button" onclick="javascript:window.history.back();">
                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
            </button>
        </div>
    </form>
</body>
</html>
