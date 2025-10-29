using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helsinki.Infrastructure.Options
{
    /// <summary>Options for Open Exchange Rates integration.</summary>
    public class OpenExchangeRatesOptions
    {
        public const string SectionName = "OpenExchangeRates";
        /// <summary>API key for openexchangerates.org.</summary>
        public string? ApiKey { get; set; }
    }
}
