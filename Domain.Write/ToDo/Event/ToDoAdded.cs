using System;

namespace Domain.Write.ToDo.Event
{
    public class ToDoAddedV1Data : EventData
    {
        public Guid ToDoListId { get; }
        
        public string Label { get; }
        
        public string Description { get; }

        public ToDoAddedV1Data(Guid toDoListId, string label, string description)
        {
            ToDoListId = toDoListId;
            Label = label;
            Description = description;
        }
    }
    
    public class ToDoAddedV1 : Domain.Event
    {
        public override ToDoAddedV1Data Data { get; }
        
        public ToDoAddedV1(Guid aggregateId, Guid toDoListId, string label, string description)
            : base(aggregateId, 1, "ToDoAdded")
        {
            Data = new ToDoAddedV1Data(toDoListId, label, description);
        }
    }
}