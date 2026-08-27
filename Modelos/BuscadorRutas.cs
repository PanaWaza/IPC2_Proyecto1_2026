using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class BuscadorRutas
    {
        public static NodoRuta BuscarRuta(MatrizOrtogonal malla, Celda entrada, Celda destino, Robot robot)
        {
            malla.ReiniciarVisitados();

            Cola cola = new Cola();
            NodoRuta inicio = new NodoRuta(entrada, null, robot.CapacidadInicial());
            cola.encolar(inicio);
            entrada.Visitado = true;

            while (!cola.estavacia())
            {
                NodoRuta actual = (NodoRuta)cola.desencolar();

                if (actual.CeldaActual == destino)
                {
                    return actual;
                }

                ListasDoblementeEnlazada vecinos = malla.ObtenerVecinos(actual.CeldaActual.Fila, actual.CeldaActual.Columna);

                for (int i = 0; i < vecinos.obtenertamano(); i++)
                {
                    Celda vecino = (Celda)vecinos.obtenerporindice(i);

                    if (vecino.Visitado)
                    {
                        continue;
                    }
                        
                    if (robot.PuedeAtravesar(vecino, actual.CapacidadRestante))
                    {
                        int capacidadNueva = actual.CapacidadRestante;

                        // Si el vecino tiene unidad militar el robot la derroto ? 
                        if (vecino.TieneUnidadMilitar())
                        {
                            capacidadNueva -= vecino.CapacidadUnidadMilitar.Value;
                        }

                        vecino.Visitado = true;
                        cola.encolar(new NodoRuta(vecino, actual, capacidadNueva));
                    }
                }
            }

            return null;  // mision imposible
        }
    }
}