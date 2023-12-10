using models;
using System.Collections.Generic;

namespace dal
{
	public interface IOnderdeelRepository
	{
		Onderdeel GetPartById(int partID);
		IEnumerable<Onderdeel> GetPartsAndCars();
		IEnumerable<Onderdeel> GetPartsAndCarsWhereAmountNotZero();
		IEnumerable<Onderdeel> GetPartsAndCarsByCategorieId(int categorieId);
		IEnumerable<Onderdeel> GetPartsBySearch(string search);
		IEnumerable<Onderdeel> GetPartsByCategorieAndSearch(int categorieID, string search);
		bool InsertPart(Onderdeel onderdeel);
		bool UpdatePart(Onderdeel onderdeel);
		bool DeletePart(int partID);
	}
}
