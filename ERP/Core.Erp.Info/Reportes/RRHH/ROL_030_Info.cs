using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.RRHH
{
    public class ROL_030_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdRol { get; set; }
        public Nullable<int> IdSucursal { get; set; }
        public int IdNominaTipo { get; set; }
        public int IdNominaTipoLiqui { get; set; }
        public int IdPeriodo { get; set; }
        public Nullable<decimal> IdEmpleado { get; set; }
        public string IdRubro { get; set; }
        public string ca_descripcion { get; set; }
        public string pe_cedulaRuc { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string rub_codigo { get; set; }
        public string ru_codRolGen { get; set; }
        public Nullable<int> ru_orden { get; set; }
        public string ru_descripcion { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<double> Valor { get; set; }
    }
}
