using Dapper;
using models;
using System.Collections.Generic;

namespace dal
{
	public class BestellingOnderdeelRepository : SqliteBaseRepository, IBestellingOnderdeelRepository
	{
		public BestellingOnderdeelRepository()
		{
			if (!DatabaseExists())
			{
				CreateDatabase();
			}
		}

		// onderdelen ophalen uit actieve bestelling
		public IEnumerable<BestellingOnderdeel> GetPartsInActiveShoppingCart(int BestellingId)
		{
			string sql = @"SELECT BO.*, '' AS SplitCol, O.*
						   FROM BestellingOnderdeel AS BO
						   INNER JOIN Onderdeel AS O ON BO.onderdeelId = O.id
						   WHERE BO.bestellingId = @bestellingid";

			using (var connection = DBConnectionGBAutoParts())
			{
				var bestelling = connection.Query<BestellingOnderdeel, Onderdeel, BestellingOnderdeel>(
					sql,
					(bestellingOnderdeel, onderdeel) =>
					{
						bestellingOnderdeel.Onderdeel = onderdeel;
						return bestellingOnderdeel;
					},
					new { @bestellingid = BestellingId },
					splitOn: "SplitCol"
				);
				return bestelling;
			}
		}

		// onderdelen ophalen uit actieve bestelling met zoekfunctie
		public IEnumerable<BestellingOnderdeel> GetPartsInActiveShoppingCartBySearch(int BestellingId, string Search)
		{
			string sql = @"SELECT BO.*, '' AS SplitCol, O.*
						   FROM BestellingOnderdeel AS BO
						   INNER JOIN Onderdeel AS O ON BO.onderdeelId = O.id
						   WHERE BO.bestellingId = @bestellingid 
						   AND O.omschrijving  LIKE '%' || @search || '%'";

			using (var connection = DBConnectionGBAutoParts())
			{
				var bestelling = connection.Query<BestellingOnderdeel, Onderdeel, BestellingOnderdeel>(
					sql,
					(bestellingOnderdeel, onderdeel) =>
					{
						bestellingOnderdeel.Onderdeel = onderdeel;
						return bestellingOnderdeel;
					},
					new { @bestellingid = BestellingId, @search = Search },
					splitOn: "SplitCol"
				);
				return bestelling;
			}
		}

		// onderdelen toevoegen uit actieve bestelling
		public bool InsertPartInOrder(BestellingOnderdeel bestellingOnderdeel)
		{
			string sql = @"INSERT INTO BestellingOnderdeel (aantal, onderdeelId, bestellingId)
                           VALUES (@Aantal, @OnderdeelId, @BestellingId)";

			var parameters = new
			{
				@Aantal = bestellingOnderdeel.Aantal,
				@OnderdeelId = bestellingOnderdeel.OnderdeelId,
				@BestellingId = bestellingOnderdeel.BestellingId
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

		// onderdelen updaten van actieve bestelling
		public bool UpdatePartInOrder(BestellingOnderdeel bestellingOnderdeel)
		{
			string sql = @"UPDATE BestellingOnderdeel SET 
											aantal = @aantal,
                                            onderdeelId = @onderdeelId,
                                            bestellingId = @bestellingId
                          WHERE id = @id";

			var parameters = new
			{
				@id = bestellingOnderdeel.Id,
				@aantal = bestellingOnderdeel.Aantal,
				@onderdeelId = bestellingOnderdeel.OnderdeelId,
				@bestellingId = bestellingOnderdeel.BestellingId,
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

		// onderdelen verwijderen uit actieve bestelling
		public bool DeletePartInOrder(int bestellingOnderdeelID)
		{
			string sql = @"DELETE FROM BestellingOnderdeel 
                           WHERE Id = @bestellingOnderdeelid";

			var parameters = new
			{
				@bestellingOnderdeelid = bestellingOnderdeelID
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
