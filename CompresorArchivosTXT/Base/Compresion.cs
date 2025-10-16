namespace CompresorArchivosTXT.Base;
//Se crea una interfaz para la compresion de archivos para que esta pueda ser inplementada unicamente cuandi se quiera comprimir un archivo, en
//este ejemplo no tiene tanto sentido separarlo ya que si o si debe hacer ambas pero es parte de las buenas practicas de programacion 
public interface Compresion
{
    public bool ComprimirArchivo  (string rutaArchivo, string rutaArchivoComprimido, out string mensajeResult);
}