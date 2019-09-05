using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Contabilidad
{
    public class Dinardap_Data
    {
        ct_periodo_Data data_perido = new ct_periodo_Data();
        public List<CXC_010_Info> get_info(int IdEmpresa, int IdPeriodo, int IdSucursal)
        {
            try
            {
                var perido_info = data_perido.get_info(IdEmpresa, IdPeriodo);
                int IdSucursalInicio = Convert.ToInt32(IdSucursal);
                int IdSucursalFin = Convert.ToInt32(IdSucursal) == 0 ? 9999 : Convert.ToInt32(IdSucursal);
                List<CXC_010_Info> Lista = new List<CXC_010_Info>();

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {

                    Lista = Context.GenerarDINARDAP(IdEmpresa, IdSucursalInicio, IdSucursalFin, perido_info.pe_FechaIni, perido_info.pe_FechaFin).Select(q => new CXC_010_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCliente = q.IdCliente,
                        Codigo = q.Codigo,
                        IdCbteVta = q.IdCbteVta,
                        CodCbteVta = q.CodCbteVta,
                        vt_fecha = q.vt_fecha,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_serie1 = q.vt_serie1,
                        vt_serie2 = q.vt_serie2,
                        vt_tipoDoc = q.vt_tipoDoc,
                        Su_Descripcion = q.Su_Descripcion,
                        pe_cedulaRuc = q.pe_cedulaRuc,
                        pe_telefonoOfic = q.pe_telefonoOfic,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        Dias_Vencidos = q.Dias_Vencidos,
                        Idtipo_cliente = q.Idtipo_cliente,
                        Total_Pagado = q.Total_Pagado
                    }).ToList();

                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
