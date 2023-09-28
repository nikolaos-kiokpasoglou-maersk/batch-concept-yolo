using FluentAssertions;
using MediatR;
using NSubstitute;
using System.Text;

namespace TestProject1
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(1000)]
        public void Test1(int batchSize)
        {
            var fileContent = AccessContent();

            var commands = new List<CreateDataLineCommand>();

            foreach (var line in fileContent)
            {
                commands.Add(new CreateDataLineCommand(line));
            }

            var commandBatches = new List<CreateDataLineBatchCommand>();

            foreach (var chunk in fileContent.Chunk(batchSize))
            {
                commandBatches.Add(new CreateDataLineBatchCommand(chunk.Select(x => new CreateDataLineCommand(x))));
            }

            commandBatches.Should().HaveCount((int)Math.Floor((commands.Count + batchSize - 1) / batchSize * 1.0));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(1000)]
        public async Task SampleBatchProcess(int batchSize)
        {
            var createDataLineBatchCommandHandler = new CreateDataLineBatchCommandHandler();

            var processDataLineBatchCommandHandler = new ProcessDataLineBatchCommandHandler();

            var fileContent = AccessContent();

            foreach (var chunk in fileContent.Chunk(batchSize))
            {
                var createBatchCommand = new CreateDataLineBatchCommand(chunk.Select(x => new CreateDataLineCommand(x)));

                var dataBatch = await createDataLineBatchCommandHandler.Handle(createBatchCommand, default);

                var processBatchCommand = new ProcessDataLineBatchCommand(dataBatch);

                var processBatchResult = await processDataLineBatchCommandHandler.Handle(processBatchCommand, default);

                processBatchResult.DataLineProcessOutcome.Should().AllSatisfy(x => x.ProcessOutcome.IsSuccess.Should().BeTrue());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(1000)]
        public async Task BatchProcess(int batchSize)
        {
            var mediator = Substitute.For<IMediator>();

            var batchProcess = new BatchProcessCommandHandler(mediator);

            var batchProcessResult = await batchProcess.Handle(new BatchProcessCommand(new BatchProcessContext(batchSize)), default);

            var numberOfBatches = batchProcessResult.BatchResult.Count();

            await mediator.Received(numberOfBatches).Send(Arg.Any<CreateDataLineBatchCommand>());
            await mediator.Received(numberOfBatches).Send(Arg.Any<ProcessDataLineBatchCommand>());
        }

        private static IEnumerable<string> AccessContent()
            => File.ReadLines("test.txt", Encoding.UTF8);
    }
}