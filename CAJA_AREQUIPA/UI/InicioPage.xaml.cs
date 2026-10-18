using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Servicios;

namespace CajaArequipa.UI
{
    public partial class InicioPage : ContentView, IVistaRefrescable
    {
        private ProcesadorOperaciones procesador;

        public InicioPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();
            this.procesador = procesador;
        }

        public void AlMostrar()
        {
            lblAgencias.Text = procesador.ObtenerAgencias().Count.ToString();
            lblConexiones.Text = Presentacion.UnaPorEnlace(procesador.ObtenerConexiones()).Count.ToString();
            lblPendientes.Text = procesador.ObtenerOperacionesPendientes().Count.ToString();
            lblProcesadas.Text = procesador.ObtenerPilaReversiones().Count.ToString();

            List<string> avisos = procesador.Advertencias;

            if (avisos.Count == 0)
            {
                lblEstado.Text = "Todo en orden: los datos se cargaron correctamente.";
                lblEstado.TextColor = Paleta.Exito;
            }
            else
            {
                string texto = "";
                for (int i = 0; i < avisos.Count; i++)
                {
                    texto = texto + (i > 0 ? "\n" : "") + "⚠ " + avisos[i];
                }

                lblEstado.Text = texto;
                lblEstado.TextColor = Paleta.Aviso;
            }

            lblCarpeta.Text = "Archivos JSON en: " + MauiProgram.CarpetaDatos;
        }
    }
}
