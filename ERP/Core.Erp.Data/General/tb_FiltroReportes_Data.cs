using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.General
{
    public class tb_FiltroReportes_Data
    {
        public bool GuardarDB(int IdEmpresa, int[] IdSucursal, string IdUsuario)
        {
            try
            {
                using (Entities_general db = new Entities_general())
                {
                    var ls = db.tb_FiltroReportes.Where(q => q.IdUsuario == IdUsuario).ToList();
                    db.tb_FiltroReportes.RemoveRange(ls);
                    if (IdSucursal != null)
                    {
                        foreach (var item in IdSucursal)
                        {
                            db.tb_FiltroReportes.Add(new tb_FiltroReportes
                            {
                                IdEmpresa = IdEmpresa,
                                IdSucursal = item,
                                IdUsuario = IdUsuario,
                                Fecha = DateTime.Now
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
