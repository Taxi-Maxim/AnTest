using AnTest.Models;
using AnTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;



namespace AnTest.Tests
{
    [TestClass]
    public class EmailProcessingTests
    {
        [TestMethod]
        public void ProcessEmailCopy_AddsAddress_WhenBusinessDomainExists()
        {
            var email = new EmailModel
            {
                To = "q.qweshnikov@batut.com; w.petrov@alfa.com;",
                Copy = "f.patit@buisness.com"
            };

            var result = EmailProcessing.ProcessEmailCopy(email);

            Assert.AreEqual("f.patit@buisness.com;v.vladislavovich@alfa.com;", result);
        }

        [TestMethod]
        public void ProcessEmailCopy_DoesNotAddAddress_WhenExceptionExists()
        {
            var email = new EmailModel
            {
                To = "t.kogni@acl.com",
                Copy = "i.ivanov@tbank.ru"
            };

            var result = EmailProcessing.ProcessEmailCopy(email);

            Assert.AreEqual("i.ivanov@tbank.ru;", result);
        }

        [TestMethod]
        public void ProcessEmailCopy_RemovesAddresses_WhenExceptionExists()
        {
            var email = new EmailModel
            {
                To = "t.kogni@acl.com; i.ivanov@tbank.ru;",
                Copy = "e.gras@tbank.ru; t.tbankovich@tbank.ru; v.veronickovna@tbank.ru;"
            };

            var result = EmailProcessing.ProcessEmailCopy(email);

            Assert.AreEqual("e.gras@tbank.ru;", result);
        }

        [TestMethod]
        public void ProcessEmailCopy_DoesNotChange_WhenNoBusinessDomains()
        {
            var email = new EmailModel
            {
                To = "z.xcy@email.com",
                Copy = "p.rivet@email.com"
            };

            var result = EmailProcessing.ProcessEmailCopy(email);

            Assert.AreEqual("p.rivet@email.com;", result);
        }
    }
}
