using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class FaleConoscoRN
    {
        private FaleConoscoAD _faleConoscoAd;

        public FaleConoscoRN()
        {
            _faleConoscoAd = new FaleConoscoAD();
        }

        public Results<FaleConoscoOV> Consultar(Pesquisa query)
        {
            return _faleConoscoAd.Consultar(query);
        }

        public FaleConoscoOV Doc(ulong id_doc)
        {
            var faleConoscoOv = _faleConoscoAd.Doc(id_doc);
            if (faleConoscoOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return faleConoscoOv;
        }

        public FaleConoscoOV Doc(string ch_chamado)
        {
            return _faleConoscoAd.Doc(ch_chamado);
        }

        public string JsonReg(ulong id_doc)
        {
            return _faleConoscoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _faleConoscoAd.JsonReg(query);
        }

        public ulong Incluir(FaleConoscoOV faleConoscoOv)
        {
            faleConoscoOv.ch_chamado = Guid.NewGuid().ToString("N");
            
            Validar(faleConoscoOv);
            faleConoscoOv.st_atendimento = "Novo";
            var dt_controle = DateTime.Now.ToString("ddMMyyyyHHmm").Substring(0, 11);
            faleConoscoOv.ch_controle_excesso_email = faleConoscoOv.ds_email + "_" + dt_controle;

            Pesquisa query = new Pesquisa() { select = new string[0] };
            query.literal = string.Format("ch_controle_excesso_email='{0}'", faleConoscoOv.ch_controle_excesso_email);

            if (Consultar(query).result_count > 0)
            {
                throw new DocDuplicateKeyControlException();
            }

            return _faleConoscoAd.Incluir(faleConoscoOv);
        }

        public bool Atualizar(ulong id_doc, FaleConoscoOV faleConoscoOv)
        {
            Validar(faleConoscoOv);
            return _faleConoscoAd.Atualizar(id_doc, faleConoscoOv);
        }

        public bool Excluir(ulong id_doc)
        {
            return _faleConoscoAd.Excluir(id_doc);
        }

        private void Validar(FaleConoscoOV faleConoscoOv)
        {
            if (string.IsNullOrEmpty(faleConoscoOv.nm_user))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            if (string.IsNullOrEmpty(faleConoscoOv.ds_assunto))
            {
                throw new DocValidacaoException("Assunto inválido.");
            }
            if (string.IsNullOrEmpty(faleConoscoOv.ds_email))
            {
                throw new DocValidacaoException("E-mail inválido.");
            }
            if (string.IsNullOrEmpty(faleConoscoOv.ds_msg))
            {
                throw new DocValidacaoException("Mensagem inválida.");
            }
        }
    }
}
