using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using IPC2_PROYECTO1_2026.Estructuras;

namespace IPC2_PROYECTO1_2026.Modelos
{
    public class GeneradorGraphviz
    {
        
        private const string COLOR_INICIO = "4CAF50";           // verde
        private const string COLOR_OBJETIVO = "F44336";         // rojo
        private const string COLOR_CAMINO_RECORRIDO = "FFF59D"; // amarillo claro
        private const string COLOR_COMBATE = "FF9800";          // naranja
        private const string COLOR_INTRANSITABLE = "424242";    // gris oscuro
        private const string COLOR_CAMINO = "FFFFFF";           // blanco
        private const string COLOR_CIVIL = "90CAF9";            // celeste (no seleccionado)
        private const string COLOR_RECURSO = "CE93D8";          // lila (no seleccionado)
        private const string COLOR_ENTRADA_SIN_USAR = "C8E6C9"; // verde muy claro

        // Genera el archivo .dot describiendo la mision (ruta ya calculada con Ejecutar()).
        public void GenerarDot(Mision mision, string rutaArchivoDot)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("digraph Ciudad {");
            builder.AppendLine("    node [shape=none];");
            builder.AppendLine("    mapa [label=<");
            builder.AppendLine("        <TABLE BORDER=\"1\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"6\">");

            MatrizOrtogonal malla = mision.CiudadSeleccionada.Malla;

            for (int f = 0; f < malla.TotalFilas; f++)
            {
                builder.AppendLine("            <TR>");
                for (int c = 0; c < malla.TotalColumnas; c++)
                {
                    Celda celda = malla.ObtenerCelda(f, c);
                    string color = DeterminarColor(celda, mision);
                    string texto = DeterminarTexto(celda);

                    builder.AppendLine("                <TD BGCOLOR=\"#" + color + "\">" + texto + "</TD>");
                }
                builder.AppendLine("            </TR>");
            }

            builder.AppendLine("        </TABLE>");
            builder.AppendLine("    >];");
            builder.AppendLine("}");

            File.WriteAllText(rutaArchivoDot, builder.ToString());
        }

        private string DeterminarColor(Celda celda, Mision mision)
        {
            // Solo si la mision fue exitosa tiene sentido resaltar la ruta
            if (mision.Exitosa)
            {
                if (celda == mision.CeldaEntrada)
                    return COLOR_INICIO;

                if (celda == mision.CeldaDestino)
                    return COLOR_OBJETIVO;

                if (EstaEnRuta(celda, mision.RutaResultante))
                {
                    // Si la celda tenia unidad militar, el robot combatio ahi
                    if (celda.TieneUnidadMilitar())
                        return COLOR_COMBATE;

                    return COLOR_CAMINO_RECORRIDO;
                }
            }

            // Celdas fuera de la ruta (o mision fallida): color segun su tipo base
            switch (celda.Tipo)
            {
                case TipoCelda.Intransitable: return COLOR_INTRANSITABLE;
                case TipoCelda.Entrada: return COLOR_ENTRADA_SIN_USAR;
                case TipoCelda.UnidadCivil: return COLOR_CIVIL;
                case TipoCelda.Recurso: return COLOR_RECURSO;
                default: return COLOR_CAMINO;
            }
        }

        private string DeterminarTexto(Celda celda)
        {
            if (celda.TieneUnidadMilitar())
                return "M(" + celda.CapacidadUnidadMilitar + ")";

            switch (celda.Tipo)
            {
                case TipoCelda.Intransitable: return "*";
                case TipoCelda.Entrada: return "E";
                case TipoCelda.UnidadCivil: return "C";
                case TipoCelda.Recurso: return "R";
                default: return "&nbsp;"; // espacio en blanco visible dentro de la celda HTML
            }
        }

        private bool EstaEnRuta(Celda celda, ListasDoblementeEnlazada ruta)
        {
            for (int i = 0; i < ruta.obtenertamano(); i++)
            {
                Celda celdaRuta = (Celda)ruta.obtenerporindice(i);
                if (celdaRuta == celda)
                    return true;
            }
            return false;
        }

        // Invoca el ejecutable "dot" de Graphviz para convertir el .dot en PNG.
        // Devuelve true si genero la imagen correctamente.
        public bool GenerarImagen(string rutaArchivoDot, string rutaImagenPng, out string mensajeError)
        {
            mensajeError = "";

            try
            {
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = "-Tpng \"" + rutaArchivoDot + "\" -o \"" + rutaImagenPng + "\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process proceso = Process.Start(info))
                {
                    string errorSalida = proceso.StandardError.ReadToEnd();
                    proceso.WaitForExit();

                    if (proceso.ExitCode != 0)
                    {
                        mensajeError = "Graphviz devolvio un error: " + errorSalida;
                        return false;
                    }
                }

                return true;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                mensajeError = "No se encontro el ejecutable 'dot'. Verifica que Graphviz este instalado y en el PATH del sistema.";
                return false;
            }
            catch (Exception ex)
            {
                mensajeError = "Error inesperado al generar la imagen: " + ex.Message;
                return false;
            }
        }
    }
}