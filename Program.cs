using System;
using IPC2_PROYECTO1_2026.Modelos;
using IPC2_PROYECTO1_2026.Estructuras;

class Program
{
    static SistemaControl sistema = new SistemaControl();
    static GeneradorGraphviz generador = new GeneradorGraphviz();

    //  se llena en la Opcion 2 y se usa en la Opcion 3
    static Ciudad ciudadSeleccionada = null;
    static Robot robotSeleccionado = null;

    // Resultado de la ultima mision ejecutada, se usa en la Opcion 4
    static Mision misionActual = null;

    static void Main()
    {
        bool continuar = true;

        while (continuar)
        {
            MostrarMenu();
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    OpcionCargarConfiguracion();
                    break;
                case "2":
                    OpcionSeleccionarCiudadYRobot();
                    break;
                case "3":
                    OpcionEjecutarMision();
                    break;
                case "4":
                    OpcionMostrarResultado();
                    break;
                case "5":
                    continuar = false;
                    Console.WriteLine("Hasta luego.");
                    break;
                default:
                    Console.WriteLine("Opcion no valida.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void MostrarMenu()
    {
        Console.WriteLine(" SISTEMA DE CONTROL - CHAPIN WARRIORS");
        Console.WriteLine("----------------------------------------");

        Console.WriteLine("Ciudad activa: " + (ciudadSeleccionada != null ? ciudadSeleccionada.Nombre : "(ninguna)"));
        Console.WriteLine("Robot activo:  " + (robotSeleccionado != null ? robotSeleccionado.Codigo : "(ninguno)"));
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Cargar archivo de configuracion XML");
        Console.WriteLine("2. Consultar / Seleccionar ciudad y robot");
        Console.WriteLine("3. Ejecutar mision (rescate o extraccion)");
        Console.WriteLine("4. Mostrar resultado y generar grafica");
        Console.WriteLine("5. Salir");
        Console.Write("Elige una opcion: ");
    }

    // OPCION 1 ----------------
    static void OpcionCargarConfiguracion()
    {
        Console.Write("Ruta del archivo XML: ");
        string ruta = Console.ReadLine();

        string error;
        bool ok = sistema.CargarConfiguracion(ruta, out error);

        if (ok)
        {
            Console.WriteLine("Configuracion cargada correctamente.");
            Console.WriteLine("Ciudades disponibles: " + sistema.ObtenerCiudades().obtenertamano());
            Console.WriteLine("Robots disponibles: " + sistema.ObtenerRobots().obtenertamano());
        }
        else
        {
            Console.WriteLine("No se pudo cargar el archivo: " + error);
        }
    }

    // OPCION 2 ----------------
    static void OpcionSeleccionarCiudadYRobot()
    {
        ListasDoblementeEnlazada ciudades = sistema.ObtenerCiudades();
        if (ciudades.obtenertamano() == 0)
        {
            Console.WriteLine("No hay ciudades cargadas.");
            return;
        }

        Console.WriteLine(" \nCiudades disponibles ---");
        for (int i = 0; i < ciudades.obtenertamano(); i++)
        {
            Ciudad c = (Ciudad)ciudades.obtenerporindice(i);
            Console.WriteLine((i + 1) + ". " + c.Nombre + " (" + c.Filas + "x" + c.Columnas + ")"
                + " | Entradas: " + c.ObtenerEntradas().obtenertamano()
                + " | Civiles: " + c.ObtenerCiviles().obtenertamano()
                + " | Recursos: " + c.ObtenerRecursos().obtenertamano());
        }

        int indiceCiudad = LeerIndiceValido("\nSelecciona una ciudad : ", ciudades.obtenertamano());
        if (indiceCiudad == -1) return;
        ciudadSeleccionada = (Ciudad)ciudades.obtenerporindice(indiceCiudad);

        ListasDoblementeEnlazada robots = sistema.ObtenerRobots();
        if (robots.obtenertamano() == 0)
        {
            Console.WriteLine("No hay robots cargados.");
            robotSeleccionado = null;
            return;
        }

        Console.WriteLine(" Robots disponibles ---");
        for (int i = 0; i < robots.obtenertamano(); i++)
        {
            Robot r = (Robot)robots.obtenerporindice(i);
            if (r is ChapinFighter f)
            {
                Console.WriteLine((i + 1) + ". " + r.Codigo + " - ChapinFighter (capacidad combate: " + f.CapacidadCombate + ")");

            }
            else
            {
                Console.WriteLine((i + 1) + ". " + r.Codigo + " - ChapinRescue");

            }
        }

        int indiceRobot = LeerIndiceValido("Selecciona un robot: ", robots.obtenertamano());
        if (indiceRobot == -1) return;
        robotSeleccionado = (Robot)robots.obtenerporindice(indiceRobot);

        Console.WriteLine("Actualmente: " + ciudadSeleccionada.Nombre + " - " + robotSeleccionado.Codigo);
    }

    // OPCION 3 ----------------
    static void OpcionEjecutarMision()
    {
        if (ciudadSeleccionada == null || robotSeleccionado == null)
        {
            Console.WriteLine("Primero selecciona una ciudad y un robot");
            return;
        }

        TipoMision tipo;
        ListasDoblementeEnlazada destinosPosibles;

        if (robotSeleccionado is ChapinRescue)
        {
            tipo = TipoMision.Rescate;
            destinosPosibles = ciudadSeleccionada.ObtenerCiviles();

            if (destinosPosibles.obtenertamano() == 0)
            {
                Console.WriteLine("Esta ciudad no tiene unidades civiles, no se puede hacer una mision de rescate aqui.");
                return;
            }
        }
        else
        {
            tipo = TipoMision.Extraccion;
            destinosPosibles = ciudadSeleccionada.ObtenerRecursos();

            if (destinosPosibles.obtenertamano() == 0)
            {
                Console.WriteLine("Esta ciudad no tiene recursos, no se puede hacer una mision de extraccion aqui.");
                return;
            }
        }

        ListasDoblementeEnlazada entradas = ciudadSeleccionada.ObtenerEntradas();
        if (entradas.obtenertamano() == 0)
        {
            Console.WriteLine("Esta ciudad no tiene puntos de entrada.");
            return;
        }

        Celda entradaElegida = SeleccionarCelda(entradas, "punto de entrada");
        if (entradaElegida == null) return;

        Celda destinoElegido = SeleccionarCelda(destinosPosibles, tipo == TipoMision.Rescate ? "unidad civil" : "recurso");
        if (destinoElegido == null) return;

        try
        {
            misionActual = new Mision(tipo, ciudadSeleccionada, robotSeleccionado, entradaElegida, destinoElegido);
            misionActual.Ejecutar();

            Console.WriteLine();
            Console.WriteLine("Mision ejecutada. Usa la opcion 4 para ver el resultado y la grafica.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("No se pudo crear la mision: " + ex.Message);
            misionActual = null;
        }
    }

    // Si hay una sola celda en la lista, la usa directo (sin preguntar).
    // Si hay varias, muestra un mini-menu para elegir.
    static Celda SeleccionarCelda(ListasDoblementeEnlazada celdas, string etiqueta)
    {
        if (celdas.obtenertamano() == 1)
        {
            return (Celda)celdas.obtenerporindice(0);
        }

        Console.WriteLine("--- Selecciona " + etiqueta + " ---");
        for (int i = 0; i < celdas.obtenertamano(); i++)
        {
            Celda c = (Celda)celdas.obtenerporindice(i);
            Console.WriteLine((i + 1) + ". (" + c.Fila + "," + c.Columna + ")");
        }

        int indice = LeerIndiceValido("Numero: ", celdas.obtenertamano());
        if (indice == -1) return null;
        return (Celda)celdas.obtenerporindice(indice);
    }

    // OPCION 4 ----------------
    static void OpcionMostrarResultado()
    {
        if (misionActual == null)
        {
            Console.WriteLine("Todavia no se ha ejecutado ninguna mision (opcion 3).");
            return;
        }

        Console.WriteLine("Tipo de mision: " + misionActual.Tipo);
        Console.WriteLine("Ciudad: " + misionActual.CiudadSeleccionada.Nombre);
        Console.WriteLine("Robot: " + misionActual.RobotSeleccionado.Codigo);
        Console.WriteLine("Entrada: (" + misionActual.CeldaEntrada.Fila + "," + misionActual.CeldaEntrada.Columna + ")");
        Console.WriteLine("Destino: (" + misionActual.CeldaDestino.Fila + "," + misionActual.CeldaDestino.Columna + ")");
        Console.WriteLine();

        if (!misionActual.Exitosa)
        {
            Console.WriteLine("RESULTADO: MISION IMPOSIBLE");
        }
        else
        {
            Console.WriteLine("RESULTADO: MISION EXITOSA");
            Console.Write("Ruta: ");
            for (int i = 0; i < misionActual.RutaResultante.obtenertamano(); i++)
            {
                Celda c = (Celda)misionActual.RutaResultante.obtenerporindice(i);
                Console.Write("(" + c.Fila + "," + c.Columna + ")");
                if (i < misionActual.RutaResultante.obtenertamano() - 1) Console.Write(" -> ");
            }
            Console.WriteLine();
        }

        string nombreDot = "mision_resultado.dot";
        string nombrePng = "mision_resultado.png";

        generador.GenerarDot(misionActual, nombreDot);
        string errorImagen;
        bool okImagen = generador.GenerarImagen(nombreDot, nombrePng, out errorImagen);

        if (okImagen)
        {
            Console.WriteLine();
            Console.WriteLine("Grafica generada: " + nombrePng);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("No se pudo generar la grafica: " + errorImagen);
        }
    }

    // Lee un numero de menu, valida el rango, y devuelve el INDICE 
    // Devuelve -1 si la entrada no es valida.
    static int LeerIndiceValido(string mensaje, int cantidadOpciones)
    {
        Console.Write(mensaje);
        string entrada = Console.ReadLine();

        int numero;
        if (!int.TryParse(entrada, out numero))
        {
            Console.WriteLine("Entrada invalida, debe ser un numero.");
            return -1;
        }

        if (numero < 1 || numero > cantidadOpciones)
        {
            Console.WriteLine("Numero fuera de rango.");
            return -1;
        }

        return numero - 1;
    }
}