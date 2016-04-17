using System;
using ComicCompressGTK;
using ComicCompressPortable.ComicClasses;
using Gtk;

namespace ComicCompressGTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ComicViewWidget : Gtk.Bin
    {
        private Comic comic;
        private int currentPage;
        //private int pixbufHeight;
        private int maxPixbufWidth;
        private int maxPixbufHeight;

        public ComicViewWidget()
        {
            this.Build();
            currentPage = 0;
            //pixbufHeight = 250;

            maxPixbufWidth = 250;
            maxPixbufHeight = 250;
            imageComicPage.SetSizeRequest(maxPixbufWidth,maxPixbufHeight);
        }

        public void SetComic(Comic newComic)
        {
            comic = newComic;

            spinbutton1.SetRange(1,comic.Pages.Count);

            GoToPage(0);
        }

        public void SetMaxBounds(int maxWidth, int maxHeight)
        {
            maxPixbufWidth = maxWidth;
            maxPixbufHeight = maxHeight;
            imageComicPage.SetSizeRequest(maxPixbufWidth, maxPixbufHeight);
        }

        private void SetImageFromPath(string path)
        {
            if (imageComicPage.Pixbuf != null)
            {
                imageComicPage.Pixbuf.Dispose();
            }
            
            Gdk.Pixbuf image = new Gdk.Pixbuf(path);

            float ratio = (float)image.Width / (float)image.Height;

            if (ratio >= 1) //if ratio is more than one, width must be larger than height
            {
                image = image.ScaleSimple(maxPixbufWidth, (int)Math.Round(maxPixbufWidth / ratio), Gdk.InterpType.Bilinear);
            }
            else
            {
                image = image.ScaleSimple((int)Math.Round(maxPixbufHeight * ratio), maxPixbufHeight, Gdk.InterpType.Bilinear);
            }

            imageComicPage.Pixbuf = image;
            image.Dispose();
        }

        public void GoToPage(int page)
        {
            if (comic != null)
            {
                if (page >= 0 && page < comic.Pages.Count)
                {
                    SetImageFromPath(comic.Pages[page].PagePath);
                    currentPage = page;
                    spinbutton1.Value = currentPage + 1;
                }
            }
            
        }

        protected void OnSpinbutton1ValueChanged(object sender, EventArgs e)
        {
            GoToPage((int)spinbutton1.Value - 1);
        }
            
    }
}

