// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Text;
using System.Linq;
using CompresorArchivosTXT.Logic;
using CompresorArchivosTXT.Base;

namespace CompresorArchivosTXT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║     PRUEBA DEL COMPRESOR DE HUFFMAN           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝\n");

            // Prueba 1: Texto simple
            PruebaBasica();
            
            Console.WriteLine("\n" + new string('═', 50) + "\n");
            
            // Prueba 2: Texto con repeticiones
            PruebaConRepeticiones();
            
            Console.WriteLine("\n" + new string('═', 50) + "\n");
            
            // Prueba 3: Texto variado
            PruebaTextoVariado();

            Console.WriteLine("\n" + new string('═', 50) + "\n");

            // Prueba 4: CICLO COMPLETO (Comprimir y Descomprimir archivo real)
            PruebaCicloCompleto();

            Console.WriteLine("\n\n✓ TODAS LAS PRUEBAS COMPLETADAS");
            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static void PruebaBasica()
        {
            Console.WriteLine("🔷 PRUEBA 1: Texto simple");
            string texto = "hola mundo";
            ProbarCompresion(texto);
        }

        static void PruebaConRepeticiones()
        {
            Console.WriteLine("🔷 PRUEBA 2: Texto con repeticiones");
            string texto = "aaaaaabbbbcccdde";
            ProbarCompresion(texto);
        }

        static void PruebaTextoVariado()
        {
            Console.WriteLine("🔷 PRUEBA 3: Texto variado");
            string texto = "Este es un texto de prueba para el algoritmo de Huffman!";
            ProbarCompresion(texto);
        }

        static void PruebaCicloCompleto()
        {
            Console.WriteLine("🔷 PRUEBA 4: CICLO COMPLETO - Comprimir y Descomprimir Archivo Real");
            
            try
            {
                // Crear archivo de prueba
                string textoOriginal = "Este es un texto de prueba para verificar el ciclo completo de compresión y descompresión usando el algoritmo de Huffman. ¡Debe funcionar perfectamente!";
                string archivoOriginal = "prueba_original.txt";
                string archivoComprimido = "prueba_comprimido.huff";
                string archivoDescomprimido = "prueba_descomprimido.txt";

                // Crear archivo original
                File.WriteAllText(archivoOriginal, textoOriginal, Encoding.UTF8);
                Console.WriteLine($"📄 Archivo original creado: '{archivoOriginal}'");
                Console.WriteLine($"   Contenido: \"{textoOriginal.Substring(0, Math.Min(50, textoOriginal.Length))}...\"");
                Console.WriteLine($"   Tamaño: {new FileInfo(archivoOriginal).Length} bytes\n");

                // Comprimir
                MotorHuffman motor = new MotorHuffman();
                Console.WriteLine("🗜️  COMPRIMIENDO...");
                bool exitoCompresion = motor.ComprimirArchivo(archivoOriginal, archivoComprimido, out string mensajeCompresion);
                Console.WriteLine(mensajeCompresion);

                if (!exitoCompresion)
                {
                    Console.WriteLine("❌ La compresión falló");
                    return;
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");

                // Descomprimir
                Console.WriteLine("📂 DESCOMPRIMIENDO...");
                bool exitoDescompresion = motor.DescomprimirArchivo(archivoComprimido, archivoDescomprimido, out string mensajeDescompresion);
                Console.WriteLine(mensajeDescompresion);

                if (!exitoDescompresion)
                {
                    Console.WriteLine("❌ La descompresión falló");
                    return;
                }

                // Verificar que los archivos son idénticos
                string textoRecuperado = File.ReadAllText(archivoDescomprimido, Encoding.UTF8);
                bool sonIguales = textoOriginal == textoRecuperado;

                Console.WriteLine("\n🔍 VERIFICACIÓN:");
                Console.WriteLine($"   Texto original:    {textoOriginal.Length} caracteres");
                Console.WriteLine($"   Texto recuperado:  {textoRecuperado.Length} caracteres");
                
                if (sonIguales)
                {
                    Console.WriteLine("\n   ✅ ¡ÉXITO TOTAL! Los archivos son idénticos");
                    Console.WriteLine("   ✅ El ciclo de compresión-descompresión funciona correctamente");
                }
                else
                {
                    Console.WriteLine("\n   ❌ ERROR: Los archivos NO son idénticos");
                    Console.WriteLine($"\n   Original:   \"{textoOriginal.Substring(0, Math.Min(50, textoOriginal.Length))}...\"");
                    Console.WriteLine($"   Recuperado: \"{textoRecuperado.Substring(0, Math.Min(50, textoRecuperado.Length))}...\"");
                }

                // Limpiar archivos de prueba (opcional)
                Console.WriteLine("\n🗑️  Limpiando archivos de prueba...");
                if (File.Exists(archivoOriginal)) File.Delete(archivoOriginal);
                if (File.Exists(archivoComprimido)) File.Delete(archivoComprimido);
                if (File.Exists(archivoDescomprimido)) File.Delete(archivoDescomprimido);
                Console.WriteLine("   Archivos temporales eliminados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR EN CICLO COMPLETO: {ex.Message}");
                Console.WriteLine($"   Tipo: {ex.GetType().Name}");
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");
            }
        }

        static void ProbarCompresion(string textoOriginal)
        {
            try
            {
                Console.WriteLine($"📝 Texto: \"{textoOriginal}\"");
                Console.WriteLine($"   Longitud: {textoOriginal.Length} caracteres\n");

                // Paso 1: Analizar frecuencias
                Frecuencias analizador = new Frecuencias();
                var frecuencias = analizador.AnalizarFrecuencias(textoOriginal);
                
                Console.WriteLine("📊 Frecuencias calculadas:");
                foreach (var par in frecuencias.OrderByDescending(p => p.Value).Take(5))
                {
                    char display = par.Key == ' ' ? '␣' : par.Key;
                    Console.WriteLine($"   '{display}' -> {par.Value} veces");
                }

                // Paso 2: Construir árbol
                ArbolHuffman arbol = new ArbolHuffman();
                arbol.construccionArbol(frecuencias);
                Console.WriteLine("\n🌳 Árbol construido correctamente");

                // Paso 3: Generar códigos
                var codigos = arbol.GenerarCodigos();
                Console.WriteLine($"🔢 Códigos generados: {codigos.Count} símbolos únicos");

                // Mostrar algunos códigos
                Console.WriteLine("\n   Ejemplos de códigos:");
                int count = 0;
                foreach (var par in codigos.OrderBy(p => p.Value.Length).Take(3))
                {
                    char display = par.Key == ' ' ? '␣' : par.Key;
                    Console.WriteLine($"   '{display}' = {par.Value}");
                    count++;
                }

                // Paso 4: Comprimir
                StringBuilder textoComprimido = new StringBuilder();
                foreach (char c in textoOriginal)
                {
                    textoComprimido.Append(codigos[c]);
                }

                int bitsOriginales = textoOriginal.Length * 8;
                int bitsComprimidos = textoComprimido.Length;
                double ahorro = (1 - (double)bitsComprimidos / bitsOriginales) * 100;

                Console.WriteLine($"\n💾 Compresión:");
                Console.WriteLine($"   Original:    {bitsOriginales} bits");
                Console.WriteLine($"   Comprimido:  {bitsComprimidos} bits");
                Console.WriteLine($"   Ahorro:      {ahorro:F2}%");

                // Paso 5: Descomprimir
                string textoRecuperado = arbol.Decodificador(textoComprimido.ToString());

                // Paso 6: Verificar
                bool correcto = textoOriginal == textoRecuperado;
                
                if (correcto)
                {
                    Console.WriteLine($"\n✅ ÉXITO: Texto recuperado correctamente");
                }
                else
                {
                    Console.WriteLine($"\n❌ ERROR: El texto no coincide");
                    Console.WriteLine($"   Original:   \"{textoOriginal}\"");
                    Console.WriteLine($"   Recuperado: \"{textoRecuperado}\"");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR: {ex.Message}");
                Console.WriteLine($"   Tipo: {ex.GetType().Name}");
            }
        }
    }
}