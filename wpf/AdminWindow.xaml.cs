using System.Windows;
using System.Windows.Input;

namespace wpf
{
	/// <summary>
	/// Interaction logic for AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		public AdminWindow()
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


		private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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