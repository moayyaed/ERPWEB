using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
   public class ro_area_x_departamento_Data
    {
        public List<ro_area_x_departamento_Info> get_list(int IdEmpresa)
        {
            try
            {
                List<ro_area_x_departamento_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = Context.vwro_area_x_departamento.Where(q => q.IdEmpresa == IdEmpresa).Select(q => new ro_area_x_departamento_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        Secuencia = q.Secuencia,
                        IdDivision = q.IdDivision,
                        IdArea = q.IdArea,
                        IdDepartamento = q.IdDepartamento,
                        area = q.AreaDescripcion,
                        division = q.DivisionDescripcion,
                        departamento = q.DepartamentoDescripcion
                    }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_area_x_departamento_Info get_info(int IdEmpresa, int Secuencia)
        {
            try
            {
                ro_area_x_departamento_Info info = new ro_area_x_departamento_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_area_x_departamento q = Context.ro_area_x_departamento.FirstOrDefault(v => v.IdEmpresa == IdEmpresa && v.Secuencia==Secuencia);
                    if (q == null) return null;

                    info = new ro_area_x_departamento_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        Secuencia=q.Secuencia,
                        IdDivision = q.IdDivision,
                        IdArea = q.IdArea,
                        IdDepartamento = q.IdDepartamento                       
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool si_existe(int IdEmpresa, int IdDivision, int IdArea, int IdDepartaento)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    var lst = from q in Context.ro_area_x_departamento
                              where q.IdEmpresa == IdEmpresa
                              && q.IdDivision==IdDivision
                              && q.IdArea==IdArea
                              && q.IdDepartamento==IdDepartaento
                              select q;

                    if (lst.Count() > 0)
                        return true;
                    else
                        return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(ro_area_x_departamento_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_area_x_departamento Entity = new ro_area_x_departamento
                    {
                        IdEmpresa = info.IdEmpresa,
                        Secuencia =info.Secuencia= get_id(info.IdEmpresa),
                        IdDivision = info.IdDivision,
                        IdArea = info.IdArea,
                        IdDepartamento = info.IdDepartamento
                    };
                    Context.ro_area_x_departamento.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool modificarDB(ro_area_x_departamento_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_area_x_departamento Entity = Context.ro_area_x_departamento.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa&& q.Secuencia==info.Secuencia);
                    if (Entity == null)
                        return false;
                    Entity.IdEmpresa = info.IdEmpresa;
                    Entity.IdDivision = info.IdDivision;
                    Entity.IdArea = info.IdArea;
                    Entity.IdDepartamento = info.IdDepartamento;    
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool AnularDB(ro_area_x_departamento_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_area_x_departamento Entity = Context.ro_area_x_departamento.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.Secuencia == info.Secuencia);
                    if (Entity == null)
                        return false;
                    Context.ro_area_x_departamento.Remove(Entity);
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int get_id(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    var lst = from q in Context.ro_area_x_departamento
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.Secuencia) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
