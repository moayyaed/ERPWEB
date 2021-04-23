using Core.Erp.Data;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
    public class tb_TarjetaCredito_Data
    {
        public List<tb_TarjetaCredito_Info> getList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<tb_TarjetaCredito_Info> Lista = new List<tb_TarjetaCredito_Info>();

                using (Entities_general odata = new Entities_general())
                {
                    //q.IdEmpresa ==IdEmpresa && 
                    var lst = odata.tb_TarjetaCredito.Where(q => q.Estado == (MostrarAnulados ? q.Estado : true)).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new tb_TarjetaCredito_Info
                        {
                            //IdEmpresa = q.IdEmpresa,
                            IdTarjeta = q.IdTarjeta,
                            NombreTarjeta =q.NombreTarjeta,
                            Estado = q.Estado
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_TarjetaCredito_Info getInfo(int IdEmpresa, int IdTarjeta)
        {
            try
            {
                tb_TarjetaCredito_Info info;

                using (Entities_general db = new Entities_general())
                {
                    //q.IdEmpresa== IdEmpresa && 
                    var Entity = db.tb_TarjetaCredito.Where(q =>q.IdTarjeta == IdTarjeta).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new tb_TarjetaCredito_Info
                    {
                        //IdEmpresa = Entity.IdEmpresa,
                        IdTarjeta = Entity.IdTarjeta,
                        NombreTarjeta = Entity.NombreTarjeta,
                        //IdBanco = Entity.IdBanco,
                        Estado = Entity.Estado
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int getId(int IdEmpresa)
        {
            try
            {
                int ID = 1;
                //.Where(q => q.IdEmpresa == IdEmpresa)
                using (Entities_general Context = new Entities_general())
                {
                    var cont = Context.tb_TarjetaCredito.Count();
                    if (cont > 0)
                        ID = Context.tb_TarjetaCredito.Max(q => q.IdTarjeta) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(tb_TarjetaCredito_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    tb_TarjetaCredito Entity = new tb_TarjetaCredito
                    {
                        //IdEmpresa = info.IdEmpresa,
                        IdTarjeta = info.IdTarjeta = getId(info.IdEmpresa),
                        NombreTarjeta = info.NombreTarjeta,
                        //IdBanco = info.IdBanco,
                        Estado = true,
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = info.Fecha_Transac = DateTime.Now
                    };
                    Context.tb_TarjetaCredito.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(tb_TarjetaCredito_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    //q.IdEmpresa== info.IdEmpresa &&
                    tb_TarjetaCredito Entity = Context.tb_TarjetaCredito.FirstOrDefault(q => q.IdTarjeta == info.IdTarjeta);
                    if (Entity == null)
                        return false;

                    Entity.NombreTarjeta = info.NombreTarjeta;
                    //Entity.IdBanco = info.IdBanco;
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

        public bool anularDB(tb_TarjetaCredito_Info info)
        {
            try
            {
                using (Entities_general Context = new Entities_general())
                {
                    // q.IdEmpresa == info.IdEmpresa &&
                    tb_TarjetaCredito Entity = Context.tb_TarjetaCredito.FirstOrDefault(q => q.IdTarjeta == info.IdTarjeta);
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
