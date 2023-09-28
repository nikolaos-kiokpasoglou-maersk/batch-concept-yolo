using MediatR;

namespace TestProject1
{
    public class CreateDataLineBatchCommandHandler : IRequestHandler<CreateDataLineBatchCommand, DataLineBatch>
    {
        public Task<DataLineBatch> Handle(CreateDataLineBatchCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new DataLineBatch(request.CreateDataLineCommands.Select(x => new DataLine(x.LineContent)))
            );
        }
    }
}
