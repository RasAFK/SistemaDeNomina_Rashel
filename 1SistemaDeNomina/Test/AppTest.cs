using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1SistemaDeNomina.Core;

namespace _1SistemaDeNomina.Test
{
    class AppTest
    {    
        public static void pruebaRendimiento()
        {
            var empleados = new Empleado_Asalariado[1000];
            for (int i = 0; i < empleados.Length; i++)
            {
                empleados[i] = new Empleado_Asalariado($"Empleado{i}", "Apellido", 100000 + i, 1000 + i);
            }

            var stopwatch = Stopwatch.StartNew();

            decimal totalPagos = empleados.AsParallel().Sum(e => e.calcularPagoSemanal());

            stopwatch.Stop();
            Console.WriteLine($"Tiempo de procesamiento: {stopwatch.ElapsedMilliseconds} .Cumple con la condicion: {(stopwatch.ElapsedMilliseconds < 2000 ? "Sí" : "No")}");
        }
    }
}

