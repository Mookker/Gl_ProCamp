using CommonLibrary.Models.Search;

namespace AuthApi.Models.Search
{
    /// <inheritdoc />
    public class UserSearchOptions : BaseSearchOptions
    {
        public string Login { get; set; }
    }
}