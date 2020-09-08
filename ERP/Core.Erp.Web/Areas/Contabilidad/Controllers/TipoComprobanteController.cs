using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    [SessionTimeout]
    public class TipoComprobanteController : Controller
    {
        #region Index
        ct_cbtecble_tipo_Bus bus_comprobante_tipo = new ct_cbtecble_tipo_Bus();
        ct_cbtecble_tipo_List Lista_ComprobanteTipo = new ct_cbtecble_tipo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "TipoComprobante", "Index");
            if (info != null)
            {
                ViewBag.Nuevo = info.Nuevo;
                ViewBag.Modificar = info.Modificar;
                ViewBag.Anular = info.Anular;
            }else
            {
                ViewBag.Nuevo = false;
                ViewBag.Modificar = false;
                ViewBag.Anular = false;
            }
            
            #endregion

            ct_cbtecble_tipo_Info model = new ct_cbtecble_tipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_comprobante_tipo.get_list(model.IdEmpresa, true);
            Lista_ComprobanteTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_comprobante_tipo(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_ComprobanteTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_comprobante_tipo", model);
        }
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_cbtecble_tipo_Bus bus_tipo = new ct_cbtecble_tipo_Bus();
            var lst_tipo = bus_tipo.get_list(IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;
        }
        #endregion
        
        #region Acciones

        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "TipoComprobante", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_cbtecble_tipo_Info model = new ct_cbtecble_tipo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTipoCbte_Anul = 1,
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_cbtecble_tipo_Info model)
        {
            if (!bus_comprobante_tipo.guardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoCbte = model.IdTipoCbte, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdTipoCbte = 0, bool Exito=false)
        {
            ct_cbtecble_tipo_Info model = bus_comprobante_tipo.get_info(IdEmpresa,IdTipoCbte);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "TipoComprobante", "Index");
            if (model.tc_Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdTipoCbte = 0)
        {
            ct_cbtecble_tipo_Info model = bus_comprobante_tipo.get_info(IdEmpresa,IdTipoCbte);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "TipoComprobante", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_cbtecble_tipo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!bus_comprobante_tipo.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoCbte = model.IdTipoCbte, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdTipoCbte = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "TipoComprobante", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_cbtecble_tipo_Info model = bus_comprobante_tipo.get_info(IdEmpresa, IdTipoCbte);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_cbtecble_tipo_Info model)
        {
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (!bus_comprobante_tipo.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class ct_cbtecble_tipo_List
    {
        string Variable = "ct_cbtecble_tipo_Info";
        public List<ct_cbtecble_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_cbtecble_tipo_Info> list = new List<ct_cbtecble_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_cbtecble_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_cbtecble_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}