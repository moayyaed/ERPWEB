using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Inventario
{
    public class INV_021_Data
    {
        public List<INV_021_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, DateTime fecha_corte)
        {
            try
            {
                int IdSucursal_ini = IdSucursal;
                int IdSucursal_fin = IdSucursal == 0 ? 999999 : IdSucursal;
                int IdBodega_ini = IdBodega;
                int IdBodega_fin = IdBodega == 0 ? 999999 : IdBodega;

                List<INV_021_Info> Lista = null;
                using (Entities_reportes Context = new Entities_reportes())
                {

                    Lista = (from q in Context.SPINV_021(IdEmpresa, IdSucursal_ini, IdSucursal_fin, IdBodega_ini, IdBodega_fin, fecha_corte)
                             select new INV_021_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                IdSucursal = q.IdSucursal,
                                IdBodega = q.IdBodega,
                                Su_Descripcion = q.Su_Descripcion,
                                bo_Descripcion = q.bo_Descripcion,
                                cm_fecha = q.cm_fecha,
                                cm_observacion = q.cm_observacion,
                                CodMoviInven = q.CodMoviInven,
                                co_factura= q.co_factura,
                                co_FechaContabilizacion = q.co_FechaContabilizacion,
                                Desc_mov_inv = q.Desc_mov_inv,
                                Estado = q.Estado,
                                EstadoAprobacion = q.EstadoAprobacion,
                                IdEstadoAproba = q.IdEstadoAproba,
                                IdMovi_inven_tipo= q.IdMovi_inven_tipo,
                                IdMotivo_Inv=q.IdMotivo_Inv,
                                IdNumMovi=q.IdNumMovi,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                signo = q.signo,
                                tm_descripcion = q.tm_descripcion

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
