using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TestProgrammingSchad.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        [Required]
        public string CustName { get; set; }
        public string Adress { get; set; }
        public bool? Status { get; set; }
        public int CustomerTypeId { get; set; }

        public virtual CustomerType CustomerType { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
