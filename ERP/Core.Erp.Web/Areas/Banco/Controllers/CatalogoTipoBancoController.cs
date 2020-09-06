using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Banco;
using Core.Erp.Info.Banco;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    [SessionTimeout]
    public class CatalogoTipoBancoController : Controller
    {
        #region Index
        ba_CatalogoTipo_Bus bus_catalogotipo = new ba_CatalogoTipo_Bus();
        ba_CatalogoTipo_List Lista_CatalogoTipo = new ba_CatalogoTipo_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            ba_CatalogoTipo_Info model = new ba_CatalogoTipo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            var lst = bus_catalogotipo.get_list();
            Lista_CatalogoTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogotipo_banco(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_CatalogoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_catalogotipo_banco", model);
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ba_CatalogoTipo_Info model = new ba_CatalogoTipo_Info();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ba_CatalogoTipo_Info model)
        {
            if (bus_catalogotipo.validar_existe_IdTipoCatalogo(model.IdTipoCatalogo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                ViewBag.IdTipoCatalogo = model.IdTipoCatalogo;
                return View(model);
            }
            if (!bus_catalogotipo.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoCatalogo = model.IdTipoCatalogo, Exito = true });
        }

        public ActionResult Consultar(string IdTipoCatalogo = "", bool Exito = false)
        {
            ba_CatalogoTipo_Info model = bus_catalogotipo.get_info(IdTipoCatalogo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Modificar(string IdTipoCatalogo = "")
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "CatalogoTipoBanco", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ba_CatalogoTipo_Info model = bus_catalogotipo.get_info(IdTipoCatalogo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ba_CatalogoTipo_Info model)
        {
            if (!bus_catalogotipo.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdTipoCatalogo = model.IdTipoCatalogo, Exito = true });
        }

        #endregion
    }

    public class ba_CatalogoTipo_List
    {
        string Variable = "ba_CatalogoTipo_Info";
        public List<ba_CatalogoTipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_CatalogoTipo_Info> list = new List<ba_CatalogoTipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_CatalogoTipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_CatalogoTipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}