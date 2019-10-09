using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.RRHH;
using Core.Erp.Bus.RRHH;
using Core.Erp.Web.Helps;

namespace Core.Erp.Web.Areas.RRHH.Controllers
{
    public class CatalogoRRHHController : Controller
    {
        ro_catalogo_Bus bus_catalogo = new ro_catalogo_Bus();
        List<ro_catalogoTipo_Info> lista_tipo = new List<ro_catalogoTipo_Info>();
        ro_catalogoTipo_Bus bus_tipo = new ro_catalogoTipo_Bus();
        ro_catalogo_List Lista_Catalogo = new ro_catalogo_List();
        public ActionResult Index(int IdTipoCatalogo=0)
        {
            ViewBag.IdTipoCatalogo = IdTipoCatalogo;

            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            ro_catalogo_Info model = new ro_catalogo_Info
            {
                IdTipoCatalogo = IdTipoCatalogo,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<ro_catalogo_Info> lista = bus_catalogo.get_list_x_tipo(model.IdTipoCatalogo);
            Lista_Catalogo.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_catalogos(int IdTipoCatalogo=0)
        {
            try
            {
                ViewBag.IdTipoCatalogo = IdTipoCatalogo;
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<ro_catalogo_Info> model = Lista_Catalogo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_catalogos", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(ro_catalogo_Info info)
        {
            try
            {

                info.IdUsuario = SessionFixed.IdUsuario;
                ViewBag.IdTipoCatalogo = info.IdTipoCatalogo;
                cargar_combos();
                if (ModelState.IsValid)
                {
                    if (bus_catalogo.si_existe_codigo(info.CodCatalogo))
                    {
                        ViewBag.mensaje = "El código ya se encuentra registrado";
                        return View(info);
                    }
                    else
                    {
                        info.Fecha_Transac = DateTime.Now;
                        info.IdUsuario = Session["IdUsuario"].ToString();
                        if (!bus_catalogo.guardarDB(info))
                            return View(info);
                        else
                            return RedirectToAction("Index", new { IdTipoCatalogo = info.IdTipoCatalogo });
                    }
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Nuevo( int IdTipoCatalogo=0)
        {
            try
            {
                ro_catalogo_Info model = new ro_catalogo_Info
                {
                    IdTipoCatalogo = IdTipoCatalogo
                };
                ViewBag.IdTipoCatalogo = IdTipoCatalogo;
                cargar_combos();
                return View(model);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(ro_catalogo_Info info)
        {
            try
            {
                info.IdUsuarioUltMod = SessionFixed.IdUsuario;
                info.Fecha_UltMod = DateTime.Now;
                info.IdUsuarioUltMod = Session["IdUsuario"].ToString();
                if (!bus_catalogo.modificarDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index", new { IdTipoCatalogo = info.IdTipoCatalogo });

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Modificar(int IdTipoCatalogo,int IdCatalogo = 0)
        {
            try
            {
                cargar_combos();
                ViewBag.IdTipoCatalogo = IdTipoCatalogo;
                return View(bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo));

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]

        public ActionResult Anular(ro_catalogo_Info info)
        {
            try
            {
                info.Fecha_UltAnu = DateTime.Now;
                info.IdUsuarioUltAnu = SessionFixed.IdUsuario;
                if (!bus_catalogo.anularDB(info))
                    return View(info);
                else
                    return RedirectToAction("Index");


            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Anular(int IdTipoCatalogo, int IdCatalogo = 0)
        {
            try
            {
                cargar_combos();
                ViewBag.IdTipoCatalogo = IdTipoCatalogo;
                return View(bus_catalogo.get_info(IdTipoCatalogo, IdCatalogo));

            }
            catch (Exception)
            {

                throw;
            }
        }
      
        private void cargar_combos()
        {
            try
            {
                lista_tipo = bus_tipo.get_list(false);
                ViewBag.lst_tipos = lista_tipo;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }

    public class ro_catalogo_List
    {
        string Variable = "ro_catalogo_Info";
        public List<ro_catalogo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ro_catalogo_Info> list = new List<ro_catalogo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ro_catalogo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ro_catalogo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}