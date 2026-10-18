namespace CajaArequipa.Modelos
{
    // Estados válidos de una operación. Evita escribir cadenas sueltas por todo el código.
    public static class EstadoOperacion
    {
        public const string Pendiente = "PENDIENTE";
        public const string Procesada = "PROCESADA";
        public const string Revertida = "REVERTIDA";

        public static bool EsValido(string estado)
        {
            return estado == Pendiente || estado == Procesada || estado == Revertida;
        }
    }
}
