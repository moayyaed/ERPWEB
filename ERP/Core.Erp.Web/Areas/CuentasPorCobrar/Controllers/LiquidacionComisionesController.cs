using DevExpress.Web.Mvc;
using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Web.Helps;
using System;
using System.Web.Mvc;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Core.Erp.Info.Helps;

namespace Core.Erp.Web.Areas.CuentasPorCobrar.Controllers
{
    [SessionTimeout]
    public class LiquidacionComisionesController : Controller
    {
        #region Variables
        cxc_liquidacion_comisiones_Bus bus_liq = new cxc_liquidacion_comisiones_Bus();
        fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
        cxc_liquidacion_comisiones_det_Bus bus_det = new cxc_liquidacion_comisiones_det_Bus();
        cxc_liquidacion_det_List List_det = new cxc_liquidacion_det_List();
        string MensajeSuccess = string.Empty;
        #endregion
        #region Index
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            return View(model);
        }
        

        [ValidateInput(false)]
        public ActionResult GridViewPartial_liquidacion_com()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = bus_liq.get_list(IdEmpresa, true);
            return PartialView("_GridViewPartial_liquidacion_com", model);
        }

        #endregion
        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {
            var lst_vendedor = bus_vendedor.get_list(IdEmpresa, false);
            ViewBag.lst_vendedor = lst_vendedor;
        }

        #endregion
        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0 )
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cxc_liquidacion_comisiones_Info model = new cxc_liquidacion_comisiones_Info
            {
                IdEmpresa = IdEmpresa,
                Fecha = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)

            };
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(cxc_liquidacion_comisiones_Info model)
        {
            model.IdUsuario = Session["IdUsuario"].ToString();
            model.lst_det = List_det.get_list(model.IdTransaccionSession);
            if (!bus_liq.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            MensajeSuccess = "Registro creado exitósamente";
            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdLiquidacion = model.IdLiquidacion, MensajeSuccess });
        }

        public ActionResult Modificar(int IdEmpresa = 0 , decimal IdLiquidacion = 0, string MensajeSuccess="")
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cxc_liquidacion_comisiones_Info model = bus_liq.get_info(IdEmpresa, IdLiquidacion);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            model.lst_det = bus_det.get_list(IdEmpresa, IdLiquidacion);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);

            ViewBag.MensajeSuccess = MensajeSuccess == "" ? null : MensajeSuccess;
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cxc_liquidacion_comisiones_Info model)
        {
            model.IdUsuarioUltMod = Session["IdUsuario"].ToString();
            model.lst_det = List_det.get_list(model.IdTransaccionSession);

            if (!bus_liq.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            MensajeSuccess = "Registro actualizado exitósamente";
            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdLiquidacion = model.IdLiquidacion, MensajeSuccess });
        }
        public ActionResult Anular(int IdEmpresa = 0, decimal IdLiquidacion = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_liquidacion_comisiones_Info model = bus_liq.get_info(IdEmpresa, IdLiquidacion);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(IdEmpresa);
            model.lst_det = bus_det.get_list(IdEmpresa, IdLiquidacion);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_det.set_list(model.lst_det, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cxc_liquidacion_comisiones_Info model)
        {
            model.IdUsuarioUltAnu = Session["IdUsuario"].ToString();
            if (!bus_liq.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region GRids

        [ValidateInput(false)]
        public ActionResult GridViewPartial_liquidacion_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_liquidacion_det", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] cxc_liquidacion_comisiones_det_Info info_det)
        {
            if (ModelState.IsValid)
                List_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_liquidacion_det", model);
        }

        #endregion

        public JsonResult get_list_x_liquidar(int IdEmpresa = 0 , decimal IdTransaccionSession = 0, int IdVendedor = 0)
        {
            var resultado = bus_det.get_list_x_liquidar(IdEmpresa, IdVendedor);
            List_det.set_list(resultado, IdTransaccionSession);
            return Json(resultado, JsonRequestBehavior.AllowGet);
                
       }
    }

    public class cxc_liquidacion_det_List
    {
        string variable = "cxc_liquidacion_comisiones_det_Info";
        public List<cxc_liquidacion_comisiones_det_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_liquidacion_comisiones_det_Info> list = new List<cxc_liquidacion_comisiones_det_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_liquidacion_comisiones_det_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_liquidacion_comisiones_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void UpdateRow(cxc_liquidacion_comisiones_det_Info info_det, decimal IdTransaccionSession)
        {
            cxc_liquidacion_comisiones_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.BaseComision = info_det.BaseComision;
            edited_info.PorcentajeComision = info_det.PorcentajeComision;
            edited_info.TotalAComisionar = info_det.TotalAComisionar;
            edited_info.TotalComisionado = info_det.TotalComisionado;
            edited_info.TotalLiquidacion = info_det.TotalLiquidacion;
            edited_info.NoComisiona = info_det.NoComisiona;

        }
        
    }

}