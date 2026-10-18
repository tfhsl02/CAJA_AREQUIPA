using System;
using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Modelos;
using CajaArequipa.Servicios;

namespace CajaArequipa.UI
{
    public partial class ConexionesPage : ContentView, IVistaRefrescable
    {
        private ProcesadorOperaciones procesador;
        private List<Agencia> agencias = new List<Agencia>();

        public ConexionesPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();
            this.procesador = procesador;
        }

        public void AlMostrar()
        {
            CargarAgencias();
            RefrescarLista();
        }

        private void CargarAgencias()
        {
            int origenAnterior = pkOrigen.SelectedIndex;
            int destinoAnterior = pkDestino.SelectedIndex;
            agencias = procesador.ObtenerAgencias();

            List<string> nombres = new List<string>();
            for (int i = 0; i < agencias.Count; i++)
            {
                nombres.Add(agencias[i].CodigoAgencia + " - " + agencias[i].Nombre);
            }

            pkOrigen.ItemsSource = nombres;
            pkDestino.ItemsSource = new List<string>(nombres);
            pkOrigen.SelectedIndex = origenAnterior < nombres.Count ? origenAnterior : -1;
            pkDestino.SelectedIndex = destinoAnterior < nombres.Count ? destinoAnterior : -1;
        }

        // Registrar conexión entre agencias.
        private void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (pkOrigen.SelectedIndex < 0)
            {
                MostrarMensaje("Seleccione la agencia de origen.", false);
                return;
            }

            if (pkDestino.SelectedIndex < 0)
            {
                MostrarMensaje("Seleccione la agencia de destino.", false);
                return;
            }

            double tiempo;
            string error;

            if (!InputHelper.TryLeerDouble(entTiempo.Text, "El tiempo de sincronización", 0.01, 86400, 2, out tiempo, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            string origen = agencias[pkOrigen.SelectedIndex].CodigoAgencia;
            string destino = agencias[pkDestino.SelectedIndex].CodigoAgencia;

            try
            {
                procesador.RegistrarConexion(origen, destino, tiempo);
                MostrarMensaje("Conexión registrada correctamente.", true);

                entTiempo.Text = "";
                RefrescarLista();
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo registrar la conexión: " + ex.Message, false);
            }
        }

        private void BtnActivar_Clicked(object sender, EventArgs e)
        {
            CambiarDisponibilidad(true);
        }

        private void BtnDesactivar_Clicked(object sender, EventArgs e)
        {
            CambiarDisponibilidad(false);
        }

        // Simula que un enlace entre agencias se cae o se recupera (conectividad intermitente).
        private void CambiarDisponibilidad(bool disponible)
        {
            if (pkOrigen.SelectedIndex < 0 || pkDestino.SelectedIndex < 0)
            {
                MostrarMensaje("Seleccione la agencia de origen y la de destino del enlace.", false);
                return;
            }

            string origen = agencias[pkOrigen.SelectedIndex].CodigoAgencia;
            string destino = agencias[pkDestino.SelectedIndex].CodigoAgencia;

            try
            {
                procesador.CambiarDisponibilidadConexion(origen, destino, disponible);
                MostrarMensaje("El enlace " + origen + " ↔ " + destino + " quedó " + (disponible ? "disponible." : "no disponible."), true);
                RefrescarLista();
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, false);
            }
        }

        // Mostrar conexiones (un solo renglón por enlace).
        private void RefrescarLista()
        {
            List<Conexion> enlaces = Presentacion.UnaPorEnlace(procesador.ObtenerConexiones());
            List<ItemLista> items = new List<ItemLista>();
            int disponibles = 0;

            for (int i = 0; i < enlaces.Count; i++)
            {
                if (enlaces[i].Disponible)
                {
                    disponibles = disponibles + 1;
                }

                items.Add(Presentacion.DeConexion(enlaces[i]));
            }

            lstConexiones.ItemsSource = items;
            lstConexiones.IsVisible = items.Count > 0;
            vacio.IsVisible = items.Count == 0;
            lblTotal.Text = enlaces.Count.ToString();
            lblDisponibles.Text = disponibles.ToString();
        }

        private void MostrarMensaje(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.TextColor = correcto ? Paleta.Exito : Paleta.Error;
        }
    }
}
