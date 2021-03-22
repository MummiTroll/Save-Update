using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace Save
{
    public class Source : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel ViewModel { get; set; }
        public List<string> SourcePathList { get; set; }
        public List<bool> fileOrNot { get; set; }
        public List<bool> SourceWS { get; set; }
        private string sourcePath { get; set; } = "";
        public string SourcePath
        {
            get
            {
                return sourcePath as string;
            }
            set
            {
                if (sourcePath != value)
                {
                    sourcePath = value;
                    OnPropertyChange(nameof(SourcePath));
                }
            }
        }
        public bool fileNotDir { get; set; }
        private string fileName { get; set; } = "";
        public string FileName
        {
            get
            {
                return fileName as string;
            }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                }
            }
        }
        #endregion
        public void MakeSourcePathList()
        {
            if (String.IsNullOrEmpty(ViewModel.Item) || String.IsNullOrWhiteSpace(ViewModel.Item))
            {
                ViewModel.Item = "Forgot to type the project name?";
            }
            SourcePathList = new List<string>();
            SourcePath = string.Empty;
            fileOrNot = new List<bool>();
            SourceWS = new List<bool>();
            fileNotDir = false;
            FileName = string.Empty;

            //######## Filling SourcePathList, fileOrNot and  SourceWS lists #########################################################
            for (int i = 0; i < ViewModel.PathProject.PathList[0].Count; i++)
            {
                SourcePathList.Add((!String.IsNullOrEmpty(ViewModel.PathProject.PathList[0][i])
                    && !String.IsNullOrWhiteSpace(ViewModel.PathProject.PathList[0][i])) ? ViewModel.PathProject.PathList[0][i] : string.Empty);

                //######## Do source paths exist?#########################################################
                if (File.Exists(ViewModel.PathProject.PathList[0][i]) || Directory.Exists(ViewModel.PathProject.PathList[0][i]))
                {
                    fileOrNot.Add(ViewModel.SaveObject.FileOrNot(SourcePathList[i]));
                }
                else if (!File.Exists(ViewModel.PathProject.PathList[0][i]) && !Directory.Exists(ViewModel.PathProject.PathList[0][i]))
                {
                    fileOrNot.Add(false);
                    MakeSourceWarningMessage("Source doesn't exist!", i);
                    SourcePathList[i] = "Source doesn't exist!";
                }

                //######## Do source paths contain white spaces?#########################################################
                if (ViewModel.PathProject.PathList[0][i].Contains(" "))
                {
                    if (Directory.Exists(ViewModel.PathProject.PathList[0][i].Replace(" ", "_")) || File.Exists(ViewModel.PathProject.PathList[0][i].Replace(" ", "_")))
                    {
                        SourceWS.Add(false);
                        MakeSourceWarningMessage("Remove the path without spaces!", i);
                    }
                    else if (!Directory.Exists(ViewModel.PathProject.PathList[0][i].Replace(" ", "_")) && !File.Exists(ViewModel.PathProject.PathList[0][i].Replace(" ", "_")))
                    {
                        SourceWS.Add(true);
                    }
                }
                else if (!ViewModel.PathProject.PathList[0][i].Contains(" "))
                {
                    SourceWS.Add(true);
                }
            }
        }
        public void RemoveWhitespacesFromSourcePath()
        {
            List<string> split = new List<string>(ViewModel.Source.SourcePath.Split('\\'));
            string pathOld = string.Empty;
            string pathNew = string.Empty;
            if (ViewModel.Source.fileNotDir == true)
            {
                for (int i = 0; i < split.Count - 1; i++)
                {
                    pathOld = pathNew;
                    if (!String.IsNullOrEmpty(split[i]) && !String.IsNullOrWhiteSpace(split[i]))
                    {
                        if (!split[i].Contains(" "))
                        {
                            pathOld += split[i] + "\\";
                            pathNew += split[i] + "\\";
                        }
                        else
                        {
                            pathOld += split[i] + "\\";
                            pathNew += split[i].Replace(" ", "_") + "\\";
                        }
                        if (pathNew != pathOld)
                        {
                            Directory.Move(pathOld, pathNew);
                        }
                    }
                }
                pathOld = pathNew;
                pathOld += split[split.Count - 1];
                pathNew += split[split.Count - 1].Replace(" ", "_");
                File.Move(pathOld, pathNew);
            }
            else
            {
                foreach (string str in split)
                {
                    pathOld = pathNew;

                    if (!String.IsNullOrEmpty(str) && !String.IsNullOrWhiteSpace(str))
                    {
                        if (!str.Contains(" "))
                        {
                            pathOld += str + "\\";
                            pathNew += str + "\\";
                        }
                        else
                        {
                            pathOld += str + "\\";
                            pathNew += str.Replace(" ", "_") + "\\";
                        }
                        if (pathNew != pathOld)
                        {
                            Directory.Move(pathOld, pathNew);
                        }
                    }
                }
            }
            ViewModel.Source.SourcePath = pathNew;
        }
        public void MakeSourceWarningMessage(string message, int ind)
        {
            foreach (TextBox tbs in ViewModel.MainWindow.Source.Children)
            {
                if (tbs.Name == "TBSource" + ind.ToString())
                {
                    tbs.Text = message;
                }
            }
        }
        public Source(ViewModel viewModel)
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
