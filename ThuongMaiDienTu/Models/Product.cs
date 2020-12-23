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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.DanhGias = new HashSet<DanhGia>();
            this.GioHangs = new HashSet<GioHang>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.SanPhamKhuyenMais = new HashSet<SanPhamKhuyenMai>();
            this.VoteLogs = new HashSet<VoteLog>();
        }
    
        public int IDProduct { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public string MoTa { get; set; }
        public Nullable<int> PromotionPrice { get; set; }
        public string MaSP { get; set; }
        public string ChuDe { get; set; }
        public string XuatXu { get; set; }
        public string Images { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string KichThuoc { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> Gia { get; set; }
        public Nullable<int> IDSex { get; set; }
        public Nullable<int> IDCate { get; set; }
        public Nullable<int> IDBrand { get; set; }
        public Nullable<int> IDAge { get; set; }
    
        public virtual Age Age { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Sex Sex { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VoteLog> VoteLogs { get; set; }
    }
}
