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
		public string BstarExponent
		{
			get
			{
				return bstar_exponent;
			}
		}
		public string Bstar
		{
			get
			{
				return bstar;
			}
		}
		public string EpochYear
		{
			get { return epoch_year; }
		}
		public string BstarMantissa
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
		public int SecurityClassification
		{
			get
			{
				return security_classification;
			}
		}
		public int InternationalIdentification
		{
			get
			{
				return international_identification;
			}
		}
		public int Epoch
		{
			get
			{
				return epoch;
			}
		}
		public int FirstDerivativeMeanMotion
		{
			get
			{
				return first_derivative_mean_motion;
			}
		}
		public int SecondDerivativeMeanMotion
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
		public int Inclination
		{
			get
			{
				return inclination;
			}
		}
		public int RightAscension
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
		public int ArgumentOfPerigee
		{
			get
			{
				return argument_of_perigee;
			}
		}
		public int MeanAnomaly
		{
			get
			{
				return mean_anomaly;
			}
		}
		public int MeanMotion
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

		string name, epoch_year, bstar_mantissa, bstar_exponent, bstar;
		int line_number_1, catalog_no_1, security_classification, international_identification, epoch, first_derivative_mean_motion;
		int second_derivative_mean_motion, ephemeris_type, element_number, check_sum_1, line_number_2, catalog_no_2;
		int inclination, right_ascension, eccentricity, argument_of_perigee, mean_anomaly, mean_motion, rev_number_at_epoch, check_sum_2;

		public OrbitalElements(TLE tle)
		{
			name = tle.Name;

			//slice
			line_number_1 = int.Parse(tle.FirstLine.Substring(0, 0));
			catalog_no_1 = int.Parse(tle.FirstLine.Substring(2, 6));
			line_number_2 = int.Parse(tle.SecondLine.Substring(0, 0));
			catalog_no_2 = int.Parse(tle.SecondLine.Substring(2, 7));
			security_classification = int.Parse(tle.FirstLine.Substring(7, 7));
			international_identification = int.Parse(tle.FirstLine.Substring(9, 17));

			//substring
			epoch = int.Parse(tle.FirstLine.Substring(20, 32));
			first_derivative_mean_motion = int.Parse(tle.FirstLine.Substring(33, 43));
			second_derivative_mean_motion = int.Parse(tle.FirstLine.Substring(44, 52));
			ephemeris_type = int.Parse(tle.FirstLine.Substring(62, 63));
			element_number = int.Parse(tle.FirstLine.Substring(64, 68));
			check_sum_1 = int.Parse(tle.FirstLine.Substring(69, 69));
			inclination = int.Parse(tle.SecondLine.Substring(8, 16));
			right_ascension = int.Parse(tle.SecondLine.Substring(17, 25));
			eccentricity = int.Parse(tle.SecondLine.Substring(26, 33));
			argument_of_perigee = int.Parse(tle.SecondLine.Substring(34, 42));
			mean_anomaly = int.Parse(tle.SecondLine.Substring(43, 51));
			mean_motion = int.Parse(tle.SecondLine.Substring(52, 63));
			rev_number_at_epoch = int.Parse(tle.SecondLine.Substring(64, 68));
			check_sum_2 = int.Parse(tle.SecondLine.Substring(68, 69));	
		}
			
	}
}
