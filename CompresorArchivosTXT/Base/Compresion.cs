namespace CompresorArchivosTXT.Base;

public interface Compresion
{
    public bool ComprimirArchivo  (string rutaArchivo, string rutaArchivoComprimido, out string mensajeResult);
}