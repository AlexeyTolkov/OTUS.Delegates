using System;
using System.IO;
using DocuHelper;

namespace Delegates
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderName = "InputData";
            string folderPath = Path.Combine(Environment.CurrentDirectory, folderName);

            using (var receiver = new DocumentsReceiver())
            {
                receiver.Start(folderPath, 60);
            }

            Console.ReadKey();
        }
    }
}