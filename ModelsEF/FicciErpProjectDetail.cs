﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FICCI_API.ModelsEF;

public partial class FicciErpProjectDetail
{
    public int ProjectId { get; set; }

    public string ProjectCode { get; set; }

    public string ProjectName { get; set; }

    public string ProjectDepartmentCode { get; set; }

    public string ProjectDepartmentName { get; set; }

    public string ProjectDivisionCode { get; set; }

    public string ProjectDivisionName { get; set; }

    public string TlApprover { get; set; }

    public string ChApprover { get; set; }

    public string FinanceApprover { get; set; }

    public string SupportApprover { get; set; }

    public bool? ProjectActive { get; set; }

    public bool IsDelete { get; set; }

    public string DimensionCode { get; set; }
}