using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TS3CustomLauncher
{

    class Program
    {
        static void Main()
        {
            // Cargar la configuración desde el archivo config.txt (debe estar en el mismo directorio que el ejecutable)
            Configuration config = Configuration.ReadConfiguration("config.txt");
            if (config == null || string.IsNullOrEmpty(config.RutaExe) || string.IsNullOrEmpty(config.Prioridad))
            {
                // Si ocurre un error o no se han definido los parámetros, terminamos la aplicación.
                Console.WriteLine("Error: No se pudo cargar la configuración");
                Console.ReadKey();
                return;
            }

            LoadingScreen.SelectScreen(config);

            // Inicia el proceso usando la ruta proporcionada en la configuración.
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = config.RutaExe,
                    UseShellExecute = true,
                    CreateNoWindow = true // Mantiene oculta la ventana del proceso lanzado.
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error o mostrar un mensaje de depuración.
                return;
            }

            Console.WriteLine("Esperando a que se inicie el juego...");

            // Espera a que el proceso se inicie, comprobándolo cada 5 segundos.
            string processName = Path.GetFileNameWithoutExtension(config.RutaExe);
            Process[] processes = Process.GetProcessesByName(processName);
            while (processes == null || processes.Length == 0)
            {
                Thread.Sleep(5000);
                processes = Process.GetProcessesByName(processName);
            }

            // Convertir el valor de la prioridad a la enumeración ProcessPriorityClass.
            ProcessPriorityClass priorityClass;
            if (!Enum.TryParse(config.Prioridad, true, out priorityClass))
            {
                // Si el valor no es válido, se utiliza una prioridad por defecto (Normal).
                priorityClass = ProcessPriorityClass.Normal;
            }

            // Cambiar la prioridad de todos los procesos encontrados.
            foreach (Process proc in processes)
            {
                try
                {
                    proc.PriorityClass = priorityClass;
                    Console.WriteLine($"Prioridad \"{config.Prioridad}\" establecida.");
                }
                catch (Exception ex)
                {
                    // Puede ocurrir que no se tengan los permisos necesarios o que el proceso haya finalizado.
                    // Aquí se podría registrar el error o manejarlo de otro modo.
                }
            }

            // La aplicación finaliza sin abrir ventana alguna.
        }

        
    }
}
