namespace IPC2_PROYECTO1_2026.Modelos
{
    public abstract class Robot
    {
        public string Codigo { get; set; }
        public int FilaActual { get; set; }
        public int ColumnaActual { get; set; }

        public Robot(string codigo, int filaInicial, int columnaInicial)
        {
            Codigo = codigo;
            FilaActual = filaInicial;
            ColumnaActual = columnaInicial;
        }

        public abstract bool PuedeAtravesar(Celda celda, int capacidadDisponible);

        // Capacidad con la que el robot inica cualquier mision.
        // Para ChapinRescue se deja en 0
        public abstract int CapacidadInicial();
    }
}