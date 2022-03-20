using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Entities
{
    public class EndangeredAnimal
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }
    }
}
