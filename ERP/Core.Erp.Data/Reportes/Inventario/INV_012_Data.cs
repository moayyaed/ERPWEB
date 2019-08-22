using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.Inventario
{
   public class INV_012_Data
    {
        public List<INV_012_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, string tipo_movi, int IdMovi_inven_tipo, decimal IdNumMovi, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

                int IdBodegaIni = IdBodega;
                int IdBodegaFin = IdBodega == 0 ? 9999 : IdBodega;

                int IdMovi_inven_tipoIni = IdMovi_inven_tipo;
                int IdMovi_inven_tipoFin = IdMovi_inven_tipo == 0 ? 999999 : IdMovi_inven_tipo;

                List<INV_012_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.VWINV_012.Where(q =>
                        q.IdEmpresa == q.IdEmpresa
                        && IdSucursalIni <= q.IdSucursal
                        && q.IdSucursal <= IdSucursalFin
                        && IdBodegaIni <= q.IdBodega
                        && q.IdBodega <= IdBodegaFin
                        && q.cm_fecha >= FechaIni
                        && q.cm_fecha <= FechaFin                        
                        && q.cm_tipo_movi == (string.IsNullOrEmpty(tipo_movi) ? q.cm_tipo_movi : tipo_movi)
                        && IdMovi_inven_tipoIni <= q.IdMovi_inven_tipo
                        && q.IdMovi_inven_tipo <= IdMovi_inven_tipoFin
                        && q.IdNumMovi == ((IdNumMovi == 0) ? q.IdNumMovi : IdNumMovi)
                        ).Select(q => new INV_012_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdNumMovi = q.IdNumMovi,
                            bo_Descripcion = q.bo_Descripcion,
                            cc_Descripcion = q.cc_Descripcion,
                            cm_fecha = q.cm_fecha,
                            cm_observacion = q.cm_observacion,
                            cm_tipo_movi = q.cm_tipo_movi,
                            CodMoviInven = q.CodMoviInven,
                            CostoTotal = q.CostoTotal,
                            dm_cantidad_sinConversion = q.dm_cantidad_sinConversion,
                            IdBodega = q.IdBodega,
                            IdEstadoAproba = q.IdEstadoAproba,
                            IdMovi_inven_tipo = q.IdMovi_inven_tipo,
                            IdProducto = q.IdProducto,
                            IdSucursal = q.IdSucursal,
                            MotivoCabecera = q.MotivoCabecera,
                            MotivoDetalle = q.MotivoDetalle,
                            mv_costo_sinConversion = q.mv_costo_sinConversion,
                            NomUnidad = q.NomUnidad,
                            pr_codigo = q.pr_codigo,
                            pr_descripcion = q.pr_descripcion,
                            Secuencia = q.Secuencia,
                            Su_Descripcion = q.Su_Descripcion,
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
