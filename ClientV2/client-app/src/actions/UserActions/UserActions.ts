import { TypeMap } from "../../types/typeMap"
import { UserType } from "../../types/userType"

export enum USER_ACTIONS {
  UPDATE_JWT = 'UPDATE_JWT'
}

export type UserPayloadType = {
  [USER_ACTIONS.UPDATE_JWT]: UserType
}

export type UserActionsType = TypeMap<UserPayloadType>[keyof TypeMap<UserPayloadType>]

export const UpdateJwt = (jwt: string) => ({
  type: USER_ACTIONS.UPDATE_JWT,
  payload: jwt
})