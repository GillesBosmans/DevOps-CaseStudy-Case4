using models;
using System;
using System.Collections.Generic;

namespace dal
{
	public interface ICategorieRepository
	{
		public IEnumerable<Categorie> GetCategories();
		bool InsertCategorie(Categorie categorie);
	}
}
