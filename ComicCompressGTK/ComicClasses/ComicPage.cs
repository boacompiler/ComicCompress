
namespace ComicCompressPortable.ComicClasses
{
    public class ComicPage
    {
        private int pageNumber;
        private string pagePath;

        public ComicPage(string path,int number)
        {
            PagePath = path;
            PageNumber = number;
        }

        public int PageNumber
        {
            get
            {
                return pageNumber;
            }

            set
            {
                pageNumber = value;
            }
        }

        public string PagePath
        {
            get
            {
                return pagePath;
            }

            set
            {
                pagePath = value;
            }
        }
    }
}
