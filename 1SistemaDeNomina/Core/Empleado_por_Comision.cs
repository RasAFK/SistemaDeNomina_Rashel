using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1SistemaDeNomina.Core
{
    class Empleado_por_Comision : Empleados
    {
        #region Atributos unicos para esta clase
        public decimal ventasBrutas { get; set; }
        public decimal tarifaComision { get; set; }
        #endregion
        

        #region Constructor
        public Empleado_por_Comision(string PrimerNombre, string ApellidoPaterno, int NumeroSeguroSocial,
        decimal VentasBrutas, decimal TarifaComision) : base(PrimerNombre, ApellidoPaterno, NumeroSeguroSocial)
        {
            ventasBrutas = VentasBrutas;
            tarifaComision = TarifaComision;
        }
        #endregion


        #region Metodo implementado
        public override decimal calcularPagoSemanal()
        {
            return ventasBrutas * tarifaComision;
        }
        #endregion
    }
}
