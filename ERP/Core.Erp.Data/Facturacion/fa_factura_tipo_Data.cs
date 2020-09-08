using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
    public class fa_factura_tipo_Data
    {
        public List<fa_factura_tipo_Info> GetList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<fa_factura_tipo_Info> Lista = new List<fa_factura_tipo_Info>();

                using (Entities_facturacion db = new Entities_facturacion())
                {
                    List<fa_factura_tipo> lst;
                    if (MostrarAnulados)
                        lst = db.fa_factura_tipo.Where(q => q.IdEmpresa == IdEmpresa).ToList();
                    else
                        lst = db.fa_factura_tipo.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == true).ToList();

                    foreach (var item in lst)
                    {
                        Lista.Add(new fa_factura_tipo_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdFacturaTipo = item.IdFacturaTipo,
                            Codigo = item.Codigo,
                            Descripcion = item.Descripcion,
                            Estado = item.Estado
                        });
                    }
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public fa_factura_tipo_Info GetInfo(int IdEmpresa, int IdFacturaTipo)
        {
            try
            {
                fa_factura_tipo_Info info = new fa_factura_tipo_Info();

                using (Entities_facturacion db = new Entities_facturacion())
                {
                    var Entity = db.fa_factura_tipo.Where(q => q.IdEmpresa == IdEmpresa && q.IdFacturaTipo == IdFacturaTipo).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new fa_factura_tipo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdFacturaTipo = Entity.IdFacturaTipo,
                        Codigo = Entity.Codigo,
                        Descripcion = Entity.Descripcion,
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

        private int GetId(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (Entities_facturacion db = new Entities_facturacion())
                {
                    var Cont = db.fa_factura_tipo.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (Cont > 0)
                        ID = db.fa_factura_tipo.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdFacturaTipo) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(fa_factura_tipo_Info info)
        {
            try
            {
                using (Entities_facturacion db = new Entities_facturacion())
                {
                    db.fa_factura_tipo.Add(new fa_factura_tipo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdFacturaTipo = info.IdFacturaTipo = GetId(info.IdEmpresa),
                        Codigo = info.Codigo,
                        Descripcion = info.Descripcion,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    });
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarDB(fa_factura_tipo_Info info)
        {
            try
            {
                using (Entities_facturacion db = new Entities_facturacion())
                {
                    var Entity = db.fa_factura_tipo.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdFacturaTipo == info.IdFacturaTipo).FirstOrDefault();
                    if (Entity == null)
                        return false;

                    Entity.Codigo = info.Codigo;
                    Entity.Descripcion = info.Descripcion;
                    Entity.IdUsuarioModificacion = info.IdUsuarioCreacion;
                    Entity.FechaModificacion = DateTime.Now;

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularDB(fa_factura_tipo_Info info)
        {
            try
            {
                using (Entities_facturacion db = new Entities_facturacion())
                {
                    var Entity = db.fa_factura_tipo.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdFacturaTipo == info.IdFacturaTipo).FirstOrDefault();
                    if (Entity == null)
                        return false;

                    Entity.Estado = false;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioCreacion;
                    Entity.FechaAnulacion = DateTime.Now;
                    Entity.MotivoAnulacion = info.MotivoAnulacion;

                    db.SaveChanges();
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
