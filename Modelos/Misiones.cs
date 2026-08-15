using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public enum TipoMision
    {
        Rescate,
        Extraccion
    }

    public class Mision
    {
        public TipoMision Tipo { get; set; }
        public Ciudad CiudadSeleccionada { get; set; }
        public Robot RobotSeleccionado { get; set; }
        public Celda CeldaEntrada { get; set; }
        public Celda CeldaDestino { get; set; }

        public ListasDoblementeEnlazada RutaResultante { get; set; }
        public bool Exitosa { get; private set; }

        public Mision(TipoMision tipo, Ciudad ciudad, Robot robot, Celda entrada, Celda destino)
        {
            Tipo = tipo;
            CiudadSeleccionada = ciudad;
            RobotSeleccionado = robot;
            CeldaEntrada = entrada;
            CeldaDestino = destino;
            RutaResultante = new ListasDoblementeEnlazada();
            Exitosa = false;
        }

        public void Ejecutar()
        {
            NodoRuta resultado = BuscadorRutas.BuscarRuta(CiudadSeleccionada.Malla, CeldaEntrada, CeldaDestino, RobotSeleccionado);

            if (resultado == null)
            {
                Exitosa = false;
                return; // Mision Imposible: RutaResultante se queda vacia
            }

            Exitosa = true;
            RutaResultante = ReconstruirRutaEnOrden(resultado);
        }

        private ListasDoblementeEnlazada ReconstruirRutaEnOrden(NodoRuta destino)
        {
            // "destino" viene con .Padre apuntando hacia atras hasta la entrada.
            // Si lo agregaramos directo a una lista, quedaria INVERTIDO
            // (destino, ..., entrada). Necesitamos el orden correcto
            // (entrada, ..., destino) para mostrarlo.

            Pila pila = new Pila();
            NodoRuta actual = destino;
            while (actual != null)
            {
                pila.apilar(actual.CeldaActual);
                actual = actual.Padre;
            }

            // Ahora la Pila tiene: tope=entrada ... fondo=destino
            // (porque el ultimo que apilamos fue la entrada)
            ListasDoblementeEnlazada ruta = new ListasDoblementeEnlazada();
            while (!pila.estavacia())
            {
                ruta.AgregarFinal((Celda)pila.desapilar());
            }

            return ruta;
        }
    }
}