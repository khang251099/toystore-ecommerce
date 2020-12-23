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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Shippings = new HashSet<Shipping>();
        }
    
        public int IDOrder { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<int> IDKhachHang { get; set; }
        public Nullable<int> IDMethod { get; set; }
        public string Address_Cus { get; set; }
        public string Status { get; set; }
        public string Phone_Cus { get; set; }
        public string GhiChu { get; set; }
        public Nullable<int> PhiShip { get; set; }
        public Nullable<int> IDGioHang { get; set; }
        public string FullName { get; set; }
        public string DiaChi_Cus { get; set; }
        public string Email { get; set; }
        public string MethodName { get; set; }
        public string StatusPayment { get; set; }
        public Nullable<double> PaypalAmount { get; set; }
        public Nullable<double> GiamGia { get; set; }
        public Nullable<double> ThueVAT { get; set; }
        public Nullable<double> TongTien { get; set; }
        public Nullable<double> ThanhTien { get; set; }
    
        public virtual GioHang GioHang { get; set; }
        public virtual KhachHang KhachHang { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual PayMethod PayMethod { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shipping> Shippings { get; set; }
    }
}
