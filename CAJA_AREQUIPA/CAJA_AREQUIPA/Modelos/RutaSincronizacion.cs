using System.Collections.Generic;

namespace CajaArequipa.Modelos
{
    public class RutaSincronizacion
    {
        public bool Existe { get; }
        public List<string> Agencias { get; }   
        public double TiempoTotal { get; }        

        public RutaSincronizacion(bool existe, List<string> agencias, double tiempoTotal)
        {
            Existe = existe;
            Agencias = agencias;
            TiempoTotal = tiempoTotal;
        }
    }
}
