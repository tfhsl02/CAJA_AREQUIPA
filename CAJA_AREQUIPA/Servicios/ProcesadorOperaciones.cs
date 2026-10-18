using System;
using System.Collections.Generic;
using CajaArequipa.Data;
using CajaArequipa.Estructuras;
using CajaArequipa.Modelos;

namespace CajaArequipa.Servicios
{

    public class ProcesadorOperaciones
    {
        private ColaOperaciones cola;
        private PilaReversiones pila;
        private ArbolOperaciones arbol;
        private GrafoAgencias grafo;
        private ListaSimple<Operacion> historialOperaciones;
        private ListaDoble<Operacion> bitacoraProcesadas;
        private ListaCircular<Agencia> turnosSincronizacion;
        private AlmacenDatos almacen;
        private List<string> advertencias;


        public ProcesadorOperaciones(string carpetaDatos)
        {
            cola = new ColaOperaciones();
            pila = new PilaReversiones();
            arbol = new ArbolOperaciones();
            grafo = new GrafoAgencias();
            historialOperaciones = new ListaSimple<Operacion>();
            bitacoraProcesadas = new ListaDoble<Operacion>();
            turnosSincronizacion = new ListaCircular<Agencia>();
            almacen = new AlmacenDatos(carpetaDatos);
            advertencias = new List<string>();

            CargarDatos();
        }

        public List<string> Advertencias
        {
            get { return new List<string>(advertencias); }
        }


        private static string NormalizarCodigo(string codigo)
        {
            return (codigo ?? "").Trim().ToUpperInvariant();
        }

        // AGENCIAS
        public void RegistrarAgencia(string codigoAgencia, string nombre, string direccion)
        {
            codigoAgencia = NormalizarCodigo(codigoAgencia);

            if (grafo.BuscarAgencia(codigoAgencia) != null)
            {
                throw new ReglaNegocioException("Ya existe una agencia registrada con ese código.");
            }

            Agencia nuevaAgencia = new Agencia(codigoAgencia, nombre, direccion);
            grafo.AgregarAgencia(nuevaAgencia);
            turnosSincronizacion.Agregar(nuevaAgencia);

            GuardarAgencias();
        }

        public void RegistrarConexion(string codigoOrigen, string codigoDestino, double tiempoSincronizacion)
        {
            codigoOrigen = NormalizarCodigo(codigoOrigen);
            codigoDestino = NormalizarCodigo(codigoDestino);


            Conexion nuevaConexion = new Conexion(codigoOrigen, codigoDestino, tiempoSincronizacion);
            grafo.AgregarConexion(nuevaConexion);

            GuardarConexiones();
        }

        public void CambiarDisponibilidadConexion(string codigoOrigen, string codigoDestino, bool disponible)
        {
            grafo.CambiarDisponibilidad(NormalizarCodigo(codigoOrigen), NormalizarCodigo(codigoDestino), disponible);

            GuardarConexiones();
        }

        public RutaSincronizacion CalcularRutaMinima(string codigoOrigen, string codigoDestino)
        {
            return grafo.CalcularRutaMinima(NormalizarCodigo(codigoOrigen), NormalizarCodigo(codigoDestino));
        }

        public Agencia AvanzarTurnoSincronizacion()
        {
            return turnosSincronizacion.SiguienteTurno();
        }

        public List<Agencia> ObtenerAgencias()
        {
            return new List<Agencia>(grafo.ObtenerAgencias());
        }


        public bool AgenciaTieneConexiones(string codigoAgencia)
        {
            return grafo.TieneConexiones(codigoAgencia);
        }


        public List<Conexion> ObtenerConexiones()
        {
            return grafo.ObtenerTodasLasConexiones();
        }

        public List<Conexion> ObtenerVecinos(string codigoAgencia)
        {
            return grafo.ObtenerVecinos(codigoAgencia);
        }


        // OPERACIONES
        public void RegistrarOperacion(string codigoOperacion, string tipoOperacion, decimal monto, string codigoAgencia)
        {
            codigoOperacion = NormalizarCodigo(codigoOperacion);
            codigoAgencia = NormalizarCodigo(codigoAgencia);

            if (grafo.BuscarAgencia(codigoAgencia) == null)
            {
                throw new ReglaNegocioException("La agencia indicada no existe.");
            }

            if (arbol.Buscar(codigoOperacion) != null)
            {
                throw new ReglaNegocioException("Ya existe una operación registrada con ese código.");
            }

            Operacion nuevaOperacion = new Operacion(codigoOperacion, tipoOperacion, monto, codigoAgencia);

            arbol.Insertar(nuevaOperacion);
            historialOperaciones.AgregarAlFinal(nuevaOperacion);
            cola.Encolar(nuevaOperacion);

            GuardarOperaciones();
        }

        public Operacion ProcesarSiguienteOperacion()
        {
            Operacion operacion = cola.Desencolar();

            operacion.MarcarProcesada();
            pila.Apilar(operacion);
            bitacoraProcesadas.AgregarAlFinal(operacion);

            GuardarOperaciones();

            return operacion;
        }

        public Operacion RevertirUltimaOperacion()
        {
            Operacion operacion = pila.Desapilar();

            operacion.MarcarRevertida();

            GuardarOperaciones();

            return operacion;
        }

        public Operacion BuscarOperacion(string codigoOperacion)
        {
            return arbol.Buscar(NormalizarCodigo(codigoOperacion));
        }

        public List<Operacion> ObtenerOperacionesPendientes()
        {
            return cola.ObtenerLista();
        }

        public List<Operacion> ObtenerArbolOperaciones()
        {
            return arbol.RecorridoInOrden();
        }

        public int ObtenerAlturaArbol()
        {
            return arbol.Altura;
        }

        public List<Operacion> ObtenerPilaReversiones()
        {
            return pila.ObtenerLista();
        }

        public List<Operacion> ObtenerBitacoraProcesadas()
        {
            return bitacoraProcesadas.ObtenerAtras();
        }

        public List<Operacion> ObtenerHistorialOperaciones()
        {
            return historialOperaciones.ObtenerLista();
        }

        private void GuardarAgencias()
        {
            almacen.GuardarAgencias(grafo.ObtenerAgencias());
        }

        private void GuardarConexiones()
        {
            almacen.GuardarConexiones(grafo.ObtenerConexiones());
        }

        private void GuardarOperaciones()
        {
            almacen.GuardarOperaciones(historialOperaciones.ObtenerLista());
        }

        public void GuardarDatos()
        {
            GuardarAgencias();
            GuardarConexiones();
            GuardarOperaciones();
        }

        private void CargarDatos()
        {
            int descartados = 0;

            List<Agencia> agenciasGuardadas = almacen.CargarAgencias();
            for (int i = 0; i < agenciasGuardadas.Count; i++)
            {
                try
                {
                    grafo.AgregarAgencia(agenciasGuardadas[i]);
                    turnosSincronizacion.Agregar(agenciasGuardadas[i]);
                }
                catch (ReglaNegocioException)
                {
                    descartados = descartados + 1;
                }
            }

            List<Conexion> conexionesGuardadas = almacen.CargarConexiones();
            for (int i = 0; i < conexionesGuardadas.Count; i++)
            {
                try
                {
                    grafo.AgregarConexion(conexionesGuardadas[i]);
                }
                catch (ReglaNegocioException)
                {
                    descartados = descartados + 1;
                }
            }

            List<Operacion> operacionesGuardadas = almacen.CargarOperaciones();
            for (int i = 0; i < operacionesGuardadas.Count; i++)
            {
                Operacion operacion = operacionesGuardadas[i];

                if (!arbol.Insertar(operacion))
                {
                    descartados = descartados + 1;   
                    continue;
                }

                historialOperaciones.AgregarAlFinal(operacion);

                if (operacion.Estado == EstadoOperacion.Pendiente)
                {
                    cola.Encolar(operacion);
                }
                else if (operacion.Estado == EstadoOperacion.Procesada)
                {
                    pila.Apilar(operacion);
                    bitacoraProcesadas.AgregarAlFinal(operacion);
                }
                else if (operacion.Estado == EstadoOperacion.Revertida)
                {
                    bitacoraProcesadas.AgregarAlFinal(operacion);
                }
            }

            advertencias.AddRange(almacen.Advertencias);

            if (descartados > 0)
            {
                advertencias.Add("Se descartaron " + descartados + " registro(s) de los archivos JSON por ser repetidos o inconsistentes.");
            }
        }
    }
}
