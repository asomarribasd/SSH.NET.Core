using Flurl;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //PasswordConnectionInfo conn = new PasswordConnectionInfo("[IP]", 22, "root", "[password]");
            //SshClient sshClient = new SshClient(conn);
            //sshClient.Connect();


            //while (true)
            //{
            //    var command = Console.ReadLine();

            //    var comm = sshClient.CreateCommand(command);
            //    comm.CommandTimeout = TimeSpan.FromSeconds(30);

            //    Console.WriteLine(comm.Execute());
            //}
            

            //using (var output = File.OpenWrite(@"c:\!temp\syslog8"))
            //{
            //    var input =  sftpClient.OpenRead("/opt/test/syslog8");

            //    input.CopyToAsync(output).Wait();
            //}

            
            
            //var d = sftpClient.ListDirectory("");
            //Console.WriteLine(string.Join(", ", d.Select(s => s.Name)));

            //sftpClient.UploadFile(File.OpenRead(@"C:\Users\khamzatsalikhov\Source\Workspaces\MicroCollection\DistributedCryptolog\Main\src\DistributedCryptolog\workers\parser\azure.JPG"), "/home/clog/cryptolog/workers/parser/azure.JPG");

        }
    }

    public static class Extentions
    {
        public static void EnsureDirectoryExist(this SftpClient sftpClient, string directory)
        {
            if (directory!= null && directory.Contains('\\'))
            {
                directory = directory.Replace('\\', '/'); 
            }

            if (string.IsNullOrWhiteSpace(directory) || !directory.Any(l => l != '/')) return;

            if (!sftpClient.Exists(directory))
            {
                //check if parent exists
                    var parent = Regex.Replace(directory, @"[^/].?[/$]", "");
                sftpClient.EnsureDirectoryExist(parent);

                sftpClient.CreateDirectory(directory);
            }
        }

        public static void UploadDirectory(this SftpClient sftpClient, string sourceDir, string destDir, bool overWrite = false)
        {
            if (string.IsNullOrWhiteSpace(sourceDir))
            {
                throw new ArgumentException("Path must not be empty", nameof(sourceDir));
            }

            if (string.IsNullOrWhiteSpace(destDir))
            {
                throw new ArgumentException("Path must not be empty", nameof(destDir));
            }

            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Directory \"{sourceDir}\" not exists");
            }

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                try
                {
                    sftpClient.UploadFile(File.OpenRead(file), destDir.AppendPathSegment(Path.GetFileName(file)));
                }
                catch (Exception ex)
                {
                    throw new IOException($"Error uploading directory {sourceDir} to {destDir} on file {Path.GetFileName(file)}", ex);
                }
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                sftpClient.UploadDirectory(dir, destDir.AppendPathSegment(Path.GetDirectoryName(dir)));
            }
        }
    }
}
