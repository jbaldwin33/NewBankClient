using GalaSoft.MvvmLight;
using NewBankShared.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.CommandWpf;
using NewBankClientConfiguration.Models;
using NewBankShared;

namespace NewBankClientConfiguration.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private static readonly string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ClientConfiguration.xml");
    private bool localConnection = true;
    private string endpoint;
    private int port;
    private RelayCommand saveCommand;

    public MainViewModel()
    {
      if (File.Exists(filename))
        LoadFromFile();

    }

    public string SaveLabel => new SaveTranslatable();

    public bool LocalConnection
    {
      get => localConnection;
      set => Set(ref localConnection, value);
    }


    public string Endpoint
    {
      get => endpoint;
      set => Set(ref endpoint, value);
    }

    public int Port
    {
      get => port;
      set => Set(ref port, value);
    }

    public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);

    private bool SaveCommandCanExecute()
    {
      return true;
    }

    private void SaveCommandExecute()
    {
      var serializer = new XmlSerializer(typeof(ConfigurationModel));
      var stream = File.Create(filename);
      var model = new ConfigurationModel
      {
        LocalConnection = LocalConnection,
        Endpoint = Endpoint,
        Port = Port
      };
      serializer.Serialize(stream, model);
    }


    private void CreateConfigFile()
    {
      throw new NotImplementedException();
    }

    private void LoadFromFile()
    {
      var serializer = new XmlSerializer(typeof(ConfigurationModel));
      using var stream = new StreamReader(filename);
      var model = serializer.Deserialize(stream) as ConfigurationModel;
      LocalConnection = model.LocalConnection;
      Endpoint = model.Endpoint;
      Port = model.Port;
    }



  }
}
