using Core.Erp.Data.Reportes.Inventario;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Inventario
{
  public  class INV_009_Bus
    {
        INV_009_Data odata = new INV_009_Data();
        public List<INV_009_Info> GetList(int IdEmpresa, string IdUsuario, int IdSucursal, int IdBodega, int IdCategoria, int IdLinea, int IdGrupo, int IdSubgrupo, bool considerarSinAprobar, bool mostrarSinMovimiento, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdUsuario, IdSucursal, IdBodega, IdCategoria, IdLinea, IdGrupo, IdSubgrupo, considerarSinAprobar, mostrarSinMovimiento, fecha_ini, fecha_fin);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
