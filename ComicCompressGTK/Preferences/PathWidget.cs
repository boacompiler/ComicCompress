using System;
using Gtk;

namespace ComicCompressGTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class PathWidget : Gtk.Bin
    {
        private string compressionPath;

        public PathWidget()
        {
            this.Build();
            entryCompressionPath.Sensitive = !checkbuttonCompressionParentDirectory.Active;
            buttonBrowse.Sensitive = !checkbuttonCompressionParentDirectory.Active;
        }

        protected void OnButtonBrowseClicked(object sender, EventArgs e)
        {
            FileChooserDialog folderDialog = new FileChooserDialog("Choose a comic folder to load",null,FileChooserAction.SelectFolder,"Cancel",ResponseType.Cancel,"Open",ResponseType.Accept);
            int response = folderDialog.Run();
            if (response == (int)ResponseType.Accept)
            {
                compressionPath = folderDialog.Filename;
                entryCompressionPath.Text = compressionPath;
            }

            folderDialog.Destroy();
        }

        protected void OnCheckbuttonCompressionParentDirectoryToggled(object sender, EventArgs e)
        {
            entryCompressionPath.Sensitive = !checkbuttonCompressionParentDirectory.Active;
            buttonBrowse.Sensitive = !checkbuttonCompressionParentDirectory.Active;
        }

        public bool CheckbuttonCompressionParentDirectory
        {
            get
            {
                return checkbuttonCompressionParentDirectory.Active;
            }

            set
            {
                checkbuttonCompressionParentDirectory.Active = value;
            }
        }

        public string CompressionPath
        {
            get
            {
                return entryCompressionPath.Text;
            }

            set
            {
                entryCompressionPath.Text = value;
            }
        }
    }
}

