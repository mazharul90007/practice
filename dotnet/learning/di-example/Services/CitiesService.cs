using ServiceContracts;

namespace Services;

public class CitiesService: ICitiesServices, IDisposable
{
    private List<string> _cities;

    //Contructor
    public CitiesService()
    {
        _cities = new List<string>()
        {
            "Dhaka",
            "Chittagong",
            "Rajshahi",
            "Barishal",
            "Khulna",
            "Sylhet"
        };

        //Add logic to open the db connection
    }


    public List<string> GetCities()
    {
        return _cities;
    }

    public void Dispose()
    {
        //Add logic to close the db connection
    }
}
