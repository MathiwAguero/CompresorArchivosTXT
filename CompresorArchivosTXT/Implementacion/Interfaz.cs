using CompresorArchivosTXT.Logic;

namespace CompresorArchivosTXT.Implementacion;

public class Interfaz
{
    private MotorHuffman motor;

    public Interfaz()
    {
        motor = new MotorHuffman();
    }

    public void Iniciar()
    {
        int opcion = -1;
        do
        {
            Console.WriteLine("\nIniciando archivo...");
            Console.WriteLine("[1] Comprimir Archivo");
            Console.WriteLine("[2] Descomprimir Archivo");
            Console.WriteLine("[3] Salir");
            Console.Write("Seleccione una opción: ");

            string input = Console.ReadLine();
            int.TryParse(input, out opcion);

            switch (opcion)
            {
                case 1:
                    ComprimirArchivo();
                    break;
                case 2:
                    DescomprimirArchivo();
                    break;
                case 3:
                    Console.WriteLine("Saliendo...");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }
        } while (opcion != 3);
    }

    public void ComprimirArchivo()
    {
        Console.Write("Ingrese la ruta del archivo a comprimir: ");
        string rutaEntrada = Console.ReadLine();
        if (!File.Exists(rutaEntrada))
        {
            Console.WriteLine("La ruta del archivo no es válida. Intente de nuevo.");
            return;
        }

        string directorio = Path.GetDirectoryName(rutaEntrada);
        string nombreExtension = Path.GetFileNameWithoutExtension(rutaEntrada);
        string rutaSalida = Path.Combine(directorio, nombreExtension + ".huff");
        if (motor.ComprimirArchivo(rutaEntrada, rutaSalida, out string mensajeResult))
        {
            Console.WriteLine(mensajeResult);
        }
        else
        {
            Console.WriteLine(mensajeResult);
        }
    }

    public void DescomprimirArchivo()
    {
        Console.Write("Ingrese la ruta del descomprimir: ");
        string rutaEntrada = Console.ReadLine();
        if (!File.Exists(rutaEntrada))
        {
            Console.WriteLine("La ruta del archivo no es válida. Intente de nuevo.");
            return;
        }

        string directorio = Path.GetDirectoryName(rutaEntrada);
        string nombreExtension = Path.GetFileNameWithoutExtension(rutaEntrada);
        string rutaSalida = Path.Combine(directorio, nombreExtension + ".txt");
        if (motor.DescomprimirArchivo(rutaEntrada, rutaSalida, out string mensajeResult))
        {
            Console.WriteLine(mensajeResult);
        }
        else
        {
            Console.WriteLine(mensajeResult);
        }
    }
}