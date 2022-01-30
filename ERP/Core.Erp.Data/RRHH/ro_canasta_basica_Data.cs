using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
   public class ro_canasta_basica_Data
    {
        public List<ro_canasta_basica_Info> get_list()
        {
            try
            {
                List<ro_canasta_basica_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = Context.ro_canasta_basica.Select(q => new ro_canasta_basica_Info
                    {
                        Anio = q.Anio,
                        MultiploCanastaBasica = q.MultiploCanastaBasica,
                        MultiploFraccionBasica = q.MultiploFraccionBasica,
                        Observacion = q.Observacion,
                       valorCanasta=q.valorCanasta
                    }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_canasta_basica_Info get_info(int IdEmpresa, int Anio)
        {
            try
            {
                ro_canasta_basica_Info info = new ro_canasta_basica_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_canasta_basica Entity = Context.ro_canasta_basica.FirstOrDefault(q => q.Anio == Anio);
                    if (Entity == null) return null;

                    info = new ro_canasta_basica_Info
                    {
                        Anio = Entity.Anio,
                        MultiploCanastaBasica = Entity.MultiploCanastaBasica,
                        MultiploFraccionBasica = Entity.MultiploFraccionBasica,
                        Observacion = Entity.Observacion,
                        valorCanasta = Entity.valorCanasta
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
         public bool guardarDB(ro_canasta_basica_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_canasta_basica Entity = new ro_canasta_basica
                    {
                        Anio = info.Anio,
                        MultiploFraccionBasica = info.MultiploFraccionBasica,
                        MultiploCanastaBasica = info.MultiploCanastaBasica,
                        valorCanasta = info.valorCanasta,
                        Observacion = info.Observacion,
                        IdUsuario=info.IdUsuario,
                        FechaTansaccion=DateTime.Now
                    };
                    Context.ro_canasta_basica.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarDB(ro_canasta_basica_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_canasta_basica Entity = Context.ro_canasta_basica.FirstOrDefault(q => q.Anio == info.Anio);
                    if (Entity == null)
                        return false;
                    Entity.MultiploCanastaBasica = info.MultiploCanastaBasica;
                    Entity.MultiploFraccionBasica = info.MultiploFraccionBasica;
                    Entity.valorCanasta = info.valorCanasta;
                    Entity.Observacion = info.Observacion;
                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.FechaUltModi = info.FechaUltModi;

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
