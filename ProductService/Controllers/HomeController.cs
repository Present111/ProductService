using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductService.Controllers
{
    public class HomeController : Controller
    {
        private DBContextDataContext _context;
        public HomeController()
        {
            // Retrieve the connection string from Web.config
            var connectionString = ConfigurationManager.ConnectionStrings["SOADemoConnectionString"].ConnectionString;
            _context = new DBContextDataContext(connectionString);
        }
        public ActionResult ListProduct()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.InsertOnSubmit(product);
                _context.SubmitChanges();
                return RedirectToAction("ListProduct");
            }
            return View(product);
        }
        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.SingleOrDefault(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                // Update product properties
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.CreatedDate = product.CreatedDate;

                _context.SubmitChanges();
                return RedirectToAction("ListProduct");
            }
            return View(product);
        }
        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Check if there are any orders associated with this product
            var hasOrders = _context.Orders.Any(o => o.ProductId == id);
            if (hasOrders)
            {
                // Prevent deletion and show an error message
                TempData["ErrorMessage"] = "Cannot delete this product because there are existing orders associated with it.";
                return RedirectToAction("ListProduct");
            }

            // No orders associated, safe to delete
            _context.Products.DeleteOnSubmit(product);
            _context.SubmitChanges();

            return RedirectToAction("ListProduct");
        }
    }
}