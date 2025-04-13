using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS3CustomLauncher
{
    // Clase para almacenar la configuración
    public class Configuration
    {
        public string RutaEjecutarExe { get; set; }
        public string RutaPrioridadExe { get; set; }
        public string Prioridad { get; set; }
        public string RutaCarpetaPantallaMods { get; set; }

        /// <summary>
        /// Lee el archivo de configuración y retorna un objeto Configuration con los parámetros.
        /// </summary>
        /// <param name="path">Ruta del archivo de configuración.</param>
        /// <returns>Objeto Configuration o null si ocurre algún error.</returns>
        public static Configuration ReadConfiguration(string path)
        {
            if (!File.Exists(path))
            {
                // El archivo de configuración no existe.
                return null;
            }
            var config = new Configuration();
            foreach (string line in File.ReadAllLines(path))
            {
                // Se ignoran líneas vacías o comentarios (líneas que comienzan con # o //).
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith("//"))
                    continue;

                // Se espera que cada línea tenga el formato clave=valor.
                string[] parts = trimmed.Split(new char[] { '=' }, 2);
                if (parts.Length != 2)
                    continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                if (key.Equals("RutaEjecutarExe", StringComparison.OrdinalIgnoreCase))
                {
                    config.RutaEjecutarExe = value;
                }
                else if (key.Equals("RutaPrioridadExe", StringComparison.OrdinalIgnoreCase))
                {
                    config.RutaPrioridadExe = value;
                }
                else if (key.Equals("Prioridad", StringComparison.OrdinalIgnoreCase))
                {
                    config.Prioridad = value;
                }
                else if (key.Equals("RutaCarpetaPantallaMods", StringComparison.OrdinalIgnoreCase))
                {
                    config.RutaCarpetaPantallaMods = value;
                }
            }
            return config;
        }
    }
}
