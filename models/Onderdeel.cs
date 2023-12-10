using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace models
{
	public class Onderdeel : BasisKlasse
	{
		// Properties
		public int Id { get; set; }
		public string Naam { get; set; }
		public string Type { get; set; }
		public string Foto { get; set; }
		public string Omschrijving { get; set; }
		public decimal Prijs { get; set; }
		public int Aantal { get; set; }
		public int AutoID { get; set; }
		public int CategorieID { get; set; }

		// Navigation Properties
		public Auto Auto { get; set; }
		public Categorie Categorie { get; set; }
		public IEnumerable<BestellingOnderdeel> BestellingOnderdelen { get; set; }

		public override string this[string columnName] //valideer(string columName)
		{
			get
			{
				if (columnName == nameof(Naam) && string.IsNullOrWhiteSpace(Naam))
				{
					return "Naam is verplicht.";
				}
				else if (columnName == nameof(Type) && string.IsNullOrWhiteSpace(Type))
				{
					return "Type is verplicht.";
				}
				else if (columnName == nameof(Foto) && string.IsNullOrWhiteSpace(Foto))
				{
					return "Foto url is verplicht.";
				}
				else if (columnName == nameof(Omschrijving) && string.IsNullOrWhiteSpace(Omschrijving))
				{
					return "Omschrijving is verplicht.";
				}
				else if (columnName == nameof(Prijs) && Prijs < 0)
				{
					return "Prijs is verplicht en moet een numerieke waarde zijn.";
				}
				else if (columnName == nameof(Aantal) && Aantal < 0)
				{
					return "Aantal is verplicht en moet een numerieke waarde zijn.";
				}
				return "";
			}
		}
	}
}
