using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Enums
{
    public enum ShippingProvider
    {
        // FedEx Available Regions
        FedEx,         // Azerbaijan, Turkey, Georgia

        // Azerbaijan
        Azerpost,      // Azərpoçt
        PashaExpress,  // Pasha Express

        // Russia (FedEx Not Available)
        CDEK,          // CDEK Logistics
        RussianPost,   // Russian Post
        Boxberry,      // Boxberry Logistics

        // Iran (FedEx Not Available)
        Tipax,         // Tipax Courier
        IranPost,      // Post Company of Iran

        // Turkey
        PTTKargo,      // PTT Kargo
        YurtiçiKargo,  // Yurtiçi Kargo
        ArasKargo,     // Aras Kargo

        // Georgia
        GeorgianPost,  // Georgian Post
        Cargoline,     // Cargoline

        Other          // Other or unspecified carrier
    }
}
