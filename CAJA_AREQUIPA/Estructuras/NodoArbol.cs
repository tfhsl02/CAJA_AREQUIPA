using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{
    public class NodoArbol
    {
        private Operacion dato;
        private NodoArbol izquierdo;
        private NodoArbol derecho;
        private int altura;

        public NodoArbol(Operacion dato)
        {
            this.dato = dato;
            izquierdo = null;
            derecho = null;
            altura = 1;   
        }

        public Operacion Dato
        {
            get { return dato; }
            set { dato = value; }
        }

        public NodoArbol Izquierdo
        {
            get { return izquierdo; }
            set { izquierdo = value; }
        }

        public NodoArbol Derecho
        {
            get { return derecho; }
            set { derecho = value; }
        }

        public int Altura
        {
            get { return altura; }
            set { altura = value; }
        }
    }
}
