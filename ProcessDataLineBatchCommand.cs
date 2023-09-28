using MediatR;

namespace TestProject1
{
    public record ProcessDataLineBatchCommand(DataLineBatch DataLineBatch) : IRequest<ProcessDataLineBatchResult>;
}
