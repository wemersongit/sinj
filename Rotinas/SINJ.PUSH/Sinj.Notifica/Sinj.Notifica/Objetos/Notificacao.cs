namespace Sinj.Notifica.Objetos
{
    public class Notificacao
    {
        public int IdUsuario { get; set; }
        public int IdNorma { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Tentativa { get; set; }
    }
}