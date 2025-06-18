using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities;
internal class Permisstion
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; }
    public DateTime CreatedDate { get; set; }

}
