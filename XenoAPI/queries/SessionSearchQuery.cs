using XeniaWebServices.Controllers;

namespace XeniaWebServices.XenoAPI.queries
{
    internal class SessionSearchQuery
    {
        private TitleId titleId;
        private object searchIndex;
        private object resultsCount;

        public SessionSearchQuery(TitleId titleId, object searchIndex, object resultsCount)
        {
            this.titleId = titleId;
            this.searchIndex = searchIndex;
            this.resultsCount = resultsCount;
        }
    }
}