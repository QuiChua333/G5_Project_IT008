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
    
    public partial class Trouble
    {
        public int Id { get; set; }
        public Nullable<int> TroubleType { get; set; }
        public string TroubleInfo { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<int> TroubleStatus { get; set; }
        public Nullable<int> StaffId { get; set; }
    
        public virtual Staff Staff { get; set; }
    }
}