using System.Windows;
using System.Windows.Input;

namespace wpf
{
	/// <summary>
	/// Interaction logic for AdminAssortimentCruWindow.xaml
	/// </summary>
	public partial class AdminAssortimentCruWindow : Window
	{
		public AdminAssortimentCruWindow()
		{
			InitializeComponent();
		}

		private void btnOpslaanCar_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnAddCategorie_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnNormal_Click(object sender, RoutedEventArgs e)
		{

        }

		private void btnMinimize_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{

		}

		private void cmbCars_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{

		}

		private void cmbCategorie_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{

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