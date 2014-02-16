using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orb
{
    public class Satellite
    {
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

		private SGP4ExecuteData SetSGP4(OrbitalElements orbital_elements)
		{
			var sgp = new SGP4ExecuteData();
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

		private double CalculateTsince(Time time, OrbitalElements orbital_elements)
		{
			var epoch_year = orbital_elements.EpochYear;
			var epoch = orbital_elements.Epoch;
			var year2 = epoch_year - 1;
			var now_sec = new DateTime(time.Year, time.Month - 1, time.Day, time.Hours, time.Minutes, time.Seconds, DateTimeKind.Utc);
			var epoch_sec = new DateTime(year2, 11, 31, 0, 0, 0, DateTimeKind.Utc);
			epoch_sec.AddSeconds(epoch * 24 * 60 * 60);
			
			var elapsed_time = (now_sec - epoch_sec).TotalMilliseconds / (60 * 1000);
			return elapsed_time;
		}

		private SGP4CalculatedData ExecuteSGP4(Time time, SGP4ExecuteData sgp4)
		{
			var calculated = new SGP4CalculatedData();

			var rad = Math.PI / 180;
			var orbital_elements = sgp4.orbital_elements;
			var tsince = CalculateTsince(time, orbital_elements);

			var xmo = sgp4.xmo;
			var xmdot = sgp4.xmdot;
			var omegao = sgp4.omegao;
			var omgdot = sgp4.omgdot;
			var xnodeo = sgp4.xnodeo;
			var xnodot = sgp4.xnodot;
			var xnodcf = sgp4.xnodcf;
			var bstar = sgp4.bstar;
			var t2cof = sgp4.t2cof;
			var omgcof = sgp4.omgcof;
			var isimp = sgp4.isimp;
			var xmcof = sgp4.xmcof;
			var eta = sgp4.eta;
			var delmo = sgp4.delmo;
			var c1 = sgp4.c1;
			var c4 = sgp4.c4;
			var c5 = sgp4.c5;
			var d2 = sgp4.d2;
			var d3 = sgp4.d3;
			var d4 = sgp4.d4;
			var sinmo = sgp4.sinmo;
			var t3cof = sgp4.t3cof;
			var t4cof = sgp4.t4cof;
			var t5cof = sgp4.t5cof;
			var aodp = sgp4.aodp;
			var eo = sgp4.eo;
			var xnodp = sgp4.xnodp;
			var xke = sgp4.xke;
			var xlcof = sgp4.xlcof;
			var aycof = sgp4.aycof;
			var x3thm1 = sgp4.x3thm1;
			var x1mth2 = sgp4.x1mth2;
			var xincl = sgp4.xincl;
			var cosio = sgp4.cosio;
			var sinio = sgp4.sinio;
			var e6a = sgp4.e6a;
			var ck2 = sgp4.ck2;
			var x7thm1 = sgp4.x7thm1;
			var xkmper = sgp4.xkmper;
			var epoch_year = sgp4.epoch_year;
			var epoch = sgp4.epoch;

			var xmdf = xmo + xmdot * tsince;
			var omgadf = omegao + omgdot * tsince;
			var xnoddf = xnodeo + xnodot * tsince;
			var omega = omgadf;
			var xmp = xmdf;
			var tsq = tsince * tsince;
			var xnode = xnoddf + xnodcf * tsq;
			var tempa = 1.0 - c1 * tsince;
			var tempe = bstar * c4 * tsince;
			var templ = t2cof * tsq;
			double temp;

			if (isimp != 1)
			{
				var delomg = omgcof * tsince;
				var delm = xmcof * (Math.Pow((1.0 + eta * Math.Cos(xmdf)), 3) - delmo);
				temp = delomg + delm;
				xmp = xmdf + temp;
				omega = omgadf - temp;
				var tcube = tsq * tsince;
				var tfour = tsince * tcube;
				tempa = tempa - d2 * tsq - d3 * tcube - d4 * tfour;
				tempe = tempe + bstar * c5 * (Math.Sin(xmp) - sinmo);
				templ = templ + t3cof * tcube + tfour * (t4cof + tsince * t5cof);
			}
			var a = aodp * tempa * tempa;
			var e = eo - tempe;
			var xl = xmp + omega + xnode + xnodp * templ;
			var beta = Math.Sqrt(1.0 - e * e);
			var xn = xke / Math.Pow(a, 1.5);

			// long period periodics
			var axn = e * Math.Cos(omega);
			temp = 1.0 / (a * beta * beta);
			var xll = temp * xlcof * axn;
			var aynl = temp * aycof;
			var xlt = xl + xll;
			var ayn = e * Math.Sin(omega) + aynl;

			// solve keplers equation
			var capu = (xlt - xnode) % (2.0 * Math.PI);
			var temp2 = capu;
			double temp3 = 0, temp4 = 0, temp5 = 0, temp6 = 0;
			double sinepw = 0, cosepw = 0;
			for (var i = 1; i <= 10; i++)
			{
				sinepw = Math.Sin(temp2);
				cosepw = Math.Cos(temp2);
				temp3 = axn * sinepw;
				temp4 = ayn * cosepw;
				temp5 = axn * cosepw;
				temp6 = ayn * sinepw;
				var epw = (capu - temp4 + temp3 - temp2) / (1.0 - temp5 - temp6) + temp2;
				if (Math.Abs(epw - temp2) <= e6a)
				{
					break;
				};
				temp2 = epw;
			}
			// short period preliminary quantities

			var ecose = temp5 + temp6;
			var esine = temp3 - temp4;
			var elsq = axn * axn + ayn * ayn;
			temp = 1.0 - elsq;
			var pl = a * temp;
			var r = a * (1.0 - ecose);
			var temp1 = 1.0 / r;
			var rdot = xke * Math.Sqrt(a) * esine * temp1;
			var rfdot = xke * Math.Sqrt(pl) * temp1;
			temp2 = a * temp1;
			var betal = Math.Sqrt(temp);
			temp3 = 1.0 / (1.0 + betal);
			var cosu = temp2 * (cosepw - axn + ayn * esine * temp3);
			var sinu = temp2 * (sinepw - ayn - axn * esine * temp3);
			var u = Math.Atan2(sinu, cosu);
			if (u < 0) { u += 2 * Math.PI; }
			var sin2u = 2.0 * sinu * cosu;
			// var cos2u=2.0*cosu*cosu-1.; // ???
			var cos2u = 2.0 * cosu * cosu - 1.0;
			temp = 1.0 / pl;
			temp1 = ck2 * temp;
			temp2 = temp1 * temp;

			// update for short periodics

			var rk = r * (1.0 - 1.5 * temp2 * betal * x3thm1) + 0.5 * temp1 * x1mth2 * cos2u;
			var uk = u - 0.25 * temp2 * x7thm1 * sin2u;
			var xnodek = xnode + 1.5 * temp2 * cosio * sin2u;
			var xinck = xincl + 1.5 * temp2 * cosio * sinio * cos2u;
			var rdotk = rdot - xn * temp1 * x1mth2 * sin2u;
			var rfdotk = rfdot + xn * temp1 * (x1mth2 * cos2u + 1.5 * x3thm1);

			// orientation vectors

			var sinuk = Math.Sin(uk);
			var cosuk = Math.Cos(uk);
			var sinik = Math.Sin(xinck);
			var cosik = Math.Cos(xinck);
			var sinnok = Math.Sin(xnodek);
			var cosnok = Math.Cos(xnodek);
			var xmx = -sinnok * cosik;
			var xmy = cosnok * cosik;
			var ux = xmx * sinuk + cosnok * cosuk;
			var uy = xmy * sinuk + sinnok * cosuk;
			var uz = sinik * sinuk;
			var vx = xmx * cosuk - cosnok * sinuk;
			var vy = xmy * cosuk - sinnok * sinuk;
			var vz = sinik * cosuk;
			var x = rk * ux;
			var y = rk * uy;
			var z = rk * uz;
			var xdot = rdotk * ux + rfdotk * vx;
			var ydot = rdotk * uy + rfdotk * vy;
			var zdot = rdotk * uz + rfdotk * vz;


			calculated.x = (x * xkmper);
			calculated.y = (y * xkmper);
			calculated.z = (z * xkmper);
			calculated.xdot = (xdot * xkmper / 60);
			calculated.ydot = (ydot * xkmper / 60);
			calculated.zdot = (zdot * xkmper / 60);


			return calculated;
		}

		private Geographic CalculatedDataToGeographic(Time _time, SGP4CalculatedData rect)
		{
			var time = _time;
			var xkm = rect.x;
			var ykm = rect.y;
			var zkm = rect.z;
			var xdotkmps = rect.xdot;
			var ydotkmps = rect.ydot;
			var zdotkmps = rect.zdot;
			var rad = Math.PI / 180;
			var gmst = time.Gmst;
			var lst = gmst * 15;
			var f = 0.00335277945; //Earth's flattening term in WGS-72 (= 1/298.26)
			var a = 6378.135;	//Earth's equational radius in WGS-72 (km)
			var r = Math.Sqrt(xkm * xkm + ykm * ykm);
			var lng = Math.Atan2(ykm, xkm) / rad - lst;
			if (lng > 360)
				lng = lng % 360;
			if (lng < 0)
				lng = lng % 360 + 360;
			if (lng > 180)
				lng = lng - 360;

			var lat = Math.Atan2(zkm, r);
			var e2 = f * (2 - f);
			var tmp_lat = 0.0;
			double c;

			do
			{
				tmp_lat = lat;
				var sin_lat = Math.Sin(tmp_lat);
				c = 1 / Math.Sqrt(1 - e2 * sin_lat * sin_lat);
				lat = Math.Atan2(zkm + a * c * e2 * (Math.Sin(tmp_lat)), r);
			} while (Math.Abs(lat - tmp_lat) > 0.0001);
			var alt = r / Math.Cos(lat) - a * c;
			var v = Math.Sqrt(xdotkmps * xdotkmps + ydotkmps * ydotkmps + zdotkmps * zdotkmps);

			var geo = new Geographic();
			geo.longitute = lng;
			geo.latitute = lat / rad;
			geo.altitude = alt;
			geo.velocity = v;
			return geo;
		}
    }
}
