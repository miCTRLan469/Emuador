using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static void Main(string[] args)
{
    int filas;

    Console.Write("Ingrese el número de filas del triángulo: ");
    filas = 5;

    int i = 1;

    while (i <= filas)
    {
        int j = 1;
        while (j <= i)
        {
            Console.Write("*");
            j++;
        }
        Console.WriteLine("");
        i++;
    }
}