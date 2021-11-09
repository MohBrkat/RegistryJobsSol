using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundraisingOrdersJob.Models
{
    [Keyless]
    public class Configuration
    {
        public string ParameterName { get; set; }
        public string Value { get; set; }
        public int ClientId { get; set; }
        public string FeatureKey { get; set; }
    }
}
