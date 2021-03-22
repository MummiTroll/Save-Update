using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save
{
    class Garbage
    {
        void AA(List<bool> list, string listName, string procedureName)
        {
            int count = 0;
            foreach (bool s in list)
            {
                count += 1;
                Debug.WriteLine("######################List: " + listName + " in procedure: " + procedureName + "######################");
                Debug.WriteLine("[" + count + "], value: " + s);
            }
        }
        void BB(List<string> list, string listName, string procedureName)
        {
            int count = 0;
            foreach (string s in list)
            {
                count += 1;
                Debug.WriteLine("######################List: " + listName + " in procedure: " + procedureName + "######################");
                Debug.WriteLine("[" + count + "], value: " + s);
            }
        }

        //        public List<Paths> PathsList = new List<Paths>();
        //private List<string> PrepareDrives(string drivesExcluded, string drives)
        //{
        //    if (!String.IsNullOrEmpty(drivesExcluded) && !String.IsNullOrEmpty(drives))
        //    {
        //        Paths.DriveString = SaveObject.ExcludeDrivesForTarget(drivesExcluded, drives);
        //    }
        //    List<string> splitlist = new List<string>(Paths.DriveString.Split(',').ToList());
        //    return splitlist;
        //}

        //        private void MakePathsList()
        //        {
        //            //########################################################################################################
        //            List<string> drives = new List<string>(PrepareDrives(DrivesExcluded1, Drives1));
        //            Paths.SourcePath = Source1;
        //            if (!String.IsNullOrEmpty(Paths.SourcePath) && !String.IsNullOrWhiteSpace(Paths.SourcePath))
        //            {
        //                Paths.fileNotDir = SaveObject.FileOrNot(Paths.SourcePath);
        //                if (Paths.fileNotDir == true)
        //                {
        //                    SaveObject.MakeFileName();
        //                }
        //                Paths.SourcePath = Paths.SourcePath.Split(':')[1];
        //                for (int i = 0; i < drives.Count(); i++)
        //                {
        //                    var paths1 = new Paths(this);
        //                    Paths.SourcePath = drives[i] + ":" + Paths.SourcePath;
        //                    PathsList.Add(paths1);
        //                }
        //            }
        //            //########################################################################################################
        //            drives = new List<string>(PrepareDrives(DrivesExcluded2, Drives2));
        //            Paths.SourcePath = Source2;
        //            if (!String.IsNullOrEmpty(Paths.SourcePath) && !String.IsNullOrWhiteSpace(Paths.SourcePath))
        //            {
        //                Paths.fileNotDir = SaveObject.FileOrNot(Paths.SourcePath);
        //                if (Paths.fileNotDir == true)
        //                {
        //                    SaveObject.MakeFileName();
        //                }
        //                Paths.SourcePath = Paths.SourcePath.Split(':')[1];

        //                for (int i = 0; i < drives.Count(); i++)
        //                {
        //                    var paths2 = new Paths(this);
        //                    Paths.SourcePath = drives[i] + ":" + Paths.SourcePath;
        //                    PathsList.Add(paths2);
        //                }
        //            }
        //            //########################################################################################################
        //            drives = new List<string>(PrepareDrives(DrivesExcluded3, Drives3));
        //            Paths.SourcePath = Source3;
        //            if (!String.IsNullOrEmpty(Paths.SourcePath) && !String.IsNullOrWhiteSpace(Paths.SourcePath))
        //            {
        //                Paths.fileNotDir = SaveObject.FileOrNot(Paths.SourcePath);
        //                if (Paths.fileNotDir == true)
        //                {
        //                    SaveObject.MakeFileName();
        //                }
        //                Paths.SourcePath = Paths.SourcePath.Split(':')[1];
        //                for (int i = 0; i < drives.Count(); i++)
        //                {
        //                    var paths3 = new Paths(this);
        //                    Paths.SourcePath = drives[i] + ":" + Paths.SourcePath;
        //                    PathsList.Add(paths3);
        //                }
        //            }
        //            //########################################################################################################
        //        }
        //        public void DeleteObject()
        //        {
        //            if (ItemSaveDel == "Delete")
        //            {
        //                if (Info != "Really delete?")
        //                {
        //                    Info = "Really delete?";
        //                    return;
        //                }
        //                if (Info == "Really delete?")
        //                {
        //                    MakePathsList();
        //                    for (int i = 0; i < PathsList.Count; i++)
        //                    {
        //                        //Debug.WriteLine("Source1: " + Source1);
        //                        //Debug.WriteLine("Drives1: " + Drives1);
        //                        //Debug.WriteLine("ExcludedDrives1: " + DrivesExcluded1);
        //                        //Debug.WriteLine("Source2: " + Source2);
        //                        //Debug.WriteLine("Drives2: " + Drives2);
        //                        //Debug.WriteLine("ExcludedDrives2: " + DrivesExcluded2);
        //                        //Debug.WriteLine("Source3: " + Source3);
        //                        //Debug.WriteLine("Drives3: " + Drives3);
        //                        //Debug.WriteLine("ExcludedDrives3: " + DrivesExcluded3);

        //                        Debug.WriteLine("SourcePath [" + i + "]: " + PathsList[i].SourcePath);
        //                        Debug.WriteLine("fileNotDir [" + i + "]: " + PathsList[i].fileNotDir);
        //                        Debug.WriteLine("FileName [" + i + "]: " + PathsList[i].FileName);
        //                        Debug.WriteLine("DriveString [" + i + "]: " + PathsList[i].DriveString);

        //                        if (!String.IsNullOrEmpty(PathsList[i].SourcePath) || !String.IsNullOrWhiteSpace(PathsList[i].SourcePath))
        //                        {
        //                            if (Paths.fileOrNot[i] == false && Directory.Exists(PathsList[i].SourcePath))
        //                            {
        //                                Directory.Delete(Paths.SourcePathList[i], true);
        //                            }
        //                            else if (PathsList[i].fileNotDir == false && !Directory.Exists(PathsList[i].SourcePath))
        //                            {
        //                                Info = "Directory absent";
        //                            }
        //                            if (PathsList[i].fileNotDir == true && File.Exists(PathsList[i].SourcePath))
        //                            {
        //                                File.Delete(PathsList[i].SourcePath);
        //                            }
        //                            else if (PathsList[i].fileNotDir == true && !File.Exists(PathsList[i].SourcePath))
        //                            {
        //                                Info = "File absent";
        //                            }
        //                        }
        //                    }
        //                    Info = "Deleted";
        //                    string[] sd = { "Save", "Delete" };
        //                    ItemSaveDel = "Save";
        //                }
        //            }
        //        }

    }
}
