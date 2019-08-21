using Core.Erp.Data.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Inventario
{
    public class INV_020_Bus
    {
        INV_020_Data odata = new INV_020_Data();
        public List<INV_020_Info> get_list(int IdEmpresa, int idSucursal, int IdMovi_inven_tipo, decimal IdNumMovi)
        {
            try
            {
                return odata.get_list(IdEmpresa, idSucursal, IdMovi_inven_tipo, IdNumMovi);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
