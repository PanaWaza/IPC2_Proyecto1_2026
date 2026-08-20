using System;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public enum TipoCelda
    {
        Intransitable,   // *
        Camino,          // espacio
        Entrada,         // E
        UnidadCivil,     // C
        Recurso          // R
    }

    public class Celda
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public TipoCelda Tipo { get; set; }
        public bool visitado {get;set;}

        // null = no hay unidad militar 
        public int? CapacidadUnidadMilitar { get; set; }

        public Celda(int fila, int columna, TipoCelda tipo)
        {
            Fila = fila;
            Columna = columna;
            Tipo = tipo;
            CapacidadUnidadMilitar = null;
            visitado = false;
        }

        public bool TieneUnidadMilitar()
        {
            return CapacidadUnidadMilitar != null;
        }

        public bool EsTransitableParaRescate()
        {
            if (Tipo == TipoCelda.Intransitable || Tipo == TipoCelda.Recurso)
            {
                return false;
            }
                
            if (TieneUnidadMilitar())
            {
                return false;
            }
                
            return true;
        }

        public bool EsTransitableParaFighter(int capacidadActual)
        {
            if (Tipo == TipoCelda.Intransitable)
            {
                return false;
            }
                

            if (TieneUnidadMilitar())
            {
                return capacidadActual > CapacidadUnidadMilitar.Value;
            }
                
            return true;
        }
    }
}