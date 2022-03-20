// See https://aka.ms/new-console-template for more information
using HttpClientSample;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

string apiBaseUri = "https://localhost:7026";
string loginPath = "/api/v2/login";
string animalsPath = "/api/EndangeredAnimal";
HttpClient httpClient = new HttpClient();


 async Task<T> ReadResponseContentAsync<T>(HttpContent httpContent)
{
    using (var stream = await httpContent.ReadAsStreamAsync())
    using (var streamReader = new StreamReader(stream))
    using (var jsonReader = new JsonTextReader(streamReader))
    {
        var serializer = new JsonSerializer();
        return serializer.Deserialize<T>(jsonReader);

    }
}

 async Task<string> GetTokenAsync()
{
    var authenticateRequest = new AuthenticateRequest
    {
        Email = "tamas.varga@rebeldot.com",
        Password = "password"
    };

    var bodyJson = JsonConvert.SerializeObject(authenticateRequest);
    var stringContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
    using (var hc = new HttpClient())
    {
        hc.BaseAddress = new Uri(apiBaseUri);
        hc.DefaultRequestHeaders.Accept.Clear();
        hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage responseMessage = await hc.PostAsync(loginPath, stringContent);

        if (responseMessage.IsSuccessStatusCode)
        {
            return await ReadResponseContentAsync<string>(responseMessage.Content);
        }
        return null;
    }
}
 
async Task<EndangeredAnimal> GetEndangeredAnimalAsync(string id)
{
    var path = $"{animalsPath}/{id}";
    HttpResponseMessage responseMessage = await httpClient.GetAsync(path);
    if (responseMessage.StatusCode == HttpStatusCode.OK)
    {
        var result = await ReadResponseContentAsync<EndangeredAnimal>(responseMessage.Content);
        return result;
    } 
    else
    {
        Console.WriteLine(responseMessage.StatusCode);
        var error = await responseMessage.Content.ReadAsStringAsync();
        Console.WriteLine(error.ToString());
        return null;
     }
}

async Task<EndangeredAnimal> CreateAsync(EndangeredAnimal endangeredAnimal)
{
    var bodyJson = JsonConvert.SerializeObject(endangeredAnimal);
    var stringContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
  
    HttpResponseMessage responseMessage = await httpClient.PostAsync(animalsPath,stringContent);
    if (responseMessage.StatusCode == HttpStatusCode.OK)
    {
        return await ReadResponseContentAsync<EndangeredAnimal>(responseMessage.Content);
    }
    else
    {
        Console.Write(responseMessage.StatusCode);
        var error = await responseMessage.Content?.ReadAsStringAsync();
        Console.WriteLine(error.ToString());
        return null;
    }
}

async Task UpdateAsync(EndangeredAnimal endangeredAnimal)
{
    var bodyJson = JsonConvert.SerializeObject(endangeredAnimal);
    var stringContent = new StringContent(bodyJson,Encoding.UTF8, "application/json");
    var path = $"{animalsPath}/{endangeredAnimal.Id}";
    HttpResponseMessage responseMessage = await httpClient.PutAsync(path, stringContent);

    if (!responseMessage.IsSuccessStatusCode)
    {
        Console.WriteLine($"Update {endangeredAnimal.Id}: {responseMessage.StatusCode} ");
        var error = await responseMessage.Content.ReadAsStringAsync();
        Console.WriteLine(error);
    }
}

async Task DeleteAsync(string id)
{
    var path = $"{animalsPath}/{id}";
    HttpResponseMessage responseMessage = await httpClient.DeleteAsync(path);

    if (!responseMessage.IsSuccessStatusCode)
    {
        Console.WriteLine($"Delete{id}: {responseMessage.StatusCode} ");
        var error = await responseMessage.Content.ReadAsStringAsync();
        Console.WriteLine(error);
    }
}

async Task<string> RefreshTokenAsync()
{
    var path = $"{loginPath}";
    HttpResponseMessage responseMessage = await httpClient.GetAsync(path);
    if (responseMessage.IsSuccessStatusCode)
    {
        return await ReadResponseContentAsync<string>(responseMessage.Content);
    }
    return null;
}

async Task RunAsync()
{
    httpClient.BaseAddress = new Uri(apiBaseUri);

    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var bearerToken = await GetTokenAsync();

    Console.WriteLine("Token: {0}", bearerToken);
    Console.WriteLine();
    httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", bearerToken);

    try
    {
        var endangeredAnimal = new EndangeredAnimal
        {
            Id = "",
            Name = "Unicorn"
        };
        var createTask = CreateAsync(endangeredAnimal);

        Console.WriteLine("Calling the API to create an item...");
        var createResult = await createTask;
        Console.WriteLine("Created: {0}",JsonConvert.SerializeObject(createResult));
        Console.WriteLine();

        Console.WriteLine("Calling the API to Get an item...");
        var getTask = GetEndangeredAnimalAsync(createResult.Id);
        var getResult = await getTask;
        Console.WriteLine("Got:{0}",JsonConvert.SerializeObject(getResult));
        Console.WriteLine();

        Console.WriteLine("Calling the API to Update an item...");
        createResult.Name = "Dragon";
        await UpdateAsync(createResult);

        Console.WriteLine("Calling the API to Get an item...");
        var getTaskUpdated = GetEndangeredAnimalAsync(createResult.Id);
        var getResultUpdated = await getTaskUpdated;
        Console.WriteLine("Updated:{0}", JsonConvert.SerializeObject(getResultUpdated));
        Console.WriteLine();

        Console.WriteLine("Calling the API to Delete an item...");
        await DeleteAsync(createResult.Id);
        Console.WriteLine();

        Console.WriteLine("Calling the API to refresh the token...");
        var refreshTokenTask = RefreshTokenAsync();
        var refreshedToken = await refreshTokenTask;
        Console.WriteLine("Refreshed token: {0}", refreshedToken);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.WriteLine("Press Enter to stop...");
    Console.ReadLine();

}

RunAsync().GetAwaiter().GetResult();