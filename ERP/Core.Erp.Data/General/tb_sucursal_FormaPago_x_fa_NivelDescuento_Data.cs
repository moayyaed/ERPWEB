using Core.Erp.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.General
{
    public class tb_sucursal_FormaPago_x_fa_NivelDescuento_Data
    {
        public List<tb_sucursal_FormaPago_x_fa_NivelDescuento_Info> get_list(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<tb_sucursal_FormaPago_x_fa_NivelDescuento_Info> Lista;

                using (Entities_general db = new Entities_general())
                {
                    Lista = db.tb_sucursal_FormaPago_x_fa_NivelDescuento.Where(q => q.IdEmpresa == IdEmpresa).Select(q => new tb_sucursal_FormaPago_x_fa_NivelDescuento_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        Secuencia = q.Secuencia,
                        IdCatalogo = q.IdCatalogo,
                        IdNivel = q.IdNivel

                    }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_sucursal_FormaPago_x_fa_NivelDescuento_Info get_info(int IdEmpresa, int IdSucursal, int Secuencia)
        {
            try
            {
                tb_sucursal_FormaPago_x_fa_NivelDescuento_Info info = new tb_sucursal_FormaPago_x_fa_NivelDescuento_Info();
                using (Entities_general Context = new Entities_general())
                {
                    tb_sucursal_FormaPago_x_fa_NivelDescuento Entity = Context.tb_sucursal_FormaPago_x_fa_NivelDescuento.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.Secuencia == Secuencia).FirstOrDefault();

                    if (Entity == null) return null;
                    info = new tb_sucursal_FormaPago_x_fa_NivelDescuento_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        Secuencia = Entity.Secuencia,
                        IdCatalogo = Entity.IdCatalogo,
                        IdNivel = Entity.IdNivel
                    };
                }

                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarBD(tb_sucursal_FormaPago_x_fa_NivelDescuento_Info info)
        {
            try
            {
                using (Entities_general db = new Entities_general())
                {
                    if (info.ListaNivelDescuento != null)
                    {
                        int Secuencia = 1;
                        foreach (var item in info.ListaNivelDescuento)
                        {
                            db.tb_sucursal_FormaPago_x_fa_NivelDescuento.Add(new tb_sucursal_FormaPago_x_fa_NivelDescuento
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdSucursal = info.IdSucursal,
                                Secuencia = Secuencia++,
                                IdCatalogo = item.IdCatalogo,
                                IdNivel = item.IdNivel
                            });

                        }
                    }
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModificarBD(tb_sucursal_FormaPago_x_fa_NivelDescuento_Info info)
        {
            try
            {
                using (Entities_general db = new Entities_general())
                {
                    var lst_det_grupo = db.tb_sucursal_FormaPago_x_fa_NivelDescuento.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal).ToList();
                    db.tb_sucursal_FormaPago_x_fa_NivelDescuento.RemoveRange(lst_det_grupo);

                    if (info.ListaNivelDescuento != null)
                    {
                        int Secuencia = 1;

                        foreach (var item in info.ListaNivelDescuento)
                        {
                            db.tb_sucursal_FormaPago_x_fa_NivelDescuento.Add(new tb_sucursal_FormaPago_x_fa_NivelDescuento
                            {
                                IdEmpresa = info.IdEmpresa,
                                IdSucursal = info.IdSucursal,
                                Secuencia = Secuencia++,
                                IdCatalogo = item.IdCatalogo,
                                IdNivel = item.IdNivel
                            });
                        }
                    }
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
