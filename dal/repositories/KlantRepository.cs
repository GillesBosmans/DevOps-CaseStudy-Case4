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
	}
}
