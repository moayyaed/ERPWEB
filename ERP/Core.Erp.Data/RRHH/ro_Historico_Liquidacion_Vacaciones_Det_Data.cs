using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
namespace Core.Erp.Data.RRHH
{
   public class ro_Historico_Liquidacion_Vacaciones_Det_Data
    {
        public bool Guardar_DB(ro_Historico_Liquidacion_Vacaciones_Det_Info info)
        {
            try
            {
                using (Entities_rrhh db = new Entities_rrhh())
                {
                    ro_Historico_Liquidacion_Vacaciones_Det add = new ro_Historico_Liquidacion_Vacaciones_Det();
                    add.IdEmpresa = info.IdEmpresa;
                    add.IdLiquidacion = info.IdLiquidacion;
                    add.Anio = info.Anio;
                    add.Mes = info.Mes;
                    add.Total_Remuneracion = info.Total_Remuneracion;
                    add.Total_Vacaciones = info.Total_Vacaciones;
                    add.Valor_Cancelar = info.Valor_Cancelar;
                    add.Secuencia = info.Secuencia;
                    db.ro_Historico_Liquidacion_Vacaciones_Det.Add(add);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception )
            {
                throw ;
            }
        }
        public bool Eliminar(ro_Historico_Liquidacion_Vacaciones_Info info)
        {
            try
            {
                using (Entities_rrhh db = new Entities_rrhh())
                {
                    db.Database.ExecuteSqlCommand(" delete ro_Historico_Liquidacion_Vacaciones_Det where IdEmpresa='" + info.IdEmpresa + "'  and IdEmpleado='" + info.IdEmpleado + "'  and IdLiquidacion ='" + info.IdLiquidacion + "'");
                    return true;
                }
            }
            catch (Exception )
            {
                throw ;
            }
        }
        public List<ro_Historico_Liquidacion_Vacaciones_Det_Info> Get_Lis(int IdEmpresa,  decimal IdLiquidacion)
        {
            List<ro_Historico_Liquidacion_Vacaciones_Det_Info> Lst = new List<ro_Historico_Liquidacion_Vacaciones_Det_Info>();
            Entities_rrhh oEnti = new Entities_rrhh();
            try
            {
              
                Lst = oEnti.ro_Historico_Liquidacion_Vacaciones_Det.Where(q => q.IdEmpresa == IdEmpresa && q.IdLiquidacion ==IdLiquidacion).Select(q => new ro_Historico_Liquidacion_Vacaciones_Det_Info
                {
                    IdEmpresa = q.IdEmpresa,
                    IdLiquidacion = q.IdLiquidacion,
                    Anio = q.Anio,
                    Mes = q.Mes,
                    Total_Remuneracion = q.Total_Remuneracion,
                    Total_Vacaciones = q.Total_Vacaciones,
                    Valor_Cancelar = q.Valor_Cancelar,
                    Secuencia = q.Secuencia,
            }).ToList();
            return Lst;
            }
            catch (Exception )
            {
               
                throw;
            }

        }

    }
}
