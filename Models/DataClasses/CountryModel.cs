using System.Data;
using System;

namespace Projekt3.Models
{
	public class CountryModel
	{
		public int ID { get; }
		public string Name { get; set; }
		public string Emoji { get; set; }

		public CountryModel(DataRow values)
		{
			System.Diagnostics.Debug.WriteLine("Det här är emoji: "+values[2].ToString());
			ID = int.Parse(values[0].ToString());
			Name = values[1].ToString();
			Emoji = values[2].ToString();
		}
	}
}
