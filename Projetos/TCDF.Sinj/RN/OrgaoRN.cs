using System;
using neo.BRLightREST;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using System.Collections.Generic;

namespace TCDF.Sinj.RN
{
    public class OrgaoRN
    {
        private OrgaoAD _orgaoAd;

        public OrgaoRN()
        {
            _orgaoAd = new OrgaoAD();
        }

        public Results<OrgaoOV> Consultar(Pesquisa query)
        {
            return _orgaoAd.Consultar(query);
        }

        public OrgaoOV Doc(ulong id_doc)
        {
            var orgaoOv = _orgaoAd.Doc(id_doc);
            if (orgaoOv == null)
            {
                throw new DocNotFoundException("Órgão não Encontrado.");
            }
            return orgaoOv;
        }

        public OrgaoOV Doc(string ch_orgao)
        {
            return _orgaoAd.Doc(ch_orgao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _orgaoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _orgaoAd.JsonReg(query);
        }

        public string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _orgaoAd.PathPut(id_doc, path, value, retorno);
        }

        public string PathPut(Pesquisa pesquisa, List<opMode<OrgaoOV>> listopMode)
        {
            return _orgaoAd.PathPut(pesquisa, listopMode);
        }

        public string PathGet(ulong id_doc, string path)
        {
            return _orgaoAd.PathGet(id_doc, path);
        }

        public ulong Incluir(OrgaoOV orgaoOv)
        {
            Validar(orgaoOv);
            orgaoOv.st_autoridade = false;
            return _orgaoAd.Incluir(orgaoOv);
        }

        public bool Atualizar(ulong id_doc, OrgaoOV orgaoOv, bool validar = true)
        {
            if(validar) Validar(orgaoOv);
            //Note:Ajusta a hierarquia dos órgãos filhos caso a hierarquia do órgão pai tenha sido alterada
            var orgaoOv_antes = Doc(orgaoOv._metadata.id_doc);
            if (orgaoOv_antes.sg_hierarquia != orgaoOv.sg_hierarquia || orgaoOv_antes.nm_hierarquia != orgaoOv.nm_hierarquia)
            {
                var orgaos_filhos = BuscarOrgaosFilhos(orgaoOv.ch_orgao);
                if (orgaos_filhos != null && orgaos_filhos.Count > 0)
                {
                    foreach (var orgao_filho in orgaos_filhos)
                    {
                        orgao_filho.sg_hierarquia = orgaoOv.sg_hierarquia + ">" + orgao_filho.sg_orgao;
                        orgao_filho.nm_hierarquia = orgaoOv.nm_hierarquia + ">" + orgao_filho.nm_orgao;
                        orgao_filho.ch_hierarquia = orgaoOv.ch_hierarquia + "." + orgao_filho.ch_orgao;
                        _orgaoAd.Atualizar(orgao_filho._metadata.id_doc, orgao_filho);
                    }
                }
            }
            return _orgaoAd.Atualizar(id_doc, orgaoOv);
        }

        //public List<OrgaoOV> BuscarOrgaosDaHierarquia(string ch_orgao)
        //{
        //    Pesquisa query = new Pesquisa();
        //    query.limit = null;
        //    query.literal = "Upper(ch_hierarquia) = '" + ch_orgao.ToUpper() + "' OR Upper(ch_hierarquia) like '" + ch_orgao.ToUpper() + ".%' OR Upper(ch_hierarquia) like '%." + ch_orgao.ToUpper() + ".%'";
        //    return Consultar(query).results;
        //}


        public List<OrgaoOV> BuscarOrgaosDaHierarquia(string ch_orgao, string ch_hierarquia)
        {
            Pesquisa query = new Pesquisa();
            query.limit = null;
            var ch_hierarquia_split = ch_hierarquia.Split('.');
            foreach (var chave in ch_hierarquia_split)
            {
                query.literal += (!string.IsNullOrEmpty(query.literal) ? " OR " : "") + "ch_orgao='" + chave + "'";
            }
            query.literal += (!string.IsNullOrEmpty(query.literal) ? " OR " : "") + "Upper(ch_hierarquia) like '" + ch_orgao.ToUpper() + ".%' OR Upper(ch_hierarquia) like '%." + ch_orgao.ToUpper() + ".%'";

            return Consultar(query).results;
        }

        public List<OrgaoOV> BuscarOrgaosDaCronologia(string ch_orgao)
        {
            OrgaoOV orgaoOv = new OrgaoOV();
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.literal = "ch_orgao='" + ch_orgao + "'";
            orgaoOv = Consultar(query).results[0];
            query.limit = null;
            if (orgaoOv.ch_cronologia.Count > 0)
            {
                query.literal = "";
                foreach (var ch_crono in orgaoOv.ch_cronologia)
                {
                    var split_ch_crono = ch_crono.Split('.');
                    foreach (var ch_orgao_cronologico in split_ch_crono)
                    {
                        query.literal += (query.literal != "" ? " OR " : "id_doc=ANY(select id_doc from (select id_doc, unnest(ch_cronologia) ch) x where ") + "ch_orgao = '" + ch_orgao_cronologico + "'";

                    }
                }
                query.literal += (query.literal != "" ? " OR " : "") + "ch like '" + ch_orgao + ".%' OR ch like '%." + ch_orgao + ".%')";
                return Consultar(query).results;
            }
            else
            {
                query.literal = string.Format("id_doc=ANY(select id_doc from (select id_doc, unnest(ch_cronologia) ch) x where ch='{0}' OR ch like '%.{0}' OR ch like '{0}.%')", ch_orgao);
                return Consultar(query).results;                
            }
        }

        public List<OrgaoOV> BuscarTodaHierarquiaEmQualquerEpoca(string ch_orgao, string ch_hierarquia)
        {
            OrgaoOV orgaoOv = new OrgaoOV();
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.literal = "ch_orgao='" + ch_orgao + "'";
            orgaoOv = Consultar(query).results[0];
            query.limit = null;
            if (orgaoOv.ch_cronologia.Count > 0)
            {
                query.literal = "";
                foreach (var ch_crono in orgaoOv.ch_cronologia)
                {
                    var split_ch_crono = ch_crono.Split('.');
                    foreach (var ch_orgao_cronologico in split_ch_crono)
                    {
                        query.literal += (query.literal != "" ? " OR " : "id_doc=ANY(select id_doc from (select id_doc, unnest(ch_cronologia) ch) x where ") + "ch_orgao = '" + ch_orgao_cronologico + "'";

                    }
                }
                query.literal += (query.literal != "" ? " OR " : "") + "ch like '" + ch_orgao + ".%' OR ch like '%." + ch_orgao + ".%')";
            }
            else
            {
                query.literal = string.Format("id_doc=ANY(select id_doc from (select id_doc, unnest(ch_cronologia) ch) x where ch='{0}' OR ch like '%.{0}' OR ch like '{0}.%')", ch_orgao);
            }

            var query_hierarquia = "";
            var ch_hierarquia_split = ch_hierarquia.Split('.');
            foreach (var chave in ch_hierarquia_split)
            {
                query_hierarquia += (!string.IsNullOrEmpty(query_hierarquia) ? " OR " : "") + "ch_orgao='" + chave + "'";
            }
            query_hierarquia += (!string.IsNullOrEmpty(query_hierarquia) ? " OR " : "") + "Upper(ch_hierarquia) like '" + ch_orgao.ToUpper() + ".%' OR Upper(ch_hierarquia) like '%." + ch_orgao.ToUpper() + ".%'";

            if (!string.IsNullOrEmpty(query_hierarquia))
            {
                query.literal += " OR " + query_hierarquia;
            }

            return Consultar(query).results;
        }

        //public List<OrgaoOV> BuscarOrgaosDaCronologia(string ch_orgao)
        //{
        //    Pesquisa query = new Pesquisa();
        //    query.limit = null;
        //    //query.literal = "'" + ch_orgao + "'=any(ch_cronologia)";
        //    query.literal = string.Format("id_doc=ANY(select id_doc from (select id_doc, unnest(ch_cronologia) ch) x where ch='{0}' OR ch like '%.{0}' OR ch like '{0}.%')", ch_orgao);
        //    return Consultar(query).results;
        //}

        public List<OrgaoOV> BuscarOrgaosFilhos(string ch_orgao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = null;
            query.literal = string.Format("ch_orgao_pai='{0}'", ch_orgao);
            return Consultar(query).results;
        }

        public List<OrgaoOV> BuscarOrgaosAnteriores(string[] ch_orgao_anterior)
        {
            Pesquisa query = new Pesquisa();
            query.limit = null;
            for (var i = 0; i < ch_orgao_anterior.Length; i++)
            {
                query.literal += (string.IsNullOrEmpty(query.literal) ? "" : " OR ") + string.Format("ch_orgao='{0}'", ch_orgao_anterior[i]);
            }
            return Consultar(query).results;
        }

        public List<OrgaoOV> BuscarOrgaosPosteriores(string ch_orgao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = null;
            query.literal = string.Format("'{0}'=any(ch_orgao_anterior)", ch_orgao);
            return Consultar(query).results;
        }

        public List<OrgaoOV> BuscarHierarquiaSuperior(string ch_hierarquia)
        {
            Pesquisa query = new Pesquisa();
            query.limit = null;
            var ch_hierarquia_split = ch_hierarquia.Split('.');
            foreach (var chave in ch_hierarquia_split)
            {
                query.literal += (!string.IsNullOrEmpty(query.literal) ? " OR " : "") + "ch_orgao='" + chave + "'";
            }
            return Consultar(query).results;
        }

        public List<OrgaoOV> BuscarHierarquiaInferior(string ch_orgao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = null;
            query.literal = "Upper(ch_hierarquia) like '" + ch_orgao.ToUpper() + ".%' OR Upper(ch_hierarquia) like '%." + ch_orgao.ToUpper() + ".%' OR Upper(ch_orgao) = '" + ch_orgao.ToUpper() +"'";
            return Consultar(query).results;
        }

        public bool Excluir(ulong id_doc)
        {
            var orgaoOv = Doc(id_doc);
            ValidarDepencias(orgaoOv);
            return _orgaoAd.Excluir(id_doc);
        }

        private void Validar(OrgaoOV orgaoOv)
        {
            if (string.IsNullOrEmpty(orgaoOv.nm_orgao))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            if (string.IsNullOrEmpty(orgaoOv.sg_orgao))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            if (!orgaoOv.st_orgao && string.IsNullOrEmpty(orgaoOv.dt_fim_vigencia))
            {
                throw new DocValidacaoException("Data fim vigência inválida. Órgão inativo precisa da data de fim de vigência.");
            }
            if (string.IsNullOrEmpty(orgaoOv.dt_inicio_vigencia))
            {
                throw new DocValidacaoException("Data início vigência inválida.");
            }
            orgaoOv.dt_inicio_vigencia = (Convert.ToDateTime(orgaoOv.dt_inicio_vigencia)).ToString("dd'/'MM'/'yyyy");

			if (!string.IsNullOrEmpty(orgaoOv.dt_fim_vigencia))
			{
				orgaoOv.dt_fim_vigencia = (Convert.ToDateTime(orgaoOv.dt_fim_vigencia)).ToString("dd'/'MM'/'yyyy");
				if (Convert.ToDateTime(orgaoOv.dt_fim_vigencia) > DateTime.Now || Convert.ToDateTime(orgaoOv.dt_fim_vigencia) < Convert.ToDateTime(orgaoOv.dt_inicio_vigencia))
				{
					throw new DocValidacaoException("Data fim vigência inválida.");
				}

			}
            if (orgaoOv.ambito == null)
            {
                throw new DocValidacaoException("Âmbito inválido.");
            }
            if (string.IsNullOrEmpty(orgaoOv.nm_login_usuario_cadastro))
            {
                throw new DocValidacaoException("Usuário Cadastro inválido.");
            }
            if (string.IsNullOrEmpty(orgaoOv.dt_cadastro))
            {
                throw new DocValidacaoException("Data Cadastro inválido.");
            }
        }

        private void ValidarDepencias(OrgaoOV orgaoOv)
        {
            var orgaos_filhos = BuscarOrgaosFilhos(orgaoOv.ch_orgao);
            if (orgaos_filhos.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Órgão não pode ser excluído possuindo órgãos na hierarquia inferior.");
            }
            var orgaos_posteriores = BuscarOrgaosPosteriores(orgaoOv.ch_orgao);
            if (orgaos_posteriores.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Órgão não pode ser excluído possuindo órgãos posteriores.");
            }
            var normas_do_orgao = new NormaRN().BuscarNormasDoOrgao(orgaoOv.ch_orgao);
            if(normas_do_orgao.Count > 0){
                throw new DocDependenciesException("Erro de dependência. O Órgão está sendo usado por uma ou mais normas.");
            }
        }

        public bool ValidarFilhos(OrgaoOV orgaoOv, ref List<ValidacaoFilhos> lista_validacao_erros)
        {
            try
            {
                Validar(orgaoOv);
                return true;
            }
            catch (Exception ex) 
            {
                lista_validacao_erros.Add(new ValidacaoFilhos{ch_orgao = orgaoOv.ch_orgao, nm_orgao = orgaoOv.nm_orgao, error_msg = ex.Message});
                return false;
            }
        }
    }
}
