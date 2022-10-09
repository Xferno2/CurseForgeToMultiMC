using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using CurseForgeToMultiMC.Models;
using System.IO;
using System.Drawing;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Drawing.Imaging;
using CurseForgeToMultiMC.Properties;

namespace CurseForgeToMultiMC
{
    class Program
    {
        static FileStream ostrm;
        static StreamWriter writer;
        static bool isFileOpened = false;

        static void Logger(string msg) {
            var formated = "[" + DateTime.Now + "] " + msg;
            if (!isFileOpened)
                OpenFile();
            Console.WriteLine(formated);
            writer.WriteLine(formated);
        }
        static void OpenFile() {
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
        enum modLoaderENUM
        {
            UNKNOWN, Forge, Fabric, Quilt, Lite
        }
        static void Main(string[] args)
        {
            curseData[] curseDatDeserialized;
            string appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            Console.WriteLine("CurseForge modpacks to MultiMC converter");
            Console.WriteLine("Please specify MultiMC location");
            string mmcLocation = Console.ReadLine();
            if (!Directory.Exists(mmcLocation)) {
                Logger("Invalid file pathing. Try again");
                writer.Close();
                ostrm.Close();
                Thread.Sleep(3000);
                Environment.Exit(12);
            }
            Logger("Checking default location for Curse data...");
            string curseDataLocation = appDataLocal + "\\Overwolf\\Curse\\GameInstances\\MinecraftGameInstance.json";
            if (System.IO.File.Exists(curseDataLocation))
            {
                Logger("Folder Valid");
            }
            else {
                Logger("Folder not found. Curse not installed?");
                Logger("This application will close in 5 sec");
                writer.Close();
                ostrm.Close();
                Thread.Sleep(5000);
                Environment.Exit(12);
            }
            curseDatDeserialized = JsonConvert.DeserializeObject<curseData[]>(System.IO.File.ReadAllText(curseDataLocation));
            ConstructProfiles(curseDatDeserialized, mmcLocation);
        }
        static void ConstructProfiles(curseData[] curseDatDeserialized, string mmcLocation)
        {
            string mmcInstancesLocation = mmcLocation + "\\instances";
            string mmcIcons = mmcLocation + "\\icons";
            int totalModpacks = curseDatDeserialized.Length;
            int succesfulModpacks = 0;
            int failedModpacks = 0;
            int skippedModpacks = 0;
            foreach (var modpack in curseDatDeserialized)
            {
                var name = modpack.name.Trim();
                var mmcInstanceDirectory = mmcInstancesLocation + "\\" + name;
                // Define variables for easier use
                string author = "";
                string version = "";
                string imageUrl = "";
                string webSite = "";
                bool isCustomPack = false;

                if (modpack.installedModpack != null)
                {
                    author = modpack.manifest.author;
                    version = modpack.manifest.version;
                    imageUrl = modpack.installedModpack.thumbnailUrl;
                    webSite = modpack.installedModpack.webSiteURL;
                }
                else
                {
                    isCustomPack = true;
                }
                string mcVersion = modpack.gameVersion;
                DateTime lastPlayed = modpack.lastPlayed;
                string modloaderVersion = modpack.baseModLoader.forgeVersion;
                modLoaderENUM modloaderType = modLoaderENUM.UNKNOWN;
                string installPath = modpack.installPath;

                if (Directory.Exists(mmcInstanceDirectory))
                {
                    var localManifestLocation = mmcInstanceDirectory + "\\.minecraft\\manifest.json";
                    if (System.IO.File.Exists(localManifestLocation))
                    {
                        localManifest localManifestData = JsonConvert.DeserializeObject<localManifest>(System.IO.File.ReadAllText(localManifestLocation));
                        if (localManifestData.version != modpack.manifest.version)
                        {
                            name += " (" + modpack.manifest.version + ")";
                            mmcInstanceDirectory = mmcInstancesLocation + "\\" + name;
                            Logger("Instance with the same name and different version found. Updating variables");
                            if (Directory.Exists(mmcInstanceDirectory))
                            {
                                Logger("This version of the instance already exists, skipping");
                                skippedModpacks++;
                                continue;
                            }
                        }
                        else
                        {
                            Logger("Instance with the same name and same version found, skipping");
                            skippedModpacks++;
                            continue;
                        }
                    }
                    else
                    {
                        Logger("Custom Instance detected, one with the same name already exist. Either rename it in Curse or it's the same one already existent, skipping");
                        skippedModpacks++;
                        continue;
                    }
                }

                // Im not sure if Curse even supports Quilt or LiteLoader but just incase
                switch (modpack.baseModLoader.name.Split('-')[0])
                { // split baseLoader name to get the type of modloader
                    case "forge":
                        modloaderType = modLoaderENUM.Forge;
                        break;
                    case "fabric":
                        modloaderType = modLoaderENUM.Fabric;
                        break;
                    case "quilt":
                        modloaderType = modLoaderENUM.Quilt;
                        break;
                    case "liteloader":
                        modloaderType = modLoaderENUM.Lite;
                        break;
                    default:
                        modloaderType = modLoaderENUM.UNKNOWN;
                        break;

                };

                Bitmap mmcIcon = null;
                if (!isCustomPack)
                    mmcIcon = getImage(imageUrl);

                Logger("Creating instance directory for " + name);
                Directory.CreateDirectory(mmcInstanceDirectory);
                Logger("Generating mmc-pack for " + name);
                mmcPack mmcPack = generateMMCPack(mcVersion, modloaderVersion, modloaderType);
                Logger("Generating instange config for " + name);
                mmcInstance mmcInstance = generateMMCInstance(name, webSite, isCustomPack);
                Logger("Attempting to create symlink for " + name);
                bool isMKLinkpossible = callExternalProcess("cmd.exe", "mklink /J \"" + mmcInstanceDirectory + "\\.minecraft" + "\" \"" + installPath + "\"");

                if (isMKLinkpossible)
                {
                    Logger("Symlink created succesfully");
                    string serializedMMCPack = JsonConvert.SerializeObject(mmcPack);
                    string serializedMMCInstance = mmcInstance.serializeInstance();
                    try
                    {
                        Logger("Writing mmc-pack.json");
                        System.IO.File.WriteAllText(mmcInstanceDirectory + "\\mmc-pack.json", serializedMMCPack);
                        Logger("Writing instance.cfg");
                        System.IO.File.WriteAllText(mmcInstanceDirectory + "\\instance.cfg", serializedMMCInstance);
                        Logger("Saving image for the modpack");
                        if (mmcIcon != null)
                            mmcIcon.Save(mmcIcons + "\\" + name.Replace('.', '_') + ".png", ImageFormat.Png);
                        Logger("Writing .curseToMMC file");
                        System.IO.File.WriteAllText(mmcInstanceDirectory + "\\.curseToMMC", name + Environment.NewLine + (isCustomPack ? "true": "false"));
                        System.IO.File.SetAttributes(mmcInstanceDirectory + "\\.curseToMMC", System.IO.File.GetAttributes(mmcInstanceDirectory + "\\.curseToMMC") | FileAttributes.Hidden);
                    }
                    catch (Exception ex)
                    {
                        Logger("Exception encountered:");
                        Logger(ex.Message);
                        Logger($"Modpack {name} failed.");
                        failedModpacks++;
                    }
                    succesfulModpacks++;
                }
                else
                {
                    Logger("Symlink failed, skipping");
                    failedModpacks++;
                }
            }
            Logger($"Out of {totalModpacks} instances: {succesfulModpacks} succesful, {failedModpacks} failed, {skippedModpacks} skipped.");

            cleanInvalidInstances(mmcInstancesLocation);

            Logger("Finished execution and will close in 5 sec.");
            writer.Close();
            ostrm.Close();
            Thread.Sleep(5000);
            Environment.Exit(10);
        }

        private static void cleanInvalidInstances(string mmcInstancesLocation)
        {
            Logger("Cleaning out inexistent directories");
            int deletedInstances = 0;
            foreach (var directory in Directory.GetDirectories(mmcInstancesLocation))
            {
                if (!System.IO.File.Exists(directory + "\\.curseToMMC"))
                { continue; }
                else
                {
                    bool isSymlinkValid = false;
                    try
                    {
                        isSymlinkValid = Directory.Exists(directory + "\\.minecraft\\saves");
                    }
                    catch (Exception e)
                    { isSymlinkValid = false; }
                    if (isSymlinkValid)
                    { continue; }
                    else
                    {
                        Logger("Deleteing " + directory);
                        deletedInstances++;
                        try
                        {
                            Directory.Delete(directory, true); // this will probably get access denied when it deletes the junction but it does delete it
                            // it throws an exception either way and it stops the actual folder from being delted
                        }
                        catch (Exception e)
                        {
                            Directory.Delete(directory); // this solves that problem
                        }
                    }
                }
            }

            Logger($"{deletedInstances} instances have been deleted");
        }

        private static bool callExternalProcess(string fileName, string arguments) {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = " /C " + arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                var timeout = 2000;
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null){
                            outputWaitHandle.Set();
                        }
                        else{
                            output.AppendLine(e.Data);
                        }};

                    process.ErrorDataReceived += (sender, e) =>{
                        if (e.Data == null){
                            errorWaitHandle.Set();
                        }
                        else{
                            error.AppendLine(e.Data);
                        }};

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (process.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))   
                    { return true; }
                    else 
                    { return false; }
                }
            }
        } 

        private static Bitmap getImage(string url) {
            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();
            Stream responseStream =
                response.GetResponseStream();

            Bitmap bmp = new Bitmap(responseStream);

            responseStream.Dispose();
            return bmp;
        }

        private static mmcInstance generateMMCInstance(string modpackName, string webSite, bool isCustomPack)
        {
            // Most of these are the default when creating a profile
            mmcInstance mmcInstanceTemp = new mmcInstance
            {
                InstanceType = "OneSix",
                JoinServerOnLaunch = false,
                OverrideCommands = false,
                OverrideConsole = false,
                OverrideGameTime = false,
                OverrideJavaArgs = false,
                OverrideJavaLocation = false,
                OverrideMemory = false,
                OverrideNativeWorkArounds = false,
                OverrideWindow = false,
                name = modpackName,
                notes = "This pack was imported using CurseForgeToMultiMC (" + modpackName + ", " + webSite + ")",
            };
            if (isCustomPack) {
                mmcInstanceTemp.iconKey = "bee";
            }
            else
            {
                mmcInstanceTemp.iconKey = modpackName.Replace('.', '_');
            }
            return mmcInstanceTemp;
        }

        static mmcPack generateMMCPack(string mcVersion, string modloaderVersion, modLoaderENUM modloaderType)
        {
            // Deserialize model mmc-pack.json
            mmcPack mmcPackModel = JsonConvert.DeserializeObject<mmcPack>(Resources.mmc_pack);
            // All mmcComponents list
            List<mmcComponent> mmcComponents = new List<mmcComponent>();
            // Create Forge cahe
            mmcCachedRequire cachedRequire = new mmcCachedRequire();
            // Create Forge component
            mmcComponent mmcPackComponent = new mmcComponent();

            // Add already assigne values to the mmcPackComponent
            mmcPackComponent.cachedVersion = modloaderVersion;
            mmcPackComponent.version = modloaderVersion;
            mmcPackComponent.cachedRequires = new List<mmcCachedRequire>();

            //  Create universal Intermediary Mappings (only required for Fabric based loaders)
            mmcComponent intermediaryMappings = new mmcComponent();
            intermediaryMappings.cachedName = "Intermediary Mappings";
            mmcCachedRequire IMcachedRequire = new mmcCachedRequire();
            IMcachedRequire.equals = mcVersion;
            IMcachedRequire.uid = "net.minecraft";
            intermediaryMappings.cachedRequires = new List<mmcCachedRequire>();
            intermediaryMappings.cachedRequires.Add(IMcachedRequire);
            intermediaryMappings.cachedVersion = mcVersion;
            intermediaryMappings.cachedVolatile = true;
            intermediaryMappings.dependencyOnly = true;
            intermediaryMappings.uid = "net.fabricmc.intermediary";
            intermediaryMappings.version = mcVersion;

            switch (modloaderType)
            {
                case modLoaderENUM.Forge:
                    cachedRequire.uid = "net.minecraft";
                    cachedRequire.equals = mcVersion;
                    mmcPackComponent.cachedName = "Forge";
                    mmcPackComponent.uid = "net.minecraftforge";
                    mmcPackComponent.cachedRequires.Add(cachedRequire);
                    mmcComponents.Add(mmcPackComponent);
                    break;
                case modLoaderENUM.Fabric:
                    mmcComponents.Add(intermediaryMappings);
                    cachedRequire.uid = "net.fabricmc.intermediary";
                    mmcPackComponent.cachedName = "Fabric Loader";
                    mmcPackComponent.uid = "net.fabricmc.fabric-loader";
                    mmcPackComponent.cachedRequires.Add(cachedRequire);
                    mmcComponents.Add(mmcPackComponent);
                    break;
                case modLoaderENUM.Quilt:
                    mmcComponents.Add(intermediaryMappings);
                    cachedRequire.uid = "net.fabricmc.intermediary";
                    mmcPackComponent.cachedName = "Quilt Loader";
                    mmcPackComponent.uid = "net.fabricmc.fabric-loader";
                    mmcPackComponent.cachedRequires.Add(cachedRequire);
                    mmcComponents.Add(mmcPackComponent);
                    break;
                case modLoaderENUM.Lite:
                    cachedRequire.uid = "net.minecraft";
                    cachedRequire.equals = mcVersion;
                    mmcPackComponent.cachedName = "LiteLoader";
                    mmcPackComponent.uid = "com.mumfrey.liteloader";
                    mmcPackComponent.cachedRequires.Add(cachedRequire);
                    mmcComponents.Add(mmcPackComponent);
                    break;
            }

            foreach (var component in mmcComponents)
            {
                mmcPackModel.components.Add(component);
            }

            return mmcPackModel;
        }
    }
}
