using Backup.Mysql.Db.Producer.Config;
using Backup.Mysql.Db.Producer.Helpers;
using System.IO.Compression;

namespace Backup.Mysql.Db.Producer.Services
{
    internal class ZipFileService
    {
        private static string _pathBackup = AppSettings.FolderBackup;
        private static string _folderBackupSubmit = AppSettings.FolderBackupSubmit;
        internal static void Execute(string dateTimeString)
        {
            LogService.Write($"START: {nameof(ZipFileService)}.{nameof(Execute)}");

            try
            {
                var pathZip = $"{_folderBackupSubmit}RBBSolucoes_{dateTimeString}.zip";

                ZipFile.CreateFromDirectory(_pathBackup, pathZip);

            }
            catch (Exception ex)
            {
                LogService.Write($"ERROR: {nameof(ZipFileService)}.{nameof(Execute)} | {ex.Message}");
                throw;
            }

            LogService.Write($"END: {nameof(ZipFileService)}.{nameof(Execute)}");
        }
    }
}
