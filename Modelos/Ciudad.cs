using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class Ciudad
    {
        public string Nombre{get;set;}
        public MatrizOrtogonal Malla {get; set;}

        public ListasDoblementeEnlazada RecursosDisponibles;
        public ListasDoblementeEnlazada CivilesDisponibles;
        private ListasDoblementeEnlazada EntradasDisponibles;
        
        public int Filas
        {
            get { return Malla.TotalFilas; }
        }

        public int Columnas => Malla.TotalColumnas; // forma mas corta de hacer lo de arriba

        public Ciudad (string nombre, int filas , int columnas)
        {
            Nombre = nombre;
            Malla = new MatrizOrtogonal(filas, columnas);

            RecursosDisponibles = new ListasDoblementeEnlazada();
            CivilesDisponibles = new ListasDoblementeEnlazada();
            EntradasDisponibles = new ListasDoblementeEnlazada();
        }

        public void EscanearCeldasEspeciales()
        {
            for(int f =0 ; f < Filas; f++)
            {
                for(int c =0 ; c < Columnas; c++)
                {
                    Celda celda = Malla.ObtenerCelda(f,c);

                    if(celda.Tipo == TipoCelda.UnidadCivil)
                    {
                        CivilesDisponibles.AgregarFinal(celda);
                    }
                    else if (celda.Tipo == TipoCelda.Recurso)
                    {
                        RecursosDisponibles.AgregarFinal(celda);
                    }
                    else if (celda.Tipo == TipoCelda.Entrada)
                    {
                        EntradasDisponibles.AgregarFinal(celda);
                    }
                }
            }
        }

        public ListasDoblementeEnlazada ObtenerCiviles()
        {
            return CivilesDisponibles;
        }

        public ListasDoblementeEnlazada ObtenerRecursos()
        {
            return RecursosDisponibles;
        }

        public ListasDoblementeEnlazada ObtenerEntradas()
        {
            return EntradasDisponibles;
        }
    }
}