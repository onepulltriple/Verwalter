using AnwendungWPF.Models;

namespace AnwendungWPF.ViewModels
{
    public class AddressViewModel : ViewModelBase
    {
        public AddressModel Model { get; }

        public AddressViewModel(AddressModel model)
        {
            Model = model;
        }

        public Guid AddressId => Model.AddressId;

        public Guid PlaceId => Model.PlaceId;

        public string AddressLine1 => Model.AddressLine1;

        public string? AddressLine2 => Model.AddressLine2;

        public string? AddressLine3 => Model.AddressLine3;

        public string? Name1 => Model.Name1;

        public string? Name2 => Model.Name2;
    }
}