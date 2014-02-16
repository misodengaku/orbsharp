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

		private SGP4Data SetSGP4(OrbitalElements orbital_elements)
		{
			var sgp = new SGP4Data();
			var torad = Math.PI / 180;
			var ck2 = 5.413080e-4;
			var ck4 = 0.62098875e-6;
			var e6a = 1.0e-6;
			var qoms2t = 1.88027916e-9;
			var s = 1.01222928; // 1.0+78.0/xkmper
			var tothrd = 0.66666667;
			var xj3 = -0.253881e-5;
			var xke = 0.743669161e-1;
			var xkmper = 6378.135;
			var xmnpda = 1440.0; // min_par_day
			var ae = 1.0;
			var pi = Math.PI;
			var pio2 = pi / 2;
			var twopi = pi * 2;
			var x3pio2 = pio2 * 3;

			sgp.epoch = orbital_elements.Epoch;
			sgp.epoch_year = orbital_elements.EpochYear;
			sgp.bstar = orbital_elements.Bstar;
			sgp.xincl = orbital_elements.Inclination * torad;
			sgp.xnodeo = orbital_elements.RightAscension * torad;
			sgp.eo = orbital_elements.Eccentricity * 1e-7;
			sgp.omegao = orbital_elements.ArgumentOfPerigee * torad;
			sgp.xmo = orbital_elements.MeanAnomaly * torad;
			var xno = orbital_elements.MeanMotion * 2.0 * Math.PI / 1440.0;
			var a1 = Math.Pow(xke / xno, tothrd);
			sgp.cosio = Math.Cos(sgp.xincl);
			var theta2 = sgp.cosio * sgp.cosio;
			sgp.x3thm1 = 3 * theta2 - 1.0;
			var eosq = sgp.eo * sgp.eo;
			var betao2 = 1 - eosq;
			var betao = Math.Sqrt(betao2);
			var del1 = 1.5 * ck2 * sgp.x3thm1 / (a1 * a1 * betao * betao2);
			var ao = a1 * (1 - del1 * ((1.0 / 3.0) + del1 * (1.0 + (134.0 / 81.0) * del1)));
			var delo = 1.5 * ck2 * sgp.x3thm1 / (ao * ao * betao * betao2);
			sgp.xnodp = xno / (1.0 + delo); //original_mean_motion
			sgp.aodp = ao / (1.0 - delo); //semi_major_axis


			sgp.orbital_period = 1440.0 / orbital_elements.MeanMotion;

			sgp.isimp = 0;
			if ((sgp.aodp * (1.0 - sgp.eo) / ae) < (220.0 / xkmper + ae))
			{
				sgp.isimp = 1;
			}

			var s4 = s;
			var qoms24 = qoms2t;
			sgp.perige = (sgp.aodp * (1.0 - sgp.eo) - ae) * xkmper;
			sgp.apoge = (sgp.aodp * (1.0 + sgp.eo) - ae) * xkmper;
			if (sgp.perige < 156.0)
			{
				s4 = sgp.perige - 78.0;
				if (sgp.perige <= 98.0)
				{
					s4 = 20.0;
				}
				else
				{
					qoms24 = Math.Pow(((120.0 - s4) * ae / xkmper), 4);
					s4 = s4 / xkmper + ae;
				}
			}
			var pinvsq = 1.0 / (sgp.aodp * sgp.aodp * betao2 * betao2);
			var tsi = 1.0 / (sgp.aodp - s4);
			sgp.eta = sgp.aodp * sgp.eo * tsi;
			var etasq = sgp.eta * sgp.eta;
			var eeta = sgp.eo * sgp.eta;
			var psisq = Math.Abs(1.0 - etasq);
			var coef = qoms24 * Math.Pow(tsi, 4);
			var coef1 = coef / Math.Pow(psisq, 3.5);
			var c2 = coef1 * sgp.xnodp * (sgp.aodp * (1.0 + 1.5 * etasq + eeta * (4.0 + etasq)) + 0.75 * ck2 * tsi / psisq * sgp.x3thm1 * (8.0 + 3.0 * etasq * (8.0 + etasq)));
			sgp.c1 = sgp.bstar * c2;
			sgp.sinio = Math.Sin(sgp.xincl);
			var a3ovk2 = -xj3 / ck2 * Math.Pow(ae, 3);
			var c3 = coef * tsi * a3ovk2 * sgp.xnodp * ae * sgp.sinio / sgp.eo;
			sgp.x1mth2 = 1.0 - theta2;
			sgp.c4 = 2.0 * sgp.xnodp * coef1 * sgp.aodp * betao2 * (sgp.eta * (2.0 + 0.5 * etasq) + sgp.eo * (0.5 + 2.0 * etasq) - 2.0 * ck2 * tsi / (sgp.aodp * psisq) * (-3.0 * sgp.x3thm1 * (1.0 - 2.0 * eeta + etasq * (1.5 - 0.5 * eeta)) + 0.75 * sgp.x1mth2 * (2.0 * etasq - eeta * (1.0 + etasq)) * Math.Cos((2.0 * sgp.omegao))));
			sgp.c5 = 2.0 * coef1 * sgp.aodp * betao2 * (1.0 + 2.75 * (etasq + eeta) + eeta * etasq);

			var theta4 = theta2 * theta2;
			var temp1 = 3.0 * ck2 * pinvsq * sgp.xnodp;
			var temp2 = temp1 * ck2 * pinvsq;
			var temp3 = 1.25 * ck4 * pinvsq * pinvsq * sgp.xnodp;
			sgp.xmdot = sgp.xnodp + 0.5 * temp1 * betao * sgp.x3thm1 + 0.0625 * temp2 * betao * (13.0 - 78.0 * theta2 + 137.0 * theta4);

			var x1m5th = 1.0 - 5.0 * theta2;
			sgp.omgdot = -0.5 * temp1 * x1m5th + 0.0625 * temp2 * (7.0 - 114.0 * theta2 + 395.0 * theta4) + temp3 * (3.0 - 36.0 * theta2 + 49.0 * theta4);
			var xhdot1 = -temp1 * sgp.cosio;
			sgp.xnodot = xhdot1 + (0.5 * temp2 * (4.0 - 19.0 * theta2) + 2.0 * temp3 * (3.0 - 7.0 * theta2)) * sgp.cosio;
			sgp.omgcof = sgp.bstar * c3 * Math.Cos(sgp.omegao);
			sgp.xmcof = -tothrd * coef * sgp.bstar * ae / eeta;
			sgp.xnodcf = 3.5 * betao2 * xhdot1 * sgp.c1;
			sgp.t2cof = 1.5 * sgp.c1;
			sgp.xlcof = 0.125 * a3ovk2 * sgp.sinio * (3.0 + 5.0 * sgp.cosio) / (1.0 + sgp.cosio);
			sgp.aycof = 0.25 * a3ovk2 * sgp.sinio;
			sgp.delmo = Math.Pow((1.0 + sgp.eta * Math.Cos(sgp.xmo)), 3);
			sgp.sinmo = Math.Sin(sgp.xmo);
			sgp.x7thm1 = 7.0 * theta2 - 1.0;

			if (sgp.isimp != 1)
			{
				var c1sq = sgp.c1 * sgp.c1;
				sgp.d2 = 4.0 * sgp.aodp * tsi * c1sq;
				var temp = sgp.d2 * tsi * sgp.c1 / 3.0;
				sgp.d3 = (17.0 * sgp.aodp + s4) * temp;
				sgp.d4 = 0.5 * temp * sgp.aodp * tsi * (221.0 * sgp.aodp + 31.0 * s4) * sgp.c1;
				sgp.t3cof = sgp.d2 + 2.0 * c1sq;
				sgp.t4cof = 0.25 * (3.0 * sgp.d3 + sgp.c1 * (12.0 * sgp.d2 + 10.0 * c1sq));
				sgp.t5cof = 0.2 * (3.0 * sgp.d4 + 12.0 * sgp.c1 * sgp.d3 + 6.0 * sgp.d2 * sgp.d2 + 15.0 * c1sq * (2.0 * sgp.d2 + c1sq));
			}
			sgp.epoch_in_date = new DateTime(sgp.epoch_year - 1, 11, 31, 0, 0, 0, DateTimeKind.Utc);
			sgp.epoch_in_date.AddSeconds(sgp.epoch * 24 * 60 * 60);

			return sgp;
		}
    }
}
