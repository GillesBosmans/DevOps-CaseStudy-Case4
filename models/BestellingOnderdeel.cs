using System;
using System.Collections.Generic;
using System.Text;

namespace models
{
	public class BestellingOnderdeel
	{
		// Properties
		public int Id { get; set; }
		public int Aantal { get; set; }
		public int OnderdeelId { get; set; }
		public int BestellingId { get; set; }

		//navigation properties
		public Onderdeel Onderdeel { get; set; }
		public Bestelling Bestelling { get; set; }
	}
}
