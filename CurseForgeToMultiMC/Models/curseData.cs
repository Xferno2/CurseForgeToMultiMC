using System;
using System.Collections.Generic;
using System.Text;

namespace CurseForgeToMultiMC
{
    public class Author
    {
        public string name { get; set; }
    }

    public class BaseModLoader
    {
        public string forgeVersion { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string downloadUrl { get; set; }
        public string filename { get; set; }
        public int installMethod { get; set; }
        public bool latest { get; set; }
        public bool recommended { get; set; }
        public string versionJson { get; set; }
        public string librariesInstallLocation { get; set; }
        public string minecraftVersion { get; set; }
        public string installProfileJson { get; set; }
    }

    public class CachedScan
    {
        public string folderName { get; set; }
        public object fingerprint { get; set; }
        public object fileDateHash { get; set; }
        public int sectionID { get; set; }
        public List<object> individualFingerprints { get; set; }
        public int status { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime lastWriteTimeUtc { get; set; }
        public DateTime queryTimestamp { get; set; }
        public int fileCount { get; set; }
        public int fileSize { get; set; }
    }

    public class Dependency
    {
        public int addonId { get; set; }
        public int type { get; set; }
    }

    public class File
    {
        public int projectID { get; set; }
        public int fileID { get; set; }
        public bool required { get; set; }
    }

    public class Hash
    {
        public string value { get; set; }
    }

    public class InstalledAddon
    {
        public int addonID { get; set; }
        public int gameID { get; set; }
        public string gameInstanceID { get; set; }
        public string name { get; set; }
        public List<Author> authors { get; set; }
        public string primaryAuthor { get; set; }
        public int primaryCategoryId { get; set; }
        public int packageType { get; set; }
        public string webSiteURL { get; set; }
        public string thumbnailUrl { get; set; }
        public InstalledFile installedFile { get; set; }
        public DateTime dateInstalled { get; set; }
        public DateTime dateUpdated { get; set; }
        public DateTime dateLastUpdateAttempted { get; set; }
        public int status { get; set; }
        public int installSource { get; set; }
        public object preferenceReleaseType { get; set; }
        public bool preferenceAutoInstallUpdates { get; set; }
        public bool preferenceAlternateFile { get; set; }
        public bool preferenceIsIgnored { get; set; }
        public bool isModified { get; set; }
        public bool isWorkingCopy { get; set; }
        public bool isFuzzyMatch { get; set; }
        public object manifestName { get; set; }
        public List<object> installedTargets { get; set; }
        public LatestFile latestFile { get; set; }
    }

    public class InstalledFile
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public DateTime fileDate { get; set; }
        public int fileLength { get; set; }
        public int releaseType { get; set; }
        public int fileStatus { get; set; }
        public string downloadUrl { get; set; }
        public bool isAlternate { get; set; }
        public int alternateFileId { get; set; }
        public List<object> dependencies { get; set; }
        public bool isAvailable { get; set; }
        public List<Module> modules { get; set; }
        public object packageFingerprint { get; set; }
        public List<string> gameVersion { get; set; }
        public bool hasInstallScript { get; set; }
        public bool isCompatibleWithClient { get; set; }
        public int restrictProjectFileAccess { get; set; }
        public int projectStatus { get; set; }
        public int projectId { get; set; }
        public string FileNameOnDisk { get; set; }
        public List<Hash> Hashes { get; set; }
        public List<SortableGameVersion> sortableGameVersion { get; set; }
    }

    public class InstalledModpack
    {
        public int addonID { get; set; }
        public int gameID { get; set; }
        public string gameInstanceID { get; set; }
        public string name { get; set; }
        public List<Author> authors { get; set; }
        public string primaryAuthor { get; set; }
        public int primaryCategoryId { get; set; }
        public int packageType { get; set; }
        public string webSiteURL { get; set; }
        public string thumbnailUrl { get; set; }
        public InstalledFile installedFile { get; set; }
        public DateTime dateInstalled { get; set; }
        public DateTime dateUpdated { get; set; }
        public DateTime dateLastUpdateAttempted { get; set; }
        public int status { get; set; }
        public int installSource { get; set; }
        public object preferenceReleaseType { get; set; }
        public bool preferenceAutoInstallUpdates { get; set; }
        public bool preferenceAlternateFile { get; set; }
        public bool preferenceIsIgnored { get; set; }
        public bool isModified { get; set; }
        public bool isWorkingCopy { get; set; }
        public bool isFuzzyMatch { get; set; }
        public object manifestName { get; set; }
        public List<object> installedTargets { get; set; }
        public LatestFile latestFile { get; set; }
    }

    public class LatestFile
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public DateTime fileDate { get; set; }
        public int fileLength { get; set; }
        public int releaseType { get; set; }
        public int fileStatus { get; set; }
        public string downloadUrl { get; set; }
        public bool isAlternate { get; set; }
        public int alternateFileId { get; set; }
        public List<object> dependencies { get; set; }
        public bool isAvailable { get; set; }
        public List<Module> modules { get; set; }
        public object packageFingerprint { get; set; }
        public List<string> gameVersion { get; set; }
        public List<SortableGameVersion> sortableGameVersion { get; set; }
        public bool hasInstallScript { get; set; }
        public bool isCompatibleWithClient { get; set; }
        public int restrictProjectFileAccess { get; set; }
        public int projectStatus { get; set; }
        public int projectId { get; set; }
        public string FileNameOnDisk { get; set; }
        public List<Hash> Hashes { get; set; }
    }

    public class Manifest
    {
        public Minecraft minecraft { get; set; }
        public string manifestType { get; set; }
        public int manifestVersion { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string author { get; set; }
        public object description { get; set; }
        public int? projectID { get; set; }
        public List<File> files { get; set; }
        public string overrides { get; set; }
    }

    public class Minecraft
    {
        public string version { get; set; }
        public object additionalJavaArgs { get; set; }
        public List<ModLoader> modLoaders { get; set; }
        public object libraries { get; set; }
    }

    public class ModLoader
    {
        public string id { get; set; }
        public bool primary { get; set; }
    }

    public class Module
    {
        public string foldername { get; set; }
        public object fingerprint { get; set; }
        public bool invalidFingerprint { get; set; }
    }

    public class curseData
    {
        public BaseModLoader baseModLoader { get; set; }
        public bool isUnlocked { get; set; }
        public object javaArgsOverride { get; set; }
        public DateTime lastPlayed { get; set; }
        public int playedCount { get; set; }
        public Manifest manifest { get; set; }
        public DateTime fileDate { get; set; }
        public InstalledModpack installedModpack { get; set; }
        public int projectID { get; set; }
        public int fileID { get; set; }
        public object customAuthor { get; set; }
        public List<string> modpackOverrides { get; set; }
        public bool isMemoryOverride { get; set; }
        public int allocatedMemory { get; set; }
        public object profileImagePath { get; set; }
        public bool isVanilla { get; set; }
        public string guid { get; set; }
        public int gameTypeID { get; set; }
        public string installPath { get; set; }
        public string name { get; set; }
        public List<CachedScan> cachedScans { get; set; }
        public bool isValid { get; set; }
        public DateTime lastPreviousMatchUpdate { get; set; }
        public DateTime lastRefreshAttempt { get; set; }
        public bool isEnabled { get; set; }
        public string gameVersion { get; set; }
        public object gameVersionFlavor { get; set; }
        public object gameVersionTypeId { get; set; }
        public bool preferenceAlternateFile { get; set; }
        public bool preferenceAutoInstallUpdates { get; set; }
        public bool preferenceQuickDeleteLibraries { get; set; }
        public bool preferenceDeleteSavedVariables { get; set; }
        public bool preferenceProcessFileCommands { get; set; }
        public int preferenceReleaseType { get; set; }
        public object preferenceModdingFolderPath { get; set; }
        public SyncProfile syncProfile { get; set; }
        public DateTime installDate { get; set; }
        public List<InstalledAddon> installedAddons { get; set; }
        public bool wasNameManuallyChanged { get; set; }
    }

    public class SortableGameVersion
    {
        public string gameVersion { get; set; }
        public string gameVersionName { get; set; }
        public int gameVersionTypeId { get; set; }
    }

    public class SyncProfile
    {
        public bool PreferenceEnabled { get; set; }
        public bool PreferenceAutoSync { get; set; }
        public bool PreferenceAutoDelete { get; set; }
        public bool PreferenceBackupSavedVariables { get; set; }
        public string GameInstanceGuid { get; set; }
        public int SyncProfileID { get; set; }
        public object SavedVariablesProfile { get; set; }
        public DateTime LastSyncDate { get; set; }
    }


}
