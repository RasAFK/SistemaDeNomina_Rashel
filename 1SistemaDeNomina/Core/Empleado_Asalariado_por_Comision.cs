using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1SistemaDeNomina.Core
{
    class Empleado_Asalariado_por_Comision : Empleados
    {
        #region Atributos unicos para esta clase
        public decimal ventasBrutas { get; set; }
        public decimal tarifaComision { get; set; }
        public decimal salarioBase { get; set; }
        #endregion


        #region Constructor
        public Empleado_Asalariado_por_Comision(string PrimerNombre, string ApellidoPaterno, int NumeroSeguroSocial,
        decimal VentasBrutas, decimal TarifaComision, decimal SalarioBase) : base(PrimerNombre, ApellidoPaterno, NumeroSeguroSocial)
        {
            ventasBrutas = VentasBrutas;
            tarifaComision = TarifaComision;
            salarioBase = SalarioBase;
        }
        #endregion


        #region Metodo implementado
        public override decimal calcularPagoSemanal()
        {
            return (ventasBrutas * tarifaComision) + salarioBase + (salarioBase * 0.10m);
        }
        #endregion
    }
}
