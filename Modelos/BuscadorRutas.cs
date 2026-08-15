using IPC2_PROYECTO1_2026.Estructuras;


namespace IPC2_PROYECTO1_2026.Modelos
{
    public class BuscadorRutas
    {
        public static NodoRuta BuscarRuta(MatrizOrtogonal malla, Celda entrada, Celda destino,bool esRescate, int capacidadCombate)
        {
            bool[,] visitado = new bool[malla.TotalFilas, malla.TotalColumnas];

            Cola cola = new Cola();
            NodoRuta inicio = new NodoRuta(entrada, null);
            cola.encolar(inicio);
            visitado[entrada.Fila, entrada.Columna] = true;

            while (!cola.estavacia())
            {
                NodoRuta actual = (NodoRuta)cola.desencolar();

                if (actual.CeldaActual == destino)
                {
                    return actual; // ¡Llegamos! Aquí está la ruta completa (subiendo por Padre)
                }

                ListasDoblementeEnlazada vecinos = malla.ObtenerVecinos(actual.CeldaActual.Fila, actual.CeldaActual.Columna);

                for (int i = 0; i < vecinos.obtenertamano(); i++)
                {
                    Celda vecino = (Celda)vecinos.obtenerporindice(i);

                    if (visitado[vecino.Fila, vecino.Columna])
                        continue; // ya lo exploramos, saltarlo

                    bool esTransitable = esRescate? vecino.EsTransitableParaRescate(): vecino.EsTransitableParaFighter(capacidadCombate);

                    if (esTransitable)
                    {
                        visitado[vecino.Fila, vecino.Columna] = true;
                        cola.encolar(new NodoRuta(vecino, actual));
                    }
                }
            }

            return null; // Cola se vació sin llegar al destino -> "Misión Imposible"
        }
    }
}