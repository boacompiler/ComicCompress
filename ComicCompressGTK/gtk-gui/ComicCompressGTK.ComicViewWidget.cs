
// This file has been generated by the GUI designer. Do not modify.
namespace ComicCompressGTK
{
	public partial class ComicViewWidget
	{
		private global::Gtk.VBox vbox2;
		
		private global::Gtk.Image imageComicPage;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Arrow arrow2;
		
		private global::Gtk.SpinButton spinbutton1;
		
		private global::Gtk.Arrow arrow1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ComicCompressGTK.ComicViewWidget
			global::Stetic.BinContainer.Attach (this);
			this.CanFocus = true;
			this.Name = "ComicCompressGTK.ComicViewWidget";
			// Container child ComicCompressGTK.ComicViewWidget.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.imageComicPage = new global::Gtk.Image ();
			this.imageComicPage.Name = "imageComicPage";
			this.vbox2.Add (this.imageComicPage);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.imageComicPage]));
			w1.Position = 1;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Homogeneous = true;
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.arrow2 = new global::Gtk.Arrow (((global::Gtk.ArrowType)(2)), ((global::Gtk.ShadowType)(2)));
			this.arrow2.Name = "arrow2";
			this.arrow2.Xalign = 1F;
			this.hbox1.Add (this.arrow2);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.arrow2]));
			w2.Position = 0;
			w2.Expand = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.spinbutton1 = new global::Gtk.SpinButton (0D, 100D, 1D);
			this.spinbutton1.CanFocus = true;
			this.spinbutton1.Name = "spinbutton1";
			this.spinbutton1.Adjustment.PageIncrement = 10D;
			this.spinbutton1.ClimbRate = 1D;
			this.spinbutton1.Numeric = true;
			this.hbox1.Add (this.spinbutton1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.spinbutton1]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.arrow1 = new global::Gtk.Arrow (((global::Gtk.ArrowType)(3)), ((global::Gtk.ShadowType)(2)));
			this.arrow1.Name = "arrow1";
			this.arrow1.Xalign = 0F;
			this.hbox1.Add (this.arrow1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.arrow1]));
			w4.Position = 2;
			w4.Expand = false;
			this.vbox2.Add (this.hbox1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			this.Add (this.vbox2);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.spinbutton1.ValueChanged += new global::System.EventHandler (this.OnSpinbutton1ValueChanged);
		}
	}
}
