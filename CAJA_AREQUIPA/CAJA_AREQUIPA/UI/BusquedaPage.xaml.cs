using System;
using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Modelos;
using CajaArequipa.Servicios;

namespace CajaArequipa.UI
{
    public partial class BusquedaPage : ContentView, IVistaRefrescable
    {
        private ProcesadorOperaciones procesador;

        public BusquedaPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();
            this.procesador = procesador;
        }

        public void AlMostrar()
        {
            RefrescarArbol();
            RefrescarBitacora();
        }

        // Buscar operación (búsqueda en el árbol binario por código).
        private void BtnBuscar_Clicked(object sender, EventArgs e)
        {
            string codigo;
            string error;

            lblDetalle.Text = "";

            if (!InputHelper.TryLeerCodigo(entCodigo.Text, "El código de operación", 1, 20, out codigo, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            Operacion operacion = procesador.BuscarOperacion(codigo);

            if (operacion == null)
            {
                MostrarMensaje("No se encontró ninguna operación con ese código.", false);
                return;
            }

            MostrarMensaje("Operación encontrada:", true);
            lblDetalle.Text = InputHelper.DescribirOperacion(operacion);
        }

        // Árbol de operaciones (recorrido en orden).
        private void RefrescarArbol()
        {
            List<ItemLista> items = new List<ItemLista>();
            List<Operacion> operaciones = procesador.ObtenerArbolOperaciones();

            for (int i = 0; i < operaciones.Count; i++)
            {
                items.Add(Presentacion.DeOperacion(operaciones[i], 0));
            }

            lstArbol.ItemsSource = items;
            lstArbol.IsVisible = items.Count > 0;
            vacioArbol.IsVisible = items.Count == 0;
            lblTotal.Text = operaciones.Count.ToString();
            lblAltura.Text = procesador.ObtenerAlturaArbol().ToString();
        }

        // Bitácora de procesadas: recorrido hacia atrás de la lista doble.
        private void RefrescarBitacora()
        {
            List<ItemLista> items = new List<ItemLista>();
            List<Operacion> procesadas = procesador.ObtenerBitacoraProcesadas();

            for (int i = 0; i < procesadas.Count; i++)
            {
                items.Add(Presentacion.DeOperacion(procesadas[i], 0));
            }

            lstBitacora.ItemsSource = items;
            lstBitacora.IsVisible = items.Count > 0;
            vacioBitacora.IsVisible = items.Count == 0;
        }

        private void MostrarMensaje(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.TextColor = correcto ? Paleta.Exito : Paleta.Error;
        }
    }
}
