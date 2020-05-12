namespace TCDF.Sinj.OV
{
    public class InformacoesSobreVersaoOV
    {
        private uint id;
        private string rotulo;
        private string comentario;


        public InformacoesSobreVersaoOV()
        {
            Id = 0;
            Rotulo = "";
            Comentario = "";
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
