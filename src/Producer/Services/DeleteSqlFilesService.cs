using Backup.Mysql.Db.Producer.Config;
using Backup.Mysql.Db.Producer.Helpers;

namespace Backup.Mysql.Db.Producer.Services
{
    internal class DeleteSqlFilesService
    {
        private static string _pathBackup = AppSettings.FolderBackup;

        internal static void Execute()
        {
            LogService.Write($"START: {nameof(DeleteSqlFilesService)}.{nameof(Execute)}");

            try
            {
                var filesDelete = Directory.GetFiles(_pathBackup);
                foreach (var fileDelete in filesDelete)
                    File.Delete(fileDelete);
            }
            catch (Exception ex)
            {
                LogService.Write($"ERROR: {nameof(DeleteSqlFilesService)}.{nameof(Execute)} | {ex.Message}");
                throw;
            }

            LogService.Write($"END: {nameof(DeleteSqlFilesService)}.{nameof(Execute)}");
        }
    }
}
