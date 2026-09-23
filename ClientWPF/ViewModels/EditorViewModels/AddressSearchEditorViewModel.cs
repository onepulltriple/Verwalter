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
    }
}
