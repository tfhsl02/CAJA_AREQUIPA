using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{
    public class NodoCola
    {
        private Operacion dato;
        private NodoCola siguiente;

        public NodoCola(Operacion dato)
        {
            this.dato = dato;
            siguiente = null;
        }

        public Operacion Dato
        {
            get { return dato; }
            set { dato = value; }
        }

        public NodoCola Siguiente
        {
            get { return siguiente; }
            set { siguiente = value; }
        }
    }
}
