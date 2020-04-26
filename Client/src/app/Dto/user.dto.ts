export class UserDto {
    userId: number;
    email: string;
    firstName: string;
    lastName: string;
    
    constructor(private user: any) {
        this.userId = user.userId; 
        this.email = user.email;
        this.firstName = user.firstName;
        this.lastName = user.lastName;
    }
}