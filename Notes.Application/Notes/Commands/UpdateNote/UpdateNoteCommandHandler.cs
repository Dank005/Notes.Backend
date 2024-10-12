using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        private readonly INotesDbContext dbContext;
        public UpdateNoteCommandHandler(INotesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Notes
                .Where(note => note.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if(entity == null || request.UserId != entity.UserId) 
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            entity.Title = request.Title;
            entity.Details = request.Details;
            entity.EditDate = DateTime.Now;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
