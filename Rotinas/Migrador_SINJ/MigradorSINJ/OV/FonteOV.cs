using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class FonteLBW
    {
        public string TipoPublicacao { get; set; }
        public string TipoFonte { get; set; }
        public string DataPublicacao { get; set; }

        public Guid Id { get; set; }
        public int? Pagina { get; set; }
        public int? Coluna { get; set; }
        public int IdNorma { get; set; }
        public bool Pesquisavel { get; set; }
        public string Observacao { get; set; }
        public string MotivoReduplicacao { get; set; }
        public string ConteudoArquivoTexto { get; set; }
        public string CaminhoArquivoTexto { get; set; }
        public string NomeArquivoTexto { get; set; }
        public string CaminhoDoArquivoTemporario { get; set; }
        public byte[] DadosDoArquivoTemporario;
        
        public TipoDeEdicaoEnum TipoEdicao { get; set; }
    }
    public class Fonte
    {
        public string ch_fonte { get; set; }

        //TipoDePublicacaoOV
        public string ch_tipo_publicacao { get; set; }
        public string nm_tipo_publicacao { get; set; }

        //TipoDeFonteOV
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }

        public TipoDeEdicaoEnum in_tipo_edicao { get; set; }
        public string nm_tipo_edicao { get { return util.BRLight.Util.GetEnumDescription(in_tipo_edicao); } }

        public string dt_publicacao { get; set; }
        public string nr_pagina { get; set; }
        public string nr_coluna { get; set; }
        public string ds_observacao_fonte { get; set; }
        public string ds_republicacao { get; set; }
        public ArquivoOV ar_fonte { get; set; }


        public Fonte()
        {
            ar_fonte = new ArquivoOV();
        }

    }
}
