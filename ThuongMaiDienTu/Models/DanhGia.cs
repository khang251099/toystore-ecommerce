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
    
    public partial class DanhGia
    {
        public int IDDanhGia { get; set; }
        public string DanhGia1 { get; set; }
        public Nullable<int> IDProduct { get; set; }
        public Nullable<int> IDKhachHang { get; set; }
        public Nullable<System.DateTime> NgayDanhGia { get; set; }
        public string Content { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
        public virtual Product Product { get; set; }
    }
}