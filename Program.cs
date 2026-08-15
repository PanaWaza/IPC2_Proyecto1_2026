using System;
using IPC2_PROYECTO1_2026.Modelos;

class Program
{
    static void Main()
    {
        // Malla 4 filas x 5 columnas
        // Fila 0: "E    "
        // Fila 1: "**** "   <- pared completa, unico hueco en columna 4
        // Fila 2: "    C"
        // Fila 3: "   *R"
        string[] filasTexto = {
            "E    ",
            "**** ",
            "    C",
            "   *R"
        };

        Ciudad ciudad = new Ciudad("CiudadPrueba", 4, 5);
        for (int i = 0; i < filasTexto.Length; i++)
        {
            ciudad.Malla.AgregarFila(filasTexto[i], i);
        }
        ciudad.Malla.AsignarUnidadMilitar(1, 4, 20);
        ciudad.EscanearCeldasEspeciales();

        Celda entrada = ciudad.Malla.ObtenerCelda(0, 0);
        Celda civil   = (Celda)ciudad.ObtenerCiviles().obtenerporindice(0);
        Celda recurso = (Celda)ciudad.ObtenerRecursos().obtenerporindice(0);

        Console.WriteLine("Civiles encontrados por EscanearCeldasEspeciales: " + ciudad.ObtenerCiviles().obtenertamano());
        Console.WriteLine("Recursos encontrados por EscanearCeldasEspeciales: " + ciudad.ObtenerRecursos().obtenertamano());
        Console.WriteLine("Civil en: (" + civil.Fila + "," + civil.Columna + ")");
        Console.WriteLine("Recurso en: (" + recurso.Fila + "," + recurso.Columna + ")");
        Console.WriteLine();

        Robot rescue = new ChapinRescue("R-01", 0, 0);
        Robot fighterDebil = new ChapinFighter("F-01", 0, 0, 15);
        Robot fighterFuerte = new ChapinFighter("F-02", 0, 0, 100);

        Console.WriteLine("=== MISION 1: Rescate con ChapinRescue ===");
        Mision mision1 = new Mision(TipoMision.Rescate, ciudad, rescue, entrada, civil);
        mision1.Ejecutar();
        MostrarMision(mision1);

        Console.WriteLine();
        Console.WriteLine("=== MISION 2: Extraccion con ChapinFighter debil (cap 15) ===");
        Mision mision2 = new Mision(TipoMision.Extraccion, ciudad, fighterDebil, entrada, recurso);
        mision2.Ejecutar();
        MostrarMision(mision2);

        Console.WriteLine();
        Console.WriteLine("=== MISION 3: Extraccion con ChapinFighter fuerte (cap 100) ===");
        Mision mision3 = new Mision(TipoMision.Extraccion, ciudad, fighterFuerte, entrada, recurso);
        mision3.Ejecutar();
        MostrarMision(mision3);

        Console.WriteLine();
        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadKey();
    }

    static void MostrarMision(Mision mision)
    {
        if (!mision.Exitosa)
        {
            Console.WriteLine("Resultado: Mision Imposible");
            return;
        }

        Console.WriteLine("Resultado: EXITOSA");
        Console.Write("Ruta (en orden, entrada -> destino): ");

        for (int i = 0; i < mision.RutaResultante.obtenertamano(); i++)
        {
            Celda c = (Celda)mision.RutaResultante.obtenerporindice(i);
            Console.Write("(" + c.Fila + "," + c.Columna + ")");
            if (i < mision.RutaResultante.obtenertamano() - 1)
                Console.Write(" -> ");
        }
        Console.WriteLine();
    }
}