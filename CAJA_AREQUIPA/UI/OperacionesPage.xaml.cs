using System;
using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Modelos;
using CajaArequipa.Servicios;

namespace CajaArequipa.UI
{
    public partial class OperacionesPage : ContentView, IVistaRefrescable
    {
        private static readonly string[] tiposOperacion = { "ABONO", "RETIRO", "TRANSFERENCIA" };

        private ProcesadorOperaciones procesador;
        private List<Agencia> agencias = new List<Agencia>();

        public OperacionesPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();
            this.procesador = procesador;

            pkTipo.ItemsSource = new List<string>(tiposOperacion);
            pkTipo.SelectedIndex = 0;
        }

        public void AlMostrar()
        {
            CargarAgencias();
            RefrescarLista();
        }

        private void CargarAgencias()
        {
            int seleccion = pkAgencia.SelectedIndex;
            agencias = procesador.ObtenerAgencias();

            List<string> nombres = new List<string>();
            for (int i = 0; i < agencias.Count; i++)
            {
                nombres.Add(agencias[i].CodigoAgencia + " - " + agencias[i].Nombre);
            }

            pkAgencia.ItemsSource = nombres;
            pkAgencia.SelectedIndex = seleccion < nombres.Count ? seleccion : -1;
        }

        // Registrar operación (queda PENDIENTE y entra a la cola).
        private void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            string codigo;
            decimal monto;
            string error;

            if (!InputHelper.TryLeerCodigo(entCodigo.Text, "El código de operación", 2, 20, out codigo, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            if (pkTipo.SelectedIndex < 0)
            {
                MostrarMensaje("Seleccione el tipo de operación.", false);
                return;
            }

            if (!InputHelper.TryLeerDecimal(entMonto.Text, "El monto", 0.01m, 999999.99m, 2, out monto, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            if (pkAgencia.SelectedIndex < 0)
            {
                MostrarMensaje("Seleccione la agencia. Si no hay ninguna, regístrela primero en la sección Agencias.", false);
                return;
            }

            string tipo = tiposOperacion[pkTipo.SelectedIndex];
            string codigoAgencia = agencias[pkAgencia.SelectedIndex].CodigoAgencia;

            try
            {
                procesador.RegistrarOperacion(codigo, tipo, monto, codigoAgencia);
                MostrarMensaje("Operación registrada correctamente con estado PENDIENTE.", true);

                entCodigo.Text = "";
                entMonto.Text = "";
                RefrescarLista();
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo registrar la operación: " + ex.Message, false);
            }
        }

        // Procesar siguiente operación (sale de la cola y entra a la pila de reversiones).
        private void BtnProcesar_Clicked(object sender, EventArgs e)
        {
            try
            {
                Operacion operacion = procesador.ProcesarSiguienteOperacion();
                MostrarMensaje("Operación procesada correctamente:", true);
                lblDetalle.Text = InputHelper.DescribirOperacion(operacion);
                RefrescarLista();
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo procesar la operación: " + ex.Message, false);
            }
        }

        // Revertir última operación (sale de la cima de la pila).
        private void BtnRevertir_Clicked(object sender, EventArgs e)
        {
            try
            {
                Operacion operacion = procesador.RevertirUltimaOperacion();
                MostrarMensaje("Operación revertida correctamente:", true);
                lblDetalle.Text = InputHelper.DescribirOperacion(operacion);
                RefrescarLista();
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo revertir la operación: " + ex.Message, false);
            }
        }

        // Mostrar operaciones pendientes.
        private void RefrescarLista()
        {
            List<ItemLista> items = new List<ItemLista>();
            List<Operacion> pendientes = procesador.ObtenerOperacionesPendientes();

            for (int i = 0; i < pendientes.Count; i++)
            {
                items.Add(Presentacion.DeOperacion(pendientes[i], i + 1));
            }

            lstPendientes.ItemsSource = items;
            lstPendientes.IsVisible = items.Count > 0;
            vacio.IsVisible = items.Count == 0;
            lblPendientes.Text = pendientes.Count.ToString();
            lblRevertibles.Text = procesador.ObtenerPilaReversiones().Count.ToString();
        }

        private void MostrarMensaje(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.TextColor = correcto ? Paleta.Exito : Paleta.Error;
            lblDetalle.Text = "";
        }
    }
}
