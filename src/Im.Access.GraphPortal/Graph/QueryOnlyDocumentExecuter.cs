using System;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;

namespace Im.Access.GraphPortal.Graph
{
    public class QueryOnlyDocumentExecuter : DocumentExecuter
    {
        public QueryOnlyDocumentExecuter()
        {
        }

        public QueryOnlyDocumentExecuter(
            IDocumentBuilder documentBuilder,
            IDocumentValidator documentValidator,
            IComplexityAnalyzer complexityAnalyzer)
            : base(documentBuilder, documentValidator, complexityAnalyzer)
        {
        }

        protected override IExecutionStrategy SelectExecutionStrategy(ExecutionContext context)
        {
            if (context.Operation.OperationType != OperationType.Query)
            {
                throw new InvalidOperationException("Only query operations are supported.");
            }

            return base.SelectExecutionStrategy(context);
        }
    }
}