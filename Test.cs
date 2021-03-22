using System.Collections.Generic;
using System.Diagnostics;

namespace Save
{
    public class Test
    {
        public void AA(List<string> list, string procedureName, string listName)
        {
            foreach (string i in list)
            {
                Debug.WriteLine("In " + procedureName + ", " + listName + ": " + i);
            }
        }
    }
}