
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

        public agregar(object dato){
            
        }

        public eliminar(){}
        public obtenerporindice(){}
        public buscar(){}
        public obtenertamano(){}

    }

}