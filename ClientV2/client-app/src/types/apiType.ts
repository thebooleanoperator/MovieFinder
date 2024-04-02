export type ServiceResponseType<T> = {
  value?: T
  isSuccess: boolean
}

export type LoginResponseDataType = {
  jwt: string,
  userId?: string
}

export type LoginResponseType = ServiceResponseType<LoginResponseDataType>

export type RegisterResponseType = ServiceResponseType<null>