using System;
using System.Collections.Generic;
using System.Text;

namespace models
{
	public class Klant
	{
		// Properties
		public int Id { get; set; }
		public string Naam { get; set; }
		public string Voornaam { get; set; }
		public string Email { get; set; }
		public string Straat { get; set; }
		public string Nummer { get; set; }
		public string Postcode { get; set; }
		public string Gemeente { get; set; }
		public string Land { get; set; }
		public string Paswoord { get; set; }
		public bool Status { get; set; }

		// Navigation Properties
		public IEnumerable<Bestelling> Bestelling { get; set; }
	}
}
