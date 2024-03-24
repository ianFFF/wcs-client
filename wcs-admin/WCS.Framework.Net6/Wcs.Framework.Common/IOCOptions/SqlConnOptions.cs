using System;
using System.Collections.Generic;

namespace Wcs.Framework.Common.IOCOptions
{
	public class SqlConnOptions
	{
		public string WriteUrl { get; set; }
		public List<string> ReadUrl { get; set; }
	}
}
