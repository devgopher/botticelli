using Botticelli.Locations.Models;
using Mapster;
using Nominatim.API.Models;

namespace Botticelli.Locations.Mapping;

public class AddressMappingRegister : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        // Put your mapping logic here
        config
                .NewConfig<AddressResult, Address>()
                .IgnoreNullValues(true)
                .IgnoreNonMapped(true);


    }
}