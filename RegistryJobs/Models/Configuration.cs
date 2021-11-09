using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RegistryJob.Models
{
    [Keyless]
    public class Configuration
    {
        [Key]
        [Column(Order = 1)]
        public string ParameterName { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ClientId { get; set; }
        public string Value { get; set; }
        public string FeatureKey { get; set; }
    }
}
