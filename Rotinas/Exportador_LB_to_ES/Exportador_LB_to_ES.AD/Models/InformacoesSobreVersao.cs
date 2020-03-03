namespace Exportador_LB_to_ES.AD.Models
{
    public class InformacoesSobreVersao
    {
        private uint id;
        private string rotulo;
        private string comentario;


        public InformacoesSobreVersao()
        {
            this.Id = 0;
            this.Rotulo = "";
            this.Comentario = "";
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Rotulo
        {
            get { return rotulo; }
            set { rotulo = value; }
        }

        public string Comentario
        {
            get { return comentario; }
            set { comentario = value; }
        }
    }
}
