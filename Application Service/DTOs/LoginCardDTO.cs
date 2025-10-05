using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Application_Service.DTOs
{
    public class LoginCardDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool? IsSuccess { get; set; }

    }
}
