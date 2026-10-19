using System.Text.Json.Serialization;

namespace CajaArequipa.Modelos
{
    public class Conexion
    {
        public string AgenciaOrigen { get; }
        public string AgenciaDestino { get; }
        public bool Disponible { get; set; }            
        public double TiempoSincronizacion { get; set; }

        public Conexion(string agenciaOrigen, string agenciaDestino, double tiempoSincronizacion)
            : this(agenciaOrigen, agenciaDestino, tiempoSincronizacion, true)
        {
        }

        [JsonConstructor]
        public Conexion(string agenciaOrigen, string agenciaDestino, double tiempoSincronizacion, bool disponible)
        {
            if (string.IsNullOrWhiteSpace(agenciaOrigen) || string.IsNullOrWhiteSpace(agenciaDestino))
            {
                throw new ReglaNegocioException("La conexión debe indicar agencia de origen y de destino.");
            }

            if (tiempoSincronizacion <= 0 || double.IsNaN(tiempoSincronizacion) || double.IsInfinity(tiempoSincronizacion))
            {
                throw new ReglaNegocioException("El tiempo de sincronización debe ser mayor a 0.");
            }

            AgenciaOrigen = agenciaOrigen;
            AgenciaDestino = agenciaDestino;
            TiempoSincronizacion = tiempoSincronizacion;
            Disponible = disponible;
        }
    }
}
