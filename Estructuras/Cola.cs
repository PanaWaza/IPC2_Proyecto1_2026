namespace IPC2_PROYECTO1_2026.Estructuras
{
    class Cola
    {
        private ListasDoblementeEnlazada lista;

        public Cola ()
        {
            lista = new ListasDoblementeEnlazada();
        }

        public void encolar (object dato){
            lista.AgregarInicio(dato);
        }

        public object desencolar()
        {
            if (lista.estavacia())
            {
                return null;
            }
            return lista.EliminarFinal();
        }

        public object verfrente()
        {
            if (estavacia())
            {
                return null;
            }
            return lista.Cola.Dato; 
        }

        public bool estavacia(){
            return lista.estavacia();
        }

        public int ObtenerTamano(){
            return lista.obtenertamano();
        }
    }
}