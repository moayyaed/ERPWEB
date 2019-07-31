using Core.Erp.Data.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Inventario
{
   public class INV_008_Bus
    {
        INV_008_Data odata = new INV_008_Data();
        public List<INV_008_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, int IdMovi_inven_tipo)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdBodega, IdProducto, fecha_ini, fecha_fin, IdCentroCosto, IdMovi_inven_tipo);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
