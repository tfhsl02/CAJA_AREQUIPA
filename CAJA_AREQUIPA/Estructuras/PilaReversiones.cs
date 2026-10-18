using System;
using System.Collections.Generic;
using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{
    public class PilaReversiones
    {
        private NodoPila cima;
        private int cantidad;

        public PilaReversiones()
        {
            cima = null;
            cantidad = 0;
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public void Apilar(Operacion operacion)
        {
            NodoPila nuevoNodo = new NodoPila(operacion);
            nuevoNodo.Siguiente = cima;
            cima = nuevoNodo;
            cantidad = cantidad + 1;
        }

        public Operacion Desapilar()
        {
            if (EstaVacia())
            {
                throw new ReglaNegocioException("No hay operaciones para revertir.");
            }

            Operacion operacionRetirada = cima.Dato;
            cima = cima.Siguiente;
            cantidad = cantidad - 1;
            return operacionRetirada;
        }

        public List<Operacion> ObtenerLista()
        {
            List<Operacion> lista = new List<Operacion>();
            NodoPila actual = cima;

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
