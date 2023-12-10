using models;
using System.Windows.Controls;

namespace wpf
{
	/// <summary>
	/// Interaction logic for AdminAssortimentView.xaml
	/// </summary>
	public partial class AdminAssortimentView : UserControl
	{
		public AdminAssortimentView()
		{
			InitializeComponent();
		}

		private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}

		private void cmbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void datagridAssortiment_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			btnEdit.IsEnabled = true;
			btnDelete.IsEnabled = true;
		}
	}
}