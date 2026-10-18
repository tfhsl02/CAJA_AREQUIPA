using System.Collections.Generic;
using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{

    public class ListaCircular<T>
    {
        private class Nodo
        {
            public T Dato;
            public Nodo Siguiente;

            public Nodo(T dato)
            {
                Dato = dato;
                Siguiente = null;
            }
        }

        private Nodo ultimo;  
        private Nodo turno;    
        private int cantidad;

        public ListaCircular()
        {
            ultimo = null;
            turno = null;
            cantidad = 0;
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public bool EstaVacia()
        {
            return cantidad == 0;
        }

        public void Agregar(T dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (ultimo == null)
            {
                nuevo.Siguiente = nuevo;  
                ultimo = nuevo;
                turno = nuevo;
            }
            else
            {
                nuevo.Siguiente = ultimo.Siguiente;
                ultimo.Siguiente = nuevo;
                ultimo = nuevo;
            }

            cantidad = cantidad + 1;
        }


        public T SiguienteTurno()
        {
            if (turno == null)
            {
                throw new ReglaNegocioException("No hay agencias registradas para asignar turnos.");
            }

            T dato = turno.Dato;
            turno = turno.Siguiente;
            return dato;
        }

        public List<T> ObtenerLista()
        {
            List<T> lista = new List<T>();

            if (ultimo == null)
            {
                return lista;
            }

            Nodo actual = ultimo.Siguiente;

            for (int i = 0; i < cantidad; i++)
            {
                lista.Add(actual.Dato);
                actual = actual.Siguiente;
            }

            return lista;
        }
    }
}
