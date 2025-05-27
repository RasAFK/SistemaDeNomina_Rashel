using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1SistemaDeNomina.Core
{
    class Empleado_por_Horas : Empleados
    {
        #region Atributos unicos para esta clase
        public decimal sueldoPorHora { get; set; }
        public int horasTrabajadas { get; set; }
        #endregion


        #region Constructor
        public Empleado_por_Horas(string PrimerNombre, string ApellidoPaterno, int NumeroSeguroSocial,
        decimal SueldoPorHora, int HorasTrabajadas) : base(PrimerNombre, ApellidoPaterno, NumeroSeguroSocial)
        {
            horasTrabajadas = HorasTrabajadas;
            sueldoPorHora = SueldoPorHora;
        }
        #endregion


        #region Método implementado
        public override decimal calcularPagoSemanal()
        {
            if (horasTrabajadas <= 40)
            {
                return sueldoPorHora * horasTrabajadas;
            }
            else
            {
                return (sueldoPorHora * 40) + (sueldoPorHora * 1.5m * (horasTrabajadas - 40));
            }
        }
        #endregion
    }
}
