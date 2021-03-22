using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Save
{
    public class Target : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel ViewModel { get; set; }
        public List<string> TargetDirList { get; set; }
        private string targetDir { get; set; } = "";
        public string TargetDir
        {
            get
            {
                return targetDir as string;
            }
            set
            {
                if (targetDir != value)
                {
                    targetDir = value;
                    OnPropertyChange(nameof(TargetDir));
                }
            }
        }
        public void MakeTargetPathList()
        {
            //######## Filling TargetDirList #########################################################

            TargetDir = string.Empty;
            TargetDirList = new List<string>();

            for (int i = 0; i < ViewModel.PathProject.PathList[1].Count; i++)
            {
                //##### Removing drive symbol and checking backslashes at both ends of the dir ################################
                if (!String.IsNullOrEmpty(ViewModel.PathProject.PathList[1][i]) && !String.IsNullOrWhiteSpace(ViewModel.PathProject.PathList[1][i]))
                {
                    if (ViewModel.PathProject.PathList[1][i].Contains(":"))
                    {
                        ViewModel.PathProject.PathList[1][i] = ViewModel.PathProject.PathList[1][i].Split(':')[1];
                    }
                    if (ViewModel.PathProject.PathList[1][i][ViewModel.PathProject.PathList[1][i].Length - 1] != '\\')
                    {
                        ViewModel.PathProject.PathList[1][i] += @"\";
                    }
                    if (ViewModel.PathProject.PathList[1][i][0] != '\\')
                    {
                        ViewModel.PathProject.PathList[1][i] = @"\" + ViewModel.PathProject.PathList[1][i];
                    }
                    TargetDirList.Add(ViewModel.PathProject.PathList[1][i]);
                }
                else if (String.IsNullOrEmpty(ViewModel.PathProject.PathList[1][i]) || String.IsNullOrWhiteSpace(ViewModel.PathProject.PathList[1][i]))
                {
                    if (!String.IsNullOrEmpty(ViewModel.Source.SourcePathList[i]))
                    {
                        if (ViewModel.Source.SourcePathList[i].Contains(":"))
                        {
                            ViewModel.PathProject.PathList[1][i] = ViewModel.Source.SourcePathList[i].Split(':')[1];
                        }
                        else
                        {
                            ViewModel.PathProject.PathList[1][i] = ViewModel.Source.SourcePathList[i];
                        }
                        var splitlist = new List<string>(SplitString(ViewModel.PathProject.PathList[1][i], '\\'));

                        if (ViewModel.Source.fileOrNot[i] == true)
                        {
                            splitlist.RemoveAt(splitlist.Count - 1);
                        }
                        if (splitlist[0] == "Home")
                        {
                            splitlist.RemoveAt(0);
                        }
                        if (splitlist.Count == 0)
                        {
                            ViewModel.PathProject.PathList[1][i] = @"\";
                            ViewModel.Purge = false;
                        }
                        else
                        {
                            ViewModel.PathProject.PathList[1][i] = @"\" + splitlist.Aggregate((string a, string b) => a + @"\" + b) + @"\";
                        }
                        if (ViewModel.PathProject.PathList[1][i][ViewModel.PathProject.PathList[1][i].Length - 1] != '\\')
                        {
                            ViewModel.PathProject.PathList[1][i] += @"\";
                        }
                        if (ViewModel.PathProject.PathList[1][i][0] != '\\')
                        {
                            ViewModel.PathProject.PathList[1][i] = @"\" + ViewModel.PathProject.PathList[1][i];
                        }

                        //##### Removing white spaces ################################
                        if (ViewModel.PathProject.PathList[1][i].Contains(" "))
                        {
                            ViewModel.PathProject.PathList[1][i] = ViewModel.PathProject.PathList[1][i].Replace(" ", "_");
                        }
                        TargetDirList.Add(ViewModel.PathProject.PathList[1][i]);
                    }
                    else
                    {
                        TargetDirList.Add(string.Empty);
                    }
                }
            }
            Debug.WriteLine("TargetDirList.Count [Target]: " + TargetDirList.Count);
        }
        public void MakeTargetWarningMessage(string message, int ind)
        {
            foreach (TextBox tbt in ViewModel.MainWindow.Source.Children)
            {
                if (tbt.Name == "TBTarget" + ind.ToString())
                {
                    tbt.Text = message;
                }
            }
        }
        public List<string> SplitString(string str, char ch)
        {
            List<string> splitlist = new List<string>();
            string[] split1 = str.Split(ch);
            foreach (string spl in split1)
            {
                if (!String.IsNullOrEmpty(spl) && !String.IsNullOrWhiteSpace(spl) && spl != "\n")
                {
                    splitlist.Add(spl);
                }
            }
            return splitlist;
        }
        public Target(ViewModel viewModel)
        {
            ViewModel = viewModel;
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
