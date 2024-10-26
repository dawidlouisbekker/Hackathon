using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using System.Windows.Input;
using HackaThon;
using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
using eqonecs;
using HackaThon.encryption;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

public class ApiData : INotifyPropertyChanged
{
    private string _username = "";
    private string _password = "";
    const string HttpServerUrl = "http://13.246.49.85:8080/";
    SymmetricEncryption _symmetricEncryption;
    HttpClient Client { get; set; } = new HttpClient();

    public string UserName
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(UserName));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public ICommand UpdateCommand { get; }

    public ApiData()
    {
        UpdateCommand = new Command(Update);

    }

    private void Update()
    {
        // Logic to update the model or perform actions based on user input
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


  //  public Task<bool> InitConnection()
  //  {
        
 //   }

    public async Task<string> SaveUserCredentialsAsync(Credentials credentials)
    {
        _symmetricEncryption = new SymmetricEncryption();
        try
        {

             Eqone calc = new Eqone();
             credentials.password = calc.Eqautions(1, 2, 3, 4, 0, 1023282783);
             byte[] encryptedUsernameBytes = _symmetricEncryption.EncryptString(credentials.username);
             credentials.username = Convert.ToBase64String(encryptedUsernameBytes);



          /*  var keyBytes = Encoding.Unicode.GetBytes();
            using (var md5 = MD5.Create())  //using sha256 stops the call to the login endpoint to be made for some reason
            {
                return md5.ComputeHash(keyBytes);
            }*/

            string json = JsonSerializer.Serialize(credentials);
            StringContent content = new StringContent(json,Encoding.Unicode, "application/json");

            Console.WriteLine("Request content: " + json);

            HttpResponseMessage response = await Client.PostAsync(HttpServerUrl + "login", content);
            if (response.IsSuccessStatusCode)
            {
                var response_data = await response.Content.ReadAsStringAsync();
                return response_data;
            }
            else
            {
                // Handle unsuccessful responses
                return "none";
            }
        }
        catch (Exception ex)
        {
            return "hello";
        }
    }
}



public class Credentials
{
    public required string username { get; set; }
    public required double password { get; set; }
}


//if we implement OAuth2.0 but it is pain in the ass
public class OAuth2
{
    public required string token { get; set; }
}


