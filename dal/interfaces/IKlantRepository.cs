using models;

namespace dal
{
	public interface IKlantRepository
	{
		Klant GetKlantByEmail(string email);
	}
}
