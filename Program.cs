using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderName = "InputData";
            string folderPath = Path.Combine(Environment.CurrentDirectory, folderName);

            var receiver = new DocumentsReceiver();
            receiver.Start(folderPath, 60);
            Console.ReadKey();
        }
    }
}