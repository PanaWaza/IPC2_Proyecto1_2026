
namespace IPC2_PROYECTO1_2026.Estructuras
{
    public class ListasDoblementeEnlazada (){
        public Nodo? Cabeza { set; get;}
        public Nodo? Cola {get; set;}
        public int Tamano {get;set;}

        ListasDoblementeEnlazada(){
            Cabeza = null;
            Cola = null;
            Tamano = 0;
        }

        public bool estavacia(){
            return Cabeza == null;
        }

        ublic void Agregar(object dato)
        {
            Nodo nuevoNodo = new Nodo(dato);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = Cabeza;  
                Cabeza.Anterior = nuevoNodo; 
                Cabeza = nuevoNodo;         
            }
            Tamano++; 
        }

        public bool Eliminar(object dato)
        {
        Nodo actual = Cabeza;
        while (actual != null)
        {
            if (actual.Dato != null && actual.Dato.Equals(dato))
            {
                // CASO 1:  único elemento 
                if (actual == Cabeza && actual == Cola)
                {
                    Cabeza = null;
                    Cola = null;
                }
                // CASO 2: Eliminar al inicio 
                else if (actual == Cabeza)
                {
                    Cabeza = Cabeza.Siguiente;
                    Cabeza.Anterior = null;
                }
                // CASO 3: Eliminar al final 
                else if (actual == Cola)
                {
                    Cola = Cola.Anterior;
                    Cola.Siguiente = null;
                }
                // CASO 4: Eliminar en medio
                else
                {
                    actual.Anterior.Siguiente = actual.Siguiente;
                    actual.Siguiente.Anterior = actual.Anterior;
                }

                Tamano--;
                return true; 
            }
            actual = actual.Siguiente;
        }
        return false; 
        }


        public object obtenerporindice(int indice){
            // se devolvera un elemento (objeto) segun el indice

            // indice incorrecto o fuera de rango
            if (indice<0 || indice >= Tamano)  
            {
                return null;
            }

            int contador = 0 ; 
            Nodo actual = Cabeza;

            while (actual != null)
            {
                if (contador == indice)
                {
                    return actual.Dato;
                }
                actual = actual.Siguiente;
                contador++;
            }
        }


        public int obtenertamano(){
            return Tamano;
        }


        public object buscar(object dato){
            Nodo actual = Cabeza;
            while (actual != null)
            {
                if (actual.Dato.Equals(dato) && actual.Dato != null)
                {
                    return actual.Dato;
                }
            actual = actual.Siguiente;
            }
            return null;
        }


    }

}