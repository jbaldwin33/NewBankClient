using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace NewBankClientGrpc.Localization
{
  [DataContract]
  public class Translation
  {
    public Translation() { }

    internal Translation(CultureInfo cultureInfo, String text)
    {
      LCID = cultureInfo.LCID;
      Text = text;
    }

    internal Translation(String defaultText, params Object[] parameters)
    {
      LCID = 1033;
      Text = String.Format(defaultText, parameters);
    }

    internal Translation(Int32 lcid, String text, String defaultText, params Object[] parameters)
    {
      LCID = lcid;
      try
      {
        Text = String.Format(text, parameters);
      }
      catch
      {
        LCID = 1033;
        Text = String.Format(defaultText, parameters);
      }
    }

    [DataMember]
    public Int32 LCID { get; set; }

    [DataMember]
    public String Text { get; set; }

    public override String ToString() => Text;
  }
}