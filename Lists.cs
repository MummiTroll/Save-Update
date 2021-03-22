using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace Save
{
    public class Lists
    {
        #region Objects
        public ViewModel ViewModel { get; set; }
        Test test = new Test();
        #endregion

        public Lists(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public void MakeLists()
        {
            ViewModel.Info = string.Empty;
            ViewModel.PathProject.MakePathList();
            ViewModel.Source.MakeSourcePathList();
            ViewModel.Target.MakeTargetPathList();
            ViewModel.Drives.MakeDrivesList();
        }
        public void AddToList(List<string> list, string str)
        {
            list.Add((!String.IsNullOrEmpty(str) && !String.IsNullOrWhiteSpace(str)) ? str : "");
        }
        public List<string> LoadList(string fileName)
        {
            List<string> list = new List<string>();
            if (!Directory.Exists(ViewModel.dir)) { Directory.CreateDirectory(ViewModel.dir); }
            if (Directory.Exists(ViewModel.dir))
            {
                if (File.Exists(ViewModel.dir + fileName + ".bin"))
                {
                    FileStream fs = new FileStream(ViewModel.dir + fileName + ".bin", FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    list = (List<string>)formatter.Deserialize(fs); fs.Close();
                }
            }
            return list;
        }
        public List<List<string>> LoadList2D(string fileName)
        {
            //MessageBox.Show("LoadList2D: " + ViewModel.dir);
            List<List<string>> list = new List<List<string>>();
            if (!Directory.Exists(ViewModel.dir)) { Directory.CreateDirectory(ViewModel.dir); }
            if (Directory.Exists(ViewModel.dir))
            {
                if (File.Exists(ViewModel.dir + fileName + ".bin"))
                {
                    FileStream fs = new FileStream(ViewModel.dir + fileName + ".bin", FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    list = (List<List<string>>)formatter.Deserialize(fs); fs.Close();
                }
            }
            return list;
        }
        public void SaveList(List<string> list, string fileName)
        {
            //MessageBox.Show("SaveList: "+ ViewModel.dir);
            if (!Directory.Exists(ViewModel.dir))
            {
                Directory.CreateDirectory(ViewModel.dir);
            }
            else
            {
                var binaryFormatter = new BinaryFormatter(); var fi = new FileInfo(ViewModel.dir + fileName + ".bin");
                using (var binaryFile = fi.Create())
                { binaryFormatter.Serialize(binaryFile, list); binaryFile.Flush(); }
            }
        }
        public void SaveList2D(List<List<string>> list, string fileName)
        {
            if (!Directory.Exists(ViewModel.dir))
            {
                Directory.CreateDirectory(ViewModel.dir);
            }
            else
            {
                var binaryFormatter = new BinaryFormatter(); var fi = new FileInfo(ViewModel.dir + fileName + ".bin");
                using (var binaryFile = fi.Create())
                { binaryFormatter.Serialize(binaryFile, list); binaryFile.Flush(); }
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
    }
}
