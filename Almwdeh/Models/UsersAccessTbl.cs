//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Almwdeh.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersAccessTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsersAccessTbl()
        {
            this.MessagesTbls = new HashSet<MessagesTbl>();
            this.MessagesTbls1 = new HashSet<MessagesTbl>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<int> UserRoleIDs { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> AccessFailedCount { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public Nullable<bool> Verified { get; set; }
        public string ResetPasswordCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessagesTbl> MessagesTbls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessagesTbl> MessagesTbls1 { get; set; }
        public virtual UsersRolesTbl UsersRolesTbl { get; set; }
    }
}
