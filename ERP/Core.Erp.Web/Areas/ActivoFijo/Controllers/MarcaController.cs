using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Info.ActivoFijo;
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
        #region Index
        Af_Marca_Bus bus_marca = new Af_Marca_Bus();
        public ActionResult Index()
        {
            var model = new Af_Marca_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_marca_af()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = bus_marca.GetList(IdEmpresa, true);
            return PartialView("_GridViewPartial_marca_af", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
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
            return RedirectToAction("Index");
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdMarca = 0)
        {
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
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdMarca = 0)
        {
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