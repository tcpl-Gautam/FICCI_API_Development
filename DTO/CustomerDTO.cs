using FICCI_API.Models;
using FICCI_API.ModelsEF;
using System.Text.Json.Serialization;

namespace FICCI_API.DTO
{
    public class CustomerDTO
    {
        public bool isupdate { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string PaymentMethod { get; set; }
        public string? Location { get; set; }
        public string? VatRegistration { get; set; }
        public string? GenBusPost { get; set; }
        public string? PinCode { get; set; }
        public string Email { get; set; }
        public bool? IsDraft { get; set; }
        public string? PAN { get; set; }
        public string? GSTNumber { get; set; }
        public string? TLApprover { get; set; }
        public string? CLApprover { get; set; }
        //public CityInfo? City { get; set; }
        //public StateInfo? State { get; set; }
        //public CountryInfo? Country { get; set; }
        public GSTCustomerTypeInfo? GstType { get; set; }
    }


    public class CustomerPostUpdate
    {
        [JsonPropertyName("No")]
        public string No { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Name2")]
        public string Name2 { get; set; }
        [JsonPropertyName("Address")]
        public string Address { get; set; }
        [JsonPropertyName("Address2")]
        public string Address2 { get; set; }
        [JsonPropertyName("City")]
        public string City { get; set; }
        [JsonPropertyName("Contact")]
        public string Contact { get; set; }
        [JsonPropertyName("PostCode")]
        public string PostCode { get; set; }
        [JsonPropertyName("State_Code")]
        public string State_Code { get; set; }
        [JsonPropertyName("Country_Region_Code")]
        public string Country_Region_Code { get; set; }
        [JsonPropertyName("GSTRegistrationNo")]
        public string GSTRegistrationNo { get; set; }
        [JsonPropertyName("GSTCustomerType")]
        public string GSTCustomerType { get; set; }
        [JsonPropertyName("PAN_No")]
        public string PAN_No { get; set; }
        [JsonPropertyName("EMail")]
        public string EMail { get; set; }
        [JsonPropertyName("PrimaryContactNo")]
        public string PrimaryContactNo { get; set; }

        //public Boolean Status { get; set; }
        //public string Message { get; set; }
        //public List<CustomerPost> Data { get; set; }


    }
    public class CustomerList
    {
        public int CustomerId { get; set;}
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Pincode { get; set; }
        public string CustomerLastName { get; set; }
        public string Address2 { get; set; }
        public string Contact { get; set; }
        public string? GSTNumber { get; set; }
        //public int GSTCustomerType { get; set; }
        public string? PAN { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDraft { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? TLApprover { get; set; }
        public string? CLApprover { get; set; }
   //     public string? SGApprover { get; set; }
        public string? CustomerStatus { get; set; }

        public string? customerRemarks { get; set; }

        public string? accountsRemarks { get; set; }

   
        public int? CustomerStatusId { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        //public CityInfo? City { get; set; }
        //public StateInfo? State { get; set; }
        //public CountryInfo? Country { get; set; }
        public CityInfo? CityList { get; set; }
        public StateInfo? StateList { get; set; }
        public CountryInfo? CountryList { get; set; }
        public GSTCustomerTypeInfo? GstType { get; set; }
        public List<FicciImwd>? WorkFlowHistory {  get; set; }
    }
    public class CityInfo
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string cityCode { get; set; }
        //public StateInfo? State { get; set; }
    }
    public class StateInfo
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string stateCode { get; set; }
        // public CountryInfo? Country { get; set; }

    }
    public class CountryInfo
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string countryCode { get; set; }

    }

    public class GSTCustomerTypeInfo
    {
        public int GstTypeId { get; set; }
        public string GstTypeName { get; set; }
    }
    public class ErpDetailCustNo
    {
        public string GstNumber { get; set; }
    }

    public class GstRegistrationNo
    {
        public string CustNo { get; set; }
        public string? Code { get; set; }
        public string? CustName { get; set; }
        public string? CustName2 { get; set; }
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? Contact { get; set; }
        public string? PinCode { get; set; }
        public string? StateCode { get; set; }
        public string? CountryCode { get; set; }
        public string GstNumber { get; set; }
        public string? GstCustomerType { get; set; }
        public string? PAN { get; set; }
        public string? Email { get; set; }
        public string? PrimaryContact { get; set; }
    }

    public class new_Customer
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<CustomerRequest> Data { get; set; }
    }

    public class CustomerResponse
    {
        //[JsonProperty("@odata.context")]
        //public string ODataContext { get; set; }

        //[JsonProperty("@odata.etag")]
        //public string ODataEtag { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string PostCode { get; set; }
        public string State_Code { get; set; }
        public string Country_Region_Code { get; set; }
        public string GSTRegistrationNo { get; set; }
        public string GSTCustomerType { get; set; }
        public string PAN_No { get; set; }
        public string EMail { get; set; }
        public string PrimaryContactNo { get; set; }

    }


   

    public class CustomerPost
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Name2")]
        public string Name2 { get; set; }
        [JsonPropertyName("Address")]
        public string Address { get; set; }
        [JsonPropertyName("Address2")]
        public string Address2 { get; set; }
        [JsonPropertyName("City")]
        public string City { get; set; }
        [JsonPropertyName("Contact")]
        public string Contact { get; set; }
        [JsonPropertyName("PostCode")]
        public string PostCode { get; set; }
        [JsonPropertyName("State_Code")]
        public string State_Code { get; set; }
        [JsonPropertyName("Country_Region_Code")]
        public string Country_Region_Code { get; set; }
        [JsonPropertyName("GSTRegistrationNo")]
        public string GSTRegistrationNo { get; set; }
        [JsonPropertyName("GSTCustomerType")]
        public string GSTCustomerType { get; set; }
        [JsonPropertyName("PAN_No")]
        public string PAN_No { get; set; }
        [JsonPropertyName("EMail")]
        public string EMail { get; set; }
        [JsonPropertyName("PrimaryContactNo")]
        public string PrimaryContactNo { get; set; }

        //public Boolean Status { get; set; }
        //public string Message { get; set; }
        //public List<CustomerPost> Data { get; set; }


    }


    public class CustomerRequest
    {
        public bool isupdate { get; set; } //update
        public int? CustomerId { get; set; } //update
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? Contact { get; set; }
        public string? Phone { get; set; }
        public string? PinCode { get; set; }
        public string? Email { get; set; }
      //  public int Cityid { get; set; }
        public bool IsDraft { get; set; } //check submit
        public string? GSTNumber { get; set; }
        public string? PAN { get; set; }
        public int GSTCustomerType { get; set; }
        public string LoginId { get; set; }
        public int? CustomerStatus { get; set; }
        public string? CustomerRemarks { get; set; }
        public string RoleName { get; set; }
        public string? CityCode { get; set; }
        public string? StateCode { get; set; }
        public string? CountryCode { get; set; }


    }

    public class Ficci_imwd
    {
        public string ImwdScreenName { get; set; }

        public string ImwdInitiatedBy { get; set; }

        public string ImwdRole { get; set; }

        public string ImwdRemarks { get; set; }

        public string ImwdStatus { get; set; }

        public string ImwdPendingAt { get; set; }

        public string ImwdCreatedBy { get; set; }

        public DateTime ImwdCreatedOn { get; set; }

        public int? CustomerId { get; set; }
    }


    public class Approval_Customer
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<Approval_CustomerValue> Data { get; set; }

    }
    public class Approval_CustomerValue
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Pincode { get; set; }
        public string CustomerLastName { get; set; }
        public string Address2 { get; set; }
        public string Contact { get; set; }
        public string? GSTNumber { get; set; }
       // public int GSTCustomerType { get; set; }
        public string? PAN { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDraft { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? TLApprover { get; set; }
        public string? CLApprover { get; set; }
        public string? CustomerStatus { get; set; }
        public int? CustomerStatusId { get; set; }
        public string LastUpdateBy { get; set; }

        public CityInfo? CityList { get; set; }
        public StateInfo? StateList { get; set; }
        public CountryInfo? CountryList { get; set; }
        public GSTCustomerTypeInfo? GstType { get; set; }
    }

}
