// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Text;
using System.Linq;
using CompresorArchivosTXT.Logic;
using CompresorArchivosTXT.Base;
using CompresorArchivosTXT.Implementacion;

namespace CompresorArchivosTXT
{
    class Program
    {
        static void Main(string[] args)
        {
            Interfaz interfaz = new Interfaz();
            interfaz.Iniciar();
        }
    }
}