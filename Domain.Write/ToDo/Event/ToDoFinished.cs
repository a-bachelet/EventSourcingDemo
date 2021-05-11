using System;

namespace Domain.Write.ToDo.Event
{
    public class ToDoFinishedV1Data : EventData
    {
        public Guid ToDoListId { get; }
        
        public ToDoFinishedV1Data(Guid toDoListId)
        {
            ToDoListId = toDoListId;
        }
    }
    
    public class ToDoFinishedV1 : Domain.Event
    {
        public override ToDoFinishedV1Data Data { get; }
        
        public ToDoFinishedV1(Guid aggregateId, Guid toDoListId) : base(aggregateId, 1, "ToDoFinished")
        {
            Data = new ToDoFinishedV1Data(toDoListId);
        }
    }
}