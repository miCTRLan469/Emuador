using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emulador
{
    public class Program : Token
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje lexico = new())
                {
                    lexico.Programa();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}