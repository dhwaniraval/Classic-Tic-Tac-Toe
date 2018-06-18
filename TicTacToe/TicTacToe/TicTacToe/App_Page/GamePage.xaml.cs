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

namespace TicTacToe.App_Page
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public static int checkCount { get; set; }
        public static string checkValue { get; set; }
        public static string checkTurn { get; set; }

        public GamePage()
        {
            this.InitializeComponent();
            this.Loaded += GamePage_Loaded;
            this.Unloaded += GamePage_Unloaded;
        }

        void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            DetermineVisualState();
        }

        void GamePage_Unloaded(object sender, RoutedEventArgs e)
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
            if (App.gameMode == "PlayerVsComputer")
            {
                lblPlayer1.Text = "X-" + App.playerOne + " :";
                lblPlayer1S.Text = "X-" + App.playerOne + " :";
                lblPlayer2.Text = "O-Computer :";
                lblPlayer2S.Text = "O-Computer :";
                App.playerOneScore = 0;
                App.playerTwoScore = 0;
                App.drawScore = 0;
                lblPlayer1Score.Text = App.playerOneScore.ToString();
                lblPlayer1ScoreS.Text = App.playerOneScore.ToString();
                lblPlayer2Score.Text = App.playerTwoScore.ToString();
                lblPlayer2ScoreS.Text = App.playerTwoScore.ToString();
                lblDrawScore.Text = App.drawScore.ToString();
                lblDrawScoreS.Text = App.drawScore.ToString();
            }
            else if (App.gameMode == "PlayerVsPlayer")
            {
                lblPlayer1.Text = "X-" + App.playerOne + " :";
                lblPlayer1S.Text = "X-" + App.playerOne + " :";
                lblPlayer2.Text = "O-" + App.playerTwo + " :";
                lblPlayer2S.Text = "O-" + App.playerTwo + " :";
                App.playerOneScore = 0;
                App.playerTwoScore = 0;
                App.drawScore = 0;
                lblPlayer1Score.Text = App.playerOneScore.ToString();
                lblPlayer1ScoreS.Text = App.playerOneScore.ToString();
                lblPlayer2Score.Text = App.playerTwoScore.ToString();
                lblPlayer2ScoreS.Text = App.playerTwoScore.ToString();
                lblDrawScore.Text = App.drawScore.ToString();
                lblDrawScoreS.Text = App.drawScore.ToString();
                checkCount = 0;
                checkTurn = "0";
            }
            else
            {

            }
            showTitleAnimation.Begin();
            showScoreBoardAnimation.Begin();
            showShareAnimation.Begin();
            showRateAnimation.Begin();
            showSettingsAnimation.Begin();
            showLine1Animation.Begin();
            showLine2Animation.Begin();
            showLine3Animation.Begin();
            showLine4Animation.Begin();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                ScaleTransform scaleTransform = new ScaleTransform();
                scaleTransform.CenterX = 0;
                scaleTransform.ScaleX = 0;
                recH1.RenderTransform = scaleTransform;
                recH2.RenderTransform = scaleTransform;
                scaleTransform = new ScaleTransform();
                scaleTransform.CenterY = 0;
                scaleTransform.ScaleY = 0;
                recV1.RenderTransform = scaleTransform;
                recV2.RenderTransform = scaleTransform;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void imgBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Value11_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue11.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue11.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue11.Text == checkValue) && (lblValue12.Text == checkValue) && (lblValue13.Text == checkValue))
                        {
                            showWin1Animation.Begin();
                            showWin1Animation.Completed += showWin1Animation_Completed;
                        }
                        else if ((lblValue11.Text == checkValue) && (lblValue21.Text == checkValue) && (lblValue31.Text == checkValue))
                        {
                            showWin4Animation.Begin();
                            showWin4Animation.Completed += showWin4Animation_Completed;
                        }
                        else if ((lblValue11.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            recAWin7.Visibility = Visibility.Visible;
                            setWinner();
                            OverlayGrid.Visibility = Visibility.Visible;
                            puShowWinner.IsOpen = true;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value12_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue12.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue12.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue11.Text == checkValue) && (lblValue12.Text == checkValue) && (lblValue13.Text == checkValue))
                        {
                            showWin1Animation.Begin();
                            showWin1Animation.Completed += showWin1Animation_Completed;
                        }
                        else if ((lblValue12.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue32.Text == checkValue))
                        {
                            showWin5Animation.Begin();
                            showWin5Animation.Completed += showWin5Animation_Completed;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value13_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue13.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue13.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue11.Text == checkValue) && (lblValue12.Text == checkValue) && (lblValue13.Text == checkValue))
                        {
                            showWin1Animation.Begin();
                            showWin1Animation.Completed += showWin1Animation_Completed;
                        }
                        else if ((lblValue13.Text == checkValue) && (lblValue23.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            showWin6Animation.Begin();
                            showWin6Animation.Completed += showWin6Animation_Completed;
                        }
                        else if ((lblValue13.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue31.Text == checkValue))
                        {
                            recAWin8.Visibility = Visibility.Visible;
                            setWinner();
                            OverlayGrid.Visibility = Visibility.Visible;
                            puShowWinner.IsOpen = true;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value21_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue21.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue21.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue11.Text == checkValue) && (lblValue21.Text == checkValue) && (lblValue31.Text == checkValue))
                        {
                            showWin4Animation.Begin();
                            showWin4Animation.Completed += showWin4Animation_Completed;
                        }
                        else if ((lblValue21.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue23.Text == checkValue))
                        {
                            showWin2Animation.Begin();
                            showWin2Animation.Completed += showWin2Animation_Completed;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value22_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue22.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue22.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue12.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue32.Text == checkValue))
                        {
                            showWin5Animation.Begin();
                            showWin5Animation.Completed += showWin5Animation_Completed;
                        }
                        else if ((lblValue21.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue23.Text == checkValue))
                        {
                            showWin2Animation.Begin();
                            showWin2Animation.Completed += showWin2Animation_Completed;
                        }
                        else if ((lblValue11.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            recAWin7.Visibility = Visibility.Visible;
                            setWinner();
                            OverlayGrid.Visibility = Visibility.Visible;
                            puShowWinner.IsOpen = true;
                        }
                        else if ((lblValue13.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue31.Text == checkValue))
                        {
                            recAWin8.Visibility = Visibility.Visible;
                            setWinner();
                            OverlayGrid.Visibility = Visibility.Visible;
                            puShowWinner.IsOpen = true;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value23_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue23.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue23.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue13.Text == checkValue) && (lblValue23.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            showWin6Animation.Begin();
                            showWin6Animation.Completed += showWin6Animation_Completed;
                        }
                        else if ((lblValue21.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue23.Text == checkValue))
                        {
                            showWin2Animation.Begin();
                            showWin2Animation.Completed += showWin2Animation_Completed;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value31_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue31.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue31.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue11.Text == checkValue) && (lblValue21.Text == checkValue) && (lblValue31.Text == checkValue))
                        {
                            showWin4Animation.Begin();
                            showWin4Animation.Completed += showWin4Animation_Completed;
                        }
                        else if ((lblValue31.Text == checkValue) && (lblValue32.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            showWin3Animation.Begin();
                            showWin3Animation.Completed += showWin3Animation_Completed;
                        }
                        else if ((lblValue13.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue31.Text == checkValue))
                        {
                            recAWin8.Visibility = Visibility.Visible;
                            setWinner();
                            OverlayGrid.Visibility = Visibility.Visible;
                            puShowWinner.IsOpen = true;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value32_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue32.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue32.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue12.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue32.Text == checkValue))
                        {
                            showWin5Animation.Begin();
                            showWin5Animation.Completed += showWin5Animation_Completed;
                        }
                        else if ((lblValue31.Text == checkValue) && (lblValue32.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            showWin3Animation.Begin();
                            showWin3Animation.Completed += showWin3Animation_Completed;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private void Value33_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblValue33.Text))
            {
                if (App.gameMode == "PlayerVsPlayer")
                {
                    lblValue33.Text = setValue();
                    if (checkCount >= 5)
                    {
                        if ((lblValue13.Text == checkValue) && (lblValue23.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            showWin6Animation.Begin();
                            showWin6Animation.Completed += showWin6Animation_Completed;
                        }
                        else if ((lblValue31.Text == checkValue) && (lblValue32.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            showWin3Animation.Begin();
                            showWin3Animation.Completed += showWin3Animation_Completed;
                        }
                        else if ((lblValue11.Text == checkValue) && (lblValue22.Text == checkValue) && (lblValue33.Text == checkValue))
                        {
                            recAWin7.Visibility = Visibility.Visible;
                            setWinner();
                            OverlayGrid.Visibility = Visibility.Visible;
                            puShowWinner.IsOpen = true;
                        }
                        else
                        {
                            if (checkCount >= 9)
                            {
                                App.drawScore++;
                                lblDrawScore.Text = App.drawScore.ToString();
                                lblShowWinner.Text = "Draw";
                                OverlayGrid.Visibility = Visibility.Visible;
                                puShowWinner.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        private string setValue()
        {
            string setValue = "";

            if (checkCount == 0)
            {
                if (checkTurn == "0")
                {
                    checkValue = "X";
                    setValue = "X";
                    checkCount++;
                    checkTurn = "1";
                }
                else if (checkTurn == "1")
                {
                    checkValue = "O";
                    setValue = "O";
                    checkCount++;
                    checkTurn = "0";
                }
            }
            else if (checkCount <= 9)
            {
                if (checkValue == "X")
                {
                    checkValue = "O";
                    setValue = "O";
                    checkCount++;
                }
                else if (checkValue == "O")
                {
                    checkValue = "X";
                    setValue = "X";
                    checkCount++;
                }
            }

            return setValue;
        }

        void showWin1Animation_Completed(object sender, object e)
        {
            setWinner();
            OverlayGrid.Visibility = Visibility.Visible;
            puShowWinner.IsOpen = true;
        }

        void showWin2Animation_Completed(object sender, object e)
        {
            setWinner();
            OverlayGrid.Visibility = Visibility.Visible;
            puShowWinner.IsOpen = true;
        }

        void showWin3Animation_Completed(object sender, object e)
        {
            setWinner();
            OverlayGrid.Visibility = Visibility.Visible;
            puShowWinner.IsOpen = true;
        }

        void showWin4Animation_Completed(object sender, object e)
        {
            setWinner();
            OverlayGrid.Visibility = Visibility.Visible;
            puShowWinner.IsOpen = true;
        }

        void showWin5Animation_Completed(object sender, object e)
        {
            setWinner();
            OverlayGrid.Visibility = Visibility.Visible;
            puShowWinner.IsOpen = true;
        }

        void showWin6Animation_Completed(object sender, object e)
        {
            setWinner();
            OverlayGrid.Visibility = Visibility.Visible;
            puShowWinner.IsOpen = true;
        }

        private void setWinner()
        {
            if (checkValue == "X")
            {
                App.playerOneScore++;
                lblShowWinner.Text = "Winner : " + App.playerOne;
                lblPlayer1Score.Text = App.playerOneScore.ToString();
                lblPlayer1ScoreS.Text = App.playerOneScore.ToString();
            }
            else if (checkValue == "O")
            {
                App.playerTwoScore++;
                lblShowWinner.Text = "Winner : " + App.playerTwo;
                lblPlayer2Score.Text = App.playerTwoScore.ToString();
                lblPlayer2ScoreS.Text = App.playerTwoScore.ToString();
            }
        }

        private void clearGame()
        {
            recAWin7.Visibility = Visibility.Collapsed;
            recAWin8.Visibility = Visibility.Collapsed;
            lblValue11.Text = string.Empty;
            lblValue12.Text = string.Empty;
            lblValue13.Text = string.Empty;
            lblValue21.Text = string.Empty;
            lblValue22.Text = string.Empty;
            lblValue23.Text = string.Empty;
            lblValue31.Text = string.Empty;
            lblValue32.Text = string.Empty;
            lblValue33.Text = string.Empty;
            checkCount = 0;
        }

        private void imgNext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.CenterX = 0;
            scaleTransform.ScaleX = 0;
            recHWin1.RenderTransform = scaleTransform;
            recHWin2.RenderTransform = scaleTransform;
            recHWin3.RenderTransform = scaleTransform;
            scaleTransform = new ScaleTransform();
            scaleTransform.CenterY = 0;
            scaleTransform.ScaleY = 0;
            recVWin4.RenderTransform = scaleTransform;
            recVWin5.RenderTransform = scaleTransform;
            recVWin6.RenderTransform = scaleTransform;
            OverlayGrid.Visibility = Visibility.Collapsed;
            puShowWinner.IsOpen = false;
            clearGame();
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
    }
}
