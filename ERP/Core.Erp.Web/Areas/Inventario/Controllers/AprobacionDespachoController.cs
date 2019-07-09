using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Bus.Inventario;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    public class AprobacionDespachoController : Controller
    {
        #region Variables
        in_Ing_Egr_Inven_List List_inv = new in_Ing_Egr_Inven_List();
        in_Ing_Egr_Inven_Bus bus_inv = new in_Ing_Egr_Inven_Bus();
        #endregion

        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
            in_Ing_Egr_Inven_Info model = new in_Ing_Egr_Inven_Info
            {
                IdEmpresa = IdEmpresa,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                FechaDespacho = null,
                IdUsuarioDespacho = "APRO"
            };
            return View(model);
        }
        #endregion

        #region INV
        [ValidateInput(false)]
        public ActionResult GridViewPartial_aprobacion_despacho(decimal IdTransaccionSession = 0)
        {
            ViewBag.IdTransaccionSession = IdTransaccionSession;
            var model = List_inv.get_list(IdTransaccionSession);
            return PartialView("_GridViewPartial_aprobacion_despacho", model);
        }
        #endregion
    }
    public class in_Ing_Egr_Inven_List
    {
        string variable = "in_Ing_Egr_Inven_Info";
        public List<in_Ing_Egr_Inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_Info> list = new List<in_Ing_Egr_Inven_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Ing_Egr_Inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(in_Ing_Egr_Inven_Info info_det, decimal IdTransaccionSession)
        {
            List<in_Ing_Egr_Inven_Info> list = get_list(IdTransaccionSession);
            info_det.IdMovi_inven_tipo = list.Count == 0 ? 1 : list.Max(q => q.IdMovi_inven_tipo) + 1;
            list.Add(info_det);
        }

        public void UpdateRow(in_Ing_Egr_Inven_Info info_det, decimal IdTransaccionSession)
        {
            in_Ing_Egr_Inven_Info edited_info = get_list(IdTransaccionSession).Where(m => m.IdMovi_inven_tipo == info_det.IdMovi_inven_tipo).First();
        }

        public void DeleteRow(int IdMovi_inven_tipo, decimal IdTransaccionSession)
        {
            List<in_Ing_Egr_Inven_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.IdMovi_inven_tipo == IdMovi_inven_tipo).First());
        }
    }
}