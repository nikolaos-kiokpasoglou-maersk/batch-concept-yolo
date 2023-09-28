using CSharpFunctionalExtensions;
using MediatR;

namespace TestProject1
{
    public class ProcessDataLineBatchCommandHandler : IRequestHandler<ProcessDataLineBatchCommand, ProcessDataLineBatchResult>
    {
        public Task<ProcessDataLineBatchResult> Handle(ProcessDataLineBatchCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new ProcessDataLineBatchResult(request.DataLineBatch.DataLines.Select(x => new DataLineProcessOutcome(x, Result.Success())))
            );
        }
    }
}
