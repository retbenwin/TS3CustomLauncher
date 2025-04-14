using System;
using System.IO;
using System.Reflection;

namespace TS3CustomLauncher
{
    class LoadingScreen
    {
        /// <summary>
        /// Selecciona de forma aleatoria una imagen de pantalla de carga desde el directorio configurado
        /// y la copia sobre el archivo que se utiliza en el mod "Variable loading screen *Updated for Into The Future*".
        /// </summary>
        /// <param name="config">Configuración que contiene las rutas necesarias.</param>
        public static void SelectScreen(Configuration config)
        {
            // Validar que se hayan definido las rutas necesarias en la configuración.
            if (string.IsNullOrEmpty(config.RutaCarpetaPantallaMods))
            {
                // Podrías notificar o registrar este error según tu manejo de errores.
                Console.WriteLine("No se han configurado ubicación de la carpeta de mods dónde se instalará el mod de pantalla de carga.");
                return;
            }

            // Verificar que el directorio de imágenes exista.
            if (!Directory.Exists(config.RutaCarpetaPantallaMods))
            {
                Console.WriteLine("El directorio de ubicación de la carpeta de mods dónde se instalará el mod de pantalla de carga no existe: " + config.RutaCarpetaPantallaMods);
                return;
            }

            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string loadingScreensDirectory = Path.Combine(exeDirectory, "LoadingScreens");


            // Listar los archivos de imagen (por ejemplo, *.png) en el directorio.
            string[] imageFiles = Directory.GetFiles(loadingScreensDirectory, "*.package");
            if (imageFiles.Length == 0)
            {
                Console.WriteLine("No se encontraron los archivos de pantallas de carga en " + loadingScreensDirectory);
                return;
            }

            // Seleccionar de forma aleatoria una imagen.
            Random rnd = new Random();
            int index = rnd.Next(imageFiles.Length + 1);
            string selectedImage;

            if (index >= imageFiles.Length)
            {
                selectedImage = "INTO_THE_FUTURE";
            }
            else
            {
                selectedImage = imageFiles[index];
            }

            // Mostrar mensaje informativo (puede ser útil para depuración).
            Console.WriteLine("Pantalla de carga seleccionada: " + selectedImage);

            try
            {
                //Limpiar directorio
                foreach (string file in Directory.GetFiles(config.RutaCarpetaPantallaMods))
                {
                    File.Delete(file);
                }
                if (selectedImage != "INTO_THE_FUTURE")
                {
                    string rutaFinal = Path.Combine(config.RutaCarpetaPantallaMods, Path.GetFileName(selectedImage));
                    // Copiar (o sobreescribir) la imagen seleccionada en la ruta que utiliza el mod.
                    // El parámetro 'true' indica que se sobreescribe el archivo si ya existe.
                    File.Copy(selectedImage, rutaFinal, true);
                }

                // Confirmación de que se ha realizado la copia.
                Console.WriteLine("La pantalla de carga ha sido actualizada.");
            }
            catch (Exception ex)
            {
                // Manejo básico del error. Puedes registrar el error o mostrar un mensaje más detallado.
                Console.WriteLine("Error al actualizar la pantalla de carga: " + ex.Message);
            }
        }
    }
}
