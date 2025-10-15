using System.Collections;
using System.Diagnostics;
using System.Text;
using CompresorArchivosTXT.Base;

namespace CompresorArchivosTXT.Logic;

public class MotorHuffman
{
    private Frecuencias analizador;
    private Compresor compresor;
    private ArbolHuffman arbol;
    
    public MotorHuffman()
    {
        analizador = new Frecuencias();
        compresor = new Compresor();
        arbol = new ArbolHuffman();
    }
    
    public bool ComprimirArchivo(string rutaEntrada,string rutaSalida,out string mensajeResult)
    {
     Stopwatch sw= Stopwatch.StartNew();
     try
     {
         String texto = File.ReadAllText(rutaEntrada, Encoding.UTF8);
         long tamanioOriginal = new FileInfo(rutaEntrada).Length;
         Dictionary<char, int> frecuencias = analizador.AnalizarFrecuencias(texto);
         arbol.construccionArbol(frecuencias);
         Dictionary<char, string> codigos = arbol.GenerarCodigos();
         MostrarTablaSimbolos(frecuencias, codigos, texto.Length);
         StringBuilder bitsComprimidos = new StringBuilder();
         foreach (char c in texto)
         {
             bitsComprimidos.Append(codigos[c]);
         }

         compresor.GuardarArchivoComprimido(rutaSalida, bitsComprimidos.ToString(), codigos, frecuencias);
         long tamanioComprimido = new FileInfo(rutaSalida).Length;
         double ratioCompresion = (1 - (double)tamanioComprimido / tamanioOriginal) * 100;
         sw.Stop();

         MostrarEstadisticasCompresion(tamanioOriginal, tamanioComprimido, ratioCompresion, sw.ElapsedMilliseconds,
             rutaSalida);
         mensajeResult = $"✅ Archivo comprimido exitosamente a '{rutaSalida}'";
         return true;


     }
     catch (Exception e)
     {
         mensajeResult=$"❌ Error al comprimir: {e.Message}";
         return false;
     }
    }
    
    public bool DescomprimirArchivo(string rutaEntrada,string rutaSalida,out string mensajeResult)
    {
        Stopwatch sw= Stopwatch.StartNew();
        try
        {
            var (codigos, frecuencias, bitsComprimidos) = compresor.LeerArchivoComprimido(rutaEntrada);
          
            arbol.ReconstruirArbol(codigos);
            
            string textoRecuperado = arbol.Decodificador(bitsComprimidos);
            
            File.WriteAllText(rutaSalida, textoRecuperado, Encoding.UTF8);
         
            sw.Stop();
            
            long tamanioComprimido = new FileInfo(rutaEntrada).Length;
            long tamanioRecuperado = new FileInfo(rutaSalida).Length;
            
            MostrarEstadisticasDescompresion(tamanioComprimido, tamanioRecuperado, sw.ElapsedMilliseconds, rutaSalida);
            mensajeResult = $"✅ Archivo descomprimido exitosamente a '{rutaSalida}'";
            return true;
        }
        catch (Exception e)
        {
            mensajeResult=$"❌ Error al descomprimir: {e.Message}";
            return false;
        }
    }
    
    
    private string FormatearTamaño(long bytes)
    {
        string[] sufijos = { "B", "KB", "MB", "GB" };
        int indice = 0;
        double tamaño = bytes;

        while (tamaño >= 1024 && indice < sufijos.Length - 1)
        {
            tamaño /= 1024;
            indice++;
        }

        return $"{tamaño:F2} {sufijos[indice]}";
    }
    
    public void MostrarEstadisticasDescompresion(long comprimido, long recuperado, long tiempo,string rutaSalida)
    {
        Console.WriteLine("\n📊 Estadísticas de Descompresión:");
        Console.WriteLine($"   Tamaño Comprimido: {FormatearTamaño(comprimido),30}");
        Console.WriteLine($"   Tamaño Recuperado:  {FormatearTamaño(recuperado),30}");
        Console.WriteLine($"   Tiempo de Descompresión: {tiempo,24} ms");
        Console.WriteLine($"   Archivo Guardado en: {Path.GetFileName(rutaSalida),-30}");
    }
    
    public void MostrarTablaSimbolos(Dictionary<char, int> frecuencias, Dictionary<char, string> codigos, int totalCaracteres)
    {
        Console.WriteLine("\n📋 Tabla de Símbolos:");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("| Símbolo | Frecuencia | Porcentaje | Código Huffman |");
        Console.WriteLine("-------------------------------------------------");

        foreach (var par in frecuencias.OrderByDescending(p => p.Value))
        {
            char display = par.Key == ' ' ? '␣' : par.Key;
            double porcentaje = (double)par.Value / totalCaracteres * 100;
            string codigo = codigos.ContainsKey(par.Key) ? codigos[par.Key] : "";
            Console.WriteLine($"|   '{display}'   |    {par.Value,6}   |   {porcentaje,7:F2}%  |    {codigo,-15} |");
        }

        Console.WriteLine("-------------------------------------------------");
       
    }
    public void MostrarEstadisticasCompresion(long original, long comprimido, double ratio, long tiempo,string rutaSalida)
    {
       Console.WriteLine("\n📊 Estadísticas de Compresión:");
       Console.WriteLine($"   Tamaño Original:   {FormatearTamaño(original),30}");
       Console.WriteLine($"   Tamaño Comprimido: {FormatearTamaño(comprimido),30}");
       Console.WriteLine($"   Ratio de Compresión: {ratio,27:F2}%");
       Console.WriteLine($"   Tiempo de Compresión: {tiempo,24} ms");
       Console.WriteLine($"   Archivo Guardado en: {Path.GetFileName(rutaSalida),-30}");
    
    }
    
    
}