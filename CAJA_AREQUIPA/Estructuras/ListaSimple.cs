using System;
using System.Collections.Generic;

namespace CajaArequipa.Estructuras
{

    public class ListaSimple<T>
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

        private Nodo cabeza;
        private Nodo ultimo;
        private int cantidad;

        public ListaSimple()
        {
            cabeza = null;
            ultimo = null;
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

        public void AgregarAlInicio(T dato)
        {
            Nodo nuevo = new Nodo(dato);
            nuevo.Siguiente = cabeza;
            cabeza = nuevo;

            if (ultimo == null)
            {
                ultimo = nuevo;
            }

            cantidad = cantidad + 1;
        }

        public void AgregarAlFinal(T dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else
            {
                ultimo.Siguiente = nuevo;
            }

            ultimo = nuevo;
            cantidad = cantidad + 1;
        }

        public T BuscarPrimero(Func<T, bool> condicion)
        {
            Nodo actual = cabeza;

            while (actual != null)
            {
                if (condicion(actual.Dato))
                {
                    return actual.Dato;
                }

                actual = actual.Siguiente;
            }

            return default(T);
        }

        public bool Eliminar(Func<T, bool> condicion)
        {
            Nodo anterior = null;
            Nodo actual = cabeza;

            while (actual != null)
            {
                if (condicion(actual.Dato))
                {
                    if (anterior == null)
                    {
                        cabeza = actual.Siguiente;
                    }
                    else
                    {
                        anterior.Siguiente = actual.Siguiente;
                    }

                    if (actual == ultimo)
                    {
                        ultimo = anterior;
                    }

                    cantidad = cantidad - 1;
                    return true;
                }

                anterior = actual;
                actual = actual.Siguiente;
            }

            return false;
        }

        public List<T> ObtenerLista()
        {
            List<T> lista = new List<T>();
            Nodo actual = cabeza;

            while (actual != null)
            {
                lista.Add(actual.Dato);
                actual = actual.Siguiente;
            }

            return lista;
        }
    }
}
