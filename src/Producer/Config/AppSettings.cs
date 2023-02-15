using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace Backup.Mysql.Db.Producer.Config
{
    public static class AppSettings
    {
        public static string[] DataBase { get { return GetAppSettings().GetValues(nameof(DataBase)).First().Split(";"); } }
        public static string ConnectionString { get { return GetAppSettings().GetValues(nameof(ConnectionString))?.FirstOrDefault() ?? String.Empty; } }
        public static string FolderBackup { get { return GetAppSettings().GetValues(nameof(FolderBackup))?.FirstOrDefault() ?? String.Empty; } }
        public static string FolderBackupSubmit { get { return GetAppSettings().GetValues(nameof(FolderBackupSubmit))?.FirstOrDefault() ?? String.Empty; } }
        public static string FolderLog { get { return GetAppSettings().GetValues(nameof(FolderLog))?.FirstOrDefault() ?? String.Empty; } }

        private static NameValueCollection GetAppSettings()
            => ConfigurationManager.AppSettings;
    }
}
