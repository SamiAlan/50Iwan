namespace Iwan.Shared.Dtos.Accounts
{
    public class ChangePasswordDto
    {
        /// <summary>
        /// The user's old password
        /// </summary>
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// The user's new password
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// The user's confirm new password
        /// </summary>
        public string ConfirmNewPassword { get; set; }
    }
}
