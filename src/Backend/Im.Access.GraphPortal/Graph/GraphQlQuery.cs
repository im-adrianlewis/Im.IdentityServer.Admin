using Newtonsoft.Json.Linq;

namespace Im.Access.GraphPortal.Graph
{
    /// <summary>
    /// Encapsulates a Graph-QL statement that will be executed by the
    /// Graph-QL processing engine
    /// </summary>
    public class GraphQlQuery
    {
        /// <summary>
        /// Gets or sets the query to be executed
        /// </summary>
        /// <remarks>
        /// This field is required unless a named query is specified
        /// or the query is passed as a parameter on the URL.
        /// </remarks>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the operation name
        /// </summary>
        /// <remarks>
        /// The operation name is used to select the query to execute
        /// when multiple queries are available (either in the actual
        /// query posted or the ultimate query used if referencing a
        /// named query)
        /// </remarks>
        public string OperationName { get; set; }

        /// <summary>
        /// Gets or sets the named query to execute
        /// </summary>
        public string NamedQuery { get; set; }

        /// <summary>
        /// Gets or sets the variables referenced by the query to execute
        /// </summary>
        public JObject Variables { get; set; } //https://github.com/graphql-dotnet/graphql-dotnet/issues/389
    }
}