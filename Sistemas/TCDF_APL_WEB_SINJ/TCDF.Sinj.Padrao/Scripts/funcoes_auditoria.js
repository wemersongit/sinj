function DetalhesAcesso(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sContent: data.error_message,
            sType: "error",
            sWidth: ""
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        $('#div_ds_login').text(data.ds_login == "SINJ.PUSH" ? "Push" : "Sistema");
        $('#div_nm_user_acesso').text(data.nm_user_acesso);
        if (IsNotNullOrEmpty(data.nr_ip_usuario)) {
            var ips = "<div style='text-align:left;'>";
            if (isJson(data.nr_ip_usuario)) {
                var json = JSON.parse(data.nr_ip_usuario);
                if (IsNotNullOrEmpty(json.ip)){
                    for (var j = 0; j < json.ip.length; j++) {
                        ips = ips + json.ip[j] + "<br>";
                    }
                }
            } else {
                ips = data.nr_ip_usuario;
            }
            ips = ips + "</div>";
            $('#div_nr_ip_usuario').html(ips);
        }

        $('#div_dt_acesso').text(data.dt_acesso);
        $('#div_ip_servidor_porta').text(data.ip_servidor_porta);
        if (IsNotNullOrEmpty(data.ds_browser)) {
            var info = platform.parse(getVal(data.ds_browser));
            var saida = "";
            saida = saida + "<b>Nome: </b>" + getVal(info.name) + " <br>";
            saida = saida + "<b>Versão: </b>" + getVal(info.version) + " <br>";
            saida = saida + "<b>Produto: </b>" + getVal(info.product) + " <br>";
            saida = saida + "<b>Empresa: </b>" + getVal(info.manufacturer) + " <br>";
            saida = saida + "<b>Motor: </b>" + getVal(info.layout) + " <br>";
            saida = saida + "<b>OS: </b>" + getVal(info.os) + " <br>";
            $('#div_ds_browser').html(saida);
        }
        if(IsNotNullOrEmpty(data.ds_obs_login)){
            var msg = JSON.parse(data.ds_obs_login);
            if (msg.login) {
                msg.login = "Acesso realizado com sucesso!!!";
            }
            $('#div_ds_obs_login').html(montarTabelaJson(msg, "Informações do Acesso"));

        }
    }
}

function DetalhesErroSistema(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sTitle: "Erro",
            sContent: data.error_message,
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        var nm_operacao = operacoes_parse[data.ch_operacao];
        if(!IsNotNullOrEmpty(nm_operacao)){
            nm_operacao = data.ch_operacao;
        }
        $('#div_operacao').text(nm_operacao);
        $('#div_tipo_erro').text(data.nm_tipo);
        $('#div_dt_log_erro').text(data.dt_log_erro);
        $('#div_nm_user_erro').text(data.nm_user_erro);

        if (IsNotNullOrEmpty(data.nr_ip_usuario)) {
            var ips = "<div style='text-align:left;'>";
            if (isJson(data.nr_ip_usuario)) {
                var json = JSON.parse(data.nr_ip_usuario);
                if (IsNotNullOrEmpty(json.ip)){
                    for (var j = 0; j < json.ip.length; j++) {
                        ips = ips + json.ip[j] + "<br>";
                    }
                }
            } else {
                ips = data.nr_ip_usuario;
            }
            ips = ips + "</div>";
            $('#div_nr_ip_usuario').html(ips);
        }
        if (IsNotNullOrEmpty(data.ds_browser)) {
            var info = platform.parse(getVal(data.ds_browser));
            var saida = "";
            saida = saida + "<b>Nome: </b>" + getVal(info.name) + " <br>";
            saida = saida + "<b>Versão: </b>" + getVal(info.version) + " <br>";
            saida = saida + "<b>Produto: </b>" + getVal(info.product) + " <br>";
            saida = saida + "<b>Empresa: </b>" + getVal(info.manufacturer) + " <br>";
            saida = saida + "<b>Motor: </b>" + getVal(info.layout) + " <br>";
            saida = saida + "<b>OS: </b>" + getVal(info.os) + " <br>";
            $('#div_ds_browser').html(saida);
        }
        $('#div_ds_erro').html(montarTabelaJson(JSON.parse(data.ds_erro), "Detalhes do Erro"));
    }
}

function DetalhesErroExtracao(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sTitle: "Erro",
            sContent: data.error_message,
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        var nm_base = (data.nm_base.toLowerCase() == "sinj_norma" ? "Norma" : (data.nm_base.toLowerCase() == "sinj_diario" ? "Diário" : "Não identificado"));
        var link_arquivo = (data.nm_base.toLowerCase() == "sinj_norma" ? "BaixarArquivoNorma.aspx" : (data.nm_base.toLowerCase() == "sinj_diario" ? "BaixarArquivoDiario.aspx" : ""));
        if (link_arquivo != "") {
            link_arquivo += "?id_file=" + data.id_file_orig;
        }
        var link_detalhes = (data.nm_base.toLowerCase() == "sinj_norma" ? "DetalhesDeNorma.aspx" : (data.nm_base.toLowerCase() == "sinj_diario" ? "DetalhesDeDiario.aspx" : ""));
        if (link_detalhes != "") {
            link_detalhes += "?id_doc=" + data.id_doc_orig;
        }
        $('#div_nm_base').text(nm_base);
        $('#div_dt_error').text(data.dt_error);
        $('#div_error_msg').html(montarTabelaJson(JSON.parse(data.error_msg), "Detalhes do Erro"));

        $('#div_link_documento').html('<a href="./' + link_detalhes + '" title="Visualizar Detalhes"><img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" alt="detalhes"/></a>');
        $('#div_link_arquivo').html('<a href="./' + link_arquivo + '" title="Baixar o arquivo"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download"/> ' + data.file_name + '</a>');
    }
}

function DetalhesErroIndexacao(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sTitle: "Erro",
            sContent: data.error_message,
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        var nm_base = (data.nm_base.toLowerCase() == "sinj_norma" ? "Norma" : (data.nm_base.toLowerCase() == "sinj_diario" ? "Diário" : "Não identificado"));
        
        var link_detalhes = (data.nm_base.toLowerCase() == "sinj_norma" ? "DetalhesDeNorma.aspx" : (data.nm_base.toLowerCase() == "sinj_diario" ? "DetalhesDeDiario.aspx" : ""));
        if (link_detalhes != "") {
            link_detalhes += "?id_doc=" + data.id_doc_orig;
        }
        $('#div_nm_base').text(nm_base);
        $('#div_dt_error').text(data.dt_error);
        $('#div_error_msg').text(data.error_msg);

        $('#div_link_documento').html('<a href="./' + link_detalhes + '" title="Visualizar Detalhes"><img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" alt="detalhes"/></a>');
    }
}

function DetalhesOperacao(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sTitle: "Erro",
            sContent: data.error_message,
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        $('#div_nm_operacao').text(operacoes_parse[data.ch_operacao]);
        $('#div_dt_inicio').text(data.dt_inicio);
        $('#div_nm_user_operacao').text(data.nm_user_operacao);
        var json_parse = JSON.parse(data.ds_operacao_detalhes);
        $('#div_ds_operacao_detalhes').html(montarTabelaJson(json_parse, "Detalhes da Operação"));
        if(data.ch_operacao == "NOR.INC" || data.ch_operacao == "NOR.EDT"){
            $('#div_link_registro').html('<a href="./DetalhesDeNorma.aspx?id_norma='+json_parse.operacao.registro.ch_norma+'" >visualizar documento <img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" alt="detalhes"/></a>');
        }
        else if(data.ch_operacao == "NOR.VIS"){
            $('#div_link_registro').html('<a href="./DetalhesDeNorma.aspx?id_norma='+json_parse.operacao.ch_doc+'" >visualizar documento <img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" alt="detalhes"/></a>');
        }
    }
}

function DetalhesNotificacao(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sContent: data.error_message,
            sType: "error",
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        var ds_operacao_detalhes = JSON.parse(data.ds_operacao_detalhes);
        for(var i = 0; i < ds_operacao_detalhes.operacao.emails.length; i++){
            $('#div_emails').append(ds_operacao_detalhes.operacao.emails[i] + "<br/>");
        }
        $('#div_dt_inicio').text(data.dt_inicio);
        $('#div_nm_user_operacao').text(data.nm_user_operacao);
        $('#div_assunto').text(ds_operacao_detalhes.operacao.assunto);
        $('#div_mensagem').html(ds_operacao_detalhes.operacao.mensagem);
    }
}

function DetalhesPush(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao_notifiqueme').messagelight({
            sTitle: "Erro",
            sContent: data.error_message,
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
    else if (IsNotNullOrEmpty(data, 'email_usuario_push')) {
        $('#div_email_usuario_push').text(data.email_usuario_push);
        $('#div_nm_usuario_push').text(data.nm_usuario_push);
        $('#div_st_push').text(data.st_push ? "Ativo" : "Inativo");
        for (var i = 0; i < data.normas_monitoradas.length; i++) {
            $('#tbody_normas_monitoradas').append('<tr><td>' + GetText(data.normas_monitoradas[i].nm_tipo_norma_monitorada) + ' ' + GetText(data.normas_monitoradas[i].dt_assinatura_norma_monitorada) + '</td><td>' + GetText(data.normas_monitoradas[i].dt_cadastro_norma_monitorada) + '</td><td>' + TratarCamposBooleanos(data.normas_monitoradas[i].st_norma_monitorada) + '</td></tr>');
        }
        for (var i = 0; i < data.criacao_normas_monitoradas.length; i++) {
            $('#tbody_criacao_normas').append('<tr><td>' + GetText(data.criacao_normas_monitoradas[i].nm_tipo_norma_criacao) + '</td><td>' + GetText(data.criacao_normas_monitoradas[i].primeiro_conector_criacao) + '</td><td>' + GetText(data.criacao_normas_monitoradas[i].nm_orgao_criacao) + '</td><td>' + GetText(data.criacao_normas_monitoradas[i].segundo_conector_criacao) + '</td><td>' + GetText(data.criacao_normas_monitoradas[i].nm_termo_criacao) + '</td><td>' + TratarCamposBooleanos(data.criacao_normas_monitoradas[i].st_criacao) + '</td></tr>');
        }
        for (var i = 0; i < data.termos_diarios_monitorados.length; i++) {
            $('#tbody_termos_diarios').append('<tr><td>' + GetText(data.termos_diarios_monitorados[i].nm_tipo_fonte_diario_monitorado) + '</td><td>' + GetText(data.termos_diarios_monitorados[i].ds_termo_diario_monitorado) + '</td><td>' + (data.termos_diarios_monitorados[i].in_exata_diario_monitorado ? "Exata" : "Aproximada") + '</td><td>' + TratarCamposBooleanos(data.termos_diarios_monitorados[i].st_termo_diario_monitorado) + '</td></tr>');
        }
    }
}

function selecionarTodos(element) {
    var _class = $(element).attr('class_childs');
    if(IsNotNullOrEmpty(_class)){
        push_checked_all = $(element).prop('checked');
        $('.' + _class).prop('checked', $(element).prop('checked'));
    }
}

function preencherEmail() {
    $('#div_notificacao').messagelight("destroy");
    var checkeds_push = $('.check_push:checked');
    if (checkeds_push.length > 0) {
        $('#form_email input[name="email"]').remove();

        for (var i = 0; i < checkeds_push.length; i++) {
            $('#form_email').append('<input type="hidden" name="email" value="' + checkeds_push[i].value + '" />');
        }

        if ($("#modal_email").hasClass('ui-dialog-content')) {
            $('#modal_email').modallight("open");
        }
        else {
            $('#modal_email').modallight({
                sTitle: "E-mail",
                sType: "default",
                oButtons: [
                        { html: '<img alt="send" src="' + _urlPadrao + '/Imagens/ico_email_p.png" />Enviar', click:
                            function () {
                                enviarEmail();
                            }
                        },
                        { html: '<img alt="clear" src="' + _urlPadrao + '/Imagens/ico_eraser_p.png" />Limpar', click: function () { $(this).dialog('close'); } }
                    ],
                sWidth: 500
            });
        }
    }
    else {
        $('#div_notificacao').messagelight({
            sContent: "É necessário selecionar um ou mais destinatários.",
            sType: "error",
            sWidth: ""
        });
    }
}

function enviarEmail() {
    try {
        Validar(form_email);
        var sucesso = function (data) {
            $('#super_loading').hide();
            if (IsNotNullOrEmpty(data, 'error_message')) {
                $('#div_notificacao_email').messagelight({
                    sContent: data.error_message,
                    sType: "error"
                });
            }
            else if (IsNotNullOrEmpty(data ,'alert_message')) {
                $('#div_notificacao_email').messagelight({
                    sContent: data.alert_message,
                    sType: "alert"
                });
            }
            else if (IsNotNullOrEmpty(data ,'info_message')) {
                $('#div_notificacao_email').messagelight({
                    sContent: data.info_message,
                    sType: "info"
                });
            }
            else if (IsNotNullOrEmpty(data ,'success_message')) {
                $('#div_notificacao_email').messagelight({
                    sContent: data.success_message,
                    sType: "success"
                });
            }
            else {
                $('#div_notificacao_email').messagelight({
                    sContent: "O sistema não retornou uma mensagem de confirmação.",
                    sType: "alert"
                });
            }
        }
        var beforeSubmit = function () {
            $('#super_loading').show();
        };
        $.ajaxlight({
            bAsync: true,
            sUrl: "./ashx/Email/EnviarEmail.ashx",
            sType: "POST",
            fnSuccess: sucesso,
            sFormId: "form_email",
            fnBeforeSubmit: beforeSubmit
        });
    }
    catch (ex) {
        $("#div_notificacao_email").messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
    }
}

function DetalhesLixeira(data) {
    if (IsNotNullOrEmpty(data, 'error_message')) {
        $('#div_notificacao').messagelight({
            sTitle: "Erro",
            sContent: data.error_message,
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
    else if (IsNotNullOrEmpty(data, '_metadata.id_doc')) {
        var map_bases = {
            "sinj_norma" : "Norma",
            "sinj_diario" : "Diário",
            "sinj_orgao" : "Órgão",
            "sinj_vocabulario" : "Vocabulário",
            "sinj_autoria" : "Autoria"
        };
        var nm_base = map_bases[data.nm_base_excluido] || "Não identificado";

        $('#div_nm_base').text(nm_base);
        $('#div_dt_exclusao').text(data.dt_exclusao);
        $('#div_usuario_exclusao').text(data.nm_login_usuario_exclusao);
        $('#div_registro').html(montarTabelaJson(JSON.parse(data.json_doc_excluido),"Registro Excluído"));
    }
}

function VerificarCamposPorBase(base) {
    var mapCamposBases = {
        "sinj_vocabulario": [
            {
                "label": "Nome",
                "campo": "nm_termo"
            },
            {
                "label": "Tipo",
                "campo": "ch_tipo_termo"
            },
            {
                "label": "Lista",
                "campo": "nm_lista_superior"
            },
            {
                "label": "Usuário Cadastrador",
                "campo": "nm_login_usuario_cadastro"
            },
            {
                "label": "Data de Cadastro",
                "campo": "dt_cadastro"
            }
        ],
        "sinj_orgao": [
            {
                "label": "Nome",
                "campo": "nm_orgao"
            },
            {
                "label": "Sigla",
                "campo": "sg_orgao"
            },
            {
                "label": "Hierarquia",
                "campo": "nm_hierarquia"
            },
            {
                "label": "Início de Vigência",
                "campo": "dt_inicio_vigencia"
            },
            {
                "label": "Fim de Vigência",
                "campo": "dt_fim_vigencia"
            },
            {
                "label": "Usuário Cadastrador",
                "campo": "nm_login_usuario_cadastro"
            },
            {
                "label": "Data de Cadastro",
                "campo": "dt_cadastro"
            }
        ],
            "sinj_autoria": [
            {
                "label": "Nome",
                "campo": "nm_autoria"
            },
            {
                "label": "Usuário Cadastrador",
                "campo": "nm_login_usuario_cadastro"
            },
            {
                "label": "Data de Cadastro",
                "campo": "dt_cadastro"
            }
        ],
        "sinj_diario": [
            {
                "label": "Tipo Fonte:",
                "campo": "nm_tipo_fonte"
            },
            {
                "label": "Número:",
                "campo": "nr_diario"
            },
            {
                "label": "Seção:",
                "campo": "secao_diario"
            },
            {
                "label": "Data de Assinatura:",
                "campo": "dt_assinatura"
            },
            {
                "label": "Usuário Cadastrador",
                "campo": "nm_login_usuario_cadastro"
            },
            {
                "label": "Data de Cadastro",
                "campo": "dt_cadastro"
            }
        ]
    };
    if (IsNotNullOrEmpty(mapCamposBases[base])) {
        return mapCamposBases[base];
    }
    else return null;
}

function VisualizarExcluido(json, id_doc, base) {
    var oCamposBases = VerificarCamposPorBase(base);    
    try {
        var oJsonExcluido = JSON.parse(json);
        if (IsNotNullOrEmpty(oCamposBases)) {
            var $html = $('<div id="div_registro_excluido" class="mauto table"></div>');
            for (var i = 0; i < oCamposBases.length; i++) {
                if (IsNotNullOrEmpty(oJsonExcluido[oCamposBases[i]["campo"]])) {
                    var linha = '<div class="line">' +
                                '<div class="column w-30-pc">' +
                                    '<div class="cell fr">' +
                                        '<label> ' + oCamposBases[i]["label"] + '</label>' +
                                    '</div>' +
                                '</div>' +
                                '<div class="column w-60-pc">' +
                                    '<div class="cell">' +
                                        oJsonExcluido[oCamposBases[i]["campo"]];
                    '</div>' +
                                '</div>' +
                            '</div>';
                    $html.append(linha);
                }
            }
            $('<div id="modal_registro_excluido"/>').modallight({
                sTitle: "Registro Excluído",
                sContent: $html,
                sType: "success",
                oButtons: [{ text: "Fechar", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    $(this).remove();
                }
            });
        }
        else throw "Não foi possível visualizar o registro."
    } catch (e) {
        $('<div id="modal_registro_excluido"/>').modallight({
            sTitle: "Registro Excluído",
            sContent: e,
            sType: "alert",
            oButtons: [{ text: "Fechar", click: function () { $(this).dialog('close'); } }],
            fnClose: function () {
                $(this).remove();
            }
        });
    }
}

function RestaurarExcluido(json_excluido) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, "error_message")) {
            $('<div id="modal_registro_restaurado"/>').modallight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                oButtons: [{ text: "Fechar", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    $(this).remove();
                }
            });
        }
        else {
            $('<div id="modal_registro_restaurado"/>').modallight({
                sTitle: "Registro Restaurado",
                sContent: data.success_message,
                sType: "success",
                oButtons: [
                    { 
                        text: "Visualizar", 
                        click: function () { 
                            var map_bases = {
                                "sinj_norma" : "Norma",
                                "sinj_diario" : "Diario",
                                "sinj_orgao" : "Orgao",
                                "sinj_vocabulario" : "Vocabulario",
                                "sinj_autoria" : "Autoria"
                            };
                            var link = "./DetalhesDe"+map_bases[data.base]+".aspx?id_doc=" + data.id_doc;
                            window.location.href = "./DetalhesDe"+map_bases[data.base]+".aspx?id_doc=" + data.id_doc;
                        } 
                    },
                    { 
                        text: "Fechar", 
                        click: function () { 
                            window.location.href = "./PesquisarLixeira.aspx";
                        } 
                    },
                ],
                fnClose: function () {
                    $(this).remove();
                }
            });
        }
    }
    
    $.ajaxlight({
        sUrl: "./ashx/RestaurarExcluido.ashx?json_excluido=" + JSON.stringify(json_excluido),
        sType: "POST",
        fnSuccess: sucesso,
        bAsync: true
    });

}

var operacoes = [
    {ch_operacao:"NOR.INC",nm_operacao:"Incluir Norma"},
    {ch_operacao:"NOR.EDT",nm_operacao:"Editar Norma"},
    {ch_operacao:"NOR.EXC",nm_operacao:"Excluir Norma"},
    {ch_operacao:"NOR.VIS",nm_operacao:"Visualizar Norma"},
    {ch_operacao:"NOR.PES",nm_operacao:"Pesquisa Norma"},
    {ch_operacao:"NOR.DWN",nm_operacao:"Download de Norma"},
    {ch_operacao:"NOR.EDT,VIDE.INC",nm_operacao:"Incluir Vide"},
    {ch_operacao:"NOR.EDT,VIDE.EDT",nm_operacao:"Editar Vide"},
    {ch_operacao:"NOR.EDT,VIDE.EXC",nm_operacao:"Excluir Vide"},
    {ch_operacao:"AUT.INC",nm_operacao:"Incluir Autoria"},
    {ch_operacao:"AUT.EDT",nm_operacao:"Editar Autoria"},
    {ch_operacao:"AUT.EXC",nm_operacao:"Excluir Autoria"},
    {ch_operacao:"AUT.VIS",nm_operacao:"Visualizar Autoria"},
    {ch_operacao:"AUT.PES",nm_operacao:"Pesquisa Autoria"},
    {ch_operacao:"ORG.INC",nm_operacao:"Incluir Órgão"},
    {ch_operacao:"ORG.EDT",nm_operacao:"Editar Órgão"},
    {ch_operacao:"ORG.EXC",nm_operacao:"Excluir Órgão"},
    {ch_operacao:"ORG.PES",nm_operacao:"Pesquisar Órgão"},
    {ch_operacao:"ORG.VIS",nm_operacao:"Visualizar Órgão"},
    {ch_operacao:"DIO.INC",nm_operacao:"Incluir Diário"},
    {ch_operacao:"DIO.EDT",nm_operacao:"Editar Diário"},
    {ch_operacao:"DIO.EXC",nm_operacao:"Excluir Diário"},
    {ch_operacao:"DIO.PES",nm_operacao:"Pesquisar Diário"},
    {ch_operacao:"DIO.VIS",nm_operacao:"Visualizar Diário"},
    {ch_operacao:"TDF.INC",nm_operacao:"Incluir Tipo de Fonte"},
    {ch_operacao:"TDF.EDT",nm_operacao:"Editar Tipo de Fonte"},
    {ch_operacao:"TDF.EXC",nm_operacao:"Excluir Tipo de Fonte"},
    {ch_operacao:"TDF.PES",nm_operacao:"Pesquisar Tipo de Fonte"},
    {ch_operacao:"TDF.VIS",nm_operacao:"Visualizar Tipo de Fonte"},
    {ch_operacao:"TDN.INC",nm_operacao:"Incluir Tipo de Norma"},
    {ch_operacao:"TDN.EDT",nm_operacao:"Editar Tipo de Norma"},
    {ch_operacao:"TDN.EXC",nm_operacao:"Excluir Tipo de Norma"},
    {ch_operacao:"TDN.PES",nm_operacao:"Pesquisar Tipo de Norma"},
    {ch_operacao:"TDN.VIS",nm_operacao:"Visualizar Tipo de Norma"},
    {ch_operacao:"TDP.INC",nm_operacao:"Incluir Tipo de Publicação"},
    {ch_operacao:"TDP.EDT",nm_operacao:"Editar Tipo de Publicação"},
    {ch_operacao:"TDP.EXC",nm_operacao:"Excluir Tipo de Publicação"},
    {ch_operacao:"TDP.PES",nm_operacao:"Pesquisar Tipo de Publicação"},
    {ch_operacao:"TDP.VIS",nm_operacao:"Visualizar Tipo de Publicação"},
    {ch_operacao:"INT.INC",nm_operacao:"Incluir Interessado"},
    {ch_operacao:"INT.EDT",nm_operacao:"Editar Interessado"},
    {ch_operacao:"INT.EXC",nm_operacao:"Excluir Interessado"},
    {ch_operacao:"INT.PES",nm_operacao:"Pesquisar Interessado"},
    {ch_operacao:"INT.VIS",nm_operacao:"Visualizar Interessado"},
    {ch_operacao:"SIT.INC",nm_operacao:"Incluir Situação"},
    {ch_operacao:"SIT.EDT",nm_operacao:"Editar Situação"},
    {ch_operacao:"SIT.EXC",nm_operacao:"Excluir Situação"},
    {ch_operacao:"SIT.PES",nm_operacao:"Pesquisar Situação"},
    {ch_operacao:"SIT.VIS",nm_operacao:"Visualizar Situação"},
    {ch_operacao:"TDR.INC",nm_operacao:"Incluir Tipo de Relação"},
    {ch_operacao:"TDR.EDT",nm_operacao:"Editar Tipo de Relação"},
    {ch_operacao:"TDR.EXC",nm_operacao:"Excluir Tipo de Relação"},
    {ch_operacao:"TDR.PES",nm_operacao:"Pesquisar Tipo de Relação"},
    {ch_operacao:"TDR.VIS",nm_operacao:"Visualizar Tipo de Relação"},
    {ch_operacao:"RQE.INC",nm_operacao:"Incluir Requerente"},
    {ch_operacao:"RQE.EDT",nm_operacao:"Editar Requerente"},
    {ch_operacao:"RQE.EXC",nm_operacao:"Excluir Requerente"},
    {ch_operacao:"RQE.PES",nm_operacao:"Pesquisar Requerente"},
    {ch_operacao:"RQE.VIS",nm_operacao:"Visualizar Requerente"},
    {ch_operacao:"RQI.INC",nm_operacao:"Incluir Requerido"},
    {ch_operacao:"RQI.EDT",nm_operacao:"Editar Requerido"},
    {ch_operacao:"RQI.EXC",nm_operacao:"Excluir Requerido"},
    {ch_operacao:"RQI.PES",nm_operacao:"Pesquisar Requerido"},
    {ch_operacao:"RQI.VIS",nm_operacao:"Visualizar Requerido"},
    {ch_operacao:"REL.INC",nm_operacao:"Incluir Relator"},
    {ch_operacao:"REL.EDT",nm_operacao:"Editar Relator"},
    {ch_operacao:"REL.EXC",nm_operacao:"Excluir Relator"},
    {ch_operacao:"REL.PES",nm_operacao:"Pesquisar Relator"},
    {ch_operacao:"REL.VIS",nm_operacao:"Visualizar Relator"},
    {ch_operacao:"PRO.INC",nm_operacao:"Incluir Procurador"},
    {ch_operacao:"PRO.EDT",nm_operacao:"Editar Procurador"},
    {ch_operacao:"PRO.EXC",nm_operacao:"Excluir Procurador"},
    {ch_operacao:"PRO.PES",nm_operacao:"Pesquisar Procurador"},
    {ch_operacao:"PRO.VIS",nm_operacao:"Visualizar Procurador"},
    {ch_operacao:"USR.INC",nm_operacao:"Incluir Usuário"},
    {ch_operacao:"USR.EDT",nm_operacao:"Editar Usuário"},
    {ch_operacao:"USR.EXC",nm_operacao:"Excluir Usuário"},
    {ch_operacao:"USR.PES",nm_operacao:"Pesquisar Usuário"},
    {ch_operacao:"USR.VIS",nm_operacao:"Visualizar Usuário"},
    {ch_operacao:"USR.SAI",nm_operacao:"Logoff"},
    {ch_operacao:"RIO.PES",nm_operacao:"Relatório"},
    {ch_operacao:"VOC.INC",nm_operacao:"Incluir Vocabulário"},
    {ch_operacao:"VOC.EDT",nm_operacao:"Editar Vocabulário"},
    {ch_operacao:"VOC.EXC",nm_operacao:"Excluir Vocabulário"},
    {ch_operacao:"VOC.PES",nm_operacao:"Pesquisar Vocabulário"},
    {ch_operacao:"VOC.VIS",nm_operacao:"Visualizar Vocabulário"},
    {ch_operacao:"VOC.GER",nm_operacao:"Gerenciar Vocabulário"},
    {ch_operacao:"AUD.ERR.EXT.PES",nm_operacao:"Auditoria - Pesquisar Erro de Extração"},
    {ch_operacao:"AUD.ERR.EXT.VIS",nm_operacao:"Auditoria - Visualizar Erro de Extração"},
    {ch_operacao:"AUD.ERR.IND.PES",nm_operacao:"Auditoria - Pesquisar Erro de Indexação"},
    {ch_operacao:"AUD.ERR.IND.VIS",nm_operacao:"Auditoria - Visualizar Erro de Indexação"},
    {ch_operacao:"AUD.ERR.SIS.PES",nm_operacao:"Auditoria - Pesquisar Erro de Sistema"},
    {ch_operacao:"AUD.ERR.SIS.VIS",nm_operacao:"Auditoria - Visualizar Erro de Sistema"},
    {ch_operacao:"AUD.PUS.PES",nm_operacao:"Auditoria - Pesquisar Push"},
    {ch_operacao:"AUD.PUS.VIS",nm_operacao:"Auditoria - Visualizar Push"},
    {ch_operacao:"AUD.PUS.EMAIL",nm_operacao:"Auditoria - Notificar Usuários do Push"},
    {ch_operacao:"AUD.OPE.PES",nm_operacao:"Auditoria - Pesquisar Operação"},
    {ch_operacao:"AUD.OPE.VIS",nm_operacao:"Auditoria - Visualizar Operação"},
    {ch_operacao:"AUD.LIX.PES",nm_operacao:"Auditoria - Pesquisar Lixeira"},
    {ch_operacao:"AUD.LIX.VIS",nm_operacao:"Auditoria - Visualizar Lixeira"},
    {ch_operacao:"AUD.LIX.RES",nm_operacao:"Auditoria - Restaurar da Lixeira"},
    {ch_operacao:"AUD.ACE.PES",nm_operacao:"Auditoria - Pesquisar Acesso"},
    {ch_operacao:"AUD.ACE.VIS",nm_operacao:"Auditoria - Visualizar Acesso"},
    {ch_operacao:"AUD.PES.EST",nm_operacao:"Auditoria - Estatísticas de Pesquisa"},
    {ch_operacao:"AUD.SES.EXC",nm_operacao:"Auditoria - Excluir Sessões"},
    {ch_operacao:"UPL.sinj_norma",nm_operacao:"Arquivo - Upload de Norma"},
    {ch_operacao:"UPL.sinj_diario",nm_operacao:"Arquivo - Upload de Diário"},
    {ch_operacao:"ARQ.PRO.INC",nm_operacao:"Editor HTML - Criar Arquivo"},
    {ch_operacao:"ARQ.PRO.EDT",nm_operacao:"Editor HTML - Editar Arquivo"},
    {ch_operacao:"ARQ.PRO.EXC",nm_operacao:"Editor HTML - Excluir Arquivo"},
    {ch_operacao:"ARQ.PRO.IMP",nm_operacao:"Editor HTML - Importar Arquivo na Norma"},
    {ch_operacao:"CFG.EDT",nm_operacao:"Editar Configuração"},
    {ch_operacao:"CFG.VIS",nm_operacao:"Visualizar Configuração"},
    {ch_operacao:"APP.PUS.EMAIL"}
]

var operacoes_parse = {
    "NOR.INC":"Incluir Norma",
    "NOR.EDT":"Editar Norma",
    "NOR.EXC":"Excluir Norma",
    "NOR.VIS":"Visualizar Norma",
    "NOR.PES":"Pesquisa Norma",
    "NOR.DWN":"Download de Norma",
    "NOR.EDT,VIDE.INC":"Incluir Vide",
    "NOR.EDT,VIDE.EDT":"Editar Vide",
    "NOR.EDT,VIDE.EXC":"Excluir Vide",
    "AUT.INC":"Incluir Autoria",
    "AUT.EDT":"Editar Autoria",
    "AUT.EXC":"Excluir Autoria",
    "AUT.VIS":"Visualizar Autoria",
    "AUT.PES":"Pesquisa Autoria",
    "ORG.INC":"Incluir Órgão",
    "ORG.EDT":"Editar Órgão",
    "ORG.EXC":"Excluir Órgão",
    "ORG.PES":"Pesquisar Órgão",
    "ORG.VIS":"Visualizar Órgão",
    "DIO.INC":"Incluir Diário",
    "DIO.EDT":"Editar Diário",
    "DIO.EXC":"Excluir Diário",
    "DIO.PES":"Pesquisar Diário",
    "DIO.VIS":"Visualizar Diário",
    "DIO.DWN":"Download de Diário",
    "TDF.INC":"Incluir Tipo de Fonte",
    "TDF.EDT":"Editar Tipo de Fonte",
    "TDF.EXC":"Excluir Tipo de Fonte",
    "TDF.PES":"Pesquisar Tipo de Fonte",
    "TDF.VIS":"Visualizar Tipo de Fonte",
    "TDN.INC":"Incluir Tipo de Norma",
    "TDN.EDT":"Editar Tipo de Norma",
    "TDN.EXC":"Excluir Tipo de Norma",
    "TDN.PES":"Pesquisar Tipo de Norma",
    "TDN.VIS":"Visualizar Tipo de Norma",
    "TDP.INC":"Incluir Tipo de Publicação",
    "TDP.EDT":"Editar Tipo de Publicação",
    "TDP.EXC":"Excluir Tipo de Publicação",
    "TDP.PES":"Pesquisar Tipo de Publicação",
    "TDP.VIS":"Visualizar Tipo de Publicação",
    "INT.INC":"Incluir Interessado",
    "INT.EDT":"Editar Interessado",
    "INT.EXC":"Excluir Interessado",
    "INT.PES":"Pesquisar Interessado",
    "INT.VIS":"Visualizar Interessado",
    "SIT.INC":"Incluir Situação",
    "SIT.EDT":"Editar Situação",
    "SIT.EXC":"Excluir Situação",
    "SIT.PES":"Pesquisar Situação",
    "SIT.VIS":"Visualizar Situação",
    "TDR.INC":"Incluir Tipo de Relação",
    "TDR.EDT":"Editar Tipo de Relação",
    "TDR.EXC":"Excluir Tipo de Relação",
    "TDR.PES":"Pesquisar Tipo de Relação",
    "TDR.VIS":"Visualizar Tipo de Relaçã",
    "RQE.INC":"Incluir Requerente",
    "RQE.EDT":"Editar Requerente",
    "RQE.EXC":"Excluir Requerente",
    "RQE.PES":"Pesquisar Requerente",
    "RQE.VIS":"Visualizar Requerente",
    "RQI.INC":"Incluir Requerido",
    "RQI.EDT":"Editar Requerido",
    "RQI.EXC":"Excluir Requerido",
    "RQI.PES":"Pesquisar Requerido",
    "RQI.VIS":"Visualizar Requerido",
    "REL.INC":"Incluir Relator",
    "REL.EDT":"Editar Relator",
    "REL.EXC":"Excluir Relator",
    "REL.PES":"Pesquisar Relator",
    "REL.VIS":"Visualizar Relator",
    "PRO.INC":"Incluir Procurador",
    "PRO.EDT":"Editar Procurador",
    "PRO.EXC":"Excluir Procurador",
    "PRO.PES":"Pesquisar Procurador",
    "PRO.VIS":"Visualizar Procurador",
    "USR.INC":"Incluir Usuário",
    "USR.EDT":"Editar Usuário",
    "USR.EXC":"Excluir Usuário",
    "USR.PES":"Pesquisar Usuário",
    "USR.VIS":"Visualizar Usuário",
    "USR.SAI":"Logoff",
    "RIO.PES":"Relatório",
    "VOC.INC":"Incluir Vocabulário",
    "VOC.EDT":"Editar Vocabulário",
    "VOC.EXC":"Excluir Vocabulário",
    "VOC.PES":"Pesquisar Vocabulário",
    "VOC.VIS":"Visualizar Vocabulário",
    "VOC.GER":"Gerenciar Vocabulário",
    "AUD.ERR.EXT.PES":"Auditoria - Pesquisar Erro de Extração",
    "AUD.ERR.EXT.VIS":"Auditoria - Visualizar Erro de Extração",
    "AUD.ERR.IND.PES":"Auditoria - Pesquisar Erro de Indexação",
    "AUD.ERR.IND.VIS":"Auditoria - Visualizar Erro de Indexação",
    "AUD.ERR.SIS.PES":"Auditoria - Pesquisar Erro de Sistema",
    "AUD.ERR.SIS.VIS":"Auditoria - Visualizar Erro de Sistema",
    "AUD.PUS.PES":"Auditoria - Pesquisar Push",
    "AUD.PUS.VIS":"Auditoria - Visualizar Push",
    "AUD.PUS.EMAIL":"Auditoria - Notificar Usuários do Push",
    "AUD.OPE.PES":"Auditoria - Pesquisar Operação",
    "AUD.OPE.VIS":"Auditoria - Visualizar Operação",
    "AUD.LIX.PES":"Auditoria - Pesquisar Lixeira",
    "AUD.LIX.VIS":"Auditoria - Visualizar Lixeira",
    "AUD.LIX.RES":"Auditoria - Restaurar da Lixeira",
    "AUD.ACE.PES":"Auditoria - Pesquisar Acesso",
    "AUD.ACE.VIS":"Auditoria - Visualizar Acesso",
    "AUD.PES.EST":"Auditoria - Estatísticas de Pesquisa",
    "AUD.SES.EXC":"Auditoria - Excluir Sessões",
    "UPL.sinj_norma":"Arquivo - Upload de Norma",
    "UPL.sinj_diario":"Arquivo - Upload de Diário",
    "ARQ.PRO.INC":"Editor HTML - Criar Arquivo",
    "ARQ.PRO.EDT":"Editor HTML - Editar Arquivo",
    "ARQ.PRO.EXC":"Editor HTML - Excluir Arquivo",
    "ARQ.PRO.IMP":"Editor HTML - Importar Arquivo na Norma",
    "CFG.EDT":"Editar Configuração",
    "CFG.VIS":"Visualizar Configuração",
    "APP.PUS.EMAIL": "PUSH - Notificar Usuários"
};

function getOperacao(ch_operacao){
    var operacao = operacoes_parse[ch_operacao];
    if(isNullOrEmpty(operacao)){
        operacao = ch_operacao;
    }
    return operacao;
}