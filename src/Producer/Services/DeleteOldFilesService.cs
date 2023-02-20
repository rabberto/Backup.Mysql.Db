using Backup.Mysql.Db.Producer.Config;
using Backup.Mysql.Db.Producer.Helpers;

namespace Backup.Mysql.Db.Producer.Services
{
    internal class DeleteOldFilesService
    {
        private static string _folderBackupSubmit = AppSettings.FolderBackupSubmit;

        internal static void Execute()
        {
            try
            {
                LogService.Write($"START: {nameof(DeleteOldFilesService)}.{nameof(Execute)}");

                DirectoryInfo directoryInfo = new DirectoryInfo(_folderBackupSubmit);

                foreach (FileInfo file in directoryInfo.GetFiles())
                    if (DateTime.Now.Subtract(file.LastWriteTime).Days > 5)
                        file.Delete();
            }
            catch (Exception ex)
            {
                LogService.Write($"ERROR: {nameof(DeleteOldFilesService)}.{nameof(Execute)} | {ex.Message}");
            }

            LogService.Write($"END: {nameof(DeleteOldFilesService)}.{nameof(Execute)}");
        }
    }
}
