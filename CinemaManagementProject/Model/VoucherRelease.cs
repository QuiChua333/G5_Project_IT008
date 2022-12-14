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
    
    public partial class VoucherRelease
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VoucherRelease()
        {
            this.Vouchers = new HashSet<Voucher>();
        }
    
        public int Id { get; set; }
        public string VoucherReleaseCode { get; set; }
        public string VoucherReleaseName { get; set; }
        public Nullable<float> Price { get; set; }
        public Nullable<bool> VoucherReleaseStatus { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string TypeObject { get; set; }
        public Nullable<float> MinimizeTotal { get; set; }
        public Nullable<bool> EnableMerge { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
