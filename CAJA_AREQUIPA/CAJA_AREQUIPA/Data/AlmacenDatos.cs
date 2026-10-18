using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CajaArequipa.Modelos;

namespace CajaArequipa.Data
{

    public class AlmacenDatos
    {

        private readonly string carpetaDatos;

        private readonly List<string> advertencias = new List<string>();

        private const string ARCHIVO_AGENCIAS = "agencias.json";
        private const string ARCHIVO_OPERACIONES = "operaciones.json";
        private const string ARCHIVO_CONEXIONES = "conexiones.json";


        private static readonly JsonSerializerOptions opcionesJson = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public AlmacenDatos(string carpetaDatos)
        {
            this.carpetaDatos = carpetaDatos;
        }

        public string CarpetaDatos
        {
            get { return carpetaDatos; }
        }

        public List<string> Advertencias
        {
            get { return new List<string>(advertencias); }
        }

        private void AsegurarCarpetaDatos()
        {
            if (!Directory.Exists(carpetaDatos))
            {
                Directory.CreateDirectory(carpetaDatos);
            }
        }

        // GUARDAR
 
        private void Guardar<T>(string nombreArchivo, List<T> datos)
        {
            AsegurarCarpetaDatos();

            string ruta = Path.Combine(carpetaDatos, nombreArchivo);
            string temporal = ruta + ".tmp";
            string json = JsonSerializer.Serialize(datos, opcionesJson);

            File.WriteAllText(temporal, json);
            File.Move(temporal, ruta, true);
        }

        public void GuardarAgencias(List<Agencia> agencias)
        {
            Guardar(ARCHIVO_AGENCIAS, agencias);
        }

        public void GuardarConexiones(List<Conexion> conexiones)
        {
            Guardar(ARCHIVO_CONEXIONES, conexiones);
        }

        public void GuardarOperaciones(List<Operacion> operaciones)
        {
            Guardar(ARCHIVO_OPERACIONES, operaciones);
        }

        // CARGAR
        private List<T> Cargar<T>(string nombreArchivo)
        {
            string ruta = Path.Combine(carpetaDatos, nombreArchivo);

            if (!File.Exists(ruta))
            {
                return new List<T>();
            }

            try
            {
                string json = File.ReadAllText(ruta);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<T>();
                }

                List<T> datos = JsonSerializer.Deserialize<List<T>>(json, opcionesJson);
                return datos ?? new List<T>();
            }
            catch (Exception ex)
            {
                string respaldo = ruta + ".corrupto-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                try
                {
                    File.Move(ruta, respaldo);
                    advertencias.Add("No se pudo leer " + nombreArchivo + " (" + ex.Message + "). Se guardó una copia como " + Path.GetFileName(respaldo) + " y se inició vacío.");
                }
                catch (Exception)
                {
                    advertencias.Add("No se pudo leer " + nombreArchivo + " (" + ex.Message + ").");
                }

                return new List<T>();
            }
        }

        public List<Agencia> CargarAgencias()
        {
            return Cargar<Agencia>(ARCHIVO_AGENCIAS);
        }

        public List<Conexion> CargarConexiones()
        {
            return Cargar<Conexion>(ARCHIVO_CONEXIONES);
        }

        public List<Operacion> CargarOperaciones()
        {
            return Cargar<Operacion>(ARCHIVO_OPERACIONES);
        }
    }
}
