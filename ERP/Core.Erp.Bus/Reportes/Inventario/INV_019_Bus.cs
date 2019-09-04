using Core.Erp.Data.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Inventario
{
   public class INV_019_Bus
    {
        INV_019_Data odata = new INV_019_Data();
        public List<INV_019_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, string Tipo, string IdEstadoAproba, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdBodega, IdProducto, Tipo, IdEstadoAproba, fecha_ini, fecha_fin);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
