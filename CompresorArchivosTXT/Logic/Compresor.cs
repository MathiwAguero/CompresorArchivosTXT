using System.Text;

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
    public (Dictionary<char, string>, Dictionary<char, int>) LeerArchivoComprimido(string rutaEntrada)
    {
        using (FileStream fs = new FileStream(rutaEntrada, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            int numSimbolos = reader.ReadInt32();
            int numBitsComprimidos = reader.ReadInt32();
            Dictionary<char, string> codigos = new Dictionary<char, string>();
            Dictionary<char, int> frecuencias = new Dictionary<char, int>();
            for (int i = 0; i < numSimbolos; i++)
            {
                char simbolo = reader.ReadChar();
                int frecuencia = reader.ReadInt32();
                byte longitudCodigo = reader.ReadByte();
                char[] codigoChars = reader.ReadChars(longitudCodigo);
                string codigo = new string(codigoChars);
                codigos[simbolo] = codigo;
                frecuencias[simbolo] = frecuencia;
            }
            byte[] bytesComprimidos = reader.ReadBytes((int)(fs.Length - fs.Position));
            string bitsComprimidos = ConvertirBitsABites(bytesComprimidos, numBitsComprimidos);
            return (codigos, frecuencias);
        }
    }

    private byte[] ConvertirBitsABytes(string bitsComprimidos)
    {
        int numBytes = (bitsComprimidos.Length + 7) / 8;
        byte[] bytesComprimidos = new byte[bitsComprimidos.Length/8];
        for (int i = 0; i < bitsComprimidos.Length; i += 8)
        {
            if (bitsComprimidos[i] == '1')
            {
                int byteIndex = i / 8;
                int bitIndex = 7 - (i % 8);
                bytesComprimidos[byteIndex] |= (byte)(1 << bitIndex);
            }
        }
        return bytesComprimidos;
    }
    private string ConvertirBitsABites(byte[] bytesComprimidos, int numBitsComprimidos)
    {
        StringBuilder bitsComprimidos = new StringBuilder();
        foreach (byte b in bytesComprimidos)
        {
            for(int i=7;i>=0;i--)
            {
                bitsComprimidos.Append((b & (1 << i)) != 0 ? '1' : '0');
            }
        }
        return bitsComprimidos.ToString();
    }
}