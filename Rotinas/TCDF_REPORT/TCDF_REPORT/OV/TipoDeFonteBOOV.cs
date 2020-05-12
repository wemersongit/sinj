namespace TCDF_REPORT.OV
{
    public class TipoDeFonteBOOV
    {
        public long Id { get; set; }
        public string Nome { get; set; }

        public TipoDeFonteBOOV()
        {
        }

        public TipoDeFonteBOOV(string nome)
        {
            Nome = nome;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Nome) ? "Não tem definido." : Nome;
        }
    }
}