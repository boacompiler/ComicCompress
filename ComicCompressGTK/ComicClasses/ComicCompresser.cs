using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

namespace ComicCompressPortable.ComicClasses
{
    public class ComicCompresser
    {
        private List<string> imageExtensions;
        private string temporaryDirectory;

        public string TemporaryDirectory
        {
            get
            {
                return temporaryDirectory;
            }

            set
            {
                temporaryDirectory = value;
            }
        }

        public ComicCompresser()
        {
            //supported image extensions
            imageExtensions = new List<string>() { ".jpg", ".png", "jpeg", ".bmp", ".gif" };//TODO , ".tif" does not seem to work
            temporaryDirectory = "Temp";
        }
        /// <summary>
        /// Checks if a file is an image supported by comic compresser
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>Returns True if the file is an image (".jpg", ".png", "jpeg", ".bmp", ".gif")</returns>
        public bool CheckImage(string path)
        {
            if (path.Length > 4)
            {
                string ext = path.Substring(path.Length - 4);

                if (imageExtensions.Contains(ext))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a directory is a valid Comic
        /// A valid comic is a directory that contains at least one image
        /// </summary>
        /// <param name="path">the directory to check</param>
        /// <returns>Returns True if directory is a valid comic</returns>
        public bool DirectoryIsComic(string path)
        {
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                if (CheckImage(files[i]))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a directory is a valid Comic series
        /// A valid series is a directory that contains at least one comic
        /// (comics are directories that contain at least one image)
        /// </summary>
        /// <param name="path">the directory to check</param>
        /// <returns>Returns True if directory is a valid comic series</returns>
        public bool DirectoryIsComicSeries(string path)
        {
            string[] directories = Directory.GetDirectories(path);
            for (int i = 0; i < directories.Length; i++)
            {
                if (DirectoryIsComic(path))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Initializes a comic from a given directory
        /// </summary>
        /// <param name="path">directory of the comic</param>
        /// <returns>Returns an initialized comic class</returns>
        public Comic GetComicFromDirectory(string path)
        {
            Comic comic = new Comic(Path.GetFileName(path), 0, path);
            comic.GeneratePagesFromPath(this);
            return comic;
        }
        /// <summary>
        /// Initializes a comic series from a .cbz file
        /// </summary>
        /// <param name="path">path of the .cbz</param>
        /// <returns></returns>
        public ComicSeries GetComicSeriesFromArchive(string path)
        {
            DeCompressComic(path);
            return GetComicSeriesFromDirectory(temporaryDirectory + Path.DirectorySeparatorChar + Path.GetFileName(path));
        }
        /// <summary>
        /// Initializes a comic series from a directory
        /// </summary>
        /// <param name="path">directory of the comic series</param>
        /// <returns></returns>
        public ComicSeries GetComicSeriesFromDirectory(string path)//TODO should this be in series itself?
        {
            ComicSeries series = new ComicSeries(Path.GetFileName(path));
            string[] directories = Directory.GetDirectories(path);

            if (directories.Length < 1)
            {
                if (DirectoryIsComic(path))
                {
                    series.AddComic(GetComicFromDirectory(path));
                }                
            }
            else
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    series.AddComic(GetComicFromDirectory(directories[i]));
                    
                }
            }

            return series;
        }
        /// <summary>
        /// Compress a single comic to a given directory
        /// </summary>
        /// <param name="comic">the comic to compress</param>
        /// <param name="path">the directory to compress to</param>
        public void CompressComic(Comic comic, string path)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFiles(comic.PagePaths, "");
                zip.Save(path + Path.DirectorySeparatorChar + comic.Title +".cbz");
            }
        } 
        /// <summary>
        /// Compress a single comic to it's parent directory
        /// </summary>
        /// <param name="comic"> the comic to compress</param>
        public void CompressComic(Comic comic)
        {
            CompressComic(comic, Directory.GetParent(comic.ComicPath).FullName);
        }
        /// <summary>
        /// Decompresses a comic to the temporary directory
        /// </summary>
        /// <param name="path">path of the comic archive</param>
        public void DeCompressComic(string path)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                zip.ExtractAll(temporaryDirectory + Path.DirectorySeparatorChar + Path.GetFileName(path),ExtractExistingFileAction.OverwriteSilently);
            }
        }
        /// <summary>
        /// Compress multiple comics
        /// </summary>
        /// <param name="series">the series of comics to compress</param>
        /// <param name="singleFile">Compile the comics into a single large file or separate files</param>
        /// <param name="path">the path to compress to</param>
        public void CompressSeries(ComicSeries series, bool singleFile, string path)
        {
            if (singleFile)
            {
                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < series.Comics.Count; i++)
                    {
                        zip.AddFiles(series.Comics[i].PagePaths, series.Comics[i].Title);
                    }
                    zip.Save(path + Path.DirectorySeparatorChar + series.Title + ".cbz");//TODO handle other extensions
                }
            }
            else
            {
                for (int i = 0; i < series.Comics.Count; i++)
                {
                    CompressComic(series.Comics[i],path);
                }
            }
        }
        /// <summary>
        /// Compress multiple comics to their parent directory
        /// </summary>
        /// <param name="comics"> A List of comics to be compressed</param>
        /// <param name="singleFile"> Compile the comics into a single large file or separate files</param>
        public void CompressSeries(ComicSeries series, bool singleFile)
        {
            CompressSeries(series,singleFile, Directory.GetParent(series.Comics[0].ComicPath).FullName);//TODO this does not work well when you are compressing comics from multiple directories (everything goes in the first directory)
        }
        /// <summary>
        /// Deletes the temporary directory
        /// </summary>
        public void DeleteTemporaryDirectory()
        {

            if (Directory.Exists(temporaryDirectory))
            {
                Directory.Delete(temporaryDirectory, true);
            }

        }

    }
}
