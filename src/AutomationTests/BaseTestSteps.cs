namespace AutomationTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    internal abstract class BaseTestSteps<T>
        where T : class
    {
        protected Exception ThrownException { get; set; }

        protected object Result { get; set; }

        protected abstract T GetStepClass();

        public T ThenTheResultShouldBe<U>(U expected)
        {
            this.Result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
            return this.GetStepClass();
        }

        public T ThenThrownExceptionShouldBe(Type exceptionType)
        {
            this.ThrownException.Should().NotBeNull();
            this.ThrownException.Should().BeOfType(exceptionType);
            return this.GetStepClass();
        }

        protected T RecordException(Action testCode)
        {
            this.ThrownException = Record.Exception(testCode);
            return this.GetStepClass();
        }

        protected async Task RecordExceptionAsync(Func<Task> testCode)
        {
            this.ThrownException = await Record
                .ExceptionAsync(async() => await testCode().ConfigureAwait(false))
                .ConfigureAwait(false); 
        }

        protected async Task RecordExceptionAsync<U>(Func<Task<U>> testCode)
        {
            this.ThrownException = await Record
                .ExceptionAsync(async () => this.Result = await testCode().ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }
}