using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferService.Application_Service.DTOs
{
    public class CardTransferDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
    }
}
