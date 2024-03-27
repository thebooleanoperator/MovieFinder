import { RegisterResponse } from "../../types/apiType";
import { ApiService } from "../CoreApi/CoreApi";

export type RegisterProps = {
  email: string,
  password: string
}

export class RegisterService {
  private _apiService: ApiService
  constructor (apiService: ApiService) {
    this._apiService = apiService
  }

  Register = async (email: string, password: string): Promise<RegisterResponse> => {
    var response = await this._apiService.Post('/Accounts/Register', {email, password})
    return { response, isSuccess: response ? true : false }
  }
}