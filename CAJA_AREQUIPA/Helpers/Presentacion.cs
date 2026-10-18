using System.Collections.Generic;
using CajaArequipa.Modelos;

namespace CajaArequipa.Helpers
{
    public static class Presentacion
    {
        public static ItemLista DeAgencia(Agencia agencia, bool tieneConexion)
        {
            return new ItemLista
            {
                Titulo = agencia.Nombre,
                Subtitulo = agencia.CodigoAgencia + "  ·  " + agencia.Direccion,
                Etiqueta = tieneConexion ? "Con conexión" : "Sin conexión",
                ColorEtiqueta = tieneConexion ? Paleta.Exito : Paleta.Aviso
            };
        }

        public static ItemLista DeConexion(Conexion conexion)
        {
            return new ItemLista
            {
                Titulo = conexion.AgenciaOrigen + "  ↔  " + conexion.AgenciaDestino,
                Subtitulo = "Tiempo de sincronización: " + InputHelper.FormatearSegundos(conexion.TiempoSincronizacion),
                Etiqueta = conexion.Disponible ? "Disponible" : "No disponible",
                ColorEtiqueta = conexion.Disponible ? Paleta.Exito : Paleta.Error
            };
        }

        public static ItemLista DeOperacion(Operacion operacion, int posicion)
        {
            string detalle = InputHelper.FormatearFecha(operacion.Fecha);

            if (posicion > 0)
            {
                detalle = "Posición " + posicion + " en la cola  ·  " + detalle;
            }

            return new ItemLista
            {
                Titulo = operacion.CodigoOperacion + "  ·  " + operacion.TipoOperacion,
                Subtitulo = InputHelper.FormatearMonto(operacion.Monto) + "  ·  Agencia " + operacion.CodigoAgencia,
                Detalle = detalle,
                Etiqueta = operacion.Estado,
                ColorEtiqueta = ColorDeEstado(operacion.Estado)
            };
        }

        private static Color ColorDeEstado(string estado)
        {
            if (estado == EstadoOperacion.Procesada)
            {
                return Paleta.Exito;
            }

            if (estado == EstadoOperacion.Revertida)
            {
                return Paleta.Error;
            }

            return Paleta.Aviso;
        }

        public static List<Conexion> UnaPorEnlace(List<Conexion> todas)
        {
            List<Conexion> unicas = new List<Conexion>();

            for (int i = 0; i < todas.Count; i++)
            {
                if (string.CompareOrdinal(todas[i].AgenciaOrigen, todas[i].AgenciaDestino) < 0)
                {
                    unicas.Add(todas[i]);
                }
            }

            return unicas;
        }
    }
}
