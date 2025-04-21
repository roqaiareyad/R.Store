using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BasketDeletebadRequest() :
        BadRequestException($"Invalid Operation when Delete basket !")
    {
    }
}