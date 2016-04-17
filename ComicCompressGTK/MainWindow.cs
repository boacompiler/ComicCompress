using System;
using System.Collections.Generic;
using Gtk;
using ComicCompressPortable.ComicClasses;
using ComicCompressGTK;

public partial class MainWindow: Gtk.Window
{
    private int pixbufWidth;
    private int pixbufWidthPrevious;

    //temporary changes to comic names
    private string prefix;
    private string postfix;
    private int frontCut;
    private int backCut;
    private int padding;
    private float newNumber;
    private string newTitle;

    private string compressionPath;
    private bool compressToParent;
    private Comic selectedComic;
    private int selectedComicIndex;

    private ComicCompresser comicCompresser;
    private ComicSeries series;

    //Treeview columns
    private TreeViewColumn pixbufColumn;
    private TreeViewColumn titleColumn;
    private TreeViewColumn numberColumn;
    private TreeViewColumn pagesColumn;
    private TreeViewColumn pathColumn;

    private CellRendererPixbuf pixbufRenderer;
    private CellRendererText textRenderer;
    private ListStore comicListStore;

    private List<Gdk.Pixbuf> thumbnails;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        comicCompresser = new ComicCompresser();
        series = new ComicSeries();

        thumbnails = new List<Gdk.Pixbuf>();

        SetupTree();

        //load settings
        pixbufWidth = ComicCompressGTK.Properties.Settings.Default.PixbufWidth;
        pixbufWidthPrevious = pixbufWidth;
        treeviewComics.GetColumn(0).Visible = ComicCompressGTK.Properties.Settings.Default.ThumbnailColumnActive;
        treeviewComics.GetColumn(1).Visible = ComicCompressGTK.Properties.Settings.Default.TitleColumnActive;
        treeviewComics.GetColumn(2).Visible = ComicCompressGTK.Properties.Settings.Default.NumberColumnActive;
        treeviewComics.GetColumn(3).Visible = ComicCompressGTK.Properties.Settings.Default.PagesColumnActive;
        treeviewComics.GetColumn(4).Visible = ComicCompressGTK.Properties.Settings.Default.PathColumnActive;
        frameCurrentComic.Visible = ComicCompressGTK.Properties.Settings.Default.CurrentComicPreviewActive;

        compressionPath = ComicCompressGTK.Properties.Settings.Default.CompressionPath;
        compressToParent = ComicCompressGTK.Properties.Settings.Default.CompressToParent;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        try
        {
            //the temporary directory used for unzipping comics is deleted
            comicCompresser.DeleteTemporaryDirectory();
        }
        catch (Exception)
        {
            MessageDialog md = new MessageDialog((Window)this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "There was an error removing temporary files at " + comicCompresser.TemporaryDirectory);
            md.Run();
        }
        
        a.RetVal = true;
    }

    void SetupTree()
    {
        //sets up the treeview
        pixbufColumn = new TreeViewColumn();
        pixbufRenderer = new CellRendererPixbuf();
        pixbufColumn.Title = "Thumbnail";
        pixbufColumn.PackStart(pixbufRenderer, false);

        titleColumn = new TreeViewColumn();
        textRenderer = new CellRendererText();
        titleColumn.Title = "Title";
        titleColumn.PackStart(textRenderer, false);

        numberColumn = new TreeViewColumn();
        numberColumn.Title = "Number";
        numberColumn.PackStart(textRenderer, false);

        pagesColumn = new TreeViewColumn();
        pagesColumn.Title = "Pages";
        pagesColumn.PackStart(textRenderer, false);

        pathColumn = new TreeViewColumn();
        pathColumn.Title = "Path";
        pathColumn.PackStart(textRenderer, false);

        treeviewComics.AppendColumn(pixbufColumn);
        treeviewComics.AppendColumn(titleColumn);
        treeviewComics.AppendColumn(numberColumn);
        treeviewComics.AppendColumn(pagesColumn);
        treeviewComics.AppendColumn(pathColumn);

        pixbufColumn.AddAttribute(pixbufRenderer,"pixbuf",0);
        titleColumn.AddAttribute(textRenderer,"markup",1);
        numberColumn.AddAttribute(textRenderer, "markup", 2);
        pagesColumn.AddAttribute(textRenderer, "text", 3);
        pathColumn.AddAttribute(textRenderer, "text", 4);

        comicListStore = new ListStore(typeof(Gdk.Pixbuf), typeof(string), typeof(string), typeof(string), typeof(string));
        treeviewComics.Model = comicListStore;
    }

    void RefreshTree()
    {
        //this refreshes the treeview
        //this also displays any temporary changes made to the comics while editing
        //temporary changes have been coloured using markup

        comicListStore.Clear();
        if (checkbuttonNumbered.Active)
        {
            //this calculates how many leading zeroes are needed for numbering comics
            padding = (series.Comics.Count + 1).ToString().Length;//TODO add custom formatting settings
        }

        for (int i = 0; i < series.Comics.Count; i++)
        {
            string title = series.Comics[i].Title;
            string number = series.Comics[i].Number.ToString();

            //Single Comic Changes
            if (series.Comics[i] == selectedComic)
            {
                selectedComicIndex = i;
                if (title != newTitle && frontCut == 0 && backCut == 0)
                {
                    title = "<span foreground='blue'>" + newTitle + "</span>";
                }
                if (number != newNumber.ToString())
                {
                    number = "<span foreground='red'>" + newNumber + "</span>";
                }

            }
            //Change numbers of comic after the selected comic when numbered neighbours is enabled
            if (checkbuttonNumberNeighbors.Active && i > selectedComicIndex && newNumber != selectedComic.Number)
            {
                number = "<span foreground='red'>" + (newNumber + (i - selectedComicIndex)) + "</span>";
            }

            //All Comics Changes
            //cut from front
            if (!(frontCut > title.Length))
            {
                title = title.Substring(frontCut, title.Length - frontCut);
            }
            else
            {
                title = "";
            }
            //cut from back
            if (!(backCut > title.Length))
            {
                title = title.Substring(0, title.Length - backCut);
            }
            else
            {
                title = "";
            }

            //adding prefix/postfix
            title = "<span foreground='red'>" + prefix + "</span>" + title + "<span foreground='red'>" + postfix + "</span>";

            //adding comic number
            if (checkbuttonNumbered.Active)
            {
                title = "<span foreground='green'>" + series.Comics[i].Number.ToString().PadLeft(padding, '0') + "</span> - " + title;
            }

            //Only append thumbnails if their column is visible
            if (treeviewComics.GetColumn(0).Visible)
            {
                comicListStore.AppendValues(thumbnails[i], title, number, series.Comics[i].Pages.Count.ToString(), series.Comics[i].ComicPath);
            }
            else
            {
                comicListStore.AppendValues(null, title, number, series.Comics[i].Pages.Count.ToString(), series.Comics[i].ComicPath);
            }

        }

        treeviewComics.Model = comicListStore;
    }

    public void RefreshThumbnails()
    {
        //only perform this if the thumbnails column is visible
        if (treeviewComics.GetColumn(0).Visible)
        {
            if (pixbufWidth != pixbufWidthPrevious)
            {
                //resizing
                // if the thumbnail needs to be scaled up we reload it to avoid low fidelity images
                //reinitialising thumbnails as new list means the for loop does not run, and the if lower down is run.
                if (pixbufWidth > pixbufWidthPrevious)
                {
                    thumbnails = new List<Gdk.Pixbuf>();
                }

                for (int i = 0; i < thumbnails.Count; i++)
                {
                    float ratio = (float)thumbnails[i].Width / (float)thumbnails[i].Height;
                    thumbnails[i] = thumbnails[i].ScaleSimple(pixbufWidth, (int)Math.Round(pixbufWidth / ratio), Gdk.InterpType.Bilinear);
                }

                pixbufWidthPrevious = pixbufWidth;
            }

            if (thumbnails.Count != series.Comics.Count)
            {
                //fetching thumbnails
                thumbnails = new List<Gdk.Pixbuf>();
                for (int i = 0; i < series.Comics.Count; i++)
                {
                    thumbnails.Add(new Gdk.Pixbuf(series.Comics[i].Pages[0].PagePath));
                    //resize
                    float ratio = (float)thumbnails[i].Width / (float)thumbnails[i].Height;
                    thumbnails[i] = thumbnails[i].ScaleSimple(pixbufWidth, (int)Math.Round(pixbufWidth / ratio), Gdk.InterpType.Bilinear);
                }
            }
        }
    }

    void RefreshFormControls()
    {
        RefreshThumbnails();

        RefreshTree();

        entrySeriesTitle.Text = series.Title;
    }

    protected void OnTreeviewComicsCursorChanged(object sender, EventArgs e)
    {
        selectedComic = series.Comics[treeviewComics.Selection.GetSelectedRows()[0].Indices[0]];

        this.comicviewwidget1.SetComic(selectedComic);

        entryTitle.Text = selectedComic.Title;
        spinbuttonNumber.Value = selectedComic.Number;
        checkbuttonNumberNeighbors.Active = false;
    }

    #region Toolbar Actions

    protected void OnOpenActionActivated(object sender, EventArgs e)
    {
        FileChooserDialog folderDialog = new FileChooserDialog("Choose a comic folder to load",this,FileChooserAction.SelectFolder,"Cancel",ResponseType.Cancel,"Open",ResponseType.Accept);
        int response = folderDialog.Run();
        if (response == (int)ResponseType.Accept)
        {

            try
            {
                series = comicCompresser.GetComicSeriesFromDirectory(folderDialog.Filename);
                RefreshFormControls();
            }
            catch (Exception)
            {
                MessageDialog md = new MessageDialog((Window)this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Not a valid comic");
                md.Run();
                md.Destroy();

                series = new ComicSeries();
                selectedComic = null;
                RefreshFormControls();
            }
            
        }

        folderDialog.Destroy();

    }    protected void OnAddComicActionActivated(object sender, EventArgs e)
    {
        FileChooserDialog folderDialog = new FileChooserDialog("Choose a comic folder to load", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        int response = folderDialog.Run();
        if (response == (int)ResponseType.Accept)
        {

            try
            {
                series.Comics.AddRange(comicCompresser.GetComicSeriesFromDirectory(folderDialog.Filename).Comics);

                if (series.Title == null)
                {
                    series.Title = System.IO.Path.GetFileNameWithoutExtension(folderDialog.Filename);

                }

                RefreshFormControls();
            }
            catch (Exception)
            {

                MessageDialog md = new MessageDialog((Window)this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Not a valid comic");
                md.Run();
                md.Destroy();

                series = new ComicSeries();
                selectedComic = null;
                RefreshFormControls();
            }

            

        }

        folderDialog.Destroy();
    }

    protected void OnImportCompressedComicActionActivated(object sender, EventArgs e)
    {
        FileChooserDialog folderDialog = new FileChooserDialog("Choose a comic archive to load", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        int response = folderDialog.Run();
        if (response == (int)ResponseType.Accept)
        {
            try
            {
                series.Comics.AddRange(comicCompresser.GetComicSeriesFromArchive(folderDialog.Filename).Comics);

                if (series.Title == null)
                {
                    series.Title = System.IO.Path.GetFileNameWithoutExtension(folderDialog.Filename);

                }
                RefreshFormControls();
            }
            catch (Exception)
            {

                MessageDialog md = new MessageDialog((Window)this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Not a valid comic");
                md.Run();
                md.Destroy();

                series = new ComicSeries();
                selectedComic = null;
                RefreshFormControls();
            }
            

        }

        folderDialog.Destroy();
    }

    protected void OnClearComicsActionActivated(object sender, EventArgs e)
    {
        series = new ComicSeries();
        selectedComic = null;
        RefreshFormControls();
    }
    protected void OnPreferencesActionActivated(object sender, EventArgs e)
    {
        PreferencesDialog preferencesDialog = new PreferencesDialog();

        preferencesDialog.DisplayWidget.SpinButtonWidth = pixbufWidth;
        preferencesDialog.DisplayWidget.CheckbuttonThumbnail = treeviewComics.GetColumn(0).Visible;
        preferencesDialog.DisplayWidget.CheckbuttonTitle = treeviewComics.GetColumn(1).Visible;
        preferencesDialog.DisplayWidget.CheckbuttonNumber = treeviewComics.GetColumn(2).Visible;
        preferencesDialog.DisplayWidget.CheckbuttonPages = treeviewComics.GetColumn(3).Visible;
        preferencesDialog.DisplayWidget.CheckbuttonPath = treeviewComics.GetColumn(4).Visible;
        preferencesDialog.DisplayWidget.CheckbuttonPreview = frameCurrentComic.Visible;

        preferencesDialog.PathWidget.CheckbuttonCompressionParentDirectory = compressToParent;
        preferencesDialog.PathWidget.CompressionPath = compressionPath;

        preferencesDialog.Response += delegate (object o, ResponseArgs ra)
        {
            if (ra.ResponseId == ResponseType.Apply)
            {
                pixbufWidth = preferencesDialog.DisplayWidget.SpinButtonWidth;
                treeviewComics.GetColumn(0).Visible = preferencesDialog.DisplayWidget.CheckbuttonThumbnail;
                treeviewComics.GetColumn(1).Visible = preferencesDialog.DisplayWidget.CheckbuttonTitle;
                treeviewComics.GetColumn(2).Visible = preferencesDialog.DisplayWidget.CheckbuttonNumber;
                treeviewComics.GetColumn(3).Visible = preferencesDialog.DisplayWidget.CheckbuttonPages;
                treeviewComics.GetColumn(4).Visible = preferencesDialog.DisplayWidget.CheckbuttonPath;

                if (treeviewComics.GetColumn(0).Visible)
                { 
                    //This resizes the column to match the width of the thumbnails. It's a little hacky
                    treeviewComics.GetColumn(0).Visible = false;
                    treeviewComics.GetColumn(0).Visible = true;
                }

                frameCurrentComic.Visible = preferencesDialog.DisplayWidget.CheckbuttonPreview;

                compressToParent = preferencesDialog.PathWidget.CheckbuttonCompressionParentDirectory;
                compressionPath = preferencesDialog.PathWidget.CompressionPath;

                RefreshFormControls();

                ComicCompressGTK.Properties.Settings.Default.PixbufWidth = pixbufWidth;
                ComicCompressGTK.Properties.Settings.Default.ThumbnailColumnActive = treeviewComics.GetColumn(0).Visible;
                ComicCompressGTK.Properties.Settings.Default.TitleColumnActive = treeviewComics.GetColumn(1).Visible;
                ComicCompressGTK.Properties.Settings.Default.NumberColumnActive = treeviewComics.GetColumn(2).Visible;
                ComicCompressGTK.Properties.Settings.Default.PagesColumnActive = treeviewComics.GetColumn(3).Visible;
                ComicCompressGTK.Properties.Settings.Default.PathColumnActive = treeviewComics.GetColumn(4).Visible;
                ComicCompressGTK.Properties.Settings.Default.CurrentComicPreviewActive = frameCurrentComic.Visible;
                ComicCompressGTK.Properties.Settings.Default.CompressionPath = compressionPath;
                ComicCompressGTK.Properties.Settings.Default.CompressToParent = compressToParent;
                ComicCompressGTK.Properties.Settings.Default.Save();
            }
            else
            {
                preferencesDialog.Destroy();
            }
        };
        preferencesDialog.Run();  
    }

    protected void OnComicActionActivated(object sender, EventArgs e)
    {
        //TODO a better comic viewer would be nice
        if (selectedComic != null)
        {
            Dialog dialog = new Dialog();
            ComicViewWidget cvw = new ComicViewWidget();
            cvw.SetMaxBounds(500, 500);
            cvw.SetComic(selectedComic);

            dialog.VBox.Add(cvw);
            dialog.VBox.PackStart(cvw, true, true, 0);
            cvw.Show();
            dialog.Resizable = false;
            dialog.Show();
        }

    }
    #endregion

    #region Single Comic Actions

    protected void OnButtonOpenPathClicked(object sender, EventArgs e)
    {
        if (selectedComic != null)
        {
            System.Diagnostics.Process.Start(selectedComic.ComicPath);
        }
    }

    protected void OnEntryTitleChanged(object sender, EventArgs e)
    {
        if (selectedComic != null)
        {
            if (entryTitle.Text == selectedComic.Title)
            {
                newTitle = selectedComic.Title;
            }
            else
            {
                newTitle = entryTitle.Text;
                RefreshFormControls();
            }
        }
    }

    protected void OnSpinbuttonNumberValueChanged(object sender, EventArgs e)
    {
        if (selectedComic != null)
        {
            if (spinbuttonNumber.Value == selectedComic.Number)
            {
                newNumber = selectedComic.Number;
            }
            else
            {
                newNumber = (float)spinbuttonNumber.Value;
                RefreshFormControls();
            }
        }
    }

    protected void OnCheckbuttonNumberNeighborsToggled(object sender, EventArgs e)
    {
        RefreshFormControls();
    }

    protected void OnButtonSingleClearClicked(object sender, EventArgs e)
    {
        if (selectedComic != null)
        {
            entryTitle.Text = selectedComic.Title;
            spinbuttonNumber.Value = selectedComic.Number;
            checkbuttonNumberNeighbors.Active = false;
            RefreshFormControls();
        }
    }

    protected void OnButtonSingleApplyClicked(object sender, EventArgs e)
    {
        for (int i = 0; i < series.Comics.Count; i++)
        {
            if (series.Comics[i] == selectedComic)
            {
                selectedComicIndex = i;
                if (selectedComic.Title != newTitle)
                {
                    selectedComic.Title = newTitle;
                }
                if (selectedComic.Number != newNumber)
                {
                    selectedComic.Number = newNumber;
                }

            }
            if (checkbuttonNumberNeighbors.Active && i > selectedComicIndex)
            {
                series.Comics[i].Number = newNumber + (i - selectedComicIndex);
            }

        }
        RefreshFormControls();

    }

    #endregion

    #region All Comic Actions
    protected void OnEntryPostfixChanged(object sender, EventArgs e)
    {
        postfix = entryPostfix.Text;
        RefreshFormControls();
    }

    protected void OnEntryPrefixChanged(object sender, EventArgs e)
    {
        prefix = entryPrefix.Text;
        RefreshFormControls();
    }
        
    protected void OnSpinbuttonFrontValueChanged(object sender, EventArgs e)
    {
        frontCut = (int)spinbuttonFront.Value;
        RefreshFormControls();
    }

    protected void OnSpinbuttonBackValueChanged(object sender, EventArgs e)
    {
        backCut = (int)spinbuttonBack.Value;
        RefreshFormControls();
    }

    protected void OnCheckbuttonNumberedToggled(object sender, EventArgs e)
    {
        RefreshFormControls();
    }

    protected void OnButtonApplyClicked(object sender, EventArgs e)
    {
        for (int i = 0; i < series.Comics.Count; i++)
        {

            string title = series.Comics[i].Title;

            if (!(frontCut > title.Length))
            {
                title = title.Substring(frontCut, title.Length - frontCut);
            }
            else
            {
                title = "";
            }
            if (!(backCut > title.Length))
            {
                title = title.Substring(0, title.Length - backCut);
            }
            else
            {
                title = "";
            }

            title = prefix + title +  postfix;

            if (checkbuttonNumbered.Active)
            {
                title = series.Comics[i].Number.ToString().PadLeft(padding, '0') + " - " + title;
            }

            series.Comics[i].Title = title;

            
        }
        spinbuttonBack.Value = 0;
        spinbuttonFront.Value = 0;
        entryPostfix.Text = "";
        entryPrefix.Text = "";
        checkbuttonNumbered.Active = false;
        if (selectedComic != null)
        {
            newTitle = selectedComic.Title;
        }
        series.Title = entrySeriesTitle.Text;
        RefreshFormControls();
    }

    #endregion

    protected void OnButtonCompressClicked(object sender, EventArgs e)
    {
        switch (comboboxCompressOption.Active)
        {
            //Compress all comics separately
            default:
            case 0:
                if (compressToParent)
                {
                    comicCompresser.CompressSeries(series, false);
                }
                else
                {
                    comicCompresser.CompressSeries(series, false, compressionPath);
                }
                
                break;
            //Compress all comics as a single file
            case 1:
                if (compressToParent)
                {
                    comicCompresser.CompressSeries(series, true);
                }
                else
                {
                    comicCompresser.CompressSeries(series, true, compressionPath);
                }
                break;
            //Compress selected comic
            case 2:
                if (compressToParent)
                {
                    comicCompresser.CompressComic(selectedComic);
                }
                else
                {
                    comicCompresser.CompressComic(selectedComic, compressionPath);
                }
                
                break;
        }
    }
        
}
