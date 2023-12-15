using dal;
using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf
{
	/// <summary>
	/// Interaction logic for OrderView.xaml
	/// </summary>
	public partial class OrderView : UserControl
	{
		private IBestellingRepository _bestellingRepository = new BestellingRepository();
		private IBestellingOnderdeelRepository _bestellingOnderdeelRepository = new BestellingOnderdeelRepository();

		private int _klantID;
		private MainWindow _window;
		private Bestelling bestelling;
		private int _currentBestellingID;
		private decimal _totalPrice;

		private IEnumerable<BestellingOnderdeel> bestellingOnderdelen = new List<BestellingOnderdeel>();
		private List<BestellingOnderdeel> updatedBestellingOnderdelen = new List<BestellingOnderdeel>();
		private IOnderdeelRepository _onderdeelRepository = new OnderdeelRepository();

		public OrderView(int klantID, MainWindow window)
		{
			InitializeComponent();
			_window = window;
			this._klantID = klantID;
		}

		private void LoadData()
		{
			_totalPrice = 0;
			bestelling = _bestellingRepository.GetActiveOrderByKlantId(_klantID);

			if (bestelling == null)
			{
				MessageBox.Show("Fout bij het ophalen van u bestelling.\nMogelijk heeft U nog geen artikelen toegevoegd aan uw bestelling.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				_window.SelectIndex(0);
			}
			else
			{
				_currentBestellingID = bestelling.Id;
				bestellingOnderdelen = _bestellingOnderdeelRepository.GetPartsInActiveShoppingCart(_currentBestellingID);
				if (bestellingOnderdelen != null)
				{
					datagridOrders.ItemsSource = bestellingOnderdelen;
					foreach (var item in bestellingOnderdelen)
					{
						_totalPrice += item.Onderdeel.Prijs * item.Aantal;
					}
					lblTotal.Content = "€" + _totalPrice;
				}
				else
				{
					MessageBox.Show("U heeft nog geen artikelen toegevoegd aan uw bestelling.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (datagridOrders.SelectedItem is BestellingOnderdeel bestellingOnderdeel)
			{
				if (MessageBox.Show($"Weet je zeker dat je dit \'{bestellingOnderdeel.Onderdeel.Naam} - type:{bestellingOnderdeel.Onderdeel.Type}\' wilt verwijderen uit je bestelling?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
				{
					_bestellingOnderdeelRepository.DeletePartInOrder(bestellingOnderdeel.Id);
					LoadData();
				}
				else
				{
					MessageBox.Show($"\'{bestellingOnderdeel.Onderdeel.Naam} - type:{bestellingOnderdeel.Onderdeel.Type}\' niet verwijderen.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			else
			{
				MessageBox.Show("Geen onderdeel geselecteerd.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			updatedBestellingOnderdelen.Clear();
			foreach (var item in datagridOrders.Items)
			{
				if (item is BestellingOnderdeel bo)
				{
					updatedBestellingOnderdelen.Add(bo);
				}
			}

			foreach (var lijnOnderdeel in updatedBestellingOnderdelen)
			{
				if (!_bestellingOnderdeelRepository.UpdatePartInOrder(lijnOnderdeel))
				{
					MessageBox.Show("Er is iets misgegaan bij het update van uw bestelling.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			LoadData();
		}

		private void btnSearch_Click_1(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(txtSearch.Text))
			{
				datagridOrders.ItemsSource = _bestellingOnderdeelRepository.GetPartsInActiveShoppingCartBySearch(_currentBestellingID, txtSearch.Text);
			}
			else
			{
				LoadData();
			}
			txtSearch.Clear();
		}

		private void btnPay_Click(object sender, RoutedEventArgs e)
		{
			string foutmelding = string.Empty;

			if (bestellingOnderdelen.Count() <= 0)
			{
				foutmelding += "Eerst onderdelen toevoegen aan uw bestelling.\n";
				_window.SelectIndex(0);
			}
			else
			{
				foutmelding = ControleerOnderdelen(foutmelding);
				if (string.IsNullOrEmpty(foutmelding))
				{

					string error = UpdateOnderdeelToDatabase();
					if (string.IsNullOrEmpty(error))
					{
						//bestelling leegmaken en active bestelling op false zetten
						BestellingAfhandelen();
					}
					else
					{
						MessageBox.Show(error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else
				{
					MessageBox.Show(foutmelding, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private string ControleerOnderdelen(string foutmelding)
		{
			foreach (var onderdeelBestelling in bestellingOnderdelen)
			{
				Onderdeel onderdeel = _onderdeelRepository.GetPartById(onderdeelBestelling.Onderdeel.Id);
				if (onderdeel == null)
				{
					foutmelding += "Onderdeel in bestelling bestaat niet meer.\n";
				}
				else
				{
					if (onderdeelBestelling.Aantal > onderdeel.Aantal)
					{
						foutmelding += $"Te weinig in stock ({onderdeel.Aantal}) van {onderdeel.Naam}.\n";
					}
				}
			}

			return foutmelding;
		}

		private string UpdateOnderdeelToDatabase()
		{
			string error = string.Empty;

			foreach (var onderdeelBestelling in bestellingOnderdelen)
			{
				Onderdeel onderdeel = _onderdeelRepository.GetPartById(onderdeelBestelling.Onderdeel.Id);
				if (onderdeel != null && onderdeelBestelling.Aantal <= onderdeel.Aantal)
				{
					onderdeel.Aantal -= onderdeelBestelling.Aantal;
					if (!_onderdeelRepository.UpdatePart(onderdeel))
					{
						error += "Fout bij update database.";
					}
				}
				else
				{
					error += "Er is iets foutgegaan bij het wegschrijven van uw bestelling.";
				}
			}
			return error;
		}

		private void BestellingAfhandelen()
		{
			bestelling.BestellingCheck = false;
			if (_bestellingRepository.UpdatePart(bestelling))
			{
				lblTotal.Content = "Bestelling plaatsen gelukt.";
				datagridOrders.ItemsSource = null;
			}
			else
			{
				MessageBox.Show("Er is iets foutgegaan bij het wegschrijven van uw bestelling.", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			LoadData();

		}
	}
}
