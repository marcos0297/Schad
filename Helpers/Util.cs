using System.Collections.Generic;
using System.Linq;
using TestProgrammingSchad.Models;

namespace TestProgrammingSchad.Helpers
{
    public static class Util
    {
        private const double ITBIS = 0.18;

        public static void CalculateInvoiceTotal(Invoice invoice)
        {
            invoice.TotalItbis = invoice.SubTotal * (decimal)ITBIS;

            invoice.Total = invoice.SubTotal + invoice.TotalItbis;
        }

        public static void CalculateItemTotal(InvoiceDetail invoiceDetail)
        {
            decimal cantidad = (invoiceDetail.Price * invoiceDetail.Qty);

            invoiceDetail.SubTotal = cantidad;
            invoiceDetail.TotalItbis = cantidad * (decimal)ITBIS;
            invoiceDetail.Total = invoiceDetail.SubTotal + invoiceDetail.TotalItbis;
        }

        public static void ReCalculateInvoiceTotals(Invoice invoice, InvoiceDetail invoiceDetail)
        {
            var invoiceList = new List<InvoiceDetail>();

            foreach (InvoiceDetail item in invoice.InvoiceDetails.ToList())
            {
                if (item.Id == invoiceDetail.Id)
                {
                    invoiceList.Add(invoiceDetail);
                    continue;
                }
                invoiceList.Add(item);
            }

            if (invoiceList != null && invoiceList.Count > 0)
            {
                decimal subTotal = default;
                decimal totalItbis = default;
                decimal total = default;

                foreach (InvoiceDetail item in invoiceList)
                {
                    decimal cantidad = (item.Price * item.Qty);
                    subTotal += cantidad;
                    totalItbis += cantidad * (decimal)ITBIS;
                    total += subTotal + totalItbis;
                }

                invoice.SubTotal = subTotal;
                invoice.TotalItbis = totalItbis;
                invoice.Total = total;
            }
        }
    }
}
