namespace Api.Models;

public class Token
{
    public int Token_Id { get; set; }
    public string Value { get; set; }
    public long Created_Date { get; set; }
    public int User_Id { get; set; }
    public long Last_Modified_Date { get; set; }
    public long Expiry_Time { get; set; }
}
