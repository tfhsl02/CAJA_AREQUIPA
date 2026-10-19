using System;
using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Modelos;
using CajaArequipa.Servicios;

namespace CajaArequipa.UI
{
    public partial class RutasPage : ContentView, IVistaRefrescable
    {
        private ProcesadorOperaciones procesador;
        private List<Agencia> agencias = new List<Agencia>();

        public RutasPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();
            this.procesador = procesador;
        }

        public void AlMostrar()
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

        private void BtnCalcular_Clicked(object sender, EventArgs e)
        {
            lblDetalle.Text = "";

            if (pkOrigen.SelectedIndex < 0 || pkDestino.SelectedIndex < 0)
            {
                MostrarMensaje("Seleccione la agencia de origen y la de destino.", false);
                return;
            }

            if (pkOrigen.SelectedIndex == pkDestino.SelectedIndex)
            {
                MostrarMensaje("El origen y el destino deben ser agencias distintas.", false);
                return;
            }

            string origen = agencias[pkOrigen.SelectedIndex].CodigoAgencia;
            string destino = agencias[pkDestino.SelectedIndex].CodigoAgencia;

            RutaSincronizacion ruta = procesador.CalcularRutaMinima(origen, destino);

            if (!ruta.Existe)
            {
                MostrarMensaje("No existe una ruta con enlaces disponibles entre " + origen + " y " + destino + ".", false);
                return;
            }

            MostrarMensaje("Ruta mínima encontrada:", true);
            lblDetalle.Text = InputHelper.DescribirRuta(ruta);
        }

        private void BtnTurno_Clicked(object sender, EventArgs e)
        {
            try
            {
                Agencia agencia = procesador.AvanzarTurnoSincronizacion();
                lblTurno.Text = "Turno de sincronización: " + agencia.CodigoAgencia + " - " + agencia.Nombre;
                lblTurno.TextColor = Paleta.Exito;
            }
            catch (Exception ex)
            {
                lblTurno.Text = ex.Message;
                lblTurno.TextColor = Paleta.Error;
            }
        }

        private void MostrarMensaje(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.TextColor = correcto ? Paleta.Exito : Paleta.Error;
        }
    }
}
