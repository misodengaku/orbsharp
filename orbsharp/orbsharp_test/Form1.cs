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
			var sat = new Satellite(tle, new DateTime(2014, 2, 17));
			var lat = sat.CalculatedData.position.geographic.latitude;
			var lon = sat.CalculatedData.position.geographic.longitude;
			var alt = sat.CalculatedData.position.geographic.altitude;
			textBox2.Text = "Satellite\r\n";
			textBox2.Text += "Longitude: " + lon + "\r\n";
			textBox2.Text += "Latitude: " + lat + "\r\n";
			textBox2.Text += "Altitude: " + alt + "\r\n";

		}
	}
}
