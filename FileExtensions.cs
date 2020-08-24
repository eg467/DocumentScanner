using DocumentScanner.Properties;
using DocumentScanner.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Path = System.IO.Path;

namespace DocumentScanner
{
    public static class FileExtensions
    {
        public static void ShowInExplorer(this FileInfo fi)
        {
            if (fi?.Exists != true)
                return;
            string argument = "/select, \"" + fi.FullName + "\"";
            Process.Start("explorer.exe", argument);
        }

        public static bool ConfirmAction(string action) =>
            DialogResult.OK ==
            MessageBox.Show(
                $"Are you sure you want to do the following:\r\n{action}",
                "Confirm Action",
                MessageBoxButtons.OKCancel);

        public static bool SafeDelete(this FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists) return false;
            try
            {
                file.Delete();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error deleting file: " + ex.Message);
                return false;
            }
        }

        public static string SanitizeFilename(this string filename)
        {
            var sanitizedFilename = filename;
            var illegalChars = Path.GetInvalidFileNameChars().Select(c => c.ToString());
            illegalChars.ForEach(c => sanitizedFilename = sanitizedFilename.Replace(c, ""));
            return sanitizedFilename;
        }

        public static void RecursiveDelete(this DirectoryInfo baseDir)
        {
            if (baseDir is null)
            {
                throw new ArgumentNullException(nameof(baseDir));
            }

            var exceptions = new List<Exception>();
            baseDir.RecursiveDeleteHelper(exceptions);

            if (exceptions.Any())
                throw new AggregateException(
                    "An error occurred while deleting files or folders",
                    exceptions);
        }

        private static void RecursiveDeleteHelper(this DirectoryInfo baseDir, List<Exception> exceptions)
        {
            // From: https://stackoverflow.com/questions/925192/recursive-delete-of-files-and-directories-in-c-sharp
            if (!baseDir.Exists)
                return;

            baseDir.EnumerateDirectories().ForEach(RecursiveDelete);

            var files = baseDir.GetFiles();
            foreach (var file in files)
            {
                // TODO: Speed up with one failure per batch of files?
                try
                {
                    file.IsReadOnly = false;
                    file.Delete();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                baseDir.Delete();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        /// <summary>
        /// Prompts user to enter an existing file path to open.
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="path"></param>
        /// <returns>True if an existing file was chosen.</returns>
        public static bool TryGetPath(this OpenFileDialog dialog, out string path)
        {
            if (dialog is null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }

            dialog.InitialDirectory = Settings.Default.LastAccessedDirectory;
            if (DialogResult.OK != dialog.ShowDialog())
            {
                path = null;
                return false;
            }

            path = dialog.FileName;

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    "File does not exist.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                path = null;
                return false;
            }

            Settings.Default.LastAccessedDirectory = Path.GetDirectoryName(path);
            Settings.Default.Save();

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="emptyDirectory">True if all preexisting files should be deleted.</param>
        /// <returns>True to continue with the scanning operation, false to abort</returns>
        public static bool TryGetPath(this FolderBrowserDialog dialog, out string path)
        {
            if (dialog is null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }

            if (Directory.Exists(Settings.Default.LastAccessedDirectory))
                dialog.SelectedPath = Settings.Default.LastAccessedDirectory;

            path = DialogResult.OK == dialog.ShowDialog() ? dialog.SelectedPath : null;

            if (!Directory.Exists(path))
            {
                path = null;
                return false;
            }

            Settings.Default.LastAccessedDirectory = path;
            Settings.Default.Save();

            return true;
        }

        private static bool TryDelete(this DirectoryInfo dirInfo)
        {
            try
            {
                dirInfo.RecursiveDelete();
                return true;
            }
            catch (Exception ex)
            {
                var exceptionMessage = (ex is AggregateException agEx)
                    ? string.Join(Environment.NewLine, agEx.InnerExceptions.Select(e => e.Message))
                    : ex.Message;

                var deletionFailureResponse = MessageBox.Show(
                    "Select Abort to abort the operation, "
                    + "retry to retry deleting the files, "
                    + "or ignore to leave the current file in place.\r\n\r\n" +
                    exceptionMessage,
                    "Error deleting file.",
                    MessageBoxButtons.AbortRetryIgnore);

                switch (deletionFailureResponse)
                {
                    case DialogResult.Ignore:
                        return true;

                    case DialogResult.Retry:
                        return dirInfo.TryDelete();

                    default:
                        // Abort
                        return false;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="path"></param>
        /// <returns>True to continue with the scanning operation, false to abort</returns>
        public static bool TryGetPath(this SaveFileDialog dialog, out string path, bool forceOverwrite = false)
        {
            if (dialog is null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }

            dialog.InitialDirectory = Settings.Default.LastAccessedDirectory;
            path = DialogResult.OK == dialog.ShowDialog()
                    ? dialog.FileName
                    : null;

            if (path == null)
                return false;

            Settings.Default.LastAccessedDirectory = Path.GetDirectoryName(path);
            Settings.Default.Save();

            return forceOverwrite || !File.Exists(path) || dialog.ConfirmOverwrite(ref path);
        }

        /// <summary>
        /// Confirm whether the user wants to overwrite an existing
        /// path that was selected as a save path.
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="existingFile"></param>
        /// <returns>True to continue with the scanning operation, false to abort</returns>
        public static bool ConfirmOverwrite(this SaveFileDialog dialog, ref string existingFile)
        {
            var response = MessageBox.Show(
                "Select Abort to cancel this operation, Retry to choose another path, or Ignore to overwrite the file.",
                "The selected file already exists",
                MessageBoxButtons.AbortRetryIgnore);

            if (response == DialogResult.Abort)
                return false;

            if (response == DialogResult.Retry)
                return TryGetPath(dialog, out existingFile);

            try
            {
                File.Delete(existingFile);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete the file: " + ex.Message, "Error");
                return false;
            }
        }
    }

    public enum OverwriteBehavior
    {
        Abort,
        Ignore,
        DeleteWithPrompt,
        ForceDelete
    }
}