using FICCI_API.DTO;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace FICCI_API.Controller.API
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class PurchaseInvoiceController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public PurchaseInvoiceController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PurchaseInvoiceDTO data)
        {
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string? HeaderPiNo = null;
                    var year = DateTime.Now.Year;
                    var prefix = $"HPI/{year}/";
                    var lastPurchase = await _dbContext.FicciImpiHeaders.Where(a => a.ImpiHeaderPiNo.StartsWith(prefix)).OrderByDescending(c => c.ImpiHeaderId).FirstOrDefaultAsync();
                    int nextnumber = 1;
                    if(lastPurchase != null && int.TryParse(lastPurchase.ImpiHeaderPiNo.Split('/').Last(), out int lastNumber))
                    {
                        nextnumber = lastNumber + 1;
                    }
                    HeaderPiNo = $"{prefix}{nextnumber:D4}";
                    data.HeaderPiNo = HeaderPiNo;
                   // data.Attachment = fileToUpload;
                    var PurchaseInvoice = new FicciImpiHeader
                    {
                        ImpiHeaderPiNo = data.HeaderPiNo,
                        ImpiHeaderInvoiceType = data.InvoiceType,
                        ImpiHeaderProjectCode = data.ProjectCode,
                        ImpiHeaderDepartment = data.Department,
                        ImpiHeaderDivison = data.Division,
                        ImpiHeaderPanNo = data.PAN,
                        ImpiHeaderGstNo = data.GST,
                        ImpiHeaderCustomerAddress = data.Address,
                        ImpiHeaderCustomerCode = data.CustomerCode,
                        ImpiHeaderCustomerCity = data.City,
                        ImpiHeaderCustomerName = data.CustomerName,
                        ImpiHeaderCustomerGstNo = data.CustomerGST,
                        ImpiHeaderCustomerPinCode = data.Pincode,
                        ImpiHeaderCustomerContactPerson = data.ContactPerson,
                        ImpiHeaderCustomerEmailId = data.Email,
                        ImpiHeaderCustomerPhoneNo =data.Phone,
                        ImpiHeaderCreatedOn = DateTime.Now,
                        ImpiHeaderPaymentTerms = data.PaymentTerms,
                        ImpiHeaderRemarks = data.Remarks,
                        IsDraft = true,
                    };
                    _dbContext.FicciImpiHeaders.Add(PurchaseInvoice);
                    await _dbContext.SaveChangesAsync();

                    if(data.LineItems!= null)
                    {
                        foreach(var item in data.LineItems)
                        {
                            string? LinePiNo = null;
                            var lineprefix = $"LPI/{year}/";
                            var lastPurchaseline = await _dbContext.FicciImpiLines.Where(a => a.ImpiLinePiNo.StartsWith(lineprefix)).OrderByDescending(c => c.ImpiLineId).FirstOrDefaultAsync();
                            if (lastPurchase != null && int.TryParse(lastPurchaseline.ImpiLinePiNo.Split('/').Last(), out int lineNumber))
                            {
                                nextnumber = lineNumber + 1;
                            }
                            LinePiNo = $"{lineprefix}{nextnumber:D4}";
                            item.LinePiNo = LinePiNo ;
                            _dbContext.FicciImpiLines.Add(new FicciImpiLine
                            {
                                ImpiLineDescription =item.Description,
                                ImpiLineQuantity = item.Quantity,
                                ImpiLineUnitPrice = item.UnitPrice,
                                ImpiLineDiscount = item.Discount,
                                ImpiLineAmount = item.Amount,
                                ImpiLinePiNo = item.LinePiNo,
                                PiHeaderId = PurchaseInvoice.ImpiHeaderId,
                                ImpiLineCreatedOn = DateTime.Now
                            });
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    var response = new
                    {
                        status = true,
                        message = "Data Insert successfully",
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
