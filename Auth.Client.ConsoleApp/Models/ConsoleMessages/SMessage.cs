namespace Auth.Client.ConsoleApp.Models.ConsoleMessages
{
    internal struct SMessage(bool isVisible, string text)
    {
        public bool IsVisible { get; set; } = isVisible;
        public string Text { get; init; } = text;
    }
}
