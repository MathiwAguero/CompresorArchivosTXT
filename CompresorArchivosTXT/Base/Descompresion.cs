namespace CompresorArchivosTXT.Base;

public interface  Descompresion
{
    public  bool DescomprimirArchivo(string rutaEntrada, string rutaSalida, out string mensajeResult);

}