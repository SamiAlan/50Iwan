using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwan.Server.Models
{
    public class PagedViewModel<TData>
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; } = 1;
        public int PageSize { get; private set; } = 12;
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public List<int> Pages { get; private set; }
        public bool AddFirstPageWithDots { get; private set; }
        public bool AddLastPageWithDots { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => TotalPages > CurrentPage;
        public IList<TData> Data { get; set; }

        public PagedViewModel(
            int totalItems,
            int currentPage = 1,
            int pageSize = 10,
            int maxPages = 3)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            // ensure current page isn't out of range
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            // create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, (endPage + 1) - startPage).ToList();

            // update object instance with all pager properties required by the view
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Pages = pages;

            if (!pages.Contains(1))
            {
                var firstPage = Pages.ElementAt(0);

                if ((firstPage - 1) <= 2)
                    for (int i = 0, j = 1; j < firstPage; i++, j++)
                        Pages.Insert(i, j);
                else
                    AddFirstPageWithDots = true;
            }

            if (!pages.Contains(TotalPages))
            {
                var lastPage = pages.Last();

                if ((TotalPages - lastPage) <= 2)
                    for (int i = lastPage + 1; i <= totalPages; i++)
                        Pages.Add(i);
                else
                    AddLastPageWithDots = true;
            }
        }
    }
}
