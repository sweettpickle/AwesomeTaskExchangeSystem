

namespace TaskManager.Persistence.Utils
{
    /// <summary>
    /// Contains persistence-related settings: connection string, query tracing settings & etc.
    /// </summary>
    public class PersistenceConfig
    {
        /// <summary>
        /// A database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Enables tracing diagnostic information into logs.
        /// </summary>
        public bool EnableTracing { get; set; }
    }
}
