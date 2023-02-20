using Backup.Mysql.Db.Producer.Helpers;
using Backup.Mysql.Db.Producer.Services;

LogService.Write($"START:  {nameof(Program)}");

var dateTimeString = CreateDateTime();

CreateBackupService.Execute(dateTimeString);

ZipFileService.Execute(dateTimeString);

DeleteSqlFilesService.Execute();

DeleteOldFilesService.Execute();

LogService.Write($"END:  {nameof(Program)}");

string CreateDateTime()
    => $"{DateTime.Now.Year.ToString().PadLeft(4, '0')}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}";