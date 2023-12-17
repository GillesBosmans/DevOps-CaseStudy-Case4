using dal;
using dal.api;
using models;
using System.Windows;
using System.Windows.Input;

namespace wpf
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private IKlantRepository _klantRepository = new KlantRepository();
		ApiLoginValidatie apiLoginValidatie = new ApiLoginValidatie();

		public LoginWindow()
		{
			InitializeComponent();
		}

		private async void btnLogIn_Click(object sender, RoutedEventArgs e)
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
			
			LoginValidatie loginValidatie = await apiLoginValidatie.AuthenticateKlantAsync(email, password);

			if (loginValidatie != null)
			{
				if (_klantRepository.GetKlantByEmail(loginValidatie.Name) is Klant klant)
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
					lblnotifications.Content = $"{loginValidatie.Message}";
				}
			}
			else
			{
				lblnotifications.Content = "Gegevens om in te loggen zijn niet geldig!";
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
