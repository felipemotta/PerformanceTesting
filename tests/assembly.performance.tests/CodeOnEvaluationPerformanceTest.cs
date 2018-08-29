namespace DevExperience.Assembly.Performance.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CodeOnEvaluationTest
    {
        [TestMethod]
        public void PerformanceTest()
        {
            // Arrange
            var testee = new CodeOnEvaluation();
            
            // Act
            Action notimplemented = () => testee.Method();
            // Assert
            notimplemented.Should().Throw<NotImplementedException>();
        }
    }
}
