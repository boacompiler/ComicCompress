using System;
using Gtk;

namespace ComicCompressGTK
{
    class MainClass
    {
        public static MainWindow window;
        public static void Main(string[] args)
        {
            Application.Init();
            window = new MainWindow();
            window.Show();
            Application.Run();
        }
    }
}
