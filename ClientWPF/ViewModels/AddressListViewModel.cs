//using AnwendungWPF.Clients;
//using AnwendungWPF.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AnwendungWPF.ViewModels
//{
//    public class AddressListViewModel : ViewModelBase
//    {
//        private readonly AddressApiClient _addressApiClient;

//        public ObservableCollection<AddressViewModel> Addresses { get; }
//            = new();

//        public AsyncRelayCommand LoadCommand { get; }

//        public AddressListViewModel(AddressApiClient addressApiClient)
//        {
//            _addressApiClient = addressApiClient;

//            LoadCommand = new AsyncRelayCommand(LoadAsync);
//        }

//        private async Task LoadAsync()
//        {
//            var addresses = await _addressApiClient.GetAllAsync();

//            Addresses.Clear();

//            foreach (var address in addresses)
//            {
//                Addresses.Add(new AddressViewModel(address));
//            }
//        }
//    }
//}
