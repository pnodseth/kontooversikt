using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using BlazorApp.Data.Models;
using BlazorApp.Store;
using IdentityModel.Client;

namespace BlazorApp;

public class AccountsService
{
    private readonly IConfiguration _configuration;
    private readonly AppState _appState;

    private HttpClient HttpClient { get; set; } = new HttpClient();

    public AccountsService(IConfiguration configuration, AppState appState)
    {
        _configuration = configuration;
        _appState = appState;
        var myCreator = new TextWriterTraceListener(Console.Out);
        Trace.Listeners.Add(myCreator);
    }

    public async Task Connect()
    {
        // First: get the OpenId configuration from Sbanken.
        var discoHttpClient = new HttpClient();
        var discoveryEndpoint = _configuration.GetValue<string>("DiscoveryEndpoint");
        var discoveryDocumentResponse = await discoHttpClient.GetDiscoveryDocumentAsync(discoveryEndpoint);


        if (discoveryDocumentResponse.Error != null)
        {
            throw new Exception(discoveryDocumentResponse.Error);
        }

        // The application now knows how to talk to the token endpoint.

        var tokenClient = new HttpClient();

        // Second: the application authenticates against the token endpoint
        var clientId = _configuration.GetValue<string>("ClientId");
        var clientSecret = _configuration.GetValue<string>("ClientSecret");
        if (clientId is null || clientSecret is null)
        {
            throw new Exception("ClientId or ClientSecret is missing");
        }

        var tokenRequest = new ClientCredentialsTokenRequest()
        {
            Address = discoveryDocumentResponse.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
        {
            throw new Exception(tokenResponse.ErrorDescription);
        }

        if (tokenResponse.AccessToken is null)
        {
            throw new Exception("Access token is null");
        }
        // The application now has an access token.

        var apiBaseAddress = _configuration.GetValue<string>("ApiBaseAddress");
        if (apiBaseAddress is null)
        {
            throw new Exception("ApiBaseAddress is missing");
        }

        HttpClient = new HttpClient()
        {
            BaseAddress = new Uri(apiBaseAddress)
        };

        // Finally: Set the access token on the connecting client. 
        // It will be used with all requests against the API endpoints.
        this.HttpClient.SetBearerToken(tokenResponse.AccessToken);
    }

    public async Task GetCustomers()
    {
        // The application retrieves the customer's information.
        var customerResponse = await this.HttpClient.GetAsync("api/v1/Customers");
        var customerResult = await customerResponse.Content.ReadAsStringAsync();

        Trace.WriteLine($"CustomerResult:{customerResult}");
    }

    public async Task<AccountItem?> GetAccountInfo(string name)
    {
        // The application retrieves the customer's accounts.
        var accountResponse = await this.HttpClient.GetAsync("api/v1/Accounts");

        var accountResult = await accountResponse.Content.ReadAsStringAsync();

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var accountsList = JsonSerializer.Deserialize<AccountsList>(accountResult, serializeOptions);
        var test = accountsList?.Items.Find(a => a.Name.Contains(name));
        if (test is null)
        {
            throw new Exception("Imre is null");
        }

        // Trace.WriteLine($"AccountResult:{accountResult}");


        var spesificAccountResponse =
            await HttpClient.GetAsync($"api/v1/Accounts/{test.AccountId}");
        var spesificAccountResult = await spesificAccountResponse.Content.ReadAsStringAsync();
        var account = JsonSerializer.Deserialize<AccountResponse>(spesificAccountResult, serializeOptions);
        return account?.Item;
    }

    public async Task TransferFromMainAccount(Children child, int amount, string message)
    {
        // todo: Move to environment variables
        var mainAccountId = "E30501EA273E7A838E2C9522239917FF";
        var imreAccountId = "23BF8A2419C862D26F891AD49BAA93B3";
        var klaraAccountId = "B44FBC5DAA9EBE0873BBEED87EE99CA4";

        var transferToId = child == Children.Imre ? imreAccountId : klaraAccountId;

        await this.Connect();

        var data = new TransferDto(mainAccountId, transferToId, message, amount);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        var jsonString = JsonSerializer.Serialize(data, options);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response =
            await HttpClient.PostAsync($"api/v1/Transfers", content);
        Console.WriteLine(response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                Console.WriteLine(errorMessage);
            }
            throw new Exception("Error transferring money");
        }
        else
        {
            if (child == Children.Imre)
            {
                _appState.AvailableImre += amount;
            }
            else
            {
                _appState.AvailableKlara += amount;
            }
        }
    }
}

public record TransferDto(string FromAccountId, string toAccountId, string Message, int amount);