using MediatR;

namespace TestProject1
{
    public record CreateDataLineBatchCommand(IEnumerable<CreateDataLineCommand> CreateDataLineCommands) : IRequest<DataLineBatch>;
}
