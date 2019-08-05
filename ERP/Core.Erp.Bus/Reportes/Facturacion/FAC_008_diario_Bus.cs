using Core.Erp.Data.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Facturacion
{
    public class FAC_008_diario_Bus
    {
        FAC_008_diario_Data odata = new FAC_008_diario_Data();
        public List<FAC_008_diario_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdBodega, IdNota);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
