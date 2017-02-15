function (json_tipo_norma){
	'var checkox = \'\''+
	'for(var i = 0; i < json_tipo_norma.length; i++){'+
		'checkbox += \'<label><input type="checkbox" value="\' + json_tipo_norma[i].id_tipo_norma + \'" />\' + json_tipo_norma[i].nm_tipo_norma + \'</label>\';' +
	'}'+
	'<div class="line h-150-px display-if-added" style="display:none;">' +
		'<div class="column w-50-pc border1">' +
			'<div>' +
				'<div class="w-100-pc">' +
					'<b>Pesquisar nas seguintes Normas:</b>' +
				'</div>' +
				'<div>' +
					'<label><input type="checkbox" id="checkbox_all_tipo_norma" onchange="verifyCheckboxTipoNorma(event);" checked="checked" value="todas"/>Todas</label>' +
				'</div>' +
				'<div id="div_checkbox_tipo_norma" class="w-95-pc h-100-px scroll-y" style="padding-left:5%;">'+
					'checkbox' +
				'</div>' +
			'</div>' +
		'</div>' +
		'<script type="text/javascript" language="javascript">'+
			'function verifyCheckboxTipoNorma(e){'+
				'var $target = $(e.target);'+
				'var checked = #target.is(\':checked\');'+
				'if(checked){'+
					'$(\'input[type=checkbox]\', \'#div_checkbox_tipo_norma\').attr(\'checked\', false);'+
					'$(\'input[type=checkbox]\', \'#div_checkbox_tipo_norma\').attr(\'enabled\', false);'+
				'}'+
				'else{'+
					'$(\'input[type=checkbox]\', \'#div_checkbox_tipo_norma\').attr(\'enabled\', true);'+
				'}'+
			'}'+
		'</script>'+
	'</div>' +
}