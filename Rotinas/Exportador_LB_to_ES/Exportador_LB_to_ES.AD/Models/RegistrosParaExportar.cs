using System;

namespace Exportador_LB_to_ES.AD.Models
{
    public class RegistrosParaExportar
    {
        public string Id { get; set; }
        public string CaminhoArquivoTexto { get; set; }
        public string Nome { get; set; }
    }

    public class RegistrosParaDeletar
    {
        public string Id { get; set; }
        public string DataDeletado { get; set; }
        public string Nome { get; set; }
    }
}
