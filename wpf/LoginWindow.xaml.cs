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
using System.Xml.Linq;

namespace wpf
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private IKlantRepository _klantRepository = new KlantRepository();

		public LoginWindow()
		{
			InitializeComponent();
		}

		private void btnLogIn_Click(object sender, RoutedEventArgs e)
		{
			string email = txtEmail.Text.ToLower();
			string password = txtPassword.Password;


			if (string.IsNullOrEmpty(email))
			{
				lblnotifications.Content = "Email moet ingevuld zijn!";
				return;
			}
			if (string.IsNullOrEmpty(password))
			{
				lblnotifications.Content = "Paswoord moet ingevuld zijn!";
				return;
			}

			if (_klantRepository.GetKlantByEmail(email) is Klant klant)
			{
				if (klant.Paswoord.Trim() != password.Trim())
				{
					lblnotifications.Content = "Paswoord is fout!";
					return;
				}

				if (klant.Status)
				{
					AdminWindow adminWindow = new AdminWindow(klant.Id);
					adminWindow.Show();
					this.Close();
				}
				else
				{
					MainWindow mainWindow = new MainWindow(klant.Id);
					mainWindow.Show();
					this.Close();
				}
			}
			else
			{
				lblnotifications.Content = "Email is niet geldig!";
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
