using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using Moq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Tms.Logger;

namespace Tms.Test.Extensions
{
    public static class TestExtensions
    {
        public static void SetSource<T>(this Mock<DbSet<T>> mockSet, IEnumerable<T> source) where T : class
        {
            var data = source.AsQueryable();
            mockSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new TestAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        public static string GetErrorMessage(this HttpResponseMessage response)
        {
            try
            {
                // content
                string json = response.Content.ReadAsStringAsync().Result;

                // anonymous object
                var anonymousErrorObject = new
                {
                    Status = HttpStatusCode.OK,
                    Errors = new Dictionary<string, string[]>()
                };

                // deserialize
                var jsonObject = JsonConvert.DeserializeAnonymousType(json, anonymousErrorObject);

                // if errors
                if (jsonObject.Status != HttpStatusCode.OK)
                {
                    var errors = jsonObject.Errors.Select(x => $"{(string.IsNullOrEmpty(x.Key) ? "" : x.Key + ": ")}{string.Join(", ", x.Value)}");
                    return string.Join("; ", errors);
                }
                return string.Empty;

            }
            catch (Exception ex)
            {
                ITmsLogger _logger = new TmsLogger();
                _logger.Error(ex);
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
