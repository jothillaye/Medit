using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medit
{
    [MetadataType(typeof(TravEntMetaData))]
    public partial class TravEnt
    {
        public class TravEntMetaData { 
            
        }
    }

    [MetadataType(typeof(TravailleurMetaData))]
    public partial class Travailleur
    {
        public class TravailleurMetaData
        {

        }
    }
}