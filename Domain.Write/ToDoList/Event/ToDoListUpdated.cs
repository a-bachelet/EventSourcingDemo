using System;

namespace Domain.Write.ToDoList.Event
{
    public class ToDoListUpdatedV1Data : EventData
    {
        public string Label { get; set; }
        
        public string Description { get; set; }
    }
    
    public class ToDoListUpdatedV1 : Domain.Event
    {
        public override ToDoListUpdatedV1Data Data { get; }
        
        public ToDoListUpdatedV1(Guid aggregateId,
            string label,
            string description) : base(aggregateId,
            1,
            "ToDoListUpdated")
        {
            Data = new ToDoListUpdatedV1Data
            {
                Label = label,
                Description = description
            };
        }
    }
}