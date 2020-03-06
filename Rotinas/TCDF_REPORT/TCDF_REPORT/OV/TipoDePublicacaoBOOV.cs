namespace TCDF_REPORT.OV
{
    public class TipoDePublicacaoBOOV
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public TipoDePublicacaoBOOV()
        {
        }

        public TipoDePublicacaoBOOV(string nome)
        {
            Nome = nome;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Nome) ? "Não tem definido." : Nome;
        }
    }
}