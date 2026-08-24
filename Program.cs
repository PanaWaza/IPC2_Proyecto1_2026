using System;
using IPC2_PROYECTO1_2026.Modelos;
using IPC2_PROYECTO1_2026.Estructuras;

class Program
{
    static void Main()
    {
        SistemaControl sistema = new SistemaControl();

        string error;

        Console.WriteLine("=== Cargando config_prueba.xml (4x5, F-01 cap 15) ===");
        bool ok1 = sistema.CargarConfiguracion("config_prueba.xml", out error);
        Console.WriteLine("Exito: " + ok1 + (ok1 ? "" : (" Error: " + error)));
        Console.WriteLine("Ciudades: " + sistema.ObtenerCiudades().obtenertamano());
        Console.WriteLine("Robots: " + sistema.ObtenerRobots().obtenertamano());

        Console.WriteLine();
        Console.WriteLine("=== Cargando config_prueba2.xml (misma ciudad 2x3, F-01 cap 999) ===");
        bool ok2 = sistema.CargarConfiguracion("config_prueba2.xml", out error);
        Console.WriteLine("Exito: " + ok2 + (ok2 ? "" : (" Error: " + error)));
        Console.WriteLine("Ciudades: " + sistema.ObtenerCiudades().obtenertamano() + " (debe seguir siendo 1, no 2)");
        Console.WriteLine("Robots: " + sistema.ObtenerRobots().obtenertamano() + " (debe seguir siendo 3, no 4: R-01, F-01 actualizado, F-02)");

        Console.WriteLine();
        Ciudad ciudad = (Ciudad)sistema.ObtenerCiudades().obtenerporindice(0);
        Console.WriteLine("Ciudad '" + ciudad.Nombre + "' ahora es " + ciudad.Filas + "x" + ciudad.Columnas + " (debe ser 2x3, la version nueva)");

        for (int i = 0; i < sistema.ObtenerRobots().obtenertamano(); i++)
        {
            Robot r = (Robot)sistema.ObtenerRobots().obtenerporindice(i);
            if (r is ChapinFighter f)
                Console.WriteLine(r.Codigo + " -> ChapinFighter cap=" + f.CapacidadCombate);
            else
                Console.WriteLine(r.Codigo + " -> ChapinRescue");
        }

        Console.WriteLine();
        Console.WriteLine("=== Probando manejo de excepciones: archivo que no existe ===");
        bool ok3 = sistema.CargarConfiguracion("archivo_que_no_existe.xml", out error);
        Console.WriteLine("Exito: " + ok3 + " | Mensaje: " + error);

        Console.WriteLine();
        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadKey();
    }
}