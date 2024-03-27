import { AxiosResponse } from "axios"

export type ServiceResponse<T> = {
  response: AxiosResponse<T, any> | null
  isSuccess: boolean
}

export type LoginResponse = ServiceResponse<any>

export type RegisterResponse = ServiceResponse<any>