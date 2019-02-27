using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.RRHH
{
    public class ROL_025_Data
    {
        public List<ROL_021_Info> get_list(int IdEmpresa, int IdSucursal, int IdNominaTipo, int IdPeriodo)
        {
            try
            {
                int IdSucursalInicio = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;


                List<ROL_025_Info> Lista = new List<ROL_025_Info>();
                using (Entities_reportes Context = new Entities_reportes())
                {                                     
                        Lista = (from q in Context.SPROL_025
                                 where q.IdEmpresa == IdEmpresa
                                 && q.IdPeriodo == IdPeriodo
                                 && IdSucursalInicio <= q.IdSucursal && q.IdSucursal <= IdSucursalFin
                                 && q.IdNominaTipoLiqui == IdNominaTipo
                                 select new ROL_025_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdRol = q.IdRol,
                                     IdSucursal = q.IdSucursal,
                                     IdNominaTipo = q.IdNominaTipo,
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
