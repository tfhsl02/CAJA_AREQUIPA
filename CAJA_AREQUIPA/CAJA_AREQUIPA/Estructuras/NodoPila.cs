using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{
    public class NodoPila
    {
        private Operacion dato;
        private NodoPila siguiente;

        public NodoPila(Operacion dato)
        {
            this.dato = dato;
            siguiente = null;
        }

        public Operacion Dato
        {
            get { return dato; }
            set { dato = value; }
        }

        public NodoPila Siguiente
        {
            get { return siguiente; }
            set { siguiente = value; }
        }
    }
}
