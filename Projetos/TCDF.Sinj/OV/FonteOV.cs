using System;
using System.Data;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class Fonte
    {
        public string ch_fonte { get; set; }

        //TipoDePublicacaoOV
        public string ch_tipo_publicacao { get; set; }
        public string nm_tipo_publicacao { get; set; }

        //TipoDeFonteOV
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }

        //public TipoDeEdicaoEnum in_tipo_edicao { get; set; }
        //public string nm_tipo_edicao { get { return util.BRLight.Util.GetEnumDescription(in_tipo_edicao); }  }

        //public string ch_tipo_edicao { get; set; }
        //public string nm_tipo_edicao { get; set; }

        public string dt_publicacao { get; set; }
        //public string secao { get; set; }
        public string nr_pagina { get; set; }
        public string nr_coluna { get; set; }
        public string ds_observacao_fonte { get; set; }
        public string ds_republicacao { get; set; }
        public ArquivoOV ar_fonte { get; set; }
        
        //Representa a descri��o do di�rio (DODF 155 Edi��o Extra Suplemento 3 Se��o 1 de 19/02/2001)
        public string ds_diario { get; set; }
        //Representa o json file do di�rio
        public ArquivoOV ar_diario { get; set; }
        
        public Fonte()
        {
            ar_diario = new ArquivoOV();
            ar_fonte = new ArquivoOV();
        }

    }
}