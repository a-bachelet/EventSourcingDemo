using System;

namespace Domain.Write.ToDoList.Event
{
    public class ToDoListDeletedV1Data : EventData
    {
    }
    
    public class ToDoListDeletedV1 : Domain.Event
    {
        public override ToDoListDeletedV1Data Data { get; }
        
        public ToDoListDeletedV1(Guid aggregateId) : base(aggregateId, 1, "ToDoListDeleted")
        {
            Data = new ToDoListDeletedV1Data();
        }
    }
}