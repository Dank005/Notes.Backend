
namespace Notes.Application.Notes.Commands.DeleteCommand
{
    public class DeleteNoteCommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
