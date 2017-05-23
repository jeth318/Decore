using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DecoreStudentFront.EmployeeServiceRef;
using DecoreStudentFront.EventServiceRef;
using DecoreStudentFront.ViewModels;

namespace DecoreStudentFront.ViewModels
{
    public class EventViewModel
    {


    
        public TicketViewModel TicketViewModel { get; set; }



        public int Id { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventType> EventTypes { get; set; }
        public ICollection<SectionInfo> SectionTypes { get; set; }

        public int EventTypeId { get; set; }
        public string Title { get; set; }
        public string ZipCode { get; set; }
        public string Adress { get; set; }
        public string ImageURL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime SaleStop { get; set; }
        public int SectionId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Description { get; set; }
        public float BasePrice { get; set; }
        public float MemberPrice { get; set; }
    }
}