using Dapper;
using models;
using System.Collections.Generic;

namespace dal
{
	public class CategorieRepository : SqliteBaseRepository, ICategorieRepository
	{
		public CategorieRepository()
		{
			if (!DatabaseExists())
			{
				CreateDatabase();
			}
		}

		// categorieën ophalen
		public IEnumerable<Categorie> GetCategories()
		{
			string sql = "SELECT * FROM Categorie";
			using (var connection = DBConnectionGBAutoParts())
			{
				return connection.Query<Categorie>(sql);
			}
		}

		//categorie toevoegen
		public bool InsertCategorie(Categorie categorie)
		{
			string sql = @"INSERT INTO Categorie (naam)
                           VALUES (@Naam)";

			var parameters = new
			{
				@Naam = categorie.Naam
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
