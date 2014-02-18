using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orb
{
	public class Time
	{
		DateTime date, et;
		TimeSpan timezone;
		int year, month, day, hours, minutes, seconds, miliseconds;
		double jd, delta_t, gmst, mjd, tjd;

		#region Properties
		public DateTime Date
		{
			get
			{
				return date;
			}
		}

		public int Year
		{
			get
			{
				return year;
			}
		}

		public int Month
		{
			get
			{
				return month;
			}
		}

		public int Day
		{
			get
			{
				return day;
			}
		}

		public int Hours
		{
			get
			{
				return hours;
			}
		}

		public int Minutes
		{
			get
			{
				return minutes;
			}
		}

		public int Seconds
		{
			get
			{
				return seconds;
			}
		}

		public int Miliseconds
		{
			get
			{
				return miliseconds;
			}
		}

		public TimeSpan Timezone
		{
			get
			{
				return timezone;
			}
		}

		public double JD
		{
			get
			{
				return GetJulianDay();
			}
		}

		public double MJD
		{
			get
			{
				return GetMJD();
			}
		}

		public double TJD
		{
			get
			{
				return GetTJD(); ;
			}
		}

		public double DeltaT
		{
			get
			{
				return GetDeltaT();
			}
		}

		public double Gmst
		{
			get
			{
				return GetGmst();
			}
		}

		public DateTime Et
		{
			get
			{
				return GetEt();
			}
		}


		#endregion

		#region Methods
		private double GetJulianDay()
		{
			var _month = date.Month;
			var _year = date.Year;
			var _day = date.Day;
			var calender = ""; // ???
			var transition_offset = 0;

			if (month <= 2)
			{
				_year = _year - 1;
				_month = _month + 12;
			}
			var julian_day = Math.Floor(365.25 * (_year + 4716)) + Math.Floor(30.6001 * (_month + 1)) + _day - 1524.5;

			if (calender == "julian")
			{
				// nop
			}
			else if (calender == "gregorian")
			{
				var tmp = Math.Floor(_year / 100.0);
				transition_offset = 2 - (int)tmp + (int)(Math.Floor(tmp / 4.0));
			}
			else if (julian_day < 2299160.5)
			{
				transition_offset = 0;
			}
			else
			{
				var tmp = Math.Floor(_year / 100.0);
				transition_offset = 2 - (int)tmp + (int)(Math.Floor(tmp / 4.0));
			}
			var jd = julian_day + transition_offset;
			return jd;
		}

		private double GetMJD()
		{
			var jd = GetJulianDay();
			return jd - 2400000.5;
		}

		private double GetTJD()
		{
			var jd = GetJulianDay();
			return jd - 2440000.5;
		}

		private double GetGmst()
		{
			var rad = Math.PI / 180;
			var time_in_sec = date.Hour * 3600 + date.Minute * 60 + date.Second;
			var jd = GetJulianDay();
			//gmst at 0:00
			var t = (jd - 2451545.0) / 36525.0;
			var gmst_at_zero = (24110.5484 + 8640184.812866 * t + 0.093104 * t * t + 0.0000062 * t * t * t) / 3600.0;
			if (gmst_at_zero > 24)
				gmst_at_zero = gmst_at_zero % 24;
			//gmst at target time
			var gmst = gmst_at_zero + (time_in_sec * 1.00273790925) / 3600.0;
			//mean obliquity of the ecliptic
			var e = 23 + 26.0 / 60 + 21.448 / 3600 - 46.8150 / 3600 * t - 0.00059 / 3600 * t * t + 0.001813 / 3600 * t * t * t;
			//nutation in longitude
			var omega = 125.04452 - 1934.136261 * t + 0.0020708 * t * t + t * t * t / 450000.0;
			var long1 = 280.4665 + 36000.7698 * t;
			var long2 = 218.3165 + 481267.8813 * t;
			var phai = -17.20 * Math.Sin(omega * rad) - (-1.32 * Math.Sin(2 * long1 * rad)) - 0.23 * Math.Sin(2 * long2 * rad) + 0.21 * Math.Sin(2 * omega * rad);
			gmst = gmst + ((phai / 15.0) * (Math.Cos(e * rad))) / 3600.0;
			if (gmst < 0) 
				gmst = gmst % 24 + 24;
			if (gmst > 24)
				gmst = gmst % 24;
			return gmst;
		}

		private double GetDeltaT()
		{
			var y = year + (month - 0.5) / 12;
			double dt = 0;

			if (year <= -500)
			{
				var u = (y - 1820) / 100;
				dt = -20 + 32 * u * u;
			}
			else if (year > -500 && year <= 500)
			{
				var u = y / 100;
				dt = 10583.6 - 1014.41 * u + 33.78311 * u * u - 5.952053 * u * u * u - 0.1798452 * u * u * u * u + 0.022174192 * u * u * u * u * u + 0.0090316521 * u * u * u * u * u;
			}
			else if (year > 500 && year <= 1600)
			{
				var u = (y - 1000) / 100;
				dt = 1574.2 - 556.01 * u + 71.23472 * u * u + 0.319781 * u * u * u - 0.8503463 * u * u * u * u - 0.005050998 * u * u * u * u * u + 0.0083572073 * u * u * u * u * u * u;
			}
			else if (year > 1600 && year <= 1700)
			{
				var t = y - 1600;
				dt = 120 - 0.9808 * t - 0.01532 * t * t + t * t * t / 7129;
			}
			else if (year > 1700 && year <= 1800)
			{
				var t = y - 1700;
				dt = 8.83 + 0.1603 * t - 0.0059285 * t * t + 0.00013336 * t * t * t - t * t * t * t / 1174000;
			}
			else if (year > 1800 && year <= 1860)
			{
				var t = y - 1800;
				dt = 13.72 - 0.332447 * t + 0.0068612 * t * t + 0.0041116 * t * t * t - 0.00037436 * t * t * t * t + 0.0000121272 * t * t * t * t * t - 0.0000001699 * t * t * t * t * t * t + 0.000000000875 * t * t * t * t * t * t * t;
			}
			else if (year > 1860 && year <= 1900)
			{
				var t = y - 1860;
				dt = 7.62 + 0.5737 * t - 0.251754 * t * t + 0.01680668 * t * t * t - 0.0004473624 * t * t * t * t + t * t * t * t * t / 233174;
			}
			else if (year > 1900 && year <= 1920)
			{
				var t = y - 1900;
				dt = -2.79 + 1.494119 * t - 0.0598939 * t * t + 0.0061966 * t * t * t - 0.000197 * t * t * t * t;
			}
			else if (year > 1920 && year <= 1941)
			{
				var t = y - 1920;
				dt = 21.20 + 0.84493 * t - 0.076100 * t * t + 0.0020936 * t * t * t;
			}
			else if (year > 1941 && year <= 1961)
			{
				var t = y - 1950;
				dt = 29.07 + 0.407 * t - t * t / 233 + t * t * t / 2547;
			}
			else if (year > 1961 && year <= 1986)
			{
				var t = y - 1975;
				dt = 45.45 + 1.067 * t - t * t / 260 - t * t * t / 718;
			}
			else if (year > 1986 && year <= 2005)
			{
				var t = y - 2000;
				dt = 63.86 + 0.3345 * t - 0.060374 * t * t + 0.0017275 * t * t * t + 0.000651814 * t * t * t * t + 0.00002373599 * t * t * t * t * t;
			}
			else if (year > 2005 && year <= 2050)
			{
				var t = y - 2000;
				dt = 62.92 + 0.32217 * t + 0.005589 * t * t;
			}
			else if (year > 2050 && year <= 2150)
			{
				/*
				This expression is derived from estimated values of ƒ¢T in the years 2010 and 2050. The value for 2010 (66.9 seconds) is based on a linearly extrapolation from 2005 using 0.39 seconds/year (average from 1995 to 2005). The value for 2050 (93 seconds) is linearly extrapolated from 2010 using 0.66 seconds/year (average rate from 1901 to 2000).
				*/
				dt = -20 + 32 * ((y - 1820) / 100) * ((y - 1820) / 100) - 0.5628 * (2150 - y);
				//The last term is introduced to eliminate the discontinuity at 2050.
			}
			else if (year > 2150)
			{
				var u = (y - 1820) / 100;
				dt = -20 + 32 * u * u;
			}
			return dt;
		}

		private DateTime GetEt()
		{
			var _et = new DateTime();
			_et.AddMilliseconds(GetDeltaT());
			return _et;
		}
		#endregion

		public Time(DateTime _date)
		{
			if (_date == null)
				_date = DateTime.Now;
			var tzi = TimeZoneInfo.Local;
			date = _date.ToUniversalTime();
			timezone = tzi.GetUtcOffset(_date);


			year = date.Year;
			month = date.Month;
			day = date.Day;
			hours = date.Hour;
			minutes = date.Minute;
			seconds = date.Second;
			miliseconds = date.Millisecond;
			/*
			year = date.Year;
			month = date.Month + 1;
			day = date.Day;
			hours = date.Hour;
			minutes = date.Minute;
			seconds = date.Second;
			miliseconds = date.Millisecond;
			*/
			/*
			jd = GetJulianDay();
			gmst = GetGmst();
			mjd = GetMJD();
			tjd = GetTJD();
			delta_t = GetDeltaT();
			et = GetEt();
			 */
		}
	}

	/*
	public class Position
	{
		public object EquatorialToRectangular
		{
			get
			{
				
			  if(obj.position.equatorial){
				var ra = obj.position.equatorial.ra;
				var dec = obj.position.equatorial.dec;
				var distance = obj.position.equatorial.distance;
			  }
			  var value = {};
			  var rad=Math.PI/180;
			  obj.position.rectangular = {};
			  obj.position.rectangular.x = distance*Math.cos(dec*rad)*Math.cos(ra*rad);
			  obj.position.rectangular.y = distance*Math.cos(dec*rad)*Math.sin(ra*rad);
			  obj.position.rectangular.z = distance*Math.sin(dec*rad);
			  return obj;
			}
		}
	}
	*/
}
