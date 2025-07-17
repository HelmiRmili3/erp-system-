using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities;
public class Role
{
    public int Id { get; set; }
    public required string UserRole { get; set; }
    public required string RoleName { get; set; }
    public required string RoleDescription { get; set; }


}
