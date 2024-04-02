import { LoginResponseType } from "../../types/apiType";
import { ApiService } from "../CoreApi/CoreApi";

export type LoginProps = {
  email: string,
  password: string
}

export class LoginService {
  private _apiService: ApiService
  constructor (apiService: ApiService) {
    this._apiService = apiService
  }

  Login = async (email: string, password: string): Promise<LoginResponseType> => {
    var response = await this._apiService.Post('/Accounts/Login', {email, password})
    return { value: response?.data, isSuccess: response ? true : false }
  }

  LoginAsGuest = async (): Promise<LoginResponseType> => {
    var response = await this._apiService.Post('/Accounts/GuestLogin')
    return { value: response?.data, isSuccess: response ? true : false }
  }
}