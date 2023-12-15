using dal;
using models;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for AdminCarInsertView.xaml
	/// </summary>
	public partial class AdminCarInsertView : UserControl
	{
		private IAutoRepository _autoRepository = new AutoRepository();
		private IOnderdeelRepository _onderdeelRepository = new OnderdeelRepository();

		public AdminCarInsertView()
		{
			InitializeComponent();
			LoadCars();
		}

		public void LoadCars()
		{
			IEnumerable<Auto> autos = _autoRepository.GetCars();

			cmbCars.ItemsSource = autos;
			cmbCars.Items.Refresh();
		}

		private void btnAddCar_Click(object sender, RoutedEventArgs e)
		{
			string foutmelding = Valideer();
			lblNotifications.Content = string.Empty;

			if (!string.IsNullOrEmpty(foutmelding))
			{
				lblNotifications.Content = foutmelding;
				return;
			}

			Auto auto = new Auto()
			{
				Merk = txtBrand.Text.ToUpper(),
				Model = txtModel.Text.ToUpper(),
				Prijs = decimal.Parse(txtPrice.Text),
				Productiejaar = int.Parse(txtproductionYear.Text),
				Kleur = txtColor.Text.ToLower(),
				Chassisnummer = txtChassisNumber.Text
			};

			if (!auto.IsValid())
			{
				lblNotifications.Content = auto.Error;
				return;
			}

			if (!_autoRepository.InsertCar(auto))
			{
				lblNotifications.Content = "Er is iets misgegaan bij het toevoegen van een auto.";
				return;
			}

			lblNotifications.Content = "Auto succesvol toegevoegd.";
			ClearFields();
			LoadCars();
		}

		private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
		{
			string foutmeliding = string.Empty;
			if (cmbCars.SelectedItem is Auto auto)
			{
				//Controle of de auto nog onderdelen bevat zo ja, kan de auto niet verwijdert worden.
				if (CheckCarHasPart(auto))
				{
					if (MessageBox.Show($"Weet je zeker dat je de auto met chassisnummer: \'{auto.Chassisnummer}\' wilt verwijderen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
					{
						if (_autoRepository.DeleteCar(auto.Id))
						{
							LoadCars();
							lblNotifications.Content = $"{auto.Merk} - {auto.Model} - VIN:{auto.Chassisnummer} is verwijderd.";
						}
						else
						{
							foutmeliding += "Fout bij toevoegen van auto.";
						}
					}
					else
					{
						foutmeliding += $"\'{auto.Chassisnummer}\' niet verwijderen.";
					}
				}
				else
				{
					foutmeliding += "Auto heeft nog onderdelen.\nDeze dienen eerst verwijdert te worden.";
				}
			}
			else
			{
				foutmeliding += "Geen auto geselecteerd";
			}
			ShowFoutmelding(foutmeliding);
		}

		private bool CheckCarHasPart(Auto auto)
		{
			bool status = true;
			IEnumerable<Onderdeel> onderdelen = _onderdeelRepository.GetPartsAndCars();
			foreach (var onderdeel in onderdelen)
			{
				if (onderdeel.AutoID == auto.Id)
				{
					status = false;
				}
			}
			return status;
		}

		private string Valideer()
		{
			string foutmelding = "";

			if (!decimal.TryParse(txtPrice.Text, out decimal price))
			{
				foutmelding += "Prijs moet een numerieke waarde zijn." + Environment.NewLine;
			}
			if (!int.TryParse(txtproductionYear.Text, out int productioYear))
			{
				foutmelding += "Productiejaar moet een numerieke waarde zijn." + Environment.NewLine;
			}

			return foutmelding;
		}

		private void ClearFields()
		{
			txtBrand.Text = string.Empty;
			txtModel.Text = string.Empty;
			txtPrice.Text = string.Empty;
			txtproductionYear.Text = string.Empty;
			txtColor.Text = string.Empty;
			txtChassisNumber.Text = string.Empty;
		}

		private void ShowFoutmelding(string foutmeliding)
		{
			if (!string.IsNullOrEmpty(foutmeliding))
			{
				MessageBox.Show(foutmeliding, "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
}
