namespace IPC2_PROYECTO1_2026.Modelos
{
    public class ChapinRescue : Robot
    {
        public ChapinRescue(string codigo, int filaInicial, int columnaInicial)
            : base(codigo, filaInicial, columnaInicial)
        {
        }

        public override bool PuedeAtravesar(Celda celda)
        {
            return celda.EsTransitableParaRescate();
        }
    }
}