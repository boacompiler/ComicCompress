using System;
using System.Collections.Generic;
using System.IO;

namespace ComicCompressPortable.ComicClasses
{
    public class Comic
    {
        private String title;
        private float number;
        private List<ComicPage> pages;
        private string comicPath;

        public Comic(string title,int number,string path)
        {
            Title = title;
            Number = number;
            ComicPath = path;
            pages = new List<ComicPage>();
        }

        #region Getters And Setters
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public float Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }

        internal List<ComicPage> Pages
        {
            get
            {
                return pages;
            }

            set
            {
                pages = value;
            }
        }

        public string ComicPath
        {
            get
            {
                return comicPath;
            }

            set
            {
                comicPath = value;
            }
        }
        public List<string> PagePaths
        {
            get
            {
                List<string> pathList = new List<string>();
                for (int i = 0; i < pages.Count; i++)
                {
                    pathList.Add(pages[i].PagePath);
                }
                return pathList;
            }

        }
        #endregion

        /// <summary>
        /// Initialises a new comic page from an image file and adds it to the comic
        /// </summary>
        /// <param name="path">the path of the page image</param>
        public void AddPage(string path)
        {
            ComicPage page = new ComicPage(path, pages.Count + 1);
            pages.Add(page);
        }
        /// <summary>
        /// Initialises comic pages from image files in the comics path
        /// </summary>
        /// <param name="comicCompresser">an initialised comicCompresser is required for validating the image files</param>
        public void GeneratePagesFromPath(ComicCompresser comicCompresser)
        {
            //TODO this functionality can probably be moved to comiccompressor and called here
            pages = new List<ComicPage>();
            string[] files = Directory.GetFiles(comicPath);
            for (int i = 0; i < files.Length; i++)
            {
                if (comicCompresser.CheckImage(files[i]))
                {
                    AddPage(files[i]);
                }
            }
        }
    }
}
