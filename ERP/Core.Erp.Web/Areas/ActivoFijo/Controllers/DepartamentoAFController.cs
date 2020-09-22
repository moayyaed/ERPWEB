using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    public class DepartamentoAFController : Controller
    {
        Af_Departamento_Bus bus_dep = new Af_Departamento_Bus();
        Af_Departamento_List Lista_Departamento = new Af_Departamento_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        public ActionResult Index(decimal IdArea = 0)
        {
            ViewBag.IdArea = IdArea;
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "AreaAF", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            Af_Departamento_Info model = new Af_Departamento_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdArea = IdArea
            };

            var lst = bus_dep.GetList(model.IdEmpresa, model.IdArea,true);
            Lista_Departamento.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_departamento_af(decimal IdArea = 0, bool Nuevo=false)
        {
            ViewBag.IdArea = IdArea;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Departamento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_departamento_af", model);
        }

        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            Af_Area_Bus bus_area = new Af_Area_Bus();
            var lst_area = bus_area.GetList(IdEmpresa, false);
            ViewBag.lst_area = lst_area;
        }

        #region Acciones
        public ActionResult Nuevo(decimal IdArea = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "AreaAF", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            Af_Departamento_Info model = new Af_Departamento_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdArea = IdArea
            };
            ViewBag.IdArea = model.IdArea;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(Af_Departamento_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_dep.GuardarDB(model))
            {
                ViewBag.IdArea = model.IdArea;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdArea = model.IdArea });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdArea = 0, decimal IdDepartamento = 0, bool Exito=false)
        {
            Af_Departamento_Info model = bus_dep.GetInfo(IdEmpresa, IdArea, IdDepartamento);
            if (model == null)
                return RedirectToAction("Index", new { IdArea = IdArea });

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "AreaAF", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            ViewBag.IdArea = model.IdArea;
            cargar_combos();
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , decimal IdArea = 0, decimal IdDepartamento = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "AreaAF", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            Af_Departamento_Info model = bus_dep.GetInfo(IdEmpresa,IdArea, IdDepartamento);
            if (model == null)
                return RedirectToAction("Index", new { IdArea = IdArea });
            ViewBag.IdArea = model.IdArea;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(Af_Departamento_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_dep.ModificarDB(model))
            {
                ViewBag.IdArea = model.IdArea;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdArea = model.IdArea });
        }
        public ActionResult Anular(int IdEmpresa = 0, decimal IdArea = 0, decimal IdDepartamento = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "AreaAF", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            Af_Departamento_Info model = bus_dep.GetInfo(IdEmpresa,IdArea, IdDepartamento);
            if (model == null)
                return RedirectToAction("Index", new { IdArea = IdArea });
            ViewBag.IdArea = model.IdArea;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(Af_Departamento_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_dep.AnularDB(model))
            {
                cargar_combos();
                ViewBag.IdArea = model.IdArea;
                return View(model);
            }
            return RedirectToAction("Index", new { IdArea = model.IdArea });
        }

        #endregion

    }

    public class Af_Departamento_List
    {
        string Variable = "Af_Departamento_Info";
        public List<Af_Departamento_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Departamento_Info> list = new List<Af_Departamento_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Departamento_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Departamento_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}