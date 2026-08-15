namespace IPC2_PROYECTO1_2026.Modelos
{
    public class ChapinFighter : Robot
    {
        public int CapacidadCombate { get; set; }

        public ChapinFighter(string codigo, int filaInicial, int columnaInicial, int capacidadCombate)
            : base(codigo, filaInicial, columnaInicial)
        {
            CapacidadCombate = capacidadCombate;
        }

        public override bool PuedeAtravesar(Celda celda)
        {
            return celda.EsTransitableParaFighter(CapacidadCombate);
        }
    }
}