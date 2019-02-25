using Core.Erp.Data.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.Reportes.Facturacion
{
    public class FAC_002_Bus
    {
        FAC_002_Data odata = new FAC_002_Data();
    
        public List<FAC_002_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCliente, int Idtipo_cliente, DateTime fechaCorte, bool MostrarSoloCarteraVencida)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdCliente, Idtipo_cliente, fechaCorte, MostrarSoloCarteraVencida);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
