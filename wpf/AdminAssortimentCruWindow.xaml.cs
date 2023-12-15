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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace wpf
{
	/// <summary>
	/// Interaction logic for AdminAssortimentCruWindow.xaml
	/// </summary>
	public partial class AdminAssortimentCruWindow : Window
	{
		private int? _partID;
		private bool _edit = false;
		private IAutoRepository _autoRepository = new AutoRepository();
		private ICategorieRepository _categorieRepository = new CategorieRepository();
		private IOnderdeelRepository _onderdeelRepository = new OnderdeelRepository();

		public AdminAssortimentCruWindow(int? partID = null)
		{
			this._partID = partID;
			InitializeComponent();
			LoadData();
		}

		public void LoadData()
		{
			cmbCars.ItemsSource = _autoRepository.GetCars();
			cmbCars.SelectedValuePath = "Id";
			cmbCars.Items.Refresh();
			LoadCategories();

			if (_partID != null)
			{
				Onderdeel onderdeel = _onderdeelRepository.GetPartById(_partID.Value);
				if (onderdeel != null)
				{
					cmbCategorie.SelectedValue = onderdeel.CategorieID;
					cmbCars.SelectedValue = onderdeel.AutoID;
					txtName.Text = onderdeel.Naam;
					txtType.Text = onderdeel.Type;
					txtPrice.Text = onderdeel.Prijs.ToString();
					txtAmount.Text = onderdeel.Aantal.ToString();
					txtPhoto.Text = onderdeel.Foto;
					txtDescription.Text = onderdeel.Omschrijving;
					_edit = true;
				}
				else
				{
					MessageBox.Show("Fout opgetreden bij het ophalen van het onderdeel.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		public void LoadCategories()
		{
			cmbCategorie.ItemsSource = _categorieRepository.GetCategories();
			cmbCategorie.SelectedValuePath = "Id";
			cmbCategorie.Items.Refresh();
		}

		private void btnAddCategorie_Click(object sender, RoutedEventArgs e)
		{
			Categorie categorie = new Categorie();
			categorie.Naam = txtCategorie.Text;
			if (!categorie.IsValid())
			{
				MessageBox.Show("Categorie mag niet leeg zijn.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			categorie.Naam = char.ToUpper(categorie.Naam[0]) + categorie.Naam.Substring(1).ToLower();
			if (!_categorieRepository.InsertCategorie(categorie))
			{
				MessageBox.Show("Er is iets fout opgetreden bij het toevoegen van een categorie.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			LoadCategories();
			lblNotifications.Content = $"Nieuwe categorie \'{categorie.Naam}\' succesvol toegevoegd.";
			txtCategorie.Clear();
		}

		private void btnOpslaanCar_Click(object sender, RoutedEventArgs e)
		{
			string foutmelding = Valideer();
			lblNotifications.Content = string.Empty;

			if (!string.IsNullOrEmpty(foutmelding))
			{
				lblNotifications.Content = foutmelding;
				return;
			}
			Onderdeel onderdeel = new Onderdeel()
			{
				Id = _edit ? _partID.Value : 0,
				Naam = txtName.Text,
				Type = txtType.Text,
				Prijs = decimal.Parse(txtPrice.Text),
				Aantal = int.Parse(txtAmount.Text),
				Foto = txtPhoto.Text,
				Omschrijving = txtDescription.Text,
				AutoID = ((Auto)cmbCars.SelectedItem).Id,
				CategorieID = ((Categorie)cmbCategorie.SelectedItem).Id
			};

			if (!onderdeel.IsValid())
			{
				lblNotifications.Content = onderdeel.Error;
				return;
			}

			if (_edit)
			{
				if (!_onderdeelRepository.UpdatePart(onderdeel))
				{
					lblNotifications.Content = "Er is een fout opgetreden bij het bewerken van een onderdeel.";
				}
				else
				{
					lblNotifications.Content = $"Onderdeel \'{onderdeel.Naam} - {onderdeel.Type}\' succesvol bewerkt.";
					ClearFields();
				}
			}
			else
			{
				if (!_onderdeelRepository.InsertPart(onderdeel))
				{
					lblNotifications.Content = "Er is een fout opgetreden bij het toevoegen van een onderdeel.";
				}
				else
				{
					lblNotifications.Content = $"Onderdeel '{onderdeel.Naam} - {onderdeel.Type}' succesvol toegevoegd.";
					ClearFields();
				}
			}

		}

		private string Valideer()
		{
			string foutmelding = "";

			if (!decimal.TryParse(txtPrice.Text, out decimal price))
			{
				foutmelding += "Prijs is moet een numerieke waarde zijn." + Environment.NewLine;
			}
			if (!int.TryParse(txtAmount.Text, out int number))
			{
				foutmelding += "Aantal moet een numerieke waarde zijn." + Environment.NewLine;
			}
			if ((cmbCategorie.SelectedIndex == -1 && string.IsNullOrEmpty(txtCategorie.Text)) || (!string.IsNullOrEmpty(txtCategorie.Text) && cmbCategorie.SelectedIndex != -1))
			{
				foutmelding += "Categorie is verplicht en u kan dit maar aan 1 categorie koppelen." + Environment.NewLine;
			}
			if (cmbCars.SelectedIndex == -1)
			{
				foutmelding += "Auto is verplicht." + Environment.NewLine;
			}

			return foutmelding;
		}

		private void ClearFields()
		{
			cmbCategorie.SelectedItem = null;
			cmbCars.SelectedItem = null;
			txtName.Text = string.Empty;
			txtType.Text = string.Empty;
			txtPrice.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtPhoto.Text = string.Empty;
			txtDescription.Text = string.Empty;
		}

		private void btnNormal_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Normal;
		}

		private void btnMinimize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
		}

	}
}
