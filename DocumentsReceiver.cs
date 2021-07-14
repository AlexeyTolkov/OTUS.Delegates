using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Timers;

namespace Delegates
{
    public class DocumentsReceiver
    {
        private string _targetDirectory;
        private DocumentsReceiverStatus _documentsStatus;

        private delegate void DocumentsReadyHandler(string message);
        private event DocumentsReadyHandler DocumentsReady;

        private readonly Timer _timer;
        private readonly FileSystemWatcher _fileSystemWatcher;

        public DocumentsReceiver()
        {
            _timer = new Timer();
            _fileSystemWatcher = new FileSystemWatcher();
        }

        public void Start(string targetDirectory, int waitingIntervalInSeconds)
        {
            _targetDirectory = targetDirectory;
            Directory.CreateDirectory(_targetDirectory);

            _timer.Interval = waitingIntervalInSeconds * 1000;
            _timer.Elapsed += TimedOut;
            _timer.Enabled = true;

            _fileSystemWatcher.Path = _targetDirectory;
            _fileSystemWatcher.Changed += ValidateFiles;
            _fileSystemWatcher.EnableRaisingEvents = true;

            DocumentsReady += DisplayMessage;
        }

        private void TimedOut(Object source, ElapsedEventArgs e)
        {
            Stop();
        }

        public void Stop()
        {
            NotifyEndUser(_documentsStatus);

            _timer.Elapsed -= TimedOut;
            _fileSystemWatcher.Changed -= ValidateFiles;
        }

        private void ValidateFiles(object sender, FileSystemEventArgs e)
        {
            Collection<string> filesCollection = new Collection<string>();

            FileInfo[] files = new DirectoryInfo(_targetDirectory).GetFiles();
            foreach (var file in files)
            {
                filesCollection.Add(file.Name);
            }

            if (filesCollection.Contains("Паспорт.jpg") &&
                filesCollection.Contains("Заявление.txt") &&
                filesCollection.Contains("Фото.jpg"))
            {
                _documentsStatus = DocumentsReceiverStatus.Succeeded;
                Stop();
            }
            else
            {
                _documentsStatus = DocumentsReceiverStatus.Failed;
            }
        }

        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void NotifyEndUser(DocumentsReceiverStatus documentsStatus)
        {
            switch (documentsStatus)
            {
                case DocumentsReceiverStatus.Succeeded:
                    DocumentsReady?.Invoke("All files've been received");
                    break;

                case DocumentsReceiverStatus.Failed:
                    DocumentsReady?.Invoke("Timed out");
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}