/*
REQUERIMIENTOS AUTOMATAS I:
    1) Indicar en el error Léxico o sintáctico, el número de línea y caracter [DONE]
    2) En el log colocar el nombre del archivo a compilar, la fecha y la hora [DONE]
    3)  Agregar el resto de asignaciones [DONE]
            Asignacion -> 
            Id = Expresion
            Id++
            Id--
            Id IncrementoTermino Expresion
            Id IncrementoFactor Expresion
            Id = Console.Read()
            Id = Console.ReadLine()
    4) Emular el Console.Write() & Console.WriteLine() [DONE] 
    5) Emular el Console.Read() & Console.ReadLine() [DONE]

NUEVOS REQUERIMIENTOS AUTOMATAS I:
    1) Concatenación [DONE]
    2) Inicializar una variable desde la declaración [DONE]
    3) Evaluar las expresiones matemáticas [DONE]
    4) Levantar una excepción si en el Console.(Read | ReadLine) no ingresan números [DONE]
    5) Modificar la variable con el resto de operadores (Incremento de factor y termino) [DONE]
    6) Implementar el else [DONE]



    ***********************REQUERIMIENTOS AUTOMATAS II:********************************
    1) Implementar set y get para la clase token (listo)
    2) Implementar parametros por default en el constructor del archivo lexico (listo)
    3) Implementar linea y columna en los errores semanticos[Listo]
    4) Implementar maxTipo en la asignacion, es decir, cuando se haga v.setValor(r)
    5) Implementar el casteo en el stack

    -------------------------REQUERIMIENTOS TERCER PARCIAL----------------------------
    REQUERIMIENTOS
    1. Excepción en el Console.Read(). [OK]
    2. Programar MathFunction método. [OK]
    3. Programar el for. La segunda asignación del for (incremento) 
       debe de ejecutarse después del bloque de instrucciones o instrucción.
    4. Programar el while.
    ---------------------------------------------------------------------------------
    ***********************************************************************************
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Emulador
{
    public class Lenguaje : Sintaxis
    {
        Stack<float> s;
        List<Variable> l;
        Variable.TipoDato maxTipo;
        public Lenguaje() : base()
        {
            s = new Stack<float>();
            l = new List<Variable>();
            maxTipo = Variable.TipoDato.Char;
            log.WriteLine("Constructor lenguaje");
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            s = new Stack<float>();
            l = new List<Variable>();
            maxTipo = Variable.TipoDato.Char;
            log.WriteLine("Constructor lenguaje");
        }

        private void displayStack()
        {
            Console.WriteLine("Contenido del stack: ");
            foreach (float elemento in s)
            {
                Console.WriteLine(elemento);
            }
        }

        private void displayLista()
        {
            log.WriteLine("Lista de variables: ");
            foreach (Variable elemento in l)
            {
                log.WriteLine($"{elemento.getNombre()} {elemento.getTipoDato()} {elemento.getValor()}");
            }
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (Contenido == "using")
            {
                Librerias();
            }
            if (Clasificacion == Tipos.TipoDato)
            {
                Variables();
            }
            Main();
            displayLista();
        }
        //Librerias -> using ListaLibrerias; Librerias?

        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");
            if (Contenido == "using")
            {
                Librerias();
            }
        }
        //Variables -> tipo_dato Lista_identificadores; Variables?

        private void Variables()
        {
            Variable.TipoDato t = Variable.TipoDato.Char;
            switch (Contenido)
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(t);
            match(";");
            if (Clasificacion == Tipos.TipoDato)
            {
                Variables();
            }
        }
        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);
            if (Contenido == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }
        //ListaIdentificadores -> identificador (= Expresion)? (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(variable => variable.getNombre() == Contenido) != null)
            {
                throw new Error($"La variable {Contenido} ya existe", log, linea, columna);
            }
            //l.Add(new Variable(t, getContenido()));
            Variable v = new Variable(t, Contenido);
            l.Add(v);

            match(Tipos.Identificador);
            if (Contenido == "=")
            {
                match("=");
                if (Contenido == "Console")
                {
                    match("Console");
                    match(".");
                    if (Contenido == "Read")
                    {
                        match("Read");
                        int r = Console.Read();
                        v.setValor(r); // Asignamos el último valor leído a la última variable detectada
                    }
                    else
                    {
                        match("ReadLine");
                        string? r = Console.ReadLine();
                        if (float.TryParse(r, out float valor))
                        {
                            v.setValor(valor);
                        }
                        else
                        {
                            throw new Error("Sintaxis. No se ingresó un número ", log, linea, columna);
                        }
                    }
                    match("(");
                    match(")");
                }
                else
                {
                    // Como no se ingresó un número desde el Console, entonces viene de una expresión matemática
                    Expresion();
                    float resultado = s.Pop();
                    l.Last().setValor(resultado);
                }
            }
            if (Contenido == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        //BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool ejecuta)
        {
            match("{");
            if (Contenido != "}")
            {
                ListaInstrucciones(ejecuta);
            }
            else
            {
                match("}");
            }
        }
        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta)
        {
            Instruccion(ejecuta);
            if (Contenido != "}")
            {
                ListaInstrucciones(ejecuta);
            }
            else
            {
                match("}");
            }
        }

        //Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool ejecuta)
        {
            if (Contenido == "Console")
            {
                console(ejecuta);
            }
            else if (Contenido == "if")
            {
                If(ejecuta);
            }
            else if (Contenido == "while")
            {
                While(ejecuta);
            }
            else if (Contenido == "do")
            {
                Do(ejecuta);
            }
            else if (Contenido == "for")
            {
                For(ejecuta);
            }
            else if (Clasificacion == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
                match(";");
            }
        }
        //Asignacion -> Identificador = Expresion; (DONE)
        /*
        Id++ (DONE)
        Id-- (DONE)
        Id IncrementoTermino Expresion (DONE)
        Id IncrementoFactor Expresion (DONE)
        Id = Console.Read() (DONE)
        Id = Console.ReadLine() (DONE)
        */

        //4) Implementar maxTipo en la asignacion, es decir, cuando se haga v.setValor(r)
        private void Asignacion()
        {
            maxTipo = Variable.TipoDato.Char;
            float r;
            Variable? v = l.Find(variable => variable.getNombre() == Contenido);
            if (v == null)
            {
                throw new Error("Sintaxis: La variable " + Contenido + " no está definida", log, linea, columna);
            }
            //Console.Write(getContenido() + " = ");
            match(Tipos.Identificador);
            if (Contenido == "++")
            {
                match("++");
                r = v.getValor() + 1;
                v.setValor(r);
            }
            else if (Contenido == "--")
            {
                match("--");
                r = v.getValor() - 1;
                v.setValor(r);
            }
            else if (Contenido == "=")
            {
                match("=");
                if (Contenido == "Console")
                {
                    match("Console");
                    match(".");
                    if (Contenido == "Read")
                    {
                        match("Read");
                        match("(");
                        int readInput = Console.Read();
                        if (readInput < 48 || readInput > 57)
                        {
                            throw new Error("Entrada invalida: Solo se permiten numeros entre 0 y 9.");
                        }
                        readInput -= 48;
                        s.Push(readInput);
                        v?.setValor(readInput, maxTipo);

                    }
                    else
                    {
                        match("ReadLine");
                        match("(");
                        string? lineaLeida = Console.ReadLine();
                        if (!float.TryParse(lineaLeida, out float numero))
                        {
                            throw new Error("Entrada invalida: Solo se permiten numeros enteros.");
                        }
                        s.Push(numero);
                        v?.setValor(numero, maxTipo);
                    }
                    match(")");
                }
                else
                {
                    Expresion();
                    r = s.Pop();
                    v.setValor(r, maxTipo);
                }
            }
            else if (Contenido == "+=")
            {
                match("+=");
                Expresion();
                r = v.getValor() + s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "-=")
            {
                match("-=");
                Expresion();
                r = v.getValor() - s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "*=")
            {
                match("*=");
                Expresion();
                r = v.getValor() * s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "/=")
            {
                match("/=");
                Expresion();
                r = v.getValor() / s.Pop();
                v.setValor(r);
            }
            else if (Contenido == "%=")
            {
                match("%=");
                Expresion();
                r = v.getValor() % s.Pop();
                v.setValor(r);
            }
            //displayStack();
        }
        /*If -> if (Condicion) bloqueInstrucciones | instruccion
        (else bloqueInstrucciones | instruccion)?*/
        private void If(bool ejecuta2)
        {
            match("if");
            match("(");
            bool ejecuta = Condicion() && ejecuta2;
            //Console.WriteLine(ejecuta);
            match(")");
            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }
            if (Contenido == "else")
            {
                match("else");
                bool ejecutarElse = !ejecuta && ejecuta2; // Solo se ejecuta el else si el if no se ejecutó
                if (Contenido == "{")
                {
                    BloqueInstrucciones(ejecutarElse);
                }
                else
                {
                    Instruccion(ejecutarElse);
                }
            }
        }
        //Condicion -> Expresion operadorRelacional Expresion
        private bool Condicion()
        {
            maxTipo = Variable.TipoDato.Char;
            Expresion();
            float valor1 = s.Pop();
            string operador = Contenido;
            match(Tipos.OperadorRelacional);
            maxTipo = Variable.TipoDato.Char;
            Expresion();
            float valor2 = s.Pop();
            switch (operador)
            {
                case ">": return valor1 > valor2;
                case ">=": return valor1 >= valor2;
                case "<": return valor1 < valor2;
                case "<=": return valor1 <= valor2;
                case "==": return valor1 == valor2;
                default: return valor1 != valor2;
            }
        }
        //While -> while(Condicion) bloqueInstrucciones | instruccion
        //bool ejecutaWhile;
        private void While(bool ejecuta)
        {
            bool ejecutaWhile;
            int charTMP = charCount - 6; //posicion de inicio del while
            int lineaTMP = linea; // linea de inicio del while

            match("while");
            match("(");
            ejecutaWhile = Condicion() && ejecuta;
            match(")");
            if (Contenido == "{")
            {
                BloqueInstrucciones(ejecutaWhile);
            }
            else
            {
                Instruccion(ejecutaWhile);
            }
            //Seek
            if (ejecutaWhile)
            {
                archivo.DiscardBufferedData(); // limpia el buffer
                archivo.BaseStream.Seek(charTMP, SeekOrigin.Begin); // lo regresa a la posicion de inicio del while
                charCount = charTMP;
                linea = lineaTMP;
                nextToken();
            }
        }
        /*Do -> do bloqueInstrucciones | intruccion 
        while(Condicion);*/
        private void Do(bool ejecuta)
        {
            //match("do");
            int charTMP = charCount - 3; // Se resta 3 porque se lee el do
            int lineaTMP = linea;
            bool ejecutaDo;
            do
            {
                match("do");
                if (Contenido == "{")
                {
                    BloqueInstrucciones(ejecuta);
                }
                else
                {
                    Instruccion(ejecuta);
                }
                match("while");
                match("(");
                ejecutaDo = Condicion() && ejecuta;
                match(")");
                match(";");
                if (ejecutaDo)
                {
                    //Seek
                    archivo.DiscardBufferedData();
                    archivo.BaseStream.Seek(charTMP, SeekOrigin.Begin);
                    charCount = charTMP;
                    linea = lineaTMP;
                    nextToken();
                }
            } while (ejecutaDo);
        }
        /*For -> for(Asignacion; Condicion; Asignacion) 
        BloqueInstrucciones | Intruccion*/
        private void For(bool ejecuta)
        {
            bool bandera = true;
            int charTMP = charCount - 4; // Se resta 4 porque se lee el for
            int lineaTMP = linea;
            bool ejecutaFor;
            match("for");
            match("(");
            Asignacion();
            int inicioCondicion = charCount;
            int lineaCondicion = linea;
            match(";");
            while (bandera)
            {
                ejecutaFor = Condicion() && ejecuta;
                int inicioIncremento = charCount;
                int lineaIncremento = linea;
                match(";");
                while (Contenido != ")"){
                    nextToken();
                }
                //Asignacion();
                match(")");
                if (Contenido == "{")
                {
                    BloqueInstrucciones(ejecutaFor);
                }
                else
                {
                    Instruccion(ejecutaFor);
                }
                //SALTO A SEGUNDA CONDICION
                archivo.DiscardBufferedData();
                archivo.BaseStream.Seek(inicioIncremento, SeekOrigin.Begin);
                charCount = inicioIncremento;
                linea = lineaIncremento;
                nextToken();
                Asignacion();

                //SALTO A EVALUAR CONDICION
                archivo.DiscardBufferedData();
                archivo.BaseStream.Seek(inicioCondicion, SeekOrigin.Begin);
                charCount = inicioCondicion;
                linea = lineaCondicion;
                nextToken();

                if (ejecutaFor == false)
                {
                    bandera = false;
                }
            }

            while (Contenido != "{")
            {
                nextToken();
            }
            if (Contenido == "{")
            {
                BloqueInstrucciones(false);
            }
            else
            {
                Instruccion(false);
            }
        }
        //Console -> Console.(WriteLine|Write) (cadena? concatenaciones?);
        private void console(bool ejecuta)
        {
            bool isWriteLine = false;
            match("Console");
            match(".");

            if (Contenido == "WriteLine")
            {
                match("WriteLine");
                isWriteLine = true;
            }
            else
            {
                match("Write");
            }

            match("(");

            string concatenaciones = "";

            if (Clasificacion == Tipos.Cadena)
            {
                concatenaciones = Contenido.Trim('"');
                match(Tipos.Cadena);
            }
            else
            {
                Variable? v = l.Find(var => var.getNombre() == Contenido);
                if (v == null)
                {
                    throw new Error("Sintaxis: La variable " + Contenido + " no está definida", log, linea, columna);
                }
                else
                {
                    concatenaciones = v.getValor().ToString();
                    match(Tipos.Identificador);
                }
            }

            if (Contenido == "+")
            {
                match("+");
                concatenaciones += Concatenaciones();  // Se acumula el resultado de las concatenaciones
            }

            match(")");
            match(";");
            if (ejecuta)
            {
                if (isWriteLine)
                {
                    Console.WriteLine(concatenaciones);
                }
                else
                {
                    Console.Write(concatenaciones);
                }
            }
        }
        // Concatenaciones -> Identificador|Cadena ( + concatenaciones )?
        private string Concatenaciones()
        {
            string resultado = "";
            if (Clasificacion == Tipos.Identificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == Contenido);
                if (v != null)
                {
                    resultado = v.getValor().ToString(); // Obtener el valor de la variable y convertirla
                }
                else
                {
                    throw new Error("La variable " + Contenido + " no está definida", log, linea, columna);
                }
                match(Tipos.Identificador);
            }
            else if (Clasificacion == Tipos.Cadena)
            {
                resultado = Contenido.Trim('"');
                match(Tipos.Cadena);
            }
            if (Contenido == "+")
            {
                match("+");
                resultado += Concatenaciones();  // Acumula el siguiente fragmento de concatenación
            }
            return resultado;
        }
        //Main -> static void Main(string[] args) BloqueInstrucciones 
        private void Main()
        {
            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            BloqueInstrucciones(true);
        }
        // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (Clasificacion == Tipos.OperadorTermino)
            {
                string operador = Contenido;
                match(Tipos.OperadorTermino);
                Termino();
                //Console.Write(operador + " ");
                float n1 = s.Pop();
                float n2 = s.Pop();
                switch (operador)
                {
                    case "+": s.Push(n2 + n1); break;
                    case "-": s.Push(n2 - n1); break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (Clasificacion == Tipos.OperadorFactor)
            {
                string operador = Contenido;
                match(Tipos.OperadorFactor);
                Factor();
                //Console.Write(operador + " ");
                float n1 = s.Pop();
                float n2 = s.Pop();
                switch (operador)
                {
                    case "*": s.Push(n2 * n1); break;
                    case "/": s.Push(n2 / n1); break;
                    case "%": s.Push(n2 % n1); break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)

        private void Factor()
        {
            if (Clasificacion == Tipos.Numero)
            {
                // Si el tipo de dato del número es mayor al tipo de dato actual, cambiarlo
                if (maxTipo < Variable.valorToTipoDato(float.Parse(Contenido)))
                {
                    maxTipo = Variable.valorToTipoDato(float.Parse(Contenido));
                }

                s.Push(float.Parse(Contenido));
                match(Tipos.Numero);
            }
            else if (Contenido == "-")
            {
                match("-");
                Factor();
                float valor = s.Pop();
                s.Push(-valor);

            }
            else if (Clasificacion == Tipos.Identificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == Contenido);

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + Contenido + " no está definida ", log, linea, columna);
                }

                if (maxTipo < v.getTipoDato())
                {
                    maxTipo = v.getTipoDato();
                }

                s.Push(v.getValor());
                match(Tipos.Identificador);
            }
            else if (Clasificacion == Tipos.FuncionMatematica)
            {
                string nombreFuncion = Contenido;
                match(Tipos.FuncionMatematica);
                match("(");
                Expresion();
                match(")");
                float resultado = s.Pop();
                float mathResult = funcionMatematica(resultado, nombreFuncion);
                s.Push(mathResult);
            }
            else
            {
                match("(");

                Variable.TipoDato tipoCasteo = Variable.TipoDato.Char;
                bool huboCasteo = false;

                // Verificar si hay un tipo de dato explícito (casteo)
                if (Clasificacion == Tipos.TipoDato)
                {
                    switch (Contenido)
                    {
                        case "int": tipoCasteo = Variable.TipoDato.Int; break;
                        case "float": tipoCasteo = Variable.TipoDato.Float; break;
                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                    huboCasteo = true;
                }

                Expresion();

                if (huboCasteo)
                {
                    float valor = s.Pop(); // Obtener el valor actual del stack
                    switch (tipoCasteo)
                    {
                        case Variable.TipoDato.Int:
                            valor %= 65536; //  2^16
                            break;
                        case Variable.TipoDato.Char:
                            valor %= 256;
                            break;
                    }
                    s.Push(valor); // Regresar el valor casteado al stack
                    maxTipo = tipoCasteo; // Actualizar el tipo máximo
                }
                match(")");
            }
        }
        float resultado;
        Random rndNum = new Random();
        private float funcionMatematica(float valor, string nombre)
        {

            switch (nombre)
            {
                case "abs": resultado = Math.Abs(valor); break;
                case "pow": resultado = (float)Math.Pow(valor, 2); break;
                case "sqrt": resultado = (float)Math.Sqrt(valor); break;
                case "ceil": resultado = (float)Math.Ceiling(valor); break; // redondea hacia arriba
                case "floor": resultado = (float)Math.Floor(valor); break; // redondea hacia abajo
                case "exp": resultado = (float)Math.Exp(valor); break;
                // 0 - 255
                // 256 - 65535
                case "max":
                    if (valor > 0 && valor <= 255)
                    {
                        resultado = Math.Max(valor, 255);
                        return resultado;
                    }
                    else if (valor >= 256 && valor <= 65535)
                    {
                        resultado = Math.Max(valor, 65535);
                        return resultado;
                    }
                    else
                    {
                        return resultado;
                    }

                case "log10": resultado = (float)Math.Log10(valor); break;
                case "log2": resultado = (float)Math.Log2(valor); break;
                case "rand": resultado = rndNum.Next((int)valor); break;
                case "truncate": resultado = (float)Math.Truncate(valor); break;
                case "round": resultado = (float)Math.Round(valor); break;
                default: throw new Error("Semantico: La funcion " + nombre + " no existe", log, linea, columna);
            }

            return resultado;
        }
    }
}