using Core.Erp.Info.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Contabilidad
{
    public class ct_ClasificacionEBIT_Data
    {
        public List<ct_ClasificacionEBIT_Info> GetList()
        {
            try
            {
                List<ct_ClasificacionEBIT_Info> Lista = new List<ct_ClasificacionEBIT_Info>();

                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    Lista = db.ct_ClasificacionEBIT.Select(q => new ct_ClasificacionEBIT_Info
                    {
                        IdClasificacionEBIT = q.IdClasificacionEBIT,
                        ebit_Codigo = q.ebit_Codigo,
                        ebit_Descripcion = q.ebit_Descripcion,
                        AplicaEBIT =q.AplicaEBIT,
                        AplicaEBITDA = q.AplicaEBITDA

                    }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_ClasificacionEBIT_Info GetInfo(int IdClasificacionEBIT)
        {
            try
            {
                ct_ClasificacionEBIT_Info info = new ct_ClasificacionEBIT_Info();

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_ClasificacionEBIT Entity = Context.ct_ClasificacionEBIT.Where(q => q.IdClasificacionEBIT == IdClasificacionEBIT).FirstOrDefault();

                    if (Entity == null) return null;
                    info = new ct_ClasificacionEBIT_Info
                    {
                        IdClasificacionEBIT = Entity.IdClasificacionEBIT,
                        ebit_Codigo = Entity.ebit_Codigo,
                        ebit_Descripcion = Entity.ebit_Descripcion,
                        AplicaEBIT = Entity.AplicaEBIT,
                        AplicaEBITDA = Entity.AplicaEBITDA
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int get_id()
        {

            try
            {
                int ID = 1;
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    var Lista = db.ct_ClasificacionEBIT.Select(q => q.IdClasificacionEBIT);

                    if (Lista.Count() > 0)
                        ID = Convert.ToInt32(Lista.Max() + 1);
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool GuardarBD(ct_ClasificacionEBIT_Info info)
        {
            try
            {
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    db.ct_ClasificacionEBIT.Add(new ct_ClasificacionEBIT
                    {
                        IdClasificacionEBIT = get_id(),
                        ebit_Codigo = info.ebit_Codigo,
                        ebit_Descripcion = info.ebit_Descripcion,
                        AplicaEBIT = info.AplicaEBIT,
                        AplicaEBITDA = info.AplicaEBITDA
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

        public bool ModificarBD(ct_ClasificacionEBIT_Info info)
        {
            try
            {
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    ct_ClasificacionEBIT entity = db.ct_ClasificacionEBIT.Where(q => q.IdClasificacionEBIT == info.IdClasificacionEBIT).FirstOrDefault();

                    if (entity == null)
                    {
                        return false;
                    }

                    entity.ebit_Descripcion = info.ebit_Descripcion;
                    entity.ebit_Codigo = info.ebit_Codigo;
                    entity.AplicaEBIT = info.AplicaEBIT;
                    entity.AplicaEBITDA = info.AplicaEBITDA;

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularBD(ct_ClasificacionEBIT_Info info)
        {
            try
            {
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    ct_ClasificacionEBIT entity = db.ct_ClasificacionEBIT.Where(q => q.IdClasificacionEBIT == info.IdClasificacionEBIT).FirstOrDefault();

                    if (entity == null)
                    {
                        return false;
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
