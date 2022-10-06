namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;
    using Microsoft.WindowsAPICodePack.Dialogs;

    public class MicrosoftApiSelectDirectoryService : ISelectDirectoryService
    {
        #region Properties
        public string FileName { get; set; }
        public string Filter { get; set; }
        public string DirectoryName { get; private set; }
        public bool ShowNewFolderButton { get; set; }
        public string InitialDirectory { get; set; }
        public string Title { get; set; }
        #endregion

        #region ISelectDirectoryService Members
        public async Task<bool> DetermineDirectoryAsync()
        {
            using (var browserDialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Title = Title,
                    InitialDirectory = InitialDirectory
                })
            {
                if (browserDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    DirectoryName = browserDialog.FileName;
                    return true;
                }

                DirectoryName = string.Empty;
                return false;
            }
        }

        public async Task<DetermineDirectoryResult> DetermineDirectoryAsync(DetermineDirectoryContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            using (var browserDialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Title = Title,
                    InitialDirectory = context.InitialDirectory
                })
            {
                var dialogResult = browserDialog.ShowDialog();

                var result = new DetermineDirectoryResult
                {
                    Result = dialogResult == CommonFileDialogResult.Ok,
                };

                // Note: only get properties when succeeded, otherwise it will throw exceptions
                if (result.Result)
                {
                    result.DirectoryName = browserDialog.FileName;
                }

                return result;
            }
        }
        #endregion
    }
}
