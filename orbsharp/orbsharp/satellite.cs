using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orb
{
    public class Satellite
    {
		string line1, line2;
		OrbitalElements ele;

		public Satellite (TLE tle)
		{
			ele = new OrbitalElements(tle);
		}

		public Satellite (string[] _tle)
		{
			var tle = new TLE();
			tle.Name = _tle[0];
			tle.FirstLine = _tle[1];
			tle.SecondLine = _tle[2];

			ele = new OrbitalElements(tle);
		}
    }
}
