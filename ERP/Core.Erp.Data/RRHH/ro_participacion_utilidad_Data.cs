using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
  public  class ro_participacion_utilidad_Data
    {
        public List<ro_participacion_utilidad_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<ro_participacion_utilidad_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.vwro_participacion_utilidad
                                 where q.IdEmpresa == IdEmpresa
                                 select new ro_participacion_utilidad_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdUtilidad=q.IdUtilidad,
                                     IdPeriodo = q.IdPeriodo,
                                     UtilidadDerechoIndividual=q.UtilidadDerechoIndividual,
                                     UtilidadCargaFamiliar=q.UtilidadCargaFamiliar,
                                     pe_FechaIni=q.pe_FechaIni,
                                     pe_FechaFin=q.pe_FechaFin,
                                     Estado=q.Estado,

                                     EstadoBool = q.Estado == "A" ? true : false

                                 }).ToList();
                    else
                        Lista = (from q in Context.vwro_participacion_utilidad
                                 where q.IdEmpresa == IdEmpresa
                                 select new ro_participacion_utilidad_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdPeriodo = q.IdPeriodo,
                                     pe_FechaIni = q.pe_FechaIni,
                                     pe_FechaFin = q.pe_FechaFin,

                                     EstadoBool = q.Estado == "A" ? true : false
                                 }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_participacion_utilidad_Info get_info(int IdEmpresa, int IdPeriodo)
        {
            try
            {
                ro_participacion_utilidad_Info info = new ro_participacion_utilidad_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_participacion_utilidad Entity = Context.ro_participacion_utilidad.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdUtilidad == IdPeriodo);
                    if (Entity == null) return null;

                    info = new ro_participacion_utilidad_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdUtilidad = Entity.IdUtilidad,
                        IdPeriodo=Entity.IdPeriodo,
                        UtilidadCargaFamiliar=Entity.UtilidadCargaFamiliar,
                        UtilidadDerechoIndividual=Entity.UtilidadDerechoIndividual
                    };
                }

                return info;
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
                    var lst = from q in Context.ro_participacion_utilidad
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdUtilidad) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(ro_participacion_utilidad_Info info, ref int IdUtilidad)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_participacion_utilidad Entity = new ro_participacion_utilidad
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdUtilidad = info.IdUtilidad = get_id(info.IdEmpresa),
                        IdPeriodo = info.IdPeriodo,
                        FechaIngresa = DateTime.Now,
                        UsuarioIngresa=info.UsuarioIngresa,
                        Estado="A"
                    };
                    Context.ro_participacion_utilidad.Add(Entity);
                    Context.SaveChanges();
                    IdUtilidad = info.IdUtilidad;
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarDB(ro_participacion_utilidad_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_participacion_utilidad Entity = Context.ro_participacion_utilidad.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdUtilidad == info.IdUtilidad);
                    if (Entity == null)
                        return false;
                    Entity.IdUsuarioModifica = info.IdUsuarioModifica;
                    Entity.UtilidadDerechoIndividual = info.UtilidadDerechoIndividual;
                    Entity.UtilidadCargaFamiliar = info.UtilidadCargaFamiliar;
                    Entity.Fecha_ultima_modif = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool anularDB(ro_participacion_utilidad_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_participacion_utilidad Entity = Context.ro_participacion_utilidad.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdUtilidad == info.IdUtilidad);
                    if (Entity == null)
                        return false;
                    Entity.Estado  = "I";

                    Entity.IdUsuario_anula = info.IdUsuario_anula;
                    Entity.Fecha_anulacion  = DateTime.Now;
                    Context.SaveChanges();
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
