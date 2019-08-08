using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.Facturacion
{
    public class FAC_008_diario_Data
    {
        public List<FAC_008_diario_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                List<FAC_008_diario_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWFAC_008_diario
                             where q.no_IdEmpresa == IdEmpresa
                             && q.no_IdSucursal == IdSucursal
                             && q.no_IdBodega == IdBodega
                             && q.no_IdNota == IdNota
                             select new FAC_008_diario_Info
                             {
                                 no_IdEmpresa = q.no_IdEmpresa,
                                 no_IdNota = q.no_IdNota,
                                 no_IdBodega = q.no_IdBodega,
                                 no_IdSucursal = q.no_IdSucursal,
                                 cc_Descripcion = q.cc_Descripcion,
                                 ct_IdCbteCble = q.ct_IdCbteCble,
                                 ct_IdEmpresa = q.ct_IdEmpresa,
                                 ct_IdTipoCbte = q.ct_IdTipoCbte,
                                 dc_Valor = q.dc_Valor,
                                 dc_Valor_Debe = q.dc_Valor_Debe,
                                 dc_Valor_Haber = q.dc_Valor_Haber,
                                 IdCentroCosto = q.IdCentroCosto,
                                 IdCtaCble = q.IdCtaCble,
                                 IdPunto_cargo = q.IdPunto_cargo,
                                 IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
                                 nom_punto_cargo = q.nom_punto_cargo,
                                 nom_punto_cargo_grupo = q.nom_punto_cargo_grupo,
                                 pc_Cuenta = q.pc_Cuenta,
                                 secuencia = q.secuencia

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
