using models;
using System;
using System.Collections.Generic;

namespace dal
{
	public interface IBestellingOnderdeelRepository
	{
		IEnumerable<BestellingOnderdeel> GetPartsInActiveShoppingCart(int BestellingId);
		IEnumerable<BestellingOnderdeel> GetPartsInActiveShoppingCartBySearch(int BestellingId, string Search);
		bool InsertPartInOrder(BestellingOnderdeel bestellingOnderdeel);
		bool UpdatePartInOrder(BestellingOnderdeel bestellingOnderdeel);
		bool DeletePartInOrder(int bestellingOnderdeelID);
	}
}
