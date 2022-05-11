using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestProgrammingSchad.Models;

namespace TestProgrammingSchad.Controllers
{
    public class CustomerTypesController : Controller
    {
        private readonly Test_InvoiceContext _context;

        public CustomerTypesController(Test_InvoiceContext context)
        {
            _context = context;
        }

        // GET: CustomerTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerTypes.ToListAsync());
        }

        // GET: CustomerTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerType = await _context.CustomerTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerType == null)
            {
                return NotFound();
            }

            return View(customerType);
        }

        // GET: CustomerTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] CustomerType customerType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerType);
        }

        // GET: CustomerTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerType = await _context.CustomerTypes.FindAsync(id);
            if (customerType == null)
            {
                return NotFound();
            }
            return View(customerType);
        }

        // POST: CustomerTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] CustomerType customerType)
        {
            if (id != customerType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerTypeExists(customerType.Id))
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
            return View(customerType);
        }

        // GET: CustomerTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerType = await _context.CustomerTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerType == null)
            {
                return NotFound();
            }

            return View(customerType);
        }

        // POST: CustomerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerType = await _context.CustomerTypes.FindAsync(id);
            _context.CustomerTypes.Remove(customerType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerTypeExists(int id)
        {
            return _context.CustomerTypes.Any(e => e.Id == id);
        }
    }
}
