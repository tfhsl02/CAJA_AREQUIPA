using System;
using System.Globalization;
using System.Text;
using CajaArequipa.Modelos;

namespace CajaArequipa.Helpers
{
    public enum TipoEntrada
    {
        Texto,     
        Codigo,    
        Entero,    
        Decimal     
    }
    public static class InputHelper
    {
        private static readonly CultureInfo cultura = CultureInfo.InvariantCulture;
        public static string Filtrar(string texto, TipoEntrada tipo, int maxLongitud, int maxDecimales)
        {
            if (texto == null)
            {
                return "";
            }

            switch (tipo)
            {
                case TipoEntrada.Entero:
                    return FiltrarEntero(texto, maxLongitud);

                case TipoEntrada.Decimal:
                    return FiltrarDecimal(texto, maxLongitud, maxDecimales);

                case TipoEntrada.Codigo:
                    return FiltrarCodigo(texto, maxLongitud);

                default:
                    return FiltrarTexto(texto, maxLongitud);
            }
        }

        private static string FiltrarEntero(string texto, int maxLongitud)
        {
            StringBuilder resultado = new StringBuilder();

            for (int i = 0; i < texto.Length; i++)
            {
                if (EsDigito(texto[i]) && resultado.Length < maxLongitud)
                {
                    resultado.Append(texto[i]);
                }
            }

            string s = resultado.ToString();
            int prueba;
            while (s.Length > 0 && !int.TryParse(s, NumberStyles.None, cultura, out prueba))
            {
                s = s.Substring(0, s.Length - 1);
            }

            return s;
        }

        private static string FiltrarDecimal(string texto, int maxEnteros, int maxDecimales)
        {
            StringBuilder resultado = new StringBuilder();
            bool hayPunto = false;
            int enteros = 0;
            int decimales = 0;

            for (int i = 0; i < texto.Length; i++)
            {
                char c = texto[i];

                if (c == ',')
                {
                    c = '.'; 
                }

                if (EsDigito(c))
                {
                    if (hayPunto)
                    {
                        if (decimales >= maxDecimales) continue;
                        decimales++;
                    }
                    else
                    {
                        if (enteros >= maxEnteros) continue;
                        enteros++;
                    }

                    resultado.Append(c);
                }
                else if (c == '.' && !hayPunto && maxDecimales > 0)
                {
                    hayPunto = true;
                    resultado.Append('.');
                }
            }

            return resultado.ToString();
        }

        private static string FiltrarCodigo(string texto, int maxLongitud)
        {
            StringBuilder resultado = new StringBuilder();
            string mayusculas = texto.ToUpperInvariant();

            for (int i = 0; i < mayusculas.Length; i++)
            {
                char c = mayusculas[i];
                bool permitido = (c >= 'A' && c <= 'Z') || EsDigito(c) || c == '-' || c == '_';

                if (permitido && resultado.Length < maxLongitud)
                {
                    resultado.Append(c);
                }
            }

            return resultado.ToString();
        }

        private static string FiltrarTexto(string texto, int maxLongitud)
        {
            StringBuilder resultado = new StringBuilder();

            for (int i = 0; i < texto.Length; i++)
            {
                if (!char.IsControl(texto[i]) && resultado.Length < maxLongitud)
                {
                    resultado.Append(texto[i]);
                }
            }

            return resultado.ToString();
        }

        public static bool TryLeerTexto(string texto, string campo, int minLongitud, int maxLongitud,
                                        out string valor, out string error)
        {
            valor = (texto ?? "").Trim();
            error = null;

            if (valor.Length == 0)
            {
                error = campo + " es obligatorio.";
                return false;
            }

            if (valor.Length < minLongitud)
            {
                error = campo + " debe tener al menos " + minLongitud + " caracteres.";
                return false;
            }

            if (valor.Length > maxLongitud)
            {
                error = campo + " admite como máximo " + maxLongitud + " caracteres.";
                return false;
            }

            for (int i = 0; i < valor.Length; i++)
            {
                if (char.IsControl(valor[i]))
                {
                    error = campo + " contiene caracteres no permitidos.";
                    return false;
                }
            }

            return true;
        }

        public static bool TryLeerCodigo(string texto, string campo, int minLongitud, int maxLongitud,
                                         out string valor, out string error)
        {
            string limpio = (texto ?? "").Trim().ToUpperInvariant();
            valor = limpio;

            if (!TryLeerTexto(limpio, campo, minLongitud, maxLongitud, out valor, out error))
            {
                return false;
            }

            for (int i = 0; i < valor.Length; i++)
            {
                char c = valor[i];
                bool permitido = (c >= 'A' && c <= 'Z') || EsDigito(c) || c == '-' || c == '_';

                if (!permitido)
                {
                    error = campo + " solo puede tener letras, números, guion (-) y guion bajo (_).";
                    return false;
                }
            }

            return true;
        }

        public static bool TryLeerEntero(string texto, string campo, int minimo, int maximo,
                                         out int valor, out string error)
        {
            valor = 0;
            error = null;
            string t = (texto ?? "").Trim();

            if (t.Length == 0)
            {
                error = campo + " es obligatorio.";
                return false;
            }

            if (!int.TryParse(t, NumberStyles.AllowLeadingSign, cultura, out valor))
            {
                error = campo + " debe ser un número entero.";
                return false;
            }

            if (valor < minimo || valor > maximo)
            {
                error = campo + " debe estar entre " + FormatearEntero(minimo) + " y " + FormatearEntero(maximo) + ".";
                return false;
            }

            return true;
        }

        public static bool TryLeerDecimal(string texto, string campo, decimal minimo, decimal maximo,
                                          int maxDecimales, out decimal valor, out string error)
        {
            valor = 0;
            error = null;
            string t = (texto ?? "").Trim().Replace(',', '.');

            if (t.Length == 0)
            {
                error = campo + " es obligatorio.";
                return false;
            }

            if (!decimal.TryParse(t, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, cultura, out valor))
            {
                error = campo + " debe ser un número (ejemplo: 150.50).";
                return false;
            }

            if (decimal.Round(valor, maxDecimales) != valor)
            {
                error = campo + " admite como máximo " + maxDecimales + " decimales.";
                return false;
            }

            if (valor < minimo || valor > maximo)
            {
                error = campo + " debe estar entre " + minimo.ToString("0.##", cultura) +
                        " y " + maximo.ToString("0.##", cultura) + ".";
                return false;
            }

            return true;
        }

        public static bool TryLeerDouble(string texto, string campo, double minimo, double maximo,
                                         int maxDecimales, out double valor, out string error)
        {
            valor = 0;
            error = null;
            string t = (texto ?? "").Trim().Replace(',', '.');

            if (t.Length == 0)
            {
                error = campo + " es obligatorio.";
                return false;
            }

            if (!double.TryParse(t, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, cultura, out valor)
                || double.IsNaN(valor) || double.IsInfinity(valor))
            {
                error = campo + " debe ser un número (ejemplo: 2.5).";
                return false;
            }

            if (Math.Round(valor, maxDecimales) != valor)
            {
                error = campo + " admite como máximo " + maxDecimales + " decimales.";
                return false;
            }

            if (valor < minimo || valor > maximo)
            {
                error = campo + " debe estar entre " + minimo.ToString("0.##", cultura) +
                        " y " + maximo.ToString("0.##", cultura) + ".";
                return false;
            }

            return true;
        }



        public static string FormatearMonto(decimal monto)
        {
            return "S/ " + monto.ToString("N2", cultura);
        }

        public static string FormatearEntero(long valor)
        {
            return valor.ToString("N0", cultura);
        }

        public static string FormatearSegundos(double segundos)
        {
            return segundos.ToString("0.##", cultura) + " s";
        }

        public static string FormatearFecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm", cultura);
        }



        public static string DescribirOperacion(Operacion operacion)
        {
            return "Código   : " + operacion.CodigoOperacion + "\n" +
                   "Tipo     : " + operacion.TipoOperacion + "\n" +
                   "Monto    : " + FormatearMonto(operacion.Monto) + "\n" +
                   "Agencia  : " + operacion.CodigoAgencia + "\n" +
                   "Fecha    : " + FormatearFecha(operacion.Fecha) + "\n" +
                   "Estado   : " + operacion.Estado;
        }

        public static string DescribirAgencia(Agencia agencia, bool tieneConexion)
        {
            return "Código    : " + agencia.CodigoAgencia + "\n" +
                   "Nombre    : " + agencia.Nombre + "\n" +
                   "Dirección : " + agencia.Direccion + "\n" +
                   "Tiene conexión : " + (tieneConexion ? "Sí" : "No");
        }

        public static string DescribirConexion(Conexion conexion)
        {
            string estadoEnlace = conexion.Disponible ? "Disponible" : "No disponible (conectividad intermitente)";

            return conexion.AgenciaOrigen + " -> " + conexion.AgenciaDestino + "\n" +
                   "Tiempo de sincronización: " + FormatearSegundos(conexion.TiempoSincronizacion) +
                   "   [" + estadoEnlace + "]";
        }

        public static string DescribirRuta(RutaSincronizacion ruta)
        {
            return "Ruta: " + string.Join(" -> ", ruta.Agencias) + "\n" +
                   "Tiempo total de sincronización: " + FormatearSegundos(ruta.TiempoTotal) + "\n" +
                   "Tramos: " + (ruta.Agencias.Count - 1);
        }

        private static bool EsDigito(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
