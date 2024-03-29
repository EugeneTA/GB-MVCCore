﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DAL.Entities
{
    [Table("Orders")]
    public class Order : Entity
    {
        public DateTime OrderDate { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public Buyer Buyer { get; set; } = null!;

        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

    }
}
