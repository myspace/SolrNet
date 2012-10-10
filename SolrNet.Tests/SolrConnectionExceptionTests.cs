using System;
using System.Net;
using MbUnit.Framework;
using SolrNet.Exceptions;

namespace SolrNet.Tests
{
    /// <summary>
    /// Class to unit test SolrConnectionException
    /// </summary>
    [TestFixture]
    public class SolrConnectionExceptionTests
    {
        const string message = "Test Message";
        const string url = "http://localhost:8983/solr";
        const string innerExceptionMessage = "Web exception test message";
        Exception innerException = new WebException(innerExceptionMessage);
        const HttpStatusCode solrErrorStatus = HttpStatusCode.Conflict;

        [Test]
        public void MessageConstructorTest()
        {
            var exception = new SolrConnectionException(message);
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void InnerExceptionConstructorTest()
        {
            var exception = new SolrConnectionException(innerException);
            Assert.IsInstanceOfType<WebException>(exception.InnerException);
            Assert.AreEqual(innerExceptionMessage, exception.Message);
        }

        [Test]
        public void UrlConstructorTest()
        {
            var exception = new SolrConnectionException(innerException, url);
            Assert.IsInstanceOfType<WebException>(exception.InnerException);
            Assert.AreEqual(innerExceptionMessage, exception.Message);
            Assert.AreEqual(url, exception.Url);
        }

        [Test]
        public void HttpStatusCodeConstructorTest()
        {
            var exception = new SolrConnectionException(message, innerException, url, solrErrorStatus);
            Assert.IsInstanceOfType<WebException>(exception.InnerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(url, exception.Url);
            Assert.AreEqual(solrErrorStatus, exception.SolrHttpStatusCode);
        }

    }
}
