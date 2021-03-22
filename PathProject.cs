using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace Save
{
    public class PathProject : INotifyPropertyChanged
    {
        #region Objects
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel ViewModel { get; set; }
        #endregion

        #region Properties
        public string PathProjectName;
        public List<List<string>> PathList = new List<List<string>>();
        public List<string> TmpList = new List<string>() { "Tmp" };
        public List<List<string>> Tmp = new List<List<string>>()
        {
            new List<string>(){@"C:\Tmp\"},
            new List<string>(){@"\Tmp\"},
            new List<string>(){"D,F,G"},
            new List<string>(){"A,B"}
        };
        #endregion
        public PathProject(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public void MakePathList()
        {
            ViewModel.NSources = ViewModel.MainWindow.Source.Children.OfType<TextBox>().Count();
            PathList = new List<List<string>>();
            var SourceListTmp = new List<string>();
            var TargetListTmp = new List<string>();
            var DrivesListTmp = new List<string>();
            var ExcludedListTmp = new List<string>();
            if (ViewModel.MultipleTargets == false)
            {
                for (int i = 0; i < ViewModel.NSources; i++)
                {
                    foreach (TextBox tbs in ViewModel.MainWindow.Source.Children)
                    {
                        if (tbs.Name == "TBSource" + i.ToString() && !String.IsNullOrEmpty(tbs.Text))
                        {
                            SourceListTmp.Add(tbs.Text);
                        }
                    }
                }
                for (int i = 0; i < ViewModel.NTargets; i++)
                {
                    foreach (TextBox tbd in ViewModel.MainWindow.Drives.Children)
                    {
                        if (tbd.Name == "TBDrives" + i.ToString())
                        {
                            DrivesListTmp.Add(tbd.Text);
                        }
                    }
                    foreach (TextBox tbt in ViewModel.MainWindow.Target.Children)
                    {
                        if (tbt.Name == "TBTarget" + i.ToString())
                        {
                            TargetListTmp.Add(tbt.Text);
                        }
                    }
                    foreach (TextBox tbe in ViewModel.MainWindow.Excluded.Children)
                    {
                        if (tbe.Name == "TBExcluded" + i.ToString())
                        {
                            ExcludedListTmp.Add(tbe.Text);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    foreach (TextBox tbs in ViewModel.MainWindow.Source.Children)
                    {
                        if (tbs.Name == "TBSource" + i.ToString() && !String.IsNullOrEmpty(tbs.Text))
                        {
                            SourceListTmp.Add(tbs.Text);
                        }
                    }
                }
                for (int i = 0; i < ViewModel.NTargets; i++)
                {
                    foreach (TextBox tbt in ViewModel.MainWindow.Target.Children)
                    {
                        if (tbt.Name == "TBTarget" + i.ToString())
                        {
                            TargetListTmp.Add(tbt.Text);
                        }
                    }
                    foreach (TextBox tbd in ViewModel.MainWindow.Drives.Children)
                    {
                        if (tbd.Name == "TBDrives" + i.ToString())
                        {
                            DrivesListTmp.Add(tbd.Text);
                        }
                    }
                    foreach (TextBox tbe in ViewModel.MainWindow.Excluded.Children)
                    {
                        if (tbe.Name == "TBExcluded" + i.ToString())
                        {
                            ExcludedListTmp.Add(tbe.Text);
                        }
                    }
                }
            }
            PathList.Add(SourceListTmp);
            PathList.Add(TargetListTmp);
            PathList.Add(DrivesListTmp);
            PathList.Add(ExcludedListTmp);
        }
        public void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}