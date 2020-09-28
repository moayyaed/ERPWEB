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
        public List<INV_008_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, string signo, int IdMovi_inven_tipo, int IdProductoTipo, string IdCategoria, int IdLinea, int IdGrupo, int IdSubGrupo)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdBodega, IdProducto, fecha_ini, fecha_fin, IdCentroCosto, signo, IdMovi_inven_tipo, IdProductoTipo, IdCategoria, IdLinea, IdGrupo,IdSubGrupo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<INV_008_Info> GetListResumen(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, string signo, int IdMovi_inven_tipo, int IdProductoTipo, string IdCategoria, int IdLinea, int IdGrupo, int IdSubGrupo)
        {
            try
            {
                return odata.GetListResumen(IdEmpresa, IdSucursal, IdBodega, IdProducto, fecha_ini, fecha_fin, IdCentroCosto, signo, IdMovi_inven_tipo, IdProductoTipo, IdCategoria, IdLinea, IdGrupo, IdSubGrupo);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
