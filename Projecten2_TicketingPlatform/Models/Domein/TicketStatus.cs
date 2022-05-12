using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform
{
    public enum TicketStatus
    {
        Aangemaakt, //0
        [Display(Name ="In behandeling")]
        InBehandeling, //1           
        Afgehandeld, //2
        Geannuleerd, //3
        [Display(Name = "Wachten op informatie klant")]
        WachtenOpInformatieKlant, //4
        [Display(Name = "Informatie klant ontvangen")]
        InformatieKlantOntvangen, //5
        [Display(Name = "In development")]
        InDevelopment, //6
        Standaard, //7
        [Display(Name = "Alle tickets")]
        Alle //8
    }
}

public static class EnumExtensions
{
    public static string GetDisplayAttributeFrom(this Enum enumValue, Type enumType)
    {
        MemberInfo info = enumType.GetMember(enumValue.ToString()).First();

        string displayName;
        if (info != null && info.CustomAttributes.Any())
        {
            DisplayAttribute nameAttr = info.GetCustomAttribute<DisplayAttribute>();
            displayName = nameAttr != null ? nameAttr.Name : enumValue.ToString();
        }
        else
        {
            displayName = enumValue.ToString();
        }
        return displayName;
    }
}
