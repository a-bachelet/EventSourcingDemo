using System;

namespace Domain.Write.ToDo.Event
{
    public class ToDoResetedV1Data : EventData
    {
        public Guid ToDoListId { get; }
        
        public ToDoResetedV1Data(Guid toDoListId)
        {
            ToDoListId = toDoListId;
        }
    }
    
    public class ToDoResetedV1 : Domain.Event
    {
        public override ToDoResetedV1Data Data { get; }
        
        public ToDoResetedV1(Guid aggregateId, Guid toDoListId) : base(aggregateId, 1, "ToDoReseteds")
        {
            Data = new ToDoResetedV1Data(toDoListId);
        }
    }
}