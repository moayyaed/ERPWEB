using Core.Erp.Data.Reportes.Base;
using Core.Erp.Data.RRHH;
using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.RRHH
{
    public class ROL_030_Data
    {
        public List<ROL_030_Info> get_list(int IdEmpresa, int IdSucursal, int IdNominaTipo, int IdNominaTipoLiqui, int IdPeriodo)
        {
            try
            {
                ro_Parametros_Data odata = new ro_Parametros_Data();
                var para = odata.get_info(IdEmpresa);
                List<ROL_030_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWROL_030
                             where (q.IdEmpresa == IdEmpresa
                             && q.IdNominaTipo== para.IdNomina_General
                             && q.IdNominaTipoLiqui== para.IdNomina_TipoLiqui_PagoUtilidad
                             && q.IdPeriodo==IdPeriodo
                            )
                             select new ROL_030_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdRol = q.IdRol,
                                 IdSucursal = q.IdSucursal,
                                 IdNominaTipo = q.IdNominaTipo,
                                 IdNominaTipoLiqui = q.IdNominaTipoLiqui,
                                 IdPeriodo = q.IdPeriodo,
                                 IdEmpleado = q.IdEmpleado,
                                 IdRubro = q.IdRubro,
                                 ca_descripcion = q.ca_descripcion,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 rub_codigo = q.rub_codigo,
                                 ru_codRolGen = q.ru_codRolGen,
                                 ru_orden = q.ru_orden,
                                 ru_descripcion = q.ru_descripcion,
                                 Orden = q.Orden,
                                 Valor = q.Valor


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
