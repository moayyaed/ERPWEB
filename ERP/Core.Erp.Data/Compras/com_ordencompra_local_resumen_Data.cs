using Core.Erp.Data.Compras.Base;
using Core.Erp.Info.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Compras
{
    public class com_ordencompra_local_resumen_Data
    {
        public com_ordencompra_local_resumen_Info get_info(int IdEmpresa, int IdSucursal, decimal IdOrdenCompra)
        {
            try
            {
                com_ordencompra_local_resumen_Info info = new com_ordencompra_local_resumen_Info();
                using (Entities_compras Context = new Entities_compras())
                {
                    com_ordencompra_local_resumen Entity = Context.com_ordencompra_local_resumen.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdOrdenCompra == IdOrdenCompra).FirstOrDefault();
                    if (Entity == null) return null;

                    info = new com_ordencompra_local_resumen_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdOrdenCompra = Entity.IdOrdenCompra,
                        SubtotalIVASinDscto = Entity.SubtotalIVASinDscto,
                        SubtotalSinIVASinDscto = Entity.SubtotalSinIVASinDscto,
                        SubtotalSinDscto = Entity.SubtotalSinDscto,
                        Descuento = Entity.Descuento,
                        SubtotalIVAConDscto = Entity.SubtotalIVAConDscto,
                        SubtotalSinIVAConDscto = Entity.SubtotalSinIVAConDscto,
                        SubtotalConDscto = Entity.SubtotalConDscto,
                        ValorIVA = Entity.ValorIVA,
                        Total = Entity.Total
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
