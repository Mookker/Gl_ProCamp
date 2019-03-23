using System;
using CommonLibrary.Models.Search;

namespace ProCamp.Models.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class FixturesSearchOptions : BaseSearchOptions
    {
        /// <summary>
        /// Home team
        /// </summary>
        public string HomeTeamName { get; set; }
        
        /// <summary>
        /// Away team
        /// </summary>
        public string AwayTeamName { get; set; }
        
        /// <summary>
        /// Minimum time
        /// </summary>
        public DateTime? DateFrom { get; set; }
        
        /// <summary>
        /// Max time
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}