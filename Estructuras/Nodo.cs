namespace IPC2_PROYECTO1_2026.Estructuras
{
    public class Nodo
    {
        public object? Dato { get; set; }
        public Nodo? Siguiente { get; set; }
        public Nodo? Anterior { get; set; }

        public Nodo(object? dato)
        {
            Dato = dato;
            Siguiente = null;
            Anterior = null;
        }
    }
}