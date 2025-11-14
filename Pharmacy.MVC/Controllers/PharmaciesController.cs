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
    public class PharmaciesController : Controller
    {
        private readonly PharmacyAppContext _context;

        public PharmaciesController(PharmacyAppContext context)
        {
            _context = context;
        }

        // GET: Pharmacies
        public async Task<IActionResult> Index()
        {
            var pharmacyAppContext = _context.Pharmacies.Include(p => p.ContactCustomer);
            return View(await pharmacyAppContext.ToListAsync());
        }

        // GET: Pharmacies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacyModel = await _context.Pharmacies
                .Include(p => p.ContactCustomer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmacyModel == null)
            {
                return NotFound();
            }

            return View(pharmacyModel);
        }

        // GET: Pharmacies/Create
        public IActionResult Create()
        {
            ViewData["ContactCustomerId"] = new SelectList(_context.Customers, "Id", "Address");
            return View();
        }

        // POST: Pharmacies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ContactCustomerId,Address")] PharmacyModel pharmacyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pharmacyModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactCustomerId"] = new SelectList(_context.Customers, "Id", "Address", pharmacyModel.ContactCustomerId);
            return View(pharmacyModel);
        }

        // GET: Pharmacies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacyModel = await _context.Pharmacies.FindAsync(id);
            if (pharmacyModel == null)
            {
                return NotFound();
            }
            ViewData["ContactCustomerId"] = new SelectList(_context.Customers, "Id", "Address", pharmacyModel.ContactCustomerId);
            return View(pharmacyModel);
        }

        // POST: Pharmacies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContactCustomerId,Address")] PharmacyModel pharmacyModel)
        {
            if (id != pharmacyModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacyModelExists(pharmacyModel.Id))
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
            ViewData["ContactCustomerId"] = new SelectList(_context.Customers, "Id", "Address", pharmacyModel.ContactCustomerId);
            return View(pharmacyModel);
        }

        // GET: Pharmacies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacyModel = await _context.Pharmacies
                .Include(p => p.ContactCustomer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmacyModel == null)
            {
                return NotFound();
            }

            return View(pharmacyModel);
        }

        // POST: Pharmacies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pharmacyModel = await _context.Pharmacies.FindAsync(id);
            if (pharmacyModel != null)
            {
                _context.Pharmacies.Remove(pharmacyModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PharmacyModelExists(int id)
        {
            return _context.Pharmacies.Any(e => e.Id == id);
        }
    }
}
