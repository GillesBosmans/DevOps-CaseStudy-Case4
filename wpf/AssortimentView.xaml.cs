using dal;
using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// Interaction logic for AssortimentView.xaml
	/// </summary>
	public partial class AssortimentView : UserControl
	{
		private int _klantID;
		private ICategorieRepository _categorieRepository = new CategorieRepository();
		private IOnderdeelRepository _onderdeelRepository = new OnderdeelRepository();
		private IBestellingRepository _bestellingRepository = new BestellingRepository();
		private IBestellingOnderdeelRepository _bestellingOnderdeelRepository = new BestellingOnderdeelRepository();

		public AssortimentView(int klantID)
		{
			InitializeComponent();
			btnAddToOrder.IsEnabled = false;
			this._klantID = klantID;
			LoadData();
		}

		private void LoadData()
		{
			cmbCategorie.ItemsSource = _categorieRepository.GetCategories();
			cmbCategorie.SelectedValuePath = "Id";
			LoadPartsAndCars();
		}

		private void LoadPartsAndCars()
		{
			datagridAssortiment.ItemsSource = _onderdeelRepository.GetPartsAndCarsWhereAmountNotZero();
		}

		private void btnAddToOrder_Click(object sender, RoutedEventArgs e)
		{
			lblNotifications.Content = string.Empty;

			if (datagridAssortiment.SelectedItem is Onderdeel onderdeel)
			{
				if (int.TryParse(txtAmount.Text, out int amount) && amount <= onderdeel.Aantal && amount > 0)
				{
					ProcessOrder(onderdeel, amount);
				}
				else
				{
					lblNotifications.Content = amount > 0 ? $"Er is maar \'{onderdeel.Aantal}\' in stock." : "Aantal moet groter zijn dan 0.";
				}
			}
			else
			{
				MessageBox.Show("Geselecteerd onderdeel is niet geldig.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		//Voor het toevoegen van een onderdeel of het aanmaken van een bestelling.
		private void ProcessOrder(Onderdeel onderdeel, int amount)
		{
			Bestelling bestelling = _bestellingRepository.GetActiveOrderByKlantId(_klantID);

			if (bestelling == null)
			{
				CreateNewBestelling(onderdeel, amount);
			}
			else
			{
				InsertPartIntoBestelling(onderdeel, amount, bestelling);
			}
		}

		//Voor het aanmaken van een nieuwe bestelling en het toevoegen van een onderdeel met een hoeveelheid.
		//indien er nog geen bestelling is.
		private void CreateNewBestelling(Onderdeel onderdeel, int amount)
		{
			Bestelling bestelling = new Bestelling
			{
				Datum = DateTime.Now,
				BestellingCheck = true,
				KlantId = _klantID
			};

			if (_bestellingRepository.CreateOrder(bestelling))
			{
				bestelling = _bestellingRepository.GetActiveOrderByKlantId(_klantID);

				if (bestelling == null)
				{
					ShowErrorMessage("Fout bij het ophalen van de bestelling.");
				}
				else
				{
					InsertPartIntoBestelling(onderdeel, amount, bestelling);
				}
			}
			else
			{
				ShowErrorMessage("Fout bij het maken van een bestelling.");
			}
		}

		//Voor het toevoegen van een onderdeel met een hoeveelheid
		//indien er een bestaande bestelling is.
		private void InsertPartIntoBestelling(Onderdeel onderdeel, int amount, Bestelling bestelling)
		{
			IEnumerable<BestellingOnderdeel> listBestellingOnderdeel = _bestellingOnderdeelRepository.GetPartsInActiveShoppingCart(bestelling.Id);
			bool added = false;

			if (listBestellingOnderdeel.Count()! > 0)
			{
				InsertBestellingOnderdeel(onderdeel, amount, bestelling);
				return;
			}

			foreach (var bestellingOnderdeel in listBestellingOnderdeel)
			{
				if (bestellingOnderdeel.Onderdeel.Id == onderdeel.Id)
				{
					BestellingOnderdeel bo = bestellingOnderdeel;
					bo.Aantal += amount;

					if (_bestellingOnderdeelRepository.UpdatePartInOrder(bo))
					{
						lblNotifications.Content = $"\'{onderdeel.Naam}\' is toegevoegd aan de bestelling.";
						txtAmount.Text = string.Empty;
						added = true;
					}
				}
			}
			if (!added)
			{
				InsertBestellingOnderdeel(onderdeel, amount, bestelling);
			}
		}

		//Voor het toevoegen van een onderdeel in een bestelling met een hoeveelheid.
		private void InsertBestellingOnderdeel(Onderdeel onderdeel, int amount, Bestelling bestelling)
		{
			BestellingOnderdeel bestellingOnderdeel = new BestellingOnderdeel
			{
				BestellingId = bestelling.Id,
				OnderdeelId = onderdeel.Id,
				Aantal = amount
			};

			if (_bestellingOnderdeelRepository.InsertPartInOrder(bestellingOnderdeel))
			{
				lblNotifications.Content = $"\'{onderdeel.Naam}\' is toegevoegd aan uw bestelling.";
				txtAmount.Text = string.Empty;
			}
		}

		private void cmbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbCategorie.SelectedItem is Categorie categorie)
			{
				datagridAssortiment.ItemsSource = _onderdeelRepository.GetPartsAndCarsByCategorieId(categorie.Id);
			}
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			if (cmbCategorie.SelectedItem is Categorie categorie && !string.IsNullOrEmpty(txtSearch.Text))
			{
				datagridAssortiment.ItemsSource = _onderdeelRepository.GetPartsByCategorieAndSearch(categorie.Id, txtSearch.Text);
			}
			else if (!string.IsNullOrEmpty(txtSearch.Text))
			{
				datagridAssortiment.ItemsSource = _onderdeelRepository.GetPartsBySearch(txtSearch.Text);
			}
			else
			{
				LoadPartsAndCars();
			}
			ClearFields();
		}

		private void ClearFields()
		{
			cmbCategorie.SelectedValue = null;
			txtSearch.Text = string.Empty;
		}

		private void datagridAssortiment_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			btnAddToOrder.IsEnabled = true;
		}

		private void ShowErrorMessage(string message)
		{
			MessageBox.Show(message, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
