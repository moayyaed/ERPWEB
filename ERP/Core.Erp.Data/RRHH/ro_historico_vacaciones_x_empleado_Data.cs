using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
using DevExpress.Web;

namespace Core.Erp.Data.RRHH
{
   public class ro_historico_vacaciones_x_empleado_Data
    {

        public List<ro_historico_vacaciones_x_empleado_Info> get_list(int IdEmpresa,decimal IdEmpleado)
        {
            List<ro_historico_vacaciones_x_empleado_Info> lst = new List<ro_historico_vacaciones_x_empleado_Info>();
            try
            {
                using (Entities_rrhh contex = new Entities_rrhh())
                {
                    var consultar = from q in contex.ro_historico_vacaciones_x_empleado
                                    where q.IdEmpleado == IdEmpleado &&
                                    q.IdEmpresa == IdEmpresa
                                    orderby q.FechaIni ascending
                                    select q;
                    foreach (var item in consultar)
                    {
                        ro_historico_vacaciones_x_empleado_Info info = new ro_historico_vacaciones_x_empleado_Info();
                        info.IdEmpresa = item.IdEmpresa;
                        info.IdEmpleado = item.IdEmpleado;
                        info.IdVacacion = item.IdVacacion;
                        info.IdPeriodo_Inicio = item.IdPeriodo_Inicio;
                        info.IdPeriodo_Fin = item.IdPeriodo_Fin;
                        info.FechaIni = item.FechaIni;
                        info.FechaFin = item.FechaFin;
                        info.DiasGanado = item.DiasGanado;
                        info.DiasTomados = item.DiasTomados;
                        lst.Add(info);
                    }
                }

                return lst;
            }
            catch (Exception )
            {
                
                throw ;
            }
        }
        public Boolean GrabarBD(ro_historico_vacaciones_x_empleado_Info info)
        {
            try
            {
                using (Entities_rrhh contex = new Entities_rrhh())
                {
                    ro_historico_vacaciones_x_empleado data = new ro_historico_vacaciones_x_empleado();
                    data.IdEmpresa = info.IdEmpresa;
                    data.IdEmpleado = info.IdEmpleado;
                    data.IdVacacion = get_id(info.IdEmpresa,info.IdEmpleado);
                    data.IdPeriodo_Inicio = info.IdPeriodo_Inicio;
                    data.IdPeriodo_Fin = info.IdPeriodo_Fin;
                    data.FechaIni = info.FechaIni;
                    data.FechaFin = info.FechaFin;
                    data.DiasGanado = info.DiasGanado;
                    data.DiasTomados = info.DiasTomados;
                    contex.ro_historico_vacaciones_x_empleado.Add(data);
                    contex.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {

                throw ;
            }
        }
        public Boolean ModificarBD(ro_historico_vacaciones_x_empleado_Info info)
        {
            try
            {
                using (Entities_rrhh contex = new Entities_rrhh())
                {

                    var data = contex.ro_historico_vacaciones_x_empleado.First(a => a.IdEmpresa == info.IdEmpresa 
                    && a.IdEmpleado == info.IdEmpleado
                    && a.IdPeriodo_Inicio == info.IdPeriodo_Inicio
                    && a.IdPeriodo_Fin==info.IdPeriodo_Fin);
                    data.DiasGanado = info.DiasGanado;
                    contex.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {
                
                throw ;
            }
        }
        public Boolean GetExiste(ro_historico_vacaciones_x_empleado_Info info, ref string msg)
        {
            try
            {
                Boolean valorRetornar = false;
                using (Entities_rrhh contex = new Entities_rrhh())
                {
                    int cont = (from a in contex.ro_historico_vacaciones_x_empleado
                                where a.IdEmpresa == info.IdEmpresa && a.IdEmpleado == info.IdEmpleado
                                && a.IdPeriodo_Inicio == info.IdPeriodo_Inicio
                                && a.IdPeriodo_Fin == info.IdPeriodo_Fin
                                select a).Count();

                    if (cont > 0)
                    {
                        valorRetornar = true;
                    }
                    else { valorRetornar = false; }
                }
                return valorRetornar;
            }
            catch (Exception )
            {
               
                throw ;
            }
        }
        public int get_id(int IdEmpresa, decimal IdEmpleado)
        {
            try
            {
                using (Entities_rrhh contex = new Entities_rrhh())
                {
                    var  resultado = (from a in contex.ro_historico_vacaciones_x_empleado
                                where a.IdEmpresa == IdEmpresa && a.IdEmpleado == IdEmpleado                             
                                select a);

                    if (resultado.Count() > 0)
                    {
                       return Convert.ToInt32( resultado.Max(v => v.IdVacacion)+1);
                    }
                    else
                        return 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Combo bajo demanda
        public List<ro_historico_vacaciones_x_empleado_Info> get_list(int IdEmpresa, int skip, int take, string filter)
        {
            try
            {
                List<ro_historico_vacaciones_x_empleado_Info> Lista = new List<ro_historico_vacaciones_x_empleado_Info>();

                Entities_rrhh context_g = new Entities_rrhh();

                var lstg = context_g.ro_historico_vacaciones_x_empleado.Where(q => q.IdEmpresa == IdEmpresa && (q.IdPeriodo_Inicio.ToString() + " " + q.IdPeriodo_Fin).Contains(filter)).OrderBy(q => q.IdEmpleado).Skip(skip).Take(take).Where(q=>q.DiasGanado >= q.DiasTomados);
                foreach (var q in lstg)
                {
                    Lista.Add(new ro_historico_vacaciones_x_empleado_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdPeriodo_Inicio = q.IdPeriodo_Inicio,
                        IdPeriodo_Fin = q.IdPeriodo_Fin,
                        DiasGanado = q.DiasGanado,
                        DiasTomados = q.DiasTomados,
                        FechaFin = q.FechaFin,
                        FechaIni = q.FechaIni,
                        IdEmpleado = q.IdEmpleado,
                        IdVacacion = q.IdVacacion

                    });
                }

                context_g.Dispose();
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_historico_vacaciones_x_empleado_Info get_info_demanda(int IdEmpresa, int value)
        {
            ro_historico_vacaciones_x_empleado_Info info = new ro_historico_vacaciones_x_empleado_Info();
            using (Entities_rrhh Contex = new Entities_rrhh())
            {
                info = (from q in Contex.ro_historico_vacaciones_x_empleado
                        where q.IdEmpresa == IdEmpresa
                        && q.IdPeriodo_Inicio == value
                        && q.IdPeriodo_Fin == value
                        select new ro_historico_vacaciones_x_empleado_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdPeriodo_Inicio = q.IdPeriodo_Inicio,
                            IdPeriodo_Fin = q.IdPeriodo_Fin,
                            DiasGanado = q.DiasGanado,
                            DiasTomados = q.DiasTomados,
                            FechaFin = q.FechaFin,
                            FechaIni = q.FechaIni,
                            IdEmpleado = q.IdEmpleado,
                            IdVacacion = q.IdVacacion
                        }).FirstOrDefault();
            }
            return info;
        }


        public List<ro_historico_vacaciones_x_empleado_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<ro_historico_vacaciones_x_empleado_Info> Lista = new List<ro_historico_vacaciones_x_empleado_Info>();
            Lista = get_list(IdEmpresa, skip, take, args.Filter);
            return Lista;
        }

        public ro_historico_vacaciones_x_empleado_Info get_info_bajo_demanda(int IdEmpresa, ListEditItemRequestedByValueEventArgs args)
        {
            decimal id;
            if (args.Value == null || !decimal.TryParse(args.Value.ToString(), out id))
                return null;
            return get_info_demanda(IdEmpresa, (int)args.Value);
        }

        #endregion

    }
}
