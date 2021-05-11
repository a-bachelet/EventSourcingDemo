using System;

namespace Domain.Write.ToDo.Event
{
    public class ToDoDeletedV1Data : EventData
    {
        public Guid ToDoListId { get; }
        
        public ToDoDeletedV1Data(Guid toDoListId)
        {
            ToDoListId = toDoListId;
        }
    }

    public class ToDoDeletedV1 : Domain.Event
    {
        public override ToDoDeletedV1Data Data { get; }
        
        public ToDoDeletedV1(Guid aggregateId, Guid toDoListId) : base(aggregateId, 1, "ToDoDeleted")
        {
            Data = new ToDoDeletedV1Data(toDoListId);
        }
    }
}