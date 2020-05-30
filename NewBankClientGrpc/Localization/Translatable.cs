using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static NewBankClientGrpc.Localization.Localization.TranslatableElement;

namespace NewBankClientGrpc.Localization
{
  public abstract class Translatable
  {
    private Object[] parameters;
    private TranslationBasis[] translationBasisArray;

    protected Translatable(params Object[] parameters)
    {
      this.parameters = parameters;
      translationBasisArray = TranslationCache.Get(this);
    }

    protected abstract String GetDefaultTransalationBasisText();

    internal TranslationBasis DefaultTranslationBasis
    {
      get { return new TranslationBasis(new Int32[] { 1033 }, GetDefaultTransalationBasisText()); }
    }

    public override String ToString()
    {
      return ToTranslation().Text;
    }

    public Translation ToTranslation()
    {
      var result = ToTranslation(TranslationCache.PreferredLCID);
      if (result == null)
        result = new Translation(GetDefaultTransalationBasisText(), parameters);
      return result;
    }

    public virtual Translation ToTranslation(Int32 lcid)
    {
      if (translationBasisArray != null)
      {
        foreach (TranslationBasis translationBasis in translationBasisArray)
          if (translationBasis != null)
            if (translationBasis.LCIDArray.Contains(lcid))
              return new Translation(lcid, translationBasis.Text, GetDefaultTransalationBasisText(), parameters);
      }
      return new Translation(GetDefaultTransalationBasisText(), parameters);
    }

    public virtual Translation[] ToTranslations()
    {
      List<Translation> result = new List<Translation>();
      foreach (TranslationBasis translationBasis in translationBasisArray)
      {
        if (translationBasis != null)
        {
          foreach (Int32 lcid in translationBasis.LCIDArray)
            result.Add(new Translation(lcid, translationBasis.Text, GetDefaultTransalationBasisText(), parameters));
        }
      }

      // remove any duplicates before returning
      for (Int32 idx1 = 0; idx1 < result.Count; idx1++)
        for (Int32 idx2 = result.Count - 1; idx2 > idx1; idx2--)
          if (result[idx2].LCID == result[idx1].LCID)
            result.RemoveAt(idx2);

      // always guarantee 1033 is available
      Boolean enUSExists = false;
      foreach (Translation translation in result)
        if (translation.LCID == 1033)
        {
          enUSExists = true;
          break;
        }
      if (!enUSExists)
        result.Add(new Translation(GetDefaultTransalationBasisText(), parameters));

      return result.ToArray();
    }

    public static implicit operator String(Translatable translatable)
    {
      return translatable.ToString();
    }

    //public static implicit operator Translatable(System.ServiceModel.FaultException faultException)
    //{
    //  return new FaultExceptionTranslatable(faultException);
    //}
  }

  //public class FaultExceptionTranslatable : Translatable
  //{
  //  private Dictionary<Int32, Translation> translationDictionary = new Dictionary<int, Translation>();

  //  public FaultExceptionTranslatable(System.ServiceModel.FaultException faultException) : base(faultException)
  //  {
  //    if (faultException == null)
  //      throw new ArgumentNullException(nameof(faultException));

  //    foreach (var faultReasonText in faultException.Reason.Translations)
  //    {
  //      try
  //      {
  //        CultureInfo cultureInfo = CultureInfo.GetCultureInfo(faultReasonText.XmlLang);
  //        translationDictionary[cultureInfo.LCID] = new Translation(cultureInfo, faultReasonText.Text);
  //      }
  //      catch (CultureNotFoundException) { }
  //    }

  //    if (!translationDictionary.ContainsKey(1033))
  //      translationDictionary[1033] = new Translation(CultureInfo.GetCultureInfo(1033), faultException.Message);
  //  }

  //  public override Translation[] ToTranslations()
  //  {
  //    return translationDictionary.Values.ToArray();
  //  }

  //  public override Translation ToTranslation(int lcid)
  //  {
  //    if (translationDictionary.ContainsKey(lcid))
  //      return translationDictionary[lcid];
  //    else
  //      return translationDictionary[1033];
  //  }

  //  protected override string GetDefaultTransalationBasisText()
  //  {
  //    throw new NotImplementedException();
  //  }
  //}
}
