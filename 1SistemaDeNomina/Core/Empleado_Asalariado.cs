using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1SistemaDeNomina.Core
{
    class Empleado_Asalariado : Empleados
    {
        #region Atributo unico de esta clase
        public decimal salarioSemanal { get; set; }
        #endregion


        #region Constructor
        public Empleado_Asalariado(string PrimerNombre, string ApellidoPaterno, int NumeroSeguroSocial,
        decimal SalarioSemanal) : base(PrimerNombre, ApellidoPaterno, NumeroSeguroSocial)
        {
            salarioSemanal = SalarioSemanal;
        }
        #endregion


        #region Metodo implementado
        public override decimal calcularPagoSemanal()
        {
            return salarioSemanal;
        }
        #endregion
    }
}
