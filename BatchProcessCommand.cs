using MediatR;

namespace TestProject1
{
    public record BatchProcessCommand(BatchProcessContext BatchProcessContext) : IRequest<BatchProcessResult>;
}
