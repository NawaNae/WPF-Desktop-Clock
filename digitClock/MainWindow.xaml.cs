using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
namespace digitClock
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private string exeName
        {
            get { return System.AppDomain.CurrentDomain.FriendlyName; }
        }
        private string exePath
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().Location; }
        }
        private bool bootAutoRunning
        {
            
            set {
                RegistryKey regAutoRunClock = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run",true);
                string name = exeName;
                if (value)
                    regAutoRunClock.SetValue(name, exePath);
                else
                    if (regAutoRunClock.GetValue(name) != null)
                        regAutoRunClock.DeleteValue(name, false);
                regAutoRunClock.Close();
                ; }
            get {
                RegistryKey regAutoRunClock = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);
                return regAutoRunClock.GetValue(exeName) != null && (string)regAutoRunClock.GetValue(exeName)== exePath; }
        }
        DispatcherTimer timer;
        System.IntPtr hwnd;
        
        System.Windows.Forms.NotifyIcon notifyIcon;
        
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().Location);
            while (DateTime.Now.ToString(".f").Last<char>()!='0')
                ;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += new EventHandler(clockUpdate);
            timer.Start();
            
        }
        int[] extendedStyle = new int[2];
        private void initializeExtendStyle()
        {
            hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            this.extendedStyle[1] = Win32.GetWindowLong(hwnd.ToInt32(), -20);
            this.extendedStyle[0] = this.extendedStyle[1] | 32;
        }
        private void settingLoad()
        {
            Top = Properties.Settings.Default.position.Y;
            Left = Properties.Settings.Default.position.X;
           topMostCheckbox.IsChecked =Topmost= Properties.Settings.Default.topMost;
           fontColorAlpha.Value = Properties.Settings.Default.fontColor.A;
           fontColorRed.Value = Properties.Settings.Default.fontColor.R;
           fontColorGreen.Value = Properties.Settings.Default.fontColor.G;
           fontColorBlue.Value = Properties.Settings.Default.fontColor.B;
           shadowColorRed.Value = Properties.Settings.Default.fontShadowColor.R;
           shadowColorGreen.Value = Properties.Settings.Default.fontShadowColor.G;
           shadowColorBlue.Value = Properties.Settings.Default.fontShadowColor.B;
           backgroundColorAlpha.Value = Properties.Settings.Default.fontBackgroundColor.A;
           backgroundColorRed.Value = Properties.Settings.Default.fontBackgroundColor.R;
           backgroundColorGreen.Value = Properties.Settings.Default.fontBackgroundColor.G;
           backgroundColorBlue.Value = Properties.Settings.Default.fontBackgroundColor.B;
            shadowBlurSize.Value = Properties.Settings.Default.fontShadowBlur;
           shadowAngle.Value = Properties.Settings.Default.fontShadowAngle;
           shadowDepth.Value = Properties.Settings.Default.fontShadowDepth;
           timeFormat.Text = Properties.Settings.Default.timeFormat;
           fontSize.Value = Properties.Settings.Default.fontSize;
            touchableItem.Checked = Properties.Settings.Default.touchable;
            moveableItem.Checked = Properties.Settings.Default.moveable;
            fontBold.IsChecked = Properties.Settings.Default.fontBold;
            fontItalic.IsChecked = Properties.Settings.Default.fontItalic;
        }

        private void settingSave()
        {
            Properties.Settings.Default.position = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.fontColor = System.Drawing.Color.FromArgb(
                (int)(fontColorAlpha.Value),
                (int)(fontColorRed.Value),
                (int)(fontColorGreen.Value),
                (int)(fontColorBlue.Value));
            Properties.Settings.Default.fontShadowColor = System.Drawing.Color.FromArgb(
                255,
                (int)(shadowColorRed.Value),
                (int)(shadowColorGreen.Value),
                (int)(shadowColorBlue.Value));
            Properties.Settings.Default.fontBackgroundColor = System.Drawing.Color.FromArgb(
                (int)(backgroundColorAlpha.Value),
                (int)(backgroundColorRed.Value),
                (int)(backgroundColorGreen.Value),
                (int)(backgroundColorBlue.Value));
            Properties.Settings.Default.fontShadowBlur= shadowBlurSize.Value ;
           Properties.Settings.Default.fontShadowAngle  = shadowAngle.Value;
            Properties.Settings.Default.fontShadowDepth = shadowDepth.Value;
            Properties.Settings.Default.topMost = (bool)topMostCheckbox.IsChecked;
            Properties.Settings.Default.timeFormat= timeFormat.Text;
           Properties.Settings.Default.fontSize = fontSize.Value ;
            Properties.Settings.Default.fontName =font.SelectedValue.ToString()  ;
            Properties.Settings.Default.touchable=touchableItem.Checked;
            Properties.Settings.Default.moveable=moveableItem.Checked  ;
            Properties.Settings.Default.fontBold = (bool)fontBold.IsChecked;
            Properties.Settings.Default.fontItalic =(bool)fontItalic.IsChecked ;
            Properties.Settings.Default.Save();

        }
        private void clockUpdate(object sender, EventArgs e)
        {
            try
            {
                this.timeDisplay.Content = DateTime.Now.ToString(this.timeFormat.Text);
            }
            catch { }
        }
        private void timeDisplay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (moveableItem.Checked)
                if (e.LeftButton == MouseButtonState.Pressed)
                 this.DragMove();
        }
        private void option_Click(object sender, RoutedEventArgs e)
        {
            this.optionsPanel.Visibility = 1-this.optionsPanel.Visibility;
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = (bool)((CheckBox)sender).IsChecked;
        }
        public void closeProcess()
        {
            notifyIcon.Visible = false;
            this.settingSave();
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void fontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.timeDisplay != null)
            {
                this.timeDisplay.FontSize = ((Slider)sender).Value;
                this.fontSizeDisplay.Content = "Font Size : " + Math.Round(((Slider)sender).Value,2) + " px";
            }
        }
        private void timeDisplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.controlPanel.Visibility = 1 - this.controlPanel.Visibility;//雙擊開啟關閉控制面板
        }
        private void fontColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.timeDisplay.Foreground = new SolidColorBrush(Color.FromArgb(
            (byte)this.fontColorAlpha.Value,
            (byte)this.fontColorRed.Value,
            (byte)this.fontColorGreen.Value,
            (byte)this.fontColorBlue.Value));
        }
        private void fontShadowColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           this.labelShadow.Color =  Color.FromRgb(
            (byte)this.shadowColorRed.Value,
            (byte)this.shadowColorGreen.Value,
            (byte)this.shadowColorBlue.Value);
        }
        private void shadowBlurSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.labelShadow != null)
            {
                this.labelShadow.BlurRadius = this.shadowBlurSize.Value;
                this.blurDisplay.Content = "Blur radius : " + Math.Round(this.labelShadow.BlurRadius,1) + " px";
            }
        }
        private void shadowAngle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.labelShadow.Direction = this.shadowAngle.Value;
            this.angleDisplay.Content = "Angle : " + Math.Round(this.labelShadow.Direction,1) + "°";
        }
        private void shadowDepth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.labelShadow.ShadowDepth = this.shadowDepth.Value;
            this.depthDisplay.Content = "Depth : " + Math.Round(this.labelShadow.ShadowDepth,1) + " px";
        }
        private void font_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timeDisplay != null) 
                this.timeDisplay.FontFamily = new System.Windows.Media.FontFamily(((ComboBox)sender).SelectedValue.ToString());
        }
        private void font_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox thisComboBox = (ComboBox)sender;
            foreach (System.Drawing.FontFamily oneFontFamily in System.Drawing.FontFamily.Families)
            {

                thisComboBox.Items.Add(oneFontFamily.Name);
                if (oneFontFamily.Name == Properties.Settings.Default.fontName)
                    thisComboBox.SelectedIndex = thisComboBox.Items.Count - 1;
            }
        }
        private void mousePassThrough_Click(object sender, RoutedEventArgs e)
        {
            touchableItem.Checked = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initializeNotifyIcon();
            initializeExtendStyle();
            settingLoad();
            touchableSet();
            if (!touchableItem.Checked)
                controlPanel.Visibility = Visibility.Hidden;
            moveableButtonContentChange();
            bootAutoRunCheckbox.IsChecked = bootAutoRunning;
        }
        private void fontBold_Checked(object sender, RoutedEventArgs e)
        {
            timeDisplay.FontWeight = FontWeights.Bold;
        }
        private void fontItalic_Checked(object sender, RoutedEventArgs e)
        {
            timeDisplay.FontStyle = FontStyles.Italic;
        }
        private void fontBold_Unchecked(object sender, RoutedEventArgs e)
        {
            timeDisplay.FontWeight = FontWeights.Normal;
        }
        private void fontItalic_Unchecked(object sender, RoutedEventArgs e)
        {
            timeDisplay.FontStyle = FontStyles.Normal;
        }
        private void moveableButton_Click(object sender, RoutedEventArgs e)
        {
            moveableItem.Checked = !moveableItem.Checked;
        }
        private void backgroundColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.timeDisplay.Background = new SolidColorBrush(Color.FromArgb(
            (byte)this.backgroundColorAlpha.Value,
            (byte)this.backgroundColorRed.Value,
            (byte)this.backgroundColorGreen.Value,
            (byte)this.backgroundColorBlue.Value));
        }

        private void bootAutoRunCheckbox_Click(object sender, RoutedEventArgs e)
        {
            var that = sender as CheckBox;
            bootAutoRunning = (bool)that.IsChecked;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeProcess();
        }
    }
}
