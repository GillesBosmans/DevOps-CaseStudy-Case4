using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Data;
using System.Text;
using models;
using dal;

namespace dal
{
	public class KlantRepository : SqliteBaseRepository, IKlantRepository
	{
		public KlantRepository()
		{
			if (!DatabaseExists())
			{
				CreateDatabase();
			}
		}

		public Klant GetKlantByEmail(string email)
		{
			string sql = @"SELECT * 
						   FROM Klant
						   WHERE @email LIKE email";

			var parameters = new { @email = email };

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.QuerySingleOrDefault<Klant>(sql, parameters);
			}
		}

		//public bool InsertKlant(Klant klant)
		//{
		//	string sql = @"INSERT INTO Klant (naam, type, foto, omschrijving, prijs, aantal, autoId, categorieId)
  //                         VALUES (@Naam, @Type, @Foto, @Omschrijving, @Prijs, @Aantal, @AutoID, @CategorieID)";

		//	var parameters = new
		//	{
		//		@Naam = Klant.Naam,
		//		@Type = onderdeel.Type,
		//		@Foto = onderdeel.Foto,
		//		@Omschrijving = onderdeel.Omschrijving,
		//		@Prijs = onderdeel.Prijs,
		//		@Aantal = onderdeel.Aantal,
		//		@AutoID = onderdeel.AutoID,
		//		@CategorieID = onderdeel.CategorieID
		//	};

		//	using (var connection = DBConnectionGBAutoParts())
		//	{
		//		var affectedRows = connection.Execute(sql, parameters);
		//		if (affectedRows == 1)
		//		{
		//			return true;
		//		}
		//	}
		//	return false;
		//}
	}
}
