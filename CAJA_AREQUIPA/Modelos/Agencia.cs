using System.Text.Json.Serialization;

namespace CajaArequipa.Modelos
{

    public class Agencia
    {
        public string CodigoAgencia { get; }  
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        [JsonConstructor]
        public Agencia(string codigoAgencia, string nombre, string direccion)
        {
            if (string.IsNullOrWhiteSpace(codigoAgencia))
            {
                throw new ReglaNegocioException("El código de agencia no puede estar vacío.");
            }

            CodigoAgencia = codigoAgencia;
            Nombre = nombre;
            Direccion = direccion;
        }
    }
}
