using Core.Erp.Info.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Contabilidad
{
    public class ct_periodo_x_tb_modulo_Data
    {
        public List<ct_periodo_x_tb_modulo_Info> GetList(int IdEmpresa, int IdPeriodo)
        {
            try
            {
                var Secuencia = 1;
            
                List<ct_periodo_x_tb_modulo_Info> Lista = new List<ct_periodo_x_tb_modulo_Info>();
                int IdPeriodoIni = IdPeriodo;
                int IdPeriodoFin = IdPeriodo == 0 ? 999999999 : IdPeriodo;

                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    Lista = db.ct_periodo_x_tb_modulo.Where(
                        q=> q.IdEmpresa == IdEmpresa
                        && q.IdPeriodo >= IdPeriodoIni
                        && q.IdPeriodo <= IdPeriodoIni
                        ).Select(q => new ct_periodo_x_tb_modulo_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdPeriodo = q.IdPeriodo,
                            IdModulo = q.IdModulo,
                            Cerrado = q.Cerrado,
                            IdUsuario = q.IdUsuario,
                            IdUsuarioUltModi = q.IdUsuarioUltModi,
                            FechaTransac = q.FechaTransac,
                            FechaUltModi = q.FechaUltModi
                        }).ToList();
                }

                Lista.ForEach(q=> q.Secuencia = Secuencia++);
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_periodo_x_tb_modulo_Info GetInfo(int IdEmpresa, int IdPeriodo, string IdModulo)
        {
            try
            {
                ct_periodo_x_tb_modulo_Info info = new ct_periodo_x_tb_modulo_Info();

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_periodo_x_tb_modulo Entity = Context.ct_periodo_x_tb_modulo.Where(q => q.IdEmpresa == IdEmpresa && q.IdPeriodo == IdPeriodo && q.IdModulo == IdModulo).FirstOrDefault();

                    if (Entity == null) return null;
                    info = new ct_periodo_x_tb_modulo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdPeriodo = Entity.IdPeriodo,
                        IdModulo = Entity.IdModulo,
                        Cerrado = Entity.Cerrado
                        
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarBD(ct_periodo_x_tb_modulo_Info info)
        {
            try
            {
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    db.ct_periodo_x_tb_modulo.Add(new ct_periodo_x_tb_modulo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdPeriodo = info.IdPeriodo,
                        IdModulo = info.IdModulo,
                        Cerrado = info.Cerrado,
                        IdUsuario = info.IdUsuario,
                        FechaTransac = DateTime.Now
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

        public bool ModificarBD(ct_periodo_x_tb_modulo_Info info)
        {
            try
            {
                using (Entities_contabilidad db = new Entities_contabilidad())
                {
                    ct_periodo_x_tb_modulo entity = db.ct_periodo_x_tb_modulo.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdPeriodo == info.IdPeriodo && q.IdModulo == info.IdModulo).FirstOrDefault();

                    if (entity == null)
                    {
                        return false;
                    }

                    entity.IdEmpresa = info.IdEmpresa;
                    entity.IdPeriodo = info.IdPeriodo;
                    entity.IdModulo = info.IdModulo;
                    entity.Cerrado = info.Cerrado;
                    entity.IdUsuarioUltModi = info.IdUsuarioUltModi;
                    entity.FechaUltModi = DateTime.Now;

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
