using Dapper;
using models;
using System.Collections.Generic;

namespace dal
{
	public class AutoRepository : SqliteBaseRepository, IAutoRepository
	{
		public AutoRepository() 
		{ 
			if(!DatabaseExists())
			{
				CreateDatabase();
			}
		}

		// auto's ophalen
		public IEnumerable<Auto> GetCars()
		{
			string sql = "SELECT * FROM Auto";
			using (var connection = DBConnectionGBAutoParts())
			{
				connection.Open();
				return connection.Query<Auto>(sql);
			}
		}

		//auto toevoegen
		public bool InsertCar(Auto auto)
		{
			string sql = @"INSERT INTO Auto (Productiejaar, Kleur, Prijs, Model, Merk, Chassisnummer)
                           VALUES (@Productiejaar, @Kleur, @Prijs, @Model, @Merk, @Chassisnummer)";

			var parameters = new
			{
				@Productiejaar = auto.Productiejaar,
				@Kleur = auto.Kleur,
				@Prijs = auto.Prijs,
				@Model = auto.Model,
				@Merk = auto.Merk,
				@Chassisnummer = auto.Chassisnummer
			};

			using (var connection = DBConnectionGBAutoParts())
			{
				var affectedRows = connection.Execute(sql, parameters);
				if (affectedRows == 1)
				{
					return true;
				}
			}
			return false;
		}

		//auto verwijderen
		public bool DeleteCar(int autoID)
		{
			string sql = @"DELETE FROM Auto 
                           WHERE id = @autoID";

			var parameters = new
			{
				@autoID = autoID
			};

			using (var connection = DBConnectionGBAutoParts())
			{
				var affectedRows = connection.Execute(sql, parameters);
				if (affectedRows >= 1)
				{
					return true;
				}
			}

			return false;
		}
	}
}
