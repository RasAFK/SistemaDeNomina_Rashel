using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1SistemaDeNomina.Core;

namespace _1SistemaDeNomina.Data
{
    public class Repositorio_de_Empleados
    {
        #region Colección en memoria, Lista
        public static List<Empleados> EmpleadosLista = new List<Empleados>();
        #endregion


        #region Metodos Agregar, Eliminar, Actualizar
        public void agregarEmpleado(Empleados nuevoEmpleado)
        {
            EmpleadosLista.Add(nuevoEmpleado);
        }     

        public void eliminarEmpleado(int numeroSeguroSocial)
        {
            var empleado = EmpleadosLista.FirstOrDefault(e => e.numeroSeguroSocial == numeroSeguroSocial);
            if (empleado != null)
            {
                EmpleadosLista.Remove(empleado);
            }
        }

        public void modificarEmpleado(int numeroSeguroSocial, Empleados nuevoEmpleado)
        {
            var index = EmpleadosLista.FindIndex(e => e.numeroSeguroSocial == numeroSeguroSocial);
            if (index != -1)
            {
                EmpleadosLista[index] = nuevoEmpleado;
            }
        }
        #endregion


        #region Instancias de ejemplo
        public void empleadoEjemplo()
        {
            agregarEmpleado(new Empleado_Asalariado("Juan", "Lopez", 456789012, 1000.00m));
            agregarEmpleado(new Empleado_por_Comision("Maria", "Gomez", 396389022, 900.00m, 0.15m));
            agregarEmpleado(new Empleado_por_Horas("Lady", "Gaga", 196770013, 20.00m, 15));
            agregarEmpleado(new Empleado_Asalariado_por_Comision("Zoe", "Torres", 857550101, 7500.00m, 0.08m, 1100.00m));
        }
        #endregion
    }
}