using System;
using System.Collections.Generic;

#nullable disable

namespace TestProgrammingSchad.Models
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
