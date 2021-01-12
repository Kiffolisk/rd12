using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rd12tool
{
    class Program
    {
        public static bool debugMode = false; // for debugging
        // ENCRYPTION FOR RD12
        public static string[] decrypted = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z" };
        // ENCRYPTED LETTERS ARE THE SAME AS DECRYPTED IN THIS REPO
        public static string[] encrypted = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z" };
        public static string Encrypt(string start)
        {
            for (int i=0; i<decrypted.Length; i++)
            {
                // debug mode code --> bad way of telling me what encrypted and decrypted value is chosen
                if (debugMode)
                {
                    Console.WriteLine("int i = " + i + "; \ndecrypted[" + i + "] = " + decrypted[i] + "; \nencrypted[" + i + "] = " + encrypted[i] + "; \nstart = start.Replace(" + decrypted[i] + ", " + encrypted[i] + ")");
                }
                start = start.Replace(decrypted[i], encrypted[i]);
                if (debugMode)
                {
                    Console.ReadKey();
                }
            }
            return start;
        }

        public static string Decrypt(string start)
        {
            for (int i = 0; i < encrypted.Length; i++)
            {
                if (debugMode)
                {
                    Console.WriteLine("int i = " + i + "; \ndecrypted[" + i + "] = " + decrypted[i] + "; \nencrypted[" + i + "] = " + encrypted[i] + "; \nstart = start.Replace(" + encrypted[i] + ", " + decrypted[i] + ")");
                }
                start = start.Replace(encrypted[i], decrypted[i]);
                if (debugMode)
                {
                    Console.ReadKey();
                }
            }
            return start;
        }

        public static void WriteLineForeground(string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static string IsDirectoryOrFile(string path)
        {
            string result = "";
            if (Directory.Exists(path))
            {
                result = "folder";
            }
            else if (File.Exists(path))
            {
                result = "file";
            }
            else
            {
                result = "none";
            }
            return result;
        }

        public static string targetFolder = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
        static void WaitFor()
        {
            WriteLineForeground("rd12tool - Read and write .rd12 files!", ConsoleColor.Yellow);
            Console.WriteLine("");
            Console.WriteLine(targetFolder + ") Type action:");
            string arg = Console.ReadLine();
            if (arg.ToLower().StartsWith("cd "))
            {
                if (!arg.Substring(3).StartsWith(Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location)))
                {
                    if (IsDirectoryOrFile(targetFolder + arg.Substring(3)) == "folder")
                    {
                        Console.Clear();
                        targetFolder = targetFolder + arg.Substring(3) + @"\";
                    }
                    else if (IsDirectoryOrFile(targetFolder + arg.Substring(3)) != "folder")
                    {
                        Console.Clear();
                        Console.WriteLine("Specified directory isn't a folder or doesn't exist.");
                    }
                }
                else
                {
                    if (IsDirectoryOrFile(arg.Substring(3).Replace(@"/", @"\")) == "folder")
                    {
                        Console.Clear();
                        if (arg.Substring(3).Replace(@"/", @"\").EndsWith(@"\"))
                        {
                            targetFolder = arg.Substring(3).Replace(@"/", @"\");
                        }
                        else
                        {
                            targetFolder = arg.Substring(3).Replace(@"/", @"\") + @"\";
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(@"Specified directory isn't a folder or doesn't exist. Are you sure you typed \ instead of /?");
                    }
                }
            }
            else if (arg.ToLower().StartsWith("read "))
            {
                if (IsDirectoryOrFile(targetFolder + arg.Substring(5) + ".rd12") == "file")
                {
                    Console.Clear();
                    Console.WriteLine(Decrypt(File.ReadAllText(targetFolder + arg.Substring(5) + ".rd12")));
                    WriteLineForeground("\nPress enter to continue.", ConsoleColor.DarkRed);
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Specified directory isn't a file or doesn't exist.");
                }
            }
            else if (arg.ToLower().StartsWith("write "))
            {
                if (IsDirectoryOrFile(targetFolder + arg.Substring(6)) == "file")
                {
                    Console.Clear();
                    Console.WriteLine("File already exists!");
                }
                else
                {
                    File.Create(targetFolder + arg.Substring(6) + ".rd12").Close();
                    WriteLineForeground("Now editing file: " + arg.Substring(6), ConsoleColor.Red);
                    WriteLineForeground("To add a line break, type in " +@"[LineBreak]" + ".", ConsoleColor.Red);
                    Console.WriteLine("");
                    string filewrite = Console.ReadLine().Replace(@"[LineBreak]", Environment.NewLine);
                    string filewrite2 = Encrypt(filewrite);
                    File.AppendAllText(targetFolder + arg.Substring(6) + ".rd12", Encrypt(filewrite2));
                    Console.Clear();
                }
            }
            else if (arg.ToLower() == "info")
            {
                Console.Clear();
                WriteLineForeground("rd12tool is a tool for file encryption and decryption.\nIt started as a simple file writing and reading program,\nbut slowly I started to make an encryption and decryption tool.", ConsoleColor.DarkBlue);
                Console.ReadKey();
                Console.Clear();
            }
            else if (arg.ToLower().StartsWith("delete "))
            {
                if (IsDirectoryOrFile(targetFolder + arg.Substring(7) + ".rd12") == "file")
                {
                    Console.Clear();
                    File.Delete(targetFolder + arg.Substring(7) + ".rd12");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Specified directory isn't a file or doesn't exist.");
                }
            }
            WaitFor();
        }
        static void Main(string[] args)
        {
            WaitFor();
        }
    }
}
