
namespace IPC2_PROYECTO1_2026.Estructuras
{
    class Pila 
    {
        private ListasDoblementeEnlazada lista;

        public Pila()
        {
            lista = new ListasDoblementeEnlazada();
        }

        public void apilar(object dato)
        {
            lista.AgregarInicio(dato); // inserta en la cabeza = tope de la pila
        }

        public object desapilar()
        {
            if (lista.estavacia())
            {
                return null;
            }
            return lista.EliminarInicio(); // saca del mismo extremo -> LIFO
        }

        public object vertope()
        {
            if (estavacia())
            {
                return null;
            }
            return lista.Cabeza.Dato;
        }

        public bool estavacia()
        {
            return lista.estavacia();
        }

        public int obtenertamano()
        {
            return lista.obtenertamano();
        }
    }
}