using HackaThon.encryption;

namespace HackaThon;

public partial class Login : ContentPage
{
    ApiData instance {  get; set; }

    public Login()
	{
		InitializeComponent();
		instance = new ApiData();
	}

	private async void SendCred(object sender, EventArgs e)
    {
        var credentials = new Credentials { username = username.Text , password = 0.231 };
        await instance.SaveUserCredentialsAsync(credentials);
    }

}