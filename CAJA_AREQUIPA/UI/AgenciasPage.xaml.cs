using System;
using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Modelos;
using CajaArequipa.Servicios;

namespace CajaArequipa.UI
{
    public partial class AgenciasPage : ContentView, IVistaRefrescable
    {
        private ProcesadorOperaciones procesador;

        public AgenciasPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();
            this.procesador = procesador;
        }

        public void AlMostrar()
        {
            RefrescarLista();
        }

        // Registrar agencia.
        private void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            string codigo;
            string nombre;
            string direccion;
            string error;

            if (!InputHelper.TryLeerCodigo(entCodigo.Text, "El código de la agencia", 2, 10, out codigo, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            if (!InputHelper.TryLeerTexto(entNombre.Text, "El nombre", 2, 60, out nombre, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            if (!InputHelper.TryLeerTexto(entDireccion.Text, "La dirección", 3, 80, out direccion, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            try
            {
                procesador.RegistrarAgencia(codigo, nombre, direccion);
                MostrarMensaje("Agencia registrada correctamente.", true);

                entCodigo.Text = "";
                entNombre.Text = "";
                entDireccion.Text = "";
                RefrescarLista();
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo registrar la agencia: " + ex.Message, false);
            }
        }

        // Mostrar agencias. "Activas" = agencias que tienen al menos una conexión.
        private void RefrescarLista()
        {
            List<ItemLista> items = new List<ItemLista>();
            List<Agencia> agencias = procesador.ObtenerAgencias();
            int activas = 0;

            for (int i = 0; i < agencias.Count; i++)
            {
                bool tieneConexion = procesador.AgenciaTieneConexiones(agencias[i].CodigoAgencia);

                if (tieneConexion)
                {
                    activas = activas + 1;
                }

                items.Add(Presentacion.DeAgencia(agencias[i], tieneConexion));
            }

            lstAgencias.ItemsSource = items;
            lstAgencias.IsVisible = items.Count > 0;
            vacio.IsVisible = items.Count == 0;
            lblTotal.Text = agencias.Count.ToString();
            lblActivas.Text = activas.ToString();

            List<string> avisos = procesador.Advertencias;
            lblAvisos.IsVisible = avisos.Count > 0;
            lblAvisos.TextColor = Paleta.Aviso;
            lblAvisos.Text = avisos.Count > 0 ? "⚠ " + string.Join("\n⚠ ", avisos) : "";
        }

        private void MostrarMensaje(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.TextColor = correcto ? Paleta.Exito : Paleta.Error;
        }
    }
}
