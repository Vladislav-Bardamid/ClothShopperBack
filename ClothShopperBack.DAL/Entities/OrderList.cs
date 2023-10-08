using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Entities
{
    public class OrderList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime? CommitDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}