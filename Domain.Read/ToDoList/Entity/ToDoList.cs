using System;
using System.Collections.Generic;

namespace Domain.Read.ToDoList.Entity
{
    public class ToDoList
    {
        public Guid Id { get; set; }
        
        public string Label { get; set; }
        
        public string Description { get; set; }
    }
}