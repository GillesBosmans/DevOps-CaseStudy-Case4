using Dapper;
using models;

namespace dal
{
	public class BestellingRepository : SqliteBaseRepository, IBestellingRepository
	{
		public BestellingRepository()
		{
			if (!DatabaseExists())
			{
				CreateDatabase();
			}
		}

		// bestelling ophalen door middel van klantID
		public Bestelling GetActiveOrderByKlantId(int klantID)
		{
			string sql = @"SELECT *
						   FROM Bestelling
						   WHERE klantId = @klantid
								 AND bestellingCheck = 1";

			var parameters = new
			{
				@klantid = klantID,
			};

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.QuerySingleOrDefault<Bestelling>(sql, parameters);
			}
		}

		// bestellingen aanmaken 
		public bool CreateOrder(Bestelling bestelling)
		{
			string sql = @"INSERT INTO Bestelling (datum, klantId, bestellingCheck)
                           VALUES (@Datum, @KlantID, @BestellingCheck)";

			var parameters = new
			{
				@Datum = bestelling.Datum,
				@KlantID = bestelling.KlantId,
				@BestellingCheck = bestelling.BestellingCheck
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

		// bestellingen update
		public bool UpdatePart(Bestelling bestelling)
		{
			string sql = @"UPDATE Bestelling SET 
											klantId = @klantId,
                                            bestellingCheck = @bestellingCheck
                          WHERE id = @BestellingID";

			var parameters = new
			{
				@BestellingID = bestelling.Id,
				@klantId = bestelling.KlantId,
				@bestellingCheck = bestelling.BestellingCheck
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
	}
}
