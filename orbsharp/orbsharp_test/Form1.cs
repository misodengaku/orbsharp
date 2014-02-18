using Orb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace orbsharp_test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var tle = textBox1.Text.Split(new char[] { '\n' });
			var sat = new Satellite(tle, new DateTime(2013, 12, 31, 0, 0, 0, DateTimeKind.Utc));
			var lon = sat.CalculatedData.position.geographic.longitude;
			var lat = sat.CalculatedData.position.geographic.latitude;
			var alt = sat.CalculatedData.position.geographic.altitude;
			var x = sat.CalculatedData.position.rectangular.x;
			var y = sat.CalculatedData.position.rectangular.y;
			var z = sat.CalculatedData.position.rectangular.z;
			textBox2.Text = "Satellite\r\n";
			textBox2.Text += "Longitude:\t" + lon + "\r\n";
			textBox2.Text += "Latitude:\t" + lat + "\r\n";
			textBox2.Text += "Altitude:\t" + alt + "\r\n";
			textBox2.Text += "rect x:\t" + x + "\r\n";
			textBox2.Text += "rect y:\t" + y + "\r\n";
			textBox2.Text += "rect z:\t" + z + "\r\n";

		}
	}
}
