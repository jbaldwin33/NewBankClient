using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace NewBankClientGrpc.Localization
{
  public static class TranslationCache
  {
    private static readonly string localizationFileName = Path.Combine(Environment.CurrentDirectory, "Localization.xml");
    private static readonly Localization programLocalization = new Localization();
    private static readonly int preferredLCID = 0;
    private static XmlSerializer _xmlSerializer;
    private static XmlSerializer xmlSerializer => _xmlSerializer ??= new XmlSerializer(typeof(Localization));
    static TranslationCache()
    {
      var tmpProgramLocalization = LoadLocalizatonFile(localizationFileName);

      programLocalization = tmpProgramLocalization;
      preferredLCID = System.Threading.Thread.CurrentThread.CurrentCulture.LCID;
    }

    public static Localization LoadLocalizatonFile(string filename)
    {
      var tmpLocalization = new Localization();
      if (File.Exists(filename))
      {
        var fileBytes = File.ReadAllBytes(filename);
        if (fileBytes.Length > 2)
        {
          var pre = new Byte[2] { fileBytes[0], fileBytes[1] };
          //if compare byte arrays
          try
          {
            using (TextReader reader = new StringReader(File.ReadAllText(filename)))
              tmpLocalization = xmlSerializer.Deserialize(reader) as Localization;
          }
          catch { }
        }
      }

      // remove empty translatables from programLocalization
      var programTranslatableList = new List<Localization.TranslatableElement>();
      foreach (Localization.TranslatableElement translatable in tmpLocalization.Items)
      {
        if ((translatable.Items == null) || (translatable.Items.Length == 0))
          continue;
        programTranslatableList.Add(translatable);
      }
      tmpLocalization.Items = programTranslatableList.ToArray();

      return tmpLocalization;
    }

    public static void SaveToFile()
    {
      using (var stringWriter = new StringWriter())
      {
        xmlSerializer.Serialize(stringWriter, programLocalization);
        File.WriteAllText(localizationFileName, stringWriter.ToString(), Encoding.Unicode);
      }
    }

    public static Int32 PreferredLCID
    {
      get
      {
        if (preferredLCID == 0)
          return System.Threading.Thread.CurrentThread.CurrentCulture.LCID;
        else
          return preferredLCID;
      }
    }

    public static TranslationBasis[] Get(Translatable translatable)
    {
      foreach (var tanslatableElement in programLocalization.Items)
        if (tanslatableElement.Name == translatable.GetType().FullName)
        {
          var result = new List<TranslationBasis>();
          foreach (Localization.TranslatableElement.Translation basis in tanslatableElement.Items)
            result.Add(new TranslationBasis(basis.LCIDArray, basis.Text));
          return result.ToArray();
        }

      // we only get here if it was not found so we know to add it and save the update.  This only happens in Debug to build the program translatable
      var translatableList = new List<Localization.TranslatableElement>(programLocalization.Items)
      {
        new Localization.TranslatableElement 
        {
          Name = translatable.GetType().FullName, 
          Items = new Localization.TranslatableElement.Translation[1]
          { 
            new Localization.TranslatableElement.Translation
            { 
              LCIDArray = translatable.DefaultTranslationBasis.LCIDArray, 
              Text = translatable.DefaultTranslationBasis.Text 
            }
          }
        }
      };
      programLocalization.Items = translatableList.ToArray();
      programLocalization.PreferredLCID = preferredLCID;
      SaveToFile();
      return new TranslationBasis[1] { translatable.DefaultTranslationBasis };
    }
  }
}
