using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Save
{
    public class DeleteObject
    {
        #region Objects
        public ViewModel ViewModel { get; set; }
        //public static CancellationTokenSource ts = new CancellationTokenSource();
        //public CancellationToken ct = ts.Token;
        #endregion

        public DeleteObject(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public async Task Delete_Object()
        {
            ViewModel.Info = string.Empty;
            if (ViewModel.ItemSaveDel == "Delete")
            {
                if (ViewModel.DelObject < 1)
                {
                    ViewModel.DelObject += 1;
                    ViewModel.Info = "Really delete?";
                    return;
                }
                else if (ViewModel.DelObject == 1)
                {
                    ViewModel.PathProject.MakePathList();
                    ViewModel.Source.MakeSourcePathList();
                    ViewModel.Drives.MakeDrivesListForDel();

                    //For all sources
                    for (int i = 0; i < ViewModel.Source.SourcePathList.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(ViewModel.Source.SourcePathList[i]) && !String.IsNullOrWhiteSpace(ViewModel.Source.SourcePathList[i]))
                        {
                            ViewModel.Source.SourcePath = ViewModel.Source.SourcePathList[i];
                            ViewModel.Drives.IndividualDrivesList = new List<string>(ViewModel.Drives.DrivesList[i].Split(','));
                            //For each drive
                            for (int j = 0; j < ViewModel.Drives.IndividualDrivesList.Count; j++)
                            {
                                if (!String.IsNullOrEmpty(ViewModel.Drives.IndividualDrivesList[j]) && !String.IsNullOrWhiteSpace(ViewModel.Drives.IndividualDrivesList[j]))
                                {
                                    if (ViewModel.Source.SourcePath.Contains(":"))
                                    {
                                        ViewModel.Source.SourcePath = ViewModel.Drives.IndividualDrivesList[j] + ":" + ViewModel.Source.SourcePath.Split(':')[1];                              //Adding drive letter
                                    }
                                    else if (!ViewModel.Source.SourcePath.Contains(":"))
                                    {
                                        if (ViewModel.Source.SourcePath[0] != '\\')
                                        {
                                            ViewModel.Source.SourcePath = ViewModel.Drives.IndividualDrivesList[j] + @":\" + ViewModel.Source.SourcePath;
                                        }
                                        else
                                        {
                                            ViewModel.Source.SourcePath = ViewModel.Drives.IndividualDrivesList[j] + ViewModel.Source.SourcePath;
                                        }
                                    }
                                }
                                string SourcePathHome = ViewModel.Source.SourcePath.Substring(0, 3) + ViewModel.HomeDir + @"\" + ViewModel.Source.SourcePath.Substring(3);
                                if (ViewModel.Source.fileOrNot[i] == false && Directory.Exists(ViewModel.Drives.IndividualDrivesList[j] + @":\")                                         //Delete directory
                                    && (Directory.Exists(ViewModel.Source.SourcePath) || Directory.Exists(SourcePathHome)))
                                {
                                    if (Directory.Exists(ViewModel.Drives.IndividualDrivesList[j] + @":\" + ViewModel.HomeDir + @"\"))
                                    {
                                        if (ViewModel.Source.SourcePath.Split('\\')[1] != ViewModel.HomeDir)
                                        {
                                            ViewModel.Source.SourcePath = SourcePathHome;
                                        }
                                    }
                                    Directory.Delete(ViewModel.Source.SourcePath, true);
                                    await Task.Delay(1000);
                                }
                                else if (ViewModel.Source.fileOrNot[i] == true && Directory.Exists(ViewModel.Drives.IndividualDrivesList[j] + @":\")                                    //Delete file
                                    && (File.Exists(ViewModel.Source.SourcePath) || File.Exists(SourcePathHome)))
                                {
                                    if (Directory.Exists(ViewModel.Drives.IndividualDrivesList[j] + @":\" + ViewModel.HomeDir + @"\"))
                                    {
                                        if (ViewModel.Source.SourcePath.Split('\\')[1] != ViewModel.HomeDir)
                                        {
                                            ViewModel.Source.SourcePath = SourcePathHome;
                                        }
                                    }
                                    File.Delete(ViewModel.Source.SourcePath);
                                    await Task.Delay(100);
                                }
                            }
                        }
                    }
                }
                ViewModel.ItemSaveDel = ViewModel.sd[0];
                ViewModel.Info = "Deleted";
                await Task.Delay(1000);
                ViewModel.DelObject = 0;
            }
            string message = "".PadRight(40) + "...End...";
            ViewModel.RunningString(message, ViewModel.ct);
        }
        public bool YouCanDeleteObject()
        {
            bool a = false;
            if (ViewModel.ItemSaveDel == "Delete")
            {
                a = true;
            }
            else if (ViewModel.ItemSaveDel == "Save")
            {
                a = false;
            }
            return a;
        }
    }
}