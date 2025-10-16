namespace CompresorArchivosTXT.Base;
//Se crea una interfaz para la Descompresion de archivos para que esta pueda ser inplementada unicamente cuandi se quiera descomprimir un archivo, en
//este ejemplo no tiene tanto sentido separarlo ya que si o si debe hacer ambas pero es parte de las buenas practicas de programacion 
public interface  Descompresion
{
    public  bool DescomprimirArchivo(string rutaEntrada, string rutaSalida, out string mensajeResult);

}