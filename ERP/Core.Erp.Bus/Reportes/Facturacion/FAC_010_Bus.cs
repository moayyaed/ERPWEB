using Core.Erp.Data.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Facturacion
{
    public class FAC_010_Bus
    {
        FAC_010_Data odata = new FAC_010_Data();
        public List<FAC_010_Info> get_list(int IdEmpresa, int IdSucursal, DateTime fecha_ini, DateTime fecha_fin, bool MostrarAnulados)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, fecha_ini, fecha_fin, MostrarAnulados);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
