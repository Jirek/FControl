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
    public class TransaccionController : Controller
    {
        // GET: Transaccion
        private FinancialControlEntities db = new FinancialControlEntities();
       
        public ActionResult Index(int? id)
        {
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            var transaccion = db.Transaccion.Where(d => d.UsuarioID == userId).Include("Cuenta");
           
            return View(transaccion.ToList());
        }
       
        public ActionResult Crear()
        {
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            ViewBag.BenefeciarioID = new SelectList(db.Beneficiario.Where(d => d.UsuarioID == userId), "BeneficiarioID", "Nombre");

            ViewBag.CuentaID = new SelectList(db.Cuenta.Where(d => d.UsuarioID == userId || d.CompartidoID == userId),"CuentaID","Nombre");

            ViewBag.DivisaID = new SelectList(db.Divisa, "DivisaID", "Nombre");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Crear( Transaccion transaccion)
        {
            if (ModelState.IsValid)
            {
                if (transaccion.UsuarioID == null)
                {
                    object fc = Session["UsuarioID"].ToString();
                    transaccion.FechaTransaccion = DateTime.Now;
                    transaccion.UsuarioID = Convert.ToInt16(fc);
                    transaccion.Concepto = " : " + transaccion.Concepto;
                    if (transaccion.TipoTransaccion == 2 )
                    {
                        transaccion.ImporteOtraDivisa = transaccion.Importe*-1;
                    }
                    
                    db.Transaccion.Add(transaccion);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            int userId = Convert.ToInt16(Session["UsuarioID"]);
            ViewBag.BenefeciarioID = new SelectList(db.Transaccion.Where(d => d.UsuarioID == userId), "BeneficiarioID", "Nombre", transaccion.BenefeciarioID);
            ViewBag.CuentaID = new SelectList(db.Cuenta.Where(d => d.UsuarioID == userId),"DivisaId","Nombre");
            ViewBag.DivisaID = new SelectList(db.Divisa, "DivisaID", "Nombre", transaccion.CuentaID);
     

            return View(transaccion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuenta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cuenta);
        }
        
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaccion transaccion = db.Transaccion.Find(id);
            if (transaccion == null)
            {
                return HttpNotFound();
            }
            return View(transaccion);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaccion transaccion = db.Transaccion.Find(id);
            db.Transaccion.Remove(transaccion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}