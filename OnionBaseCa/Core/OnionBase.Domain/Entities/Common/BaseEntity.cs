using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionBase.Domain.Entities.Common
{
    public class BaseEntity
    {
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set;}
        public Guid Id { get; set; }
    }
}
