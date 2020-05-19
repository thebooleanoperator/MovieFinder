import { UserDto } from './user.dto';

export class AuthDto {
    success: boolean;
    token: string;
    refreshToken: string;
    userDto: UserDto;
    error: string;
}