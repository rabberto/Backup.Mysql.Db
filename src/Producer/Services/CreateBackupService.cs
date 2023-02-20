using Backup.Mysql.Db.Producer.Config;
using Backup.Mysql.Db.Producer.Helpers;
using MySql.Data.MySqlClient;
using MySQLBackupNetCore;

namespace Backup.Mysql.Db.Producer.Services
{
    internal class CreateBackupService
    {
        private static readonly string[] _listDataBase = AppSettings.DataBase;
        private static readonly string _connectionString = AppSettings.ConnectionString;
        private static readonly string _pathBackup = AppSettings.FolderBackup;

        internal static void Execute(string dateTimeString)
        {
            LogService.Write($"START: {nameof(CreateBackupService)}.{nameof(Execute)}");

            if (!Directory.Exists(_pathBackup))
                Directory.CreateDirectory(_pathBackup);

            foreach (var dataBase in _listDataBase)
            {
                string fileName = $"{dataBase}_{dateTimeString}";
                LogService.Write($"INFO: fileName: {fileName}");

                string fullPath = $"{_pathBackup}{fileName}.sql";
                LogService.Write($"INFO: fullPath: {fullPath}");

                var connectionString = string.Format(_connectionString, dataBase);
                LogService.Write($"INFO: connectionString: {connectionString}");

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        LogService.Write($"START: {nameof(CreateBackupService)}.{nameof(Execute)} | DataBase: {dataBase}");
                        conn.Close();
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                mb.ExportToFile(fullPath);
                                conn.Close();
                            }
                        }
                        LogService.Write($"END: {nameof(CreateBackupService)}.{nameof(Execute)} | DataBase: {dataBase}");
                    }
                    catch (MySqlException ex)
                    {
                        LogService.Write($"ERROR: {nameof(CreateBackupService)}.{nameof(MySqlException)} | {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        LogService.Write($"ERROR: {nameof(CreateBackupService)}.{nameof(Exception)} | {ex.Message}");
                        throw;
                    }
                }
            }

            LogService.Write($"END: {nameof(CreateBackupService)}.{nameof(Execute)}");
        }
    }
}
