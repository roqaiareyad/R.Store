﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFoundException(string email)
           : NotFoundException($"User With Email {email} Not Found !!")
    {
    }
}