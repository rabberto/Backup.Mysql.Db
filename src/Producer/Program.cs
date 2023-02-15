using Backup.Mysql.Db.Producer.Config;
using Backup.Mysql.Db.Producer.Helpers;
using MySql.Data.MySqlClient;
using MySQLBackupNetCore;
using System.IO.Compression;

LogService.Write($"START:  {nameof(Program)}");

var listDataBase = AppSettings.DataBase;
var connectionString = AppSettings.ConnectionString;
var pathBackup = AppSettings.FolderBackup;
var folderBackupSubmit = AppSettings.FolderBackupSubmit;
var dateTimeString = CreateDateTime();

LogService.Write($"INFO: connectionString: {connectionString}");
LogService.Write($"INFO: pathBackup: {pathBackup}");

if (!Directory.Exists(pathBackup))
    Directory.CreateDirectory(pathBackup);

if (!Directory.Exists(pathBackup))
    Directory.CreateDirectory(pathBackup);

LogService.Write($"START: {nameof(Program)}.{nameof(CreateBackup)}");
CreateBackup();
LogService.Write($"END: {nameof(Program)}.{nameof(CreateBackup)}");

LogService.Write($"START: {nameof(Program)}.{nameof(ZipFiles)}");
ZipFiles();
LogService.Write($"END: {nameof(Program)}.{nameof(ZipFiles)}");

LogService.Write($"START: {nameof(Program)}.{nameof(DeleteSqlFiles)}");
DeleteSqlFiles();
LogService.Write($"END: {nameof(Program)}.{nameof(DeleteOldFiles)}");

LogService.Write($"START: {nameof(Program)}.{nameof(DeleteOldFiles)}");
DeleteOldFiles();
LogService.Write($"END: {nameof(Program)}.{nameof(DeleteOldFiles)}");

LogService.Write($"END:  {nameof(Program)}");

void CreateBackup()
{
    foreach (var dataBase in listDataBase)
    {
        string fileName = $"{dataBase}_{dateTimeString}";
        LogService.Write($"INFO: fileName: {fileName}");

        string fullPath = $"{pathBackup}{fileName}.sql";
        LogService.Write($"INFO: fullPath: {fullPath}");

        connectionString = string.Format(connectionString, dataBase);
        LogService.Write($"INFO: connectionString: {connectionString}");

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                LogService.Write($"START: {nameof(Program)}.{nameof(CreateBackup)} | DataBase: {dataBase}");
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
                LogService.Write($"END: {nameof(Program)}.{nameof(CreateBackup)} | DataBase: {dataBase}");
            }
            catch (MySqlException ex)
            {
                LogService.Write($"ERROR: {nameof(Program)}.{nameof(MySqlException)} | {ex.Message}");
            }
            catch (Exception ex)
            {
                LogService.Write($"ERROR: {nameof(Program)}.{nameof(Exception)} | {ex.Message}");
            }
        }
    }
}

void ZipFiles()
{
    try
    {
        var pathZip = $"{folderBackupSubmit}RBBSolucoes_{dateTimeString}.zip";

        ZipFile.CreateFromDirectory(pathBackup, pathZip);

    }
    catch (Exception ex)
    {
        LogService.Write($"ERROR: {nameof(Program)}.{nameof(ZipFiles)} | {ex.Message}");
    }
}

void DeleteSqlFiles()
{
    try
    {
        var filesDelete = Directory.GetFiles(pathBackup);
        foreach (var fileDelete in filesDelete)
            File.Delete(fileDelete);
    }
    catch (Exception ex)
    {
        LogService.Write($"ERROR: {nameof(Program)}.{nameof(DeleteSqlFiles)} | {ex.Message}");
    }
}

void DeleteOldFiles()
{
    try
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderBackupSubmit);

        foreach (FileInfo file in directoryInfo.GetFiles())
            if (DateTime.Now.Subtract(file.LastWriteTime).Days > 5)
                file.Delete();
    }
    catch (Exception ex)
    {
        LogService.Write($"ERROR: {nameof(Program)}.{nameof(DeleteOldFiles)} | {ex.Message}");
    }
}

string CreateDateTime()
    => $"{DateTime.Now.Year.ToString().PadLeft(4, '0')}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}";