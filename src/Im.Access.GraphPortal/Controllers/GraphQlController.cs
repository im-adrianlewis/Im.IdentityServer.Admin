using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using Im.Access.GraphPortal.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace Im.Access.GraphPortal.Controllers
{
    /// <summary>
    /// GraphQlController is the single entry-point into the GraphQL query environment.
    /// </summary>
    [Route("graphql")]
    [ApiController]
    // [Authorize]
    public class GraphQlController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly IDictionary<string, string> _namedQueries =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public GraphQlController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;

            // TODO: Initialise the list of named queries
            _namedQueries.Add("get-users", "");
        }

        /// <summary>
        /// Get endpoint supports execution of GraphQL queries via a cacheable HTTP GET. 
        /// </summary>
        /// <remarks>
        /// GraphQL mutations and subscriptions are not supported from this endpoint.
        /// </remarks>
        /// <param name="query">Mandatory query body</param>
        /// <param name="operationName">Optional operation name if query body specifies multiple queries.</param>
        /// <param name="variables">Optional set of variables to apply to the query.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Executes Graph-QL queries",
            Description = "Some queries require a bearer token before valid results can be obtained.",
            OperationId = "GetQuery",
            Tags = new[] { "Get", "Query" }
        )]
        [SwaggerResponse(400, "Query is invalid")]
        public async Task<IActionResult> Get(
            [FromQuery]
            [SwaggerParameter("Query body", Required = true)]
            string query,
            [FromQuery]
            [SwaggerParameter("Operation name", Required = false)]
            string operationName,
            [FromQuery]
            [SwaggerParameter("Query variables", Required = false)]
            string variables,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Missing query body.");
            }

            Inputs inputVariables = null;
            if (!string.IsNullOrWhiteSpace(variables))
            {
                inputVariables = JObject.Parse(variables).ToInputs();
            }

            var startTime = DateTime.UtcNow;
            var executionOptions =
                new ExecutionOptions
                {
                    Schema = _schema,
                    Query = query,
                    OperationName = operationName,
                    Inputs = inputVariables,
                    UserContext = User,
                    CancellationToken = cancellationToken,
                    ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 },
                    ExposeExceptions = true,
                    EnableMetrics = true
                };
            executionOptions.FieldMiddleware.Use<InstrumentFieldsMiddleware>();

            var result = await new QueryOnlyDocumentExecuter()
                .ExecuteAsync(executionOptions)
                .ConfigureAwait(true);

            result.EnrichWithApolloTracing(startTime);

            return result.Errors?.Count > 0 ? (IActionResult)BadRequest(result) : Json(result);
        }

        /// <summary>
        /// Post endpoint supports execution of GraphQL queries.
        /// </summary>
        /// <remarks>
        /// All GraphQL operations are valid from this endpoint (query, mutation, subscription)
        /// </remarks>
        /// <param name="query">Optional explicit query passed on the query string.</param>
        /// <param name="model">Graph model passed in the request body.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Executes Graph-QL queries, mutations and subscriptions",
            Description = "Some queries require a bearer token before valid results can be obtained.",
            OperationId = "PostQuery",
            Tags = new []{ "Post", "Query" }
        )]
        [SwaggerResponse(400, "Query is invalid")]
        public async Task<IActionResult> Post(
            [FromQuery]
            [SwaggerParameter("Explicit query", Required = false)]
            string query,
            [FromBody]
            [SwaggerParameter("Graph query model", Required = true)]
            GraphQlQuery model,
            CancellationToken cancellationToken)
        {
            if (model == null)
            {
                return BadRequest("Missing query body.");
            }

            // Determine query to execute
            var queryToExecute = query ?? model.Query;
            if (!string.IsNullOrWhiteSpace(model.NamedQuery) &&
                !_namedQueries.TryGetValue(model.NamedQuery, out queryToExecute))
            {
                return BadRequest($"Named query, {model.NamedQuery}, not found.");
            }

            var startTime = DateTime.UtcNow;
            var executionOptions =
                new ExecutionOptions
                {
                    Schema = _schema,
                    Query = queryToExecute,
                    OperationName = model.OperationName,
                    Inputs = model.Variables.ToInputs(),
                    UserContext = User,
                    CancellationToken = cancellationToken,
                    ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 },
                    ExposeExceptions = true,
                    EnableMetrics = true
                };
            executionOptions.FieldMiddleware.Use<InstrumentFieldsMiddleware>();

            var result = await _documentExecuter
                .ExecuteAsync(executionOptions)
                .ConfigureAwait(true);

            result.EnrichWithApolloTracing(startTime);

            return result.Errors?.Count > 0 ? (IActionResult) BadRequest(result) : Json(result);
        }
    }
}