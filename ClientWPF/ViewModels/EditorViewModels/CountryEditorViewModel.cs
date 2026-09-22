using AnwendungWPF.Models;

namespace AnwendungWPF.ViewModels.EditorViewModels
{
    public class CountryEditorViewModel : ViewModelBase
    {
        private CountryModel? _workingCopyCountry;
        public CountryModel? WorkingCopyCountry
        {
            get => _workingCopyCountry;
            set
            {
                if (_workingCopyCountry == value)
                    return;

                _workingCopyCountry = value;

                OnPropertyChanged(nameof(WorkingCopyCountry));

                OnPropertyChanged(nameof(NameEnGb));
                OnPropertyChanged(nameof(IsoAlpha2));
                OnPropertyChanged(nameof(IsoAlpha3));
                OnPropertyChanged(nameof(Active));

                OnPropertyChanged(nameof(IsEditing));

                Validate();
            }
        }

        public bool IsEditing => WorkingCopyCountry is not null;

        public string NameEnGb
        {
            get => WorkingCopyCountry?.NameEnGb ?? string.Empty;
            set
            {
                if (WorkingCopyCountry is null)
                    return;

                if (WorkingCopyCountry.NameEnGb == value)
                    return;

                WorkingCopyCountry.NameEnGb = value;

                OnPropertyChanged(nameof(NameEnGb));

                Validate();
            }
        }

        public string IsoAlpha2
        {
            get => WorkingCopyCountry?.IsoAlpha2 ?? string.Empty;
            set
            {
                if (WorkingCopyCountry is null)
                    return;

                if (WorkingCopyCountry.IsoAlpha2 == value)
                    return;

                WorkingCopyCountry.IsoAlpha2 = value;

                OnPropertyChanged(nameof(IsoAlpha2));

                Validate();
            }
        }

        public string IsoAlpha3
        {
            get => WorkingCopyCountry?.IsoAlpha3 ?? string.Empty;
            set
            {
                if (WorkingCopyCountry is null)
                    return;

                if (WorkingCopyCountry.IsoAlpha3 == value)
                    return;

                WorkingCopyCountry.IsoAlpha3 = value;

                OnPropertyChanged(nameof(IsoAlpha3));

                Validate();
            }
        }

        public bool Active
        {
            get => WorkingCopyCountry?.Active ?? false;
            set
            {
                if (WorkingCopyCountry is null)
                    return;

                if (WorkingCopyCountry.Active == value)
                    return;

                WorkingCopyCountry.Active = value;
                OnPropertyChanged(nameof(Active));
            }
        }


        // validation state & message
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

        public bool IsValid { get; private set; } = false;

        private void Validate()
        {
            ErrorMessage = string.Empty;
            IsValid = false;

            if (WorkingCopyCountry is null)
            {
                RaiseValidityChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkingCopyCountry.NameEnGb))
            {
                ErrorMessage = "Name is required.";
                RaiseValidityChanged();
                return;
            }

            // no or incorrect ISO codes are allowed for now 
            // TODO validate ISO codes

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
            WorkingCopyCountry = new CountryModel
            {
                NameEnGb = string.Empty,
                IsoAlpha2 = string.Empty,
                IsoAlpha3 = string.Empty,
                Active = true
            };
        }

        public void BeginEdit(CountryModel country)
        {
            WorkingCopyCountry = new CountryModel
            {
                CountryId = country.CountryId,
                NameEnGb = country.NameEnGb,
                IsoAlpha2 = country.IsoAlpha2,
                IsoAlpha3 = country.IsoAlpha3,
                Active = country.Active
            };
        }

        public void Cancel()
        {
            WorkingCopyCountry = null;
            ErrorMessage = string.Empty;
            IsValid = false;
        }
    }
}
