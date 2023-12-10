using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			string geselecteerdTabblad = (tabControl.SelectedItem as TabItem).Name;
			switch (geselecteerdTabblad)
			{
				case "tabMain":
					ContentWindow.Content = new MainView();
					break;
				case "tabAssortiment":
					ContentWindow.Content = new AssortimentView();
					break;
				case "tabOrder":
					ContentWindow.Content = new OrderView();
					break;
				default:
					break;
			}
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

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
		}
	}
}
