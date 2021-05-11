using System;

namespace Domain.Read.ToDo.Entity
{
    public enum ToDoState
    {
        Waiting = 0,
        Started = 1,
        Finished = 2,
        Deleted = 3
    }
    
    public class ToDo
    {
        public Guid Id { get; set; }
        
        public Guid ToDoListId { get; set; }
        
        public string Label { get; set; }
        
        public string Description { get; set; }

        public ToDoState State { get; set; }
        
        public DateTime StartedAt { get; set; }
        
        public DateTime EndedAt { get; set; }
    }
}