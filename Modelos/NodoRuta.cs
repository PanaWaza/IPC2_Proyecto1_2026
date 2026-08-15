namespace IPC2_PROYECTO1_2026.Modelos
{
    public class NodoRuta
    {
        public Celda CeldaActual { get; set; }
        public NodoRuta Padre { get; set; } // null si es el punto de entrada

        public NodoRuta(Celda celdaActual, NodoRuta padre)
        {
            CeldaActual = celdaActual;
            Padre = padre;
        }
    }
}