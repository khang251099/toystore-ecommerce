//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThuongMaiDienTu.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VoteLog
    {
        public int AutoId { get; set; }
        public Nullable<short> SectionID { get; set; }
        public Nullable<int> VoteForId { get; set; }
        public Nullable<short> Vote { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> IDKhachHang { get; set; }
        public Nullable<int> IDProduct { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
        public virtual Product Product { get; set; }
    }
}