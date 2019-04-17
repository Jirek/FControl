using FControl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FControl.Controllers
{
    public class TraspasoController : Controller
    {
        private FinancialControlEntities db = new FinancialControlEntities();

        public ActionResult Index(int? id)
        {
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            var traspaso = db.Traspaso.Where(d => d.UsuarioID == userId).Include("Cuenta");

            return View(traspaso.ToList());
        }

        public ActionResult Crear()
        {
            int userId = Convert.ToInt16(Session["UsuarioID"]);

            ViewBag.CuentaOrigenID = db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId).ToList();
            ViewBag.CuentaDestinoID = db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId).ToList();
            ViewBag.DivisaID = db.Divisa.ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear( Traspaso traspaso, Transaccion transaccion,Transaccion transaccion1)
        {
            if (ModelState.IsValid)
            {
                if (traspaso.UsuarioID == null)
                {
                    object fc = Session["UsuarioID"].ToString();
                    traspaso.UsuarioID = Convert.ToInt16(fc);
                    db.Traspaso.Add(traspaso);
                    db.SaveChanges();

                    transaccion.FechaTransaccion = DateTime.Now;
                    transaccion.UsuarioID= Convert.ToInt16(fc);
                    transaccion.CuentaID = traspaso.CuentaOrigenID;

                    var dato = db.Cuenta.Find(traspaso.CuentaDestinoID);

                    transaccion.Concepto = "<<" + dato.Nombre;
                    transaccion.Importe = traspaso.Monto * -1;
                    transaccion.ImporteOtraDivisa = traspaso.Monto * -1;
                    db.Transaccion.Add(transaccion);
                    db.SaveChanges();

                    var dato1 = db.Cuenta.Find(traspaso.CuentaOrigenID);

                    transaccion1.FechaTransaccion= DateTime.Now; ;
                    transaccion1.UsuarioID= Convert.ToInt16(fc);
                    transaccion1.CuentaID = traspaso.CuentaDestinoID;
                    transaccion1.Concepto = ">>" + dato1.Nombre;
                    transaccion1.Importe = traspaso.Monto;
                    transaccion1.ImporteOtraDivisa = traspaso.Monto;
                    db.Transaccion.Add(transaccion1);
                    db.SaveChanges();
                }
            }
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            ViewBag.CuentaOrigenID = db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId).ToList();
            ViewBag.CuentaDestinoID = db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId).ToList();
            ViewBag.DivisaID = db.Divisa.ToList();
            return View(traspaso);
        }
       
    }
}
