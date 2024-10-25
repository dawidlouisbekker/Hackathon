namespace HackaThon
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        ItemsView Topics { get; set; };

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}

public class ItemModel
{
    public string UserName { get; set; }
    public string TopicDetials { get; set; }
    public string TopicName { get; set; }
}
