using AnwendungWPF.Models;

namespace AnwendungWPF.ViewModels.EditorViewModels
{
    public class AddressEditorViewModel : ViewModelBase
    {
        private AddressModel? _workingCopyAddress;

        public AddressModel? WorkingCopyAddress
        {
            get => _workingCopyAddress;
            set
            {
                if (_workingCopyAddress == value)
                    return;

                _workingCopyAddress = value;

                OnPropertyChanged(nameof(WorkingCopyAddress));

                OnPropertyChanged(nameof(PlaceId));
                OnPropertyChanged(nameof(AddressLine1));
                OnPropertyChanged(nameof(AddressLine2));
                OnPropertyChanged(nameof(AddressLine3));
                OnPropertyChanged(nameof(Name1));
                OnPropertyChanged(nameof(Name2));

                OnPropertyChanged(nameof(IsEditing));

                Validate();
            }
        }

        public bool IsEditing => WorkingCopyAddress is not null;

        public Guid PlaceId
        {
            get => WorkingCopyAddress?.PlaceId ?? Guid.Empty;
            set
            {
                if (WorkingCopyAddress is null)
                    return;

                if (WorkingCopyAddress.PlaceId == value)
                    return;

                WorkingCopyAddress.PlaceId = value;

                OnPropertyChanged(nameof(PlaceId));

                Validate();
            }
        }

        public string AddressLine1
        {
            get => WorkingCopyAddress?.AddressLine1 ?? string.Empty;
            set
            {
                if (WorkingCopyAddress is null)
                    return;

                if (WorkingCopyAddress.AddressLine1 == value)
                    return;

                WorkingCopyAddress.AddressLine1 = value;

                OnPropertyChanged(nameof(AddressLine1));

                Validate();
            }
        }

        public string AddressLine2
        {
            get => WorkingCopyAddress?.AddressLine2 ?? string.Empty;
            set
            {
                if (WorkingCopyAddress is null)
                    return;

                if (WorkingCopyAddress.AddressLine2 == value)
                    return;

                WorkingCopyAddress.AddressLine2 = value;

                OnPropertyChanged(nameof(AddressLine2));
            }
        }

        public string AddressLine3
        {
            get => WorkingCopyAddress?.AddressLine3 ?? string.Empty;
            set
            {
                if (WorkingCopyAddress is null)
                    return;

                if (WorkingCopyAddress.AddressLine3 == value)
                    return;

                WorkingCopyAddress.AddressLine3 = value;

                OnPropertyChanged(nameof(AddressLine3));
            }
        }

        public string Name1
        {
            get => WorkingCopyAddress?.Name1 ?? string.Empty;
            set
            {
                if (WorkingCopyAddress is null)
                    return;

                if (WorkingCopyAddress.Name1 == value)
                    return;

                WorkingCopyAddress.Name1 = value;

                OnPropertyChanged(nameof(Name1));
            }
        }

        public string Name2
        {
            get => WorkingCopyAddress?.Name2 ?? string.Empty;
            set
            {
                if (WorkingCopyAddress is null)
                    return;

                if (WorkingCopyAddress.Name2 == value)
                    return;

                WorkingCopyAddress.Name2 = value;

                OnPropertyChanged(nameof(Name2));
            }
        }

        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                if (_errorMessage == value)
                    return;

                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsValid { get; private set; }

        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyAddress is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (WorkingCopyAddress.PlaceId == Guid.Empty)
            {
                ErrorMessage = "Place is required.";
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyAddress.AddressLine1))
            {
                ErrorMessage = "Address line 1 is required.";
                RaiseValidityChanged();
                return;
            }

            IsValid = true;
            RaiseValidityChanged();
        }

        private void RaiseValidityChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void BeginCreate()
        {
            WorkingCopyAddress = new AddressModel
            {
                AddressLine1 = string.Empty,
                AddressLine2 = string.Empty,
                AddressLine3 = string.Empty,
                Name1 = string.Empty,
                Name2 = string.Empty,
                PlaceId = Guid.Empty
            };
        }

        public void BeginEdit(AddressModel address)
        {
            WorkingCopyAddress = new AddressModel
            {
                AddressId = address.AddressId,
                PlaceId = address.PlaceId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                Name1 = address.Name1,
                Name2 = address.Name2
            };
        }

        public void Cancel()
        {
            WorkingCopyAddress = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}