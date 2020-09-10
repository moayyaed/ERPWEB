using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    public class MarcaController : Controller
    {
        #region Variables
        Af_Marca_Bus bus_marca = new Af_Marca_Bus();
        Af_Marca_List Lista_Marca = new Af_Marca_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

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

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "Marca", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            Af_Marca_Info model = new Af_Marca_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_marca.GetList(model.IdEmpresa, true);
            Lista_Marca.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_marca_af(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Marca.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_marca_af", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "Marca", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            Af_Marca_Info model = new Af_Marca_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(Af_Marca_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_marca.GuardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMarca = model.IdMarca, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdMarca = 0, bool Exito=false)
        {
            Af_Marca_Info model = bus_marca.GetInfo(IdEmpresa, IdMarca);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "Marca", "Index");
            if (model.Estado == false)
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

        public ActionResult Modificar(int IdEmpresa = 0, int IdMarca = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "Marca", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            Af_Marca_Info model = bus_marca.GetInfo(IdEmpresa, IdMarca);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(Af_Marca_Info model)
        {
            if (!bus_marca.ModificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMarca = model.IdMarca, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdMarca = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "ActivoFijo", "Marca", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            Af_Marca_Info model = bus_marca.GetInfo(IdEmpresa, IdMarca);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(Af_Marca_Info model)
        {
            if (!bus_marca.AnularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class Af_Marca_List
    {
        string Variable = "Af_Marca_Info";
        public List<Af_Marca_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Marca_Info> list = new List<Af_Marca_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Marca_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Marca_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}