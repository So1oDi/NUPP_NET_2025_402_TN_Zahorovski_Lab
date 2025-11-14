using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyApp.Infrastructure;
using PharmacyApp.Infrastructure.Models;

namespace Pharmacy.MVC.Controllers
{
    public class PrescriptionsController : Controller
    {
        private readonly PharmacyAppContext _context;

        public PrescriptionsController(PharmacyAppContext context)
        {
            _context = context;
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index()
        {
            var pharmacyAppContext = _context.Prescriptions.Include(p => p.Customer);
            return View(await pharmacyAppContext.ToListAsync());
        }

        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionModel = await _context.Prescriptions
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescriptionModel == null)
            {
                return NotFound();
            }

            return View(prescriptionModel);
        }

        // GET: Prescriptions/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address");
            return View();
        }

        // POST: Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorName,PrescriptionDate,CustomerId,Id,Name,Price,QuantityInStock")] PrescriptionModel prescriptionModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prescriptionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", prescriptionModel.CustomerId);
            return View(prescriptionModel);
        }

        // GET: Prescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionModel = await _context.Prescriptions.FindAsync(id);
            if (prescriptionModel == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", prescriptionModel.CustomerId);
            return View(prescriptionModel);
        }

        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorName,PrescriptionDate,CustomerId,Id,Name,Price,QuantityInStock")] PrescriptionModel prescriptionModel)
        {
            if (id != prescriptionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescriptionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionModelExists(prescriptionModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", prescriptionModel.CustomerId);
            return View(prescriptionModel);
        }

        // GET: Prescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionModel = await _context.Prescriptions
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prescriptionModel == null)
            {
                return NotFound();
            }

            return View(prescriptionModel);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescriptionModel = await _context.Prescriptions.FindAsync(id);
            if (prescriptionModel != null)
            {
                _context.Prescriptions.Remove(prescriptionModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionModelExists(int id)
        {
            return _context.Prescriptions.Any(e => e.Id == id);
        }
    }
}
