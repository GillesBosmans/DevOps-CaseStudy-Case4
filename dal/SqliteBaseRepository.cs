using Dapper;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Data.Common;
using System.IO;

namespace dal
{
	public class SqliteBaseRepository
	{
		public SqliteBaseRepository()
		{
		}

		public static SqliteConnection DBConnectionGBAutoParts()
		{
			return new SqliteConnection(@"DataSource=GBAutoPartsDB.sqlite");
		}

		protected static bool DatabaseExists()
		{
			return File.Exists(@"GBAutoPartsDB.sqlite");
		}

		protected static void CreateDatabase()
		{
			using (var dbConnection = DBConnectionGBAutoParts())
			{
				dbConnection.Open();

				// Create Auto table
				dbConnection.Execute(@"
                    CREATE TABLE Auto (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        productiejaar INTEGER NOT NULL,
                        kleur VARCHAR(50) NOT NULL,
                        prijs REAL NOT NULL,
                        model VARCHAR(50) NOT NULL,
                        merk VARCHAR(50) NOT NULL,
                        chassisnummer VARCHAR(50) NOT NULL
                    );
                ");

				// Create Bestelling table
				dbConnection.Execute(@"
                    CREATE TABLE Bestelling (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        datum DATETIME NOT NULL,
                        klantId INTEGER NOT NULL,
                        bestellingCheck INTEGER NOT NULL
                    );
                ");

				// Create BestellingOnderdeel table
				dbConnection.Execute(@"
                    CREATE TABLE BestellingOnderdeel (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        aantal INTEGER NOT NULL,
                        onderdeelId INTEGER,
                        bestellingId INTEGER
                    );
                ");

				// Create Categorie table
				dbConnection.Execute(@"
                    CREATE TABLE Categorie (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        naam VARCHAR(50) NOT NULL
                    );
                ");

				// Create Klant table
				dbConnection.Execute(@"
                    CREATE TABLE Klant (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        naam VARCHAR(50) NOT NULL,
                        voornaam VARCHAR(50) NOT NULL,
                        email VARCHAR(50) NOT NULL,
                        straat VARCHAR(50) NOT NULL,
                        nummer VARCHAR(50) NOT NULL,
                        postcode VARCHAR(50) NOT NULL,
                        gemeente VARCHAR(50) NOT NULL,
                        land VARCHAR(50) NOT NULL,
                        paswoord NCHAR(50) NOT NULL,
                        status INTEGER NOT NULL
                    );
                ");

				// Create Onderdeel table
				dbConnection.Execute(@"
                    CREATE TABLE Onderdeel (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        naam VARCHAR(50) NOT NULL,
                        type VARCHAR(50) NOT NULL,
                        foto VARCHAR(50) NOT NULL,
                        omschrijving VARCHAR(255) NOT NULL,
                        prijs REAL NOT NULL,
                        aantal INTEGER NOT NULL,
                        autoId INTEGER NOT NULL,
                        categorieId INTEGER NOT NULL
                    );
                "
                );

				dbConnection.Execute(@"
                    INSERT INTO Klant (naam, voornaam, email, straat, nummer, postcode, gemeente, land, paswoord, status)
                    VALUES (
                        'Admin', 
                        'Admin', 
                        'admin@gbautoparts.com', 
                        'Langstraat', 
                        '13', 
                        '3910', 
                        'Neerpelt', 
                        'BE', 
                        'Admin', 
                        1
                    );
                ");

				dbConnection.Execute(@"
                    INSERT INTO Klant (naam, voornaam, email, straat, nummer, postcode, gemeente, land, paswoord, status)
                    VALUES (
                        '1', 
                        'kassa', 
                        'kassa1@gbautoparts.com', 
                        'Langstraat', 
                        '13', 
                        '3910', 
                        'Neerpelt', 
                        'BE', 
                        'Kassa1', 
                        0
                    );
                ");

				dbConnection.Execute(@"
                    INSERT INTO Klant (naam, voornaam, email, straat, nummer, postcode, gemeente, land, paswoord, status)
                    VALUES (
                        '2', 
                        'kassa', 
                        'kassa2@gbautoparts.com', 
                        'Langstraat', 
                        '13', 
                        '3910', 
                        'Neerpelt', 
                        'BE', 
                        'Kassa2', 
                        0
                    );
                ");

				dbConnection.Close();
			}
		}
	
	}
}