// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.IO;
    using Catel;
    using Catel.Logging;

    public static class DirectoryHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs = true, bool overwriteExisting = false)
        {
            Argument.IsNotNullOrWhitespace(() => sourceDirName);
            Argument.IsNotNullOrWhitespace(() => destDirName);

            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            Log.Debug("Copying directory '{0}' to '{1}'", sourceDirName, destDirName);

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);

                if (File.Exists(temppath) && !overwriteExisting)
                {
                    Log.Debug("Skipping copying of '{0}', file already exists in target director", file.FullName);
                    continue;
                }

                file.CopyTo(temppath, overwriteExisting);
            }

            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, copySubDirs, overwriteExisting);
                }
            }
        }
    }
}