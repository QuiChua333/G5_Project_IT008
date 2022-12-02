//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CinemaManagementProject.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public int Id { get; set; }
        public Nullable<int> BillId { get; set; }
        public Nullable<int> ShowTimeId { get; set; }
        public Nullable<int> SeatId { get; set; }
    
        public virtual Bill Bill { get; set; }
        public virtual Seat Seat { get; set; }
        public virtual ShowTime ShowTime { get; set; }
    }
}
