using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
    public class fa_ProbabilidadCobroDet_Data
    {
        public List<fa_ProbabilidadCobroDet_Info> get_list(int IdEmpresa, int IdProbabilidad)
        {
            try
            {
                List<fa_ProbabilidadCobroDet_Info> Lista;

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = Context.fa_ProbabilidadCobroDet.Where(q => q.IdEmpresa == IdEmpresa && q.IdProbabilidad == IdProbabilidad).Select(q => new fa_ProbabilidadCobroDet_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdProbabilidad = q.IdProbabilidad,
                        Secuencia = q.Secuencia,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCbteVta = q.IdCbteVta,
                        vt_tipoDoc = q.vt_tipoDoc
                    }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool eliminarDB(fa_ProbabilidadCobroDet_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var sql = "delete from fa_ProbabilidadCobroDet where IdEmpresa =" + info.IdEmpresa + " and IdProbabilidad = " + info.IdProbabilidad + " and Secuencia = " + info.Secuencia;
                    Context.Database.ExecuteSqlCommand(sql);
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
