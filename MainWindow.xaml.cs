using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Save.Enums;
using System.IO;
using System.Windows.Data;
using Microsoft.Win32;

namespace Save
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Objects
        ViewModel viewModel { get; set; }
        #endregion

        #region Properties
        public int nBoxes { get; set; }
        public int NBoxes
        {
            get
            {
                return nBoxes;
            }
            set
            {
                if (nBoxes != Int32.Parse(NOfBoxes))
                {
                    nBoxes = Int32.Parse(NOfBoxes);
                    OnPropertyChange(nameof(NBoxes));
                }
            }
        }
        public List<string> SourcesList
        {
            get
            {
                return new List<string>() { "1", "2", "3", "4", "5", "6", "8", "10", "12" };
            }
        }
        private string nOfBoxes { get; set; } = "3";
        public string NOfBoxes
        {
            get
            {
                return nOfBoxes;
            }
            set
            {
                if (nOfBoxes != value)
                {
                    nOfBoxes = value;
                    viewModel.StartControls();
                    OnPropertyChange(nameof(NOfBoxes));
                }
            }
        }
        public int mainWindowHeight { get; set; }
        public int MainWindowHeight
        {
            get
            {
                return mainWindowHeight;
            }
            set
            {
                if (mainWindowHeight != value)
                {
                    mainWindowHeight = value;
                    OnPropertyChange(nameof(MainWindowHeight));
                }
            }
        }
        public string windowHeight { get; set; }
        public string WindowHeight
        {
            get
            {
                return windowHeight;
            }
            set
            {
                if (windowHeight != value)
                {
                    windowHeight = value;
                    OnPropertyChange(nameof(WindowHeight));
                }
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel(this);
            this.DataContext = viewModel;
            viewModel.MaximizeIt = new Command(() => ReshapeWindow(MainWindowFunctions.Maximize), () => YouCanNot());
            viewModel.NormalizeIt = new Command(() => ReshapeWindow(MainWindowFunctions.Normalize), () => YouCanNot());
            viewModel.MinimizeIt = new Command(() => ReshapeWindow(MainWindowFunctions.Minimize), () => YouCan());
            viewModel.CloseIt = new Command(() => ReshapeWindow(MainWindowFunctions.CloseWin), () => YouCan());
            this.MouseDown += delegate { DragMove(); };
            this.SetBinding(MainWindow.MinHeightProperty, new Binding(WindowHeight) { Source = viewModel });
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            string[] sd = { "Save", "Delete" };
            string[] df = { "Directory", "File" };
            SaveDel.ItemsSource = sd;
            SaveDel.SelectedItem = sd[0];
            DirFile.ItemsSource = df;
            DirFile.SelectedItem = df[0];
            viewModel.Info = string.Empty;
            InitializeCombo(0);
            viewModel.StartControls();
            TextBoxHomeDir.Text = "Home";
        }
        public void InitializeCombo(int nameIndex)
        {
            viewModel.Info = string.Empty;
            Combo.Text = string.Empty;
            viewModel.Item = string.Empty;
            if (!File.Exists(viewModel.dir + "PathNamesList.bin"))
            {
                viewModel.Lists.SaveList(viewModel.PathProject.TmpList, "PathNamesList");
                viewModel.Lists.SaveList2D(viewModel.PathProject.Tmp, "Tmp");
                viewModel.Info = "Please, create a path project!";
            }
            else
            {
                viewModel.PathNamesList = viewModel.Lists.LoadList("PathNamesList");
            }
            viewModel.PathProject.PathList = new List<List<string>>(viewModel.Lists.LoadList2D(viewModel.PathNamesList[nameIndex]));
            Combo.ItemsSource = viewModel.PathNamesList;
            viewModel.Item = viewModel.PathNamesList[nameIndex];
            Combo.SelectedItem = viewModel.PathNamesList[nameIndex];
            Combo.Text = viewModel.Item;
            viewModel.NOfBoxes = viewModel.PathProject.PathList[0].Count().ToString();
            viewModel.NSources = viewModel.PathProject.PathList[0].Count();
        }
        public void FillTextBoxes()
        {
            viewModel.NSources = viewModel.PathProject.PathList[0].Count;
            for (int i = 0; i < viewModel.NSources; i++)
            {
                foreach (TextBox tb in Source.Children)
                {
                    if (tb.Name == "TBSource" + i.ToString())
                    {
                        tb.Text = viewModel.PathProject.PathList[0][i];
                    }
                }
                foreach (TextBox tb in Target.Children)
                {
                    if (tb.Name == "TBTarget" + i.ToString())
                    {
                        tb.Text = viewModel.PathProject.PathList[1][i];
                    }
                }
                foreach (TextBox tb in Drives.Children)
                {
                    if (tb.Name == "TBDrives" + i.ToString())
                    {
                        tb.Text = viewModel.PathProject.PathList[2][i];
                    }
                }
                foreach (TextBox tb in Excluded.Children)
                {
                    if (tb.Name == "TBExcluded" + i.ToString())
                    {
                        tb.Text = viewModel.PathProject.PathList[3][i];
                    }
                }
            }
            viewModel.TBChanged = false;
            for (int i = 0; i < viewModel.NSources; i++)
            {
                foreach (TextBox tbs in Source.Children)
                {
                    if (tbs.Name == "TBSource" + i.ToString() && !String.IsNullOrEmpty(tbs.Text))
                    {
                        viewModel.TBSourceFilled = true;
                    }
                }
            }
        }
        private void TextBoxDoubleClick(object obj, EventArgs e)
        {
            TextBox tb = obj as TextBox;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (viewModel.ItemDirFile == "Directory")
                {
                    tb.Text = Path.GetDirectoryName(openFileDialog.FileName) + @"\";
                }
                else if (viewModel.ItemDirFile == "File")
                {
                    tb.Text = openFileDialog.FileName;
                }
            }
        }
        private void DrivesBox_MouseDoubleClick(object sender, EventArgs e)
        {
            string dr = "C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            TextBox tb = sender as TextBox;
            tb.Text = dr;
        }
        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression binding = Combo.GetBindingExpression(ComboBox.TextProperty);
            binding.UpdateSource();
        }
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (Combo.SelectedItem != null)
            {
                viewModel.Item = Combo.SelectedItem.ToString();
                Combo.Text = viewModel.Item;
            }
            else
            {
                Combo.Text = "";
            }
            if (!String.IsNullOrEmpty(viewModel.Item) && !String.IsNullOrWhiteSpace(viewModel.Item))
            {
                Combo.ItemsSource = viewModel.PathNamesList;
                if (viewModel.PathNamesList.Contains(viewModel.Item))
                {
                    int i = viewModel.PathNamesList.IndexOf(viewModel.Item);
                    Combo.SelectedItem = viewModel.PathNamesList[i];
                }
            }
            else
            {
                viewModel.Item = "Please, select a project";
            }
            viewModel.PathProject.PathList = new List<List<string>>(viewModel.Lists.LoadList2D(viewModel.Item));
            viewModel.NSources = viewModel.PathProject.PathList[0].Count();
            viewModel.NTargets = viewModel.PathProject.PathList[1].Count();
            viewModel.StartControls();
            FillTextBoxes();
        }
        void ClickEvent(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            //ClickedNumber = button.Content.ToString();
            //ButtonName = button.Name;
            //TypeOrDelete();
        }
        public bool YouCan()
        {
            return true;
        }
        public bool YouCanNot()
        {
            return false;
        }
        public void ReshapeWindow(MainWindowFunctions mode)
        {
            switch (mode)
            {
                case MainWindowFunctions.Maximize:
                    this.WindowState = WindowState.Maximized;
                    viewModel.Visible = mode;
                    break;
                case MainWindowFunctions.Normalize:
                    this.WindowState = WindowState.Normal;
                    viewModel.Visible = mode;
                    break;
                case MainWindowFunctions.Minimize:
                    this.WindowState = WindowState.Minimized;
                    break;
                case MainWindowFunctions.CloseWin:
                    Application.Current.Shutdown();
                    break;
                default:
                    throw new NotImplementedException(string.Format($"{mode.ToString()} not implemented"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
