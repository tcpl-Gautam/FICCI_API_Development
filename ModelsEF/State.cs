﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FICCI_API.ModelsEF;

public partial class State
{
    public int StateId { get; set; }

    public string StateCode { get; set; }

    public string StateName { get; set; }

    public bool? IsActive { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? CreateDate { get; set; }

    public int CountryId { get; set; }
}