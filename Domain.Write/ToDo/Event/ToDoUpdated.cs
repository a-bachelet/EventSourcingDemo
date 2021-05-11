using System;

namespace Domain.Write.ToDo.Event
{
    public class ToDoUpdatedV1Data : EventData
    {
        public Guid ToDoListId { get; }
        
        public string Label { get; }
        
        public string Description { get; }

        public ToDoUpdatedV1Data(Guid toDoListId, string label, string description)
        {
            ToDoListId = toDoListId;
            Label = label;
            Description = description;
        }
    }
    
    public class ToDoUpdatedV1 : Domain.Event
    {
        public override ToDoUpdatedV1Data Data { get; }
        
        public ToDoUpdatedV1(Guid aggregateId, Guid toDoListId, string label, string description)
            : base(aggregateId, 1, "ToDoUpdated")
        {
            Data = new ToDoUpdatedV1Data(toDoListId, label, description);
        }
    }
}