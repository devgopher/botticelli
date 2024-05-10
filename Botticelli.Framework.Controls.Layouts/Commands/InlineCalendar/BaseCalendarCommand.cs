using Botticelli.Framework.Commands;

namespace Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;

public abstract class BaseCalendarCommand: ICommand
{
    public Guid Id { get; }
    public DateTime CurrentDt { get; set; }  
}