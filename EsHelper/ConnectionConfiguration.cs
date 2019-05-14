using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace EsHelper
{
    public class ConnectionConfigurationSingle
    {
        public Uri ConnectionUri { get; set; }

        public ConnectionSettings ConnectionSettingsHelper { get; set; }

        public ElasticClient ElasticClientHelper { get; set; }
    }
}
