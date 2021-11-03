namespace aptdealzSellerMobile.Interfaces
{
    public interface IHtmlToPDF
    {
        string SaveFiles(string filename, byte[] bytes);
    }
}
