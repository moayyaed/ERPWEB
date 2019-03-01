using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
  public  class ro_Config_Param_contable_Data
    {
        public List< ro_Config_Param_contable_Info> get_list(int IdEmpresa)
        {
            try
            {
                List<ro_Config_Param_contable_Info> Lista = new List<ro_Config_Param_contable_Info>();
                int secuencia = 1;
                using (Entities_rrhh Context = new Entities_rrhh())
                {

                    string sql = " select IdEmpresa,IdDivision,IdArea,IdDepartamento,IdRubro,IdCtaCble,IdCentroCosto,DebCre,IdCtaCble_Haber IdCtaCble_prov_credito,0 as Secuencia,rub_nocontab,rub_provision,DescripcionDiv,descripcionArea,de_descripcion,ru_descripcion,pc_Cuenta_prov_debito,pc_Cuenta, ru_tipo from vwRo_Division_Area_dep_rubro where IdEmpresa=" + IdEmpresa;
                    var result = Context.Database.SqlQuery<ro_Config_Param_contable_Info>(sql).ToList();
                    Lista = result;
                    Lista.ForEach(v =>
                    {
                        v.Secuencia = secuencia++;
                        if (v.IdCtaCble == null | v.IdCtaCble == "")
                            v.IdCtaCble = v.rub_ctacon;
                        v.IdCtaCble_prov_debito = v.IdCtaCble;
                        
                        v.pc_Cuenta_prov_credito = v.pc_Cuenta;
                    });

                }


                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ro_Config_Param_contable_Info> get_list(int IdEmpresa, string es_provision)
        {
            try
            {
                List<ro_Config_Param_contable_Info> Lista = new List<ro_Config_Param_contable_Info>();
                int secuencia = 1;
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                      
                    string sql = " select IdEmpresa,IdRubro,IdCtaCble,IdCtaCble_Haber, ru_descripcion,ru_tipo,IdDepartamento, IdArea, IdDivision,DescripcionArea,de_descripcion,rub_ContPorEmpleado,pc_Cuenta_prov_debito,pc_Cuenta " +
                        "from vwRo_Division_Area_dep_rubro where IdEmpresa='" + IdEmpresa + "' and rub_provision='" + es_provision + "' and rub_nocontab='" + 1 + "' group by IdEmpresa,IdRubro,IdCtaCble, ru_descripcion,IdCtaCble_Haber,ru_tipo ,IdDepartamento, IdArea, IdDivision,DescripcionArea,de_descripcion,rub_ContPorEmpleado,pc_Cuenta_prov_debito,pc_Cuenta";
                    var result = Context.Database.SqlQuery<ro_Config_Param_contable_Info>(sql).ToList();
                    Lista = result;
                    Lista.ForEach(v =>
                    {
                        v.Secuencia = secuencia++;
                        if (v.IdCtaCble == null | v.IdCtaCble == "")
                            v.IdCtaCble = v.rub_ctacon;
                        v.IdCtaCble_prov_debito = v.IdCtaCble;

                        v.pc_Cuenta_prov_credito = v.pc_Cuenta;
                    });

                }


                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ro_Config_Param_contable_Info get_info(int IdEmpresa, int IdDivision, int IdArea, int IdDepartamento, string IdRubro)
        {
            try
            {
                 ro_Config_Param_contable_Info info = new  ro_Config_Param_contable_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                     ro_Config_Param_contable Entity = Context. ro_Config_Param_contable.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdDivision == IdDivision && q.IdArea == IdArea && q.IdDepartamento == IdDepartamento&& q.IdRubro==IdRubro);
                    if (Entity == null) return null;

                    info = new  ro_Config_Param_contable_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdDivision = Entity.IdDivision,
                        IdArea = Entity.IdArea,
                        IdDepartamento = Entity.IdDepartamento,
                        IdRubro = Entity.IdRubro,
                        IdCentroCosto = Entity.IdCentroCosto,
                        DebCre = Entity.DebCre,
                        IdCtaCble = Entity.IdCtaCble,
                        IdCtaCble_prov_credito = Entity.IdCtaCble_Haber
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(List< ro_Config_Param_contable_Info> info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    foreach (var item in info)
                    {
                       
                        if (item.IdCtaCble == "")
                            item.IdCtaCble = null;
                        if (item.IdCtaCble_prov_debito == "")
                            item.IdCtaCble_prov_debito = null;
                        if (item.rub_provision == false)
                        {
                            ro_Config_Param_contable Entity = new ro_Config_Param_contable
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdDivision = item.IdDivision,
                                IdArea = item.IdArea,
                                IdDepartamento = item.IdDepartamento,
                                IdRubro = item.IdRubro,
                                IdCentroCosto = item.IdCentroCosto,
                                DebCre = item.DebCre,
                                IdCtaCble_Haber = item.IdCtaCble
                            };
                            Context.ro_Config_Param_contable.Add(Entity);
                        }
                        else
                        {
                            ro_Config_Param_contable Entity_ = new ro_Config_Param_contable
                            {
                                IdEmpresa = item.IdEmpresa,
                                IdDivision = item.IdDivision,
                                IdArea = item.IdArea,
                                IdDepartamento = item.IdDepartamento,
                                IdRubro = item.IdRubro,
                                IdCentroCosto = item.IdCentroCosto,
                                DebCre = item.DebCre,
                                IdCtaCble = item.IdCtaCble_prov_debito,
                                IdCtaCble_Haber=item.IdCtaCble
                            };
                            Context.ro_Config_Param_contable.Add(Entity_);
                        }
                    }
                    Context.SaveChanges();

                }
                return true;
            }
            catch (Exception )
            {

                throw;
            }
        }
        public bool eliminarDB(int IdEmpresa)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    string sql = "delete  ro_Config_Param_contable where IdEmpresa='" + IdEmpresa + "'";
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
