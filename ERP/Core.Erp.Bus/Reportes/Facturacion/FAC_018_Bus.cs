using Core.Erp.Data.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Facturacion
{
    public class FAC_018_Bus
    {
        FAC_018_Data odata = new FAC_018_Data();
        public List<FAC_018_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdCliente, int IdTipoNota, DateTime fecha_ini, DateTime fecha_fin, bool mostrar_anulados)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdCliente, IdTipoNota, fecha_ini, fecha_fin, mostrar_anulados);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
