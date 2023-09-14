using OnionBase.Domain.Entities.Common;
using OnionBase.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionBase.Domain.Entities
{
    public class Question:BaseEntity
    {
        public AppUser Users { get; set; }
        public string QuestionBody { get; set; }
        public string? Answer { get; set; }
    }
}
