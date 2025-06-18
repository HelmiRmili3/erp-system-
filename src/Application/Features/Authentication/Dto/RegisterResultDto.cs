using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Features.Authentication.Dto;
public class RegisterResultDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
}
