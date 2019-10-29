using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_011_Data
    {
        public List<CONTA_011_Info> get_list(int IdEmpresa, string IdUsuario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<CONTA_011_Info> Lista = new List<CONTA_011_Info>();
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.SPCONTA_011(IdEmpresa, IdUsuario, fechaIni, fechaFin)
                             select new CONTA_011_Info
                             {
                                 IdUsuario = q.IdUsuario,
                                 IdEmpresa = q.IdEmpresa,
                                 Secuencia =q.Secuencia,
                                 Grupo = q.Grupo,
                                 Descripcion =q.Descripcion,
                                 Columna1 = q.Columna1,
                                 Columna2 = q.Columna2,
                                 Columna3 = q.Columna3,
                                 Columna4 = q.Columna4,
                                 Columna5 = q.Columna5,
                                 Columna6 = q.Columna6,
                                 Columna7 = q.Columna7,
                                 NombreC1 = q.NombreC1,
                                 NombreC2 = q.NombreC2,
                                 NombreC3 = q.NombreC3,
                                 NombreC4 = q.NombreC4,
                                 NombreC5 = q.NombreC5,
                                 NombreC6 = q.NombreC6,
                                 NombreC7 = q.NombreC7
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
