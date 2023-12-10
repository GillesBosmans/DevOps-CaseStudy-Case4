using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace models
{
	public abstract class BasisKlasse : IDataErrorInfo
	{
		public string Error
		{
			get
			{
				string foutmeldingen = "";

				foreach (var property in this.GetType().GetProperties()) //reflection
				{
					if (property.CanRead && property.CanWrite)
					{
						string fout = this[property.Name];  //Valideer(property.name)
						if (!string.IsNullOrWhiteSpace(fout))
						{
							foutmeldingen += fout + Environment.NewLine;
						}
					}
				}
				return foutmeldingen;
			}
		}

		public abstract string this[string columnName] { get; }

		public bool IsValid()
		{
			return string.IsNullOrWhiteSpace(Error);
		}
	}
}
