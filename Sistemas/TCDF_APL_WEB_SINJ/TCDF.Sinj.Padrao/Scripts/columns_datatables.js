﻿function TratarCamposBooleanos(data) {
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
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false },
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
                    if (IsNotNullOrEmpty(full.fontes[i].ar_fonte.id_file)) {
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
//				links = '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoNorma.aspx?id_norma=' + full.ch_norma + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' +
//						'&nbsp;&nbsp;' +
//						'<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a>';
			}
			return links;
		}
	}
];

	var _columns_norma_es = [
	{ "indice": 0, "isControl": true, "standard_view": true, "sTitle": '<a title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(null, true);" class="center"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /><b>TODOS</b></a>', "sWidth": "60px", "sClass": "grid-cell ws center", "mData": "_score", "bSortable": false, "visible": true,
	    "mRender": function (data, type, full) {
	        if (!IsNotNullOrEmpty(window.aHighlight)) {
	            aHighlight = [];
	        }
	        aHighlight.push({ highlight: full.highlight, doc: full.fields.partial[0]._metadata.id_doc, ch_doc: full.fields.partial[0].ch_norma });
	        return '<a valor="sinj_norma_' + full.fields.partial[0]._metadata.id_doc + '" title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(\'sinj_norma_' + full.fields.partial[0]._metadata.id_doc + '\');"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /></a>';
	        //        return '<input class="check_cesta" type="checkbox" value="sinj_norma_' + full.fields.partial[0]._metadata.id_doc + '" />';
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relevância", "sWidth": "", "sClass": "grid-cell ws", "mData": "_score", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._score;
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma", "visible": false,
	    "mRender": function (data, type, full) {
	        return '<button title="Detalhes" name="btn" type="submit" title="Detalhes" class="link" onclick="javascript:doc_clicked=' + full.fields.partial[0]._metadata.id_doc + ';"><H2>' + full.fields.partial[0].nm_tipo_norma + " " + full.fields.partial[0].nr_norma + '</H2><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /> </button>';
	    }
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura", "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].dt_assinatura;
	    }
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        var origens = "";
	        for (var i = 0; i < full.fields.partial[0].origens.length; i++) {
	            origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + full.fields.partial[0].origens[i].ch_orgao + '\')">' + full.fields.partial[0].origens[i].sg_orgao + '</a>';
	        }
	        return origens;
	    }
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "", "sClass": "grid-cell ws ds_ementa max-w-30-pc", "mData": "ds_ementa", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].ds_ementa;
	    }
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao", "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].nm_situacao;
	    }
	},
	{ "indice": 5, "isControl": false, "standard_view": true, "sTitle": "Texto Integral", "sWidth": "10%", "sClass": "left ws all", "bSortable": false, "visible": false,
	    "mRender": function (data, type, full) {
	        var id_file = "";
	        var links = "";
	        var medida = "";
	        if (IsNotNullOrEmpty(full.fields.partial[0].ar_atualizado, 'id_file')) {
	            id_file = full.fields.partial[0].ar_atualizado.id_file;
	            medida = '<br/><b>' + (full.fields.partial[0].ar_atualizado.filesize / 1024).toFixed(0) + ' KB</b><br/>';
	        }
	        else if (IsNotNullOrEmpty(full.fields.partial[0].fontes) && full.fields.partial[0].fontes.length > 0 && IsNotNullOrEmpty(full.fields.partial[0].fontes[0].ar_fonte, 'id_file')) {
	            id_file = full.fields.partial[0].fontes[0].ar_fonte.id_file;
	            medida = '<br/><b>' + (full.fields.partial[0].fontes[0].ar_fonte.filesize / 1024).toFixed(0) + ' KB</b><br/>';
	        }
	        if (IsNotNullOrEmpty(id_file)) {
	            links = '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoNorma.aspx?id_norma=' + full.fields.partial[0].ch_norma + '"><img src="' + _urlPadrao + '/Imagens/ico_download_m.png" alt="download" /></a>';
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
	                '<button title="Detalhes" name="btn" type="submit" class="link" onclick="javascript:doc_clicked=' + full.fields.partial[0]._metadata.id_doc + ';"><H2><span class="nm_tipo_norma">' + full.fields.partial[0].nm_tipo_norma + '</span> <span class="nr_norma nr_norma_text">' + full.fields.partial[0].nr_norma + '</span> de <span class="dt_assinatura dt_assinatura_text">' + full.fields.partial[0].dt_assinatura + ' <span class="st_norma nm_situacao">' + full.fields.partial[0].nm_situacao + '</span></H2><img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /> </button>' +
	            '</div>' +
            '</div>';
	        var origens = "";
	        for (var i = 0; i < full.fields.partial[0].origens.length; i++) {
	            origens += (origens != "" ? "<br/>" : "") + full.fields.partial[0].origens[i].sg_orgao + ' - ' + full.fields.partial[0].origens[i].nm_orgao;
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
	                full.fields.partial[0].ds_ementa +
	            '</div>' +
            '</div>';

	        var id_file = "";
	        var nm_file = getTitleNorma(full.fields.partial[0]);
	        var links = "";
	        var url_file = '';
	        var medida = "";
	        if (IsNotNullOrEmpty(full.fields.partial[0].ar_atualizado, 'id_file')) {
	            id_file = full.fields.partial[0].ar_atualizado.id_file;
	            url_file = './Norma/' + full.fields.partial[0].ch_norma + '/' + nm_file;
	            medida = '<b>(' + (full.fields.partial[0].ar_atualizado.filesize / 1024).toFixed(0) + ' KB)</b>';
	        }
	        else if (IsNotNullOrEmpty(full.fields.partial[0].fontes) && full.fields.partial[0].fontes.length > 0 && IsNotNullOrEmpty(full.fields.partial[0].fontes[0].ar_fonte, 'id_file')) {
	            id_file = full.fields.partial[0].fontes[0].ar_fonte.id_file;
	            url_file = './Norma/' + full.fields.partial[0].ch_norma + '/' + nm_file;
	            medida = '<b>(' + (full.fields.partial[0].fontes[0].ar_fonte.filesize / 1024).toFixed(0) + ' KB)</b>';
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
	}
];

var _columns_norma_favoritos = _columns_norma_es.slice(0);
_columns_norma_favoritos[0] = { "indice": 0, "isControl": true, "standard_view": true, "sTitle": ' ', "sWidth": "30px", "sClass": "grid-cell ws", "mData": "", "bSortable": false,
    "mRender": function (data, type, full) {
        return '<div class="button_favoritos_' + full.fields.partial[0].ch_norma + '" style="width:30px;"><a href="javascript:void(0);" title="Remover da lista de favoritos" onclick="javascript:RemoverFavoritos(\'.button_favoritos_' + full.fields.partial[0].ch_norma + '\',\'norma_' + full.fields.partial[0].ch_norma + '\');" ><img alt="*" src="' + _urlPadrao + '/Imagens/ico_del_fav.png" /></a></div>';
    }
};

var _columns_norma_cesta = _columns_norma_es.slice(0);
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
        aHighlight.push({ highlight: full.highlight, doc: full.fields.partial[0]._metadata.id_doc, ch_doc: full.fields.partial[0].ch_norma });
        return "<a title='Remover da Cesta' href='javascript:ExcluirDaCesta(\"sinj_norma_" + full.fields.partial[0]._metadata.id_doc + "\");' class='a_delete' ><img valign='absmiddle' alt='Excluir'  src='" + _urlPadrao + "/Imagens/ico_trash_p.png'  /></a>";
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
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Seção", "sWidth": "", "sClass": "grid-cell ws secao_diario", "mData": "secao_diario" },
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo de Edição", "sWidth": "15%", "sClass": "grid-cell ws nm_tipo_edicao", "mData": "nm_tipo_edicao" },
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Diferencial Edição", "sWidth": "", "sClass": "grid-cell ws nm_diferencial_edicao", "mData": "nm_diferencial_edicao" },
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
	        return '<a valor="sinj_diario_' + full.fields.partial[0]._metadata.id_doc + '" title="Adicionar na cesta" href="javascript:void(0);" onclick="javascript:AdicionarNaCesta(\'sinj_diario_' + full.fields.partial[0]._metadata.id_doc + '\');"><img src="' + _urlPadrao + '/Imagens/ico_basket_p.png" alt="cesta" /></a>';
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Relevância", "sWidth": "", "sClass": "grid-cell ws", "mData": "_score", "visible": false,
	    "mRender": function (data, type, full) {
	        return full._score;
	    }
	},
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sWidth": "", "sClass": "grid-cell ws nm_tipo_fonte", "mData": "nm_tipo_fonte", "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].nm_tipo_fonte;
	    }
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Número", "sWidth": "", "sClass": "grid-cell ws nr_diario", "mData": "nr_diario", "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].nr_diario + (IsNotNullOrEmpty(full.fields.partial[0].cr_diario) ? " " + full.fields.partial[0].cr_diario : "");
	    }
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Seção", "sWidth": "", "sClass": "grid-cell ws secao_diario", "mData": "secao_diario", "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].secao_diario;
	    }
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Data de Publicação", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura", "visible": false,
	    "mRender": function (data, type, full) {
	        return full.fields.partial[0].dt_assinatura;
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
            var html = '<div class="table w-100-pc">';
            html += '<div class="line">' +
                '<div class="column"><H1>' + montarDescricaoDiario(full.fields.partial[0]) + '</H1></div>' +
            '</div>';
            var campo_highlight = '';
            var id_file = "";
            var nm_file = "";
            var links = "";
            var url_file = '';
            var medida = "";
            if (IsNotNullOrEmpty(full.fields.partial[0], 'ar_diario.id_file')) {
                id_file = full.fields.partial[0].ar_diario.id_file;
                nm_file = full.fields.partial[0].ar_diario.filename;
                url_file = './Diario/' + full.fields.partial[0].ch_diario + '/' + id_file + '/arq/' + nm_file;
                campo_highlight = MontarHighlight(full.highlight, 'ar_diario.filetext');
                medida = '<b>(' + (full.fields.partial[0].ar_diario.filesize / 1024).toFixed(0) + ' KB)</b>';

                links = '<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="20px" /> ' + medida + '</a>';
                if (_aplicacao == "CADASTRO") {
                    links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + full.fields.partial[0].ar_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" height="20px" /></a>';
                }
            }
            else {
                campo_highlight = MontarHighlight(full.highlight, 'arquivos.arquivo_diario.filetext');
                for (var i = 0; i < full.fields.partial[0].arquivos.length; i++) {
                    var obj_arquivo = full.fields.partial[0].arquivos[i];

                    id_file = obj_arquivo.arquivo_diario.id_file;
                    nm_file = obj_arquivo.arquivo_diario.filename;
                    url_file = './Diario/' + full.fields.partial[0].ch_diario + '/' + id_file + '/arq/'+i+'/' + nm_file;
                    medida = '<b>(' + (obj_arquivo.arquivo_diario.filesize / 1024).toFixed(0) + ' KB)</b>';

                    links += '<a title="baixar arquivo" target="_blank" href="' + url_file + '"><img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="download" width="20px" /> ' + medida + '</a>';
                    if (_aplicacao == "CADASTRO") {
                        links += '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + obj_arquivo.arquivo_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" width="20px" height="20px" /></a>';
                    }
                    links += '&nbsp;&nbsp;' + obj_arquivo.ds_arquivo + '<br/>';

                }
            }

            html += '<div class="line">' +
                    '<div class="column w-80-pc">' +
	                    campo_highlight +
	                '</div>' +
                    '<div class="column w-20-pc"><div>' +
	                    '<label>Texto Integral:</label><br/>' +
	                    links +
	                '</div></div>' +
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
                arquivos = '<div class="line"><div class="column w-80-pc">' + campo_highlight + '</div><div class="column w-20-pc">' + icone + '</div></div>';
            }
            else if (IsNotNullOrEmpty(full.fields.partial[0], 'arquivos')) {
                var campo_highlight = MontarHighlight(full.highlight, 'arquivos.arquivo_diario.filetext');
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

var _columns_diario_cesta = _columns_diario_es.slice(0);
_columns_diario_cesta[0] = { "indice": 0, "isControl": true, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws all", "mData": "", "bSortable": false,
    "mRender": function (data, type, full) {
        return "<a title='Remover da Cesta' href='javascript:ExcluirDaCesta(\"sinj_diario_" + full.fields.partial[0]._metadata.id_doc + "\");' class='a_delete' ><img valign='absmiddle' alt='Excluir'  src='" + _urlPadrao + "/Imagens/ico_trash_p.png'  /></a>";
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

var _columns_notifiqueme_normas_monitoradas = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Norma", "sClass": "grid-cell ws",
		"mRender": function (data, type, full) {
			return "<a href='./DetalhesDeNorma.aspx?id_norma=" + full.ch_norma_monitorada + "' title='detalhes da norma'>" + full.nm_tipo_norma_monitorada + " " + full.nr_norma_monitorada + " de " + full.dt_assinatura_norma_monitorada + "</a>";
		}
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Data", "sClass": "grid-cell ws center", "mData": "dt_cadastro_norma_monitorada" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Ativo", "sClass": "grid-cell ws center", "mData": "st_norma_monitorada",
		"mRender": function (data, type, full) {
			return "<input type='checkbox' " + (data ? "checked='checked'" : "") + " onchange='javascript:AlterarStNormaMonitorada(event, " + full.id_norma_monitorada + ")' title='status do monitoramento' /><div class='loading-p' style='display:none'></div>";
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false, "mData": "ch_norma_monitorada",
		"mRender": function (data, type, full) {
			return "<a href='javascript:void(0);' onclick='javascript:CriarModalConfirmacaoRemoverNormaMonitorada(event,\"" + data + "\")' title='excluir monitoramento' ><img src='"+_urlPadrao+"/Imagens/ico_delete_p.png' alt='delete' /></a><div class='loading-p' style='display:none'></div>";
		}
	}
];

var _columns_notifiqueme_criacao_normas_monitoradas = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sClass": "grid-cell ws", "mData": "nm_tipo_norma_criacao" },
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "", "sClass": "grid-cell ws", "mData": "primeiro_conector_criacao" },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sClass": "grid-cell ws", "mData": "nm_orgao_criacao" },
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "", "sClass": "grid-cell ws", "mData": "segundo_conector_criacao" },
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Indexação", "sClass": "grid-cell ws", "mData": "nm_termo_criacao" },
	{ "indice": 6, "isControl": false, "standard_view": true, "sClass": "grid-cell ws center all", "bSortable": false,
		"mRender": function (data, type, full) {
			var sCriacao_normas_monitoradas = full.ch_tipo_norma_criacao + "#" + full.nm_tipo_norma_criacao + "#" + full.primeiro_conector_criacao + "#" + full.ch_orgao_criacao + "#" + full.nm_orgao_criacao + "#" + full.segundo_conector_criacao + "#" + full.ch_termo_criacao + "#" + full.ch_tipo_termo_criacao + "#" + full.nm_termo_criacao + "#" + full.st_criacao;
			return "<a href='javascript:void(0);' onclick='javascript:EditarCriacaoNormaMonitorada(event, \"" + sCriacao_normas_monitoradas + "\", \"" + full.ch_criacao_norma_monitorada + "\")' ><img src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' alt='editar' /></a>&nbsp;" +
				"<a href='javascript:void(0);' onclick='javascript:CriarModalConfirmacaoRemoverCriacaoNormaMonitorada(event, \"" + full.ch_criacao_norma_monitorada + "\")' ><img src='" + _urlPadrao + "/Imagens/ico_delete_p.png' alt='delete' /></a><div class='loading-p' style='display:none'></div>";
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
                var ds_norma = full.nm_tipo_norma + " " + full.nr_norma + " " + full.dt_assinatura;
                var selecionar = "<a href='javascript:void(0);' onclick='javascript:SelecionarNormaAssociada(\"" + full.ch_norma + "\",\"" + ds_norma + "\", \""+associacao+"\");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
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
		    var ds_norma = full.nm_tipo_norma + " " + full.nr_norma + " " + full.dt_assinatura;
			return "<a href='javascript:void(0);' onclick='javascript:SelecionarNormaVide(\"" + full.ch_norma + "\",\"" + ds_norma + "\", \"" + full.dt_assinatura +  "\", \"" + full.nm_tipo_norma + "\" ," + full.st_acao + ");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
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
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data e Hora", "sWidth": "20%", "sClass": "grid-cell ws", "sDefaultContent": "", "mData": "dt_historico", "asSorting": ["desc", "asc"],
		"mRender": function (data, type, full) {
			return full._source.dt_historico;
		}
	},
	{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Pesquisa", "sWidth": "70%", "sClass": "grid-cell ws", "sDefaultContent": "", "mData": "_source.ds_historico", "bSortable": false },
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "grid-cell ws all", "mData": "", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "<a href='./ResultadoDePesquisa.aspx?" + window.unescape(full._source.consulta) + "' title='Pesquisar'><img valign='absmiddle' alt='pesquisar' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' /></a>";
			var editar = "<a href='./Pesquisas.aspx?" + full._source.consulta + "' title='Editar'><img valign='absmiddle' alt='editar' src='" + _urlPadrao + "/Imagens/ico_pencil_p.png' /></a>";
			return detalhes + "&nbsp;" + editar;
		}
	}
];

//var _columns_norma_notifiqueme = [
//{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws _all nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
//    "mRender": function (data, type, full) {
//        return full.nm_tipo_norma + " " + full.nr_norma + " " + full.dt_assinatura;
//    }
//},
//{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
//    "mRender": function (data) {
//        var origens = "";
//        for (var i = 0; i < data.length; i++) {
//            origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + data[i].ch_orgao + '\')">' + data[i].sg_orgao + '</a>';
//        }
//        return origens;
//    }
//},
//{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false },
//{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao" },
//{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "center ws all", "bSortable": false,
//    "mRender": function (data, type, full) {
//        var detalhes = "";
//        var ds_norma = full.nm_tipo_norma + " " + full.nr_norma + " " + full.dt_assinatura;
//        var selecionar = "<a href='javascript:void(0);' onclick='javascript:Notificar(\"" + full.ch_norma + "\");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
//        return detalhes + selecionar;
//    }
//}
//];



var _columns_norma_notifiqueme = [
	{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
		"mRender": function (data, type, full) {
			return full.fields.partial[0].nm_tipo_norma + " " + (full.fields.partial[0].nr_norma != null ? full.fields.partial[0].nr_norma : "") + " " + full.fields.partial[0].dt_assinatura;
		}
	},
	{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
		"mRender": function (data, type, full) {
			var origens = "";
			for (var i = 0; i < full.fields.partial[0].origens.length; i++) {
				origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + full.fields.partial[0].origens[i].ch_orgao + '\')">' + full.fields.partial[0].origens[i].sg_orgao + '</a>';
			}
			return origens;
		}
	},
	{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "35%", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false,
		"mRender": function (data, type, full) {
			return full.fields.partial[0].ds_ementa;
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao",
		"mRender": function (data, type, full) {
			return full.fields.partial[0].nm_situacao;
		}
	},
	{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "", "sClass": "center ws all", "bSortable": false,
		"mRender": function (data, type, full) {
			var detalhes = "";
			var ds_norma = full.nm_tipo_norma + " " + full.nr_norma + " " + full.dt_assinatura;
			var selecionar = "<a href='javascript:void(0);' onclick='javascript:Notificar(\"" + full.fields.partial[0].ch_norma + "\");' title='Selecionar'><img valign='absmiddle' alt='Selecionar' src='" + _urlPadrao + "/Imagens/ico_ok_p.png' /></a>";
			return detalhes + selecionar;
		}
	}
];

//var _columns_norma_cesta = [
//{ "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Tipo e Número", "sWidth": "20%", "sClass": "grid-cell ws _all nm_tipo_norma nr_norma", "mData": "nm_tipo_norma",
//    "mRender": function (data, type, full) {
//        return '<a href="./DetalhesDeNorma.aspx?id_doc=' + full.fields.partial[0]._metadata.id_doc + '" title="Visualizar detalhes da norma" />' + full.fields.partial[0].nm_tipo_norma + " " + (full.fields.partial[0].nr_norma != 0 ? full.fields.partial[0].nr_norma : "") + ' <img alt="detalhes" src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" /></a>';
//    }
//},
//{ "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Assinatura", "sWidth": "", "sClass": "grid-cell ws dt_assinatura", "mData": "dt_assinatura",
//    "mRender": function (data, type, full) {
//        return full.fields.partial[0].dt_assinatura;
//    }
//},
//{ "indice": 2, "isControl": false, "standard_view": true, "sTitle": "Origem", "sWidth": "", "sClass": "grid-cell ws sg_orgao", "mData": "origens", "bSortable": false,
//    "mRender": function (data, type, full) {
//        var origens = "";
//        for (var i = 0; i < full.fields.partial[0].origens.length; i++) {
//            origens += (origens != "" ? "<br/>" : "") + '<a href="javascript:void(0)" onclick="javascript:CriarModalDescricaoOrigem(\'' + full.fields.partial[0].origens[i].ch_orgao + '\')">' + full.fields.partial[0].origens[i].sg_orgao + '</a>';
//        }
//        return origens;
//    }
//},
//{ "indice": 3, "isControl": false, "standard_view": true, "sTitle": "Ementa", "sWidth": "", "sClass": "grid-cell ws ds_ementa", "mData": "ds_ementa", "bSortable": false,
//    "mRender": function (data, type, full) {
//        return full.fields.partial[0].ds_ementa;
//    }
//},
//{ "indice": 4, "isControl": false, "standard_view": true, "sTitle": "Situação", "sWidth": "", "sClass": "center ws nm_situacao", "mData": "nm_situacao",
//    "mRender": function (data, type, full) {
//        return full.fields.partial[0].nm_situacao;
//    }
//},

//{ "indice": 5, "isControl": false, "standard_view": true, "sTitle": " ", "sWidth": "110px", "sClass": "center ws", "bSortable": false,
//    "mRender": function (data, type, full) {
//        var remover = "<a title='Remover da Cesta' href='javascript:ExcluirDaCesta(\"norma_" + full.fields.partial[0]._metadata.id_doc + "\");' class='a_delete' ><img valign='absmiddle' alt='Excluir'  src='" + _urlPadrao + "/Imagens/ico_trash_p.png'  /></a>";

//        var id_file = "";
//        var links = "";
//        if (IsNotNullOrEmpty(full.fields.partial[0].ar_atualizado, 'id_file')) {
//            id_file = full.fields.partial[0].ar_atualizado.id_file;
//        }
//        else if (IsNotNullOrEmpty(full.fields.partial[0].fontes) && full.fields.partial[0].fontes.length > 0 && IsNotNullOrEmpty(full.fields.partial[0].fontes[0].ar_fonte, 'id_file')) {
//            id_file = full.fields.partial[0].fontes[0].ar_fonte.id_file;
//        }
//        if (IsNotNullOrEmpty(id_file)) {
//            links = '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoNorma.aspx?id_file=' + id_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' +
//                    '&nbsp;&nbsp;' +
//                    '<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a> &nbsp;&nbsp;';
//        }
//        return links + remover;
//    }
//}
//];

var _columns_erro_sistema = [
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Data", "sWidth": "", "sClass": "grid-cell ws", "mData": "dt_log_erro" },
    { "indice": 1, "isControl": false, "standard_view": true, "sTitle": "Tipo", "sWidth": "", "sClass": "grid-cell ws", "mData": "nm_tipo" },
    { "indice": 2, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": "Usuário", "sWidth": "", "sClass": "grid-cell ws", "mData": "nm_login_user_erro" },
//    { "indice": 3, "sDefaultContent": "", "isControl": false, "standard_view": true, "sTitle": "Mensagem", "sWidth": "40%", "sClass": "center ws all", "mData": "", "bSortable": false,
//        "mRender": function (data, type, full) {
//            var oErro = JSON.parse(full.ds_erro);
//            if (IsNotNullOrEmpty(oErro, "Erro.MensagemDaExcecao")) {
//                return oErro.Erro.MensagemDaExcecao;
//            }
//        }
//    },
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
    { "indice": 0, "isControl": false, "standard_view": true, "sTitle": "Nome", "sWidth": "", "sClass": "grid-cell ws left", "mData": "nm_arquivo",
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
                html = '<div class="div_arq" chave="' + full.ch_arquivo + '">' + ico + ' ' + full.nm_arquivo + '.' + tipo + '</div>';
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
                var tipo = full.ar_arquivo.mimetype.split('/')[1];
                var ico = '<img src="' + _urlPadrao + '/Imagens/ico_file.png" width="18" height="18" /> ';
                var bt_download = '<a target="_blank" href="./Download/sinj_arquivo/' + full.ar_arquivo.id_file + '/' + full.ar_arquivo.filename + '" title="Baixar Arquivo" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_download.png" alt="download" /></a>';
                var bt_edit = '';
                var bt_link_edit = '';
                var bt_convert = '';
                var bt_import = '';
                var bt_delete = '';
                if (IsNotNullOrEmpty(window, 'bProduzir')) {
                    if (tipo == 'html') {
                        bt_edit = '&nbsp;<a href="javascript:void(0);" onclick="javascript:openDialogEditFile(this)" title="Editar Arquivo"><img height="16" src="' + _urlPadrao + '/Imagens/ico_edit_dir.png" alt="editar html" /></a>';
                        bt_link_edit = '&nbsp;<a href="javascript:void(0);" onclick="javascript:openDialogEditLink(this)" title="Editar Links"><img height="16" src="' + _urlPadrao + '/Imagens/ico_edit_link.png" alt="editar link" /></a>';
                        bt_convert = '&nbsp;<a target="_blank" href="./htmltopdf/dc_arquivo/' + full.ar_arquivo.id_file + '/' + full.ar_arquivo.filename.replace(".html", ".pdf").replace(".htm", ".pdf") + '" title="Converter e Baixar em PDF" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="converter" /></a>';
                    }
                    if (full.ch_arquivo.indexOf('000shared/images') < 0) {
                        bt_delete = '&nbsp;<a href="javascript:void(0);" onclick="javascript:openDialogDeleteFile(this)" title="Excluir Arquivo"><img height="16" src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="excluir" /></a>';
                    }
                } else {
                    if (tipo == 'html') {
                        bt_import = '&nbsp;<a target="_blank" href="javascript:void(0);" onclick="javascript:selecionarDocumentoImportar(this);" title="Selecionar documento produzido" ><img height="16" src="' + _urlPadrao + '/Imagens/ico_check_p.png" alt="selecionar" /></a>';
                    }
                }
                return bt_download + bt_import + bt_convert + bt_edit + bt_link_edit + bt_delete;
            }
            else {
                return '<a href="javascript:void(0);" onclick="javascript:openDialogDeleteFolder(this)" title="Excluir Diretório" ch_folder_selected="'+full.ch_arquivo+'" nr_nivel_arquivo_selected="'+full.nr_nivel_arquivo+'"><img height="16" src="' + _urlPadrao + '/Imagens/ico_del_dir.png" alt="excluir" /></a>';
            }
        }
    }
];