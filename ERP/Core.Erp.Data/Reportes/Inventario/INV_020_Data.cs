using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Inventario
{
    public class INV_020_Data
    {
        public List<INV_020_Info> get_list(int IdEmpresa, int idSucursal, int IdMovi_inven_tipo, decimal IdNumMovi)
        {
            try
            {
                List<INV_020_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWINV_020
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == idSucursal
                             && q.IdMovi_inven_tipo == IdMovi_inven_tipo
                             && q.IdNumMovi == IdNumMovi
                             select new INV_020_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdMovi_inven_tipo = q.IdMovi_inven_tipo,
                                 IdNumMovi = q.IdNumMovi,
                                 Secuencia = q.Secuencia,
                                 IdProducto = q.IdProducto,
                                 pr_descripcion = q.pr_descripcion,
                                 pr_codigo = q.pr_codigo,
                                 Su_Descripcion = q.Su_Descripcion,
                                 bo_Descripcion = q.bo_Descripcion,
                                 IdUnidadMedida_sinConversion = q.IdUnidadMedida_sinConversion,
                                 Descripcion = q.Descripcion,
                                 dm_cantidad_sinConversion = q.dm_cantidad_sinConversion,
                                 cm_observacion = q.cm_observacion,
                                 CodMoviInven = q.CodMoviInven,
                                 cm_fecha = q.cm_fecha,
                                 Estado = q.Estado,
                                 IdMotivo_Inv = q.IdMotivo_Inv,
                                 Desc_mov_inv = q.Desc_mov_inv,
                                 lote_num_lote = q.lote_num_lote,
                                 lote_fecha_vcto = q.lote_fecha_vcto,
                                 nom_presentacion = q.nom_presentacion,
                                 signo = q.signo,
                                 tm_descripcion = q.tm_descripcion,
                                 NomUsuario = q.NomUsuario,
                                 IdResponsable = q.IdResponsable,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 FechaDespacho = q.FechaDespacho,
                                 Fecha_Transac = q.Fecha_Transac,
                                 FechaAR = q.FechaAR,
                                 IdUsuarioDespacho = q.IdUsuarioDespacho,
                                 UsuarioDespacho = q.UsuarioDespacho
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
