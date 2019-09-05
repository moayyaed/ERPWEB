using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class DinardapController : Controller
    {
        #region Variables
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        Dinardap_Bus bus_dinardap = new Dinardap_Bus();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            Dinardap_Info model = new Dinardap_Info();
            cargar_combos();
            return View(model);
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
            var lst_periodos = bus_periodo.get_list(IdEmpresa, false);
            ViewBag.lst_periodos = lst_periodos;

            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(Convert.ToInt32(SessionFixed.IdEmpresa), false);
            lst_sucursal.Where(q => q.IdSucursal == Convert.ToInt32(SessionFixed.IdSucursal)).FirstOrDefault().Seleccionado = true;
            ViewBag.lst_sucursal = lst_sucursal;
        }
        #endregion

        #region Archivo

        //public FileResult get_archivo(string IntArray = "", int IdPeriodo = 0)
        //{
        //    int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
        //    byte[] archivo;

        //    var info_archivo = bus_dinardap.get_info(IdEmpresa, IdPeriodo);

        //    archivo = GetArchivo(info_archivo, info_archivo.Nom_Archivo);
        //    return File(archivo, "application/xml", info_archivo.Nom_Archivo + ".txt");
        //}

        //public byte[] GetArchivo(Dinardap_Info info, string nombre_file)
        //{
        //    try
        //    {
        //        return GetMulticash(info, nombre_file);

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        #endregion
    }
}