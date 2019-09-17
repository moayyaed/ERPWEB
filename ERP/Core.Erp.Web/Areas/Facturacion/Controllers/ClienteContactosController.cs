using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class ClienteContactosController : Controller
    {
        #region Variables
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        fa_cliente_contactos_Bus bus_cliente_contactos = new fa_cliente_contactos_Bus();
        tb_ciudad_Bus bus_ciudad = new tb_ciudad_Bus();
        tb_parroquia_Bus bus_parroquia = new tb_parroquia_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        fa_proforma_Bus bus_proforma = new fa_proforma_Bus();
        fa_guia_remision_Bus bus_guia = new fa_guia_remision_Bus();
        fa_notaCreDeb_Bus bus_credeb = new fa_notaCreDeb_Bus();
        string mensaje = string.Empty;
        #endregion

        #region Metodos ComboBox bajo demanda cliente
        public ActionResult CmbCliente_Factura()
        {
            decimal model = new decimal();
            return PartialView("_CmbCliente", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_cliente(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_cliente(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        #endregion

        #region Combo Bajo Demanda Ciudad
        public ActionResult CmbCiudad()
        {
            fa_cliente_contactos_Info model = new fa_cliente_contactos_Info();
            return PartialView("_CmbCiudad", model);
        }
        public List<tb_ciudad_Info> get_list_bajo_demanda_ciudad(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_ciudad.get_list_bajo_demanda(args);
        }
        public tb_ciudad_Info get_info_bajo_demanda_ciudad(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_ciudad.get_info_bajo_demanda(args);
        }
        #endregion

        #region Parroquia
        public ActionResult CmbParroquia()
        {
            SessionFixed.IdCiudad = Request.Params["IdCiudad"] != null ? Request.Params["IdCiudad"].ToString() : SessionFixed.IdCiudad;
            fa_cliente_contactos_Info model = new fa_cliente_contactos_Info();
            return PartialView("_CmbParroquia", model);
        }
        public List<tb_parroquia_Info> get_list_bajo_demanda_parroquia(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_parroquia.get_list_bajo_demanda(args, Convert.ToString(SessionFixed.IdCiudad));
        }
        public tb_parroquia_Info get_info_bajo_demanda_parroquia(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_parroquia.get_info_bajo_demanda(args, Convert.ToString(SessionFixed.IdCiudad));
        }
        #endregion

        #region Validar
        private bool validar(fa_cliente_contactos_Info i_validar, ref string msg)
        {
            var lst_factura = bus_factura.get_list_x_contacto(i_validar.IdEmpresa, i_validar.IdCliente, i_validar.IdContacto);
            var lst_proforma = bus_proforma.get_list_x_contacto(i_validar.IdEmpresa, i_validar.IdCliente, i_validar.IdContacto);
            var lst_nota_credeb = bus_credeb.get_list_x_contacto(i_validar.IdEmpresa, i_validar.IdCliente, i_validar.IdContacto);
            var lst_guia = bus_guia.get_list_x_contacto(i_validar.IdEmpresa, i_validar.IdCliente, i_validar.IdContacto);

            if (lst_factura.Count > 0)
            {
                msg = "No se puede eliminar el contacto, existe en facturas";
                return false;
            }

            if (lst_proforma.Count > 0)
            {
                msg = "No se puede eliminar el contacto, existe en proformas";
                return false;
            }

            if (lst_nota_credeb.Count > 0)
            {
                msg = "No se puede eliminar el contacto, existe en notas de crédito / débito";
                return false;
            }

            if (lst_guia.Count > 0)
            {
                msg = "No se puede eliminar el contacto, existe en guías";
                return false;
            }
            return true;
        }
        #endregion

        #region Index
        public ActionResult Index(int IdEmpresa = 0, int IdCliente = 0)
        {
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdCliente = IdCliente;
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ClienteContactos(int IdEmpresa = 0, int IdCliente = 0)
        {
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdCliente = IdCliente;
            List<fa_cliente_contactos_Info> model = bus_cliente_contactos.get_list(IdEmpresa, IdCliente);
            return PartialView("_GridViewPartial_ClienteContactos", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0, int IdCliente = 0)
        {
            fa_cliente_contactos_Info model = new fa_cliente_contactos_Info
            {
                IdEmpresa = IdEmpresa,
                IdCliente = IdCliente
            };
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdCliente = IdCliente;

            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(fa_cliente_contactos_Info model)
        {
            if (!bus_cliente_contactos.guardarDB(model))
            {
                ViewBag.IdEmpresa = model.IdEmpresa;
                ViewBag.IdCliente = model.IdCliente;

                return View(model);
            }

            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdCliente = model.IdCliente });
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdCliente = 0, int IdContacto=0)
        {
            fa_cliente_contactos_Info model = bus_cliente_contactos.get_info(IdEmpresa, IdCliente, IdContacto);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdCliente = IdCliente });
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdCliente = IdCliente;
        
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_cliente_contactos_Info model)
        {
            if (!bus_cliente_contactos.modificarDB(model))
            {
                ViewBag.IdEmpresa = model.IdEmpresa;
                ViewBag.IdCliente = model.IdCliente;
             
                return View(model);
            }

            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdCliente = model.IdCliente });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdCliente = 0, int IdContacto=0)
        {
            fa_cliente_contactos_Info model = bus_cliente_contactos.get_info(IdEmpresa, IdCliente, IdContacto);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdSucursal = IdCliente });
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdCliente = IdCliente;
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_cliente_contactos_Info model)
        {
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                return View(model);
            }

            if (!bus_cliente_contactos.eliminarDB(model))
            {
                ViewBag.IdEmpresa = model.IdEmpresa;
                ViewBag.IdCliente = model.IdCliente;
               
                return View(model);
            }

            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdCliente = model.IdCliente });
        }

        #endregion

        #region Json
        public JsonResult cargar_lista_contactos(decimal IdCliente = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            List<fa_cliente_contactos_Info> lst_cliente_contactos = bus_cliente_contactos.get_list(IdEmpresa, IdCliente);

            return Json(lst_cliente_contactos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDireccionDestino(decimal IdCliente = 0, int IdContacto=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var Direccion = "";
            var resultado = bus_cliente_contactos.get_info(IdEmpresa, IdCliente, IdContacto);

            if (resultado == null)
            {
                Direccion = "";
            }
            else
            {
                Direccion = resultado.Direccion;
            }
            return Json(Direccion, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}