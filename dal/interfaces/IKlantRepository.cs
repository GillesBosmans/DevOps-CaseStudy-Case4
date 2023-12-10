using models;
using System;
using System.Collections.Generic;

namespace dal
{
	public interface IKlantRepository
	{
		Klant GetKlantByEmail(string email);
	}
}
