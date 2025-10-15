using CompresorArchivosTXT.Base;

namespace CompresorArchivosTXT.Logic;

public class Frecuencias
{
    private Dictionary<char, int> frecuencias;
    private string texto;

    public Frecuencias()
    {
        frecuencias = new Dictionary<char, int>();
    }
    public Dictionary<char,int> AnalizarFrecuencias(string texto)
    {
        this.texto = texto;
        frecuencias.Clear();
        foreach (char c in texto)
        {
            if (frecuencias.ContainsKey(c))
            {
                frecuencias[c]++;
            }
            else
            {
                frecuencias[c] = 1;
            }
        }
        return frecuencias;
    }

    public List<Info> GenerarTablaSimbolos(Dictionary<char, int> codigos)
    {
        int totalCaracteres = texto.Length;
        List<Info> tablaSimbolos = new List<Info>();

        foreach (var kvp in codigos)
        {
            tablaSimbolos.Add(new Info
            {
                Simbolo = kvp.Key,
                Frecuencia = kvp.Value,
                Porcentaje = (double)kvp.Value / totalCaracteres * 100,
                Codigo = string.Empty 
            });
        }
        return tablaSimbolos.OrderByDescending(i => i.Frecuencia).ToList();
    }
    public Dictionary<char, int> GetFrecuencias()
    {
        return frecuencias;
    } 
}