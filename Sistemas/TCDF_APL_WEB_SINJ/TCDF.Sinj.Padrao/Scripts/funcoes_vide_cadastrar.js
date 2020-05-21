let tiposDeRelacao = [];
let tipoDeRelacaoSelecionado = {};
let flagModalNorma = '';
let idLink = '';
let optionsTooltip = {
    template: `<div class="tooltip" role="tooltip">
        <div class="tooltip-arrow"></div>
            <div class="tooltip-header">
                <div class="table w-100-pc">
                    <div class="head">
                        <div class="title">
                            <h2>Texto Novo</h2>
                        </div>
                    </div>
                </div>
            </div>
        <div class="tooltip-inner"></div>
    </div>`,
    container: '#div_tooltip_dispositivo',
    trigger: 'manual',
    title: `<div class="table w-80-pc mauto">
        <div class="line texto-novo">
            <div class="column w-100-pc">
                <textarea name="textoNovo" class="w-100-pc"></textarea>
            </div>
        </div>
        <div class="line">
            <div class="column w-100-pc text-center">
            <button type="button" class="button" onclick="clickAlterarDispositivoCadastrar()"><img src="${_urlPadrao}/Imagens/ico_check.png" width="15px" height="15px" /> Ok</button>
            &nbsp;
            <button type="button" class="button" onclick="clickCancelarAlterarDispositivo(this)"><img src="${_urlPadrao}/Imagens/ico_close.png" width="15px" height="15px" /> Cancelar</button>
            </div>
        </div>
    </div>`,
    html: true,
    placement: "right"
};

function selecionarAlteracaoCompleta(){
    //Se não há dispositivo alterador selecionado dá um alerta e interrompe a função
    if(!IsNotNullOrEmpty(normaAlteradora, 'dispositivos') && !normaAlteradora.sem_arquivo){
        $('#in_alteracao_completa').prop('checked', '');
        alert('Selecione o texto alterador.');
        return;
    }
    const dispositivosNormaAlterada = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]');
    if($('#in_alteracao_completa').is(':checked')){
        const dispositivosIdentificacaoAlteracaoCompleta = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa]');
        //se na inclusão de repristinação, revigoração, revogação, cancelamento, etc. o texto já sofre alteração completa por outro vide,
        //é necessário validar a última alteração sofrida e se o texto está, ou não, riscado
        //para cada situação haverá um comportamento:
        //1. Na repristinação
        //   1.1. se o texto estiver riscado
        //        1.1.1. incluir o link da alteração e remover o riscado do texto
        //   1.2. se o texto não estiver riscado
        //        1.2.1. incluir o link da alteração e não (tentar) remover o riscado do texto
        //2. Na revogação
        //   2.1. se o texto não estiver riscado
        //        2.1.1. incluir o link da alteração e incluir o riscado do texto
        //   2.2. se o texto estiver riscado
        //        2.2.1. incluir o link da alteração e não (tentar) incluir o riscado do texto
        if(ehRelacaoQueDesfazAlteracaoCompleta(tipoDeRelacaoSelecionado.ch_tipo_relacao)){
            if(dispositivosIdentificacaoAlteracaoCompleta.length > 0){
                const lastChNorma = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_norma_alteracao_completa');
                const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_tipo_relacao');
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${lastChNorma}]`).after(`<p ch_norma_alteracao_completa="${normaAlteradora.ch_norma}" style="text-align:center;" ch_tipo_relacao="${tipoDeRelacaoSelecionado.ch_tipo_relacao}" class="adicionado">
                    <a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}" >(${tipoDeRelacaoSelecionado.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>
                </p>`);
                if(ehRelacaoDeAlteracaoCompleta(lastChTipoRelacao) && $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]').length > 0){
                    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s')[0].remove();
                    let lastSelector = `p[ch_norma_alteracao_completa=${normaAlteradora.ch_norma}]`;
                    for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        dispositivosNormaAlterada[i].classList.add('adicionado');
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo ${lastSelector}`).after(dispositivosNormaAlterada[i]);
                        lastSelector = `p[linkname=${dispositivosNormaAlterada[i].getAttribute('linkname')}]`;
                    }
                    $('#div_cad_dispositivo_alterada [data-toggle="tooltip"]').tooltip(optionsTooltip);
                }
            }
            else{
                $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]').after(`<p ch_norma_alteracao_completa="${normaAlteradora.ch_norma}" style="text-align:center;" ch_tipo_relacao="${tipoDeRelacaoSelecionado.ch_tipo_relacao}" class="adicionado">
                        <a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}" >(${tipoDeRelacaoSelecionado.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>
                    </p>`);
            }
        }
        else{
            if(dispositivosIdentificacaoAlteracaoCompleta.length > 0){
                const lastChNorma = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_norma_alteracao_completa');
                const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_tipo_relacao');
                $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${lastChNorma}]`).after(`<p ch_norma_alteracao_completa="${normaAlteradora.ch_norma}" style="text-align:center;" ch_tipo_relacao="${tipoDeRelacaoSelecionado.ch_tipo_relacao}" class="adicionado">
                        <a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}" >(${tipoDeRelacaoSelecionado.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>
                    </p>`);
                if(!ehRelacaoDeAlteracaoCompleta(lastChTipoRelacao) && $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]').length <= 0){
                    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]').remove();
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${normaAlteradora.ch_norma}]`).after('<s></s>');
                    for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        dispositivosNormaAlterada[i].classList.add('alterado');
                        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').append(dispositivosNormaAlterada[i]);
                    }
                }
            }
            else{
                if($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]').length <= 0){
                    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]').remove();
                    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]').after(`<p ch_norma_alteracao_completa="${normaAlteradora.ch_norma}" style="text-align:center;" ch_tipo_relacao="${tipoDeRelacaoSelecionado.ch_tipo_relacao}" class="adicionado">
                            <a href="(_link_sistema_)Norma/${normaAlteradora.ch_norma}/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}" >(${tipoDeRelacaoSelecionado.ds_texto_para_alterador} pelo(a) ${normaAlteradora.ds_norma})</a>
                        </p>
                        <s></s>`);
                    for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        dispositivosNormaAlterada[i].classList.add('alterado');
                        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').append(dispositivosNormaAlterada[i]);
                    }
                }
            }
        }
        normaAlterada.in_alteracao_completa = true;
        changeStepper('selecionarDispositivoAlterado');
    }
    else{
        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${normaAlteradora.ch_norma}]`).remove();
        const dispositivosIdentificacaoAlteracaoCompleta = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa]');
        if(ehRelacaoQueDesfazAlteracaoCompleta(tipoDeRelacaoSelecionado.ch_tipo_relacao)){
            if(dispositivosIdentificacaoAlteracaoCompleta.length > 0){
                const lastChNorma = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_norma_alteracao_completa');
                const lastChTipoRelacao = dispositivosIdentificacaoAlteracaoCompleta[dispositivosIdentificacaoAlteracaoCompleta.length - 1].getAttribute('ch_tipo_relacao');
                if($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s p[linkname]').length <= 0 && ehRelacaoDeAlteracaoCompleta(lastChTipoRelacao)){
                    $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname]').remove();
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_alteracao_completa=${lastChNorma}]`).after('<s></s>');
                    for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                        dispositivosNormaAlterada[i].classList.remove('adicionado');
                        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s').append(dispositivosNormaAlterada[i]);
                    }
                }
            }
        }
        else{
            if(dispositivosIdentificacaoAlteracaoCompleta.length <= 0 || !ehRelacaoDeAlteracaoCompleta(lastChTipoRelacao)){
                $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo s')[0].remove();
                let lastSelector = 'h1[epigrafe]';
                for(let i = 0; i < dispositivosNormaAlterada.length; i++){
                    dispositivosNormaAlterada[i].classList.remove('alterado');
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo ${lastSelector}`).after(dispositivosNormaAlterada[i]);
                    lastSelector = `p[linkname=${dispositivosNormaAlterada[i].getAttribute('linkname')}]`;
                }
                $('#div_cad_dispositivo_alterada [data-toggle="tooltip"]').tooltip(optionsTooltip);
            }
        }
        normaAlterada.in_alteracao_completa = false;
        changeStepper('deselecionarDispositivoAlterado');
    }
}

function selecionarNormaForaDoSistemaCadastrar() {
    if ($('#in_norma_fora_do_sistema').is(':checked')) {
        normaAlterada.in_norma_fora_do_sistema = true;
        $('#line_norma_fora_do_sistema').removeClass('hidden');
        $('#line_norma_dentro_do_sistema').addClass('hidden');
        changeStepper('selecionarDispositivoAlterador');
        changeStepper('selecionarDispositivoAlterado');
    }
    else {
        normaAlterada.in_norma_fora_do_sistema = false;
        $('#line_norma_fora_do_sistema').addClass('hidden');
        $('#line_norma_dentro_do_sistema').removeClass('hidden');
        changeStepper('deselecionarDispositivoAlterador');
        changeStepper('deselecionarDispositivoAlterado');
    }
}

function fecharNormaForaDoSistemaCadastrar(){
    $('#in_norma_fora_do_sistema').prop('checked', false);
    selecionarNormaForaDoSistemaCadastrar();
}

function buscarTiposDeRelacao() {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data)) {
            if (data.error_message != null && data.error_message != "") {
                $('#div_notificacao_vide').messagelight({
                    sTitle: "Erro",
                    sContent: data.error_message,
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
            else if (IsNotNullOrEmpty(data.results)) {
                tiposDeRelacao = data.results;
                let selectOptions = `<option value=""></option>`;
                for(let tipo of tiposDeRelacao){
                    selectOptions += `<option value="${tipo.ch_tipo_relacao}">${tipo.nm_tipo_relacao}</option>`;
                }
                $('#selectTipoDeRelacao').html(selectOptions);
            }
        }
    };
    $.ajaxlight({
        sUrl: './ashx/Autocomplete/TipoDeRelacaoAutocomplete.ashx' + (GetParameterValue("in_acao") == "true" ? "?in_relacao_de_acao=true" : ""),
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: function(){
            if(!IsNotNullOrEmpty(id_doc)){
                gComplete();
            }
        },
        fnBeforeSend: gInicio,
        fnError: null,
        bAsync: true
    });
}

function construirControlesDinamicos() {
    buscarTiposDeRelacao();
    $('#div_autocomplete_tipo_norma_vide_fora_de_sistema').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma_vide_fora_do_sistema",
        sInputName: "nm_tipo_norma_vide_fora_do_sistema",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_norma_vide_fora_do_sistema"
    });
    $('#div_autocomplete_tipo_norma_modal').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma_modal",
        sInputName: "nm_tipo_norma_modal",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_norma_modal"
    });
    $('#div_autocomplete_tipo_fonte_vide_fora_do_sistema').autocompletelight({
        sKeyDataName: "ch_tipo_fonte",
        sValueDataName: "nm_tipo_fonte",
        sInputHiddenName: "ch_tipo_fonte_vide_fora_do_sistema",
        sInputName: "nm_tipo_fonte_vide_fora_do_sistema",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeFonteAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_fonte_vide_fora_do_sistema"
    });
}

function abrirModalSelecionarNorma(flag) {
    flagModalNorma = flag;
    form_modal_consultar_norma.reset();
    $('#datatable_normas_modal').html('');
    $('#modal_norma').modallight({
        sTitle:"Consultar Norma",
        sWidth:"800",
        oButtons:[]
    });
}

function pesquisarNorma() {
    $("#datatable_normas_modal").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/NormaDatatable.ashx?ch_tipo_norma=' + $('#ch_tipo_norma_modal').val() + '&nr_norma=' + $('#nr_norma_modal').val() + '&dt_assinatura=' + $('#dt_assinatura_modal').val(),
        aoColumns: _columns_norma_vide,
        sIdTable: 'table_normas_modal'
    });
    return false;
}

function selecionarNormaVide(ch_norma, nr_norma, dt_assinatura, nm_tipo_norma) {
    if (IsNotNullOrEmpty(ch_norma)) {
        if (flagModalNorma == 'alteradora') {
            tipoDeRelacaoSelecionado = {};
            $('#selectTipoDeRelacao').val('');
            resetarDispositivo('alteradora');
            resetarDispositivo('alterada');
            selecionarNormaAlteradoraCadastrar({ch_norma: ch_norma, nr_norma: nr_norma, dt_assinatura: dt_assinatura, nm_tipo_norma: nm_tipo_norma});
        }
        else {
            resetarDispositivo('alterada');
            selecionarNormaAlteradaCadastrar({ch_norma: ch_norma, nr_norma: nr_norma, dt_assinatura: dt_assinatura, nm_tipo_norma: nm_tipo_norma});
        }
        ahTextoIncompativel = false;
        $('#modal_norma').dialog('close');
    }
}

function selecionarNormaAlteradoraCadastrar(norma){
    normaAlteradora = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        sem_arquivo: false,
        dispositivos: []
    };

    $('#label_norma_vide_alteradora').text(normaAlteradora.ds_norma);
    selecionarArquivoDaNormaCadastrar(Object.assign({}, normaAlteradora, {sufixo: 'alteradora'}));
    changeStepper('selecionarNormaAlteradora');
}

function selecionarNormaAlteradaCadastrar(norma){
    normaAlterada = {
        ch_norma: norma.ch_norma,
        nr_norma: norma.nr_norma,
        ds_norma: montarDescricaoDaNorma(norma),
        dt_assinatura: norma.dt_assinatura,
        nm_tipo_norma: norma.nm_tipo_norma,
        sem_arquivo: false,
        dispositivos: []
    };
    const normasInvertidas = InverterAlteradaAlteradora();
    if(normasInvertidas){
        resetarDispositivo('alteradora');
        resetarDispositivo('alterada');
        selecionarNormaAlteradoraCadastrar(normasInvertidas.normaAlteradoraInvertida);
        changeStepper('selecionarTipoRelacao');
        selecionarNormaAlteradaCadastrar(normasInvertidas.normaAlteradaInvertida);
        $('#div_notificacao_vide').messagelight({
            sType: "alert",
            sContent: "A seleção de normas foi invertida porque a norma alteradora deve ser a mais recente.",
            iTime: null
        });
        return;
    }
    $('#label_norma_vide_alterada').text(normaAlterada.ds_norma);
    selecionarArquivoDaNormaCadastrar(Object.assign({}, normaAlterada, {sufixo: 'alterada'}));
    changeStepper('selecionarNormaAlterada');
}

function exibirControlesNormaAlteradaSelecionada(){
    $('#labelNormaForaDoSistema').hide();
    $('#labelLecoSemCitacao').hide();
    if(ehRelacaoDeAlteracaoCompleta(tipoDeRelacaoSelecionado.ch_tipo_relacao) || ehRelacaoQueDesfazAlteracaoCompleta(tipoDeRelacaoSelecionado.ch_tipo_relacao)){
        $('#labelAlteracaoCompleta').show();
    }
    else if(tipoDeRelacaoSelecionado.ch_tipo_relacao == '9'){
        $('#labelLecoSemCitacao').show();
    }
}

function resetarDispositivo(nm_sufixo) {
    if(nm_sufixo == 'alteradora'){
        normaAlteradora = {};
    }
    else{
        normaAlterada = {};
    }
    $('#label_norma_vide_' + nm_sufixo).text('Selecionar norma:');
    $('#label_dispositivo_norma_' + nm_sufixo).text('');

    $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo').html('');
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_conteudo_arquivo').hide();
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_enable_replaced').hide();
}

// Se a data de assinatura da norma alteradora for inferior à data de assinatura da alterada,
// deve-se inverter a seleção de alterada/alteradora.
// A unica exceção eh quando o tipo da norma for ADI.
function InverterAlteradaAlteradora() {
    if (IsNotNullOrEmpty(normaAlteradora.dt_assinatura) && IsNotNullOrEmpty(normaAlterada.dt_assinatura)) {
        if (normaAlteradora.nm_tipo_norma != "ADI") {
            if (convertStringToDateTime(normaAlteradora.dt_assinatura) < convertStringToDateTime(normaAlterada.dt_assinatura)) {
                return {
                    normaAlteradoraInvertida: normaAlterada,
                    normaAlteradaInvertida: normaAlteradora
                };
            }
        }
    }
}

function selecionarArquivoDaNormaCadastrar(norma) {
    $('#div_cad_dispositivo_' + norma.sufixo + ' div.arquivos_norma').html('');
    limparDispositivoSelecionado(norma.sufixo);

    var sucesso = function (data) {
        let semArquivo = false;
        if (IsNotNullOrEmpty(data, 'error_message')) {
            gComplete();
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
            return false;
        }
        if (IsNotNullOrEmpty(data, 'ar_atualizado.id_file')) {
            norma.arquivo = data.ar_atualizado;
            exibirControlesNormaAlteradaSelecionada();
        }
        else if (IsNotNullOrEmpty(data, 'fontes')) {
            if (IsNotNullOrEmpty(data.fontes[data.fontes.length - 1].ar_fonte, 'id_file')) {
                norma.arquivo = data.fontes[data.fontes.length - 1].ar_fonte;
                exibirControlesNormaAlteradaSelecionada();
            }
            else {
                semArquivo = true;
            }
        }
        else {
            semArquivo = true;
        }
        if(semArquivo){
            gComplete();
                $('<div id="modal_notificacao_vide" />').modallight({
                    sTitle: "Atenção",
                    sContent: "Não foi encontrado arquivo nas fontes de publicação e republicação da norma selecionada.<br/> Deseja incluir o vide mesmo assim?",
                    sType: "error",
                    oButtons: [{
                            text: "Sim",
                            click: function() {
                                limparDispositivoSelecionado(norma.sufixo);
                                if(norma.sufixo == 'alterada'){
                                    normaAlterada.sem_arquivo = true;
                                    limparDispositivoSelecionado('alteradora');
                                    // $('#div_cad_dispositivo_alterada').show();
                                    // $('#div_cad_dispositivo_alterada div.line_conteudo_arquivo').show();
                                    // $('.stepper').hide();
                                    // $('.step4').removeClass('hidden');
                                }
                                else{
                                    normaAlteradora.sem_arquivo = true;
                                }
                                $(this).dialog('close');
                            }
                        },{
                            text: "Não",
                            click: function() {
                                resetarDispositivo(norma.sufixo);
                                $(this).dialog('close');
                            }
                        }
                    ]
                });
                return;
        }
        // else{
        //     $('.stepper').show();
        // }
        $('.stepper').show();
        if(norma.sufixo == 'alteradora'){
            normaAlteradora.arquivo = norma.arquivo;
        }
        else{
            normaAlterada.arquivo = norma.arquivo;
        }
        const sucessoArquivo = function(data){
            exibirTextoDoArquivoCadastrar(norma, data);
            $('#div_cad_dispositivo_' + norma.sufixo).show();
        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + norma.arquivo.id_file,
            sType: "GET",
            fnSuccess: sucessoArquivo,
            fnComplete: gComplete,
            bAsync: true
        });
    }
    $.ajaxlight({
        sUrl: './ashx/Visualizacao/NormaDetalhes.ashx?id_norma=' + norma.ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: null,
        bAsync: true,
        iTimeout: null
    });

}

function selecionarTextoNormaAlteradoraCadastrar() {
    var text = window.getSelection().toString();
    if (text != '') {
        if($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').hasClass('copy-enabled')){
            selecionarTextoCopiar();
            return;
        }
        if(IsNotNullOrEmpty(normaAlteradora.dispositivos)){
            alert('Erro! Já existe um link selecionado.');
            return;
        }
        var parentNode = $(window.getSelection().baseNode).parents('p[linkname]');
        //Impede que selecione o link da norma alteradora antes de estar com a norma alterada selecionada.
        if ($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p').length <= 0) {
            alert('Erro! Selecione também o arquivo da norma alterada.');
            return false;
        }

        var linkname = parentNode.attr('linkname');
        if ($('p[linkname="' + linkname + '"]').text().length < text.length) {
            alert('Erro! Não selecione mais de um parágrafo por vez.');
            return false;
        }
        if (IsNotNullOrEmpty(linkname)) {
            normaAlteradora.dispositivos = [{
                linkname: linkname,
                texto: text
            }];

            var a = document.createElement('a');
            idLink = Guid();
            a.setAttribute('href', `javascript: desfazerLinkAlterador()`);
            a.setAttribute('title', 'Clique para remover a seleção do link');
            a.setAttribute('id', idLink);
            a.classList.add("link-alterador");


            window.getSelection().getRangeAt(0).surroundContents(a);

            window.getSelection().removeAllRanges();
            if(tipoDeRelacaoSelecionado.ch_tipo_relacao == '9'){
                clickAlterarDispositivoCadastrar();
                alert('Legislação correlata criada com sucesso.');
            }
            else{
                alert('O texto \'' + text + '\' foi selecionado.');
            }
            changeStepper('selecionarDispositivoAlterador');
        }
    }
}

function desfazerLinkAlterador(){
    if(normaAlterada.dispositivos && normaAlterada.dispositivos.length > 0){
        const dispositivosalterados = [...normaAlterada.dispositivos];
        for(var i = 0; i < dispositivosalterados.length; i++){
            desfazerAlteracaoDoDispositivoCadastrar(dispositivosalterados[i].linkname);
        }
    }
    if(idLink){
        const texto = $('#'+idLink).text();
        if(texto){
            $('#'+idLink).text('').after(texto).remove();
        }
        normaAlteradora.dispositivos = [];
        idLink = '';
    }
    
    changeStepper('deselecionarDispositivoAlterador');
}

function exibirTextoDoArquivoCadastrar(norma, arquivo) {
    limparDispositivoSelecionado(norma.sufixo);
    const htmlUnescaped = window.unescape(arquivo.fileencoded);
    if(ahTextoIncompativel){
        return;
    }
    if($(htmlUnescaped).closest('h1[epigrafe]').length <= 0){
        showMessageNormaIncompativel(norma);
        ahTextoIncompativel = true;
        return;
    }
    $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo').html(htmlUnescaped);
    $('#filename_arquivo_norma_alteradora').val(arquivo.filename);
    if ($('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p').length > 0) {
        //regex para localizar o link da revogação caso haja
        var regex = new RegExp(/(revogado\(a\)|declarado\(a\) inconstitucional).*?pelo\(a\)/);
        //itera em todos os paragrafos com o atributo replaced que é o que identifica se um paragrafo sofre ou não uma alteração
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p[replaced_by]').each(function (k, p) {
            //se possuir o texto de revogação
            if (regex.test($(p).text())) {
                var aLinkVide = $('a.link_vide', $(p));
                //se possuir links de vide
                if (aLinkVide.length > 0) {
                    //pega o texto do último link de vide
                    var aTexto = $(aLinkVide[aLinkVide.length - 1]).text();
                    //se o último link de vide no paragrafo for o de revogação então o mesmo não será removido no texto
                    if (regex.test(aTexto)) {
                        var replacedBy = $(p).attr('replaced_by');
                        $(p).removeAttr('replaced_by');
                        $(p).attr('replaced_by_revogado',replacedBy);
                    }
                }
            }
        });
        //esconde todos os parágrafos alterados, renumerados, etc., menos os revogados
        $('p[replaced_by]').hide();
        if(norma.sufixo == 'alterada'){
            if(tipoDeRelacaoSelecionado.ch_tipo_relacao != '9'){
                $.each($('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p'), function (key_p, value_p) {
                    var linkname = $(value_p).attr('linkname');
                    if (IsNotNullOrEmpty(linkname)) {
                        $(value_p).prepend('<button data-toggle="tooltip" linkname="' + linkname + '" class="select" type="button" onclick="javascript:clickButtonSelecionarDispositivo(this, \'' + norma.sufixo + '\');"></button>');
                    }
                });
            }
        }
        else if (norma.sufixo == 'alteradora') {
            $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo p[linkname]').mouseup(function () {
                selecionarTextoNormaAlteradoraCadastrar();
            });
        }
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_conteudo_arquivo').show();
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.line_enable_replaced').show();
        
        $('#div_cad_dispositivo_' + norma.sufixo + ' div.div_conteudo_arquivo img').filter(function() {
            if($(this).width() > $('#div_vide fieldset').width()/2){
                $(this).addClass('limit-width');
                console.log(this);
            }
        });
    }
    $('#div_cad_dispositivo_' + norma.sufixo + ' [data-toggle="tooltip"]').tooltip(optionsTooltip);

}

function selecionarLecoSemCitacao(){
    if($('#inLecoSemCitacao').is(':checked')){
        if(tipoDeRelacaoSelecionado.ch_tipo_relacao == '9'){
            if($(`#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[ch_norma_info=${normaAlterada.ch_norma}]`).length > 0){
                alert('A LECO já existe.');
                $('#inLecoSemCitacao').prop('checked','');
                return;
            }
            var $dispositivosLeco = $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[ch_norma_info]');
            var selectorInsertBefore = '#div_cad_dispositivo_alteradora div.div_conteudo_arquivo h1[epigrafe]';
            if($dispositivosLeco.length > 0){
                var expressaoRegular = /Legislação correlata - .*?([0-9]+)?(?: de ([0-9]{2}\/[0-9]{2}\/[0-9]{4}))?$/;
                for(let i = 0; i < $dispositivosLeco.length; i++){
                    let resultRegex = expressaoRegular.exec($dispositivosLeco.text());
                    if(resultRegex[2] && normaAlterada.dt_assinatura){
                        if(convertStringToDateTime(normaAlterada.dt_assinatura) > convertStringToDateTime(resultRegex[2])){
                            continue;
                        }
                        if(convertStringToDateTime(normaAlterada.dt_assinatura) == convertStringToDateTime(resultRegex[2])){
                            if(resultRegex[1] && normaAlterada.nr_norma){
                                if(parseInt(normaAlterada.nr_norma) > parseInt(resultRegex[1])){
                                    continue;
                                }
                            }
                        }
                    }
                    selectorInsertBefore = `#div_cad_dispositivo_alteradora div.div_conteudo_arquivo p[ch_norma_info=${$dispositivosLeco.attr('ch_norma_info')}]`;
                }
            }
            $(selectorInsertBefore).before(`<p ch_norma_info="${normaAlterada.ch_norma}"><a href="(_link_sistema_)Norma/${normaAlterada.ch_norma}/${normaAlterada.arquivo.filename}">Legislação correlata - ${normaAlterada.ds_norma}</a></p>`);
            changeStepper('selecionarDispositivoAlterador');
            clickAlterarDispositivoCadastrar();
        }
    }
}

function clickAlterarDispositivoCadastrar(){
    let linkSemDispositivoNormaAlteradora = linkNormaAlteradora = `(_link_sistema_)Norma/${normaAlteradora.ch_norma}`;
    if(!normaAlteradora.sem_arquivo){
        linkNormaAlteradora += `/${normaAlteradora.arquivo.filename}#${normaAlteradora.dispositivos[0].linkname}`;
        linkSemDispositivoNormaAlteradora += `/${normaAlteradora.arquivo.filename}`;
    }
    let dispositivoAlterado = {}
    if(tipoDeRelacaoSelecionado.ch_tipo_relacao == '9'){
        if($(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info=${normaAlteradora.ch_norma}]`).length > 0){
            alert('A LECO já existe.');
            return;
        }
        var $dispositivosLeco = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info]');
        var selectorInsertBefore = '#div_cad_dispositivo_alterada div.div_conteudo_arquivo h1[epigrafe]';
        if($dispositivosLeco.length > 0){
            var expressaoRegular = /Legislação correlata - .*?([0-9]+)?(?: de ([0-9]{2}\/[0-9]{2}\/[0-9]{4}))?$/;
            for(let i = 0; i < $dispositivosLeco.length; i++){
                let resultRegex = expressaoRegular.exec($dispositivosLeco.text());
                if(resultRegex[2] && normaAlteradora.dt_assinatura){
                    if(convertStringToDateTime(normaAlteradora.dt_assinatura) > convertStringToDateTime(resultRegex[2])){
                        continue;
                    }
                    if(convertStringToDateTime(normaAlteradora.dt_assinatura) == convertStringToDateTime(resultRegex[2])){
                        if(resultRegex[1] && normaAlteradora.nr_norma){
                            if(parseInt(normaAlteradora.nr_norma) > parseInt(resultRegex[1])){
                                continue;
                            }
                        }
                    }
                }
                selectorInsertBefore = `#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[ch_norma_info=${$dispositivosLeco.attr('ch_norma_info')}]`;
            }
        }
        $(selectorInsertBefore).before(`<p ch_norma_info="${normaAlteradora.ch_norma}" class="adicionado"><a href="${linkSemDispositivoNormaAlteradora}">Legislação correlata - ${normaAlteradora.ds_norma}</a></p>`);
    }
    else{
        const texto = $('#div_tooltip_dispositivo textarea[name=textoNovo]').val();
        let $buttonSelected = $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo button.selected');
        $buttonSelected.tooltip('hide');
        $buttonSelected.removeClass('selected').addClass('select');
        if(tipoDeRelacaoSelecionado.ch_tipo_relacao == '1'){
            if(!IsNotNullOrEmpty(texto)){
                return;
            }
            const textosAcrescidos = texto.split('\n');
            let $nextElement = $buttonSelected.parent();
            const linknamePrefix = $nextElement.attr('linkname');
            for(let i = 0; i < textosAcrescidos.length; i++){
                dispositivoAlterado.linkname = linknamePrefix + '_' + generateLinkNameCaput(textosAcrescidos[i]) + '_add';
                dispositivoAlterado.nm_linkName = getNomeDoLinkName(dispositivoAlterado.linkname);
                dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(dispositivoAlterado.linkname);
                dispositivoAlterado.texto = textosAcrescidos[i];
                normaAlterada.dispositivos.push(Object.assign({}, dispositivoAlterado));

                const $element = $(`<p linkname="${dispositivoAlterado.linkname}" class="adicionado"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoCadastrar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><a name="${dispositivoAlterado.linkname}"></a>${textosAcrescidos[i]}&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacaoSelecionado.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
                $nextElement.after($element);
                $nextElement = $element;
                if(IsNotNullOrEmpty(dispositivoAlterado.ds_linkname)){
                    adicionarDescricaoDispositivosAlterados(dispositivoAlterado.ds_linkname);
                }
            }
        }
        else{
            if(IsNotNullOrEmpty(texto) && texto.indexOf('\n') > -1){
                alert('Esse tipo de relação não permite que a alteração possua mais de um dispositivo por vez.');
                return;
            }
            let $elementoAlterado = $buttonSelected.parent();
            $buttonSelected.hide();
            dispositivoAlterado.linkname = $elementoAlterado.attr('linkname');
            dispositivoAlterado.nm_linkName = getNomeDoLinkName(dispositivoAlterado.linkname);
            dispositivoAlterado.ds_linkname = getDescricaoDoLinkname(dispositivoAlterado.linkname);
            dispositivoAlterado.texto = texto;
            $elementoAlterado.attr('linkname', `${dispositivoAlterado.linkname}_replaced`);
            $elementoAlterado.addClass('alterado');
            $elementoAlterado.find(`a[name=${dispositivoAlterado.linkname}]`).attr('name', `${dispositivoAlterado.linkname}_replaced`);
            $elementoAlterado.attr('replaced_by', normaAlteradora.ch_norma);
            if(dispositivoAlterado.texto){
                $elementoAlterado.after(`<p linkname="${dispositivoAlterado.linkname}" class="adicionado"><button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoCadastrar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><a name="${dispositivoAlterado.linkname}"></a>${dispositivoAlterado.texto}&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacaoSelecionado.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a></p>`);
                $elementoAlterado.html(`<s>${$elementoAlterado.html()}</s>`);
            }
            else{
                dispositivoAlterado.linkname += '_replaced'
                $elementoAlterado.html(`<button type="button" class="clean" onclick="desfazerAlteracaoDoDispositivoCadastrar('${dispositivoAlterado.linkname}')"><img src="${_urlPadrao}/Imagens/ico_undo_p.png" width="14px" height="14px" /></button><s>${$elementoAlterado.html()}</s>&nbsp;<a href="${linkNormaAlteradora}">(${dispositivoAlterado.nm_linkName ? dispositivoAlterado.nm_linkName + ' ' : ''}${tipoDeRelacaoSelecionado.ds_texto_para_alterador}(a) pelo(a) ${normaAlteradora.ds_norma})</a>`);
            }
            normaAlterada.dispositivos.push(dispositivoAlterado);
            if(IsNotNullOrEmpty(dispositivoAlterado.ds_linkname)){
                adicionarDescricaoDispositivosAlterados(dispositivoAlterado.ds_linkname);
            }
        }
    }
    changeStepper('selecionarDispositivoAlterado');
}

function desfazerAlteracaoDoDispositivoCadastrar(linkname){
    let removeIndex = -1;
    let dispositivoDesfeito = {};
    for(let i in normaAlterada.dispositivos){
        removeIndex = i;
        if(normaAlterada.dispositivos[i].linkname == linkname){
            switch(tipoDeRelacaoSelecionado.ch_tipo_relacao){
                case '1':
                    $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                    dispositivoDesfeito = normaAlterada.dispositivos.splice(removeIndex, 1);
                    break;
                default:
                    let $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}_replaced"]`);
                    if(!IsNotNullOrEmpty(normaAlterada.dispositivos[i].texto)){
                        $elementoAlterado = $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname.replace(/_replaced+/,'')}_replaced"]`);
                    }
                    const html = $elementoAlterado.find('s').html();
                    $elementoAlterado.find('s').remove();
                    $elementoAlterado.html(html);
                    if(IsNotNullOrEmpty(normaAlterada.dispositivos[i].texto)){
                        $(`#div_cad_dispositivo_alterada div.div_conteudo_arquivo p[linkname="${linkname}"]`).remove();
                    }
                    $elementoAlterado.attr('linkname', linkname);
                    $elementoAlterado.removeAttr('replaced_by');
                    $elementoAlterado.removeClass('alterado');
                    $elementoAlterado.find('button.select').show().tooltip(optionsTooltip);
                    dispositivoDesfeito = normaAlterada.dispositivos.splice(removeIndex, 1);
                    break;
            }
            if(IsNotNullOrEmpty(dispositivoDesfeito)){
                removerDescricaoDispositivosAlterados(dispositivoDesfeito[0].ds_linkname);
            }
            break;
        }
    }
    if(normaAlterada.dispositivos.length <= 0){
        changeStepper('deselecionarDispositivoAlterado');
    }
}

function changeTipoRelacao(el){
    desfazerLinkAlterador();
    if(el.value){
        const chTipoDeRelacao = $('#selectTipoDeRelacao').val();
        for(let relacao of tiposDeRelacao){
            if(relacao.ch_tipo_relacao == chTipoDeRelacao){
                tipoDeRelacaoSelecionado = relacao;
                break;
            }
        }
        switch(el.value){
            case '1':
                $('#div_tooltip_dispositivo h2').text('Informe o texto a ser acrescido.');
                break;
            case '36':
                $('#div_tooltip_dispositivo h2').text('Informe o texto renumerado.');
                break;
            default:
                $('#div_tooltip_dispositivo h2').text('Informe o texto novo, caso haja.');
                break;
        }
        if(IsNotNullOrEmpty(normaAlterada)){
            exibirControlesNormaAlteradaSelecionada();
        }
        changeStepper('selecionarTipoRelacao');
    }
    else{
        normaAlterada = {};
        $('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html('');
        resetarDispositivo('alterada');
        changeStepper('deselecionarTipoRelacao');
    }
}

function limparDispositivoSelecionado(nm_sufixo) {
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.div_conteudo_arquivo').html('');
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_conteudo_arquivo').hide();
    $('#div_cad_dispositivo_' + nm_sufixo + ' div.line_enable_replaced').hide();
}


function clickEnableReplaced(check) {
    var enabled = $(check).is(':checked');
    if(enabled){
        $('p[replaced_by]').show();
    }
    else{
        $('p[replaced_by]').hide();
    }
}

function salvarVideCadastrar(sucessoVide){
    $('#vide').val(JSON.stringify({
        relacao: tipoDeRelacaoSelecionado,
        norma_alteradora: normaAlteradora,
        norma_alterada: normaAlterada,
        ds_comentario_vide: $('#ds_comentario_vide').val()
    }));

    return fnSalvar("form_vide", "", sucessoVide);
}

function salvarArquivosVideCadastrar(sucessoVide){
    const deferredAlteradora = $.Deferred();
    const deferredAlterada = $.Deferred();
    $.when(deferredAlteradora, deferredAlterada).done(function(){
        salvarVideCadastrar(sucessoVide)
    });
    gInicio();
    limparFormatacao();
    if(!normaAlteradora.sem_arquivo){
        let htmlNormaAlteradora = "";
        if(!$('#inLecoSemCitacao').is(':checked')){
            const $conteudoArquivoNormaAlteradora = $($('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html());
            for(let i in $conteudoArquivoNormaAlteradora){
                if($conteudoArquivoNormaAlteradora[i].outerHTML){
                    if($conteudoArquivoNormaAlteradora[i].getAttribute('linkname') == normaAlteradora.dispositivos[0].linkname){
                        const $link = $($conteudoArquivoNormaAlteradora[i]).find('a[href]');
                        let link = `(_link_sistema_)Norma/${normaAlterada.ch_norma}/${normaAlterada.arquivo.filename}.html`;
                        if(normaAlterada.dispositivos.length > 0){
                            link += `#${normaAlterada.dispositivos[0].linkname}`;
                        }
                        for(let a = 0; a < $link.length; a++){
                            if($link[a].innerText == normaAlteradora.dispositivos[0].texto){
                                $link[a].setAttribute('href', link);
                                break;
                            }
                        }
                    }
                    htmlNormaAlteradora += $conteudoArquivoNormaAlteradora[i].outerHTML;
                }
            }
        }
        else{
            htmlNormaAlteradora = $('#div_cad_dispositivo_alteradora div.div_conteudo_arquivo').html();
        }
        $('#form_arquivo_norma_alteradora textarea[name="arquivo"]').val(window.encodeURI(htmlNormaAlteradora));
        $('#form_arquivo_norma_alteradora input[name=nm_arquivo]').val(normaAlteradora.arquivo.filename);
        var sucessoNormaAlteradora = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_cad_vide' + id_form, data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "id_file")) {
                normaAlteradora.arquivo_novo = data;
            }
        }
        $.ajaxlight({
            sFormId: 'form_arquivo_norma_alteradora',
            sUrl: './ashx/Arquivo/UploadHtml.ashx',
            sType: "POST",
            fnSuccess: sucessoNormaAlteradora,
            fnComplete: function(){
                deferredAlteradora.resolve();
            },
            fnBeforeSubmit: null,
            bAsync: true
        });
    }
    else{
        deferredAlteradora.resolve();
    }
    if(!normaAlterada.in_norma_fora_do_sistema && !normaAlterada.sem_arquivo){
        let htmlNormaAlterada = "";
        const $conteudoArquivoNormaAlterada = $($('#div_cad_dispositivo_alterada div.div_conteudo_arquivo').html());
        for(let i = 0; i < $conteudoArquivoNormaAlterada.length; i++){
            if($conteudoArquivoNormaAlterada[i].outerHTML){
                htmlNormaAlterada += $conteudoArquivoNormaAlterada[i].outerHTML;
            }
        }
        $('#form_arquivo_norma_alterada textarea[name="arquivo"]').val(window.encodeURI(htmlNormaAlterada));
        $('#form_arquivo_norma_alterada input[name=nm_arquivo]').val(normaAlterada.arquivo.filename);

        var sucessoNormaAlterada = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_cad_vide' + id_form, data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "id_file")) {
                normaAlterada.arquivo_novo = data;
            }
        }
        $.ajaxlight({
            sFormId: 'form_arquivo_norma_alterada',
            sUrl: './ashx/Arquivo/UploadHtml.ashx',
            sType: "POST",
            fnSuccess: sucessoNormaAlterada,
            fnComplete: function(){
                deferredAlterada.resolve();
            },
            fnBeforeSubmit: null,
            bAsync: true
        });
    }
    else{
        deferredAlterada.resolve();
    }
    return false;
}

function changeStepper(action){
    switch(action){
        case 'selecionarNormaAlteradora':
        case 'deselecionarTipoRelacao':
            //exibe o select tipo de relação
            $('.step2').removeClass('hidden');
            //oculta o selecionar norma alterada, comentário e botão de salvar
            $('.step3').addClass('hidden');
            $('.step4').addClass('hidden');
            //marca e desmarca os steppers
            $('li.norma-alteradora').addClass('active');
            $('li.relacao').removeClass('active');
            $('li.norma-alterada').removeClass('active');
            $('li.dispositivo-alterador').removeClass('active');
            $('li.dispositivo-alterado').removeClass('active');
            break;
        case 'selecionarTipoRelacao':
            //oculta o selecionar norma alterada, comentário e botão de salvar
            $('.step3').removeClass('hidden');
            $('.step4').addClass('hidden');
            //marca e desmarca os steppers
            $('li.relacao').addClass('active');
            if(!IsNotNullOrEmpty(normaAlterada)){
                $('li.norma-alterada').removeClass('active');
            }
            $('li.dispositivo-alterador').removeClass('active');
            $('li.dispositivo-alterado').removeClass('active');
            break;
        case 'selecionarNormaAlterada':
        case 'deselecionarDispositivoAlterador':
            //oculta comentário e botão de salvar
            $('.step4').addClass('hidden');
            //marca e desmarca os steppers
            $('li.norma-alterada').addClass('active');
            if(normaAlteradora.sem_arquivo){
                $('li.dispositivo-alterador').addClass('active');
            }
            else{
                $('li.dispositivo-alterador').removeClass('active');
            }
            
            $('li.dispositivo-alterado').removeClass('active');
            break;
        case 'selecionarDispositivoAlterador':
        case 'deselecionarDispositivoAlterado':
            //oculta comentário e botão de salvar
            $('.step4').addClass('hidden');
            //marca e desmarca os steppers
            $('li.dispositivo-alterador').addClass('active');
            $('li.dispositivo-alterado').removeClass('active');
            break;
        case 'selecionarDispositivoAlterado':
            //exibe comentário e botão de salvar
            $('.step4').removeClass('hidden');
            //marca e desmarca os steppers
            $('li.dispositivo-alterado').addClass('active');
            break;
    }
}