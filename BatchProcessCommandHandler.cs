using MediatR;
using System.Text;

namespace TestProject1
{
    public class BatchProcessCommandHandler : IRequestHandler<BatchProcessCommand, BatchProcessResult>
    {
        private readonly IMediator _mediator;

        public BatchProcessCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<BatchProcessResult> Handle(BatchProcessCommand request, CancellationToken cancellationToken)
        {
            var fileContent = AccessContent();

            var batchResults = new List<ProcessDataLineBatchResult>();

            foreach (var chunk in fileContent.Chunk(request.BatchProcessContext.BatchSize))
            {
                var createBatchCommand = new CreateDataLineBatchCommand(chunk.Select(x => new CreateDataLineCommand(x)));

                var dataBatch = await _mediator.Send(createBatchCommand, cancellationToken);

                var processBatchCommand = new ProcessDataLineBatchCommand(dataBatch);

                var processBatchResult = await _mediator.Send(processBatchCommand, cancellationToken);

                batchResults.Add(processBatchResult);
            }

            return new BatchProcessResult(batchResults);
        }

        private static IEnumerable<string> AccessContent()
            => File.ReadLines("test.txt", Encoding.UTF8);
    }
}
