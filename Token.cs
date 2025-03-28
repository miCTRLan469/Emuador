using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /*public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }*/
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