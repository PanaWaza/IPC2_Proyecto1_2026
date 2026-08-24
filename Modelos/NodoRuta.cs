namespace IPC2_PROYECTO1_2026.Modelos
{
    public class NodoRuta
    {
        public Celda CeldaActual { get; set; }
        public NodoRuta Padre { get; set; }
        public int CapacidadRestante { get; set; }

        public NodoRuta(Celda celdaActual, NodoRuta padre, int capacidadRestante)
        {
            CeldaActual = celdaActual;
            Padre = padre;
            CapacidadRestante = capacidadRestante;
        }
    }
}