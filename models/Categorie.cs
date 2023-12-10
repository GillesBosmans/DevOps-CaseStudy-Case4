using System;
using System.Collections.Generic;
using System.Text;

namespace models
{
	public class Categorie : BasisKlasse
	{
		// Properties
		public int Id { get; set; }
		public string Naam { get; set; }

		// Navigation Properties
		public IEnumerable<Onderdeel> Onderdelen { get; set; }

		// Methods
		public override string ToString()
		{
			return Naam;
		}

		public override string this[string columnName] //valideer(string columName)
		{
			get
			{
				if (columnName == nameof(Naam) && string.IsNullOrWhiteSpace(Naam))
				{
					return "Naam categorie is verplicht.";
				}
				return "";
			}
		}
	}
}
