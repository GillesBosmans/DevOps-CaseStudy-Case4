using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Data;
using System.Text;
using models;
using dal;
using System.Security.Cryptography;

namespace dal
{
	public class OnderdeelRepository : SqliteBaseRepository, IOnderdeelRepository
	{
		public OnderdeelRepository()
		{
			if (!DatabaseExists())
			{
				CreateDatabase();
			}
		}

		// onderdeel ophalen dmv PartID
		public Onderdeel GetPartById(int partID)
		{
			string sql = @"SELECT * 
						   FROM Onderdeel
						   WHERE id = @partID";

			var parameters = new { @partID = partID };

			using (var connection = DBConnectionGBAutoParts())
			{
				var restult = connection.QuerySingleOrDefault<Onderdeel>(sql, parameters);
				return restult;
				//return connection.QuerySingleOrDefault<Onderdeel>(sql, parameters);
			}
		}
		// onderdelen + auto's ophalen
		public IEnumerable<Onderdeel> GetPartsAndCars()
		{
			string sql = @"SELECT O.*, '' AS SplitCol, A.*
						   FROM Onderdeel O 
						   JOIN Auto A ON O.autoId = A.id";

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.Query<Onderdeel, Auto, Onderdeel>(
					sql,
					(onderdeel, auto) =>
					{
						onderdeel.Auto = auto;
						return onderdeel;
					},
					splitOn: "SplitCol"
				);
			}
		}

		// onderdelen + auto's ophalen
		public IEnumerable<Onderdeel> GetPartsAndCarsWhereAmountNotZero()
		{
			string sql = @"SELECT O.*, '' AS SplitCol, A.*
						   FROM Onderdeel O 
						   JOIN Auto A ON O.autoId = A.id
						   WHERE O.aantal > 0";

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.Query<Onderdeel, Auto, Onderdeel>(
					sql,
					(onderdeel, auto) =>
					{
						onderdeel.Auto = auto;
						return onderdeel;
					},
					splitOn: "SplitCol"
				);
			}
		}

		// onderdelen + auto's ophalen per categorie
		public IEnumerable<Onderdeel> GetPartsAndCarsByCategorieId(int categorieID)
		{
			string sql = @"SELECT O.*, '' AS SplitCol, A.*
						   FROM Onderdeel O 
						   JOIN Auto A ON O.autoId = A.id
						   WHERE categorieId = @categorieID";

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.Query<Onderdeel, Auto, Onderdeel>(
					sql,
					(onderdeel, auto) =>
					{
						onderdeel.Auto = auto;
						return onderdeel;
					},
					new { @categorieID = categorieID },
					splitOn: "SplitCol"
				);
			}
		}

		// onderdelen ophalen per zoekopdracht
		public IEnumerable<Onderdeel> GetPartsBySearch(string search)
		{
			string sql = @"SELECT O.*, '' AS SplitCol, A.*
						   FROM Onderdeel O 
						   JOIN Auto A ON O.autoId = A.id
						   WHERE O.omschrijving  LIKE '%'+@search+'%'";

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.Query<Onderdeel, Auto, Onderdeel>(
					sql,
					(onderdeel, auto) =>
					{
						onderdeel.Auto = auto;
						return onderdeel;
					},
					new { @search = search },
					splitOn: "SplitCol"
				);
			}
		}

		// onderdelen ophalen per categorie en zoekopdracht
		public IEnumerable<Onderdeel> GetPartsByCategorieAndSearch(int categorieID, string search)
		{
			string sql = @"SELECT O.*, '' AS SplitCol, A.*
						   FROM Onderdeel O 
						   JOIN Auto A ON O.autoId = A.id
						   WHERE categorieId = @categorieID
								 AND O.omschrijving LIKE '%'+@search+'%'";

			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.Query<Onderdeel, Auto, Onderdeel>(
					sql,
					(onderdeel, auto) =>
					{
						onderdeel.Auto = auto;
						return onderdeel;
					},
					new { @categorieID = categorieID, @search = search },
					splitOn: "SplitCol"
				);
			}
		}

		// onderdeel toevoegen
		public bool InsertPart(Onderdeel onderdeel)
		{
			string sql = @"INSERT INTO Onderdeel (naam, type, foto, omschrijving, prijs, aantal, autoId, categorieId)
                           VALUES (@Naam, @Type, @Foto, @Omschrijving, @Prijs, @Aantal, @AutoID, @CategorieID)";

			var parameters = new
			{
				@Naam = onderdeel.Naam,
				@Type = onderdeel.Type,
				@Foto = onderdeel.Foto,
				@Omschrijving = onderdeel.Omschrijving,
				@Prijs = onderdeel.Prijs,
				@Aantal = onderdeel.Aantal,
				@AutoID = onderdeel.AutoID,
				@CategorieID = onderdeel.CategorieID
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

		// onderdeel updaten
		public bool UpdatePart(Onderdeel onderdeel)
		{
			string sql = @"UPDATE Onderdeel SET 
											naam = @Naam,
                                            type = @Type,
                                            foto = @Foto,
                                            omschrijving = @Omschrijving,
                                            prijs = @Prijs,
                                            aantal = @Aantal,
                                            autoId = @AutoID,
                                            categorieId = @CategorieID
                          WHERE id = @onderdeelID";

			var parameters = new
			{
				@onderdeelID = onderdeel.Id,
				@Naam = onderdeel.Naam,
				@Type = onderdeel.Type,
				@Foto = onderdeel.Foto,
				@Omschrijving = onderdeel.Omschrijving,
				@Prijs = onderdeel.Prijs,
				@Aantal = onderdeel.Aantal,
				@AutoID = onderdeel.AutoID,
				@CategorieID = onderdeel.CategorieID
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
		public bool DeletePart(int partID)
		{
			string sql = @"DELETE FROM Onderdeel 
                           WHERE id = @partID";

			var parameters = new
			{
				@partID = partID
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
