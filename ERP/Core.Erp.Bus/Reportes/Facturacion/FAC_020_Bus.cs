using Core.Erp.Data.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Facturacion
{
    public class FAC_020_Bus
    {
        FAC_020_Data odata = new FAC_020_Data();
        public List<FAC_020_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, int IdGuiaRemision)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdBodega, IdGuiaRemision);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
