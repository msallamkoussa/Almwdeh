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
    
    public partial class StudentsTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentsTbl()
        {
            this.StudentsUniversitiesTbls = new HashSet<StudentsUniversitiesTbl>();
        }
    
        public int UserAccessIDs { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> Sex { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MobileSyria { get; set; }
        public string MobileIran { get; set; }
        public string WhatsappNumber { get; set; }
        public string Photo { get; set; }
        public string Doctrine { get; set; }
        public string CitySyria { get; set; }
        public string AddressSyria { get; set; }
        public string CityIran { get; set; }
        public string AddressIran { get; set; }
        public Nullable<bool> MaritalStatus { get; set; }
        public string WifeName { get; set; }
        public string ChildernNumber { get; set; }
        public string WifeDetails { get; set; }
        public string PassportNumber { get; set; }
        public Nullable<System.DateTime> PassportExpireDate { get; set; }
        public string PassportFile { get; set; }
        public string ResidentFile { get; set; }
        public Nullable<System.DateTime> ResidentExpireDate { get; set; }
        public string FinancialStatus { get; set; }
        public Nullable<bool> NeedMonthlyAid { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankCardNumber { get; set; }
        public string BankShibaNumber { get; set; }
        public Nullable<bool> Working { get; set; }
        public string WorkDetails { get; set; }
        public string Skills { get; set; }
        public string PreviousJobs { get; set; }
        public Nullable<bool> ShahidFamily { get; set; }
        public string ShahidRelationship { get; set; }
        public string ReligiousCommitment { get; set; }
        public string GeneralBehavior { get; set; }
        public string TheIdentifier { get; set; }
        public string EffectivenessAndActivity { get; set; }
        public string DisciplineSituation { get; set; }
        public string AdditionalInformation { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public Nullable<bool> EditVerified { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentsUniversitiesTbl> StudentsUniversitiesTbls { get; set; }
    }
}
