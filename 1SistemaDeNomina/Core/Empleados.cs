using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1SistemaDeNomina.Core
{
    public abstract class Empleados
    {
        #region Atributos compartidos
        public string primerNombre { get; set; }
        public string apellidoPaterno { get; set; }
        public int numeroSeguroSocial { get; set; }
        #endregion


        # region Constructor
        public Empleados(string PrimerNombre, string ApellidoPaterno, int NumeroSeguroSocial)
        {
            primerNombre = PrimerNombre;
            apellidoPaterno = ApellidoPaterno;
            numeroSeguroSocial = NumeroSeguroSocial;
        }
        #endregion


        #region Metodo abstracto para todos
        public abstract decimal calcularPagoSemanal();
        #endregion
    }

}
