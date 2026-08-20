using System;
using System.Xml.Linq;
using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class lectorxml
    {
        public ListasDoblementeEnlazada CargarCiudades(string ruta)
        {
            ListasDoblementeEnlazada ciudades = new ListasDoblementeEnlazada();

            XDocument documento = XDocument.Load(ruta);
            XElement raiz = documento.Root; // <configuracion>
            XElement listaCiudades = raiz.Element("listaCiudades");

            foreach (XElement elementoCiudad in listaCiudades.Elements("ciudad"))
            {
                Ciudad ciudad = ProcesarCiudad(elementoCiudad);
                ciudades.AgregarFinal(ciudad);
            }

            return ciudades;
        }

        private Ciudad ProcesarCiudad(XElement elementoCiudad)
        {
            XElement elementoNombre = elementoCiudad.Element("nombre");

            string nombreCiudad = elementoNombre.Value;
            int filas = (int)elementoNombre.Attribute("filas");
            int columnas = (int)elementoNombre.Attribute("columnas");

            Ciudad ciudad = new Ciudad(nombreCiudad, filas, columnas);

            foreach (XElement elementoFila in elementoCiudad.Elements("fila"))
            {
                int numeroFila = (int)elementoFila.Attribute("numero");
                string contenidoFila = LimpiarComillas(elementoFila.Value);
                ciudad.Malla.AgregarFila(contenidoFila, numeroFila);
            }

            foreach (XElement elementoUnidad in elementoCiudad.Elements("unidadMilitar"))
            {
                int fila = (int)elementoUnidad.Attribute("fila");
                int columna = (int)elementoUnidad.Attribute("columna");
                int capacidad = (int)elementoUnidad;
                ciudad.Malla.AsignarUnidadMilitar(fila, columna, capacidad);
            }

            ciudad.EscanearCeldasEspeciales();
            return ciudad;
        }

        private string LimpiarComillas(string texto)
        {
            texto = texto.Trim();
            if (texto.StartsWith("\"") && texto.EndsWith("\""))
            {
                texto = texto.Substring(1, texto.Length - 2);
            }
            return texto;
        }
    }

    public ListasDoblementeEnlazada CargarRobots(string ruta)
    {
        ListasDoblementeEnlazada robots = new ListasDoblementeEnlazada();

        XDocument documento = XDocument.Load(ruta);
        XElement raiz = documento.Root;
        XElement elementoRobots = raiz.Element("robots");

        if (elementoRobots == null)
            return robots; // este archivo no trae robots, regresamos lista vacia

        foreach (XElement elementoRobot in elementoRobots.Elements("robot"))
        {
            Robot robot = ProcesarRobot(elementoRobot);
            robots.AgregarFinal(robot);
        }

        return robots;
    }

    private Robot ProcesarRobot(XElement elementoRobot)
    {
        XElement elementoNombre = elementoRobot.Element("nombre");

        string codigo = elementoNombre.Value;
        string tipo = (string)elementoNombre.Attribute("tipo");

        if (tipo == "ChapinFighter")
        {
            int capacidad = (int)elementoNombre.Attribute("capacidad");
            return new ChapinFighter(codigo, 0, 0, capacidad);
        }
        else // "ChapinRescue"
        {
            return new ChapinRescue(codigo, 0, 0);
        }
    }
}