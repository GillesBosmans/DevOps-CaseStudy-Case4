using models;
using System.Collections.Generic;

namespace dal
{
	public interface IAutoRepository
	{
		public IEnumerable<Auto> GetCars();
		bool InsertCar(Auto auto);
		bool DeleteCar(int autoId);
	}
}
