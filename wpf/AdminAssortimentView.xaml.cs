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
	/// Interaction logic for AdminAssortimentView.xaml
	/// </summary>
	public partial class AdminAssortimentView : UserControl
	{
		private ICategorieRepository _categorieRepository = new CategorieRepository();
		private IOnderdeelRepository _onderdeelRepository = new OnderdeelRepository();

		public AdminAssortimentView()
		{
			InitializeComponent();
			LoadData();
			btnEdit.IsEnabled = false;
			btnDelete.IsEnabled = false;
		}

		private void LoadData()
		{
			cmbCategorie.ItemsSource = _categorieRepository.GetCategories();
			cmbCategorie.SelectedValuePath = "Id";
			LoadPartsAndCars();
		}

		private void LoadPartsAndCars()
		{
			datagridAssortiment.ItemsSource = _onderdeelRepository.GetPartsAndCars();
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			AdminAssortimentCruWindow AdminAssortimentCruWindow = new AdminAssortimentCruWindow();
			AdminAssortimentCruWindow.ShowDialog();
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			if (datagridAssortiment.SelectedItem is Onderdeel onderdeel)
			{
				AdminAssortimentCruWindow AdminAssortimentCruWindow = new AdminAssortimentCruWindow(onderdeel.Id);
				AdminAssortimentCruWindow.ShowDialog();
			}
			else
			{
				MessageBox.Show("Geselecteerd onderdeel is niet geldig.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (datagridAssortiment.SelectedItem is Onderdeel onderdeel)
			{
				if (MessageBox.Show($"Weet je zeker dat je dit \'{onderdeel.Type}\' wilt verwijderen?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
				{
					_onderdeelRepository.DeletePart(onderdeel.Id);
					LoadPartsAndCars();
				}
				else
				{
					MessageBox.Show($"\'{onderdeel.Type}\' niet verwijderen.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			else
			{
				MessageBox.Show("Geen auto geselecteerd");
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
			cmbCategorie.SelectedItem = null;
			txtSearch.Text = string.Empty;
		}

		private void datagridAssortiment_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			btnEdit.IsEnabled = true;
			btnDelete.IsEnabled = true;
		}
	}
}