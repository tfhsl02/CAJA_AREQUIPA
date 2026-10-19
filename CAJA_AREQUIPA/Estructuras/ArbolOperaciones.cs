using System;
using System.Collections.Generic;
using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{

    public class ArbolOperaciones
    {
        private NodoArbol raiz;
        private int cantidad;
        private bool insertado;   

        public ArbolOperaciones()
        {
            raiz = null;
            cantidad = 0;
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public int Altura
        {
            get { return AlturaDe(raiz); }
        }

        public bool Insertar(Operacion operacion)
        {
            insertado = false;
            raiz = InsertarNodo(raiz, operacion);

            if (insertado)
            {
                cantidad = cantidad + 1;
            }

            return insertado;
        }

        private NodoArbol InsertarNodo(NodoArbol nodo, Operacion operacion)
        {
            if (nodo == null)
            {
                insertado = true;
                return new NodoArbol(operacion);
            }

            int comparacion = string.CompareOrdinal(operacion.CodigoOperacion, nodo.Dato.CodigoOperacion);

            if (comparacion < 0)
            {
                nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, operacion);
            }
            else if (comparacion > 0)
            {
                nodo.Derecho = InsertarNodo(nodo.Derecho, operacion);
            }
            else
            {
                return nodo;   
            }

            return Balancear(nodo);
        }

        // BALANCEO AVL
        private int AlturaDe(NodoArbol nodo)
        {
            if (nodo == null)
            {
                return 0;
            }

            return nodo.Altura;
        }

        private void ActualizarAltura(NodoArbol nodo)
        {
            nodo.Altura = 1 + Math.Max(AlturaDe(nodo.Izquierdo), AlturaDe(nodo.Derecho));
        }

        private int FactorBalance(NodoArbol nodo)
        {
            if (nodo == null)
            {
                return 0;
            }

            return AlturaDe(nodo.Izquierdo) - AlturaDe(nodo.Derecho);
        }

        private NodoArbol RotarDerecha(NodoArbol y)
        {
            NodoArbol x = y.Izquierdo;
            NodoArbol subarbol = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = subarbol;

            ActualizarAltura(y);
            ActualizarAltura(x);
            return x;
        }

        private NodoArbol RotarIzquierda(NodoArbol x)
        {
            NodoArbol y = x.Derecho;
            NodoArbol subarbol = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = subarbol;

            ActualizarAltura(x);
            ActualizarAltura(y);
            return y;
        }

        private NodoArbol Balancear(NodoArbol nodo)
        {
            ActualizarAltura(nodo);
            int factor = FactorBalance(nodo);

            if (factor > 1)
            {
                if (FactorBalance(nodo.Izquierdo) < 0)
                {
                    nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                }

                return RotarDerecha(nodo);
            }

            if (factor < -1)
            {
                if (FactorBalance(nodo.Derecho) > 0)
                {
                    nodo.Derecho = RotarDerecha(nodo.Derecho);
                }

                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        // BÚSQUEDA
        public Operacion Buscar(string codigoOperacion)
        {
            NodoArbol actual = raiz;

            while (actual != null)
            {
                int comparacion = string.CompareOrdinal(codigoOperacion, actual.Dato.CodigoOperacion);

                if (comparacion == 0)
                {
                    return actual.Dato;
                }
                else if (comparacion < 0)
                {
                    actual = actual.Izquierdo;
                }
                else
                {
                    actual = actual.Derecho;
                }
            }

            return null;
        }

        // RECORRIDOS 
        public List<Operacion> RecorridoInOrden()
        {
            List<Operacion> lista = new List<Operacion>();
            RecorrerInOrden(raiz, lista);
            return lista;
        }

        private void RecorrerInOrden(NodoArbol nodo, List<Operacion> lista)
        {
            if (nodo == null)
            {
                return;
            }

            RecorrerInOrden(nodo.Izquierdo, lista);
            lista.Add(nodo.Dato);
            RecorrerInOrden(nodo.Derecho, lista);
        }

        public List<Operacion> RecorridoPreOrden()
        {
            List<Operacion> lista = new List<Operacion>();
            RecorrerPreOrden(raiz, lista);
            return lista;
        }

        private void RecorrerPreOrden(NodoArbol nodo, List<Operacion> lista)
        {
            if (nodo == null)
            {
                return;
            }

            lista.Add(nodo.Dato);
            RecorrerPreOrden(nodo.Izquierdo, lista);
            RecorrerPreOrden(nodo.Derecho, lista);
        }

        public List<Operacion> RecorridoPostOrden()
        {
            List<Operacion> lista = new List<Operacion>();
            RecorrerPostOrden(raiz, lista);
            return lista;
        }

        private void RecorrerPostOrden(NodoArbol nodo, List<Operacion> lista)
        {
            if (nodo == null)
            {
                return;
            }

            RecorrerPostOrden(nodo.Izquierdo, lista);
            RecorrerPostOrden(nodo.Derecho, lista);
            lista.Add(nodo.Dato);
        }
    }
}
