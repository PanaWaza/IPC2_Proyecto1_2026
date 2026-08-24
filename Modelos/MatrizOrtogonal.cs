using System;
using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class MatrizOrtogonal
    {
        private ListasDoblementeEnlazada filas;
        public int TotalFilas { get; private set; }
        public int TotalColumnas { get; private set; }

        public MatrizOrtogonal(int totalFilas, int totalColumnas)
        {
            TotalFilas = totalFilas;
            TotalColumnas = totalColumnas;
            filas = new ListasDoblementeEnlazada();
        }

        public void AgregarFila(string contenidoFila, int numeroFila)
        {
            ListasDoblementeEnlazada celdasFila = new ListasDoblementeEnlazada();

            for (int columna = 0; columna < contenidoFila.Length; columna++)
            {
                char c = contenidoFila[columna];
                TipoCelda tipo = InterpretarCaracter(c);
                Celda celda = new Celda(numeroFila, columna, tipo);
                celdasFila.AgregarFinal(celda);
            }

            filas.AgregarFinal(celdasFila);
        }

        private TipoCelda InterpretarCaracter(char c)
        {
            switch (c)
            {
                case '*': return TipoCelda.Intransitable;
                case ' ': return TipoCelda.Camino;
                case 'E': return TipoCelda.Entrada;
                case 'C': return TipoCelda.UnidadCivil;
                case 'R': return TipoCelda.Recurso;
                default:
                    throw new ArgumentException("Caracter no valido en la malla: " + c);
            }
        }

        public void AsignarUnidadMilitar(int fila, int columna, int capacidad)
        {
            Celda celda = ObtenerCelda(fila, columna);
            if (celda != null)
            {
                celda.CapacidadUnidadMilitar = capacidad;
            }
        }

        public Celda ObtenerCelda(int fila, int columna)
        {
            if (fila < 0 || fila >= TotalFilas || columna < 0 || columna >= TotalColumnas)
            {
                return null;
            }
                

            object filaObj = filas.obtenerporindice(fila);
            ListasDoblementeEnlazada celdasFila = (ListasDoblementeEnlazada)filaObj;

            object celdaObj = celdasFila.obtenerporindice(columna);
            return (Celda)celdaObj;
        }

         public ListasDoblementeEnlazada ObtenerVecinos(int fila, int columna)
        {
            ListasDoblementeEnlazada vecinos = new ListasDoblementeEnlazada();

            // creo referencia a vecino y apunto hacia (derecha)
            
            Celda vecino = ObtenerCelda(fila , columna + 1);

            if (vecino != null)
            {
                vecinos.AgregarFinal(vecino);
            }

            // izquierda
            vecino = ObtenerCelda(fila,columna - 1);
            if (vecino != null)
            {
                vecinos.AgregarFinal(vecino);
            }

            // arriba
            vecino = ObtenerCelda(fila - 1 ,columna);
            if (vecino != null)
            {
                vecinos.AgregarFinal(vecino);
            }

            // abajo
            vecino = ObtenerCelda(fila + 1 ,columna);
            if (vecino != null)
            {
                vecinos.AgregarFinal(vecino);
            }

            return vecinos;
        }

        public void ReiniciarVisitados()
        {
            for (int f = 0; f < TotalFilas; f++)
            {
                for (int c = 0; c < TotalColumnas; c++)
                {
                    Celda celda = ObtenerCelda(f, c);
                    celda.Visitado = false;
                }
            }
        }
    }
}