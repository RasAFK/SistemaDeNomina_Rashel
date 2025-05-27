using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1SistemaDeNomina.Data;
using _1SistemaDeNomina.Presentacion;
using _1SistemaDeNomina.Test;

namespace _1SistemaDeNomina
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Repositorio_de_Empleados repositorio = new Repositorio_de_Empleados();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());

            AppTest.pruebaRendimiento();
        }
    }
}
