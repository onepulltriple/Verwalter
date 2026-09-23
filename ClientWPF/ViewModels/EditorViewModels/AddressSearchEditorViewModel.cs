using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnwendungWPF.Utilities;

namespace AnwendungWPF.ViewModels.EditorViewModels
{
    public class AddressSearchEditorViewModel : ViewModelBase
    {
        private string _addressLine1 = string.Empty;
        private string _name1 = string.Empty;
        private string _cityName = string.Empty;
        private string _countryName = string.Empty;
        private string _postcode = string.Empty;

        public bool IsEditing => true;

        public string AddressLine1
        {
            get => _addressLine1;
            set
            {
                if (_addressLine1 == value)
                    return;

                _addressLine1 = value;

                OnPropertyChanged(nameof(AddressLine1));
            }
        }

        public string Name1
        {
            get => _name1;
            set
            {
                if (_name1 == value)
                    return;

                _name1 = value;

                OnPropertyChanged(nameof(Name1));
            }
        }

        public string CityName
        {
            get => _cityName;
            set
            {
                if (_cityName == value)
                    return;

                _cityName = value;

                OnPropertyChanged(nameof(CityName));
            }
        }

        public string CountryName
        {
            get => _countryName;
            set
            {
                if (_countryName == value)
                    return;

                _countryName = value;

                OnPropertyChanged(nameof(CountryName));
            }
        }

        public string Postcode
        {
            get => _postcode;
            set
            {
                if (_postcode == value)
                    return;

                _postcode = value;

                OnPropertyChanged(nameof(Postcode));
            }
        }
    }
}
