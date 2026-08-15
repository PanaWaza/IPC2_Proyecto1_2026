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

        MatrizOrtogonal malla = new MatrizOrtogonal(4, 5);
        for (int i = 0; i < filasTexto.Length; i++)
        {
            malla.AgregarFila(filasTexto[i], i);
        }

        // Colocamos una unidad militar de capacidad 20 en el único hueco (fila 1, columna 4)
        malla.AsignarUnidadMilitar(1, 4, 20);

        Celda entrada = malla.ObtenerCelda(0, 0);
        Celda civil   = malla.ObtenerCelda(2, 4);
        Celda recurso = malla.ObtenerCelda(3, 4);

        Console.WriteLine("=== CASO 1: ChapinRescue busca al civil (14,19 en el mundo real) ===");
        NodoRuta resultadoRescate = BuscadorRutas.BuscarRuta(malla, entrada, civil, esRescate: true, capacidadCombate: 0);
        MostrarResultado(resultadoRescate, "Mision Imposible: la unidad militar bloquea el unico paso y ChapinRescue no puede combatir");

        Console.WriteLine();
        Console.WriteLine("=== CASO 2: ChapinFighter (capacidad 15) busca el recurso, NO alcanza a vencer a la unidad (cap 20) ===");
        NodoRuta resultadoFighterDebil = BuscadorRutas.BuscarRuta(malla, entrada, recurso, esRescate: false, capacidadCombate: 15);
        MostrarResultado(resultadoFighterDebil, "Mision Imposible: capacidad insuficiente (15 <= 20)");

        Console.WriteLine();
        Console.WriteLine("=== CASO 3: ChapinFighter (capacidad 100) busca el recurso, SI alcanza a vencer a la unidad (cap 20) ===");
        NodoRuta resultadoFighterFuerte = BuscadorRutas.BuscarRuta(malla, entrada, recurso, esRescate: false, capacidadCombate: 100);
        MostrarResultado(resultadoFighterFuerte, "No debio fallar");

        Console.WriteLine();
        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadKey();
    }

    static void MostrarResultado(NodoRuta resultado, string mensajeSiFalla)
    {
        if (resultado == null)
        {
            Console.WriteLine("Resultado: Mision Imposible");
            Console.WriteLine("(" + mensajeSiFalla + ")");
            return;
        }

        Console.WriteLine("Resultado: ruta encontrada");
        // Reconstruir la ruta subiendo por Padre, y luego invertirla para mostrarla de inicio a fin
        System.Collections.Generic.List<string> pasos = new System.Collections.Generic.List<string>();
        NodoRuta actual = resultado;
        while (actual != null)
        {
            pasos.Add("(" + actual.CeldaActual.Fila + "," + actual.CeldaActual.Columna + ")");
            actual = actual.Padre;
        }
        pasos.Reverse();
        Console.WriteLine("Ruta: " + string.Join(" -> ", pasos));
    }
}