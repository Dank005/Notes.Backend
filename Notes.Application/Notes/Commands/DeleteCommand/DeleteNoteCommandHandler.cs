using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteCommand
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
    {
        private readonly INotesDbContext dbContext;
        public DeleteNoteCommandHandler(INotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Notes
                .Where(note => note.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null || request.UserId != entity.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            await dbContext.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
