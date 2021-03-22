using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Save
{
    public class SaveObject
    {
        #region Objects
        //public static CancellationTokenSource ts = new CancellationTokenSource();
        //public CancellationToken ct = ts.Token;
        #endregion

        #region Properties
        public int countTest = 0;
        public ViewModel ViewModel { get; set; }
        public int index = 0;
        #endregion
        public SaveObject(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public void Save()
        {
            ViewModel.TBSourceFilled = false;
            for (int i = 0; i < ViewModel.NSources; i++)
            {
                foreach (TextBox tbs in ViewModel.MainWindow.Source.Children)
                {
                    if (tbs.Name == "TBSource" + i.ToString() && !String.IsNullOrEmpty(tbs.Text))
                    {
                        ViewModel.TBSourceFilled = true;
                    }
                }
            }
            if (ViewModel.TBSourceFilled == true)
            {
                ViewModel.Info = string.Empty;
                ViewModel.Lists.MakeLists();
                if (countTest == ViewModel.Source.SourcePathList.Count)
                {
                    ViewModel.Info = "No source!";
                    return;
                }

                //########Single target################################################################################################################################################
                if (ViewModel.MultipleTargets == false)
                {
                    //########Scan through source paths#########################################################
                    for (int i = 0; i < ViewModel.Source.SourcePathList.Count; i++)
                    {
                        if (ViewModel.Source.SourcePathList[i] != "Source doesn't exist!"
                            && ViewModel.Source.SourcePathList[i] != "Remove the path without spaces!")
                        {
                            ViewModel.Source.SourcePath = ViewModel.Source.SourcePathList[i];
                            ViewModel.Source.fileNotDir = ViewModel.Source.fileOrNot[i];
                            //########White spaces#########################################################
                            if (ViewModel.Source.SourcePath.Contains(" "))
                            {
                                if (ViewModel.CorrectPath == true)
                                {
                                    if (ViewModel.Source.SourceWS[i] == true)
                                    {
                                        ViewModel.Source.RemoveWhitespacesFromSourcePath();
                                    }
                                }
                                else
                                {
                                    ViewModel.Info = "Click \"Correct path\"!";
                                    return;
                                }
                            }


                            MakeFileName();
                            SaveSingleTarget(i);
                        }
                    }
                }
                //######## Single source and multiple targets ################################################################################################################################################
                else
                {
                    if (ViewModel.Source.SourcePathList[0] != "Source doesn't exist!"
                        && ViewModel.Source.SourcePathList[0] != "Remove the path without spaces!")
                    {
                        ViewModel.Source.SourcePath = ViewModel.Source.SourcePathList[0];
                        ViewModel.Source.fileNotDir = ViewModel.Source.fileOrNot[0];
                        //########White spaces#########################################################
                        if (ViewModel.Source.SourcePath.Contains(" ") && ViewModel.CorrectPath == true)
                        {
                            if (ViewModel.Source.SourceWS[0] == true)
                            {
                                ViewModel.Source.RemoveWhitespacesFromSourcePath();
                            }
                        }
                        MakeFileName();
                        SaveMultipleTargets();
                    }
                }
                string message = "".PadRight(40) + "...End...";
                ViewModel.RunningString(message, ViewModel.ct);
                if (ViewModel.AutomaticExit == true)
                {
                    ViewModel.ExitApp();
                }
            }
        }
        public void SaveSingleTarget(int index)
        {
            //######## Drives #########################################################
            ViewModel.Drives.IndividualDrivesList = new List<string>(ViewModel.Drives.DrivesList[index].Split(','));
            for (int i = 0; i < ViewModel.Drives.IndividualDrivesList.Count; i++)
            {
                if (!String.IsNullOrEmpty(ViewModel.Drives.IndividualDrivesList[i]) && !String.IsNullOrWhiteSpace(ViewModel.Drives.IndividualDrivesList[i]))
                {
                    //######## Drive exists (?) #########################################################
                    if (Directory.Exists(ViewModel.Drives.IndividualDrivesList[i] + @":\"))
                    {
                        ViewModel.Target.TargetDir = ViewModel.Drives.IndividualDrivesList[i] + ":" + ViewModel.Target.TargetDirList[index];

                        //######## Checks the target path for Home directory and removes white spaces ######
                        TargetCheck(ViewModel.Target.TargetDir, index);

                        //######## Saves the source object to the target path################################
                        string emptySubfolders = ViewModel.Source.fileOrNot[index] ? "" : " /E";
                        string threads = ViewModel.Threads ? " /MT[:5]" : "";
                        string purge = ViewModel.Source.fileOrNot[index] ? "" : (ViewModel.Purge ? " /PURGE" : "");
                        SaveRobocopy(emptySubfolders, purge, threads);
                    }
                }
            }
        }
        public void SaveMultipleTargets()
        {
            //######## Targets #########################################################
            for (int i = 0; i < ViewModel.Target.TargetDirList.Count; i++)
            {
                if (!String.IsNullOrEmpty(ViewModel.Target.TargetDirList[i]))
                {
                    //######## Drives #########################################################
                    ViewModel.Drives.IndividualDrivesList = new List<string>(ViewModel.Drives.DrivesList[i].Split(','));
                    for (int j = 0; j < ViewModel.Drives.IndividualDrivesList.Count; j++)
                    {
                        if (!String.IsNullOrEmpty(ViewModel.Drives.IndividualDrivesList[j]) && !String.IsNullOrWhiteSpace(ViewModel.Drives.IndividualDrivesList[j]))
                        {
                            //######## Drive exists (?) #########################################################
                            if (Directory.Exists(ViewModel.Drives.IndividualDrivesList[j] + @":\"))
                            {
                                if (ViewModel.Target.TargetDirList[i].Contains(':'))
                                {
                                    ViewModel.Target.TargetDir = ViewModel.Drives.IndividualDrivesList[j] + ":" + ViewModel.Target.TargetDirList[i].Split(':')[1];
                                }
                                else
                                {
                                    ViewModel.Target.TargetDir = ViewModel.Drives.IndividualDrivesList[j] + ":" + ViewModel.Target.TargetDirList[i];
                                }
                                //######## Checks the target path for home directory and removes white spaces #######
                                TargetCheck(ViewModel.Target.TargetDir, i);
                                //######## Saves the source object to the target path################################
                                string purge = string.Empty;
                                string threads = string.Empty;
                                string emptySubfolders = string.Empty;
                                emptySubfolders = ViewModel.Source.fileOrNot[index] ? "" : " /E";
                                threads = ViewModel.Threads ? " /MT[:5]" : "";                                                       //Threading the process
                                purge = ViewModel.Source.fileOrNot[index] ? "" : (ViewModel.Purge ? " /PURGE" : "");
                                SaveRobocopy(emptySubfolders, purge, threads);
                            }
                        }
                    }
                }
            }
        }
        public void TargetCheck(string targetDir, int index)
        {
            //######## Creates the path with home directory ###################################################
            if (Directory.Exists(targetDir.Substring(0, 3) + ViewModel.HomeDir + @"\"))
            {
                if (targetDir.Split('\\')[1] != ViewModel.HomeDir)
                {
                    targetDir = targetDir.Substring(0, 3) + ViewModel.HomeDir + @"\" + targetDir.Substring(3);
                }
            }
            /*######## Checks whether the target path exists and if it has white spaces. If so, checks whether the path with replaced white spaces ################################
            ########## also exists (in this case the app stops and gives warning). If the path with replaced white spaces does not exist, replaces ################################
            ########## white spaces through "_" */
            if (Directory.Exists(targetDir) || File.Exists(targetDir))
            {
                if (targetDir.Contains(" ") && ViewModel.CorrectPath == true)
                {
                    if (Directory.Exists(targetDir.Replace(" ", "_")) || File.Exists(targetDir.Replace(" ", "_")))
                    {
                        string message = "Target without spaces exists! Remove!";
                        ViewModel.Target.MakeTargetWarningMessage(message, index);
                    }
                    RemoveWhitespacesFromTargetPath(targetDir);
                }
                else if (targetDir.Contains(" ") && ViewModel.CorrectPath == false)
                {
                    string message = "Target contains white spaces!";
                    ViewModel.Target.MakeTargetWarningMessage(message, index);
                }
            }
            else if (!Directory.Exists(targetDir) && !File.Exists(targetDir))
            {
                if (Directory.Exists(targetDir.Replace(" ", "_")) || File.Exists(targetDir.Replace(" ", "_")))                                                                    //Checks whether the drive exists
                {
                    string message = "Target without spaces exists! Remove!";
                    ViewModel.Target.MakeTargetWarningMessage(message, index);
                }
                RemoveWhitespacesFromTargetPath(targetDir);
            }
            ViewModel.Target.TargetDir = targetDir;
        }
        public void SaveRobocopy(string emptySubfolders, string purge, string threads)
        {
            if (ViewModel.LocalRobocopy == true)
            {
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.FileName = "Robocopy.exe";
                proc.StartInfo.Arguments = ViewModel.Source.SourcePath + " " + ViewModel.Target.TargetDir + ViewModel.Source.FileName + emptySubfolders + purge + " /R:50 /W:5 /J " + threads + " /NP";
                proc.Start();
                if (ViewModel.Parallel == false) { proc.WaitForExit(); }
            }
            else
            {
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "ROBOCOPY.exe");
                proc.StartInfo.Arguments = ViewModel.Source.SourcePath + " " + ViewModel.Target.TargetDir + ViewModel.Source.FileName + emptySubfolders + purge + " /R:50 /W:5 /J " + threads + " /NP";
                proc.Start();
                if (ViewModel.Parallel == false) { proc.WaitForExit(); }
            }
        }
        public void MakeFileName()
        {
            if (ViewModel.Source.fileNotDir == true)
            {
                List<string> list = new List<string>(ViewModel.Source.SourcePath.Split('\\'));
                ViewModel.Source.FileName = " " + list[list.Count - 1];
                list.Remove(list[list.Count - 1]);
                ViewModel.Source.SourcePath = list.Aggregate((string a, string b) => a + @"\" + b) + @"\";
            }
            else
            {
                ViewModel.Source.FileName = "";
            }
        }
        public string RemoveWhitespacesFromTargetPath(string path)
        {
            string pathOld = string.Empty;
            string pathNew = string.Empty;
            string str1 = string.Empty;
            List<string> split = new List<string>(path.Split('\\'));
            foreach (string str in split)
            {
                if (!String.IsNullOrEmpty(str) && !String.IsNullOrWhiteSpace(str))
                {
                    pathOld = pathNew;
                    if (!str.Contains(" "))
                    {
                        pathOld += str + @"\";
                        pathNew += str + @"\";
                    }
                    else if (str.Contains(" "))
                    {
                        pathOld += str + @"\";
                        pathNew += str.Replace(" ", "_") + @"\";
                    }
                    if (pathNew != pathOld)
                    {
                        Directory.Move(pathOld, pathNew);
                        path = pathNew;
                    }
                }
            }
            return path;
        }
        public bool FileOrNot(string path)
        {
            bool IsFile = false;
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    IsFile = false;
                    if (path[path.Length - 1] != '\\')
                    {
                        path = path + @"\";
                    }
                }
                else if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    IsFile = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is DirectoryNotFoundException || ex is FileNotFoundException)
                {
                    ViewModel.Info = "Source path does not exist!";
                    return false;
                }
                throw;
            }
            return IsFile;
        }

    }
}