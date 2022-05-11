using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestProgrammingSchad.Helpers;
using TestProgrammingSchad.Models;

namespace TestProgrammingSchad.Controllers
{
    public class InvoiceDetailsController : Controller
    {
        private const double ITBIS = 0.18;
        private readonly Test_InvoiceContext _context;

        public InvoiceDetailsController(Test_InvoiceContext context)
        {
            _context = context;
        }

        // GET: InvoiceDetails
        public async Task<IActionResult> Index()
        {
            var test_InvoiceContext = _context.InvoiceDetails.Include(i => i.Invoice);
            return View(await test_InvoiceContext.ToListAsync());
        }

        // GET: InvoiceDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewData["InvoiceId"] = new SelectList(_context.Invoices.Include("Customer"), "Id", "Customer.CustName");
            }
            else
            {
                ViewData["InvoiceId"] = new SelectList(_context.Invoices.Include("Customer"), "Id", "Customer.CustName", id);
            }

            return View();
        }

        // POST: InvoiceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceId,Qty,Price,TotalItbis,SubTotal,Total")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                Util.CalculateItemTotal(invoiceDetail);
                _context.Add(invoiceDetail);

                Invoice invoice = _context.Invoices.Include(x=>x.InvoiceDetails).FirstOrDefault(x => x.Id == invoiceDetail.InvoiceId);
                Util.ReCalculateInvoiceTotals(invoice, invoiceDetail);
                _context.Update(invoice);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceDetail.InvoiceId);
            return View(invoiceDetail);
        }



        // GET: InvoiceDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceDetail.InvoiceId);
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceId,Qty,Price,TotalItbis,SubTotal,Total")] InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Util.CalculateItemTotal(invoiceDetail);
                    _context.Update(invoiceDetail);

                    var invoice = _context.Invoices.Include(x => x.InvoiceDetails).FirstOrDefault(x => x.Id == invoiceDetail.InvoiceId);
                    Util.ReCalculateInvoiceTotals(invoice, invoiceDetail);

                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.Id))
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
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceDetail.InvoiceId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            _context.InvoiceDetails.Remove(invoiceDetail);
            await _context.SaveChangesAsync();

            var invoice = _context.Invoices.Include(x => x.InvoiceDetails).FirstOrDefault(x => x.Id == invoiceDetail.InvoiceId);
            Util.ReCalculateInvoiceTotals(invoice, invoiceDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceDetailExists(int id)
        {
            return _context.InvoiceDetails.Any(e => e.Id == id);
        }
    }
}
