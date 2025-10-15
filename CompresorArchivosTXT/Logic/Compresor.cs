namespace CompresorArchivosTXT.Logic;

public class Compresor
{
    public void GuardarArchivoComprimido(string rutaSalida, string bitsComprimidos
        ,Dictionary<char, string> codigos,   Dictionary<char, int> frecuencias){
        using (FileStream fs = new FileStream(rutaSalida, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(codigos.Count);
            writer.Write(bitsComprimidos.Length);
            foreach (var par in codigos)
            {
                writer.Write(par.Key);
                writer.Write(frecuencias[par.Key]);
                writer.Write((byte)par.Value.Length);
                writer.Write(par.Value.ToCharArray());
            }
           byte[] bytesComprimidos= ConvertirBitsABytes(bitsComprimidos);
              writer.Write(bytesComprimidos.Length);
        }
    }

    private byte[] ConvertirBitsABytes(string bitsComprimidos)
    {
        byte[] bytesComprimidos = new byte[bitsComprimidos.Length/8];
        
        
        
        return bytesComprimidos;
    }
}