﻿using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class OnOffCardStatusRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String fi { get; set; }
        public String abaBranch { get; set; }
        public String pan { get; set; }
        public String accountType { get; set; }
        public String status { get; set; }
    }
}
