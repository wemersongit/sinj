
function ExcluirDoHistorico(id) {
    $.ajaxlight({
        sUrl: './ashx/Exclusao/HistoricoDePesquisaExcluir.ashx?id=' + id,
        sType: "GET",
        bAsync: true
    });
    $('#datatable_historico tr.selected').remove();
}
function LimparHistorico() {
    var sucesso = function () {
        document.location.reload();
    }
    $.ajaxlight({
        sUrl: './ashx/Exclusao/HistoricoDePesquisaExcluir.ashx?chave=' + $.cookie('sinj_ch_history'),
        sType: "GET",
        fnSuccess: sucesso,
        bAsync: true
    });
}
