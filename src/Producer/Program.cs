using Backup.Mysql.Db.Producer.Config;
using Backup.Mysql.Db.Producer.Helpers;
using MySql.Data.MySqlClient;
using MySQLBackupNetCore;
using System.IO.Compression;

LogService.Write("Start job");
var listDataBase = AppSettings.DataBase;
var connectionString = AppSettings.ConnectionString;
var pathBackup = AppSettings.FolderBackup;
var folderBackupSubmit = AppSettings.FolderBackupSubmit;

LogService.Write($"connectionString: {connectionString}");
LogService.Write($"pathBackup: {pathBackup}");

if (!Directory.Exists(pathBackup))
    Directory.CreateDirectory(pathBackup);

if (!Directory.Exists(pathBackup))
    Directory.CreateDirectory(pathBackup);

foreach (var dataBase in listDataBase)
{
    string fileName = CreatedFileName(dataBase);
    LogService.Write($"fileName: {fileName}");

    string fullPath = $"{pathBackup}{fileName}.sql";
    LogService.Write($"fullPath: {fullPath}");

    connectionString = string.Format(connectionString, dataBase);
    LogService.Write($"connectionString: {connectionString}");

    var pathZip = $"{folderBackupSubmit}{fileName}.zip";
    LogService.Write($"pathZip: {pathZip}");

    using (MySqlConnection conn = new MySqlConnection(connectionString))
    {
        try
        {
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

            LogService.Write($"End Backup: {dataBase}");

            ZipFile.CreateFromDirectory(pathBackup, pathZip);
            LogService.Write($"Zip File: {pathZip}");

            File.Delete(fullPath);
            LogService.Write($"Delete File: {fullPath}");

        }
        catch (MySqlException ex)
        {
            LogService.Write($"ERROR: {nameof(MySqlException)} | {ex.Message}");
        }
        catch (Exception ex)
        {
            LogService.Write($"ERROR: {nameof(Exception)} | {ex.Message}");
        }
    }
}

//var files = Directory.GetFiles(folderBackupSubmit).Select(f => f.);
//foreach (var file in files)
//{
//    file.d
//}

LogService.Write($"End Job");
Console.ReadKey();

string CreatedFileName(string dataBase)
    => $"{dataBase}_{DateTime.Now.Year.ToString().PadLeft(4, '0')}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}";