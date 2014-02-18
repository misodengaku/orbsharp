using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orb
{
	public class TLE
	{
		public string Name { get; set; }
		public string FirstLine { get; set; }
		public string SecondLine { get; set; }
	}

	/// <summary>
	/// 直交座標を表します。
	/// </summary>
	public class Rectangle
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public Rectangle();

		public Rectangle(double X, double Y, double Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}
	}

	public class SGP4ExecuteData
	{
		public OrbitalElements orbital_elements { get; set; }
		public double apoge { get; set; }
		public double perige { get; set; }
		public double orbital_period { get; set; }
		public int epoch_year { get; set; }
		public double epoch { get; set; }
		public DateTime epoch_in_date { get; set; }
		public double xmo { get; set; }
		public double xmdot { get; set; }
		public double omegao { get; set; }
		public double omgdot { get; set; }
		public double xnodeo { get; set; }
		public double xnodot { get; set; }
		public double xnodcf { get; set; }
		public double bstar { get; set; }
		public double t2cof { get; set; }
		public double omgcof { get; set; }
		public double isimp { get; set; }
		public double xmcof { get; set; }
		public double eta { get; set; }
		public double delmo { get; set; }
		public double c1 { get; set; }
		public double c4 { get; set; }
		public double c5 { get; set; }
		public double d2 { get; set; }
		public double d3 { get; set; }
		public double d4 { get; set; }
		public double sinmo { get; set; }
		public double t3cof { get; set; }
		public double t4cof { get; set; }
		public double t5cof { get; set; }
		public double aodp { get; set; }
		public double eo { get; set; }
		public double xnodp { get; set; }
		public double xke { get; set; }
		public double xlcof { get; set; }
		public double aycof { get; set; }
		public double x3thm1 { get; set; }
		public double x1mth2 { get; set; }
		public double xincl { get; set; }
		public double cosio { get; set; }
		public double sinio { get; set; }
		public double e6a { get; set; }
		public double ck2 { get; set; }
		public double x7thm1 { get; set; }
		public double xkmper { get; set; }
	}

	public class SGP4CalculatedData
	{
		public double x { get; set; }
		public double y { get; set; }
		public double z { get; set; }
		public double xdot { get; set; }
		public double ydot { get; set; }
		public double zdot { get; set; }
	}

	public class OrbitalElements
	{
		#region Properties
		public string Name
		{
			get
			{
				return name;
			}
		}
		public double BstarExponent
		{
			get
			{
				return bstar_exponent;
			}
		}
		public double Bstar
		{
			get
			{
				return bstar;
			}
		}
		public int EpochYear
		{
			get
			{
				return epoch_year;
			}
		}
		public double BstarMantissa
		{
			get
			{
				return bstar_mantissa;
			}
		}

		public int LineNumber_1
		{
			get
			{
				return line_number_1;
			}
		}
		public int CatalogNo_1
		{
			get
			{
				return catalog_no_1;
			}
		}
		public string SecurityClassification
		{
			get
			{
				return security_classification;
			}
		}
		public string InternationalIdentification
		{
			get
			{
				return international_identification;
			}
		}
		public double Epoch
		{
			get
			{
				return epoch;
			}
		}
		public double FirstDerivativeMeanMotion
		{
			get
			{
				return first_derivative_mean_motion;
			}
		}
		public double SecondDerivativeMeanMotion
		{
			get
			{
				return second_derivative_mean_motion;
			}
		}
		public int EphemerisType
		{
			get
			{
				return ephemeris_type;
			}
		}
		public int ElementNumber
		{
			get
			{
				return element_number;
			}
		}
		public int CheckSum_1
		{
			get
			{
				return check_sum_1;
			}
		}
		public int LineNumber_2
		{
			get
			{
				return line_number_2;
			}
		}
		public int CatalogNo_2
		{
			get
			{
				return catalog_no_2;
			}
		}
		public double Inclination
		{
			get
			{
				return inclination;
			}
		}
		public double RightAscension
		{
			get
			{
				return right_ascension;
			}
		}
		public int Eccentricity
		{
			get
			{
				return eccentricity;
			}
		}
		public double ArgumentOfPerigee
		{
			get
			{
				return argument_of_perigee;
			}
		}
		public double MeanAnomaly
		{
			get
			{
				return mean_anomaly;
			}
		}
		public double MeanMotion
		{
			get
			{
				return mean_motion;
			}
		}
		public int RevNumberAtEpoch
		{
			get
			{
				return rev_number_at_epoch;
			}
		}
		public int CheckSum_2
		{
			get
			{
				return check_sum_2;
			}
		}
		#endregion

		string name, security_classification, international_identification;
		double bstar_mantissa, bstar_exponent, bstar, epoch, right_ascension, inclination, argument_of_perigee, mean_anomaly, mean_motion;
		double first_derivative_mean_motion, second_derivative_mean_motion;
		int line_number_1, catalog_no_1;
		int ephemeris_type, element_number, check_sum_1, line_number_2, catalog_no_2, epoch_year;
		int eccentricity, rev_number_at_epoch, check_sum_2;
		
		public OrbitalElements(TLE tle)
		{
			name = tle.Name;
			var sepy = tle.FirstLine.Substring(18, 2);
			var epy = int.Parse(sepy);
			if (epy < 57)
			{
				epoch_year = epy + 2000;
			}
			else
			{
				epoch_year = epy + 1900;
			}
			var sman = tle.FirstLine.Substring(53, 6);
			bstar_mantissa = double.Parse(sman) * 1e-5;
			bstar_exponent = double.Parse("1e" + double.Parse(tle.FirstLine.Substring(59, 3)));
			bstar = bstar_mantissa * bstar_exponent;

			//slice
			line_number_1 = int.Parse(tle.FirstLine.Substring(0, 1));
			catalog_no_1 = int.Parse(tle.FirstLine.Substring(2, 4));
			line_number_2 = int.Parse(tle.SecondLine.Substring(0, 1));
			catalog_no_2 = int.Parse(tle.SecondLine.Substring(2, 5));
			security_classification = tle.FirstLine.Substring(7, 1);
			international_identification = tle.FirstLine.Substring(9, 8);

			//substring
			epoch = double.Parse(tle.FirstLine.Substring(20, 12));
			first_derivative_mean_motion = double.Parse(tle.FirstLine.Substring(33, 10));
			var s1 = tle.FirstLine.Substring(44, 6);
			var s2 = tle.FirstLine.Substring(50, 2);
			second_derivative_mean_motion = double.Parse(s1 + "e" + s2);
			ephemeris_type = int.Parse(tle.FirstLine.Substring(62, 1));
			element_number = int.Parse(tle.FirstLine.Substring(64, 4));
			check_sum_1 = int.Parse(tle.FirstLine.Substring(68, 1));
			inclination = double.Parse(tle.SecondLine.Substring(8, 8));
			right_ascension = double.Parse(tle.SecondLine.Substring(17, 8));
			eccentricity = int.Parse(tle.SecondLine.Substring(26, 7));
			argument_of_perigee = double.Parse(tle.SecondLine.Substring(34, 8));
			mean_anomaly = double.Parse(tle.SecondLine.Substring(43, 8));
			mean_motion = double.Parse(tle.SecondLine.Substring(52, 11));
			rev_number_at_epoch = int.Parse(tle.SecondLine.Substring(64, 4));
			check_sum_2 = int.Parse(tle.SecondLine.Substring(68, 1));	
		}
			
	}


	public class Geographic
	{
		public double longitude { get; set; }
		public double latitude { get; set; }
		public double altitude { get; set; }
		public double velocity { get; set; }
	}

	public class SatelliteData
	{
		public OrbitalElements elements { get; set; }
		public DateTime epoch_in_date { get; set; }
		public double orbital_period { get; set; }

		public double apogee { get; set; }
		public double perigee { get; set; }

		public Position position { get; set; }

		/*
		public SatelliteData(Time time, SGP4ExecuteData sgp4)
		{
			elements = sgp4.orbital_elements;
			epoch_in_date = sgp4.epoch_in_date;
			orbital_period = sgp4.orbital_period;
			apogee = sgp4.apoge;
			perigee = sgp4.perige;
			position = new Position();
			var execSGP4 = 
		}
		*/
	}

	public class Position
	{
		public SGP4CalculatedData rectangular { get; set; }
		public Geographic geographic { get; set; }


	}

	/// <summary>
	/// 衛星を観測する地点を表します。
	/// </summary>
	public class SatelliteObserver
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Altitude { get; set; }

		/// <summary>
		/// 設定された地点を直交座標に変換して返します。
		/// </summary>
		/// <returns></returns>
		public Rectangle ToRectangular()
		{
			var rad = Math.PI / 180;
			var a = 6377.39715500; // earth radius
			var e2 = 0.006674372230614;

			var n = a / (Math.Sqrt(1 - e2 * Math.Cos(Latitude * rad)));
			var x = (n + Altitude) * Math.Cos(Latitude * rad) * Math.Cos(Longitude * rad);
			var y = (n + Altitude) * Math.Cos(Latitude * rad) * Math.Sin(Longitude * rad);
			var z = (n * (1 - e2) + Altitude) * Math.Sin(Latitude * rad);
			var r = new Rectangle(x, y, z);
			return r;
		}
	}
}
