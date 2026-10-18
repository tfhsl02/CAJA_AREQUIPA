using System;
using System.Collections.Generic;
using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{
    public class ColaOperaciones
    {
        private NodoCola frente;
        private NodoCola final;
        private int cantidad;

        public ColaOperaciones()
        {
            frente = null;
            final = null;
            cantidad = 0;
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public void Encolar(Operacion operacion)
        {
            NodoCola nuevoNodo = new NodoCola(operacion);

            if (EstaVacia())
            {
                frente = nuevoNodo;
                final = nuevoNodo;
            }
            else
            {
                final.Siguiente = nuevoNodo;
                final = nuevoNodo;
            }

            cantidad = cantidad + 1;
        }

        public Operacion Desencolar()
        {
            if (EstaVacia())
            {
                throw new ReglaNegocioException("La cola de operaciones pendientes está vacía.");
            }

            Operacion operacionRetirada = frente.Dato;
            frente = frente.Siguiente;

            if (frente == null)
            {
                final = null;
            }

            cantidad = cantidad - 1;
            return operacionRetirada;
        }


        public List<Operacion> ObtenerLista()
        {
            List<Operacion> lista = new List<Operacion>();
            NodoCola actual = frente;

            while (actual != null)
            {
                lista.Add(actual.Dato);
                actual = actual.Siguiente;
            }

            return lista;
        }

        public bool EstaVacia()
        {
            return cantidad == 0;
        }

    }
}
