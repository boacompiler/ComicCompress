using System;

namespace ComicCompressGTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DisplayWidget : Gtk.Bin
    {
        public DisplayWidget()
        {
            this.Build();
            
        }

        public bool CheckbuttonThumbnail
        {
            get
            {
                return checkbuttonThumbnail.Active;
            }

            set
            {
                checkbuttonThumbnail.Active = value;
            }
        }

        public bool CheckbuttonTitle
        {
            get
            {
                return checkbuttonTitle.Active;
            }

            set
            {
                checkbuttonTitle.Active = value;
            }
        }

        public bool CheckbuttonNumber
        {
            get
            {
                return checkbuttonNumber.Active;
            }

            set
            {
                checkbuttonNumber.Active = value;
            }
        }

        public bool CheckbuttonPages
        {
            get
            {
                return checkbuttonPages.Active;
            }

            set
            {
                checkbuttonPages.Active = value;
            }
        }

        public bool CheckbuttonPath
        {
            get
            {
                return checkbuttonPath.Active;
            }

            set
            {
                checkbuttonPath.Active = value;
            }
        }

        public int SpinButtonWidth
        {
            get
            {
                return (int)spinbuttonWidth.Value;
            }

            set
            {
                spinbuttonWidth.Value = value;
            }
        }

        public bool CheckbuttonPreview
        {
            get
            {
                return checkbuttonPreview.Active;
            }

            set
            {
                checkbuttonPreview.Active = value;
            }
        }

    }
}

