using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;

namespace Core.Erp.Data.RRHH
{
   public class ro_rol_detalle_Data
    {


        public List<ro_rol_detalle_Info> Get_lst_detalle_contabilizar(int idEmpresa, int idNominaTipo, int idNominaTipoLiqui, int idPeriodo,int IdRol, bool es_provision)
        {

            try
            {
                List<ro_rol_detalle_Info> oListado = new List<ro_rol_detalle_Info>();

                using (Entities_rrhh db = new Entities_rrhh())
                {
                    oListado = (from a in db.vwro_rol_detalle
                                
                                where a.IdEmpresa == idEmpresa
                                 && a.IdNominaTipo == idNominaTipo
                                 && a.IdNominaTipoLiqui == idNominaTipoLiqui
                                 && a.IdPeriodo == idPeriodo
                                 && a.IdRol == IdRol
                                 && a.Valor > 0
                                 && a.rub_provision== es_provision
                                select new ro_rol_detalle_Info
                                 {
                                     IdEmpresa = a.IdEmpresa,
                                     IdNominaTipo = a.IdNominaTipo,
                                     IdNominaTipoLiqui = a.IdNominaTipoLiqui,
                                     IdPeriodo = a.IdPeriodo,
                                     IdEmpleado = a.IdEmpleado,
                                     IdRubro = a.IdRubro,
                                     Valor = a.Valor,
                                     IdDivision = a.IdDivision,
                                     IdArea = a.IdArea,
                                     IdDepartamento = a.IdDepartamento,
                                     ru_tipo=a.ru_tipo,
                                     rub_ContPorEmpleado=a.rub_ContPorEmpleado  ,
                                     IdCtaCble_Emplea=a.IdCtaCble_Emplea,
                                    IdCtaCble_Debe = a.rub_ctacon,
                                    ru_descripcion=a.ru_descripcion,
                                    pc_CuentaEmple = a.pc_CuentaEmple,
                                    pc_CuentaRubro = a.pc_CuentaRubro
                                    
                                }).ToList();
                }
                return oListado;
            }
            catch (Exception )
            {
               
                throw ;
            }
        }
     
        public List<ro_rol_detalle_Info> get_list_nomina_sin_sueldo_percibir(int idEmpresa, decimal IdRol)
        {

            try
            {
                List<ro_rol_detalle_Info> oListado = new List<ro_rol_detalle_Info>();

                using (Entities_rrhh db = new Entities_rrhh())
                {
                    oListado = (from a in db.vwro_nomina_sin_percebir_sueldo
                                where a.IdEmpresa == idEmpresa
                                && a.IdRol == IdRol
                              
                                select new ro_rol_detalle_Info
                                {
                                    IdEmpresa=a.IdEmpresa,
                                    IdEmpleado=a.IdEmpleado,
                                    IdRol=a.IdRol,
                                    pe_nombreCompleato = a.pe_nombreCompleto,
                                    Valor_=a.Valor
                                }).ToList();
                }
                return oListado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ro_rol_detalle_Info> get_list_ajustar_anticipo(int IdEmpresa, int IdSucursal, int IdNominaTipo, int IdNominaTipoLiqui, int IdPeriodo)
        {
            try
            {
                List<ro_rol_detalle_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.vwRo_rol_detalle_saldo_por_pagar
                             where q.IdEmpresa == IdEmpresa
                                   && q.IdEmpresa == IdEmpresa
                                   && q.IdNominaTipo == IdNominaTipo
                                   && q.IdNominaTipoLiqui == IdNominaTipoLiqui
                                   && q.IdPeriodo == IdPeriodo
                                   && q.IdSucursal==IdSucursal
                                   && q.Saldo > 0
                             select new ro_rol_detalle_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdEmpleado = q.IdEmpleado,
                                 pe_nombreCompleato = q.pe_nombreCompleto,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 Valor = q.ValorGanado,
                                 IdRol=q.IdRol,
                                 IdRubro=q.IdRubro
                               
                             }).ToList();

                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool ajustar_anticipo(List<ro_rol_detalle_Info> lista)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    foreach (var item in lista)
                    {
                        ro_rol_detalle Entity = Context.ro_rol_detalle.FirstOrDefault(q => q.IdEmpresa == item.IdEmpresa && q.IdEmpleado == item.IdEmpleado 
                        && q.IdRol == item.IdRol && q.IdRubro==item.IdRubro);
                        if(Entity!=null)
                        Entity.Valor = item.Valor;
                        Context.SaveChanges();

                    }
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
