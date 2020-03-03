$(document).ready(function () {
    $('#div_autocomplete_usuario_cadastrador_modal').autocompletelight({
        sKeyDataName: "nm_login_usuario",
        sValueDataName: "nm_usuario",
        sInputHiddenName: "nm_login_usuario",
        sInputName: "nm_usuario",
        sAjaxUrl: './ashx/Autocomplete/UsuarioAutocomplete.ashx',
        bLinkAll: true,
        sLinkName: "a_usuario"
    });
    $('#button_pesquisar_erros').click(function () {
        return PesquisarErros(this);
    });
});

function PesquisarErros(button) {
    var tipo_erro = $(button).attr("tipo");
    var oDatatable = {
        "sistema": {
            sAjaxUrl: './ashx/Datatable/AuditoriaErrosSistemaDatatable.ashx?' + $('#form_erros').serialize(),
            aoColumns: _columns_erros_sistema,
            sIdTable: "tabela_erros_sistema",
            bStateSave: false
        },
        "indexacao": {
            sAjaxUrl: './ashx/Datatable/AuditoriaErrosIndexacaoDatatable.ashx?' + $('#form_erros').serialize(),
            aoColumns: _columns_erros_indexacao,
            sIdTable: "tabela_erros_indexacao",
            bStateSave: false
        },
        "extracao": {
            sAjaxUrl: './ashx/Datatable/AuditoriaErrosExtracaoDatatable.ashx?' + $('#form_erros').serialize(),
            aoColumns: _columns_erros_extracao,
            sIdTable: "tabela_erros_extracao",
            bStateSave: false
        }
    };
    $('#div_erros').dataTablesLight(oDatatable[tipo_erro]);
    return false;
}
