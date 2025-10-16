namespace CompresorArchivosTXT.Base;

//Segun copilot y algunas fuentes, es buena practica implementar IComparable en las clases de nodos para facilitar la comparacion y ordenamiento de los nodos
//aqui manejamos como seran los nodos del arbol de huffman, manejando el caracter, su frecuencia y los nodos izquierdo y derecho
public class NodoHuffman : IComparable<NodoHuffman>
{
    
    public char Caracter { get; set; }
    public int Frecuencia { get; set; }
    public NodoHuffman Izquierdo { get; set; }
    public NodoHuffman Derecho { get; set; }
    
    public NodoHuffman(char caracter, int frecuencia)
    {
        Caracter = caracter;
        Frecuencia = frecuencia;
        Izquierdo = null;
        Derecho = null;
    }
    public int CompareTo(NodoHuffman other)
    {
        return this.Frecuencia.CompareTo(other.Frecuencia);
    }
    public bool EsHoja()
    {
        return Izquierdo == null && Derecho == null;
    }   
    
    public override string ToString()
    {
        return $"Caracter: {Caracter}, Frecuencia: {Frecuencia}";
    }
}