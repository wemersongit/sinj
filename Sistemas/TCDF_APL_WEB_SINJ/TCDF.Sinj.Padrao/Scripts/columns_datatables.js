function TratarCamposBooleanos(data) {
    return '<input type="checkbox" ' + (data ? 'checked="true"' : '') + ' disabled="disabled" />';
}

var _columns_norma = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
		"mRender": function (data, type, full) {
			return '<a href="./DetalhesDeNorma.aspx?id_norma=' + full.ch_norma + '" title="Visualizar detalhes da norma" />' + full.nm_tipo_norma + " " + (full.nr_norma != null ? full.nr_norma : "") + ' <img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
		}
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
		"mRender": function (data) {
			var origens = "";
			for (var i = 0; i < data.length; i++) {
				origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + data[i].ch_orgao + '\')">' + data[i].sg_orgao + '</a>';
			}
			return origens;
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidt": "", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false },
	{ "indice": 4, "isControl": false, "stadnard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Texto Integral", "sWidth": "110px", "sClass": "center ws all", "bSortable": false,
	    "mRender": function (data, type, full) {
	        var id_file = "";
	        var links = "";
	        var tipo = "";
			if (IsNotNullOrEmpty(full.ar_atualizado, 'id_file')) {
			    id_file = full.ar_atualizado.id_file;
			    tipo = full.ar_atualizado.mimetype.split('/')[1];
			}
            else if (full.fontes.length > 0) {
                for (var i = 0; i < full.fontes.length; i++) {
                    if (IsNotNullOrEmpty(full.fontes[i].ar_fonte, 'id_file')) {
                        id_file = full.fontes[i].ar_fonte.id_file;
                        tipo = full.fontes[i].ar_fonte.mimetype.split('/')[1];
                    }
                }
            }
            if (IsNotNullOrEmpty(id_file)) {
                var filename = getTitleNorma(full);

                links = '<a title="baixar arquivo" target="_blank" href="./Norma/' + full.ch_norma + '/' + filename + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' +
						'&nbsp;&nbsp;' +
						'<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a>';
			}
			return links;
		}
	}
];

var _columns_norma_es = [
	{ "indice": 0, "isControl": true, "standard_view": true, "sTitle": '<a title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(null, true);" class="center"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /><b>TODOS</b></a>', "sWidth": "60px", "sClass": "grid-cell ws center", "mData": "_score", "bSortable": false, "visible": window.location.href.indexOf("st_habilita_pesquisa!=true") == -1,
	    "mRender": function (data, type, full) {
	        if (!IsNotNullOrEmpty(window.aHighlight)) {
	            aHighlight = [];
	        }
	        aHighlight.push({ highlight: full.highlight, nm_base: 'sinj_norma', doc: full._source._metadata.id_doc, action: './DetalhesDeNorma.aspx?id_norma=' + full._source.ch_norma });
	        return '<a valor="sinj_norma_' + full._source._metadata.id_doc + '" title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(\'sinj_norma_' + full._source._metadata.id_doc + '\');"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /></a>';
	        //        return '<input class="check_cesta" type="checkbox" value="sinj_norma_' + full._source._metadata.id_doc + '" />';
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relevância", "sWidth": "", "sClass": "grid-cell ws", "mData": "_score", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._score;
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma", "visible": false,
	    "mRender": function (data, type, full) {
	        return '<button title="Detalhes" name="btn" type="submit" title="Detalhes" class="link" onclick="javascript:doc_clicked=' + full._source._metadata.id_doc + ';base_clicked=\'sinj_norma\'"><H2>' + full._source.nm_tipo_norma + " " + full._source.nr_norma + '</H2><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /> </button>';
	    }
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._source.dt_assinatura;
	    }
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        var origens = "";
	        for (var i = 0; i < full._source.origens.length; i++) {
	            origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + full._source.origens[i].ch_orgao + '\')">' + full._source.origens[i].sg_orgao + '</a>';
	        }
	        return origens;
	    }
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "", "sClass": "grid-cell ws ds_ementa max-w-30-pc", "mData": "ds_ementa", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        return full._source.ds_ementa;
	    }
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._source.nm_situacao;
	    }
	},
	{ "indice": 5, "isControl": false, "standard_view": true, "sTitle": "Texto Integral", "sWidth": "10%", "sClass": "left ws all", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        var id_file = "";
	        var links = "";
	        var medida = "";
	        if (IsNotNullOrEmpty(full._source.ar_atualizado, 'id_file')) {
	            id_file = full._source.ar_atualizado.id_file;
	            medida = '<br/><b>' + (full._source.ar_atualizado.filesize / 1024).toFixed(0) + ' KB</b><br/>';
	        }
	        else if (IsNotNullOrEmpty(full._source.fontes) && full._source.fontes.length > 0 && IsNotNullOrEmpty(full._source.fontes[0].ar_fonte, 'id_file')) {
	            id_file = full._source.fontes[0].ar_fonte.id_file;
	            medida = '<br/><b>' + (full._source.fontes[0].ar_fonte.filesize / 1024).toFixed(0) + ' KB</b><br/>';
	        }
	        if (IsNotNullOrEmpty(id_file)) {
	            links = '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoNorma.aspx?id_norma=' + full._source.ch_norma + '"><img src="' + _urlPadrao + '/Imagens/ico_download_m.png" alt="download" /></a>';
	            if (_aplicacao == "CADASTRO") {
	                links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" /></a>';
	            }
	        }
	        var icone = '<div class="fr" style="margin:2px;">' + links + '<br/>' + medida + '</div>';
	        return icone;
	    }
	},
	{ "indice": 6, "isControl": false, "standard_view": true, "sTitle": "&nbsp;", "sWidth": "", "sClass": "left ws", "bSortable": false,
	    "mRender": function (data, type, full) {
	        var html = '<div class="table w-100-pc">';
	        html += '<div class="line">' +
                '<div class="column">' +
	                '<button title="Detalhes TERA" name="btn" type="submit" class="link" onclick="javascript:doc_clicked=' + full._source._metadata.id_doc + ';base_clicked=\'sinj_norma\'"><H2><span class="nm_tipo_norma">' + full._source.nm_tipo_norma + '</span> <span class="nr_norma nr_norma_text">' + full._source.nr_norma + '</span> de <span class="dt_assinatura dt_assinatura_text">' + full._source.dt_assinatura + (full._source.nm_tipo_norma.toLowerCase() == "parecer normativo" ? "*" : "") + ' <span class="st_norma nm_situacao">' + full._source.nm_situacao + '</span></H2><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /> </button>' +
                    (full._source.nm_tipo_norma.toLowerCase() == "parecer normativo" ? '<br/><span class="obs">*Data de Despacho do Governador</span>' : '') +
                    (full._source.st_vacatio_legis && IsNotNullOrEmpty(full._source.dt_inicio_vigencia) && convertStringToDateTime(full._source.dt_inicio_vigencia) > convertStringToDateTime(date_now) ? '<br/><span class="obs">*'+(IsNotNullOrEmpty(full._source.ds_vacatio_legis) ? full._source.ds_vacatio_legis : 'Entrará em vigor a partir de <span class="dt_inicio_vigencia">' + full._source.dt_inicio_vigencia + '</span>')+'</span>' : '') +
				'</div>' +
			'</div>';
	        var origens = "";
	        for (var i = 0; i < full._source.origens.length; i++) {
	            origens += (origens != "" ? "<br/>" : "") + full._source.origens[i].sg_orgao + ' - ' + full._source.origens[i].nm_orgao;
	        }
	        html += '<div class="line">' +
                '<div class="column w-10-pc">' +
	                '<label>Origem:</label>' +
	            '</div>' +
                '<div class="column w-90-pc sg_orgao nm_orgao">' +
	                origens +
	            '</div>' +
            '</div>';

	        html += '<div class="line">' +
                '<div class="column w-10-pc">' +
	                '<label>Ementa:</label>' +
	            '</div>' +
                '<div class="column w-90-pc text-justify ds_ementa">' +
	                full._source.ds_ementa +
	            '</div>' +
            '</div>';

	        var id_file = "";
	        var nm_file = getTitleNorma(full._source);
	        var links = "";
	        var url_file = '';
	        var medida = "";
	        if (IsNotNullOrEmpty(full._source.ar_atualizado, 'id_file')) {
	            id_file = full._source.ar_atualizado.id_file;
	            url_file = './Norma/' + full._source.ch_norma + '/' + nm_file;
	            medida = '<b>(' + (full._source.ar_atualizado.filesize / 1024).toFixed(0) + ' KB)</b>';
	        }
	        else if (IsNotNullOrEmpty(full._source.fontes) && full._source.fontes.length > 0 && IsNotNullOrEmpty(full._source.fontes[0].ar_fonte, 'id_file')) {
	            id_file = full._source.fontes[0].ar_fonte.id_file;
	            url_file = './Norma/' + full._source.ch_norma + '/' + nm_file;
	            medida = '<b>(' + (full._source.fontes[0].ar_fonte.filesize / 1024).toFixed(0) + ' KB)</b>';
	        }
	        if (IsNotNullOrEmpty(url_file)) {
	            links = '<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_m.png" alt="download" width="20px" /> ' + medida + '</a>';
	            if (_aplicacao == "CADASTRO") {
	                links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" /></a>';
	            }
	        }

	        if (links != '') {
	            html += '<div class="line">' +
                '<div class="column w-10-pc">' +
	                '<label>Texto Integral:</label>' +
	            '</div>' +
                '<div class="column w-90-pc">' +
	                links +
	            '</div>' +
            '</div></div>';
	        }
	        return html;
	    }
	},	

	{ "indice": 2, "isControl": true, "standard_view": true, "sWidth": "320px","sTitle":"<h2>Habilitar</h2>" , "sClass": "grid-cell ws center", "mData": "full", "bSortable": false, "visible": window.location.href.indexOf("st_habilita_pesquisa!=true&st_habilita_email=false") > 0,
		"mRender": function (data, type, full) {
			var htmlToAppend = ""
			if(ValidarPermissao(_grupos.nor_eml) && !full._source.st_habilita_email){
				htmlToAppend = htmlToAppend + `<div class="div-light-button" style="float:right; display: flex;"><a onclick="CriarModalEnviarEmail( ${full._source._metadata.id_doc}, ${full._source.st_habilita_pesquisa} )"><img src="${_urlPadrao}/Imagens/ico_email_p.png">Enviar e-mail</a></div>
				<div class="div-light-button" style="float:right; display: flex;"><a onclick="CriarModalDesabilitarEmail( ${full._source._metadata.id_doc}, ${full._source.st_habilita_pesquisa} )"><img src="${_urlPadrao}/Imagens/ico_stop_email_p.png">Desabilitar e-mail</a></div>`;	
			}
			if(ValidarPermissao(_grupos.nor_hsp) && !full._source.st_habilita_pesquisa){
				htmlToAppend = htmlToAppend + `<div class="div-light-button" style="float:right;"><a onclick="CriarModalHabilitarPesquisa( ${full._source._metadata.id_doc}, ${full._source.st_habilita_email} )"><img src="${_urlPadrao}/Imagens/ico_doc_m.png">Habilitar no SINJ pesquisa</a></div>`;
			}
			return htmlToAppend;
		}
		
	},
	{ "indice": 2, "isControl": true, "standard_view": true, "sWidth": "120px","sTitle":"" , "sClass": "grid-cell ws center", "mData": "full", "bSortable": false, "visible": window.location.href.indexOf("st_habilita_pesquisa!=true&st_habilita_email=false") == -1,
		"mRender": function (data, type, full) {
			var htmlToAppend = "";
			if(!full._source.st_habilita_pesquisa || !full._source.st_habilita_email){
				htmlToAppend = "<label style='color:red'>Norma pendente</label>"
			}
			return htmlToAppend;
		}
		
	},


];
var _columns_norma_favoritos = $.extend(true, [], _columns_norma_es);
_columns_norma_favoritos[0] = { "indice": 0, "isControl": true, "standard_view": true, "sTitle": ' ', "sWidth": "30px", "sClass": "grid-cell ws", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return '<div class="button_favoritos_' + full._source.ch_norma + '" style="width:30px;"><a href="javascript:void(0);" title="Remover da lista de favoritos" onclick="javascript:RemoverFavoritos(\'.button_favoritos_' + full._source.ch_norma + '\',\'norma_' + full._source.ch_norma + '\');" ><img alt="*" src="' + _urlPadrao + '/Imagens/ico_del_fav.png" /></a></div>';
    }
};
_columns_norma_favoritos[8].mRender = function (data, type, full) {
    var html = '<div class="table w-100-pc">';
	html += '<div class="line">' +
        '<div class="column">' +
	        '<H2><a title="Detalhes" class="clear" href="./DetalhesDeNorma.aspx?id_norma=' + full._source.ch_norma + '"><span class="nm_tipo_norma">' + full._source.nm_tipo_norma + '</span> <span class="nr_norma nr_norma_text">' + full._source.nr_norma + '</span> de <span class="dt_assinatura dt_assinatura_text">' + full._source.dt_assinatura + ' <span class="st_norma nm_situacao">' + full._source.nm_situacao + '</span> <img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /> </a></H2>' +
	    '</div>' +
    '</div>';
	if (full._source.st_vacatio_legis && IsNotNullOrEmpty(full._source.dt_inicio_vigencia)) {
	    html += '<div class="line">' +
            '<div class="column w-10-pc">' +
	            '<label>Vacatio Legis:</label>' +
	        '</div>' +
            '<div class="column w-90-pc text-justify">' +
	            'Em vigor a partir de <span class="dt_inicio_vigencia">' + full._source.dt_inicio_vigencia + '</span>' +
	        '</div>' +
        '</div>';
	}
	var origens = "";
	for (var i = 0; i < full._source.origens.length; i++) {
	    origens += (origens != "" ? "<br/>" : "") + full._source.origens[i].sg_orgao + ' - ' + full._source.origens[i].nm_orgao;
	}
	html += '<div class="line">' +
        '<div class="column w-10-pc">' +
	        '<label>Origem:</label>' +
	    '</div>' +
        '<div class="column w-90-pc sg_orgao nm_orgao">' +
	        origens +
	    '</div>' +
    '</div>';
	html += '<div class="line">' +
        '<div class="column w-10-pc">' +
	        '<label>Ementa:</label>' +
	    '</div>' +
        '<div class="column w-90-pc text-justify ds_ementa">' +
	        full._source.ds_ementa +
	    '</div>' +
    '</div>';

	var id_file = "";
	var nm_file = getTitleNorma(full._source);
	var links = "";
	var url_file = '';
	var medida = "";
	if (IsNotNullOrEmpty(full._source.ar_atualizado, 'id_file')) {
	    id_file = full._source.ar_atualizado.id_file;
	    url_file = './Norma/' + full._source.ch_norma + '/' + nm_file;
	    medida = '<b>(' + (full._source.ar_atualizado.filesize / 1024).toFixed(0) + ' KB)</b>';
	}
	else if (IsNotNullOrEmpty(full._source.fontes) && full._source.fontes.length > 0 && IsNotNullOrEmpty(full._source.fontes[0].ar_fonte, 'id_file')) {
	    id_file = full._source.fontes[0].ar_fonte.id_file;
	    url_file = './Norma/' + full._source.ch_norma + '/' + nm_file;
	    medida = '<b>(' + (full._source.fontes[0].ar_fonte.filesize / 1024).toFixed(0) + ' KB)</b>';
	}
	if (IsNotNullOrEmpty(url_file)) {
	    links = '<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_m.png" alt="download" width="20px" /> ' + medida + '</a>';
	    if (_aplicacao == "CADASTRO") {
	        links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" /></a>';
	    }
	}

	if (links != '') {
	    html += '<div class="line">' +
        '<div class="column w-10-pc">' +
	        '<label>Texto Integral:</label>' +
	    '</div>' +
        '<div class="column w-90-pc">' +
	        links +
	    '</div>' +
    '</div></div>';
	}
	return html;
}

var _columns_norma_cesta = $.extend(true, [], _columns_norma_es);
_columns_norma_cesta[0] = {
    "indice": 0,
    "isControl": false,
    "standard_view": true,
    "sTitle": " ",
    "sWidth": "110px",
    "sClass": "center ws",
    "bSortable": false,
    "mRender": function (data, type, full) {
        if (!IsNotNullOrEmpty(window.aHighlight)) {
            aHighlight = [];
        }
        aHighlight.push({ highlight: full.highlight, nm_base: 'sinj_norma', doc: full._source._metadata.id_doc, action: './DetalhesDeNorma.aspx?id_norma=' + full._source.ch_norma });
        return "<a title='Remover da Cesta' href='javascript:ExcluirDaCesta(\"sinj_norma_" + full._source._metadata.id_doc + "\");' class='a_delete' ><img valign='absmiddle' alt='Excluir'  src='" + _urlPadrao + "/Imagens/ico_trash_p.png'  /></a>";
    }
};

var _columns_orgao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "20%", "sClass": "grid-cell ws nm_orgao", "mData": "nm_orgao" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Sigla", "sWidth": "", "sClass": "grid-cell ws sg_hierarquia", "mData": "",
		"mRender":function(data, type, full){
			return '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoHierarquia(\''+full.ch_hierarquia+'\')">'+ full.sg_hierarquia + '</a>';
		}
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Início de Vigência", "sWidth": "", "sClass": "grid-cell ws dt_inicio_vigencia", "mData": "dt_inicio_vigencia" },
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Fim de Vigência", "sWidth": "", "sClass": "grid-cell ws dt_fim_vigencia", "mData": "dt_fim_vigencia" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Status", "sWidth": "", "sClass": "center ws st_orgao", "mData": "st_orgao",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeOrgao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.org_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarOrgao.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.org_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_diario = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sWidth": "10%", "sClass": "grid-cell ws nm_tipo_fonte", "mData": "nm_tipo_fonte" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Número", "sWidth": "", "sClass": "grid-cell ws nr_diario", "mData": "nr_diario",
	    "mRender": function (data, type, full) {
	        return full.nr_diario + (IsNotNullOrEmpty(full.cr_diario) ? " " + full.cr_diario : "");
	    }
	},
	{ "indice": 2, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": "Seção", "sWidth": "", "sClass": "grid-cell ws secao_diario", "mData": "secao_diario" },
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo de Edição", "sWidth": "15%", "sClass": "grid-cell ws nm_tipo_edicao", "mData": "nm_tipo_edicao" },
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Diferencial Edição", "sDefaultContent": "", "sWidth": "", "sClass": "grid-cell ws nm_diferencial_edicao", "mData": "nm_diferencial_edicao", "bSortable": false },
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Sumplemento", "sDefaultContent": "", "sWidth": "", "sClass": "grid-cell ws nm_diferencial_suplemento", "mData": "nm_diferencial_suplemento", "bSortable": false },
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Data de Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura", "sorting": "desc" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Pendente", "sWidth": "", "sClass": "center ws st_pendente", "mData": "st_pendente",
	    "mRender": function (data) {
	        return TratarCamposBooleanos(data);
	    }
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Detalhes", "sWidth": "50px", "sClass": "center ws all", "mData": "", "bSortable": false,
	    "mRender": function (data, type, full) {
	        return "&nbsp;<a title='Detalhes' href='./DetalhesDeDiario.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
	    }
	},
	{ "indice": 5, "isControl": false, "standard_view": true, "sTitle": "Texto Integral", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
	    "mRender": function (data, type, full) {
	        var txt_retorno = "";
	        if (IsNotNullOrEmpty(full, 'ar_diario.filename')) {
	            var arquivo = '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoDiario.aspx?id_file=' + full.ar_diario.id_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a><br/>';
	            var texto = '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + full.ar_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a>';

	            txt_retorno += texto + arquivo;
	        }
	        else if (IsNotNullOrEmpty(full, 'arquivos')) {
	            for (var i = 0; i < full.arquivos.length; i++) {
	                var obj_arquivo = full.arquivos[i];
	                var arquivo = '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoDiario.aspx?id_file=' + obj_arquivo.arquivo_diario.id_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' + obj_arquivo.ds_arquivo + '<br/>';
	                var texto = '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + obj_arquivo.arquivo_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a>';
	                txt_retorno += texto + arquivo;
	            }
	        }
	        return txt_retorno;
	    }
	}
];

var _columns_diario_es = [
	{ "indice": 0, "isControl": true, "standard_view": true, "sTitle": '<a title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(null, true);"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /><b>TODOS</b></a>', "sWidth": "60px", "sClass": "grid-cell ws center", "mData": "", "bSortable": false,
	    "mRender": function (data, type, full) {
	        if (!IsNotNullOrEmpty(window.aHighlight)) {
	            aHighlight = [];
	        }
	        aHighlight.push({ highlight: full.highlight, nm_base: 'sinj_diario', doc: full._source._metadata.id_doc, action: "./DetalhesDeDiario.aspx?id_doc=" + full._source._metadata.id_doc });
	        return '<a valor="sinj_diario_' + full._source._metadata.id_doc + '" title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(\'sinj_diario_' + full._source._metadata.id_doc + '\');"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /></a>';
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relevância", "sWidth": "", "sClass": "grid-cell ws", "mData": "_score", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._score;
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sWidth": "", "sClass": "grid-cell ws nm_tipo_fonte", "mData": "nm_tipo_fonte", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._source.nm_tipo_fonte;
	    }
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Número", "sWidth": "", "sClass": "grid-cell ws nr_diario", "mData": "nr_diario", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._source.nr_diario + (IsNotNullOrEmpty(full._source.cr_diario) ? " " + full._source.cr_diario : "");
	    }
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Seção", "sWidth": "", "sClass": "grid-cell ws secao_diario", "mData": "secao_diario", "visible": false,
	    "mRender": function (data, type, full) {
	        return getVal(full._source.secao_diario);
	    }
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Data de Publicação", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._source.dt_assinatura;
	    }
	},
	{ "indice": 5, "isControl": false, "standard_view": true, "sTitle": "Texto Integral", "sWidth": "", "sClass": "left ws all", "mData": "", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        //        return icone + campo_highlight;
	        return "";
	    }
	},
    { "indice": 6, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "left ws", "bSortable": false,
        "mRender": function (data, type, full) {
            var buttonTitle = '<H2>' + montarSpanDescricaoDiario(full._source) + '</H2>';
            if (_aplicacao == "CADASTRO") {
                buttonTitle = '<button title="Detalhes" name="btn" type="submit" class="link" onclick="javascript:doc_clicked=' + full._source._metadata.id_doc + ';base_clicked=\'sinj_diario\'">' + buttonTitle + '<img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></button>';
            }
            var campo_highlight = '';
            var id_file = "";
            var nm_file = "";
            var links = "";
            var url_file = '';
            var medida = "";
            if (IsNotNullOrEmpty(full._source, 'ar_diario.id_file')) {
                id_file = full._source.ar_diario.id_file;
                nm_file = full._source.ar_diario.filename;
                url_file = './Diario/' + full._source.ch_diario + '/' + id_file + '/arq/' + nm_file;
                campo_highlight = MontarHighlight(full.highlight, 'ar_diario.filetext');
                medida = '<b>(' + (full._source.ar_diario.filesize / 1024).toFixed(0) + ' KB)</b>';

                links = '<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="20px" /> ' + medida + '</a>';
                if (_aplicacao == "CADASTRO") {
                    links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + full._source.ar_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" height="20px" /></a>';
                }
            }
            else {
                campo_highlight = MontarHighlight(full.highlight, 'arquivos.arquivo_diario.filetext');
                for (var i = 0; i < full._source.arquivos.length; i++) {
                    var obj_arquivo = full._source.arquivos[i];

                    id_file = obj_arquivo.arquivo_diario.id_file;
                    nm_file = obj_arquivo.arquivo_diario.filename;
                    url_file = './Diario/' + full._source.ch_diario + '/' + id_file + '/arq/' + i + '/' + nm_file;
                    medida = '<b>(' + (obj_arquivo.arquivo_diario.filesize / 1024).toFixed(0) + ' KB)</b>';

                    links += '<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="20px" /> ' + medida + '</a>';
                    if (_aplicacao == "CADASTRO") {
                        links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + obj_arquivo.arquivo_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" height="20px" /></a>';
                    }
                    links += '&nbsp;&nbsp;' + obj_arquivo.ds_arquivo + '<br/>';

                }
            }

            if (IsNotNullOrEmpty(campo_highlight)) {
                campo_highlight = campo_highlight.replace(/## img.* ##/g, '').replace(/##/g, '');
            }

            var html = '<div class="table w-100-pc">'+
                '<div class="line">' +
                    '<div class="column w-80-pc">' +
                        buttonTitle +
	                    (campo_highlight.indexOf("<span class='highlight'>") > -1 ? '<br/>' + campo_highlight : '') +
	                '</div>' +
                    '<div class="column w-20-pc"><div>' +
	                    '<label>Texto Integral:</label><br/>' +
	                    links +
	                '</div>' +
                '</div>' +
            '</div>';
            return html;
        }
    }
];

var _columns_texto_diario = [
    { "indice": 5, "isControl": false, "standard_view": true, "sTitle": "<br/>", "sWidth": "", "sClass": "left ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            var medida = '';
            var links = '';
            var arquivos = '';
            var id_file = "";
            var nm_file = "";
            var url_file = "";
            if (IsNotNullOrEmpty(full.fields.partial[0], 'ar_diario.id_file')) {
                medida = (full.fields.partial[0].ar_diario.filesize / 1024).toFixed(0) + ' KB<br/>';
                links = '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + full.fields.partial[0].ar_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" /></a>';
                id_file = full.fields.partial[0].ar_diario.id_file;
                nm_file = full.fields.partial[0].ar_diario.filename;
                url_file = './Diario/' + full.fields.partial[0].ch_diario + '/' + id_file + '/arq/' + nm_file;
                links += '&nbsp;&nbsp;<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="20px" /></a>';
                var icone = '<div class="text-center">' + links + '<br/>' + medida + '</div>';
                var campo_highlight = MontarHighlight(full.highlight, 'ar_diario.filetext');
                if (IsNotNullOrEmpty(campo_highlight)) {
                    campo_highlight = campo_highlight.replace(/## img.* ##/g, '').replace(/##/g, '');
                }
                arquivos = '<div class="line"><div class="column w-80-pc">' + campo_highlight + '</div><div class="column w-20-pc">' + icone + '</div></div>';
            }
            else if (IsNotNullOrEmpty(full.fields.partial[0], 'arquivos')) {
                var campo_highlight = MontarHighlight(full.highlight, 'arquivos.arquivo_diario.filetext');
                if (IsNotNullOrEmpty(campo_highlight)) {
                    campo_highlight = campo_highlight.replace(/## img.* ##/g, '').replace(/##/g, '');
                }
                for (var i = 0; i < full.fields.partial[0].arquivos.length; i++) {
                    var obj_arquivo = full.fields.partial[0].arquivos[i];
                    medida = (obj_arquivo.arquivo_diario.filesize / 1024).toFixed(0) + ' KB<br/>';
                    id_file = obj_arquivo.arquivo_diario.id_file;
                    nm_file = obj_arquivo.arquivo_diario.filename;
                    url_file = './Diario/' + full.fields.partial[0].ch_diario + '/' + id_file + '/arq/' + i + '/' + nm_file;
                    links = '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + obj_arquivo.arquivo_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" /></a>';
                    links += '&nbsp;&nbsp;<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="20px" /></a>';
                    arquivos += '<div class="text-center"><h2>' + obj_arquivo.ds_arquivo + '</h2><br/>' + links + '<br/>' + medida + '<br/>' + '</div>';
                }
                arquivos = '<div class="line"><div class="column w-80-pc">' + campo_highlight + '</div><div class="column w-20-pc">' + arquivos + '</div></div>';
            }
            var html = '<div class="table w-100-pc"><div class="line"><div class="column w-100-pc"><div style="font-weight:bold">' + montarDescricaoDiario(full.fields.partial[0]) + '</div></div></div>' + arquivos + '</div>';
            return html;
        }
    }
];

var _columns_diario_cesta = $.extend(true, [], _columns_diario_es);
_columns_diario_cesta[0] = { "indice": 0, "isControl": true, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
    "mRender": function (data, type, full) {
        return "<a title='Remover da Cesta' href='javascript:ExcluirDaCesta(\"sinj_diario_" + full._source._metadata.id_doc + "\");' class='a_delete' ><img valign='absmiddle' alt='Excluir'  src='" + _urlPadrao + "/Imagens/ico_trash_p.png'  /></a>";
    }
};

var _columns_autoridade = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "20%", "sClass": "grid-cell ws nm_orgao", "mData": "nm_orgao" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Sigla", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "sg_orgao" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Início de Vigência", "sWidth": "", "sClass": "grid-cell ws dt_inicio_vigencia", "mData": "dt_inicio_vigencia" },
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Fim de Vigência", "sWidth": "", "sClass": "grid-cell ws dt_fim_vigencia", "mData": "dt_fim_vigencia" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Status", "sWidth": "", "sClass": "center ws st_orgao", "mData": "st_orgao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "";
			var selecionar = "<a href='javascript:void(0);' onclick='javascript:SelecionarOrgaoAutoridade(\"" + full.ch_orgao + "\",\"" + full.nm_orgao + "\",\"" + full.sg_orgao + "\",\"" + full.dt_inicio_vigencia + "\",\"" + full.dt_fim_vigencia + "\");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='"+_urlPadrao+"/Imagens/ico_ok_p.png' /></a>";
			return detalhes + selecionar;
		}
	}
];

var _columns_autoria = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Autoria", "sClass": "grid-cell ws nm_autoria", "mData": "nm_autoria" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeAutoria.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.aut_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarAutoria.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.aut_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_fonte = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Fonte", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_fonte", "mData": "nm_tipo_fonte" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_tipo_fonte", "mData": "ds_tipo_fonte" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeTipoDeFonte.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.tdf_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarTipoDeFonte.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.tdf_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_edicao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Edição", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_edicao", "mData": "nm_tipo_edicao" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_tipo_edicao", "mData": "ds_tipo_edicao" },
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Ativo <a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorStatus();' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "10%", "sToolTip": "Indica se esse tipo de Edição está ativa.", "sClass": "center grid-cell ws st_edicao tooltip", "mData": "st_edicao", "bSortable": false,
        "mRender": function (data) {
            return TratarCamposBooleanos(data);
        }
    },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
	    "mRender": function (data, type, full) {
	        var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeTipoDeEdicao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
	        var alterar = "";
	        if (ValidarPermissao(_grupos.tdf_edt)) {
	            alterar = "&nbsp;<a title='Alterar'  href='./EditarTipoDeEdicao.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='" + _urlPadrao + "/Imagens/ico_pencil_p.png'   /></a>";
	        }
	        var excluir = "";
	        if (ValidarPermissao(_grupos.tdf_exc)) {
	            excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='" + _urlPadrao + "/Imagens/ico_trash_p.png'  /></a>";
	        }
	        return detalhes + alterar + excluir;
	    }
	}
];

var _columns_tipo_de_norma = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "15%", "sClass": "grid-cell ws nm_tipo_norma", "mData": "nm_tipo_norma" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sWidth": "20%", "sClass": "grid-cell ws ds_tipo_norma", "mData": "ds_tipo_norma" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "G1", "sToolTip": "Tipos de norma que aparece o campo Autoria pra ser preenchido e pesquisado.", "sClass": "center grid-cell ws in_g1 tooltip", "mData": "in_g1",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "G2", "sToolTip": "Tipos de norma que são específicas para ações impetradas no PGDF que podem interferir na vigência de outras normas. Para estes tipos de norma deve estar disponível campos adicionais específicos e alteradas algumas regras de negocio para os campos comuns.", "sClass": "center grid-cell ws in_g2 tooltip", "mData": "in_g2",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "G3", "sToolTip": "Trata exclusivamente do tipo DODF, pois esta parte do acervo não é uma norma e tem por objetivo apenas disponibilizar o DO do DF para pesquisa. O DODF não é ato, nem norma e nem ação.", "sClass": "center grid-cell ws in_g3 tooltip", "mData": "in_g3",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "G4", "sToolTip": "Tipos de norma que aparece o campo Interessado pra ser preenchido e pesquisado (exceto as ações da PGDF).", "sClass": "center grid-cell ws in_g4 tooltip", "mData": "in_g4",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "G5", "sToolTip": "Indica os tipos de normas que podem ser cadastras no VIDES sem a necessidade da norma vinculada já estar cadastra no sistema.", "sClass": "center grid-cell ws in_g5 tooltip", "mData": "in_g5",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Conjunta", "sToolTip": "Normas conjuntas podem ser criadas por mais de um órgão de origem. Esse campo estando marcado indica ao sistema que pode permitir a selecão de mais de um órgão no cadastro da norma.", "sClass": "center grid-cell ws in_conjunta tooltip", "mData": "in_conjunta",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Questionável", "sToolTip": "Indica quais as normas são questionáveis por ações que estão sendo acompanhadas pela PGDF. Consequentemente, estes são os tipos que podem aparecer para inclusão de vides associados a uma norma do tipo ação.", "sClass": "center grid-cell ws in_questionavel tooltip", "mData": "in_questionavel",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Numeração", "sToolTip": "Indica se esse tipo de norma deve usar uma numeração a partir do orgao. Valor utilizado apenas para criação do número para a chave para não-duplicação.", "sClass": "center grid-cell ws in_numeracao_por_orgao tooltip", "mData": "in_numeracao_por_orgao",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Apelidável", "sToolTip": "Indica se esse tipo de norma pode receber apelido.", "sClass": "center grid-cell ws in_apelidavel tooltip", "mData": "in_apelidavel",
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeTipoDeNorma.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.tdn_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarTipoDeNorma.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.tdn_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_tipo_de_publicacao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Publicação", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_publicacao", "mData": "nm_tipo_publicacao" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_tipo_publicacao", "mData": "ds_tipo_publicacao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeTipoDePublicacao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.tdf_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarTipoDePublicacao.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.tdf_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_tipo_de_diario = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_diario", "mData": "nm_tipo_diario" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Sigla", "sWidth": "5%", "sClass": "grid-cell ws sg_tipo_diario", "mData": "sg_tipo_diario" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws  ds_tipo_diario", "mData": "ds_tipo_diario" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeTipoDeDiario.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.tdf_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarTipoDeDiario.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.tdf_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_interessado = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Interessado", "sWidth": "20%", "sClass": "grid-cell ws nm_interessado", "mData": "nm_interessado" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_interessado", "mData": "ds_interessado" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeInteressado.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.int_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarInteressado.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.int_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_situacao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "20%", "sClass": "grid-cell ws nm_situacao", "mData": "nm_situacao" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_situacao", "mData": "ds_situacao" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Peso", "sWidth": "20%", "sClass": "grid-cell ws nr_peso_situacao", "mData": "nr_peso_situacao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeSituacao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.int_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarSituacao.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.int_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_tipo_de_relacao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relação", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_relacao", "mData": "nm_tipo_relacao" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_tipo_relacao", "mData": "ds_tipo_relacao" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Texto para Alterador", "sWidth": "20%", "sClass": "grid-cell ws ds_texto_para_alterador", "mData": "ds_texto_para_alterador" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Texto para Alterado", "sWidth": "20%", "sClass": "grid-cell ws ds_texto_para_alterado", "mData": "ds_texto_para_alterado" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Importância", "sWidth": "20%", "sClass": "grid-cell ws nr_importancia", "mData": "nr_importancia" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relação de Ação", "sWidth": "20%", "sClass": "grid-cell ws in_relacao_de_acao", "mData": "in_relacao_de_acao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeTipoDeRelacao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.tdr_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarTipoDeRelacao.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.tdr_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_requerente = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Requerente", "sWidth": "20%", "sClass": "grid-cell ws nm_requerente", "mData": "nm_requerente" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_requerente", "mData": "ds_requerente" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeRequerente.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.rqe_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarRequerente.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.rqe_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_requerido = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Requerido", "sWidth": "20%", "sClass": "grid-cell ws nm_requerido", "mData": "nm_requerido" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_requerido", "mData": "ds_requerido" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeRequerido.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.rqi_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarRequerido.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.rqi_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_relator = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relator", "sWidth": "20%", "sClass": "grid-cell ws nm_relator", "mData": "nm_relator" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_relator", "mData": "ds_relator" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeRelator.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.rel_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarRelator.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.rel_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_procurador = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Procurador", "sWidth": "20%", "sClass": "grid-cell ws nm_procurador", "mData": "nm_procurador" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Descrição", "sClass": "grid-cell ws ds_procurador", "mData": "ds_procurador" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeProcurador.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.pro_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarProcurador.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.pro_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_usuario = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Login", "sWidth": "10%", "sClass": "grid-cell ws nm_login_usuario", "mData": "nm_login_usuario" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sClass": "grid-cell ws nm_usuario", "mData": "nm_usuario" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Ativo <a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorStatus();' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "10%", "sToolTip": "Indica se esse usuário está ativo.", "sClass": "center grid-cell ws st_usuario tooltip", "mData": "st_usuario", "bSortable": false,
		"mRender": function (data) {
			return TratarCamposBooleanos(data);
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "&nbsp;<a title='Detalhes' href='./DetalhesDeUsuario.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var alterar = "";
			if (ValidarPermissao(_grupos.usr_edt)) {
				alterar = "&nbsp;<a title='Alterar'  href='./EditarUsuario.aspx?id_doc=" + full._metadata.id_doc + "'     ><img valign='absmiddle' alt='Alterar'  src='"+_urlPadrao+"/Imagens/ico_pencil_p.png'   /></a>";
			}
			var excluir = "";
			if (ValidarPermissao(_grupos.usr_exc)) {
				excluir = "&nbsp;<a title='Excluir'  href='javascript:Excluir(" + full._metadata.id_doc + ",\"datatable\");' class='a_delete'    ><img valign='absmiddle' alt='Excluir'  src='"+_urlPadrao+"/Imagens/ico_trash_p.png'  /></a>";
			}
			return detalhes + alterar + excluir;
		}
	}
];

var _columns_vocabulario = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
	{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(PesquisarVocabulario);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "nm_termo", "bSortable": false,
		"mRender": function (data, type, full) {
			var nm_tipo_termo = "";
			if (full.ch_tipo_termo == "DE") {
				nm_tipo_termo = "Descritor";
			}
			else if (full.ch_tipo_termo == "ES") {
				nm_tipo_termo = "Especificador";
			}
			else if (full.ch_tipo_termo == "AU") {
				nm_tipo_termo = "Autoridade";
			}
			else if (full.ch_tipo_termo == "LA") {
				if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
					nm_tipo_termo = "Sublista";
				}
				else if (full.in_lista) {
					nm_tipo_termo = "Lista";
				}
				else {
					nm_tipo_termo = "Item";
				}
			}
			return nm_tipo_termo;
		}
	},
	{ "sTitle": "Aprovado", "sClass": "center", "mData": "st_aprovado",
		"mRender": function (data) {
			return (data ? '<img alt="sim" src="' + _urlPadrao + '/Imagens/ico_ok_p.png" />' : '<img alt="não" src="' + _urlPadrao + '/Imagens/ico_alert_p.png" />');
		}
	},
	{ "mData": "", "sDefaultContent": "", "sTitle": "Visualizar/Editar", "sWidth": "20%", "sClass": "center ws", "sName": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var exibir = "<a href='./DetalhesDeVocabulario.aspx?id_doc=" + full._metadata.id_doc + "' title='Exibir Detalhes'><img valign='absmiddle' alt='detalhes' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
			var editar = "<a href='./EditarVocabulario.aspx?id_doc=" + full._metadata.id_doc + "' title='Editar'><img valign='absmiddle' alt='ok' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' /></a>";
			return exibir + editar;
		}
	}
];

var _columns_vocabulario_nao_autorizados = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
	{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(ConsultarTermosNaoAutorizados);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "nm_termo", "bSortable": false,
		"mRender": function (data, type, full) {
			var nm_tipo_termo = "";
			if (full.ch_tipo_termo == "DE") {
				nm_tipo_termo = "Descritor";
			}
			else if (full.ch_tipo_termo == "ES") {
				nm_tipo_termo = "Especificador";
			}
			else if (full.ch_tipo_termo == "AU") {
				nm_tipo_termo = "Autoridade";
			}
			else if (full.ch_tipo_termo == "LA") {
				if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
					nm_tipo_termo = "Sublista";
				}
				else if (full.in_lista) {
					nm_tipo_termo = "Lista";
				}
				else {
					nm_tipo_termo = "Item";
				}
			}
			return nm_tipo_termo;
		}
	},
	{ "sTitle": "Aprovado", "sClass": "center", "mData": "st_aprovado",
		"mRender": function (data) {
			return (data ? '<img alt="sim" src="' + _urlPadrao + '/Imagens/ico_ok_p.png" />' : '<img alt="não" src="' + _urlPadrao + '/Imagens/ico_alert_p.png" />');
		}
	},
	{ "mData": "", "sDefaultContent": "", "sTitle": "Visualizar/Editar", "sWidth": "20%", "sClass": "center ws all", "sName": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var exibir = "<a href='./DetalhesDeVocabulario.aspx?id_doc=" + full._metadata.id_doc + "' title='Exibir Detalhes'><img valign='absmiddle' alt='detalhes' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
			var editar = "<a href='./EditarVocabulario.aspx?id_doc=" + full._metadata.id_doc + "' title='Editar'><img valign='absmiddle' alt='ok' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' /></a>";
			return exibir + editar;
		}
	}
];

var _columns_termos_homonimos = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws", "mData": "nm_termo" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Total", "sWidth": "150px", "sClass": "center", "mData": "nr_total" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "", "sWidth": "100px", "sClass": "center ws", "mData": "nm_termo", "bSortable": false,
		"mRender": function (data) {
			return "<a target='_blank' href='./PesquisarVocabulario.aspx?nm_termo=" + data + "' title='Pesquisar'><img valign='absmiddle' alt='detalhes' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
		}
	}
];

var _columns_vocabulario_selecionar = [
	{ "sTitle": "Termo", "sClass": "left", "mData": "nm_termo",
		"mRender": function (data, type, full) {
			return (full.in_termo_nao_autorizado && IsNotNullOrEmpty(full.nm_termo_use) ? "<s>" + full.nm_termo + "</s> - Use: " + full.nm_termo_use : full.nm_termo);
		}
	},
	{ "mData": "", "sDefaultContent": "", "sTitle": "", "sWidth": "20%", "sClass": "left ws", "sName": "", "bSortable": false,
		"mRender": function (data, type, full) {

			if (full.in_termo_nao_autorizado && IsNotNullOrEmpty(full.ch_termo_use)) {
				var display = VerificarSeTermoNaoEstaAdicionado(full.ch_termo_use) ? "" : "display:none;";
				var exibir = "<a href='javascript:void(0);' onclick='javascript:CriarModalDetalhesDoTermo(" + full._metadata.id_doc + ");'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
				var tg = "";
				var te = "";
				var tr = "";
				var tna = "";
				if (full.ch_tipo_termo == "DE") {
					tg = "<a style='" + display + "' class='a_tg' ch_termo='" + full.ch_termo_use + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo_use + "\", \"" + full.nm_termo_use + "\", \"TG\");' title='Selecionar como Termo Geral'><img valign='absmiddle' alt='TG' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_tg.png' /></a>";
					te = "<a style='" + display + "' class='a_te' ch_termo='" + full.ch_termo_use + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo_use + "\", \"" + full.nm_termo_use + "\", \"TE\");' title='Selecionar como Termo Especifico'><img valign='absmiddle' alt='TE' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_te.png' /></a>";
					tr = "<a style='" + display + "' class='a_tr' ch_termo='" + full.ch_termo_use + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo_use + "\", \"" + full.nm_termo_use + "\", \"TR\");' title='Selecionar como Termo Relacionado'><img valign='absmiddle' alt='TR' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_tr.png' /></a>";
				}
				tna = "<a style='" + display + "' class='a_tna' ch_termo='" + full.ch_termo_use + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo_use + "\", \"" + full.nm_termo_use + "\", \"TNA\");' title='Selecionar como Termo Não Autorizado'><img valign='absmiddle' alt='TNA' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_tna.png' /></a>";
				return exibir + tg + te + tr + tna;
			}
			else {
				var display = VerificarSeTermoNaoEstaAdicionado(full.ch_termo_use) ? "" : "display:none;";
				var exibir = "<a href='javascript:void(0);' onclick='javascript:CriarModalDetalhesDoTermo(" + full._metadata.id_doc + ");'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
				var tg = "";
				var te = "";
				var tr = "";
				var tna = "";
				if (full.ch_tipo_termo == "DE") {
					tg = "<a style='" + display + "' class='a_tg' ch_termo='" + full.ch_termo + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo + "\", \"" + full.nm_termo + "\", \"TG\");' title='Selecionar como Termo Geral'><img valign='absmiddle' alt='TG' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_tg.png' /></a>";
					te = "<a style='" + display + "' class='a_te' ch_termo='" + full.ch_termo + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo + "\", \"" + full.nm_termo + "\", \"TE\");' title='Selecionar como Termo Especifico'><img valign='absmiddle' alt='TE' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_te.png' /></a>";
					tr = "<a style='" + display + "' class='a_tr' ch_termo='" + full.ch_termo + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo + "\", \"" + full.nm_termo + "\", \"TR\");' title='Selecionar como Termo Relacionado'><img valign='absmiddle' alt='TR' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_tr.png' /></a>";
				}
				tna = "<a style='" + display + "' class='a_tna' ch_termo='" + full.ch_termo + "' href='javascript:void(0);' onclick='javascript:AdicionarTermo(\"" + full.ch_termo + "\", \"" + full.nm_termo + "\", \"TNA\");' title='Selecionar como Termo Não Autorizado'><img valign='absmiddle' alt='TNA' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_tna.png' /></a>";

				return exibir + tg + te + tr + tna;
			}
		}
	}
];

var _columns_vocabulario_trocar = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
	{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(PesquisarTermosTrocarTermo);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "ch_tipo_termo", "bSortable": false,
		"mRender": function (data, type, full) {
			var nm_tipo_termo = "";
			if (full.ch_tipo_termo == "DE") {
				nm_tipo_termo = "Descritor";
			}
			else if (full.ch_tipo_termo == "ES") {
				nm_tipo_termo = "Especificador";
			}
			else if (full.ch_tipo_termo == "AU") {
				nm_tipo_termo = "Autoridade";
			}
			else if (full.ch_tipo_termo == "LA") {
				if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
					nm_tipo_termo = "Sublista";
				}
				else if (full.in_lista) {
					nm_tipo_termo = "Lista";
				}
				else {
					nm_tipo_termo = "Item";
				}
			}
			return nm_tipo_termo;
		}
	},
	{ "mData": "", "sDefaultContent": "", "sTitle": "", "sWidth": "20%", "sClass": "left ws all", "sName": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var exibir = "<a href='javascript:void(0);' onclick='javascript:CriarModalDetalhesDoTermo(" + full._metadata.id_doc + ");'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' /></a>";
			var selecionar = "<a ch_termo='" + full.ch_termo + "' href='javascript:void(0);' onclick='javascript:SelecionarTrocarTermo(\"" + full._metadata.id_doc + "\", \"" + full.ch_termo + "\", \"" + full.nm_termo + "\", \"" + full.ch_tipo_termo + "\");' title='Selecionar para trocar'><img valign='absmiddle' alt='ok' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
			return exibir + selecionar;
		}
	}
];

var _columns_vocabulario_quarentena = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
	{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(ClicarAbaQuarentena);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "ch_tipo_termo", "bSortable": false,
		"mRender": function (data, type, full) {
			var nm_tipo_termo = "";
			if (full.ch_tipo_termo == "DE") {
				nm_tipo_termo = "Descritor";
			}
			else if (full.ch_tipo_termo == "ES") {
				nm_tipo_termo = "Especificador";
			}
			else if (full.ch_tipo_termo == "AU") {
				nm_tipo_termo = "Autoridade";
			}
			else if (full.ch_tipo_termo == "LA") {
				if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
					nm_tipo_termo = "Sublista";
				}
				else if (full.in_lista) {
					nm_tipo_termo = "Lista";
				}
				else {
					nm_tipo_termo = "Item";
				}
			}
			return nm_tipo_termo;
		}
	},
	{ "mData": "", "sDefaultContent": "", "sTitle": "", "sWidth": "20%", "sClass": "left ws all", "sName": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var exibir = "<a href='javascript:void(0);' onclick='javascript:CriarModalDetalhesDoTermo(" + full._metadata.id_doc + ");'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
			var restaurar = "<a href='javascript:void(0);' onclick='javascript:RestaurarTermo(" + full._metadata.id_doc + ", \"datatable_quarentena\", event);' title='Restaurar Termo'><img valign='absmiddle' alt='ok' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_undo_p.png' /></a>";
			var excluir = "<a href='javascript:void(0);' onclick='javascript:ExcluirTermo(" + full._metadata.id_doc + ", \"datatable_quarentena\");' title='Excluir Termo'><img valign='absmiddle' alt='ok' valign='absmiddle' src='"+_urlPadrao+"/Imagens/ico_delete_p.png' /></a>";
			return exibir + restaurar + excluir;
		}
	}
];

function fn_botoes_pendentes(id_doc, st_aprovado) {
    var exibir = "<a href='javascript:void(0);' onclick='javascript:CriarModalDetalhesDoTermo(" + id_doc + ");'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
    var aprovar = "<a href='javascript:void(0);' onclick='javascript:AprovarTermo(" + id_doc + "," + !st_aprovado + ", event, \"datatable_pendentes\");' title='" + (st_aprovado ? "Tornar pendente" : "Aprovar Termo") + "'><img valign='absmiddle' alt='" + (st_aprovado ? "aprovado" : "pendente") + "' valign='absmiddle' src='"+_urlPadrao+"/Imagens/" + (st_aprovado ? "ico_ok_p" : "ico_alert_p") + ".png' /></a>";
    var loading = "<div class='loading-p' style='display:none;'></div>";
    return exibir + aprovar;
}

var _columns_vocabulario_pendentes = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
	{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(ClicarAbaPendentes);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "ch_tipo_termo", "bSortable": false,
		"mRender": function (data, type, full) {
			var nm_tipo_termo = "";
			if (full.ch_tipo_termo == "DE") {
				nm_tipo_termo = "Descritor";
			}
			else if (full.ch_tipo_termo == "ES") {
				nm_tipo_termo = "Especificador";
			}
			else if (full.ch_tipo_termo == "AU") {
				nm_tipo_termo = "Autoridade";
			}
			else if (full.ch_tipo_termo == "LA") {
				if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
					nm_tipo_termo = "Sublista";
				}
				else if (full.in_lista) {
					nm_tipo_termo = "Lista";
				}
				else {
					nm_tipo_termo = "Item";
				}
			}
			return nm_tipo_termo;
		}
	},
	{ "mData": "", "sDefaultContent": "", "sTitle": "", "sWidth": "20%", "sClass": "left ws all", "sName": "", "bSortable": false,
		"mRender": function (data, type, full) {
			return fn_botoes_pendentes(full._metadata.id_doc, full.st_aprovado);
		}
	}
];

function fn_botoes_inativar(id_doc, st_ativo) {
    var exibir = "<a href='javascript:void(0);' onclick='javascript:CriarModalDetalhesDoTermo(" + id_doc + ");'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>"
    var inativar = "<a class='inativar' href='javascript:void(0);' onclick='javascript:InativarTermo(" + id_doc + ", " + !st_ativo + ", event);' title='" + (st_ativo ? "Inativar" : "Ativar") + " Termo'><img valign='absmiddle' alt='" + (st_ativo ? "inativar" : "ativar") + "' valign='absmiddle' src='"+_urlPadrao+"/Imagens/" + (st_ativo ? "ico_lock_p" : "ico_unlock_p") + ".png' /></a>"
    var loading = "<div class='loading-p' style='display:none;'></div>";
    return exibir + inativar + loading;
}

var _columns_vocabulario_inativos = [
{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(ClicarAbaInativos);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "ch_tipo_termo", "bSortable": false,
    "mRender": function (data, type, full) {
        var nm_tipo_termo = "";
        if (full.ch_tipo_termo == "DE") {
            nm_tipo_termo = "Descritor";
        }
        else if (full.ch_tipo_termo == "ES") {
            nm_tipo_termo = "Especificador";
        }
        else if (full.ch_tipo_termo == "AU") {
            nm_tipo_termo = "Autoridade";
        }
        else if (full.ch_tipo_termo == "LA") {
            if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
                nm_tipo_termo = "Sublista";
            }
            else if (full.in_lista) {
                nm_tipo_termo = "Lista";
            }
            else {
                nm_tipo_termo = "Item";
            }
        }
        return nm_tipo_termo;
    }
},
{ "mData": "", "sDefaultContent": "", "sTitle": "", "sWidth": "20%", "sClass": "left ws all", "sName": "", "bSortable": false,
    "mRender": function (data, type, full) {
        return fn_botoes_inativar(full._metadata.id_doc, full.st_ativo);
    }
}
];

var _columns_vocabulario_restaurados = [
{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws", "mData": "",
    "mRender": function (data, type, full) {
        return "<input name='ch_termo' type='checkbox' ch_termo='" + full.ch_termo + "' nm_termo='" + full.nm_termo + "' />";
    }
},
{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termo", "sClass": "grid-cell ws nm_termo", "mData": "nm_termo" },
{ "sTitle": "Tipo de Termo" + "<a href='javascript:void(0);' onclick='javascript:AbrirModalFiltrarPorTipo(ClicarAbaRestaurados);' class='fr'><img src='" + _urlPadrao + "/Imagens/ico_filter_p.png' /></a>", "sWidth": "20%", "sClass": "left", "mData": "ch_tipo_termo", "bSortable": false,
    "mRender": function (data, type, full) {
        var nm_tipo_termo = "";
        if (full.ch_tipo_termo == "DE") {
            nm_tipo_termo = "Descritor";
        }
        else if (full.ch_tipo_termo == "ES") {
            nm_tipo_termo = "Especificador";
        }
        else if (full.ch_tipo_termo == "AU") {
            nm_tipo_termo = "Autoridade";
        }
        else if (full.ch_tipo_termo == "LA") {
            if (full.in_lista && IsNotNullOrEmpty(full.ch_lista_superior)) {
                nm_tipo_termo = "Sublista";
            }
            else if (full.in_lista) {
                nm_tipo_termo = "Lista";
            }
            else {
                nm_tipo_termo = "Item";
            }
        }
        return nm_tipo_termo;
    }
},
{ "mData": "", "sDefaultContent": "", "sTitle": "", "sWidth": "20%", "sClass": "left ws all", "sName": "", "bSortable": false,
    "mRender": function (data, type, full) {
        return "<a target='_blank' href='./DetalhesDeVocabulario.aspx?id_doc=" + full._metadata.id_doc + "' title='Exibir Detalhes'><img valign='absmiddle' alt='Exibir' valign='absmiddle' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a><a class='button p' href='javascript:void(0);' onclick='javascript:trocarTermosRestaurados(\"" + full.ch_termo + "\",\"" + full.nm_termo + "\");'>Trocar</a>"
    }
}
];

var _columns_notifiqueme_termos_diarios_monitorados = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Diário", "sDefaultContent": "", "sClass": "grid-cell ws nm_tipo_fonte_diario_monitorado", "mData": "nm_tipo_fonte_diario_monitorado" },
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termos", "sClass": "grid-cell ws ds_termo_diario_monitorado", "mData": "ds_termo_diario_monitorado" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Busca", "sWidth": "100px", "sClass": "grid-cell ws center", "mData": "in_exata_diario_monitorado",
	    "mRender": function (data, type, full) {
	        return data ? "Exata" : "Aproximada";
	    }
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Habilitar/Desabilitar", "sWidth": "125px", "sClass": "grid-cell ws center", "mData": "st_termo_diario_monitorado",
	    "mRender": function (data, type, full) {
	        return "<input type='checkbox' " + (data ? "checked='checked'" : "") + " onchange='alterarStTermoDiarioMonitorado(event, \"" + full.ch_termo_diario_monitorado + "\")' title='status do monitoramento' /><div class='loading-p' style='display:none'></div>";
	    }
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false, "mData": "ch_termo_diario_monitorado",
	    "mRender": function (data, type, full) {
	        var sTermoDiarioMonitorado = getVal(full.ch_tipo_fonte_diario_monitorado) + "#" + getVal(full.nm_tipo_fonte_diario_monitorado) + "#" + full.ds_termo_diario_monitorado + "#" + (full.in_exata_diario_monitorado ? "1" : "0");
	        return "<a href='./ResultadoDePesquisa?tipo_pesquisa=notifiqueme&ch_tipo_fonte=" + full.ch_tipo_fonte_diario_monitorado + "&nm_tipo_fonte=" + full.nm_tipo_fonte_diario_monitorado + "&filetext=" + full.ds_termo_diario_monitorado + "&in_exata=" + (full.in_exata_diario_monitorado ? "1" : "0") + "' title='Pesquisar os diários, já cadastrados, com base nestes critérios' ><img src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' alt='buscar' /></a>&nbsp;" +
	            "<a href='javascript:void(0);' onclick='javascript:editarTermoDiarioMonitorado(event,\"" + sTermoDiarioMonitorado + "\", \"" + full.ch_termo_diario_monitorado + "\")' title='Editar monitoramento' ><img src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' alt='editar' /></a>&nbsp;" +
                "<a href='javascript:void(0);' onclick='javascript:CriarModalConfirmacaoRemoverTermoDiarioMonitorado(event,\"" + data + "\")' title='Excluir monitoramento' ><img src='" + _urlPadrao + "/Imagens/ico_delete_p.png' alt='delete' /></a><div class='loading-p' style='display:none'></div>";
	    }
	}
];

var _columns_notifiqueme_normas_monitoradas = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Norma", "sClass": "grid-cell ws",
		"mRender": function (data, type, full) {
			return "<a href='./DetalhesDeNorma.aspx?id_norma=" + full.ch_norma_monitorada + "' title='detalhes da norma'>" + full.nm_tipo_norma_monitorada + " " + full.nr_norma_monitorada + " de " + full.dt_assinatura_norma_monitorada + "</a>";
		}
    },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Ativo", "sClass": "grid-cell ws center", "mData": "st_norma_monitorada",
		"mRender": function (data, type, full) {
		    return "<input type='checkbox' " + (data ? "checked='checked'" : "") + " onchange='javascript:alterarStNormaMonitorada(event, \"" + full.ch_norma_monitorada + "\")' title='status do monitoramento' /><div class='loading-p' style='display:none'></div>";
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false, "mData": "ch_norma_monitorada",
		"mRender": function (data, type, full) {
			return "<a href='javascript:void(0);' onclick='javascript:CriarModalConfirmacaoRemoverNormaMonitorada(event,\"" + data + "\")' title='excluir monitoramento' ><img src='"+_urlPadrao+"/Imagens/ico_delete_p.png' alt='delete' /></a><div class='loading-p' style='display:none'></div>";
		}
	}
];

var _columns_notifiqueme_criacao_normas_monitoradas = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sClass": "grid-cell ws", "mData": "nm_tipo_norma_criacao", "sorting": "asc" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "", "sClass": "grid-cell ws", "mData": "primeiro_conector_criacao" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sClass": "grid-cell ws", "mData": "nm_orgao_criacao" },
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "", "sClass": "grid-cell ws", "mData": "segundo_conector_criacao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Indexação", "sClass": "grid-cell ws", "mData": "nm_termo_criacao" },
	{ "indice": 6, "isControl": false, "standard_view": true, "sTitle": "Ativo", "sClass": "grid-cell ws center", "mData": "st_criacao",
	    "mRender": function (data, type, full) {
	        return "<input type='checkbox' " + (data ? "checked='checked'" : "") + " onchange='javascript:alterarStCriacaoNormaMonitorada(event, \"" + full.ch_criacao_norma_monitorada + "\")' title='status do monitoramento' /><div class='loading-p' style='display:none'></div>";
	    }
	},
	{ "indice": 6, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false,
		"mRender": function (data, type, full) {
			var sCriacao_normas_monitoradas = full.ch_tipo_norma_criacao + "#" + full.nm_tipo_norma_criacao + "#" + full.primeiro_conector_criacao + "#" + full.ch_orgao_criacao + "#" + full.nm_orgao_criacao + "#" + full.segundo_conector_criacao + "#" + full.ch_termo_criacao + "#" + full.ch_tipo_termo_criacao + "#" + full.nm_termo_criacao;
			return "<a href='javascript:void(0);' onclick='javascript:EditarCriacaoNormaMonitorada(event, \"" + sCriacao_normas_monitoradas + "\", \"" + full.ch_criacao_norma_monitorada + "\")' title='editar monitoramento' ><img src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' alt='editar' /></a>&nbsp;" +
				"<a href='javascript:void(0);' onclick='javascript:CriarModalConfirmacaoRemoverCriacaoNormaMonitorada(event, \"" + full.ch_criacao_norma_monitorada + "\")' title='excluir monitoramento'><img src='" + _urlPadrao + "/Imagens/ico_delete_p.png' alt='delete' /></a><div class='loading-p' style='display:none'></div>";
		}
	}
];

var _columns_fale_conosco_atendimento = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sClass": "grid-cell ws", "sWidth": "150px", "mData": "dt_inclusao" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Assunto", "sClass": "grid-cell ws center", "mData": "ds_assunto" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Mensagem", "sClass": "grid-cell ws center", "mData": "ds_msg",
        "mRender": function (data, type, full) {
            if (data.length > 100) {
                data = data.substring(0, 100);
            }
            return data;
        }
    },
	{ "indice": 3, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false, "mData": "ch_chamado",
	    "mRender": function (data, type, full) {
	        return "<a href='javascript:void(0);' onclick='abrirChamado(\""+data+"\")' title='Exibir' ><img src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' alt='abrir' /></a>";
	    }
	}
];

	var _columns_fale_conosco_atendimento_historico = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Histórico de mensagens", "sClass": "grid-cell ws mensagens", "mData": "dt_resposta",
	    "mRender": function (data, type, full) {
	        var html = "";
	        if (_aplicacao.toUpperCase() == "CADASTRO") {
	            if (IsNotNullOrEmpty(full.nm_login_usuario_resposta)) {
	                html = "<div class='msg_sinj'><div class='dt_inclusao'><div class='dt'>" + data + "</div></div>" +
	                "<div class='ds_msg'><div class='tooltip-arrow'></div>" +
                        "<div class='msg'>" +
                            "<label>Mensagem: </label>" + full.ds_msg_resposta + "<br/>" +
                            "<label>Usuário SINJ: </label>" + full.nm_usuario_resposta + "<br/>" +
                        "</div>" +
                    "</div></div>";
	            }
	            else {
	                html = "<div class='msg_user'><div class='dt_inclusao'><div class='dt'>" + data + "</div></div>" +
	                "<div class='ds_msg'><div class='tooltip-arrow'></div><div class='msg'>" + full.ds_msg_resposta + "</div></div></div>";
	            }
	        }
	        else {
	            html = "<div class='" + (IsNotNullOrEmpty(full.nm_login_usuario_resposta) ? 'msg_sinj' : 'msg_user') + "'><div class='dt_inclusao'><div class='dt'>" + data + "</div></div>" +
	            "<div class='ds_msg'><div class='tooltip-arrow'></div><div class='msg'>" + full.ds_msg_resposta + "</div></div></div>";
	        }
	        return html;
	    }
	}
];

var _columns_total_termos_tipo = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Termos", "sClass": "grid-cell ws", "mData": "name" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Quantidade", "sClass": "grid-cell ws center", "mData": "total" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Salvar Planilha", "sClass": "grid-cell ws center", "bSortable": false,
		"mRender": function (data, type, full) {
			return "<a href='javascript:void(0);' onclick='javascript:GerarPlanilhaTermos(\"" + full.tipo + "\");' title='Salvar Planilha'><img valign='absmiddle' alt='xls' src='" + _urlPadrao + "/Imagens/ico_xls_p.png' width='15' /></a>";
		}
	}
];

	var _columns_usuario_notifiqueme = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "E-mail", "sClass": "grid-cell ws", "mData": "email_usuario_push" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Nome", "sClass": "grid-cell ws center", "mData": "nm_usuario_push" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Ativo", "sClass": "grid-cell ws center", "mData": "st_push",
	    "mRender": function (data) {
	        return TratarCamposBooleanos(data);
	    }
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Monitorando", "bSortable": false, "sClass": "grid-cell ws left",
	    "mRender": function (data, type, full) {
	        var monitorando = '';
	        if (IsNotNullOrEmpty(full.criacao_normas_monitoradas)) {
	            monitorando += 'Normas (criação): ' + full.criacao_normas_monitoradas.length + '<br/>';
	        }
	        if (IsNotNullOrEmpty(full.normas_monitoradas)) {
	            monitorando += 'Normas (alteração): ' + full.normas_monitoradas.length + '<br/>';
	        }
	        if (IsNotNullOrEmpty(full.termos_diarios_monitorados)) {
	            monitorando += 'Diários: ' + full.termos_diarios_monitorados.length + '<br/>';
	        }
	        return monitorando;
	    }
	},
	{ "indice": 6, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false,
	    "mRender": function (data, type, full) {
	        return '<a href="./DetalhesDeNotifiqueme.aspx?id_doc=' + full._metadata.id_doc + '" title="Visualizar detalhes do usuário" /><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
	    }
	}
];

var _columns_usuario_notifiqueme_auditoria = [
	{ "indice": 0, "isControl": true, "standard_view": true, "sTitle": "<label><input id='checkbox_check_all' type='checkbox' class_childs='check_push' onchange='javascript:selecionarTodos(this)' style='vertical-align: middle;' />Todos</label>", "sWidth": "80px", "sClass": "grid-cell ws text-center", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			$('#checkbox_check_all').prop('prop', push_checked_all);
			return '<input class="check_push" type="checkbox" value="' + full.email_usuario_push + '"' + ( push_checked_all ? " checked='checked'" : "" ) + '/>';
		}
	}
];
_columns_push_auditoria = _columns_usuario_notifiqueme_auditoria.concat(_columns_usuario_notifiqueme.slice(0));

function fn_columns_norma_associada(associacao) {
    return [
        { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma word-no-break", "mData": "nm_tipo_norma",
            "mRender": function (data, type, full) {
                return '<a href="./DetalhesDeNorma.aspx?id_norma=' + full.ch_norma + '" title="Visualizar detalhes da norma" />' + full.nm_tipo_norma + " " + full.nr_norma + ' <img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
            }
        },
        { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura word-no-break", "mData": "dt_assinatura" },
        { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao word-no-break", "mData": "origens", "bSortable": false,
            "mRender": function (data) {
                var origens = "";
                for (var i = 0; i < data.length; i++) {
                    origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + data[i].ch_orgao + '\')">' + data[i].sg_orgao + '</a>';
                }
                return origens;
            }
        },
        { "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa word-no-break", "mData": "ds_ementa", "bSortable": false },
        { "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao word-no-break", "mData": "nm_situacao" },
        { "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "center ws word-no-break all", "bSortable": false,
            "mRender": function (data, type, full) {
                var detalhes = "";
                var selecionar = "<a href='javascript:void(0);' onclick='javascript:SelecionarNormaAssociada(\"" + full.ch_norma + "\",\"" + montarDescricaoDaNorma(full) + "\", \""+associacao+"\");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
                return detalhes + selecionar;
            }
        }
    ];
}

var _columns_norma_vide = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
		"mRender": function (data, type, full) {
			return full.nm_tipo_norma + " " + full.nr_norma + " " + full.dt_assinatura;
		}
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
		"mRender": function (data) {
			var origens = "";
			for (var i = 0; i < data.length; i++) {
				origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + data[i].ch_orgao + '\')">' + data[i].sg_orgao + '</a>';
			}
			return origens;
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "center ws all", "bSortable": false,
		"mRender": function (data, type, full) {
			return "<a href='javascript:void(0);' onclick='javascript:selecionarNormaVide(\"" + full.ch_norma + "\",\"" + full.nr_norma + "\", \"" + full.dt_assinatura +  "\", \"" + full.nm_tipo_norma + "\" ," + full.st_acao + ");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
		}
	}
];

var _columns_norma_detalhes_orgao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
		"mRender": function (data, type, full) {
			return '<a href="./DetalhesDeNorma.aspx?id_norma=' + full.ch_norma + '" title="Visualizar detalhes da norma" />' + full.nm_tipo_norma + " " + full.nr_norma + ' <img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
		}
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
		"mRender": function (data) {
			var origens = "";
			for (var i = 0; i < data.length; i++) {
				origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + data[i].ch_orgao + '\')">' + data[i].sg_orgao + '</a>';
			}
			return origens;
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao" }
];

var _columns_norma_associada_vocabulario = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
		"mRender": function (data, type, full) {
			return '<a href="./DetalhesDeNorma.aspx?id_norma=' + full.ch_norma + '" title="Visualizar detalhes da norma" />' + full.nm_tipo_norma + " " + full.nr_norma + ' <img alt="detalhes" src="'+_urlPadrao+'/Imagens/ico_loupe_p.png" /></a>';
		}
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
		"mRender": function (data) {
			var origens = "";
			for (var i = 0; i < data.length; i++) {
				origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + data[i].ch_orgao + '\')">' + data[i].sg_orgao + '</a>';
			}
			return origens;
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao" }
];


	var _columns_historico = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data e Hora", "sWidth": "", "sClass": "grid-cell ws", "sDefaultContent": "", "mData": "dt_historico", "asSorting": ["desc", "asc"],
	    "mRender": function (data, type, full) {
	        return full._source.dt_historico;
	    }
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Pesquisa", "sWidth": "", "sClass": "grid-cell ws", "sDefaultContent": "", "mData": "_source.ds_historico", "bSortable": false },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Total", "sWidth": "10%", "sClass": "grid-cell ws", "sDefaultContent": "", "mData": "_source.total", "bSortable": false,
	    "mRender": function (data, type, full) {
	        var total = '';
	        if (IsNotNullOrEmpty(full, '_source.total')) {
	            for (var i in full._source.total) {
	                total += '<b>' + full._source.total[i].ds_base + '</b>: ' + full._source.total[i].nr_total + '<br/>';
	            }
	        }
	        return total;
	    }
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "60px", "sClass": "grid-cell ws all", "mData": "", "bSortable": false,
	    "mRender": function (data, type, full) {
	        var detalhes = "<a href='./ResultadoDePesquisa.aspx?" + window.unescape(full._source.consulta).replaceAll('#','%23') + "' title='Pesquisar'><img valign='absmiddle' alt='pesquisar' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
	        var editar = "<a href='./Pesquisas.aspx?" + window.unescape(full._source.consulta).replaceAll('#', '%23') + "' title='Editar'><img valign='absmiddle' alt='editar' src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' /></a>";
	        return detalhes + "&nbsp;" + editar;
	    }
	}
];

var _columns_norma_notifiqueme = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
		"mRender": function (data, type, full) {
			return full.nm_tipo_norma + " " + (full.nr_norma != null ? full.nr_norma : "") + " " + full.dt_assinatura;
		}
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
		"mRender": function (data, type, full) {
			var origens = "";
			for (var i = 0; i < full.origens.length; i++) {
				origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + full.origens[i].ch_orgao + '\')">' + full.origens[i].sg_orgao + '</a>';
			}
			return origens;
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false,
		"mRender": function (data, type, full) {
			return full.ds_ementa;
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao",
		"mRender": function (data, type, full) {
			return full.nm_situacao;
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "center ws all", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "";
			var ds_norma = montarDescricaoDaNorma(full);
			var selecionar = "<a href='javascript:void(0);' onclick='selecionarNorma(\"" + full.ch_norma + "\");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
			return detalhes + selecionar;
		}
	}
];

var _columns_erro_sistema = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sWidth": "", "sClass": "grid-cell ws", "mData": "dt_log_erro" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sWidth": "", "sClass": "grid-cell ws", "mData": "nm_tipo" },
    { "indice": 2, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": "Usuário", "sWidth": "", "sClass": "grid-cell ws", "mData": "nm_login_user_erro" },
    { "indice": 4, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Detalhes' href='./DetalhesDeErroSistema.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
        }
    }
];

var _columns_erro_indexacao = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sWidth": "20%", "sClass": "grid-cell ws", "mData": "dt_error" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Base", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_base" },
    { "indice": 3, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Detalhes' href='./DetalhesDeErroIndexacao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
        }
    }
];

var _columns_erro_extracao = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sWidth": "20%", "sClass": "grid-cell ws", "mData": "dt_error" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Base", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_base" },
    { "indice": 3, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": "Nome do Arquivo", "sWidth": "", "sClass": "grid-cell ws center", "mData": "file_name" },
    { "indice": 4, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Detalhes' href='./DetalhesDeErroExtracao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
        }
    }
];

var _columns_operacao = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Operação", "sWidth": "", "sClass": "grid-cell ws", "mData": "ch_operacao",
        "mRender": function (data) {
            return getOperacao(data);
        }
    },
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sWidth": "", "sClass": "grid-cell ws", "mData": "dt_inicio" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Usuário", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_user_operacao" },
    { "indice": 4, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Detalhes' href='./DetalhesDeOperacao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
        }
    }
];

var _columns_notificacao = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sWidth": "", "sClass": "grid-cell ws", "mData": "dt_inicio" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Executado por", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_user_operacao" },
	{ "indice": 4, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			return "&nbsp;<a title='Detalhes' href='./DetalhesDeNotificacao.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
		}
	}
];

var _columns_acesso = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Usuário", "sWidth": "", "sClass": "grid-cell ws", "mData": "nm_user_acesso" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Data e Hora", "sWidth": "", "sClass": "grid-cell ws center", "mData": "dt_acesso" },
    { "indice": 3, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Detalhes' href='./DetalhesDeAcesso.aspx?id_doc=" + full._metadata.id_doc + "&app=" + (full.ds_login == "SINJ.PUSH" ? "push" : "sistema") + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
        }
    }
];

var _columns_sessao = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Usuário", "sWidth": "", "sClass": "grid-cell ws", "mData": "",
        "mRender": function (data, type, full) {
            var user = full.ds_user;
            var ds_valor = JSON.parse(full.ds_valor);
            if (IsNotNullOrEmpty(ds_valor, 'nm_usuario')) {
                user = ds_valor.nm_usuario;
            }
            else if (IsNotNullOrEmpty(ds_valor, 'nm_usuario_push')) {
                user = ds_valor.nm_usuario_push;
            }
            return user;
        }
    },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Data de Criação", "sWidth": "", "sClass": "grid-cell ws center", "mData": "dt_criacao" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Data de Expiração", "sWidth": "", "sClass": "grid-cell ws center", "mData": "dt_expiracao" },
    { "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Acessado com", "sWidth": "", "sClass": "grid-cell ws center", "mData": "ds_tipo_acesso" },
    { "indice": 4, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Encerrar Sessão' href='javascript:void(0);' onclick='javascript:encerrarSessao(\"" + full._metadata.id_doc + "\");'  ><img valign='absmiddle' alt='delete' src='" + _urlPadrao + "/Imagens/ico_delete_p.png' /></a>";
        }
    }
];

var _columns_lixeira = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data e Hora", "sWidth": "20%", "sClass": "grid-cell ws center", "mData": "dt_exclusao" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Base", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_base_excluido" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Usuário", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_login_usuario_exclusao" },
    { "indice": 3, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "20px", "sClass": "center ws all", "mData": "", "bSortable": false,
        "mRender": function (data, type, full) {
            return "&nbsp;<a title='Detalhes' href='./DetalhesDeLixeira.aspx?id_doc=" + full._metadata.id_doc + "'  ><img valign='absmiddle' alt='Detalhes' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
        }
    }
];

var _columns_arquivos = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "", "sClass": "grid-cell ws left nm_arquivo", "mData": "nm_arquivo", "sorting": "desc",
        "mRender": function (data, type, full) {
            var html = '';
            if (full.nr_tipo_arquivo == 0) {
                html = '<div class="div_folder"><a href="javascript:void(0);" nivel="' + full.nr_nivel_arquivo + '" chave="' + full.ch_arquivo + '" onclick="javascript:selecionarPasta(this)" ><img src="' + _urlPadrao + '/Imagens/ico_folder.png" width="18" height="18" /> ' + full.nm_arquivo + '</a></div>';
                if ($('#div_list_dir a[chave="' + full.ch_arquivo + '"]').length <= 0) {
                    $('#div_list_dir a[chave="' + full.ch_arquivo_superior + '"]').closest('.div_folder').append('<div class="div_folder"><a href="javascript:void(0);" nivel="' + full.nr_nivel_arquivo + '" chave="' + full.ch_arquivo + '" onclick="javascript:selecionarPasta(this)" ><span>&gt;&nbsp;</span><img src="' + _urlPadrao + '/Imagens/ico_folder.png" width="18" height="18" /> ' + full.nm_arquivo + '</a></div>');
                }
            }
            else {
                var tipo = full.ar_arquivo.mimetype.split('/')[1];
                var size = (full.ar_arquivo.filesize / 1024).toFixed(0) + ' KB';
                var ico = '<img src="' + _urlPadrao + '/Imagens/ico_file.png" width="18" height="18" />';
                if (tipo == 'html') {
                    ico = '<img src="' + _urlPadrao + '/Imagens/ico_html.png" alt="html" width="18" height="18" />';
                } else if (tipo == 'pdf') {
                    ico = '<img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="pdf" width="18" height="18" />';
                }
                else if (full.ar_arquivo.mimetype.indexOf('image/') == 0) {
                    ico = '<img src="./Download/sinj_arquivo/' + full.ar_arquivo.id_file + '/' + full.ar_arquivo.filename + '" alt="' + full.nm_arquivo + '" style="max-width:50px;max-height:30px;" />';
                }
                html = '<div class="div_arq" chave="' + full.ch_arquivo + '" style="word-break: break-all;">' + ico + ' ' + full.nm_arquivo + '.' + tipo + '</div>';
            }
            return html;
        }
    },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Data de Criação", "sWidth": "", "sClass": "grid-cell ws left", "mData": "_metadata.dt_doc" },
    { "indice": 2, "sDefaultContent": "", "bSortable": false, "isControl": false, "standard_view": true, "sTitle": "Tamanho", "sWidth": "", "sClass": "grid-cell ws left", "mData": "",
        "mRender": function (data, type, full) {
            return (full.ar_arquivo.filesize / 1024).toFixed(0) + ' KB';
        }
    },
    { "indice": 3, "isControl": true, "bSortable": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "grid-cell ws left", "mData": "",
        "mRender": function (data, type, full) {
            if (full.nr_tipo_arquivo == 1) {
                var mimetypeSplited = full.ar_arquivo.mimetype.split('/');
                var tipo = mimetypeSplited[1];
                var ico = '<img src="' + _urlPadrao + '/Imagens/ico_file.png" width="18" height="18" /> ';
                var bt_download = '<a target="_blank" href="./Download/sinj_arquivo/' + full.ar_arquivo.id_file + '/' + full.ar_arquivo.filename + '" title="Baixar Arquivo" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_download.png" alt="download" /></a>';
                var bt_edit = '';
                var bt_link_edit = '';
                var bt_convert = '';
                var bt_import = '';
                var bt_delete = '';
                if (full.ch_arquivo.indexOf('000shared/images') < 0) {
                    bt_delete = '&nbsp;<a href="javascript:void(0);" onclick="javascript:openDialogDeleteFile(this)" title="Excluir Arquivo"><img height="16" src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="excluir" /></a>';
                }
                if (IsNotNullOrEmpty(window, 'bProduzir')) {
                    if (tipo == 'html') {
                        bt_edit = '&nbsp;<a href="javascript:void(0);" onclick="javascript:openDialogEditFile(this)" title="Editar Arquivo"><img height="16" src="' + _urlPadrao + '/Imagens/ico_edit_dir.png" alt="editar html" /></a>';
                        //bt_link_edit = '&nbsp;<a href="javascript:void(0);" onclick="javascript:openDialogEditLink(this)" title="Editar Links"><img height="16" src="' + _urlPadrao + '/Imagens/ico_edit_link.png" alt="editar link" /></a>';
                        bt_convert = '&nbsp;<a target="_blank" href="./htmltopdf/dc_arquivo/' + full.ar_arquivo.id_file + '/' + full.ar_arquivo.filename.replace(".html", ".pdf").replace(".htm", ".pdf") + '" title="Converter e Baixar em PDF" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="converter" /></a>';
                    }
                } else {
                    if (tipo == 'html' || mimetypeSplited[0] == 'image') {
                        bt_import = '&nbsp;<a target="" href="javascript:void(0);" onclick="javascript:selecionarDocumentoImportar(this);" title="Selecionar Arquivo" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_check_p.png" alt="selecionar" /></a>';
                    }
                }
                return bt_download + bt_import + bt_convert + bt_edit + bt_link_edit + bt_delete;
            }
            else {
                if (full.ch_arquivo.indexOf('000shared/images') < 0) {
                    return '<a href="javascript:void(0);" onclick="javascript:openDialogDeleteFolder(this)" title="Excluir Diretório" ch_folder_selected="' + full.ch_arquivo + '" nr_nivel_arquivo_selected="' + full.nr_nivel_arquivo + '"><img height="16" src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="excluir" /></a>';
                }
                return '';
            }
        }
    }
];

var _columns_imagens = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "", "sClass": "grid-cell ws left", "mData": "nm_arquivo",
        "mRender": function (data, type, full) {
            var html = '';
            if (full.nr_tipo_arquivo == 0) {
                html = '<div class="div_folder"><a href="javascript:void(0);" onclick="javascript:carregarImagens(\'' + full.ch_arquivo + '\')" ><img src="' + _urlPadrao + '/Imagens/ico_folder.png" width="18" height="18" /> ' + full.nm_arquivo + '</a></div>';
            }
            else {
                if (full.ar_arquivo.mimetype.indexOf('image/') == 0) {
                    var tipo = full.ar_arquivo.mimetype.split('/')[1];
                    var size = (full.ar_arquivo.filesize / 1024).toFixed(0) + ' KB';
                    url = "Download/sinj_arquivo/" + full.ar_arquivo.id_file + "/" + full.ar_arquivo.filename;
                    var ico = '<img src="./Download/sinj_arquivo/' + full.ar_arquivo.id_file + '/' + full.ar_arquivo.filename + '" alt="' + full.ar_arquivo.filename + '" style="max-width:50px;max-height:30px;" />';
                    html = '<div class="div_arq" chave="' + full.ch_arquivo + '"><a target="_blank" href="javascript:void(0);" onclick="javascript:selectImage(\''+url+'\');" title="Selecionar Imagem" >' + ico + ' ' + full.ar_arquivo.filename + ' <img height="16" src="' + _urlPadrao + '/Imagens/ico_check_p.png" alt="ok" /></a></div>';
                }
            }
            return html;
        }
    },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Data de Criação", "sWidth": "", "sClass": "grid-cell ws left", "mData": "_metadata.dt_doc" },
    { "indice": 2, "sDefaultContent": "", "bSortable": false, "isControl": false, "standard_view": true, "sTitle": "Tamanho", "sWidth": "", "sClass": "grid-cell ws left", "mData": "",
        "mRender": function (data, type, full) {
            return (full.ar_arquivo.filesize / 1024).toFixed(0) + ' KB';
        }
    }
];

var _columns_arquivos_versionados = [
    { "indice": 0, "isControl": false, "bSortable": false, "standard_view": true, "sTitle": "Nome", "sWidth": "", "sClass": "grid-cell ws left", "mData": "ar_arquivo_versionado",
        "mRender": function (data, type, full) {
            var tipo = data.mimetype.split('/')[1];
            var ico = '<img src="' + _urlPadrao + '/Imagens/ico_file.png" width="18" height="18" />';
            if (tipo == 'html') {
                ico = '<img src="' + _urlPadrao + '/Imagens/ico_html.png" alt="html" width="18" height="18" />';
            } else if (tipo == 'pdf') {
                ico = '<img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="pdf" width="18" height="18" />';
            }
            return '<div class="div_arq">' + ico + ' ' + data.filename + '</div>';
        }
    },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Data da versão", "sWidth": "", "sClass": "grid-cell ws left", "mData": "dt_arquivo_versionado", "sorting": "desc" },
    { "indice": 2, "sDefaultContent": "", "bSortable": false, "isControl": false, "standard_view": true, "sTitle": "Tamanho", "sWidth": "", "sClass": "grid-cell ws left", "mData": "ar_arquivo_versionado",
        "mRender": function (data) {
            return (data.filesize / 1024).toFixed(0) + ' KB';
        }
    },
    { "indice": 3, "isControl": true, "bSortable": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "grid-cell ws left", "mData": "",
        "mRender": function (data, type, full) {
            var tipo = full.ar_arquivo_versionado.mimetype.split('/')[1];
            var bt_download = '<a target="_blank" href="./Download/sinj_arquivo_versionado_norma/' + full.ar_arquivo_versionado.id_file + '/' + full.ar_arquivo_versionado.filename + '" title="Baixar Arquivo" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_download.png" alt="download" /></a>';
            var bt_recovery = '&nbsp;<button class="clean" onclick="javascript:selecionarDocumentoRecuperar(this);" title="Selecionar Arquivo" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_check_p.png" alt="selecionar" /></button>';
            return bt_download + bt_recovery;
        }
    }
];

var _columns_fale_conosco = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data e Hora", "sWidth": "20%", "sClass": "grid-cell ws center", "mData": "dt_inclusao" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "", "sClass": "grid-cell ws center", "mData": "nm_user" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Status", "sWidth": "", "sClass": "grid-cell ws center", "mData": "st_atendimento" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "E-mail", "sWidth": "", "sClass": "grid-cell ws center", "mData": "ds_email" },
    { "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Assunto", "sWidth": "", "sClass": "grid-cell ws center", "mData": "ds_assunto" },
    { "indice": 3, "isControl": true, "bSortable": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "grid-cell ws left", "mData": "",
        "mRender": function (data, type, full) {

//            if (full.st_atendimento == "Novo") {
//                btReceber = '<button title="Receber esse chamado" class="clean" ch_chamado="' + full.ch_chamado + '" onclick="receberChamado(this);"><img src="' + _urlPadrao + '/Imagens/ico_check_p.png"/></button>&nbsp;';
//            }
//            if (full.st_atendimento != "Finalizado") {
//                btFinalizar = '<button title="Finalizar esse chamado" class="clean" ch_chamado="' + full.ch_chamado + '" onclick="finalizarChamado(this);"><img src="' + _urlPadrao + '/Imagens/ico_close.png"/></button>&nbsp;';
//            }
//            btResponder = '<button title="Enviar um e-mail para esse usuário" class="clean" ch_chamado="' + full.ch_chamado + '" ds_email="' + full.ds_email + '" onclick="responderChamado(this);"><img src="' + _urlPadrao + '/Imagens/ico_email_p.png"/></button>&nbsp;';

            return '<a title="Visualizar detalhes desse chamado" href="./DetalhesDeFaleConosco.aspx?ch_chamado='+full.ch_chamado+'"><img src="' + _urlPadrao + '/Imagens/ico_loupe_fit.png"/></a>';
        }
    }
];
