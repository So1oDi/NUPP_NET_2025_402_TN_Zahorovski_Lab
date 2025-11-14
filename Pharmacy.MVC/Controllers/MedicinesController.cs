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
    public class MedicinesController : Controller
    {
        private readonly PharmacyAppContext _context;

        public MedicinesController(PharmacyAppContext context)
        {
            _context = context;
        }

        // GET: Medicines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicines.ToListAsync());
        }

        // GET: Medicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineModel = await _context.Medicines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicineModel == null)
            {
                return NotFound();
            }

            return View(medicineModel);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,QuantityInStock")] MedicineModel medicineModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicineModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicineModel);
        }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineModel = await _context.Medicines.FindAsync(id);
            if (medicineModel == null)
            {
                return NotFound();
            }
            return View(medicineModel);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,QuantityInStock")] MedicineModel medicineModel)
        {
            if (id != medicineModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicineModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineModelExists(medicineModel.Id))
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
            return View(medicineModel);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineModel = await _context.Medicines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicineModel == null)
            {
                return NotFound();
            }

            return View(medicineModel);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicineModel = await _context.Medicines.FindAsync(id);
            if (medicineModel != null)
            {
                _context.Medicines.Remove(medicineModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineModelExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
