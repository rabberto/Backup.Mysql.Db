// See https://aka.ms/new-console-template for more information
using MySql.Data.MySqlClient;
using MySQLBackupNetCore;

Console.WriteLine("Hello, World!");

String servidor = "192.99.241.251";
String usuario = "developer";
String senha = "JMzut6j8$Az$";
String banco = "db-backup-test";
String local = "c:/temp";

//faz a leitura do arquivo config.dat e captura o valor de cada linha.
//int contador = 0;
//string linha;
//System.IO.StreamReader file = new System.IO.StreamReader(@"config.dat");
//while ((linha = file.ReadLine()) != null)
//{
//    if (contador == 0) servidor = linha;
//    if (contador == 1) usuario = linha;
//    if (contador == 2) senha = linha;
//    if (contador == 3) banco = linha;
//    if (contador == 4) local = linha;
//    contador++;
//}
//file.Close();
// fim da leitura do arquivo.

string constring = "server=" + servidor + ";user=" + usuario + ";pwd=" + senha + ";database=" + banco + ";";
Console.Write(constring);

constring += "charset=utf8;convertzerodatetime=true;";

// define o nome do arquivo de backup de acordo com a data e hora.
string dia = DateTime.Now.Day.ToString();
string mes = DateTime.Now.Month.ToString();
string ano = DateTime.Now.Year.ToString();
string hora = DateTime.Now.ToLongTimeString().Replace(":", "");
string nomeDoArquivo = ano + mes + dia + "_" + hora;
// fim

// gera o conteúdo do arquivo de backup e salva no local definido no config.dat
string arquivo = local + "\\" + nomeDoArquivo + ".sql";
using (MySqlConnection conn = new MySqlConnection(constring))
{
    conn.Close();
    using (MySqlCommand cmd = new MySqlCommand())
    {
        using (MySqlBackup mb = new MySqlBackup(cmd))
        {
            cmd.Connection = conn;
            conn.Open();
            var dbs = conn.GetSchema();
            Console.Write(dbs);
            mb.ExportToFile(arquivo);
            conn.Close();
        }
    }
}
// fim