namespace CajaArequipa.Helpers
{
    public class ItemLista
    {
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Detalle { get; set; }
        public string Etiqueta { get; set; }
        public Color ColorEtiqueta { get; set; } = Paleta.Acento;

        public bool TieneDetalle
        {
            get { return !string.IsNullOrEmpty(Detalle); }
        }

        public bool TieneEtiqueta
        {
            get { return !string.IsNullOrEmpty(Etiqueta); }
        }
    }
}
