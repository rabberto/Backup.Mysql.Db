using Backup.Mysql.Db.Producer.Config;

namespace Backup.Mysql.Db.Producer.Helpers
{
    internal static class LogService
    {
        private static readonly string _folderLog = AppSettings.FolderLog;

        public static void Write(string msg)
        {
            msg = $"{DateTime.Now} - {msg}";
            try
            {
                var sw = Initial();
                sw.WriteLine(msg);
                sw.Close();
                Console.WriteLine(msg);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static StreamWriter Initial()
        {
            var filePath = BuilderFilePath(_folderLog);

            if (!Directory.Exists(_folderLog))
                Directory.CreateDirectory(_folderLog);

            if (!File.Exists(filePath))
                return new StreamWriter(filePath, false);
            else
                return new StreamWriter(filePath, true);
        }

        private static string BuilderFilePath(string path)
            => $"{path}log{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.txt";
    }
}
