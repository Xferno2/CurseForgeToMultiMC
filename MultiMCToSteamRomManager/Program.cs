using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiMCToSteamRomManager
{

    class Program
    {
        static string mmcLocation;

        static FileStream ostrm;
        static StreamWriter writer;
        static bool isFileOpened = false;

        static void Logger(string msg)
        {
            var formated = "[" + DateTime.Now + "] " + msg;
            if (!isFileOpened)
                OpenFile();
            Console.WriteLine(formated);
            writer.WriteLine(formated);
        }
        static void OpenFile()
        {
            isFileOpened = true;
            if (System.IO.File.Exists("./log.txt"))
                System.IO.File.WriteAllText("./log.txt", string.Empty);
            try
            {
                ostrm = new FileStream("./log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open log.txt for writing. Please fix this issue.");
                Console.WriteLine(e.Message);
                Thread.Sleep(5000);
                Environment.Exit(11);
                return;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("MultiMC to Steam ROM Manager quality of life improvement");
            Console.WriteLine("Please specify MultiMC location");
            mmcLocation = Console.ReadLine();
            if (!Directory.Exists(mmcLocation))
            {

                Logger("Invalid file pathing. Try again");
                writer.Close();
                ostrm.Close();
                Thread.Sleep(3000);
                Environment.Exit(12);
            }
            string mmcInstancesLocation = mmcLocation + "\\instances";
            string mmcIcons = mmcLocation + "\\icons";
            string steamIcons = mmcLocation + "\\steamicons";
            if (!Directory.Exists(steamIcons)) {
                Directory.CreateDirectory(steamIcons);
            }
            foreach (var directory in Directory.GetDirectories(mmcInstancesLocation))
            {
                string curseToMMCLocation = directory + "\\.curseToMMC";
                if (File.Exists(curseToMMCLocation))
                {
                    string[] curseData = File.ReadAllLines(curseToMMCLocation);
                    string originalName = curseData[0];
                    string isCustompack = curseData[1];
                    bool.TryParse(isCustompack, out bool isCustompackBool);
                    if (!isCustompackBool)
                    {
                        File.Copy(mmcIcons + "\\" + originalName.Replace('.', '_') + ".png", steamIcons + "\\" + originalName + ".png", true);
                        using (var image = new MagickImage(steamIcons + "\\" + originalName + ".png")) {
                            image.Write(steamIcons + "\\" + originalName + "_icon.ico");
                        }
                        File.Copy(steamIcons + "\\" + originalName + ".png", steamIcons + "\\" + originalName + "_icon.png", true);
                        File.Copy(steamIcons + "\\" + originalName + ".png", steamIcons + "\\" + originalName + "_hero.png", true);
                        File.Copy(steamIcons + "\\" + originalName + ".png", steamIcons + "\\" + originalName + "_logo.png", true);
                    }
                }
            }
        }
    }
}
