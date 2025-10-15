namespace CompresorArchivosTXT.Base;

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