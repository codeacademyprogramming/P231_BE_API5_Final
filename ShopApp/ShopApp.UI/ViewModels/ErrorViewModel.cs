namespace ShopApp.UI.ViewModels
{
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public List<ErrorViewModelItem> Errors { get; set;}
    }

    public class ErrorViewModelItem
    {
        public string Key { get; set; }
        public string ErrorMessage { get; set; }
    }
}
