export class ChangePassword {
    email: string;
    token: string;
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;

    constructor(email: string, oldPassword: string, newPassword: string, confirmPassword: string) {
        this.email = email;
        this.oldPassword = oldPassword;
        this.newPassword = newPassword;
        this.confirmPassword = confirmPassword;
    }
}