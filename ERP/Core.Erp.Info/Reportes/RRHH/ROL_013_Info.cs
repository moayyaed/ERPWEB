using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.RRHH
{
    public class ROL_013_Info
    {
        public int IdEmpresa { get; set; }
        public int IdNomina_Tipo { get; set; }
        public int IdNominaTipoLiqui { get; set; }
        public string ca_descripcion { get; set; }
        public string de_descripcion { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> em_fechaIngaRol { get; set; }
        public Nullable<int> IdArea { get; set; }
        public int IdDepartamento { get; set; }
        public Nullable<int> IdDivision { get; set; }
        public Nullable<double> Sueldo { get; set; }
        public string Empleado { get; set; }
        public string em_codigo { get; set; }
        public System.DateTime pe_FechaFin { get; set; }
        public int Prestamos { get; set; }
    }
}
