using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using Save.Enums;
using Save.Controls;

namespace Save
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Objects
        public MainWindow MainWindow { get; set; }
        public ViewModel viewModel { get; set; }
        public PathProject PathProject { get; set; }
        public SaveObject SaveObject { get; set; }
        public DeleteObject DeleteObject { get; set; }
        public Lists Lists { get; set; }
        public Source Source { get; set; }
        public Target Target { get; set; }
        public Drives Drives { get; set; }
        public static CancellationTokenSource ts = new CancellationTokenSource();
        public CancellationToken ct = ts.Token;
        #endregion

        #region Visibilities
        public Visibility Visibility => MultipleTargets == false ? Visibility.Visible : Visibility.Hidden;
        public Visibility Visibility1 => MultipleTargets == true ? Visibility.Visible : Visibility.Hidden;
        private MainWindowFunctions visible = MainWindowFunctions.Normalize;
        public MainWindowFunctions Visible
        {
            get { return this.visible; }
            set
            {
                this.visible = value;

                OnPropertyChange(nameof(Visible));
                OnPropertyChange(nameof(MaximizeButton));
                OnPropertyChange(nameof(NormalizeButton));
            }
        }
        public Visibility MaximizeButton => Visible == MainWindowFunctions.Normalize ? Visibility.Visible : Visibility.Collapsed;
        public Visibility NormalizeButton => Visible == MainWindowFunctions.Maximize ? Visibility.Visible : Visibility.Collapsed;
        private Visibility settingsVis { get; set; } = Visibility.Collapsed;
        public Visibility SettingsVis
        {
            get { return this.settingsVis; }
            set
            {
                if (settingsVis != value)
                {
                    settingsVis = value;
                }
                OnPropertyChange(nameof(SettingsVis));
            }
        }
        private Visibility sourceVis { get; set; } = Visibility.Visible;
        public Visibility SourceVis
        {
            get { return sourceVis; }
            set
            {
                if (sourceVis != value)
                {
                    sourceVis = value;
                }
                OnPropertyChange(nameof(SourceVis));
            }
        }
        #endregion

        #region Properties
        public string[] sd { get; set; } = { "Save", "Delete" };
        public string dir
        {
            get
            {
                string dir = System.IO.Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Directory.Exists(dir + HomeDir + @"\"))
                {
                    dir += HomeDir + @"\_SaveChanges\Path\";
                }
                else
                {
                    dir += @"_SaveChanges\Path\";
                }
                return dir;
            }
        }
        public bool multipleTargets = false;
        public bool MultipleTargets
        {
            get { return multipleTargets; }
            set
            {
                if (multipleTargets != value)
                {
                    multipleTargets = value;
                    OnPropertyChange(nameof(MultipleTargets));
                }
                if (value == true)
                {
                    NSources = 1;
                    NTargets = PathProject.PathList[1].Count;
                    Make_SingleSource_MultipleTarget_Project();
                }
                else
                {
                    NSources = PathProject.PathList[0].Count;
                    NTargets = NSources;
                }
                StartControls();
            }
        }
        public bool correctPath = false;
        public bool CorrectPath
        {
            get { return correctPath; }
            set
            {
                if (correctPath != value)
                {
                    correctPath = value;
                    OnPropertyChange(nameof(CorrectPath));

                }
            }
        }
        public bool purge { get; set; } = true;
        public bool Purge
        {
            get { return purge; }
            set
            {
                if (purge != value)
                {
                    purge = value;
                    OnPropertyChange(nameof(Purge));
                }
            }
        }
        public bool automaticExit { get; set; } = false;
        public bool AutomaticExit
        {
            get { return automaticExit; }
            set
            {
                if (automaticExit != value)
                {
                    automaticExit = value;
                    OnPropertyChange(nameof(AutomaticExit));
                }
            }
        }
        public bool threads = false;
        public bool Threads
        {
            get { return threads; }
            set
            {
                if (threads != value)
                {
                    threads = value;
                    OnPropertyChange(nameof(Threads));
                }
            }
        }
        public bool parallel = false;
        public bool Parallel
        {
            get { return parallel; }
            set
            {
                if (parallel != value)
                {
                    parallel = value;
                    OnPropertyChange(nameof(Parallel));
                }
            }
        }
        public string homeDir { get; set; } = "Home";
        public string HomeDir
        {
            get { return homeDir; }
            set
            {
                if (homeDir != value)
                {
                    homeDir = value;
                    OnPropertyChange(nameof(HomeDir));
                }
            }
        }
        public bool localRobocopy = false;
        public bool LocalRobocopy
        {
            get { return localRobocopy; }
            set
            {
                if (localRobocopy != value)
                {
                    localRobocopy = value;
                    OnPropertyChange(nameof(LocalRobocopy));
                }
            }
        }
        public string item { get; set; }
        public string Item
        {
            get
            {
                return item;
            }
            set
            {
                List<string> l = new List<string>() {"A", "B", "C", "D", "E", "F","G", "H", "I","J", "K", "L","M", "N", "O","P", "Q", "R","S", "T", "U","V", "W", "X","Y", "Z",
                    "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                    "0","1","2","3","4","5","6","7","8","9"," ","","_","-",",",".","[","(",")","]","&","%","$","!","ß","ä","Ä","ö","Ö","ü","Ü","#"};
                if (value != null)
                {
                    string a = value.ToString();
                    foreach (char i in a)
                    {
                        if (!l.Contains(i.ToString()))
                        {
                            a = a.Replace(i.ToString(), " ");
                        }
                    }
                    if (item != a)
                    {
                        item = a;
                        OnPropertyChange(nameof(Item));
                        //OnPropertyChange(nameof(PathNamesList));
                    }
                }
            }
        }
        public List<string> PathNamesList
        {
            get
            {
                return Lists.LoadList("PathNamesList");
            }
            set
            {
                OnPropertyChange(nameof(PathNamesList));
                OnPropertyChange(nameof(Item));
            }
        }
        public string itemDirFile { get; set; }
        public string ItemDirFile
        {
            get
            {
                return itemDirFile;
            }
            set
            {
                if (itemDirFile != value)
                {
                    itemDirFile = value;
                    OnPropertyChange(nameof(ItemDirFile));
                }
            }
        }
        public string itemSaveDel { get; set; } = "Save";
        public string ItemSaveDel
        {
            get
            {
                return itemSaveDel;
            }
            set
            {
                if (itemSaveDel != value)
                {
                    itemSaveDel = value;
                    OnPropertyChange(nameof(ItemSaveDel));
                }
            }
        }
        private string info { get; set; }
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                if (info != value)
                {
                    info = value;
                    OnPropertyChange(nameof(Info));
                }
            }
        }
        private int delObject { get; set; } = 0;
        public int DelObject
        {
            get
            {
                return delObject;
            }
            set
            {
                if (delObject != value)
                {
                    delObject = value;
                    OnPropertyChange(nameof(DelObject));
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
        private Thickness buttonMargin { get; set; }
        public Thickness ButtonMargin
        {
            get
            {
                return buttonMargin;
            }
            set
            {
                if (buttonMargin != value)
                {
                    buttonMargin = value;
                    OnPropertyChange(nameof(ButtonMargin));
                }
            }
        }
        private string nOfBoxes { get; set; } = "2";
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
                    NSources = Int32.Parse(nOfBoxes);
                    NTargets = Int32.Parse(nOfBoxes);
                    StartControls();
                    OnPropertyChange(nameof(NOfBoxes));
                }
            }
        }
        private int nSources { get; set; } = 2;
        public int NSources
        {
            get
            {
                return nSources;
            }
            set
            {
                if (nSources != value)
                {
                    nSources = value;
                    OnPropertyChange(nameof(NSources));
                }
            }
        }
        private int nTargets { get; set; } = 2;
        public int NTargets
        {
            get
            {
                return nTargets;
            }
            set
            {
                if (nTargets != value)
                {
                    nTargets = value;
                    OnPropertyChange(nameof(NTargets));
                }
            }
        }
        private bool tBChanged { get; set; } = false;
        public bool TBChanged
        {
            get
            {
                return tBChanged;
            }
            set
            {
                if (tBChanged != value)
                {
                    tBChanged = value;
                    OnPropertyChange(nameof(TBChanged));
                    OnPropertyChange(nameof(TxtChanged));
                }
            }
        }
        private string txtChanged { get; set; }
        public string TxtChanged
        {
            get
            {
                return txtChanged;
            }
            set
            {
                if (txtChanged != value)
                {
                    txtChanged = value;
                    TBChanged = true;
                    OnPropertyChange(nameof(TxtChanged));
                }
            }
        }
        private bool tBSourceFilled { get; set; } = false;
        public bool TBSourceFilled
        {
            get
            {
                return tBSourceFilled;
            }
            set
            {
                if (tBSourceFilled != value)
                {
                    tBSourceFilled = value;
                    OnPropertyChange(nameof(TBSourceFilled));
                }
            }
        }
        private bool disable { get; set; } = false;
        public bool Disable
        {
            get
            {
                return disable;
            }
            set
            {
                if (disable != value)
                {
                    disable = value;
                    OnPropertyChange(nameof(Disable));
                    OnPropertyChange(nameof(ItemSaveDel));
                }
            }
        }
        #endregion
        public ViewModel(MainWindow mainWindow)
        {
            viewModel = this;
            PathProject = new PathProject(this);
            Lists = new Lists(this);
            Source = new Source(this);
            SaveObject = new SaveObject(this);
            DeleteObject = new DeleteObject(this);
            Target = new Target(this);
            Drives = new Drives(this);
            MainWindow = mainWindow;
            Save = new Command(() => SaveObject.Save(), () => YouCanSave());
            OpenSet = new Command(() => OpenSettings(), () => YouCan());
            DeleteIt = new Command(() => DeleteObject.Delete_Object(), () => YouCanDeleteObject());
            ChangePathProject = new Command(() => ChangeProject(), () => YouCanChangePath());
            NewPathProject = new Command(() => NewProject(), () => YouCanSaveNewPath());
            MaximizeIt = new Command(() => Max(), () => YouCanNot());
            DeletePathProject = new Command(() => DeleteProject(), () => YouCanDeletePath());
            ClearCombo = new Command(() => Del("DelC"), () => YouCanClear());
            Exit = new Command(() => ExitApp(), () => YouCan());
        }
        #region Commands
        public ICommand OpenSet { get; set; }
        public ICommand Save { get; set; }
        public ICommand DeleteIt { get; set; }
        public ICommand ChangePathProject { get; set; }
        public ICommand NewPathProject { get; set; }
        public ICommand DeletePathProject { get; set; }
        public ICommand ClearCombo { get; set; }
        public ICommand Exit { get; set; }
        public ICommand MaximizeIt { get; set; }
        public ICommand NormalizeIt { get; set; }
        public ICommand MinimizeIt { get; set; }
        public ICommand CloseIt { get; set; }
        #endregion
        public void StartControls()
        {
            if (MultipleTargets == true)
            {
                NSources = 1;
            }
            if (NSources != 0)
            {
                MainWindow.Source.Children.Clear();
                MainWindow.Drives.Children.Clear();
                MainWindow.Target.Children.Clear();
                MainWindow.Excluded.Children.Clear();
                MainWindow.ButtonsDelSource.Children.Clear();
                MainWindow.ButtonsDelDrives.Children.Clear();
                MainWindow.ButtonsDelTarget.Children.Clear();
                MainWindow.ButtonsDelExcluded.Children.Clear();
                MainWindow.LabelsSource.Children.Clear();
                MainWindow.LabelsDrives.Children.Clear();
                MainWindow.LabelsTarget.Children.Clear();
                MainWindow.LabelsExcluded.Children.Clear();
                for (int i = 0; i < NSources; i++)
                {
                    //##### Labels ####################################
                    MyLabel myLabel = new MyLabel();
                    myLabel.Name = "LabelSource" + i.ToString();
                    myLabel.Content = "Source " + (i + 1).ToString();
                    MainWindow.LabelsSource.Children.Add(myLabel);

                    //##### TextBoxes ####################################
                    MyTextBox myTB = new MyTextBox();
                    myTB.Name = "TBSource" + i.ToString();
                    myTB.MouseDoubleClick += new MouseButtonEventHandler(TextBoxDoubleClick);
                    Binding binding = new Binding("TxtChanged");
                    binding.Source = viewModel;
                    binding.Mode = BindingMode.OneWayToSource;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(myTB, TextBox.TextProperty, binding);
                    myTB.SetBinding(TextBox.TextProperty, binding);
                    MainWindow.Source.Children.Add(myTB);

                    //##### Buttons ####################################
                    MyButton newBtn1 = new MyButton();
                    newBtn1.Name = "DelS" + i.ToString();
                    newBtn1.Command = new Command(() => Del(newBtn1.Name), () => YouCan());
                    MainWindow.ButtonsDelSource.Children.Add(newBtn1);
                }
                for (int i = 0; i < NTargets; i++)
                {
                    MyLabel myLabel = new MyLabel();
                    myLabel.Name = "LabelsTarget" + i.ToString();
                    myLabel.Content = "Target " + (i + 1).ToString();
                    MainWindow.LabelsTarget.Children.Add(myLabel);

                    myLabel = new MyLabel();
                    myLabel.Name = "LabelsDrives" + i.ToString();
                    myLabel.Content = "Drives " + (i + 1).ToString();
                    MainWindow.LabelsDrives.Children.Add(myLabel);

                    myLabel = new MyLabel();
                    myLabel.Name = "LabelsExcluded" + i.ToString();
                    myLabel.Content = "Excluded " + (i + 1).ToString();
                    MainWindow.LabelsExcluded.Children.Add(myLabel);

                    MyTextBox myTB = new MyTextBox();
                    myTB.Name = "TBTarget" + i.ToString();
                    myTB.MouseDoubleClick += new MouseButtonEventHandler(TextBoxDoubleClick);
                    Binding binding = new Binding("TxtChanged");
                    binding.Source = viewModel;
                    binding.Mode = BindingMode.OneWayToSource;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(myTB, TextBox.TextProperty, binding);
                    myTB.SetBinding(TextBox.TextProperty, binding);
                    MainWindow.Target.Children.Add(myTB);

                    myTB = new MyTextBox();
                    myTB.Name = "TBDrives" + i.ToString();
                    myTB.MouseDoubleClick += new MouseButtonEventHandler(DrivesBox_MouseDoubleClick);
                    binding = new Binding("TxtChanged");
                    binding.Source = viewModel;
                    binding.Mode = BindingMode.OneWayToSource;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(myTB, TextBox.TextProperty, binding);
                    myTB.SetBinding(TextBox.TextProperty, binding);
                    MainWindow.Drives.Children.Add(myTB);

                    myTB = new MyTextBox();
                    myTB.Name = "TBExcluded" + i.ToString();
                    myTB.MouseDoubleClick += new MouseButtonEventHandler(DrivesBox_MouseDoubleClick);
                    binding = new Binding("TxtChanged");
                    binding.Source = viewModel;
                    binding.Mode = BindingMode.OneWayToSource;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(myTB, TextBox.TextProperty, binding);
                    myTB.SetBinding(TextBox.TextProperty, binding);
                    MainWindow.Excluded.Children.Add(myTB);

                    MyButton newBtn3 = new MyButton();
                    newBtn3.Name = "DelT" + i.ToString();
                    newBtn3.Command = new Command(() => Del(newBtn3.Name), () => YouCan());
                    MainWindow.ButtonsDelTarget.Children.Add(newBtn3);

                    MyButton newBtn2 = new MyButton();
                    newBtn2.Name = "DelD" + i.ToString();
                    newBtn2.Command = new Command(() => Del(newBtn2.Name), () => YouCan());
                    MainWindow.ButtonsDelDrives.Children.Add(newBtn2);

                    MyButton newBtn4 = new MyButton();
                    newBtn4.Name = "DelE" + i.ToString();
                    newBtn4.Command = new Command(() => Del(newBtn4.Name), () => YouCan());
                    MainWindow.ButtonsDelExcluded.Children.Add(newBtn4);
                }
            }
            MainWindow.FillTextBoxes();
        }
        private void TextBoxDoubleClick(object obj, EventArgs e)
        {
            TextBox tb = obj as TextBox;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (ItemDirFile == "Directory")
                {
                    tb.Text = Path.GetDirectoryName(openFileDialog.FileName) + @"\";
                }
                else if (ItemDirFile == "File")
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
        public void ChangeProject()
        {
            PathProject.MakePathList();
            Lists.SaveList2D(PathProject.PathList, Item);
            TBChanged = false;
        }
        public void NewProject()
        {
            List<string> PathNamesListTmp = new List<string>();
            if (!File.Exists(dir + "PathNamesList.bin"))
            {
                PathNamesListTmp.Add(Item);
                Lists.SaveList(PathNamesListTmp, "PathNamesList");
                PathProject.MakePathList();
                Lists.SaveList2D(PathProject.PathList, Item);
            }
            else
            {
                PathNamesListTmp = new List<string>(PathNamesList);
                if (!PathNamesList.Contains(Item))
                {
                    Info = "New project";
                    PathNamesListTmp.Add(Item);
                    Lists.SaveList(PathNamesListTmp, "PathNamesList");
                    PathProject.MakePathList();
                    Lists.SaveList2D(PathProject.PathList, Item);
                }
                else
                {
                    Info = "The project already exists!";
                }
            }
            MainWindow.InitializeCombo(PathNamesList.IndexOf(Item));
            StartControls();
        }
        public void DeleteProject()
        {
            //ButtonMargin = new Thickness(1, -0.5, -0.5, 1);
            //Task.Delay(30).ContinueWith(t => ButtonMargin = new Thickness(0));
            if (File.Exists(dir + Item + ".bin")) { File.Delete(dir + Item + ".bin"); }

            if (PathNamesList.Count > 1)
            {
                List<string> PathNamesListCopy = new List<string>(PathNamesList);
                PathNamesListCopy.Remove(Item);
                Lists.SaveList(PathNamesListCopy, "PathNamesList");
            }
            else if (PathNamesList.Count == 1)
            {
                File.Delete(dir + "PathNamesList.bin");
                MainWindow.InitializeCombo(0);
            }
            MainWindow.InitializeCombo(0);
            StartControls();
        }
        public void OpenSettings()
        {
            if (SettingsVis == Visibility.Collapsed)
            {
                SettingsVis = Visibility.Visible;
                SourceVis = Visibility.Hidden;
            }
            else
            {
                SettingsVis = Visibility.Collapsed;
                SourceVis = Visibility.Visible;
            }
        }
        public async Task RunningString(string message, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                ts.Cancel();
                return;
            }

            int rounds = 0;
            string s = message;
            while (rounds < 40)
            {
                if (rounds % message.Length != 0)
                {
                    if (s[0].ToString() != " " && s[0].ToString() != ".")
                    {
                        s = s.Substring(1, s.Length - 1);
                        await Task.Delay(135);
                    }
                    else
                    {
                        s = s.Substring(1, s.Length - 1);
                        await Task.Delay(100);
                    }
                    Info = s;
                }
                else
                {
                    s = message;
                }
                rounds += 1;
            }
            Info = KillWhitespaces("".PadRight(20) + message);
        }
        public string KillWhitespaces(string line)
        {
            string result = line;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    result = result.Substring(1);
                }
                else
                {
                    break;
                }
            }
            for (int i = line.Length - 1; i > 0; i--)
            {
                if (line[i] == ' ')
                {
                    result = result.Substring(0, result.Length - 1);
                }
                else
                {
                    break;
                }
            }
            return result;
        }
        private void Make_SingleSource_MultipleTarget_Project()
        {
            if (!String.IsNullOrEmpty(Item))
            {
                PathProject.MakePathList();
                StartControls();
            }
            else
            {
                Item = "Please, load project";
            }
        }
        public bool YouCanSave()
        {
            bool a = TBSourceFilled == true ? true : false;
            return a;
        }
        public bool YouCanDeletePath()
        {
            bool a = false;
            if (PathNamesList.Contains(Item) && !String.IsNullOrEmpty(Item) && !String.IsNullOrWhiteSpace(Item))
            {
                a = true;
            }
            return a;
        }
        public bool YouCanChangePath()
        {
            bool a = TBChanged == true ? true : false;
            return a;
        }
        public bool YouCanSaveNewPath()
        {
            bool a = false;
            if (!File.Exists(dir + "PathNamesList.bin"))
            {
                a = true;
            }
            else
            {
                if (!String.IsNullOrEmpty(Item) && !PathNamesList.Contains(Item))
                {
                    a = true;
                }
            }
            return a;
        }
        public bool YouCanClear()
        {
            bool permission = false;
            if (!String.IsNullOrEmpty(Item))
            {
                permission = true;
            }
            return permission;
        }
        public bool YouCanDeleteObject()
        {
            if (ItemSaveDel == "Save")
            {
                Disable = false;
            }
            else if (ItemSaveDel == "Delete")
            {
                Disable = true;
            }
            return Disable;
        }
        private void Del(string name)
        {
            switch (name.Substring(0, 4))
            {
                case "DelC":
                    Item = string.Empty;
                    break;
                case "DelS":
                    foreach (TextBox tb in MainWindow.Source.Children)
                    {
                        if (tb.Name == "TBSource" + name.Substring(4))
                        {
                            tb.Text = string.Empty;
                        }
                    }
                    break;
                case "DelD":
                    foreach (TextBox tb in MainWindow.Drives.Children)
                    {
                        if (tb.Name == "TBDrives" + name.Substring(4))
                        {
                            tb.Text = string.Empty;
                        }
                    }
                    break;
                case "DelT":
                    foreach (TextBox tb in MainWindow.Target.Children)
                    {
                        if (tb.Name == "TBTarget" + name.Substring(4))
                        {
                            tb.Text = string.Empty;
                        }
                    }
                    break;
                case "DelE":
                    foreach (TextBox tb in MainWindow.Excluded.Children)
                    {
                        if (tb.Name == "TBExcluded" + name.Substring(4))
                        {
                            tb.Text = string.Empty;
                        }
                    }
                    break;
            }
        }
        public void ExitApp()
        {
            Application.Current.Shutdown();
        }
        public bool YouCan()
        {
            return true;
        }
        public bool YouCanNot()
        {
            return false;
        }
        private void Max() { }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
