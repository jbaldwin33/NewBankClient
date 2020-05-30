using System;
using System.Collections.Generic;
using System.Text;

namespace NewBankClientGrpc.Localization
{
  public class TranslationBasis
  {
    private Int32[] lcidArray;

    public Int32[] LCIDArray => lcidArray;

    private string text;
    public string Text => text;

    public TranslationBasis(Int32[] lcidArray, string text)
    {
      this.lcidArray = lcidArray;
      this.text = text;
    }
  }
}
