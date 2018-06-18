using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TicTacToe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Unloaded += MainPage_Unloaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            DetermineVisualState();
        }

        void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            DetermineVisualState();
        }

        private void DetermineVisualState()
        {
            var state = string.Empty;
            var applicationView = ApplicationView.Value;
            var size = Window.Current.Bounds;

            if (applicationView.ToString() == "FullScreenLandscape")
            {
                SnappedGrid.Visibility = Visibility.Collapsed;
                ContentGrid.Visibility = Visibility.Visible;
            }
            else if (applicationView.ToString() == "Filled")
            {
                SnappedGrid.Visibility = Visibility.Collapsed;
                ContentGrid.Visibility = Visibility.Visible;
            }
            else if (applicationView.ToString() == "Snapped")
            {
                SnappedGrid.Visibility = Visibility.Visible;
                ContentGrid.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            showTitleAnimation.Begin();
            showShareAnimation.Begin();
            showRateAnimation.Begin();
            showSettingsAnimation.Begin();
            showPlayerVsComputerAnimation.Begin();
            showPlayerVsPlayerAnimation.Begin();
            showMultiPlayerAnimation.Begin();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.CenterY = 100;
            scaleTransform.ScaleY = 0;
            PlayerVsComputerGrid.RenderTransform = scaleTransform;
            PlayerVsPlayerGrid.RenderTransform = scaleTransform;
            MultiPlayerGrid.RenderTransform = scaleTransform;
        }

        private void PlayerVsComputerGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OverlayGrid.Visibility = Visibility.Visible;
            puPVC.IsOpen = true;
        }

        private void PlayerVsPlayerGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OverlayGrid.Visibility = Visibility.Visible;
            puPVP.IsOpen = true;
        }

        private void imgShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                DataTransferManager.ShowShareUI();

                DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareLinkHandler);
            }
            catch (Exception ex)
            {
                MessageDialog msgDialog = new MessageDialog("Please, try again after some time.", "Sorry !");
                msgDialog.ShowAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        private void ShareLinkHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Classic Tic Tac Toe";
            request.Data.Properties.Description = "Check out 'Classic Tic Tac Toe' for Windows Store.";

            string appID = CurrentApp.AppId.ToString();
            request.Data.SetUri(new Uri("http://apps.microsoft.com/windows/app/" + appID));
        }

        private async void imgRate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            string pacFamilyName = packageId.FamilyName;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + pacFamilyName));
        }

        private void imgSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void OverlayGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OverlayGrid.Visibility = Visibility.Collapsed;
            puPVC.IsOpen = false;
            puPVP.IsOpen = false;
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "Name")
            {
                txtName.Text = string.Empty;
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrEmpty(txtName.Text)) || (string.IsNullOrWhiteSpace(txtName.Text)))
            {
                txtName.Text = "Name";
            }
        }

        private void txtName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (txtName.Text.Length > 9)
            {
                e.Handled = true;
            }
        }

        private void imgGo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((txtName.Text == "Name") || (string.IsNullOrEmpty(txtName.Text)) || (string.IsNullOrWhiteSpace(txtName.Text)))
            {
                MessageDialog msgDialog = new MessageDialog("You forgot to enter player name.", "Oops !");
                msgDialog.ShowAsync();
            }
            else
            {
                puPVC.IsOpen = false;
                App.playerOne = txtName.Text;
                App.gameMode = "PlayerVsComputer";
                this.Frame.Navigate(typeof(App_Page.GamePage));
            }
        }

        private void txtName1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtName1.Text == "First")
            {
                txtName1.Text = string.Empty;
            }
        }

        private void txtName1_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrEmpty(txtName1.Text)) || (string.IsNullOrWhiteSpace(txtName1.Text)))
            {
                txtName1.Text = "First";
            }
        }

        private void txtName1_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (txtName1.Text.Length > 9)
            {
                e.Handled = true;
            }
        }

        private void txtName2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtName2.Text == "Second")
            {
                txtName2.Text = string.Empty;
            }
        }

        private void txtName2_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrEmpty(txtName2.Text)) || (string.IsNullOrWhiteSpace(txtName2.Text)))
            {
                txtName2.Text = "Second";
            }
        }

        private void txtName2_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (txtName2.Text.Length > 9)
            {
                e.Handled = true;
            }
        }

        private void imgGo1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((txtName1.Text == "First") || (string.IsNullOrEmpty(txtName1.Text)) || (string.IsNullOrWhiteSpace(txtName1.Text)))
            {
                if ((txtName2.Text == "Second") || (string.IsNullOrEmpty(txtName2.Text)) || (string.IsNullOrWhiteSpace(txtName2.Text)))
                {
                    MessageDialog msgDialog = new MessageDialog("You forgot to enter player names.", "Oops !");
                    msgDialog.ShowAsync();
                }
                else
                {
                    MessageDialog msgDialog = new MessageDialog("You forgot to enter first player name.", "Oops !");
                    msgDialog.ShowAsync();
                }
            }
            else
            {
                if ((txtName2.Text == "Second") || (string.IsNullOrEmpty(txtName2.Text)) || (string.IsNullOrWhiteSpace(txtName2.Text)))
                {
                    MessageDialog msgDialog = new MessageDialog("You forgot to second enter player name.", "Oops !");
                    msgDialog.ShowAsync();
                }
                else
                {
                    puPVP.IsOpen = false;
                    App.playerOne = txtName1.Text;
                    App.playerTwo = txtName2.Text;
                    App.gameMode = "PlayerVsPlayer";
                    this.Frame.Navigate(typeof(App_Page.GamePage));
                }
            }
        }   
    }
}
