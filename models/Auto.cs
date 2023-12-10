using System;
using System.Collections.Generic;
using System.Text;

namespace models
{
	public class Auto : BasisKlasse
	{
		// Properties
		private string displayText;
		public int Id { get; set; }
		public int Productiejaar { get; set; }
		public string Kleur { get; set; }
		public decimal Prijs { get; set; }
		public string Model { get; set; }
		public string Merk { get; set; }
		public string Chassisnummer { get; set; }
		public string DisplayText
		{
			get { return $"{Merk} - {Model} - VIN: {Chassisnummer}"; }
			set { displayText = value; }
		}

		// Navigation Properties
		public IEnumerable<Onderdeel> Onderdelen { get; set; }

		public override string this[string columnName] //valideer(string columName)
		{
			get
			{
				if (columnName == nameof(Productiejaar) && Productiejaar < 0)
				{
					return "Productiejaar is verplicht en moet een geldige waarde zijn.";
				}
				else if (columnName == nameof(Kleur) && string.IsNullOrWhiteSpace(Kleur))
				{
					return "Kleur is verplicht.";
				}
				else if (columnName == nameof(Prijs) && Prijs < 0)
				{
					return "Prijs is verplicht en moet een numerieke waarde zijn.";
				}
				else if (columnName == nameof(Merk) && string.IsNullOrWhiteSpace(Merk))
				{
					return "Merk is verplicht.";
				}
				else if (columnName == nameof(Model) && string.IsNullOrWhiteSpace(Model))
				{
					return "Model is verplicht.";
				}
				else if (columnName == nameof(Chassisnummer) && string.IsNullOrWhiteSpace(Chassisnummer))
				{
					return "Chassisnummer is verplicht.";
				}
				return "";
			}
		}
	}
}
