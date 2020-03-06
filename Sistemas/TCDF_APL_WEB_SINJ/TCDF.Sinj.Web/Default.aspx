<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TCDF.Sinj.Web.Default" %>
<asp:Content ID="ContentHeader" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript"src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_pesquisas.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            _campos_pesquisa_avancada.push({
                ch_campo: "dt_cadastro",
                nm_campo: "Data de Cadastro",
                type: "datetime"
            });
            _campos_pesquisa_avancada.push({
                ch_campo: "nm_orgao_cadastrador",
                nm_campo: "Órgão Cadastrador",
                type: "autocomplete",
                data: '[{ "nm_orgao_cadastrador": "CLDF" }, { "nm_orgao_cadastrador": "PGDF" }, { "nm_orgao_cadastrador": "SEPLAG" }, { "nm_orgao_cadastrador": "TCDF"}]',
                url_ajax: null,
                sKeyDataName: "nm_orgao_cadastrador",
                sValueDataName: "nm_orgao_cadastrador"
            });
            montarPaginaDePesquisa();
        });
        
    </script>
    
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="Body" runat="server">
    <div id="slave">
        <div class="divIdentificadorDePagina">
	        <label>.: Pesquisas</label>
	    </div>
        <div class="mauto w-90-pc mtop25">
			<div class="accordion">
				<h3><a href="#" data-toggle="tooltip" title='Informe o número, tipo de norma, termo ou frase que deseja localizar nos atos normativos ou diários oficiais. Use as aspas para encontrar uma frase exata. Ex: “Restaurante comunitário”.'>Pesquisa Geral <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a></h3>
                    <div class="h-100-px">
						<div class="div-accordion-content">
                            <div class="pesquisa">
                                <form id="form_pesquisa_geral" name="formPesquisaGeral" action="./ResultadoDePesquisa" method="get">
                                    <input name="tipo_pesquisa" type="hidden" value="geral" />
                                    <div class="mauto table w-100-pc" style="margin-top:20px;">
                                        <div class="line">
                                            <div class="column w-100-pc">
                                                <input autofocus id="input_geral_all" name="all" type="text" value="" class="w-95-pc fl" />
                                                <button type="submit" class="fl clean" title="Pesquisar" >
                                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-lupa-p_green.png" alt="pesquisar" />
                                                </button>
                                                <a href="javascript:void(0);" data-toggle="tooltip" title='Informe o número, tipo de norma, termo ou frase que deseja localizar nos atos normativos ou diários oficiais. Use as aspas para encontrar uma frase exata. Ex: “Restaurante comunitário”.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
				<h3><a href="#" data-toggle="tooltip" title="Permite pesquisar nos atos normativos, por número, tipo de norma, ano de publicação e origem. A pesquisa também pode ser realizada por termos livres ou de indexação.">Pesquisa de Normas <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a></h3>
					<div>
						<div class="div-accordion-content">
                            <div class="pesquisa">
                                <form id="form_pesquisa_norma" name="formPesquisaNorma" action="./ResultadoDePesquisa" method="get">
                                    <input name="tipo_pesquisa" type="hidden" value="norma" />
                                    <div class="mauto table w-100-pc">
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Qualquer Campo:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <input id="input_norma_all" name="all" type="text" value="" class="w-95-pc" /><a href="javascript:void(0);" data-toggle="tooltip" title='Informe palavras ou frases que deseja buscar nos normativos. Use as aspas para encontrar uma frase exata. Ex: “Restaurante comunitário”.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Tipo:</label>
                                                </div>
                                            </div>
                                            <div id="div_autocomplete_tipo_norma" class="column w-80-pc">
                                                <input id="ch_tipo_norma" name="ch_tipo_norma" type="hidden" value="" />
                                                <input autocomplete="off" id="nm_tipo_norma" name="nm_tipo_norma" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_norma"></a><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o tipo de norma que deseja pesquisar. Ex: decreto, portaria, lei.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                         
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Número:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <input name="nr_norma" type="text" value="" class="w-20-pc" /><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o número da norma. Caso não saiba, deixe o campo em branco. Somente clique em “Norma sem número” se o ato não tiver numeração.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            	<input type="checkbox" name="norma_sem_numero" value="true" style="margin-left:3px; margin-right:0;"/> <label>Norma sem número</label>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Ano:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <input name="ano_assinatura" type="text" value="" class="w-20-pc" /><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o ano de publicação da norma pretendida, com quatro dígitos.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Assunto:</label>
                                                </div>
                                            </div>
                                            <div id="div_autocomplete_assunto" class="column w-80-pc">
                                                <input id="ch_termo_assunto" type="hidden" value="" />
                                                <input autocomplete="off" id="nm_termo_assunto" type="text" value="" class="w-80-pc" onblur="javascript:AdicionarAssunto()"= /><a title="Listar" id="a_assunto"></a><a href="javascript:void(0);" data-toggle="tooltip" title='Informe as palavras-chave do assunto da norma. Ex.: “Leite materno”.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div id="line_assuntos" class="line">
                                            <div class="column w-20-pc">
                                                &nbsp;
                                            </div>
                                            <div class="column w-80-pc">
                                                <fieldset>
                                                    <legend>Assuntos</legend>
                                                    <div id="assuntos" class="w-100-pc"></div>
                                                </fieldset>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Origem:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <div id="div_autocomplete_origem" class="cell w-100-pc">
                                                    <input id="ch_orgao" name="ch_orgao" type="hidden" value="" />
                                                    <input id="ch_hierarquia" name="ch_hierarquia" type="hidden" value="" />
                                                    <input autocomplete="off" id="sg_hierarquia_nm_vigencia" name="sg_hierarquia_nm_vigencia" type="text" value="" class="w-80-pc" onblur="javascript:SelecionarOrigem();" /><a title="Listar" id="a_origem"></a><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o órgão que elaborou a norma.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="line_origem_por" class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Origem por:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <div class="cell w-100-pc">
                                                    <select name="origem_por" >
                                                        <option value="toda_a_hierarquia_em_qualquer_epoca"></option>
                                                        <option value="somente_o_orgao_selecionado">Somente o orgão selecionado</option>
                                                        <option value="hierarquia_superior">Hierarquia Superior</option>
                                                        <option value="hierarquia_inferior">Hierarquia Inferior</option>
                                                        <option value="toda_a_hierarquia">Toda a Hierarquia</option>
                                                        <option value="em_qualquer_epoca">Em qualquer Época</option>
                                                        <option value="toda_a_hierarquia_em_qualquer_epoca">Toda a hierarquia em qualquer Época</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="width:150px;" class="loaded fr">
                                            <button type="submit" id="button_pesquisa_norma" class="pesquisa">
                                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-lupa-p_green.png" />
                                            </button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
					</div>
				<h3><a href="#" data-toggle="tooltip" title="Permite a consulta direta nos diários oficiais a partir do número, seção, data de publicação e também de forma livre nos textos do diário.">Pesquisa de Diário <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a></h3>
					<div>
						<div class="div-accordion-content">
                            <div class="pesquisa">
                                <form id="form_pesquisa_diario" name="formPesquisaDiario" action="./ResultadoDePesquisa" method="get">
                                    <input name="tipo_pesquisa" type="hidden" value="diario"/>
                                    <div class="mauto table w-100-pc">
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Tipo:</label>
                                                </div>
                                            </div>
                                            <div id="div_autocomplete_tipo_fonte" class="column w-60-pc">
                                                <input id="ch_tipo_fonte" name="ch_tipo_fonte" type="hidden" value="" />
                                                <input autocomplete="off" id="nm_tipo_fonte" name="nm_tipo_fonte" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_fonte"></a><a href="javascript:void(0);" data-toggle="tooltip" title='Informe em qual diário deseja pesquisar. Ex: DODF, DOCL, DJe.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Tipo de Edição:</label>
                                                </div>
                                            </div>
                                            <div id="div_autocomplete_tipo_edicao" class="column w-60-pc">
                                                <input id="ch_tipo_edicao" name="ch_tipo_edicao" type="hidden" value="" />
                                                <input autocomplete="off" id="nm_tipo_edicao" name="nm_tipo_edicao" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_tipo_edicao"></a><a href="javascript:void(0);" data-toggle="tooltip" title='Informe em qual tipo de Edição deseja pesquisar.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Número:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <input name="nr_diario" type="number" value="" class="w-20-pc" /><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o número do diário.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Seção:</label>
                                                </div>
                                            </div>
                                            <div class="column w-70-pc">
                                                <div class="cell w-100-pc">
                                                    <label style="margin-right:10px;"><input type="checkbox" name="secao_diario" value="1" style="vertical-align:middle;"/> 1</label>
                                                    <label style="margin-right:10px;"><input type="checkbox" name="secao_diario" value="2" style="vertical-align:middle;"/> 2</label>
                                                    <label style="margin-right:10px;"><input type="checkbox" name="secao_diario" value="3" style="vertical-align:middle;"/> 3</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Texto:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <input name="filetext" type="text" value="" class="w-90-pc" /><a href="javascript:void(0);" data-toggle="tooltip" title='Informe o assunto ou frase que queira localizar nos diários. Use as aspas para encontrar uma frase exata. Ex: “Restaurante comunitário”.'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                            </div>
                                        </div>
                                        <div class="line">
                                            <div class="column w-20-pc">
                                                <div class="cell fr">
                                                    <label>Data de Publicação:</label>
                                                </div>
                                            </div>
                                            <div class="column w-80-pc">
                                                <div class="w-100-pc fl">
                                                    <select class="fl" id="op_dt_assinatura" name="op_dt_assinatura" onchange="javascript:SelecionarOperador();">
                                                        <option value="igual">igual</option>
                                                        <option value="menor">menor</option>
                                                        <option value="menorouigual">menor ou igual</option>
                                                        <option value="maior">maior</option>
                                                        <option value="maiorouigual">maior ou igual</option>
                                                        <option value="diferente">diferente</option>
                                                        <option value="intervalo">intervalo</option>
                                                    </select>
                                                    <input name="dt_assinatura" type="text" value="" class="date fl" style="margin-left:5px; width:100px;" />
                                                    <div id="div_intervalo" class="fl" style="display:none; margin-left:5px;">
                                                        até
                                                        <input name="dt_assinatura" type="text" value="" class="date" style="margin-left:5px; width:100px;" />
                                                    </div>
                                                    <a href="javascript:void(0);" data-toggle="tooltip" title='Informe a data de publicação do diário. (dd/mm/aaaa).'>&nbsp;<img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="width:150px;" class="loaded fr">
                                            <button type="submit" id="button_pesquisa_diario" class="pesquisa">
                                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-lupa-p_green.png" />
                                            </button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
					</div>
                <h3><a href="#" data-toggle="tooltip" title="Permite realizar a busca por meio da combinação de vários campos específicos, com o objetivo de refinar o resultado da pesquisa.">Pesquisa Avan&ccedil;ada <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_help.png" alt="info" width="12px" height="12px" /></a></h3>
					<div>
						<div class="div-accordion-content">
                            <div class="pesquisa">
                                <div class="mauto table w-100-pc">
                                    <div class="line">
                                        <div class="column w-30-pc">
                                            <div class="cell">
                                                <label>Campo</label>
                                            </div>
                                        </div>
                                        <div class="column w-30-pc">
                                            <div class="cell">
                                                <label>Operador</label>
                                            </div>
                                        </div>
                                        <div class="column w-30-pc">
                                            <div class="cell">
                                                <label>Argumento</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="line">
                                        <div class="column w-30-pc">
                                            <div id="div_autocomplete_campo" class="cell w-100-pc">
                                                <input id="ch_campo" type="hidden" value="" />
                                                <input id="type" type="hidden" value="" />
                                                <input id="url_ajax" type="hidden" value="" />
                                                <input id="data" type="hidden" value="" />
                                                <input id="sKeyDataName" type="hidden" value="" />
                                                <input id="sValueDataName" type="hidden" value="" />
                                                <input id="nm_campo" type="text" value="" class="w-70-pc" onblur="javascript:SelecionarCampo();" /><a title="Listar" id="a_campo"></a>
                                            </div>
                                        </div>
                                        <div class="column w-30-pc">
                                            <div id="div_autocomplete_operador" class="cell w-100-pc">
                                                <input id="ch_operador" type="hidden" value="" />
                                                <input id="nm_operador" type="text" value="" class="w-70-pc" onblur="javascript:SelecionarOperadorPesquisaAvancada();" /><a title="Listar" id="a_operador"></a>
                                            </div>
                                        </div>
                                        <div class="column w-40-pc">
                                            <div id="div_valor" class="cell w-100-pc">
                                                <input id="ch_valor" type="hidden" value="" />
                                                <input id="nm_valor" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_valor"></a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="line">
                                        <div class="column w-100-pc">
                                            <div class="cell w-100-pc">
                                                <div class="div-light-button fr" style="margin:10px;">
                                                    <a href="javascript:void(0);" onclick="javascript:AdicionarArgumento();" title="Adicionar argumento de pesquisa"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png" >Adicionar</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <form id="form_pesquisa_avancada" name="formPesquisaAvancada" action="./ResultadoDePesquisa" method="get">
                                    <input name="tipo_pesquisa" type="hidden" value="avancada" />
                                    <div class="notify" style="display:none;"></div>
                                    <fieldset>
                                        <legend>Argumentos de Pesquisa</legend>
                                        <div id="div_argumentos" class="mauto table w-100-pc">
                                            <div class="line">
                                                <div class="column w-20-pc">
                                                    <div class="cell w-100-pc">
                                                        Campo
                                                    </div>
                                                </div>
                                                <div class="column w-10-pc">
                                                    <div class="cell w-100-pc">
                                                        Operador
                                                    </div>
                                                </div>
                                                <div class="column w-50-pc">
                                                    <div class="cell w-100-pc">
                                                        Argumento
                                                    </div>
                                                </div>
                                                <div class="column w-10-pc">
                                                    <div class="cell w-100-pc">
                                                        Conector
                                                    </div>
                                                </div>
                                                <div class="column w-10-pc">
                                                    <div class="cell w-100-pc">
                                                        Remover
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset class="w-40-pc">
                                        <legend>Pesquisar nas seguintes normas:</legend>
                                        <label><input id="ch_tipo_norma_todas" obrigatorio="sim" label="Pesquisar nas seguintes normas" type="checkbox" name="ch_tipo_norma" value="todas" onclick="javascript:SelecionarTipoDeNorma()" />Todas</label><br />
                                        <div id="loading_tipos_de_norma" class="loading-p" style="display:none;"></div>
                                        <div id="div_tipos_de_norma"  style="max-height:100px; overflow-y:scroll; display:none;padding-left:5px;"></div>
                                    </fieldset>
                                    <div class="loaded">
                                        <button type="submit" id="button_pesquisa_avancada" class="pesquisa fr">
                                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-lupa-p_green.png" />
                                        </button>
                                    </div>
                                </form>
                            </div>
                        </div>
					</div>
			</div>
        </div>
    </div>
</asp:Content>

