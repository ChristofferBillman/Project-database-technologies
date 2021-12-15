using System.Data;

namespace Projekt3.Models
{
	public class HobbyModel
	{
		public int ID { get; }
		public string Name { get; set; }
		public char Emoji { get; set; }

		public HobbyModel(DataRow values)
		{
			ID = int.Parse(values[0].ToString());
			Name = values[1].ToString();
			Emoji = char.Parse(values[2].ToString());
		}
	}
}
