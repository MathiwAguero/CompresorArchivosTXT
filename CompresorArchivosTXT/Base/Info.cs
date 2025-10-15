namespace CompresorArchivosTXT.Base;

public class Info
{
    public char Simbolo;
    public int Frecuencia;
    public double Porcentaje;
    public string Codigo;
    public int LongitudCodigo => Codigo?.Length??0;

    public string SimboloDisplay
    {
        get
        {
            return Simbolo switch
            {
                ' ' => "[ESPACIO]",
                '\n' => "[SALTO DE LINEA]",
                '\r' => "[RETORNO DE CARRO]",
                '\t' => "[TABULADOR]",
                _ => Simbolo.ToString()
            };
        }
    }
    
    
}