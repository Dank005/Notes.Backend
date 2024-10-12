using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
    {
        private readonly INotesDbContext dbContext;
        private readonly IMapper mapper;

        public GetNoteDetailsQueryHandler(INotesDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Notes
                .Where(note => note.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null || request.UserId != entity.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            return mapper.Map<NoteDetailsVm>(entity);
        }
    }
}
