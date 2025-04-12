using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
/* 
Clase para manejar los errores. 
Muestra el mensaje de error en la consola y en el log ademas de mostrar la linea y la columna.
*/

namespace Emulador
{
    public class Error : Exception
    {
        public Error(string message) : base("Error " + message) {}
        public Error(string message, StreamWriter log) : base(message)
        {
            log.WriteLine("Error: " + message);
        }
        public Error(string message, StreamWriter log, int linea, int columna) : base(message + " en [" + linea + "," + columna + "]")
        {
            log.WriteLine("Error: " + message + " en[" + linea + "," + columna + "]");
        }
    }
}