using System;
using System.Collections.Generic;
using CajaArequipa.Modelos;

namespace CajaArequipa.Estructuras
{

    public class GrafoAgencias
    {
        private List<Agencia> agencias;
        private Dictionary<string, List<Conexion>> listaAdyacencia;

        public GrafoAgencias()
        {
            agencias = new List<Agencia>();
            listaAdyacencia = new Dictionary<string, List<Conexion>>(StringComparer.OrdinalIgnoreCase);
        }

        public void AgregarAgencia(Agencia agencia)
        {
            if (listaAdyacencia.ContainsKey(agencia.CodigoAgencia))
            {
                throw new ReglaNegocioException("Ya existe una agencia registrada con ese código.");
            }

            agencias.Add(agencia);
            listaAdyacencia.Add(agencia.CodigoAgencia, new List<Conexion>());
        }

        public void AgregarConexion(Conexion conexion)
        {
            if (!listaAdyacencia.ContainsKey(conexion.AgenciaOrigen))
            {
                throw new ReglaNegocioException("La agencia de origen no existe.");
            }

            if (!listaAdyacencia.ContainsKey(conexion.AgenciaDestino))
            {
                throw new ReglaNegocioException("La agencia de destino no existe.");
            }

            if (conexion.AgenciaOrigen.Equals(conexion.AgenciaDestino, StringComparison.OrdinalIgnoreCase))
            {
                throw new ReglaNegocioException("Una agencia no puede conectarse consigo misma.");
            }

            if (TieneConexion(conexion.AgenciaOrigen, conexion.AgenciaDestino))
            {
                throw new ReglaNegocioException("Ya existe una conexión entre esas dos agencias.");
            }

            listaAdyacencia[conexion.AgenciaOrigen].Add(conexion);

            Conexion conexionInversa = new Conexion(conexion.AgenciaDestino, conexion.AgenciaOrigen, conexion.TiempoSincronizacion, conexion.Disponible);
            listaAdyacencia[conexion.AgenciaDestino].Add(conexionInversa);
        }

        public bool TieneConexiones(string codigoAgencia)
        {
            if (!listaAdyacencia.ContainsKey(codigoAgencia))
            {
                return false;
            }

            return listaAdyacencia[codigoAgencia].Count > 0;
        }

        private Conexion BuscarConexion(string origen, string destino)
        {
            if (!listaAdyacencia.ContainsKey(origen))
            {
                return null;
            }

            List<Conexion> conexionesDeOrigen = listaAdyacencia[origen];

            for (int i = 0; i < conexionesDeOrigen.Count; i++)
            {
                if (conexionesDeOrigen[i].AgenciaDestino.Equals(destino, StringComparison.OrdinalIgnoreCase))
                {
                    return conexionesDeOrigen[i];
                }
            }

            return null;
        }

        public bool TieneConexion(string origen, string destino)
        {
            return BuscarConexion(origen, destino) != null;
        }

        public bool ConsultarDisponibilidad(string origen, string destino)
        {
            Conexion conexion = BuscarConexion(origen, destino);

            if (conexion == null)
            {
                return false;
            }

            return conexion.Disponible;
        }


        public void CambiarDisponibilidad(string origen, string destino, bool disponible)
        {
            Conexion ida = BuscarConexion(origen, destino);
            Conexion vuelta = BuscarConexion(destino, origen);

            if (ida == null || vuelta == null)
            {
                throw new ReglaNegocioException("No existe una conexión entre esas dos agencias.");
            }

            ida.Disponible = disponible;
            vuelta.Disponible = disponible;
        }


        public double? ObtenerTiempoSincronizacion(string origen, string destino)
        {
            Conexion conexion = BuscarConexion(origen, destino);

            if (conexion == null)
            {
                return null;
            }

            return conexion.TiempoSincronizacion;
        }

        public Agencia BuscarAgencia(string codigoAgencia)
        {
            for (int i = 0; i < agencias.Count; i++)
            {
                if (agencias[i].CodigoAgencia.Equals(codigoAgencia, StringComparison.OrdinalIgnoreCase))
                {
                    return agencias[i];
                }
            }

            return null;
        }

        // RUTA MÍNIMA DE SINCRONIZACIÓN (algoritmo de Dijkstra)
        public RutaSincronizacion CalcularRutaMinima(string origen, string destino)
        {
            RutaSincronizacion sinRuta = new RutaSincronizacion(false, new List<string>(), 0);

            if (!listaAdyacencia.ContainsKey(origen) || !listaAdyacencia.ContainsKey(destino))
            {
                return sinRuta;
            }

            List<string> codigos = new List<string>(listaAdyacencia.Keys);
            int n = codigos.Count;

            double[] distancia = new double[n];
            int[] previo = new int[n];
            bool[] visitado = new bool[n];

            for (int i = 0; i < n; i++)
            {
                distancia[i] = double.PositiveInfinity;
                previo[i] = -1;
                visitado[i] = false;
            }

            int indiceOrigen = IndiceDe(codigos, origen);
            int indiceDestino = IndiceDe(codigos, destino);
            distancia[indiceOrigen] = 0;

            for (int paso = 0; paso < n; paso++)
            {
                int u = -1;
                for (int i = 0; i < n; i++)
                {
                    if (!visitado[i] && !double.IsPositiveInfinity(distancia[i]) && (u == -1 || distancia[i] < distancia[u]))
                    {
                        u = i;
                    }
                }

                if (u == -1)
                {
                    break;   
                }

                visitado[u] = true;

                if (u == indiceDestino)
                {
                    break;
                }

                List<Conexion> vecinos = listaAdyacencia[codigos[u]];

                for (int j = 0; j < vecinos.Count; j++)
                {
                    Conexion c = vecinos[j];

                    if (!c.Disponible)
                    {
                        continue;  
                    }

                    int v = IndiceDe(codigos, c.AgenciaDestino);

                    if (v < 0 || visitado[v])
                    {
                        continue;
                    }

                    double nuevaDistancia = distancia[u] + c.TiempoSincronizacion;

                    if (nuevaDistancia < distancia[v])
                    {
                        distancia[v] = nuevaDistancia;
                        previo[v] = u;
                    }
                }
            }

            if (double.IsPositiveInfinity(distancia[indiceDestino]))
            {
                return sinRuta;
            }

            List<string> camino = new List<string>();
            for (int actual = indiceDestino; actual != -1; actual = previo[actual])
            {
                camino.Insert(0, codigos[actual]);
            }

            return new RutaSincronizacion(true, camino, distancia[indiceDestino]);
        }

        private int IndiceDe(List<string> codigos, string codigo)
        {
            for (int i = 0; i < codigos.Count; i++)
            {
                if (codigos[i].Equals(codigo, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        public List<Conexion> ObtenerVecinos(string codigoAgencia)
        {
            if (!listaAdyacencia.ContainsKey(codigoAgencia))
            {
                return new List<Conexion>();
            }

            return new List<Conexion>(listaAdyacencia[codigoAgencia]);
        }


        public List<Conexion> ObtenerTodasLasConexiones()
        {
            List<Conexion> todas = new List<Conexion>();

            foreach (KeyValuePair<string, List<Conexion>> par in listaAdyacencia)
            {
                todas.AddRange(par.Value);
            }

            return todas;
        }

        public List<Agencia> ObtenerAgencias()
        {
            return agencias;
        }

        public List<Conexion> ObtenerConexiones()
        {
            List<Conexion> todasLasConexiones = new List<Conexion>();

            foreach (KeyValuePair<string, List<Conexion>> par in listaAdyacencia)
            {
                List<Conexion> conexionesDeAgencia = par.Value;

                for (int i = 0; i < conexionesDeAgencia.Count; i++)
                {
                    Conexion c = conexionesDeAgencia[i];

                    if (string.CompareOrdinal(c.AgenciaOrigen, c.AgenciaDestino) < 0)
                    {
                        todasLasConexiones.Add(c);
                    }
                }
            }

            return todasLasConexiones;
        }
    }
}
