using CompresorArchivosTXT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CompresorArchivosTXT.Logic;
using CompresorArchivosTXT.Base;


public class ArbolHuffman
{
    private NodoHuffman raiz;
    private Dictionary<char, string> codigos;

    public ArbolHuffman()
    {
        codigos = new Dictionary<char, string>();
    }

    public void construccionArbol(Dictionary<char, int> freciencias)
    {
        if (freciencias.Count == 0 || freciencias == null)
            throw new ArgumentException("El diccionario de frecuencias no puede estar vacío.");

        List<NodoHuffman> colaPrioridad = new List<NodoHuffman>();

        foreach (var par in freciencias)
        {
            colaPrioridad.Add(new NodoHuffman(par.Key, par.Value));
        }

        while (colaPrioridad.Count > 1)
        {
            colaPrioridad = colaPrioridad.OrderBy(n => n.Frecuencia).ToList();
            NodoHuffman izquierdo = colaPrioridad[0];
            colaPrioridad.RemoveAt(0);
            NodoHuffman derecho = colaPrioridad[0];
            colaPrioridad.RemoveAt(0);
            NodoHuffman nuevoNodo = new NodoHuffman('\0', izquierdo.Frecuencia + derecho.Frecuencia)
            {
                Izquierdo = izquierdo,
                Derecho = derecho
            };
            colaPrioridad.Add(nuevoNodo);
        }

        raiz = colaPrioridad[0];
    }   
    public Dictionary<char,string> GenerarCodigos()
    {
        codigos.Clear();
        if (raiz == null)
            throw new InvalidOperationException("El árbol de Huffman no ha sido construido.");

        codigos.Clear();
        GenerarCodigosRecursivo(raiz, "");
        return new Dictionary<char,string>(codigos);
        
    }
    
    private void GenerarCodigosRecursivo(NodoHuffman nodo, string codigoActual)
    {
        if(nodo==null)
            return;
        if (nodo.EsHoja())
        {
            codigos[nodo.Caracter] = codigoActual.Length>0?codigoActual:"0";
            return;
        }
        
        if (nodo.Izquierdo != null)
            GenerarCodigosRecursivo(nodo.Izquierdo, codigoActual + "0");

        if (nodo.Derecho != null)
            GenerarCodigosRecursivo(nodo.Derecho, codigoActual + "1");
    }

    public void ReconstruirArbol(Dictionary<char, string> codigos)
    {
        raiz = new NodoHuffman('\0', 0);
        codigos = new Dictionary<char, string>(codigos);
        foreach (var par in codigos)
        {
            NodoHuffman actual = raiz;
            foreach (char bit in par.Value)
            {
                if(bit=='0')
                {
                    if (actual.Izquierdo == null)
                        actual.Izquierdo = new NodoHuffman('\0', 0);
                    actual = actual.Izquierdo;
                }
                else 
                {
                    if (actual.Derecho == null)
                        actual.Derecho = new NodoHuffman('\0', 0);
                    actual = actual.Derecho;
                }
            }
            actual.Caracter = par.Key; 
        }
    }

    public string Decodificador(string bits)
    {
        if(raiz==null)
            throw new InvalidOperationException("El árbol de Huffman no ha sido construido.");
        StringBuilder resultado = new StringBuilder();
        NodoHuffman actual = raiz;
        foreach (char bit in bits)
        {
            actual = (bit=='0') ? actual.Izquierdo : actual.Derecho;
            
            if (actual.EsHoja())
            {
                resultado.Append(actual.Caracter);
                actual = raiz;
            }
        }
        return resultado.ToString();
    }
    public NodoHuffman GetRaiz()
    {
        return raiz;
    }
}

