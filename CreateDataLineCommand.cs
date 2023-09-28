using MediatR;

namespace TestProject1
{
    public record CreateDataLineCommand(string LineContent) : IRequest<DataLine>;
}
