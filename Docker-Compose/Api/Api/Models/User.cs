namespace Api.Models;

public class User
{
    public int User_Id { get; set; }
    public string User_Name { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string Password_Hash { get; set; }
    public string Password_Salt { get; set; }
    public string Email { get; set; }
    public int OTP { get; set; }
    public long Otp_Expiry_Time { get; set; }

}
