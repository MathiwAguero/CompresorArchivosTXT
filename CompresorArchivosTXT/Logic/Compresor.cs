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
                byte[] codigoBytes = ConvertirBitsABytes(par.Value);
                writer.Write((byte)par.Value.Length);
                writer.Write(codigoBytes);
            }
           byte[] bytesComprimidos= ConvertirBitsABytes(bitsComprimidos);
              writer.Write(bytesComprimidos);
        }
    }
    public (Dictionary<char, string>,  string) LeerArchivoComprimido(string rutaEntrada)
    {
        using (FileStream fs = new FileStream(rutaEntrada, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            int numSimbolos = reader.ReadInt32();
            int numBitsComprimidos = reader.ReadInt32();
            Dictionary<char, string> codigos = new Dictionary<char, string>();
            for (int i = 0; i < numSimbolos; i++)
            {
                char simbolo = reader.ReadChar();
                byte longitudCodigo = reader.ReadByte();
                int numBytesCodigo = (longitudCodigo + 7) / 8;
                byte[] codigoBytes = reader.ReadBytes(numBytesCodigo);
                string codigo = ConvertirBYtsABites(codigoBytes, longitudCodigo);
                codigos[simbolo] = codigo;
               
            }
            byte[] bytesComprimidos = reader.ReadBytes((int)(fs.Length - fs.Position));
            string bitsComprimidos = ConvertirBYtsABites(bytesComprimidos, numBitsComprimidos);
            return (codigos,  bitsComprimidos);
        }
    }

    private byte[] ConvertirBitsABytes(string bitsComprimidos)
    {
        int numBytes = (bitsComprimidos.Length + 7) / 8;
        byte[] bytesComprimidos = new byte[numBytes];
    
        for (int i = 0; i < bitsComprimidos.Length; i++)
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
    private string ConvertirBYtsABites(byte[] bytesComprimidos, int numBitsComprimidos)
    {
        StringBuilder bitsComprimidos = new StringBuilder();
        foreach (byte b in bytesComprimidos)
        {
            for(int i=7;i>=0;i--)
            {
                bitsComprimidos.Append((b & (1 << i)) != 0 ? '1' : '0');
            }
        }
        return bitsComprimidos.ToString().Substring(0, Math.Min(numBitsComprimidos, bitsComprimidos.Length));
    }
}