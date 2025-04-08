using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static void Main(string[] args)
{
    //PROBAR RANDOM
    int random;
    int l;
    int m=0;

    for(l = 0; l < 5; l++) {
        random = rand(100);
        m = l + 1;
        Console.WriteLine(m + " Random = " + random);
        }
        
        
    // PRUEBA DEL PROFESOR
    char n;
    int p;
    
    float altura, i, j;
    float x = 0, y = 10, z = 2;
    float c;
    
    c = (char)(100 + 200); // 44
    
    Console.Write("Valor de altura = ");
    altura = Console.Read();
    
    // Si altura = 5, entonces
    x = (char)(((3 + altura) * 8 - (10 - 4) / 2)); // 61
    x--;                                           // 60
    x += 40;                                       // 100
    x *= 2;                                        // 200
    x /= (y - 6);                                  // 50
    x = x + 5;                                     // 55
    
    Console.WriteLine("x = " + x);
    
    for (i = 1; i <= altura; i++)
    {
        for (j = 1; j <= i; j++)
        {
            if (j % 2 == 0)
            Console.Write("*");
            else
            Console.Write("-");
            }
            Console.WriteLine("");
            }
            
            i = 0;
            
            do
            {
                Console.Write("-");
                i++;
                } while (i < altura * 2);
                
                Console.WriteLine("");
                
                for (i = 1; i <= altura; i++)
                {
                    j = 1;
                    while (j <= i)
                    {
                        Console.Write("" + j);
                j++;
            }
            Console.WriteLine("");
        }

        i = 0;
        
        do
        {
            Console.Write("-");
            i++;
            } while (i < altura * 2);
            
            Console.WriteLine("");
            
            // PROBAR CICLOS
            int height;
            int k;
            int spaces;
            int asteristics;
            
            Console.Write("Height of the triangle = ");
            height = Console.Read(); // ARREGLAR READ
            
            Console.WriteLine("");
            
            for (k = 0; k < height; k++)
            {
                for (spaces = 0; spaces < k; spaces++)
            {
                Console.Write(" ");
            }
            
            for (asteristics = 0; asteristics < height - k; asteristics++)
            {
                Console.Write("* ");
                }
                
            Console.WriteLine("");
        }
            
}