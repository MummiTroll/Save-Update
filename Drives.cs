using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Save
{
    public class Drives : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel ViewModel { get; set; }
        public List<string> DrivesList { get; set; }
        public List<string> IndividualDrivesList { get; set; }
        public List<string> DrivesSplitList { get; set; }
        public List<string> ExcludedDrivesSplitList { get; set; }
        #endregion
        public Drives(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public void MakeDrivesList()
        {
            DrivesList = new List<string>();

            //##### Consolidating Drives and DrivesExcluded, changing to upper case ##############################
            for (int i = 0; i < ViewModel.Target.TargetDirList.Count; i++)
            {
                if (!String.IsNullOrEmpty(ViewModel.Target.TargetDirList[i]) && !String.IsNullOrWhiteSpace(ViewModel.Target.TargetDirList[i]))
                {
                    DrivesList.Add(ExcludeDrivesForTarget(ViewModel.PathProject.PathList[2][i], ViewModel.PathProject.PathList[3][i], i));
                }
            }
        }
        public void MakeDrivesListForDel()
        {
            DrivesList = new List<string>();

            //##### Consolidating Drives and DrivesExcluded, changing to upper case ##############################
            for (int i = 0; i < ViewModel.Source.SourcePathList.Count; i++)
            {
                if (!String.IsNullOrEmpty(ViewModel.Source.SourcePathList[i]) && !String.IsNullOrWhiteSpace(ViewModel.Source.SourcePathList[i]))
                {
                    DrivesList.Add(ExcludeDrivesForTarget(ViewModel.PathProject.PathList[2][i], ViewModel.PathProject.PathList[3][i], i));
                }
            }
        }
        public string ExcludeDrivesForTarget(string targetDrives, string excludedDrives, int index)
        {
            string drives = string.Empty; ;
            DrivesSplitList = new List<string>(targetDrives.Split(','));
            ExcludedDrivesSplitList = new List<string>(excludedDrives.Split(','));

            if (!String.IsNullOrWhiteSpace(targetDrives) && !String.IsNullOrEmpty(targetDrives) &&
                !String.IsNullOrWhiteSpace(excludedDrives) && !String.IsNullOrEmpty(excludedDrives))
            {
                foreach (string excluded in ExcludedDrivesSplitList)
                {
                    if (DrivesSplitList.Contains(excluded))
                    {
                        DrivesSplitList.Remove(excluded);
                    }
                }
                RemoveDriveFromSource(index);
            }
            drives += DrivesSplitList.Aggregate((string a, string b) => a.ToUpper() + "," + b.ToUpper());
            return drives;
        }
        private void RemoveDriveFromSource(int index)
        {
            if (ViewModel.MultipleTargets == false)
            {
                if (ViewModel.Source.fileOrNot[index] == true)
                {
                    var a = new List<string>(SplitString(ViewModel.Source.SourcePathList[index], '\\'));
                    a.Remove(a[a.Count - 1]);
                    string b = "\\" + a.Aggregate((string c, string d) => c + "\\" + d) + "\\";
                    if (DrivesSplitList.Contains(ViewModel.Source.SourcePathList[index][0].ToString()) && b.Split(':')[1] == ViewModel.Target.TargetDirList[index])
                    {
                        DrivesSplitList.Remove(ViewModel.Source.SourcePathList[index][0].ToString());
                    }
                }
                else
                {
                    if (DrivesSplitList.Contains(ViewModel.Source.SourcePathList[index][0].ToString()) && ViewModel.Source.SourcePathList[index].Split(':')[1] == ViewModel.Target.TargetDirList[index])
                    {
                        DrivesSplitList.Remove(ViewModel.Source.SourcePathList[index][0].ToString());
                    }
                }
            }
            else
            {
                if (ViewModel.Source.fileOrNot[0] == true)
                {
                    var a = new List<string>(SplitString(ViewModel.Source.SourcePathList[0], '\\'));
                    a.Remove(a[a.Count - 1]);
                    string b = "\\" + a.Aggregate((string c, string d) => c + "\\" + d) + "\\";
                    if (DrivesSplitList.Contains(ViewModel.Source.SourcePathList[0][0].ToString()) && b.Split(':')[1] == ViewModel.Target.TargetDirList[index])
                    {
                        DrivesSplitList.Remove(ViewModel.Source.SourcePathList[0][0].ToString());
                    }
                }
                else
                {
                    if (DrivesSplitList.Contains(ViewModel.Source.SourcePathList[0][0].ToString()) && ViewModel.Source.SourcePathList[0].Split(':')[1] == ViewModel.Target.TargetDirList[index])
                    {
                        DrivesSplitList.Remove(ViewModel.Source.SourcePathList[0][0].ToString());
                    }
                }
            }
        }
        public List<string> SplitString(string str, char ch)
        {
            List<string> splitlist = new List<string>();
            string[] split1 = str.Split(ch);
            foreach (string spl in split1)
            {
                if (spl != "\n" && !String.IsNullOrEmpty(spl) && !String.IsNullOrWhiteSpace(spl))
                {
                    splitlist.Add(spl);
                }
            }
            return splitlist;
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