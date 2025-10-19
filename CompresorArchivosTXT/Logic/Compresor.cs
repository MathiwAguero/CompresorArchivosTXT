using System.Text;

namespace CompresorArchivosTXT.Logic;

public class Compresor
{
    
    //Metodo principal para guardar el archivo comprimido
    // Abre un archivo binario para escritura en la ruta especificada
    // Escribe la cantidad de simbolos unicos y la longitud total de los bits comprimidos
    // Recorre el diccionario de codigos y escribe cada simbolo junto con su codigo en formato binario
    // Convierte la cadena de bits comprimidos a un arreglo de bytes y lo escribe en el archivo
    public void GuardarArchivoComprimido(string rutaSalida, string bitsComprimidos
        , Dictionary<char, string> codigos, Dictionary<char, int> frecuencias)
    {
        using (FileStream fs = new FileStream(rutaSalida, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(codigos.Count);
            writer.Write(bitsComprimidos.Length);
            foreach (var par in codigos)
            {
                writer.Write(par.Key); // Escribir el símbolo como un carácter
                byte[] codigoBytes = ConvertirBitsABytes(par.Value); // Convertir el código de bits a bytes
                writer.Write((byte)par.Value.Length); // Escribir la longitud del código en bits
                writer.Write(codigoBytes); // Escribir los bytes del código
            }

            byte[] bytesComprimidos = ConvertirBitsABytes(bitsComprimidos); // Convertir los bits comprimidos a bytes
            writer.Write(bytesComprimidos); // Escribir los bytes comprimidos
        }
    }
    
    //Metodo principal para leer el archivo comprimido
    // Abre un archivo binario para lectura en la ruta especificada
    // Lee la cantidad de simbolos unicos y la longitud total de los bits comprimidos
    // Recorre la cantidad de simbolos y lee cada simbolo junto con su codigo en formato binario
    // Convierte los bytes del codigo a una cadena de bits y los almacena en un diccionario
    // Lee los bytes comprimidos restantes y los convierte a una cadena de bits
    public (Dictionary<char, string>,  string) LeerArchivoComprimido(string rutaEntrada)
    {
        using (FileStream fs = new FileStream(rutaEntrada, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            int numSimbolos = reader.ReadInt32();// Leer la cantidad de símbolos únicos
            int numBitsComprimidos = reader.ReadInt32();// Leer la longitud total de los bits comprimidos
            Dictionary<char, string> codigos = new Dictionary<char, string>();
            for (int i = 0; i < numSimbolos; i++)
            {
                char simbolo = reader.ReadChar();// Leer el símbolo
                byte longitudCodigo = reader.ReadByte();// Leer la longitud del código en bits
                int numBytesCodigo = (longitudCodigo + 7) / 8;// Calcular el número de bytes necesarios para almacenar el código
                byte[] codigoBytes = reader.ReadBytes(numBytesCodigo);// Leer los bytes del código
                string codigo = ConvertirBYtsABites(codigoBytes, longitudCodigo);// Convertir los bytes del código a una cadena de bits
                codigos[simbolo] = codigo;// Almacenar el símbolo y su código en el diccionario
               
            }
            byte[] bytesComprimidos = reader.ReadBytes((int)(fs.Length - fs.Position));
            string bitsComprimidos = ConvertirBYtsABites(bytesComprimidos, numBitsComprimidos);
            return (codigos,  bitsComprimidos);
        }
    }

    
    // Metodos privados para conversion entre bits y bytes
    // Convierte una cadena de bits ('0' y '1') a un arreglo de bytes usando la operancion de bitsComprimidos.Lemgth +7 /8 para redondear hacia arriba
    //de esta forma nos aseguramos de que cualquier bit sobrante se incluya en un byte adicional si es necesario
    // Usa operaciones de desplazamiento de bits para establecer los bits correspondientes en cada byte
    //usa los operadores |= y << para establecer los bits en la posicion correcta dentro del byte
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
    
    // Convierte un arreglo de bytes a una cadena de bits ('0' y '1')
    // Recorre cada byte y extrae sus bits individuales usando operaciones de desplazamiento y
    // enmascaramiento de bits
    // Construye la cadena de bits resultante y la recorta a la longitud especificada por numBitsComprimidos
    // de esta forma se asegura de que solo se devuelvan los bits relevantes y obtenemos la cadena original para su posterior procesamiento
    
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