﻿using System.Collections.Generic;

namespace SurveillanceCamera.Models
{
    public class AppSettings
    {
        public string Channels { get; set; }
        public string Stream { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }
    }
}