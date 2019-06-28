using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Mvc;

namespace Im.Access.GraphPortal.Graph
{
    [Route("GraphQl")]
    [ApiController]
    public class GraphQlController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly IDictionary<string, string> _namedQueries;

        public GraphQlController(
            ISchema schema,
            IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;

            // TODO: Initialise the list of named queries
            _namedQueries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] GraphQlQuery query,
            CancellationToken cancellationToken)
        {
            if (query == null)
            {
                return BadRequest("Missing query body.");
            }

            // Determine query to execute
            var queryToExecute = query.Query;
            if (!string.IsNullOrWhiteSpace(query.NamedQuery) &&
                !_namedQueries.TryGetValue(query.NamedQuery, out queryToExecute))
            {
                return BadRequest($"Named query, {query.NamedQuery}, not found.");
            }

            var startTime = DateTime.UtcNow;
            var executionOptions =
                new ExecutionOptions
                {
                    Schema = _schema,
                    Query = queryToExecute,
                    OperationName = query.OperationName,
                    Inputs = query.Variables.ToInputs(),
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

            //result.EnrichWithApolloTracing(startTime);

            return result.Errors?.Count > 0 ? (IActionResult) BadRequest(result) : Json(result);
        }
    }
}