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
    public class GrupoContableController : Controller
    {
        #region Index
        ct_grupocble_Bus bus_grupo_cble = new ct_grupocble_Bus();
        ct_grupocble_List Lista_GrupoCont = new ct_grupocble_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "GrupoContable", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_grupocble_Info model = new ct_grupocble_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            var lst = bus_grupo_cble.get_list(true);
            Lista_GrupoCont.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_grupo_cble(bool Nuevo = false)
        {
            //List<ct_grupocble_Info> model = new List<ct_grupocble_Info>();
            //model = bus_grupo_cble.get_list(true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_GrupoCont.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_grupo_cble", model);
        }

        private void cargar_combos()
        {
            Dictionary<string, string> lst_balances = new Dictionary<string, string>();
            lst_balances.Add("BG","Balance general");
            lst_balances.Add("ER", "Estado de resultados");
            ViewBag.lst_balances = lst_balances;
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "GrupoContable", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_grupocble_Info model = new ct_grupocble_Info();
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_grupocble_Info model)
        {
            if(!bus_grupo_cble.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdGrupoCble = model.IdGrupoCble, Exito = true });
        }

        public ActionResult Consultar(string IdGrupoCble = "", bool Exito=false)
        {
            ct_grupocble_Info model = bus_grupo_cble.get_info(IdGrupoCble);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "GrupoContable", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar(string IdGrupoCble = "")
        {
            ct_grupocble_Info model = bus_grupo_cble.get_info(IdGrupoCble);
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "GrupoContable", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ct_grupocble_Info model)
        {
            if(!bus_grupo_cble.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdGrupoCble = model.IdGrupoCble, Exito = true });
        }
        public ActionResult Anular(string IdGrupoCble = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "GrupoContable", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_grupocble_Info model = bus_grupo_cble.get_info(IdGrupoCble);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(ct_grupocble_Info model)
        {
            if (!bus_grupo_cble.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class ct_grupocble_List
    {
        string Variable = "ct_grupocble_Info";
        public List<ct_grupocble_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_grupocble_Info> list = new List<ct_grupocble_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_grupocble_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_grupocble_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}