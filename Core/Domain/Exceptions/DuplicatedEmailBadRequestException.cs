using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DuplicatedEmailBadRequestException(string email) : BadRequestException($"There Is Already Email Found As This {email}")
    {
    }
}