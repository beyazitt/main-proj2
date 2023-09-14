using OnionBase.Domain.Entities.Common;
using OnionBase.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionBase.Domain.Entities
{
    public class Campaigns : BaseEntity
    {
        [Key]
        public Guid campaignId {get; set;}
        public bool isActive { get; set; }
        public float discountRate { get; set; }
        public string discountCode { get; set; }
        public int piece { get; set; }
        public ICollection<Order> order { get; set; }
        public ICollection<AppUser> user { get; set; }
        public ICollection<Product> product { get; set; }
        
    }
}
