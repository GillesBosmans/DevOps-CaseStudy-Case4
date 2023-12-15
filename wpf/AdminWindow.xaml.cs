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

namespace wpf
{
	/// <summary>
	/// Interaction logic for AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		private int klantID;
		public AdminWindow(int klantID)
		{
			InitializeComponent();
			this.klantID = klantID;
		}

		private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			string geselecteerdTabblad = (tabControl.SelectedItem as TabItem).Name;
			switch (geselecteerdTabblad)
			{
				case "tabMain":
					ContentWindow.Content = new AdminMainView();
					break;
				case "tabAssortiment":
					ContentWindow.Content = new AdminAssortimentView();
					break;
				case "tabAuto":
					ContentWindow.Content = new AdminCarInsertView();
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
	}
}
