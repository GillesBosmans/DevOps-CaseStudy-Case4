using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private int _klantID;
		public MainWindow(int klantID)
		{
			InitializeComponent();
			this._klantID = klantID;
			tabControl.SelectedIndex = 0;
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
					ContentWindow.Content = new AssortimentView(_klantID);
					break;
				case "tabOrder":
					ContentWindow.Content = new OrderView(_klantID, this);
					break;
				default:
					break;
			}
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
			Application.Current.Shutdown();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
		}

		public void SelectIndex(int tab)
		{
			tabControl.SelectedIndex = tab;
		}

	}
}
