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

  Login = async (email: string, password: string) => {
    try {
      return await this._apiService.Post('/Accounts/Login', {email, password})
    }
    catch (error) {
      console.log(error)
      return
    }
  }

  LoginAsGuest = async () => {
    return await this._apiService.Post('/Accounts/GuestLogin')
  }
}