import { UserDto } from './user.dto';

export class AuthDto {
    success: boolean;
    token: string;
    userDto: UserDto;
    error: string;
}