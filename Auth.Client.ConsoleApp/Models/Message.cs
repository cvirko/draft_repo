namespace Auth.Client.ConsoleApp.Models
{
    internal struct Message(bool isVisible, string text)
    {
        public bool IsVisible { get; set; } = isVisible;
        public string Text { get; init; } = text;
    }
}
