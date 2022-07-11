﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Config
{
    public class CloudinaryConfig
    {
        public string Cloudinary { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public int CoverLimitWidth { get; set; }

        public int CoverLimitHeight { get; set; }
    }
}
