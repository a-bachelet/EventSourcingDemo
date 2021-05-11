using System;

namespace Domain.Write.ToDo.Event
{
    public class ToDoStartedV1Data : EventData
    {
        public Guid ToDoListId { get; }
        
        public ToDoStartedV1Data(Guid toDoListId)
        {
            ToDoListId = toDoListId;
        }
    }
    
    public class ToDoStartedV1 : Domain.Event
    {
        public override ToDoStartedV1Data Data { get; }
        
        public ToDoStartedV1(Guid aggregateId, Guid toDoListId) : base(aggregateId, 1, "ToDoStarted")
        {
            Data = new ToDoStartedV1Data(toDoListId);
        }
    }
}