using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Clase para manejar los tokens.
*/

namespace Emulador
{
    public class Token
    {
        public enum Tipos
        {
            Identificador, Numero, Caracter, FinSentencia,
            InicioBloque, FinBloque, OperadorTernario,
            OperadorTermino, OperadorFactor, IncrementoTermino,
            IncrementoFactor, Puntero, Asignacion,
            OperadorRelacional, OperadorLogico, Moneda,
            Cadena, TipoDato, PalabraReservada, FuncionMatematica
        }
        private string contenido;
        private Tipos clasificacion;
        public Token()
        {
            contenido = "";
            clasificacion = Tipos.Identificador;
        }
        public string Contenido
        {
            get => contenido;
            set => contenido = value;
        }
        public Tipos Clasificacion
        {
           get => clasificacion;
           set => clasificacion = value;
        }
    }
}