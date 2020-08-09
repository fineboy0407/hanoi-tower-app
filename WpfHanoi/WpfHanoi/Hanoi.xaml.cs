using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfHanoi.Model;

namespace WpfHanoi
{
    /*
     * 
    1. Move disc from column 1 to column 3
    2. Move disc from column 1 to column 2
    3. Move disc from column 3 to column 2
    4. Move disc from column 1 to column 3
    5. Move disc from column 2 to column 1
    6. Move disc from column 2 to column 3
    7. Move disc from column 1 to column 3
    8. Repeat Step 1 until all discs are on column 3
     * 
     */
    public partial class Hanoi : Window
    {
        public List<Disc> ColumnA { get; set; }
        public List<Disc> ColumnB { get; set; }
        public List<Disc> ColumnC { get; set; }
        private int _discNumber { get; set; }

        private int moves;
        private int step;

        private bool autoMode { get; set; }

        public Hanoi(int discNumber)
        {
            InitializeComponent();
            _discNumber = discNumber;
            moves = 0;
            step = 1;
            autoMode = false;

            ColumnA = new List<Disc>();
            ColumnB = new List<Disc>();
            ColumnC = new List<Disc>();

            if (discNumber <= 1)
            {
                discNumber = 2;
            }

            for (var i = discNumber; i > 0; --i)
            {
                ColumnA.Add(new Disc
                {
                    Size = i,
                    Rectangle = new Rectangle
                    {
                        Fill = GetRandomBrush(),
                        Height = (discNumber * 25) < 230 ? 25 : 230 / discNumber,
                        Width = ((190d - 40d) / (discNumber - 1d) * (i - 1d)) + 40d
                    }
                });
                Thread.Sleep(15);
            }

            Draw();
        }

        public void RunAutoAlgorithm(object sender, RoutedEventArgs events)
        {
            autoMode = true;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (e, s) =>
            {
                Disc disc = null;
                switch (step)
                {
                    case 1:
                        disc = ColumnA.Last();
                        ColumnC.Add(disc);
                        ColumnA.Remove(disc);
                        break;
                    case 2:
                        disc = ColumnA.Last();
                        ColumnB.Add(disc);
                        ColumnA.Remove(disc);
                        break;
                    case 3:
                        disc = ColumnC.Last();
                        ColumnB.Add(disc);
                        ColumnC.Remove(disc);
                        break;
                    case 4:
                        disc = ColumnA.Last();
                        ColumnC.Add(disc);
                        ColumnA.Remove(disc);
                        break;
                    case 5:
                        disc = ColumnB.Last();
                        ColumnA.Add(disc);
                        ColumnB.Remove(disc);
                        break;
                    case 6:
                        disc = ColumnB.Last();
                        ColumnC.Add(disc);
                        ColumnB.Remove(disc);
                        break;
                    default:
                        step = 1;
                        break;
                }
                ++step;
                ++moves;

                if (ColumnA.Count == 0 && ColumnB.Count == 0)
                {
                    CheckIfSucess();
                    timer.Stop();
                }

                Draw();
            };
            timer.Start();
        }

        public void CheckIfSucess()
        {
            if (ColumnA.Count == 0 && ColumnB.Count == 0)
            {
                HanoiCanvas.Children.Clear();

                var title = new Label
                {
                    Foreground = Brushes.Green,
                    Content = autoMode ? "Very nice run!" : "You did it!",
                    FontSize = 45,
                    FontWeight = FontWeights.Black
                };
                title.SetValue(Canvas.LeftProperty, 279d);
                title.SetValue(Canvas.TopProperty, 75d);

                var titleShadow = new Label
                {
                    Foreground = Brushes.Gray,
                    Content = autoMode ? "Very nice run!" : "You did it!",
                    FontSize = 45,
                    FontWeight = FontWeights.Black
                };
                titleShadow.SetValue(Canvas.LeftProperty, 282d);
                titleShadow.SetValue(Canvas.TopProperty, 76d);
                titleShadow.SetValue(Panel.ZIndexProperty, -1);

                var points = new Label
                {
                    Foreground = Brushes.Green,
                    Content = moves + " Moves",
                    FontSize = 80
                };
                points.SetValue(Canvas.LeftProperty, 242d);
                points.SetValue(Canvas.TopProperty, 186d);

                var backToMainBtn = new Button
                {
                    BorderThickness = new Thickness(0),
                    Background = Brushes.LightGreen,
                    Height = 30,
                    Width = 162,
                    Content = "Back to Main",
                    FontSize = 15
                };
                backToMainBtn.SetValue(Canvas.LeftProperty, 328d);
                backToMainBtn.SetValue(Canvas.TopProperty, 352d);
                backToMainBtn.Click += (s, e) =>
                {
                    new MainWindow().Show();
                    Close();
                };

                HanoiCanvas.Children.Add(title);
                HanoiCanvas.Children.Add(titleShadow);
                HanoiCanvas.Children.Add(points);
                HanoiCanvas.Children.Add(backToMainBtn);
            }
        }

        private void Draw()
        {
            HanoiCanvas.Children.Clear();

            /* Auto Run Button */
            if (moves == 0)
            {
                var btn = new Button
                {
                    Content = "Auto Run",
                    BorderThickness = new Thickness(0),
                    Background = Brushes.LightGreen,
                    Padding = new Thickness(10, 4, 10, 4),
                    Margin = new Thickness(10)
                };
                btn.Click += RunAutoAlgorithm;
                HanoiCanvas.Children.Add(btn);
            }

            /* Hanoi I  */

            var rectABase = new Rectangle { Height = 15, Width = 200, Fill = Brushes.Black };
            rectABase.SetValue(Canvas.LeftProperty, 40d);
            rectABase.SetValue(Canvas.TopProperty, 340d);
            HanoiCanvas.Children.Add(rectABase);

            var rectATile = new Rectangle { Height = 250, Width = 14, Fill = Brushes.Black };
            rectATile.SetValue(Canvas.LeftProperty, 133d);
            rectATile.SetValue(Canvas.TopProperty, 100d);
            HanoiCanvas.Children.Add(rectATile);

            for (var i = 0; i < ColumnA.Count; ++i)
            {
                var disc = ColumnA[i].Rectangle;
                disc.SetValue(Canvas.LeftProperty, (140d - (disc.Width / 2)));
                disc.SetValue(Canvas.TopProperty, 340d - ((i + 1) * ((_discNumber * 25) < 230 ? 25 : 230 / _discNumber)));

                HanoiCanvas.Children.Add(disc);
            }

            /* Hanoi II  */

            var rectBBase = new Rectangle { Height = 15, Width = 200, Fill = Brushes.Black };
            rectBBase.SetValue(Canvas.LeftProperty, 280d);
            rectBBase.SetValue(Canvas.TopProperty, 340d);
            HanoiCanvas.Children.Add(rectBBase);

            var rectBTile = new Rectangle { Height = 250, Width = 15, Fill = Brushes.Black };
            rectBTile.SetValue(Canvas.LeftProperty, 373d);
            rectBTile.SetValue(Canvas.TopProperty, 100d);
            HanoiCanvas.Children.Add(rectBTile);

            for (var i = 0; i < ColumnB.Count; ++i)
            {
                var disc = ColumnB[i].Rectangle;
                disc.SetValue(Canvas.LeftProperty, (380d - (disc.Width / 2)));
                disc.SetValue(Canvas.TopProperty, 340d - ((i + 1) * ((_discNumber * 25) < 230 ? 25 : 230 / _discNumber)));

                HanoiCanvas.Children.Add(disc);
            }

            /* Hanoi III  */

            var rectCBase = new Rectangle { Height = 15, Width = 200, Fill = Brushes.Black };
            rectCBase.SetValue(Canvas.LeftProperty, 520d);
            rectCBase.SetValue(Canvas.TopProperty, 340d);
            HanoiCanvas.Children.Add(rectCBase);

            var rectCTile = new Rectangle { Height = 250, Width = 15, Fill = Brushes.Black };
            rectCTile.SetValue(Canvas.LeftProperty, 613d);
            rectCTile.SetValue(Canvas.TopProperty, 100d);
            HanoiCanvas.Children.Add(rectCTile);

            for (var i = 0; i < ColumnC.Count; ++i)
            {
                var disc = ColumnC[i].Rectangle;
                disc.SetValue(Canvas.LeftProperty, (620d - (disc.Width / 2)));
                disc.SetValue(Canvas.TopProperty, 340d - ((i + 1) * ((_discNumber * 25) < 230 ? 25 : 230 / _discNumber)));

                HanoiCanvas.Children.Add(disc);
            }

            if (!autoMode)
            {
                /* Setup Buttons */
                var aToB = new Button
                {
                    IsEnabled = CheckIfEnabled(ColumnA, ColumnB),
                    Background = Brushes.AliceBlue,
                    Padding = new Thickness(10, 4, 10, 4),
                    Content = "A --> B",
                    Width = 75
                };
                aToB.SetValue(Canvas.LeftProperty, 40d);
                aToB.SetValue(Canvas.TopProperty, 370d);
                aToB.Click += DiskMove;

                var aToC = new Button
                {
                    IsEnabled = CheckIfEnabled(ColumnA, ColumnC),
                    Background = Brushes.AliceBlue,
                    Padding = new Thickness(10, 4, 10, 4),
                    Content = "A --> C",
                    Width = 75
                };
                aToC.SetValue(Canvas.LeftProperty, 165d);
                aToC.SetValue(Canvas.TopProperty, 370d);
                aToC.Click += DiskMove;

                var bToA = new Button
                {
                    IsEnabled = CheckIfEnabled(ColumnB, ColumnA),
                    Background = Brushes.AliceBlue,
                    Padding = new Thickness(10, 4, 10, 4),
                    Content = "B --> A",
                    Width = 75
                };
                bToA.SetValue(Canvas.LeftProperty, 280d);
                bToA.SetValue(Canvas.TopProperty, 370d);
                bToA.Click += DiskMove;

                var bToC = new Button
                {
                    IsEnabled = CheckIfEnabled(ColumnB, ColumnC),
                    Background = Brushes.AliceBlue,
                    Padding = new Thickness(10, 4, 10, 4),
                    Content = "B --> C",
                    Width = 75
                };
                bToC.SetValue(Canvas.LeftProperty, 405d);
                bToC.SetValue(Canvas.TopProperty, 370d);
                bToC.Click += DiskMove;

                var cToA = new Button
                {
                    IsEnabled = CheckIfEnabled(ColumnC, ColumnA),
                    Background = Brushes.AliceBlue,
                    Padding = new Thickness(10, 4, 10, 4),
                    Content = "C --> A",
                    Width = 75
                };
                cToA.SetValue(Canvas.LeftProperty, 520d);
                cToA.SetValue(Canvas.TopProperty, 370d);
                cToA.Click += DiskMove;

                var cToB = new Button
                {
                    IsEnabled = CheckIfEnabled(ColumnC, ColumnB),
                    Background = Brushes.AliceBlue,
                    Padding = new Thickness(10, 4, 10, 4),
                    Content = "C --> B",
                    Width = 75
                };
                cToB.SetValue(Canvas.LeftProperty, 645d);
                cToB.SetValue(Canvas.TopProperty, 370d);
                cToB.Click += DiskMove;

                HanoiCanvas.Children.Add(aToB);
                HanoiCanvas.Children.Add(aToC);
                HanoiCanvas.Children.Add(bToA);
                HanoiCanvas.Children.Add(bToC);
                HanoiCanvas.Children.Add(cToA);
                HanoiCanvas.Children.Add(cToB);
                CheckIfSucess();

            }

        }

        private void DiskMove(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var start = btn.Content.ToString().ToCharArray().First();
            var end = btn.Content.ToString().ToCharArray().Last();
            Disc disc = null;

            if (start == 'A')
            {
                disc = ColumnA.Last();
                ColumnA.Remove(disc);
            }
            else if (start == 'B')
            {
                disc = ColumnB.Last();
                ColumnB.Remove(disc);
            }
            else if (start == 'C')
            {
                disc = ColumnC.Last();
                ColumnC.Remove(disc);
            }


            if (end == 'A')
            {
                ColumnA.Add(disc);
            }
            else if (end == 'B')
            {
                ColumnB.Add(disc);
            }
            else if (end == 'C')
            {
                ColumnC.Add(disc);
            }
            moves = moves + 1;
            Draw();
        }

        private Brush GetRandomBrush()
        {
            var rand = new Random();
            return new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
        }

        public bool CheckIfEnabled(List<Disc> from, List<Disc> to)
        {
            if (from.Count == 0)
                return false;

            if (to.Count == 0)
                return true;

            if (from.Last().Size < to.Last().Size)
                return true;

            return false;
        }

    }
}
