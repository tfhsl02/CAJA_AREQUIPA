using System;
using System.Text.Json.Serialization;

namespace CajaArequipa.Modelos
{
    public class Operacion
    {
        public string CodigoOperacion { get; }
        public string TipoOperacion { get; }
        public decimal Monto { get; }
        public string CodigoAgencia { get; }
        public DateTime Fecha { get; }


        public string Estado { get; private set; }

        public Operacion(string codigoOperacion, string tipoOperacion, decimal monto, string codigoAgencia)
            : this(codigoOperacion, tipoOperacion, monto, codigoAgencia, DateTime.Now, EstadoOperacion.Pendiente)
        {
        }

        [JsonConstructor]
        public Operacion(string codigoOperacion, string tipoOperacion, decimal monto, string codigoAgencia, DateTime fecha, string estado)
        {
            if (string.IsNullOrWhiteSpace(codigoOperacion))
            {
                throw new ReglaNegocioException("El código de operación no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(tipoOperacion))
            {
                throw new ReglaNegocioException("El tipo de operación no puede estar vacío.");
            }

            if (monto <= 0)
            {
                throw new ReglaNegocioException("El monto debe ser mayor a 0.");
            }

            if (string.IsNullOrWhiteSpace(codigoAgencia))
            {
                throw new ReglaNegocioException("La operación debe pertenecer a una agencia.");
            }

            if (!EstadoOperacion.EsValido(estado))
            {
                throw new ReglaNegocioException("El estado de la operación no es válido: " + estado);
            }

            CodigoOperacion = codigoOperacion;
            TipoOperacion = tipoOperacion;
            Monto = monto;
            CodigoAgencia = codigoAgencia;
            Fecha = fecha;
            Estado = estado;
        }

        public void MarcarProcesada()
        {
            Estado = EstadoOperacion.Procesada;
        }

        public void MarcarRevertida()
        {
            Estado = EstadoOperacion.Revertida;
        }
    }
}
