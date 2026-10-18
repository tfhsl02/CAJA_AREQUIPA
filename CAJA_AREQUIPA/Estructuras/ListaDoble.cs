using System;
using System.Collections.Generic;

namespace CajaArequipa.Estructuras
{
    public class ListaDoble<T>
    {
        private class Nodo
        {
            public T Dato;
            public Nodo Siguiente;
            public Nodo Anterior;

            public Nodo(T dato)
            {
                Dato = dato;
                Siguiente = null;
                Anterior = null;
            }
        }

        private Nodo cabeza;
        private Nodo cola;
        private int cantidad;

        public ListaDoble()
        {
            cabeza = null;
            cola = null;
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

            if (cabeza == null)
            {
                cabeza = nuevo;
                cola = nuevo;
            }
            else
            {
                nuevo.Siguiente = cabeza;
                cabeza.Anterior = nuevo;
                cabeza = nuevo;
            }

            cantidad = cantidad + 1;
        }

        public void AgregarAlFinal(T dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (cola == null)
            {
                cabeza = nuevo;
                cola = nuevo;
            }
            else
            {
                nuevo.Anterior = cola;
                cola.Siguiente = nuevo;
                cola = nuevo;
            }

            cantidad = cantidad + 1;
        }

        public bool Eliminar(Func<T, bool> condicion)
        {
            Nodo actual = cabeza;

            while (actual != null)
            {
                if (condicion(actual.Dato))
                {
                    if (actual.Anterior == null)
                    {
                        cabeza = actual.Siguiente;
                    }
                    else
                    {
                        actual.Anterior.Siguiente = actual.Siguiente;
                    }

                    if (actual.Siguiente == null)
                    {
                        cola = actual.Anterior;
                    }
                    else
                    {
                        actual.Siguiente.Anterior = actual.Anterior;
                    }

                    cantidad = cantidad - 1;
                    return true;
                }

                actual = actual.Siguiente;
            }

            return false;
        }

        public List<T> ObtenerAdelante()
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

        public List<T> ObtenerAtras()
        {
            List<T> lista = new List<T>();
            Nodo actual = cola;

            while (actual != null)
            {
                lista.Add(actual.Dato);
                actual = actual.Anterior;
            }

            return lista;
        }
    }
}
