using System;
using System.IO;
using System.Reflection;
using MVVMFramework.ViewModels;
using MVVMFramework.Localization;
using System.Xml.Serialization;
using NewBankShared;

namespace NewBankClientConfiguration.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private static readonly string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ClientConfiguration.xml");
        private bool localConnection = true;
        private string endpoint;
        private int port;
        private RelayCommand saveCommand;

        public bool LocalConnection
        {
            get => localConnection;
            set => SetProperty(ref localConnection, value);
        }

        public string Endpoint
        {
            get => endpoint;
            set => SetProperty(ref endpoint, value);
        }

        public int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        public string SaveLabel => new SaveTranslatable();


        public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(SaveCommandExecute, SaveCommandCanExecute);

        public MainViewModel()
        {
            if (File.Exists(filename))
                LoadFromFile();
            else
                CreateConfigFile();
        }

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
