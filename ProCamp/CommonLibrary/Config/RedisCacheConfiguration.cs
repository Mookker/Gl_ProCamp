namespace CommonLibrary.Config
{
    public class RedisCacheConfiguration
    {
        /// <summary>
        /// Environment name
        /// </summary>
        public string Environment { get; set; }
        
        /// <summary>
        /// Prefix used for keys
        /// </summary>
        public string ApiName { get; set; }
        
        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; set; }
    }
}