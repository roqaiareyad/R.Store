using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorsModels
{

    public class ValidationError
    {
        public string Feild { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
