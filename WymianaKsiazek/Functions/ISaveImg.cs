﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Functions
{
    public interface ISaveImg
    {
        string SaveImage(IFormFile image);
    }
}
