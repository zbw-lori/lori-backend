using lori.backend.SharedKernel;

namespace lori.backend.Core.ProjectAggregate.Events;

public class ToDoItemCompletedEvent : DomainEventBase
{
  public ToDoItem CompletedItem { get; set; }

  public ToDoItemCompletedEvent(ToDoItem completedItem)
  {
    CompletedItem = completedItem;
  }
}
