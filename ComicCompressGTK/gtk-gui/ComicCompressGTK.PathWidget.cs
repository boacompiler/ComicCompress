
// This file has been generated by the GUI designer. Do not modify.
namespace ComicCompressGTK
{
	public partial class PathWidget
	{
		private global::Gtk.Table table1;
		
		private global::Gtk.Button buttonBrowse;
		
		private global::Gtk.CheckButton checkbuttonCompressionParentDirectory;
		
		private global::Gtk.Entry entryCompressionPath;
		
		private global::Gtk.Label label1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ComicCompressGTK.PathWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "ComicCompressGTK.PathWidget";
			// Container child ComicCompressGTK.PathWidget.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(3)), ((uint)(3)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.buttonBrowse = new global::Gtk.Button ();
			this.buttonBrowse.CanFocus = true;
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.UseUnderline = true;
			this.buttonBrowse.Label = global::Mono.Unix.Catalog.GetString ("Browse");
			this.table1.Add (this.buttonBrowse);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonBrowse]));
			w1.TopAttach = ((uint)(2));
			w1.BottomAttach = ((uint)(3));
			w1.LeftAttach = ((uint)(2));
			w1.RightAttach = ((uint)(3));
			w1.XOptions = ((global::Gtk.AttachOptions)(4));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.checkbuttonCompressionParentDirectory = new global::Gtk.CheckButton ();
			this.checkbuttonCompressionParentDirectory.CanFocus = true;
			this.checkbuttonCompressionParentDirectory.Name = "checkbuttonCompressionParentDirectory";
			this.checkbuttonCompressionParentDirectory.Label = global::Mono.Unix.Catalog.GetString ("Parent Directory");
			this.checkbuttonCompressionParentDirectory.Active = true;
			this.checkbuttonCompressionParentDirectory.DrawIndicator = true;
			this.checkbuttonCompressionParentDirectory.UseUnderline = true;
			this.table1.Add (this.checkbuttonCompressionParentDirectory);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.checkbuttonCompressionParentDirectory]));
			w2.TopAttach = ((uint)(1));
			w2.BottomAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryCompressionPath = new global::Gtk.Entry ();
			this.entryCompressionPath.CanFocus = true;
			this.entryCompressionPath.Name = "entryCompressionPath";
			this.entryCompressionPath.IsEditable = true;
			this.entryCompressionPath.InvisibleChar = '●';
			this.table1.Add (this.entryCompressionPath);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryCompressionPath]));
			w3.TopAttach = ((uint)(2));
			w3.BottomAttach = ((uint)(3));
			w3.RightAttach = ((uint)(2));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 0F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Compress Comics to:");
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			this.Add (this.table1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.checkbuttonCompressionParentDirectory.Toggled += new global::System.EventHandler (this.OnCheckbuttonCompressionParentDirectoryToggled);
			this.buttonBrowse.Clicked += new global::System.EventHandler (this.OnButtonBrowseClicked);
		}
	}
}