using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class MatrizOrtogonal
    {
        private ListasDoblementeEnlazada filas; // cada elemento: una ListasDoblementeEnlazada de Celda
        public int TotalFilas { get; private set; }
        public int TotalColumnas { get; private set; }

        //  Parte 1 constructor 
        public MatrizOrtogonal(int totalFilas, int totalColumnas)
        {
            TotalFilas = totalFilas;
            TotalColumnas = totalColumnas;
            filas = new ListasDoblementeEnlazada();
        }

        // Parte 2 cargar una fila desde el XML 
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
                    throw new ArgumentException("Carácter no válido en la malla: " + c);
            }
        }

        // Parte 3 Acceso por coordenadas 
        public Celda ObtenerCelda(int fila, int columna)
        {
            if (fila < 0 || fila >= TotalFilas || columna < 0 || columna >= TotalColumnas)
                return null;

            object filaObj = filas.obtenerporindice(fila);
            ListasDoblementeEnlazada celdasFila = (ListasDoblementeEnlazada)filaObj;

            object celdaObj = celdasFila.obtenerporindice(columna);
            return (Celda)celdaObj;
        }

        public ListasDoblementeEnlazada ObtenerVecinos(int fila, int columna)
        {
            ListasDoblementeEnlazada vecinos = new ListasDoblementeEnlazada();

            int[] deltaFila    = { -1, 1, 0, 0 };
            int[] deltaColumna = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                Celda vecino = ObtenerCelda(fila + deltaFila[i], columna + deltaColumna[i]);
                if (vecino != null)
                {
                    vecinos.AgregarFinal(vecino);
                }
            }

            return vecinos;
        }

        public void AsignarUnidadMilitar(int fila, int columna, int capacidad)
        {
            Celda celda = ObtenerCelda(fila, columna);
            if (celda != null)
            {
                celda.CapacidadUnidadMilitar = capacidad;
            }
        }
    }
}