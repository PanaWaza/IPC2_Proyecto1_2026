using System;
using System.Xml;
using System.IO;
using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class SistemaControl
    {
        private ListasDoblementeEnlazada ciudades;
        private ListasDoblementeEnlazada robots;
        private LectorXml lector;

        public SistemaControl()
        {
            ciudades = new ListasDoblementeEnlazada();
            robots = new ListasDoblementeEnlazada();
            lector = new LectorXml();
        }

        // Devuelve true si cargo correctamente. Si algo fallo, devuelve false
        // y deja el motivo en mensajeError (parametro "out").
        public bool CargarConfiguracion(string ruta, out string mensajeError)
        {
            mensajeError = "";

            try
            {
                ListasDoblementeEnlazada ciudadesNuevas = lector.CargarCiudades(ruta);
                ListasDoblementeEnlazada robotsNuevos = lector.CargarRobots(ruta);

                for (int i = 0; i < ciudadesNuevas.obtenertamano(); i++)
                {
                    Ciudad ciudadNueva = (Ciudad)ciudadesNuevas.obtenerporindice(i);
                    AgregarOActualizarCiudad(ciudadNueva);
                }

                for (int i = 0; i < robotsNuevos.obtenertamano(); i++)
                {
                    Robot robotNuevo = (Robot)robotsNuevos.obtenerporindice(i);
                    AgregarOActualizarRobot(robotNuevo);
                }

                return true;
            }
            catch (XmlException ex)
            {
                mensajeError = "El archivo no tiene un formato XML valido: " + ex.Message;
                return false;
            }
            catch (FileNotFoundException ex)
            {
                mensajeError = "No se encontro el archivo: " + ex.Message;
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                mensajeError = "La ruta indicada no existe: " + ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                // Cualquier otro error no previsto (formato de numero invalido,
                // atributo faltante, etc.) tambien se captura para que el
                // programa nunca se cierre abruptamente al cargar un archivo.
                mensajeError = "Error inesperado al procesar el archivo: " + ex.Message;
                return false;
            }
        }

        private void AgregarOActualizarCiudad(Ciudad ciudadNueva)
        {
            for (int i = 0; i < ciudades.obtenertamano(); i++)
            {
                Ciudad existente = (Ciudad)ciudades.obtenerporindice(i);
                if (existente.Nombre == ciudadNueva.Nombre)
                {
                    ciudades.Eliminar(existente);
                    ciudades.AgregarFinal(ciudadNueva);
                    return;
                }
            }
            ciudades.AgregarFinal(ciudadNueva);
        }

        private void AgregarOActualizarRobot(Robot robotNuevo)
        {
            for (int i = 0; i < robots.obtenertamano(); i++)
            {
                Robot existente = (Robot)robots.obtenerporindice(i);
                if (existente.Codigo == robotNuevo.Codigo)
                {
                    robots.Eliminar(existente);
                    robots.AgregarFinal(robotNuevo);
                    return;
                }
            }
            robots.AgregarFinal(robotNuevo);
        }

        public ListasDoblementeEnlazada ObtenerCiudades()
        {
            return ciudades;
        }

        public ListasDoblementeEnlazada ObtenerRobots()
        {
            return robots;
        }
    }
}