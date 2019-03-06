using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Bus.General;
using Core.Erp.Web.Helps;

namespace Core.Erp.Web.Areas.SeguridadAcceso.Controllers
{
    //[SessionTimeout]
    public class UsuarioController : Controller
    {
        #region Index/Metodos
        seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
        seg_Usuario_x_Empresa_Bus bus_usuario_x_empresa = new seg_Usuario_x_Empresa_Bus();
        seg_Menu_Bus bus_menu = new seg_Menu_Bus();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_usuario()
        {
            List<seg_usuario_Info> model = bus_usuario.get_list(true);
            return PartialView("_GridViewPartial_usuario", model);
        }

        private void cargar_combos(seg_usuario_Info model)
        {
            if (!string.IsNullOrEmpty(model.IdUsuario))
                model.lst_usuario_x_empresa = bus_usuario_x_empresa.get_list(model.IdUsuario);
            else
                model.lst_usuario_x_empresa = new List<seg_Usuario_x_Empresa_Info>();

            tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
            var lst_empresa = bus_empresa.get_list(false);
            if (model.lst_usuario_x_empresa.Count == 0)
            {                
                foreach (var item in lst_empresa)
                {
                    model.lst_usuario_x_empresa.Add(new seg_Usuario_x_Empresa_Info { IdEmpresa = item.IdEmpresa, em_nombre = item.em_nombre});
                }
            }else
            {
                model.lst_usuario_x_empresa = (from e in lst_empresa
                                               join pp in model.lst_usuario_x_empresa
                                               on e.IdEmpresa equals pp.IdEmpresa into temp_emp
                                               from pp in temp_emp.DefaultIfEmpty()
                                               select new seg_Usuario_x_Empresa_Info
                                               {
                                                   IdEmpresa = e.IdEmpresa,
                                                   em_nombre = e.em_nombre,
                                                   seleccionado =  pp == null ? false : true                                                  
                                               }).ToList();
            }
            var lst_menu = bus_menu.get_list_combo(false);
            lst_menu.Add(new seg_Menu_Info { IdMenu = 0, DescripcionMenu_combo = "== Seleccione ==" });
            ViewBag.lst_menu = lst_menu;
        }

        #endregion
        #region Acciones

        public ActionResult Nuevo()
        {
            seg_usuario_Info model = new seg_usuario_Info();
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(seg_usuario_Info model)
        {
            if(bus_usuario.validar_existe_usuario(model.IdUsuario))
            {
                ViewBag.mensaje = "Usuario ya se encuentra registrado";
                return View(model);
            }

            if (!bus_usuario.guardarDB(model))
                return View(model);
            
            #region Guardar usuario_x_empresa
            bus_usuario_x_empresa.eliminarDB(model.IdUsuario);
            model.lst_usuario_x_empresa = model.lst_usuario_x_empresa.Where(q => q.seleccionado == true).ToList();
            model.lst_usuario_x_empresa.ForEach(q => q.IdUsuario = model.IdUsuario);
            bus_usuario_x_empresa.guardarDB(model.lst_usuario_x_empresa);
            #endregion

            return RedirectToAction("Index");
        }

        public ActionResult Modificar(string IdUsuario = "")
        {
            seg_usuario_Info model = bus_usuario.get_info(IdUsuario);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(seg_usuario_Info model)
        {
            if (!bus_usuario.modificarDB(model))
                return View(model);

            #region Guardar usuario_x_empresa
            bus_usuario_x_empresa.eliminarDB(model.IdUsuario);
            model.lst_usuario_x_empresa = model.lst_usuario_x_empresa.Where(q => q.seleccionado == true).ToList();
            model.lst_usuario_x_empresa.ForEach(q => q.IdUsuario = model.IdUsuario);
            bus_usuario_x_empresa.eliminarDB(model.IdUsuario);
            bus_usuario_x_empresa.guardarDB(model.lst_usuario_x_empresa);
            #endregion

            return RedirectToAction("Index");
        }

        public ActionResult Anular(string IdUsuario = "")
        {
            seg_usuario_Info model = bus_usuario.get_info(IdUsuario);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(seg_usuario_Info model)
        {
            if (!bus_usuario.anularDB(model))
                return View(model);

            return RedirectToAction("Index");
        }
        #endregion
        #region Json
        public JsonResult ResetearContrasena(string IdUsuario = "")
        {
            int resultado = 0;

            if(bus_usuario.ResetearContrasenia(IdUsuario,"1234"))
                resultado = 1;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [ValidateInput(false)]
        public ActionResult GridViewPartial_Usuario_x_Sucursal()
        {
            var model = new object[0];
            return PartialView("_GridViewPartial_Usuario_x_Sucursal", model);
        }
    }
}