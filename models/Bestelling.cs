using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace models
{
	public class Bestelling
	{
		// Properties
		public int Id { get; set; }
		public DateTime Datum { get; set; }
		public bool BestellingCheck { get; set; }
		public int KlantId { get; set; }

		//navigation properties
		public IEnumerable<BestellingOnderdeel> BestellingOnderdelen { get; set; }
		public Klant Klant { get; set; }
	}
}
