// See https://aka.ms/new-console-template for more information

using System;
using System.Text;
using System.Linq;
// Ajusta estos using según tus namespaces
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