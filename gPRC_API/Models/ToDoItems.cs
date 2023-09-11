using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gPRC_API.Models
{
    public class ToDOItems
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        
    }
}