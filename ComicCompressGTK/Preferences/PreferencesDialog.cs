using System;
using System.Collections.Generic;
using Gtk;

namespace ComicCompressGTK
{
    public partial class PreferencesDialog : Gtk.Dialog
    {
        TreeViewColumn optionsColumn;
        CellRendererText textrenderer;
        ListStore optionsListStore;

        DisplayWidget displayWidget;
        PathWidget pathWidget;
        MiscWidget miscWidget;

        List<Widget> options;

        public DisplayWidget DisplayWidget
        {
            get
            {
                return displayWidget;
            }

            set
            {
                displayWidget = value;
            }
        }

        public PathWidget PathWidget
        {
            get
            {
                return pathWidget;
            }

            set
            {
                pathWidget = value;
            }
        }

        public MiscWidget MiscWidget
        {
            get
            {
                return miscWidget;
            }

            set
            {
                miscWidget = value;
            }
        }

        public PreferencesDialog()
        {
            this.Build();

            displayWidget = new DisplayWidget();
            pathWidget = new PathWidget();
            miscWidget = new MiscWidget();

            options = new List<Widget>();

            options.Add(displayWidget);
            options.Add(pathWidget);
            options.Add(miscWidget);

            SetupTree();

            for (int i = 0; i < options.Count; i++)
            {
                vboxContent.Add(options[i]);
            }

            displayWidget.Show();

        }        

        void SetupTree()
        {
            optionsColumn = new TreeViewColumn();
            textrenderer = new CellRendererText();
            optionsColumn.Title = "Options";
            optionsColumn.PackStart(textrenderer,false);

            treeviewPreferences.AppendColumn(optionsColumn);
            optionsColumn.AddAttribute(textrenderer,"text",0);

            optionsListStore = new ListStore(typeof(string));

            optionsListStore.AppendValues("Display");
            optionsListStore.AppendValues("Path");
            optionsListStore.AppendValues("Misc");

            treeviewPreferences.Model = optionsListStore;
        }

        protected void OnTreeviewPreferencesCursorChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < options.Count; i++)
            {
                options[i].Hide();
            }
            options[treeviewPreferences.Selection.GetSelectedRows()[0].Indices[0]].Show();
        }
    }
}

