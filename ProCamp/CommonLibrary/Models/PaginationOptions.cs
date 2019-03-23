namespace CommonLibrary.Models
{
    public class PaginationOptions
    {
        /// <summary>
        /// Offset
        /// </summary>
        public int? Offset { get; set; }
        
        /// <summary>
        /// Limit
        /// </summary>
        public int? Limit { get; set; }
        
        /// <summary>
        /// Order by field
        /// </summary>
        public string OrderBy { get; set; }
    }
}