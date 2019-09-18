using Core.Erp.Data.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Inventario
{
    public class INV_021_Bus
    {
        INV_021_Data odata = new INV_021_Data();
        public List<INV_021_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, DateTime fecha_corte)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdBodega, fecha_corte);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
