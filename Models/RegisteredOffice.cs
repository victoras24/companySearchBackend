using System;
using System.Collections.Generic;
using CompanySearchBackend.Dtos;

namespace CompanySearchBackend.Models;

public partial class RegisteredOffice : AddressAndOfficialsDto
{
    public string? AddressSeqNo { get; set; }

}
