using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Im.Access.GraphPortal.Graph
{
    public class GraphQlMediaTypeFormatter : TextInputFormatter
    {
        private readonly bool _suppressInputFormatterBuffering;

        public GraphQlMediaTypeFormatter(bool suppressInputFormatterBuffering)
        {
            _suppressInputFormatterBuffering = suppressInputFormatterBuffering;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/graphql"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanReadType(Type type)
        {
            var ctor = type.GetConstructor(new Type[0]);

            var propInfo = type.GetProperty(
                "query",
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.IgnoreCase |
                BindingFlags.SetProperty);

            return ctor != null && propInfo != null;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(
            InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;
            if (!request.Body.CanSeek && !_suppressInputFormatterBuffering)
            {
                request.EnableBuffering();
                await request.Body.DrainAsync(CancellationToken.None).ConfigureAwait(false);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }

            using (var reader = context.ReaderFactory(request.Body, encoding))
            {
                var query = await reader.ReadToEndAsync().ConfigureAwait(false);

                var type = context.ModelType;
                var propInfo = type.GetProperty(
                    "query",
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase |
                    BindingFlags.SetProperty);
                if (propInfo == null)
                {
                    return InputFormatterResult.Failure();
                }

                var instance = Activator.CreateInstance(type);
                propInfo.SetValue(instance, query);

                return InputFormatterResult.Success(instance);
            }
        }
    }
}