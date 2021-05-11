using System;

namespace Domain.Write.ToDoList.Event
{
    public class ToDoListAddedV1Data : EventData
    {
        public string Label { get; set; }
        
        public string Description { get; set; }
    }
    
    public class ToDoListAddedV1 : Domain.Event
    {
        public override ToDoListAddedV1Data Data { get; }
        
        public ToDoListAddedV1(Guid aggregateId,
            string label,
            string description) : base(aggregateId,
            1,
            "ToDoListAdded")
        {
            Data = new ToDoListAddedV1Data
            {
                Label = label,
                Description = description
            };
        }
    }
}