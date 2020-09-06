using Core.Erp.Bus.Compras;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Compras;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Compras.Controllers
{
    [SessionTimeout]
    public class DepartamentoComprasController : Controller
    {
        #region Index

        com_departamento_Bus bus_dpto = new com_departamento_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        com_departamento_List Lista_Departamento = new com_departamento_List();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "DepartamentoCompras", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            com_departamento_Info model = new com_departamento_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_dpto.get_list(model.IdEmpresa, true);
            Lista_Departamento.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_departamento(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Departamento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_departamento", model);
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo (int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "DepartamentoCompras", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            com_departamento_Info model = new com_departamento_Info
            {
                IdEmpresa = IdEmpresa
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(com_departamento_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_dpto.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdDepartamento = model.IdDepartamento, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, decimal IdDepartamento = 0, bool Exito=false)
        {
            com_departamento_Info model = bus_dpto.get_info(IdEmpresa, IdDepartamento);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "DepartamentoCompras", "Index");
            if (model.Estado == "I")
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

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdDepartamento = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "DepartamentoCompras", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            com_departamento_Info model = bus_dpto.get_info(IdEmpresa, IdDepartamento);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(com_departamento_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_dpto.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdDepartamento = model.IdDepartamento, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdDepartamento = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "DepartamentoCompras", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            com_departamento_Info model = bus_dpto.get_info(IdEmpresa, IdDepartamento);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(com_departamento_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_dpto.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }

    public class com_departamento_List
    {
        string Variable = "com_departamento_Info";
        public List<com_departamento_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<com_departamento_Info> list = new List<com_departamento_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<com_departamento_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<com_departamento_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}