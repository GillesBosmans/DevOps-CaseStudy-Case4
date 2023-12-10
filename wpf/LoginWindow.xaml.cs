using System.Windows;
using System.Windows.Input;

namespace wpf
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public LoginWindow()
		{
			InitializeComponent();
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

		private void btnLogIn_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
