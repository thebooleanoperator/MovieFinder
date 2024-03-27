import { LoginResponse } from "../../types/apiType";
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

  Login = async (email: string, password: string): Promise<LoginResponse> => {
    var response = await this._apiService.Post('/Accounts/Login', {email, password})
    return { response, isSuccess: response ? true : false }
  }

  LoginAsGuest = async (): Promise<LoginResponse> => {
    var response = await this._apiService.Post('/Accounts/GuestLogin')
    return { response, isSuccess: response ? true : false }
  }
}