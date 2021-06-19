namespace AutomationTests.UnitTests.Core.Services
{
    using System;
    using Xunit;

    public sealed class ToDoServiceTests
    {
        private readonly ToDoServiceSteps steps = new();

        [Fact]
        public void Initialization_NullRepository_ThrowArgumentNullException()
        {
            this.steps
                .WhenIInitialize(isNullRepository: true)
                .ThenExceptionShouldBeThrown(typeof(ArgumentNullException));
        }
    }
}