using System;
using System.Xml;
using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class LectorXml
    {
        public ListasDoblementeEnlazada CargarCiudades(string ruta)
        {
            ListasDoblementeEnlazada ciudades = new ListasDoblementeEnlazada();

            XmlDocument documento = new XmlDocument();
            documento.Load(ruta);

            XmlNode nodoListaCiudades = documento.SelectSingleNode("/configuracion/listaCiudades");
            if (nodoListaCiudades == null)
                return ciudades;

            foreach (XmlNode nodoCiudad in nodoListaCiudades.SelectNodes("ciudad"))
            {
                Ciudad ciudad = ProcesarCiudad(nodoCiudad);
                ciudades.AgregarFinal(ciudad);
            }

            return ciudades;
        }

        private Ciudad ProcesarCiudad(XmlNode nodoCiudad)
        {
            XmlNode nodoNombre = nodoCiudad.SelectSingleNode("nombre");

            string nombreCiudad = nodoNombre.InnerText;
            int filas = int.Parse(nodoNombre.Attributes["filas"].Value);
            int columnas = int.Parse(nodoNombre.Attributes["columnas"].Value);

            Ciudad ciudad = new Ciudad(nombreCiudad, filas, columnas);

            foreach (XmlNode nodoFila in nodoCiudad.SelectNodes("fila"))
            {
                int numeroFila = int.Parse(nodoFila.Attributes["numero"].Value);
                string contenidoFila = LimpiarComillas(nodoFila.InnerText);
                ciudad.Malla.AgregarFila(contenidoFila, numeroFila);
            }

            foreach (XmlNode nodoUnidad in nodoCiudad.SelectNodes("unidadMilitar"))
            {
                int fila = int.Parse(nodoUnidad.Attributes["fila"].Value);
                int columna = int.Parse(nodoUnidad.Attributes["columna"].Value);
                int capacidad = int.Parse(nodoUnidad.InnerText);
                ciudad.Malla.AsignarUnidadMilitar(fila, columna, capacidad);
            }

            ciudad.EscanearCeldasEspeciales();
            return ciudad;
        }

        public ListasDoblementeEnlazada CargarRobots(string ruta)
        {
            ListasDoblementeEnlazada robots = new ListasDoblementeEnlazada();

            XmlDocument documento = new XmlDocument();
            documento.Load(ruta);

            XmlNode nodoRobots = documento.SelectSingleNode("/configuracion/robots");
            if (nodoRobots == null)
                return robots;

            foreach (XmlNode nodoRobot in nodoRobots.SelectNodes("robot"))
            {
                Robot robot = ProcesarRobot(nodoRobot);
                robots.AgregarFinal(robot);
            }

            return robots;
        }

        private Robot ProcesarRobot(XmlNode nodoRobot)
        {
            XmlNode nodoNombre = nodoRobot.SelectSingleNode("nombre");

            string codigo = nodoNombre.InnerText;
            string tipo = nodoNombre.Attributes["tipo"].Value;

            if (tipo == "ChapinFighter")
            {
                int capacidad = int.Parse(nodoNombre.Attributes["capacidad"].Value);
                return new ChapinFighter(codigo, 0, 0, capacidad);
            }
            else
            {
                return new ChapinRescue(codigo, 0, 0);
            }
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
}