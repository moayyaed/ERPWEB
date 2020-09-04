using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class PuntoCargoController : Controller
    {
        #region Index
        ct_punto_cargo_Bus bus_punto_cargo = new ct_punto_cargo_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_punto_cargo_List Lista_PuntoCargo = new ct_punto_cargo_List();

        public ActionResult Index(int IdPunto_cargo_grupo = 0)
        {
            ViewBag.IdPunto_cargo_grupo = IdPunto_cargo_grupo;
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_punto_cargo_Info model = new ct_punto_cargo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdPunto_cargo_grupo = IdPunto_cargo_grupo
            };

            var lst = bus_punto_cargo.GetList(model.IdEmpresa, model.IdPunto_cargo_grupo, true, false);
            Lista_PuntoCargo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        
        [ValidateInput(false)]
        public ActionResult GridViewPartial_punto_cargo(bool Nuevo= false, int IdPunto_cargo_grupo=0)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_punto_cargo.GetList(IdEmpresa, IdPunto_cargo_grupo, true,false);
            ViewBag.IdPunto_cargo_grupo = IdPunto_cargo_grupo;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_PuntoCargo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_punto_cargo", model);
        }

        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_punto_cargo_grupo_Bus bus_punto_grupo = new ct_punto_cargo_grupo_Bus();
            var lst_grupo = bus_punto_grupo.GetList(IdEmpresa, false);
            ViewBag.lst_grupo = lst_grupo;

        }
        #endregion
        #region Acciones

        public ActionResult Nuevo(int IdEmpresa= 0, int IdPunto_cargo_grupo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ct_punto_cargo_Info model = new ct_punto_cargo_Info
            {
                IdEmpresa = IdEmpresa,
                IdPunto_cargo_grupo = IdPunto_cargo_grupo
            };
            ViewBag.IdPunto_cargo_grupo = IdPunto_cargo_grupo;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ct_punto_cargo_Info model)
        {
            model.IdUsuarioCreacion = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_punto_cargo.GuardarDB(model))
            {
                ViewBag.IdPunto_cargo_grupo = model.IdPunto_cargo_grupo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdPunto_cargo_grupo = model.IdPunto_cargo_grupo });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdPunto_cargo_grupo = 0, int IdPunto_cargo = 0, bool Exito=false)
        {
            ct_punto_cargo_Info model = bus_punto_cargo.GetInfo(IdEmpresa, IdPunto_cargo);
            if (model == null)
                return RedirectToAction("Index", new { IdPunto_cargo_grupo = IdPunto_cargo_grupo });
            ViewBag.IdPunto_cargo_grupo = IdPunto_cargo_grupo;

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
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

            cargar_combos();
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdPunto_cargo_grupo = 0, int IdPunto_cargo= 0)
        {
            ct_punto_cargo_Info model = bus_punto_cargo.GetInfo(IdEmpresa, IdPunto_cargo);
            if (model == null)
                return RedirectToAction("Index", new { IdPunto_cargo_grupo = IdPunto_cargo_grupo });
            ViewBag.IdPunto_cargo_grupo = IdPunto_cargo_grupo;

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_punto_cargo_Info model)
        {
            model.IdUsuarioModificacion = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_punto_cargo.ModificarDB(model))
            {
                ViewBag.IdPunto_cargo_grupo = model.IdPunto_cargo_grupo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdPunto_cargo_grupo = model.IdPunto_cargo_grupo });
        }
        public ActionResult Anular(int IdEmpresa = 0, int IdPunto_cargo_grupo = 0, int IdPunto_cargo=0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            ct_punto_cargo_Info model = bus_punto_cargo.GetInfo(IdEmpresa, IdPunto_cargo);
            if (model == null)
                return RedirectToAction("Index", new { IdPunto_cargo_grupo = IdPunto_cargo_grupo });
            ViewBag.IdPunto_cargo_grupo = IdPunto_cargo_grupo;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_punto_cargo_Info model)
        {
            model.IdUsuarioAnulacion = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_punto_cargo.AnularDB(model))
            {
                ViewBag.IdPunto_cargo_grupo = model.IdPunto_cargo_grupo;
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index", new { IdPunto_cargo_grupo = model.IdPunto_cargo_grupo });
        }
        #endregion
    }
    public class ct_punto_cargo_List
    {
        string Variable = "ct_punto_cargo_Info";
        public List<ct_punto_cargo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_punto_cargo_Info> list = new List<ct_punto_cargo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_punto_cargo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_punto_cargo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}