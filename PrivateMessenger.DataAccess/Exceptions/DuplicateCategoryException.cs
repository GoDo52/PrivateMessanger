using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Exceptions
{
    public class DuplicateCategoryException : Exception
    {
        public DuplicateCategoryException() : base("Category with the same name already exists.") { }
    }
}
