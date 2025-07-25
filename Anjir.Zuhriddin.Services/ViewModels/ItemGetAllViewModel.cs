﻿using Anjir.Zuhriddin.Items.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjir.Zuhriddin.Items.Services.ViewModels;

public class ItemGetAllViewModel
{
    public int Total { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public List<ItemResultModel> Items { get; set; }
}
