using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.RRHH
{
    public class ro_biometrico_Data
    {
        public List<ro_biometrico_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<ro_biometrico_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.ro_biometrico
                                 where q.IdEmpresa == IdEmpresa
                                 select new ro_biometrico_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdBiometrico = q.IdBiometrico,
                                     IdEquipo = q.IdEquipo,
                                     Descripcion = q.Descripcion,
                                     StringConexion = q.StringConexion,
                                     Consulta = q.Consulta,
                                     MarcacionIngreso = q.MarcacionIngreso,
                                     MarcacionSalida = q.MarcacionSalida,
                                     SalidaLounch = q.SalidaLounch,
                                     RegresoLounch = q.RegresoLounch,
                                     CodMarcacionIngreso = q.CodMarcacionIngreso,
                                     CodMarcacionSalida = q.CodMarcacionSalida,
                                     CodSalidaLounch = q.CodSalidaLounch,
                                     CodRegresoLounch = q.CodRegresoLounch
                                 }).ToList();
                    else
                        Lista = (from q in Context.ro_biometrico
                                 where q.IdEmpresa == IdEmpresa
                                 && q.Estado == true
                                 select new ro_biometrico_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdBiometrico = q.IdBiometrico,
                                     IdEquipo = q.IdEquipo,
                                     Descripcion = q.Descripcion,
                                     StringConexion = q.StringConexion,
                                     Consulta = q.Consulta,
                                     MarcacionIngreso = q.MarcacionIngreso,
                                     MarcacionSalida = q.MarcacionSalida,
                                     SalidaLounch = q.SalidaLounch,
                                     RegresoLounch = q.RegresoLounch,
                                     CodMarcacionIngreso = q.CodMarcacionIngreso,
                                     CodMarcacionSalida = q.CodMarcacionSalida,
                                     CodSalidaLounch = q.CodSalidaLounch,
                                     CodRegresoLounch = q.CodRegresoLounch
                                 }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_biometrico_Info get_info(int IdEmpresa, int IdBiometrico)
        {
            try
            {
                ro_biometrico_Info info = new ro_biometrico_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_biometrico Entity = Context.ro_biometrico.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdBiometrico == IdBiometrico);
                    if (Entity == null) return null;

                    info = new ro_biometrico_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdBiometrico = Entity.IdBiometrico,
                        IdEquipo = Entity.IdEquipo,
                        Descripcion = Entity.Descripcion,
                        StringConexion = Entity.StringConexion,
                        Consulta = Entity.Consulta,
                        MarcacionIngreso = Entity.MarcacionIngreso,
                        MarcacionSalida = Entity.MarcacionSalida,
                        SalidaLounch = Entity.SalidaLounch,
                        RegresoLounch = Entity.RegresoLounch,
                        CodMarcacionIngreso = Entity.CodMarcacionIngreso,
                        CodMarcacionSalida = Entity.CodMarcacionSalida,
                        CodSalidaLounch = Entity.CodSalidaLounch,
                        CodRegresoLounch = Entity.CodRegresoLounch
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
                    var lst = from q in Context.ro_biometrico
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdBiometrico) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(ro_biometrico_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_biometrico Entity = new ro_biometrico
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdBiometrico = info.IdBiometrico = get_id(info.IdEmpresa),
                        IdEquipo = info.IdEquipo,
                        Descripcion = info.Descripcion,
                        StringConexion = info.StringConexion,
                        Consulta = info.Consulta,
                        MarcacionIngreso = info.MarcacionIngreso,
                        MarcacionSalida = info.MarcacionSalida,
                        SalidaLounch = info.SalidaLounch,
                        RegresoLounch = info.RegresoLounch,
                        CodMarcacionIngreso = info.CodMarcacionIngreso,
                        CodMarcacionSalida = info.CodMarcacionSalida,
                        CodSalidaLounch = info.CodSalidaLounch,
                        CodRegresoLounch = info.CodRegresoLounch,
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = info.Fecha_Transac = DateTime.Now
                    };
                    Context.ro_biometrico.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(ro_biometrico_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_biometrico Entity = Context.ro_biometrico.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdBiometrico == info.IdBiometrico);
                    if (Entity == null)
                        return false;
                    Entity.IdEquipo = info.IdEquipo;
                    Entity.Descripcion = info.Descripcion;
                    Entity.StringConexion = info.StringConexion;
                    Entity.Consulta = info.Consulta;
                    Entity.MarcacionIngreso = info.MarcacionIngreso;
                    Entity.MarcacionSalida = info.MarcacionSalida;
                    Entity.SalidaLounch = info.SalidaLounch;
                    Entity.RegresoLounch = info.RegresoLounch;
                    Entity.CodMarcacionIngreso = info.CodMarcacionIngreso;
                    Entity.CodMarcacionSalida = info.CodMarcacionSalida;
                    Entity.CodSalidaLounch = info.CodSalidaLounch;
                    Entity.CodRegresoLounch = info.CodRegresoLounch;
                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.Fecha_UltMod = info.Fecha_UltMod = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(ro_biometrico_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_biometrico Entity = Context.ro_biometrico.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdBiometrico == info.IdBiometrico);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = false;

                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    Entity.Fecha_UltAnu = info.Fecha_UltAnu = DateTime.Now;
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
