using System;
using System.Linq;
using NetMX.Expression;
using NetMX.Server;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace NetMX.Tests
{
    [TestFixture]
    public class ExpressionParserTests
    {
        [Test]
        public void It_respects_precedence_of_operators()
        {
            var result = Evaludate<Number>("1 + 2 * 3");
            Assert.AreEqual(7m, result.Value);
        }

        [Test]
        public void It_respects_parentheses()
        {
            var result = Evaludate<Number>("(1 + 2) * 3");
            Assert.AreEqual(9m, result.Value);
        }

        [Test]
        public void It_converts_strings_to_numbers_when_applicable()
        {
            var result = Evaludate<Number>("1 + '6'");
            Assert.AreEqual(7m, result.Value);
        }        
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_throws_when_child_expression_has_wrong_type()
        {
            ExpressionParser.Parse<bool>("0 and 0");
        }        

        private static T Evaludate<T>(string expression)
        {
            var expr = ExpressionParser.Parse<T>(expression);
            return expr.Evaluate(new EvaluationContext("Sample:a=b", new EmptyDynamicMBean()));
        }

        private class EmptyDynamicMBean : IDynamicMBean
        {
            public MBeanInfo GetMBeanInfo()
            {
                return new MBeanInfo("EmptyDynamicMBean", "", 
                    Enumerable.Empty<MBeanAttributeInfo>(),
                    Enumerable.Empty<MBeanConstructorInfo>(),
                    Enumerable.Empty<MBeanOperationInfo>(),
                    Enumerable.Empty<MBeanNotificationInfo>());
            }

            public object GetAttribute(string attributeName)
            {
                return null;
            }

            public void SetAttribute(string attributeName, object value)
            {                
            }

            public object Invoke(string operationName, object[] arguments)
            {
                return null;
            }
        }
    }

}
// ReSharper restore InconsistentNaming
