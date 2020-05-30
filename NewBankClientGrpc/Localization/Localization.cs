using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NewBankClientGrpc.Localization
{
  public class Localization
  {
    [DataMember]
    public Int32 PreferredLCID { get; set; }

    [DataMember]
    public TranslatableElement[] Items { get; set; }


    public Localization()
    {
      Items = new TranslatableElement[0];
    }

    public class TranslatableElement
    {
      [DataMember]
      public string Name { get; set; }

      [DataMember]
      public Translation[] Items { get; set; }

      public TranslatableElement()
      {
        Name = string.Empty;
        Items = new Translation[0];
      }

      public class Translation
      {
        [DataMember]
        public string Text { get; set; }
        public Translation()
        {
          LCIDArray = new Int32[0];
          Text = string.Empty;
        }

        internal Int32[] LCIDArray { get; set; }
        [DataMember]
        public string LCID
        {
          get => string.Join(",", LCIDArray ?? new int[0]);
          set
          {
            var lcidList = new List<int>();
            foreach (var item in value.Split(new Char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries))
              lcidList.Add(Int32.Parse(item));
            LCIDArray = lcidList.ToArray();
          }
        }
      }
    }
  }
}
