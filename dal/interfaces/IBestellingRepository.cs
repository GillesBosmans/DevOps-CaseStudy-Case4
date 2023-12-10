using models;
using System;
using System.Collections.Generic;

namespace dal
{
	public interface IBestellingRepository
	{
		Bestelling GetActiveOrderByKlantId(int klantID);
		public bool CreateOrder(Bestelling bestelling);
		public bool UpdatePart(Bestelling bestelling);
	}
}
