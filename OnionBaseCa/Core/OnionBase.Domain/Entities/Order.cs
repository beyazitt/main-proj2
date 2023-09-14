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
    public class Order:BaseEntity
    {
        [Key]
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public string Address { get; set; }
        public ICollection<Product> Products { get; set; }
        public int Phone { get; set; }  
        public bool confirmationRequest { get; set;}
        public bool isConfirmed { get; set; }
        public AppUser Users { get; set; }
        public int ProductCode { get; set; }
        public string? usedCampaignCode { get; set; }
        public ICollection<Campaigns> Campaigns { get; set; }

    }
}
