using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal.DataAccess.Exceptions
{
	public class DuplicateUserTagException : Exception
	{
		public DuplicateUserTagException() : base("This Tag is already taken!") { }
	}
}
