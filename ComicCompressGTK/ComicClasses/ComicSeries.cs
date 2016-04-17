using System;
using System.Collections.Generic;

namespace ComicCompressPortable.ComicClasses
{
    public class ComicSeries
    {
        private string title;
        private List<Comic> comics;

        public ComicSeries()
        {
            comics = new List<Comic>();
        }

        public ComicSeries(string title)
        {
            Title = title;
            comics = new List<Comic>();
        }

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

        internal List<Comic> Comics
        {
            get
            {
                return comics;
            }

            set
            {
                comics = value;
            }
        }
        public void AddComic(Comic comic)
        {
            comic.Number = comics.Count + 1;
            comics.Add(comic);
        }
        public void RemoveComic(Comic comic)
        {
            if (comics.Contains(comic))
            {
                comics.Remove(comic);
            }     
        }
        public void MoveComic(Comic comic, int destination)
        {
            throw new NotImplementedException();//TODO
        }
    }
}
